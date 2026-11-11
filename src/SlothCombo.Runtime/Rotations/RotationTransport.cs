using System.Text.Json;
using System.Text.Json.Nodes;
using Newtonsoft.Json.Linq;
using SlothCombo.Abstractions;
using SlothCombo.Runtime.WebUI;
using SlothCombo.Runtime.WebUI.Contracts;

namespace SlothCombo.Runtime.Rotations;

/// <summary>Uses the actual API and IPC serializers for rotation responses and capacity preflight.</summary>
internal static class RotationTransport
{
    private static readonly JsonSerializerOptions TransportOptions = new()
    {
        MaxDepth = RotationValidator.MaxJsonDepth + 4,
    };

    internal static ApiResult Success(object? data) => ApiResult.Success(Transport(data));

    internal static ApiResult Failure(string code, string message, object? data)
    {
        var result = ApiResult.Failure(code, message);
        result.Data = Transport(data);
        return result;
    }

    internal static IpcResponse CreateResponse(int status, ApiResult result)
    {
        var response = new ApiResponse();
        response.SetJson(status, result);
        // Match IpcServer's JSON response, including null Base64 and the content type.
        var message = new IpcResponse
        {
            Status = response.Status,
            ContentType = response.ContentType,
            Body = response.Body,
        };
        IpcFraming.ValidateMessageSize(message);
        return message;
    }

    internal static void ApplyResponse(ApiResponse response, IpcResponse message)
    {
        response.Status = message.Status;
        response.ContentType = message.ContentType;
        response.Body = message.Body;
        response.Bytes = null;
    }

    // ApiResponse uses Newtonsoft.Json; do not let it reflect over System.Text.Json nodes.
    private static object? Transport(object? data)
    {
        if (data is not JsonNode node) return data;
        using var text = new StringReader(node.ToJsonString(TransportOptions));
        using var reader = new Newtonsoft.Json.JsonTextReader(text) { MaxDepth = RotationValidator.MaxJsonDepth + 4 };
        return JToken.ReadFrom(reader);
    }
}
