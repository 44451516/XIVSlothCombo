using System.Text.Json;
using System.Text.Json.Nodes;
using SlothCombo.Abstractions;
using SlothCombo.Runtime.Rotations;
using SlothCombo.Runtime.Services;
using SlothCombo.Runtime.WebUI.Contracts;

namespace SlothCombo.Runtime.WebUI.Handlers;

/// <summary>Dispatches live rotation operations to the game thread and serves static metadata off-thread.</summary>
internal static class RotationHandler
{
    // This bounds the API envelope, not an individual plan; IPC framing also counts encoding overhead.
    private const int MaxApiEnvelopeBytes = IpcFraming.MaxMessageLength;
    private static readonly Lazy<Task<IpcResponse>> CatalogResponse = new(() => Task.Run(() =>
        RotationTransport.CreateResponse(200, RotationTransport.Success(RotationStore.ExportCatalog()))));

    internal static async Task HandleAsync(ApiRequest request, ApiResponse response, GameThreadDispatcher dispatcher)
    {
        try
        {
            var segments = request.Segments;
            if (request.Method == "GET" && segments.Length == 3 && segments[2] == "catalog")
            {
                // Embedded data and scalar property metadata do not access live game state.
                RotationTransport.ApplyResponse(response, await CatalogResponse.Value.ConfigureAwait(false));
                return;
            }
            var (status, result) = await dispatcher.RunAsync(() => Execute(request)).ConfigureAwait(false);
            RotationTransport.ApplyResponse(response, RotationTransport.CreateResponse(status, result));
        }
        catch (IpcMessageTooLargeException ex)
        {
            response.SetJson(413, ApiResult.Failure("IPC_MESSAGE_TOO_LARGE",
                "完整响应超过 IPC 32 MiB 容量上限（含响应信封和转义）：" + ex.Message));
        }
        catch (RotationConflictException ex)
        {
            response.SetJson(409, ApiResult.Failure("ROTATION_CONFLICT", ex.Message));
        }
        catch (KeyNotFoundException ex)
        {
            response.SetJson(404, ApiResult.Failure("NOT_FOUND", ex.Message));
        }
        catch (Exception ex) when (ex is ArgumentException or JsonException or InvalidOperationException
                                   or FormatException or OverflowException)
        {
            response.SetJson(400, ApiResult.Failure("INVALID_ROTATION", ex.Message));
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            response.SetJson(500, ApiResult.Failure("ROTATION_STORAGE_ERROR", ex.Message));
        }
        catch (Exception ex)
        {
            response.SetJson(500, ApiResult.Failure("ROTATION_ERROR", ex.Message));
        }
    }

    private static (int Status, ApiResult Result) Execute(ApiRequest request)
    {
        var segments = request.Segments;
        if (segments.Length > 3) return Missing();
        var section = segments.Length == 3 ? segments[2] : "";
        if (request.Method == "GET")
        {
            return section switch
            {
                "" => Snapshot(allowFault: true),
                "state" => Success(RotationEngine.GetState()),
                _ => Missing(),
            };
        }
        if (request.Method != "POST")
            return (405, ApiResult.Failure("METHOD_NOT_ALLOWED", "不支持的方法。"));
        if (section is not ("" or "control")) return Missing();

        var body = RotationValidator.ParseObject(request.Body,
            MaxApiEnvelopeBytes, RotationValidator.MaxJsonDepth + 4,
            "轮转 API 请求信封（32 MiB，完整 IPC 消息还包括封装与转义）");
        if (section == "control")
        {
            Keys(body, "command", "emergencyHotkey", "expectedJobId");
            var command = Text(body, "command");
            if (command is not ("start" or "stop" or "reset"))
                throw new ArgumentException("command 只能是 start、stop 或 reset。");
            if (body.ContainsKey("emergencyHotkey")) Text(body, "emergencyHotkey");
            if (command == "start")
            {
                RotationEngine.ValidateStartJob(body);
                if (RotationStore.GetSnapshot()["failClosed"]!.GetValue<bool>())
                    throw new ArgumentException("轮转存储异常，请先修复原文件并重新初始化。");
            }
            else if (body.ContainsKey("expectedJobId"))
                throw new ArgumentException("expectedJobId 仅适用于 start。");
            RotationEngine.Stop("rotation-api-control");
            var result = RotationEngine.Control(body);
            if (result is JsonObject reply && reply["ok"]?.GetValue<bool>() == false)
                return Failure(400, "ROTATION_CONTROL_FAILED", reply["reason"]?.GetValue<string>() ?? "操作失败。", reply);
            return Success(result);
        }

        switch (Text(body, "operation"))
        {
            case "import":
                Keys(body, "operation", "plan", "overwrite");
                var plan = Object(body, "plan");
                var overwrite = false;
                if (body.ContainsKey("overwrite"))
                {
                    if (body["overwrite"] is not JsonValue value || !value.TryGetValue<bool>(out overwrite))
                        throw new ArgumentException("overwrite 必须是布尔值。");
                }
                // Save validates the entire candidate and conflicts before stopping the engine.
                RotationStore.Save(plan, overwrite);
                break;
            case "mode":
                Keys(body, "operation", "jobId", "mode");
                RotationStore.SetMode(RotationValidator.PositiveUInt(body["jobId"], "$.jobId"), Text(body, "mode"));
                break;
            case "select":
                Keys(body, "operation", "jobId", "planId");
                RotationStore.Select(RotationValidator.PositiveUInt(body["jobId"], "$.jobId"), Text(body, "planId"));
                break;
            case "parameters":
                Keys(body, "operation", "jobId", "values");
                RotationStore.SaveParameters(RotationValidator.PositiveUInt(body["jobId"], "$.jobId"),
                    Object(body, "values"));
                break;
            case "delete":
                Keys(body, "operation", "planId");
                RotationStore.Delete(Text(body, "planId"));
                break;
            default:
                throw new ArgumentException("未知的轮转操作。");
        }
        return Snapshot();
    }

    private static (int, ApiResult) Snapshot(bool allowFault = false)
    {
        var snapshot = RotationStore.GetSnapshot();
        return !allowFault && snapshot["failClosed"]!.GetValue<bool>()
            ? Failure(400, "ROTATION_STORE_INVALID",
                snapshot["initializationError"]?.GetValue<string>() ?? "轮转存储不可用。", snapshot)
            : Success(snapshot);
    }

    private static (int, ApiResult) Success(object? data) => (200, RotationTransport.Success(data));

    private static (int, ApiResult) Failure(int status, string code, string message, object? data) =>
        (status, RotationTransport.Failure(code, message, data));

    private static (int, ApiResult) Missing() => (404, ApiResult.Failure("NOT_FOUND", "轮转接口不存在。"));

    private static string Text(JsonObject body, string key)
    {
        if (body[key] is not JsonValue value || !value.TryGetValue<string>(out var text)
            || string.IsNullOrWhiteSpace(text) || text.Length > 160)
            throw new ArgumentException($"{key} 必须是非空字符串，长度不超过 160。");
        return text;
    }

    private static JsonObject Object(JsonObject body, string key) =>
        body[key] as JsonObject ?? throw new ArgumentException($"{key} 必须是 JSON 对象。");

    private static void Keys(JsonObject body, params string[] allowed)
    {
        foreach (var (key, _) in body)
            if (!allowed.Contains(key, StringComparer.Ordinal)) throw new ArgumentException($"未知的请求字段 {key}。");
    }
}
