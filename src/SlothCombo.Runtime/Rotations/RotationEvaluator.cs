using System.Globalization;
using System.Text.Json.Nodes;

namespace SlothCombo.Runtime.Rotations;

internal sealed class RotationUnavailableException(string message) : Exception(message);

internal static class RotationJson
{
    public static string Text(JsonNode? node, string name, string? fallback = null)
        => node?[name]?.GetValue<string>() ?? fallback
            ?? throw new RotationUnavailableException($"missing:{name}");

    public static bool Flag(JsonNode? node, string name, bool fallback = false)
        => node?[name]?.GetValue<bool>() ?? fallback;

    public static JsonArray Items(JsonNode? node, string name)
        => node?[name] as JsonArray ?? throw new RotationUnavailableException($"missing-array:{name}");

    public static JsonObject Object(JsonNode? node)
        => node as JsonObject ?? throw new RotationUnavailableException("expected-object");

    public static object Scalar(JsonNode? node)
    {
        if (node is not JsonValue value)
            throw new RotationUnavailableException("expected-number-or-boolean");
        if (value.TryGetValue<bool>(out var flag))
            return flag;
        if (double.TryParse(value.ToJsonString(), NumberStyles.Float, CultureInfo.InvariantCulture,
                out var number) && double.IsFinite(number))
            return number;
        throw new RotationUnavailableException("expected-finite-number-or-boolean");
    }

    public static double Number(object value)
        => value is bool ? throw new RotationUnavailableException("expected-number")
            : Convert.ToDouble(value, CultureInfo.InvariantCulture);

    public static JsonNode Node(object value)
        => value is bool flag ? JsonValue.Create(flag)! : JsonValue.Create(Number(value))!;
}

internal sealed class RotationEvaluator(
    IReadOnlyDictionary<string, object> parameters,
    Dictionary<string, object> variables,
    Func<string, object> readField)
{
    public object Value(JsonNode? node)
    {
        var kind = RotationJson.Text(node, "kind");
        return kind switch
        {
            "literal" => RotationJson.Scalar(node?["value"]),
            "field" => readField(RotationJson.Text(node, "path")),
            "parameter" => Lookup(parameters, RotationJson.Text(node, "id"), "parameter"),
            "variable" => Lookup(variables, RotationJson.Text(node, "id"), "variable"),
            _ => throw new RotationUnavailableException($"unsupported-value:{kind}"),
        };
    }

    private static object Lookup(IReadOnlyDictionary<string, object> values, string id, string kind)
        => values.TryGetValue(id, out var value) ? value
            : throw new RotationUnavailableException($"unknown-{kind}:{id}");

    public bool Condition(JsonNode? node, int depth = 0)
    {
        if (depth > RotationValidator.MaxConditionDepth)
            throw new RotationUnavailableException("condition-depth-exceeded");
        var kind = RotationJson.Text(node, "kind");
        return kind switch
        {
            "constant" => node!["value"]!.GetValue<bool>(),
            "all" => RotationJson.Items(node, "children").All(child => Condition(child, depth + 1)),
            "any" => RotationJson.Items(node, "children").Any(child => Condition(child, depth + 1)),
            "not" => !Condition(RotationJson.Items(node, "children").Single(), depth + 1),
            "compare" => Compare(Value(node!["left"]), RotationJson.Text(node, "op"), Value(node["right"])),
            _ => throw new RotationUnavailableException($"unsupported-condition:{kind}"),
        };
    }

    private static bool Compare(object left, string op, object right)
    {
        if ((left is bool) != (right is bool))
            throw new RotationUnavailableException("comparison-type-mismatch");
        var comparison = left is bool flag ? flag.CompareTo((bool)right)
            : RotationJson.Number(left).CompareTo(RotationJson.Number(right));
        if (left is bool && op is not ("eq" or "ne"))
            throw new RotationUnavailableException("boolean-ordering-unavailable");
        return op switch
        {
            "eq" => comparison == 0,
            "ne" => comparison != 0,
            "gt" => comparison > 0,
            "gte" => comparison >= 0,
            "lt" => comparison < 0,
            "lte" => comparison <= 0,
            _ => throw new RotationUnavailableException($"unsupported-comparison:{op}"),
        };
    }

    public void Apply(JsonNode? operations)
    {
        if (operations is null)
            return;
        if (operations is not JsonArray array)
            throw new RotationUnavailableException("invalid-variable-operations");
        var next = new Dictionary<string, object>(variables, StringComparer.Ordinal);
        var staged = new RotationEvaluator(parameters, next, readField);
        foreach (var operation in array)
        {
            var id = RotationJson.Text(operation, "variableId");
            var existing = Lookup(next, id, "variable");
            var value = staged.Value(operation!["value"]);
            if ((existing is bool) != (value is bool))
                throw new RotationUnavailableException($"variable-type-mismatch:{id}");
            var result = RotationJson.Text(operation, "operation") switch
            {
                "set" => value,
                "add" when value is not bool => RotationJson.Number(existing) + RotationJson.Number(value),
                _ => throw new RotationUnavailableException($"unsupported-variable-operation:{id}"),
            };
            if (result is double number && !double.IsFinite(number))
                throw new RotationUnavailableException($"variable-overflow:{id}");
            next[id] = result;
        }
        foreach (var pair in next)
            variables[pair.Key] = pair.Value;
    }

    public static (Dictionary<string, object> Parameters, Dictionary<string, object> Variables) Initialize(JsonObject document)
    {
        if (RotationJson.Text(document, "format") != "xiv-rotation" || document["version"]?.GetValue<int>() != 1)
            throw new RotationUnavailableException("unsupported-rotation-format");
        var parameters = Definitions(document, "parameters", document["parameterValues"] as JsonObject);
        var variables = Definitions(document, "variables", null);
        var phases = RotationJson.Items(document, "phases");
        if (phases.Count is < 1 or > 128)
            throw new RotationUnavailableException("invalid-phase-count");
        var phaseIds = new HashSet<string>(StringComparer.Ordinal);
        foreach (var phase in phases)
            if (!phaseIds.Add(RotationJson.Text(phase, "id")))
                throw new RotationUnavailableException("duplicate-phase-id");
        if (!phaseIds.Contains(RotationJson.Text(document, "entryPhaseId")))
            throw new RotationUnavailableException("missing-entry-phase");
        var budget = RotationValidator.MaxConditionNodes;
        foreach (var phase in phases)
        {
            foreach (var transition in RotationJson.Items(phase, "transitions"))
            {
                if (!phaseIds.Contains(RotationJson.Text(transition, "toPhaseId")))
                    throw new RotationUnavailableException("missing-transition-phase");
                ValidateCondition(transition!["condition"], 0, ref budget);
            }
            foreach (var rule in RotationJson.Items(phase, "rules"))
            {
                ValidateCondition(rule!["condition"], 0, ref budget);
                if ((rule["action"] is null) == (rule["sequence"] is null))
                    throw new RotationUnavailableException("rule-needs-exactly-one-action-or-sequence");
                if (rule["action"] is JsonNode action)
                    ValidateAction(action);
                if (rule["sequence"] is JsonNode sequence)
                {
                    _ = RotationJson.Text(sequence, "id");
                    var timeout = sequence["timeoutMs"]?.GetValue<int>() ?? 10000;
                    if (timeout is < 50 or > 300000 ||
                        RotationJson.Text(sequence, "timeoutPolicy", "abort") is not ("abort" or "skip"))
                        throw new RotationUnavailableException("invalid-sequence-timeout");
                    var steps = RotationJson.Items(sequence, "steps");
                    if (steps.Count is < 1 or > 256)
                        throw new RotationUnavailableException("invalid-sequence-size");
                    foreach (var step in steps)
                    {
                        ValidateCondition(step!["condition"], 0, ref budget);
                        ValidateAction(step["action"]!);
                    }
                }
            }
        }
        return (parameters, variables);
    }

    private static Dictionary<string, object> Definitions(JsonObject document, string name, JsonObject? overrides)
    {
        var result = new Dictionary<string, object>(StringComparer.Ordinal);
        foreach (var definition in RotationJson.Items(document, name))
        {
            var id = RotationJson.Text(definition, "id");
            var value = RotationJson.Scalar(overrides?[id] ?? definition!["default"]);
            var type = RotationJson.Text(definition, "type");
            if (type is not ("number" or "boolean") || (type == "boolean") != (value is bool))
                throw new RotationUnavailableException($"invalid-definition-type:{id}");
            if (value is not bool)
            {
                var number = RotationJson.Number(value);
                if ((definition!["min"] is JsonNode min && number < RotationJson.Number(RotationJson.Scalar(min))) ||
                    (definition["max"] is JsonNode max && number > RotationJson.Number(RotationJson.Scalar(max))))
                    throw new RotationUnavailableException($"parameter-out-of-range:{id}");
            }
            if (!result.TryAdd(id, value))
                throw new RotationUnavailableException($"duplicate-definition:{id}");
        }
        if (overrides is not null && overrides.Any(pair => !result.ContainsKey(pair.Key)))
            throw new RotationUnavailableException("unknown-parameter-override");
        return result;
    }

    private static void ValidateCondition(JsonNode? node, int depth, ref int budget)
    {
        if (--budget < 0 || depth > RotationValidator.MaxConditionDepth)
            throw new RotationUnavailableException("condition-budget-exceeded");
        switch (RotationJson.Text(node, "kind"))
        {
            case "constant":
                _ = node!["value"]!.GetValue<bool>();
                break;
            case "all":
            case "any":
            case "not":
                var children = RotationJson.Items(node, "children");
                if (RotationJson.Text(node, "kind") == "not" && children.Count != 1)
                    throw new RotationUnavailableException("not-needs-one-child");
                foreach (var child in children)
                    ValidateCondition(child, depth + 1, ref budget);
                break;
            case "compare":
                ValidateValue(node!["left"]);
                ValidateValue(node["right"]);
                if (RotationJson.Text(node, "op") is not ("eq" or "ne" or "gt" or "gte" or "lt" or "lte"))
                    throw new RotationUnavailableException("unsupported-comparison");
                break;
            default:
                throw new RotationUnavailableException("unsupported-condition");
        }
    }

    internal static void ValidateValue(JsonNode? node)
    {
        switch (RotationJson.Text(node, "kind"))
        {
            case "literal": _ = RotationJson.Scalar(node!["value"]); break;
            case "field": _ = RotationJson.Text(node, "path"); break;
            case "parameter":
            case "variable": _ = RotationJson.Text(node, "id"); break;
            default: throw new RotationUnavailableException("unsupported-value");
        }
    }

    private static void ValidateAction(JsonNode node)
    {
        if (node["actionId"]?.GetValue<uint>() is not > 0)
            throw new RotationUnavailableException("invalid-action-id");
        var target = node["target"] ?? throw new RotationUnavailableException("missing-target");
        var kind = RotationJson.Text(target, "kind");
        if (kind is not ("current" or "self" or "partySlot" or "lowestHp" or "role"))
            throw new RotationUnavailableException("unsupported-target");
        if (kind == "partySlot" && target["slot"]?.GetValue<int>() is not (>= 1 and <= 8))
            throw new RotationUnavailableException("invalid-party-slot");
        if (kind == "role" && RotationJson.Text(target, "role") is not ("tank" or "healer" or "dps"))
            throw new RotationUnavailableException("unsupported-role");
        if (RotationJson.Text(node, "ground", "none") is not ("none" or "self" or "target"))
            throw new RotationUnavailableException("unsupported-ground");
    }
}
