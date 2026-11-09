using System;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using SlothCombo.Abstractions;

namespace SlothCombo.Tests;

/// <summary>
/// The tests of the messages that the program and the runtime exchange.
/// </summary>
internal static class IpcTest
{
    /// <summary>
    /// Writes a message and reads it back.
    /// </summary>
    public static void RoundTrip()
    {
        var request = new IpcRequest
        {
            Method = "POST",
            Path = "/api/presets/RPR_ST_SoD/toggle",
            Query = "?hd=1&t=abc",
            Body = "{\"enabled\":true}",
            Port = 41234,
        };

        using var stream = new MemoryStream();

        IpcFraming.WriteAsync(stream, request, CancellationToken.None).GetAwaiter().GetResult();

        stream.Position = 0;
        var read = IpcFraming.ReadAsync<IpcRequest>(stream, CancellationToken.None).GetAwaiter().GetResult();

        Assert.Equal(request.Method, read.Method);
        Assert.Equal(request.Path, read.Path);
        Assert.Equal(request.Query, read.Query);
        Assert.Equal(request.Body, read.Body);
        Assert.Equal(request.Port, read.Port);
    }

    /// <summary>
    /// Reads a message that carries a picture.
    /// </summary>
    public static void CarriesPicture()
    {
        var response = new IpcResponse
        {
            Status = 200,
            ContentType = "image/png",
            Base64 = Convert.ToBase64String(new byte[] { 1, 2, 3, 4, 5 }),
        };

        using var stream = new MemoryStream();

        IpcFraming.WriteAsync(stream, response, CancellationToken.None).GetAwaiter().GetResult();

        stream.Position = 0;
        var read = IpcFraming.ReadAsync<IpcResponse>(stream, CancellationToken.None).GetAwaiter().GetResult();

        Assert.Equal(response.Status, read.Status);
        Assert.Equal(response.ContentType, read.ContentType);
        Assert.Equal(5, Convert.FromBase64String(read.Base64!).Length);
    }

    /// <summary>
    /// The name of the pipe carries the name of the user, so that two
    /// programs of two users do not fight over the same pipe.
    /// </summary>
    public static void PipeCarriesUser()
    {
        Assert.True(
            IpcChannel.PipeName.Contains(Environment.UserName, StringComparison.Ordinal),
            "The pipe of the interface does not carry the name of the user.");
    }

    /// <summary>
    /// Talks to the runtime over a pipe of the machine.
    /// </summary>
    public static void ConnectsOverPipe()
    {
        using var server = new NamedPipeServerStream(
            IpcChannel.PipeName,
            PipeDirection.InOut,
            1,
            PipeTransmissionMode.Byte,
            PipeOptions.Asynchronous);

        var accept = server.WaitForConnectionAsync();

        var client = new SlothCombo.Host.Ipc.IpcClient();
        var send = client.SendAsync(
            new IpcRequest { Method = "GET", Path = "/api/health", Port = 1234 },
            CancellationToken.None);

        accept.GetAwaiter().GetResult();

        var request = IpcFraming.ReadAsync<IpcRequest>(server, CancellationToken.None).GetAwaiter().GetResult();
        Assert.Equal("/api/health", request.Path);
        Assert.Equal(1234, request.Port);

        IpcFraming.WriteAsync(
            server,
            new IpcResponse { Status = 200, Body = "{\"ok\":true}" },
            CancellationToken.None).GetAwaiter().GetResult();

        var response = send.GetAwaiter().GetResult();
        Assert.Equal(200, response.Status);
        Assert.Equal("{\"ok\":true}", response.Body);
    }
}
