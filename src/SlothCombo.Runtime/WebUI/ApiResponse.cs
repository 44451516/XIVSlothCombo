using System;
using Newtonsoft.Json;
using SlothCombo.Runtime.WebUI.Contracts;

namespace SlothCombo.Runtime.WebUI;

/// <summary>
/// The answer of the interface of the program.
/// </summary>
internal sealed class ApiResponse
{
    /// <summary>Gets or sets the status of the answer.</summary>
    public int Status { get; set; } = 200;

    /// <summary>Gets or sets the kind of the body.</summary>
    public string ContentType { get; set; } = "application/json; charset=utf-8";

    /// <summary>Gets or sets the body of the answer.</summary>
    public string Body { get; set; } = string.Empty;

    /// <summary>Gets or sets the body of the answer, when it is not text.</summary>
    public byte[]? Bytes { get; set; }

    /// <summary>
    /// Writes the answer of the interface into the answer.
    /// </summary>
    /// <param name="status">The status of the answer.</param>
    /// <param name="result">The result of the request.</param>
    public void SetJson(int status, ApiResult result)
    {
        Status = status;
        ContentType = "application/json; charset=utf-8";
        Body = JsonConvert.SerializeObject(result);
    }

    /// <summary>
    /// Writes a picture into the answer.
    /// </summary>
    /// <param name="status">The status of the answer.</param>
    /// <param name="contentType">The kind of the picture.</param>
    /// <param name="bytes">The picture itself.</param>
    public void SetBytes(int status, string contentType, byte[] bytes)
    {
        Status = status;
        ContentType = contentType;
        Bytes = bytes;
    }
}
