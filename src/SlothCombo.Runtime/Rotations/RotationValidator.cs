using System.Collections.Frozen;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using Json.Schema;
using SlothCombo.Runtime.Gauges;

namespace SlothCombo.Runtime.Rotations;

/// <summary>Validates the embedded protocol before any running or stored plan changes.</summary>
public static class RotationValidator
{
    public const int MaxJsonDepth = 128;
    public const int MaxConditionDepth = 32;
    public const int MaxNodes = 200000;
    public const int MaxConditionNodes = 8192;
    private static readonly JsonSerializerOptions Compact = new() { MaxDepth = MaxJsonDepth };

    public static IReadOnlySet<uint> SupportedJobIds { get; } = new uint[]
    {
        19, 20, 21, 22, 23, 24, 25, 27, 28, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42,
    }.ToFrozenSet();

    private static readonly Lazy<JsonSchema> Schema = new(() =>
    {
        using var stream = typeof(RotationValidator).Assembly.GetManifestResourceStream("SlothCombo.Rotations.Schema")
            ?? throw new InvalidOperationException("缺少内嵌轮转协议，无法安全验证导入。");
        using var reader = new StreamReader(stream, Encoding.UTF8);
        return JsonSchema.FromText(reader.ReadToEnd());
    });

    public static IReadOnlyDictionary<string, string> FieldTypes { get; } =
        new ReadOnlyDictionary<string, string>(new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["player.level"] = "number",
            ["player.hpPercent"] = "number",
            ["player.mp"] = "number",
            ["player.moving"] = "boolean",
            ["player.inCombat"] = "boolean",
            ["target.hpPercent"] = "number",
            ["target.distance"] = "number",
            ["target.enemyCount"] = "number",
            ["combo.action"] = "number",
            ["combo.remaining"] = "number",
            ["countdown.remaining"] = "number",
            ["lastAction"] = "number",
            ["weave.allowed"] = "boolean",
            ["party.count"] = "number",
            ["party.lowestHp"] = "number",
        });

    // Read property metadata only; validation never instantiates or dereferences a live gauge.
    public static IReadOnlyDictionary<string, string> GaugeFieldTypes { get; } = BuildGaugeFieldTypes();

    public static JsonObject ParseObject(string json, int maximumBytes,
        int maximumDepth = MaxJsonDepth, string capacityName = "JSON")
    {
        ArgumentNullException.ThrowIfNull(json);
        if (Encoding.UTF8.GetByteCount(json) > maximumBytes)
            throw Invalid("$", $"{capacityName}超过底层容量上限 {maximumBytes} 字节，不是单方案大小限制。");

        var bytes = Encoding.UTF8.GetBytes(json);
        var reader = new Utf8JsonReader(bytes, new JsonReaderOptions { MaxDepth = maximumDepth });
        var objects = new Stack<HashSet<string>>();
        while (reader.Read())
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.StartObject:
                    objects.Push(new HashSet<string>(StringComparer.Ordinal));
                    break;
                case JsonTokenType.EndObject:
                    objects.Pop();
                    break;
                case JsonTokenType.PropertyName:
                    var name = reader.GetString()!;
                    if (name is "__proto__" or "prototype" or "constructor")
                        throw Invalid("$", "不接受不安全的属性名称。");
                    if (!objects.Peek().Add(name))
                        throw Invalid("$", "JSON 包含重复属性。");
                    break;
            }
        }

        return JsonNode.Parse(bytes, documentOptions: new JsonDocumentOptions { MaxDepth = maximumDepth })
            as JsonObject ?? throw Invalid("$", "必须是 JSON 对象。");
    }

    public static void Validate(JsonObject plan)
    {
        ArgumentNullException.ThrowIfNull(plan);
        var nodes = 0;
        CheckSize(plan, 0, ref nodes);
        var entityIds = new HashSet<string>(StringComparer.Ordinal);
        var conditionCount = 0;
        UniqueEntityIds(plan, entityIds, ref conditionCount);
        var json = plan.ToJsonString(Compact);

        using var document = JsonDocument.Parse(json, new JsonDocumentOptions { MaxDepth = MaxJsonDepth });
        if (!Schema.Value.Evaluate(document.RootElement).IsValid)
            throw Invalid("$", "不符合轮转 JSON Schema：请检查必填项、未知属性、类型、目标和数量限制。");
        if (plan["format"]!.GetValue<string>() != "xiv-rotation")
            throw Invalid("$.format", "这里只接受 xiv-rotation 计划，不接受项目或目录文件。");

        var jobId = PositiveUInt(plan["jobId"], "$.jobId");
        if (!SupportedJobIds.Contains(jobId)) throw Invalid("$.jobId", "不在首版支持的 22 个战斗职业中。");
        var parameters = Definitions(plan["parameters"]!.AsArray(), true);
        var variables = Definitions(plan["variables"]!.AsArray(), false);
        ValidateParameterValues(parameters, plan["parameterValues"]!.AsObject());

        var phases = plan["phases"]!.AsArray();
        var phaseIds = UniqueIds(phases, "$.phases");
        if (!phaseIds.Contains(plan["entryPhaseId"]!.GetValue<string>()))
            throw Invalid("$.entryPhaseId", "入口阶段不存在。");

        var ruleIds = new HashSet<string>(StringComparer.Ordinal);
        var sequenceIds = new HashSet<string>(StringComparer.Ordinal);
        foreach (var phaseNode in phases)
        {
            var phase = phaseNode!.AsObject();
            var path = $"$.phases[{phase["id"]}]";
            Name(phase, path);
            Operations(phase["onEnter"]!.AsArray(), path + ".onEnter", parameters, variables);
            var transitions = phase["transitions"]!.AsArray();
            UniqueIds(transitions, path + ".transitions");
            foreach (var transitionNode in transitions)
            {
                var transition = transitionNode!.AsObject();
                if (!phaseIds.Contains(transition["toPhaseId"]!.GetValue<string>()))
                    throw Invalid(path + ".transitions", "跳转目标阶段不存在。");
                Condition(transition["condition"]!.AsObject(), path + ".transitions.condition",
                    parameters, variables, 0);
            }

            foreach (var ruleNode in phase["rules"]!.AsArray())
            {
                var rule = ruleNode!.AsObject();
                if (!ruleIds.Add(rule["id"]!.GetValue<string>()))
                    throw Invalid(path + ".rules", "规则 ID 在计划中重复。");
                Name(rule, path + ".rules");
                Condition(rule["condition"]!.AsObject(), path + ".rules.condition", parameters, variables, 0);
                if (rule["action"] is JsonObject action)
                {
                    Action(action, path + ".rules.action", parameters, variables);
                    continue;
                }

                var sequence = rule["sequence"]!.AsObject();
                if (!sequenceIds.Add(sequence["id"]!.GetValue<string>()))
                    throw Invalid(path + ".rules.sequence", "序列 ID 在计划中重复。");
                var steps = sequence["steps"]!.AsArray();
                UniqueIds(steps, path + ".rules.sequence.steps");
                foreach (var stepNode in steps)
                {
                    var step = stepNode!.AsObject();
                    Condition(step["condition"]!.AsObject(), path + ".rules.sequence.steps.condition",
                        parameters, variables, 0);
                    Action(step["action"]!.AsObject(), path + ".rules.sequence.steps.action", parameters, variables);
                }
            }
        }
    }

    public static void ValidateParameters(JsonObject plan, JsonObject values)
    {
        var nodes = 0;
        CheckSize(values, 0, ref nodes);
        ValidateParameterValues(Definitions(plan["parameters"]!.AsArray(), true), values);
    }

    /// <summary>Returns the exact scalar type, or rejects an unsupported field.</summary>
    public static string GetFieldType(string path)
    {
        if (FieldTypes.TryGetValue(path, out var type)) return type;
        if (GaugeFieldTypes.TryGetValue(path, out type)) return type;
        var action = Regex.Match(path, @"^action\.([1-9][0-9]{0,9})\.(ready|cooldown|charges|adjustedId|resourceCost|unlocked|usedThisCombat|weaveWindow)$",
            RegexOptions.CultureInvariant | RegexOptions.NonBacktracking);
        if (action.Success && uint.TryParse(action.Groups[1].Value, out _))
            return action.Groups[2].Value is "ready" or "unlocked" or "usedThisCombat" ? "boolean" : "number";
        var status = Regex.Match(path, @"^status\.(self|target)\.([1-9][0-9]{0,9})\.(remaining|stacks|mine)$",
            RegexOptions.CultureInvariant | RegexOptions.NonBacktracking);
        if (status.Success && uint.TryParse(status.Groups[2].Value, out _))
            return status.Groups[3].Value == "mine" ? "boolean" : "number";
        throw Invalid("$.path", $"不支持的字段 {path}。");
    }

    internal static uint PositiveUInt(JsonNode? node, string path)
    {
        if (node?.GetValueKind() != JsonValueKind.Number)
            throw Invalid(path, "必须是非零 uint 整数。");
        try
        {
            var result = node.Deserialize<uint>();
            return result != 0 ? result : throw Invalid(path, "必须是非零 uint 整数。");
        }
        catch (JsonException)
        {
            throw Invalid(path, "必须是非零 uint 整数。");
        }
    }

    private static Dictionary<string, JsonObject> Definitions(JsonArray entries, bool parameter)
    {
        var result = new Dictionary<string, JsonObject>(StringComparer.Ordinal);
        foreach (var entry in entries)
        {
            var definition = entry!.AsObject();
            var id = definition["id"]!.GetValue<string>();
            if (!result.TryAdd(id, definition)) throw Invalid(id, "参数或变量 ID 重复。");
            Name(definition, id);
            var type = definition["type"]!.GetValue<string>();
            if (ScalarType(definition["default"], id + ".default") != type)
                throw Invalid(id + ".default", "默认值类型不匹配。");
            if (!parameter) continue;
            if (type == "boolean" && (definition.ContainsKey("min") || definition.ContainsKey("max")))
                throw Invalid(id, "布尔参数不能有 min/max。");
            if (type == "number")
            {
                var min = definition["min"] is null ? double.NegativeInfinity : Number(definition["min"], id + ".min");
                var max = definition["max"] is null ? double.PositiveInfinity : Number(definition["max"], id + ".max");
                if (min > max) throw Invalid(id, "min 不能大于 max。");
                Bounds(definition, definition["default"], id + ".default");
            }
        }
        return result;
    }

    private static void ValidateParameterValues(Dictionary<string, JsonObject> parameters, JsonObject values)
    {
        foreach (var (id, value) in values)
        {
            if (!parameters.TryGetValue(id, out var parameter))
                throw Invalid("$.parameterValues." + id, "参数没有声明。");
            if (ScalarType(value, id) != parameter["type"]!.GetValue<string>())
                throw Invalid("$.parameterValues." + id, "参数值类型不匹配。");
            if (parameter["type"]!.GetValue<string>() == "number") Bounds(parameter, value, id);
        }
    }

    private static void Bounds(JsonObject parameter, JsonNode? value, string path)
    {
        var number = Number(value, path);
        if ((parameter["min"] is not null && number < Number(parameter["min"], path))
            || (parameter["max"] is not null && number > Number(parameter["max"], path)))
            throw Invalid(path, "参数值超出声明范围。");
    }

    private static void Condition(JsonObject condition, string path, Dictionary<string, JsonObject> parameters,
        Dictionary<string, JsonObject> variables, int depth)
    {
        if (depth > MaxConditionDepth) throw Invalid(path, $"条件嵌套不能超过 {MaxConditionDepth} 层。");
        var kind = condition["kind"]!.GetValue<string>();
        if (kind == "constant") return;
        if (kind is "all" or "any" or "not")
        {
            foreach (var child in condition["children"]!.AsArray())
                Condition(child!.AsObject(), path + ".children", parameters, variables, depth + 1);
            return;
        }
        var left = Value(condition["left"]!.AsObject(), path + ".left", parameters, variables);
        var right = Value(condition["right"]!.AsObject(), path + ".right", parameters, variables);
        if (left != right) throw Invalid(path, "比较两侧类型必须相同。");
        if (left == "boolean" && condition["op"]!.GetValue<string>() is not ("eq" or "ne"))
            throw Invalid(path, "布尔值只支持 eq/ne 比较。");
    }

    private static string Value(JsonObject value, string path, Dictionary<string, JsonObject> parameters,
        Dictionary<string, JsonObject> variables)
    {
        var kind = value["kind"]!.GetValue<string>();
        if (kind == "literal") return ScalarType(value["value"], path);
        if (kind == "field") return GetFieldType(value["path"]!.GetValue<string>());
        var definitions = kind == "parameter" ? parameters : variables;
        var id = value["id"]!.GetValue<string>();
        if (!definitions.TryGetValue(id, out var definition))
            throw Invalid(path, $"引用的 {kind} {id} 不存在。");
        return definition["type"]!.GetValue<string>();
    }

    private static void Operations(JsonArray operations, string path, Dictionary<string, JsonObject> parameters,
        Dictionary<string, JsonObject> variables)
    {
        foreach (var operationNode in operations)
        {
            var operation = operationNode!.AsObject();
            var id = operation["variableId"]!.GetValue<string>();
            if (!variables.TryGetValue(id, out var variable)) throw Invalid(path, $"变量 {id} 不存在。");
            var type = Value(operation["value"]!.AsObject(), path + ".value", parameters, variables);
            if (type != variable["type"]!.GetValue<string>()) throw Invalid(path, "变量赋值类型不匹配。");
            if (operation["operation"]!.GetValue<string>() == "add" && type != "number")
                throw Invalid(path, "add 只能用于数值变量。");
        }
    }

    private static void Action(JsonObject action, string path, Dictionary<string, JsonObject> parameters,
        Dictionary<string, JsonObject> variables)
    {
        PositiveUInt(action["actionId"], path + ".actionId");
        Operations(action["onSuccess"]!.AsArray(), path + ".onSuccess", parameters, variables);
    }

    private static string ScalarType(JsonNode? node, string path)
    {
        if (node is JsonValue value && value.TryGetValue<bool>(out _)) return "boolean";
        Number(node, path);
        return "number";
    }

    private static double Number(JsonNode? node, string path)
    {
        if (node?.GetValueKind() != JsonValueKind.Number)
            throw Invalid(path, "必须是有限数值。");
        var number = node.Deserialize<double>();
        if (!double.IsFinite(number)) throw Invalid(path, "必须是有限数值。");
        return number;
    }

    private static HashSet<string> UniqueIds(JsonArray entries, string path)
    {
        var ids = new HashSet<string>(StringComparer.Ordinal);
        foreach (var entry in entries)
            if (!ids.Add(entry!["id"]!.GetValue<string>())) throw Invalid(path, "ID 重复。");
        return ids;
    }

    private static void Name(JsonObject node, string path)
    {
        if (string.IsNullOrWhiteSpace(node["name"]!.GetValue<string>())) throw Invalid(path, "名称不能为空白。");
    }

    private static void CheckSize(JsonNode? node, int depth, ref int count)
    {
        if (depth > MaxJsonDepth || ++count > MaxNodes) throw Invalid("$", "JSON 深度或节点数量超限。");
        if (node is JsonObject obj)
            foreach (var property in obj)
            {
                if (property.Key is "__proto__" or "prototype" or "constructor")
                    throw Invalid("$", "不接受不安全的属性名称。");
                CheckSize(property.Value, depth + 1, ref count);
            }
        else if (node is JsonArray array)
            foreach (var item in array) CheckSize(item, depth + 1, ref count);
    }

    private static void UniqueEntityIds(JsonNode? node, HashSet<string> ids, ref int conditionCount, int depth = 0)
    {
        if (node is JsonObject obj)
        {
            var kind = obj["kind"] is JsonValue kindNode && kindNode.TryGetValue<string>(out var kindText)
                ? kindText : null;
            var condition = kind is "constant" or "all" or "any" or "not" or "compare";
            if (condition && (++conditionCount > MaxConditionNodes || depth > MaxConditionDepth))
                throw Invalid("$", "条件总数超过 8192 或深度超过 32。");
            // Value.id is a reference, not a stable entity declaration.
            if (obj["id"] is JsonValue id && id.TryGetValue<string>(out var text))
            {
                if (kind is not ("parameter" or "variable") && !ids.Add(text))
                    throw Invalid("$.id", "稳定实体 ID 在计划中重复（含条件）。");
            }
            foreach (var (key, child) in obj)
                UniqueEntityIds(child, ids, ref conditionCount, condition && key == "children" ? depth + 1 : 0);
        }
        else if (node is JsonArray array)
            foreach (var child in array) UniqueEntityIds(child, ids, ref conditionCount, depth);
    }

    private static IReadOnlyDictionary<string, string> BuildGaugeFieldTypes()
    {
        var fields = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (var type in typeof(JobGaugeBase).Assembly.GetTypes()
                     .Where(type => !type.IsAbstract && typeof(JobGaugeBase).IsAssignableFrom(type)))
        {
            foreach (var property in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly))
            {
                var scalar = property.PropertyType;
                if (scalar == typeof(bool)) fields["gauge." + property.Name] = "boolean";
                else if (scalar.IsEnum || scalar == typeof(byte) || scalar == typeof(sbyte)
                         || scalar == typeof(ushort) || scalar == typeof(short) || scalar == typeof(uint)
                         || scalar == typeof(int) || scalar == typeof(ulong) || scalar == typeof(long)
                         || scalar == typeof(float) || scalar == typeof(double))
                    fields["gauge." + property.Name] = "number";
            }
        }
        return new ReadOnlyDictionary<string, string>(fields);
    }

    private static ArgumentException Invalid(string path, string message) => new($"{path}: {message}");
}
