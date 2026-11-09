using Newtonsoft.Json;

namespace SlothCombo.Runtime.WebUI.Contracts;

/// <summary> Unified response envelope for every <c>/api/*</c> endpoint. </summary>
public sealed class ApiResult
{
    /// <summary> Whether the request succeeded. </summary>
    [JsonProperty("ok")]
    public bool Ok { get; set; }

    /// <summary> Endpoint specific payload, present when <see cref="Ok"/> is true. </summary>
    [JsonProperty("data")]
    public object? Data { get; set; }

    /// <summary> Error details, present when <see cref="Ok"/> is false. </summary>
    [JsonProperty("error")]
    public ApiError? Error { get; set; }

    /// <summary> Builds a successful envelope. </summary>
    public static ApiResult Success(object? data = null) => new() { Ok = true, Data = data };

    /// <summary> Builds a failed envelope. </summary>
    public static ApiResult Failure(string code, string message) =>
        new() { Ok = false, Error = new ApiError { Code = code, Message = message } };
}

/// <summary> Error payload carried by a failed <see cref="ApiResult"/>. </summary>
public sealed class ApiError
{
    /// <summary> Machine readable error code. </summary>
    [JsonProperty("code")]
    public string Code { get; set; } = string.Empty;

    /// <summary> Human readable error message. </summary>
    [JsonProperty("message")]
    public string Message { get; set; } = string.Empty;
}
