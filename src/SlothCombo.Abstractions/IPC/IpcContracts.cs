using System;

namespace SlothCombo.Abstractions;

/// <summary>
/// The request the program sends to the runtime inside the game.
/// </summary>
/// <remarks>
/// The program and the runtime live in different processes, so the request
/// carries everything that is needed to answer it without knowing anything
/// about the connection itself.
/// </remarks>
public sealed class IpcRequest
{
    /// <summary>Gets or sets the method of the request.</summary>
    public string Method { get; set; } = "GET";

    /// <summary>Gets or sets the path of the request.</summary>
    public string Path { get; set; } = "/";

    /// <summary>Gets or sets the query of the request.</summary>
    public string Query { get; set; } = string.Empty;

    /// <summary>Gets or sets the body of the request.</summary>
    public string Body { get; set; } = string.Empty;

    /// <summary>Gets or sets the port the interface of the program listens on.</summary>
    public int Port { get; set; }
}

/// <summary>
/// The answer the runtime inside the game sends back.
/// </summary>
public sealed class IpcResponse
{
    /// <summary>Gets or sets the status of the answer.</summary>
    public int Status { get; set; } = 200;

    /// <summary>Gets or sets the kind of the body.</summary>
    public string ContentType { get; set; } = "application/json; charset=utf-8";

    /// <summary>Gets or sets the body of the answer.</summary>
    public string Body { get; set; } = string.Empty;

    /// <summary>Gets or sets the body of the answer, when it is not text.</summary>
    public string? Base64 { get; set; }
}

/// <summary>
/// The pipe the program and the runtime talk over.
/// </summary>
public static class IpcChannel
{
    /// <summary>Gets the name of the pipe.</summary>
    /// <remarks>
    /// The name carries the name of the user, so that two programs of two
    /// users on the same machine do not fight over the same pipe.
    /// </remarks>
    public static string PipeName => $"SlothCombo.Ipc.{Environment.UserName}";
}
