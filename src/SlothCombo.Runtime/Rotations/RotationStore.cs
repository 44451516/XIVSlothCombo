using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using SlothCombo.Abstractions;

namespace SlothCombo.Runtime.Rotations;

/// <summary>Owns detached plan snapshots and atomically persists the complete selection state.</summary>
public static class RotationStore
{
    private const int MaxStoreBytes = 32 * 1024 * 1024;
    private static readonly object Gate = new();
    // Static catalog initialization has its own synchronization, separate from mutable store state.
    private static readonly Lazy<JsonObject> Catalog = new(LoadCatalog);
    private static readonly JsonSerializerOptions Pretty = new() { WriteIndented = true, MaxDepth = 132 };
    private static JsonObject _state = Defaults();
    private static string? _path;

    public static string? InitializationError { get; private set; }

    public static void Initialize(string folder)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(folder);
        lock (Gate)
        {
            _path = null;
            InitializationError = null;
            _state = Defaults();
            try
            {
                var directory = Path.GetFullPath(folder);
                _path = Path.Combine(directory, "rotation-store.json");
                if (File.Exists(_path))
                {
                    if (new FileInfo(_path).Length > MaxStoreBytes)
                        throw new ArgumentException("轮转存储文件超过 32 MiB（33554432 字节）的底层容量上限，原文件未修改。");
                    var candidate = RotationValidator.ParseObject(File.ReadAllText(_path, Encoding.UTF8),
                        MaxStoreBytes, RotationValidator.MaxJsonDepth + 4, "轮转存储总量（32 MiB）");
                    ValidateState(candidate);
                    ValidateSnapshotCapacity(candidate);
                    _state = candidate;
                }
                else
                {
                    foreach (var plan in _state["plans"]!.AsArray()) RotationValidator.Validate(plan!.AsObject());
                    var json = _state.ToJsonString(Pretty);
                    var normalized = RotationValidator.ParseObject(json, MaxStoreBytes,
                        RotationValidator.MaxJsonDepth + 4, "轮转存储总量（32 MiB）");
                    ValidateSnapshotCapacity(normalized);
                    Persist(json);
                    _state = normalized;
                }
            }
            catch (Exception ex)
            {
                // Never replace a damaged or incompatible file with permissive defaults.
                InitializationError = ex is IpcMessageTooLargeException
                    ? "完整轮转快照超过 IPC 32 MiB 容量上限（含响应信封和转义），原文件未修改：" + ex.Message
                    : ex.Message;
                _state = new JsonObject
                {
                    ["format"] = "xiv-rotation-store", ["version"] = 1,
                    ["plans"] = new JsonArray(), ["jobs"] = new JsonObject(),
                };
            }
        }
    }

    public static JsonObject GetSnapshot()
    {
        lock (Gate)
        {
            return Snapshot(_state, _path is null || InitializationError is not null,
                InitializationError ?? (_path is null ? "轮转存储尚未初始化。" : null));
        }
    }

    private static JsonObject Snapshot(JsonObject state, bool failClosed, string? error)
    {
        var snapshot = (JsonObject)state.DeepClone();
        snapshot["failClosed"] = failClosed;
        snapshot["initializationError"] = error;
        return snapshot;
    }

    private static void ValidateSnapshotCapacity(JsonObject candidate)
    {
        RotationTransport.CreateResponse(200, RotationTransport.Success(Snapshot(candidate, false, null)));
    }

    public static JsonObject? GetActive(uint jobId)
    {
        lock (Gate)
        {
            if (!RotationValidator.SupportedJobIds.Contains(jobId) || _path is null || InitializationError is not null)
                return null;
            var selection = _state["jobs"]![JobKey(jobId)] as JsonObject;
            var id = selection?["activePlanId"]?.GetValue<string>();
            return id is null ? null : (JsonObject?)FindPlan(_state, id)?.DeepClone();
        }
    }

    public static bool IsV8(uint jobId)
    {
        lock (Gate)
        {
            // A broken store must not silently re-enable the old PvE engine.
            if (!RotationValidator.SupportedJobIds.Contains(jobId)) return false;
            if (_path is null || InitializationError is not null) return true;
            return _state["jobs"]![JobKey(jobId)]?["mode"]?.GetValue<string>() == "v8";
        }
    }

    public static void SetMode(uint jobId, string mode)
    {
        lock (Gate)
        {
            Ready();
            JobKey(jobId);
            if (mode is not ("legacy" or "v8")) throw new ArgumentException("mode 只能是 legacy 或 v8。");
            var candidate = Copy();
            Selection(candidate, jobId)["mode"] = mode;
            Commit(candidate, "rotation-mode-changed");
        }
    }

    public static void Save(JsonObject plan, bool overwrite)
    {
        lock (Gate)
        {
            Ready();
            RotationValidator.Validate(plan);
            var id = plan["id"]!.GetValue<string>();
            var jobId = RotationValidator.PositiveUInt(plan["jobId"], "$.jobId");
            var existing = FindPlan(_state, id);
            if (existing is not null && !overwrite) throw new RotationConflictException("计划 ID 已存在，请显式允许覆盖。");
            if (existing is not null && existing["jobId"]!.GetValue<uint>() != jobId)
                throw new RotationConflictException("不能将已有计划的职业改为另一职业，请使用新计划 ID。");
            var candidate = Copy();
            var plans = candidate["plans"]!.AsArray();
            var old = FindPlan(candidate, id);
            if (old is null)
            {
                if (plans.Count >= 256) throw new ArgumentException("最多保存 256 个计划。");
                plans.Add(plan.DeepClone());
            }
            else
            {
                plans[plans.IndexOf(old)] = plan.DeepClone();
            }
            Selection(candidate, jobId);
            Commit(candidate, "rotation-plan-imported");
        }
    }

    public static void Delete(string planId)
    {
        lock (Gate)
        {
            Ready();
            var candidate = Copy();
            var plan = FindPlan(candidate, planId) ?? throw new KeyNotFoundException("计划不存在。");
            candidate["plans"]!.AsArray().Remove(plan);
            foreach (var (_, value) in candidate["jobs"]!.AsObject())
            {
                var selection = value!.AsObject();
                if (selection["activePlanId"]?.GetValue<string>() != planId) continue;
                selection["activePlanId"] = null;
            }
            Commit(candidate, "rotation-plan-deleted");
        }
    }

    public static void Select(uint jobId, string planId)
    {
        lock (Gate)
        {
            Ready();
            JobKey(jobId);
            var candidate = Copy();
            var plan = FindPlan(candidate, planId) ?? throw new KeyNotFoundException("计划不存在。");
            if (plan["jobId"]!.GetValue<uint>() != jobId) throw new ArgumentException("不能选择其他职业的计划。");
            Selection(candidate, jobId)["activePlanId"] = planId;
            Commit(candidate, "rotation-plan-selected");
        }
    }

    public static void SaveParameters(uint jobId, JsonObject values)
    {
        lock (Gate)
        {
            Ready();
            var selected = GetActive(jobId) ?? throw new ArgumentException("该职业尚未选择计划。");
            RotationValidator.ValidateParameters(selected, values);
            var candidate = Copy();
            FindPlan(candidate, selected["id"]!.GetValue<string>())!["parameterValues"] = values.DeepClone();
            Commit(candidate, "rotation-parameters-changed");
        }
    }

    public static JsonObject ExportCatalog() => (JsonObject)Catalog.Value.DeepClone();

    private static JsonObject LoadCatalog()
    {
        JsonObject catalog;
        string? error = null;
        try
        {
            using var stream = typeof(RotationStore).Assembly.GetManifestResourceStream("SlothCombo.Rotations.Catalog");
            if (stream is null)
            {
                catalog = FallbackCatalog();
            }
            else
            {
                using var reader = new StreamReader(stream, Encoding.UTF8);
                catalog = RotationValidator.ParseObject(reader.ReadToEnd(), MaxStoreBytes,
                    capacityName: "内嵌参考目录（32 MiB）");
                if (catalog["format"]?.GetValue<string>() != "xiv-rotation-catalog"
                    || catalog["version"]?.GetValue<int>() != 1 || catalog["jobs"] is not JsonArray)
                    throw new ArgumentException("内嵌参考目录格式或版本不支持。");
            }
        }
        catch (Exception ex) when (ex is IOException or JsonException or ArgumentException or InvalidOperationException
                                   or FormatException or OverflowException)
        {
            error = ex.Message;
            catalog = FallbackCatalog();
        }

        // No catalog version has been certified against a running CN client.
        catalog["certification"] = "reference";
        catalog["certified"] = false;
        catalog["referenceOnly"] = true;
        catalog["catalogError"] = error;
        var fields = new JsonArray();
        foreach (var (path, type) in RotationValidator.FieldTypes.Concat(RotationValidator.GaugeFieldTypes))
            fields.Add(new JsonObject { ["path"] = path, ["type"] = type });
        catalog["runtimeFields"] = fields;
        catalog["runtimeFieldPatterns"] = new JsonArray(
            new JsonObject { ["path"] = "action.<ID>.ready", ["type"] = "boolean" },
            new JsonObject { ["path"] = "action.<ID>.cooldown", ["type"] = "number" },
            new JsonObject { ["path"] = "action.<ID>.charges", ["type"] = "number" },
            new JsonObject { ["path"] = "status.self|target.<ID>.remaining", ["type"] = "number" },
            new JsonObject { ["path"] = "status.self|target.<ID>.stacks", ["type"] = "number" },
            new JsonObject { ["path"] = "status.self|target.<ID>.mine", ["type"] = "boolean" });
        return catalog;
    }

    private static JsonObject FallbackCatalog() => new()
    {
        ["format"] = "xiv-rotation-catalog", ["version"] = 1, ["region"] = "CN",
        ["jobs"] = new JsonArray(new Dictionary<uint, string>
        {
            [19] = "PLD", [20] = "MNK", [21] = "WAR", [22] = "DRG", [23] = "BRD", [24] = "WHM",
            [25] = "BLM", [27] = "SMN", [28] = "SCH", [30] = "NIN", [31] = "MCH", [32] = "DRK",
            [33] = "AST", [34] = "SAM", [35] = "RDM", [36] = "BLU", [37] = "GNB", [38] = "DNC",
            [39] = "RPR", [40] = "SGE", [41] = "VPR", [42] = "PCT",
        }.Select(job => (JsonNode)new JsonObject
        {
            ["id"] = job.Key, ["name"] = job.Value, ["abbreviation"] = job.Value, ["referenceOnly"] = true,
            ["actions"] = new JsonArray(), ["statuses"] = new JsonArray(), ["traits"] = new JsonArray(),
            ["gauges"] = new JsonArray(),
        }).ToArray()),
        ["fallback"] = true,
        ["actions"] = new JsonArray(), ["statuses"] = new JsonArray(), ["traits"] = new JsonArray(),
    };

    private static void Ready()
    {
        if (_path is null || InitializationError is not null)
            throw new ArgumentException("轮转存储不可写，请先修复原文件并重新初始化：" + (InitializationError ?? "尚未初始化"));
    }

    private static string JobKey(uint jobId) => !RotationValidator.SupportedJobIds.Contains(jobId)
        ? throw new ArgumentException("jobId 不在首版支持的 22 个战斗职业中。")
        : jobId.ToString(System.Globalization.CultureInfo.InvariantCulture);

    private static JsonObject Selection(JsonObject state, uint jobId)
    {
        var jobs = state["jobs"]!.AsObject();
        var key = JobKey(jobId);
        if (jobs[key] is not JsonObject selection)
        {
            selection = new JsonObject { ["mode"] = "legacy", ["activePlanId"] = null };
            jobs[key] = selection;
        }
        return selection;
    }

    private static JsonObject Copy() => (JsonObject)_state.DeepClone();

    private static JsonObject? FindPlan(JsonObject state, string id) =>
        state["plans"]!.AsArray().OfType<JsonObject>().FirstOrDefault(plan => plan["id"]!.GetValue<string>() == id);

    private static void Commit(JsonObject candidate, string reason)
    {
        var json = candidate.ToJsonString(Pretty);
        if (Encoding.UTF8.GetByteCount(json) > MaxStoreBytes)
            throw new ArgumentException("轮转存储总量超过 32 MiB（33554432 字节）的底层容量上限；包含所有方案和配置，未保存变更。");
        var normalized = RotationValidator.ParseObject(json, MaxStoreBytes,
            RotationValidator.MaxJsonDepth + 4, "轮转存储总量（32 MiB）");
        ValidateSnapshotCapacity(normalized);
        RotationEngine.Stop(reason);
        Persist(json);
        _state = normalized;
    }

    private static void Persist(string json)
    {
        var path = _path ?? throw new InvalidOperationException("轮转存储尚未初始化。");
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        var temporary = path + "." + Guid.NewGuid().ToString("N") + ".tmp";
        try
        {
            using (var stream = new FileStream(temporary, FileMode.CreateNew, FileAccess.Write, FileShare.None))
            {
                using (var writer = new StreamWriter(stream, new UTF8Encoding(false), leaveOpen: true))
                {
                    writer.Write(json);
                    writer.Flush();
                }
                stream.Flush(flushToDisk: true);
            }
            if (File.Exists(path)) File.Replace(temporary, path, null);
            else File.Move(temporary, path);
        }
        finally
        {
            try { if (File.Exists(temporary)) File.Delete(temporary); }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException) { }
        }
    }

    private static void ValidateState(JsonObject state)
    {
        if (state.Any(entry => entry.Key is not ("format" or "version" or "plans" or "jobs"))
            || state["format"]?.GetValue<string>() != "xiv-rotation-store"
            || state["version"]?.GetValue<int>() != 1
            || state["plans"] is not JsonArray plans || state["jobs"] is not JsonObject jobs || plans.Count > 256)
            throw new ArgumentException("轮转存储格式或版本不支持。");
        var ids = new HashSet<string>(StringComparer.Ordinal);
        foreach (var node in plans)
        {
            var plan = node as JsonObject ?? throw new ArgumentException("存储中的计划无效。");
            RotationValidator.Validate(plan);
            if (!ids.Add(plan["id"]!.GetValue<string>())) throw new ArgumentException("存储中的计划 ID 重复。");
        }
        foreach (var (job, node) in jobs)
        {
            if (!uint.TryParse(job, out var jobId) || JobKey(jobId) != job
                || node is not JsonObject selection
                || selection.Count != 2 || !selection.ContainsKey("activePlanId")
                || selection["mode"]?.GetValue<string>() is not ("legacy" or "v8"))
                throw new ArgumentException("存储中的职业选择无效。");
            if (selection["activePlanId"] is null) continue;
            var plan = FindPlan(state, selection["activePlanId"]!.GetValue<string>());
            if (plan is null || plan["jobId"]!.GetValue<uint>() != jobId)
                throw new ArgumentException("存储中的活动计划不存在或职业不匹配。");
        }
    }

    private static JsonObject Defaults() => new()
    {
        ["format"] = "xiv-rotation-store", ["version"] = 1,
        ["plans"] = new JsonArray(Example(false), Example(true)),
        ["jobs"] = new JsonObject
        {
            ["21"] = new JsonObject { ["mode"] = "legacy", ["activePlanId"] = null },
            ["24"] = new JsonObject { ["mode"] = "legacy", ["activePlanId"] = null },
        },
    };

    private static JsonObject Example(bool healer)
    {
        var prefix = healer ? "example-whm-120" : "example-war-31";
        return new JsonObject
        {
            ["format"] = "xiv-rotation", ["version"] = 1, ["id"] = prefix,
            ["name"] = healer ? "WHM 动作120结构示例" : "WAR 动作31结构示例",
            ["jobId"] = healer ? 24u : 21u, ["entryPhaseId"] = prefix + "-phase",
            ["parameters"] = healer ? new JsonArray(new JsonObject
            {
                ["id"] = "hpThreshold", ["name"] = "血量阈值", ["type"] = "number",
                ["default"] = 70, ["min"] = 1, ["max"] = 100,
            }) : new JsonArray(),
            ["parameterValues"] = new JsonObject(), ["variables"] = new JsonArray(),
            ["phases"] = new JsonArray(new JsonObject
            {
                ["id"] = prefix + "-phase", ["name"] = "结构示例",
                ["onEnter"] = new JsonArray(), ["transitions"] = new JsonArray(),
                ["rules"] = new JsonArray(new JsonObject
                {
                    ["id"] = prefix + "-rule", ["name"] = "默认禁用", ["enabled"] = false,
                    ["condition"] = healer ? new JsonObject
                    {
                        ["id"] = prefix + "-condition", ["kind"] = "compare",
                        ["left"] = new JsonObject { ["kind"] = "field", ["path"] = "party.lowestHp" },
                        ["op"] = "lt",
                        ["right"] = new JsonObject { ["kind"] = "parameter", ["id"] = "hpThreshold" },
                    } : new JsonObject { ["id"] = prefix + "-condition", ["kind"] = "constant", ["value"] = true },
                    ["action"] = new JsonObject
                    {
                        ["actionId"] = healer ? 120u : 31u,
                        ["target"] = new JsonObject { ["kind"] = healer ? "lowestHp" : "current" },
                        ["ground"] = "none", ["allowPrepull"] = false, ["onSuccess"] = new JsonArray(),
                    },
                }),
            }),
        };
    }
}

public sealed class RotationConflictException(string message) : InvalidOperationException(message);
