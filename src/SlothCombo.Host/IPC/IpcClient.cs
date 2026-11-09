using System;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;
using SlothCombo.Abstractions;

namespace SlothCombo.Host.Ipc;

/// <summary>
/// The end of the connection between the program and the runtime inside the game.
/// </summary>
/// <remarks>
/// The program asks the runtime inside the game to do the work that needs the
/// game. The two talk over a pipe of the machine.
/// </remarks>
internal sealed class IpcClient
{
    /// <summary>How long the program waits for the runtime to answer.</summary>
    private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(60);

    /// <summary>
    /// Asks the runtime to do the work and waits for the answer.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="token">The token that stops the wait.</param>
    /// <returns>The answer of the runtime.</returns>
    public async Task<IpcResponse> SendAsync(IpcRequest request, CancellationToken token)
    {
        using var pipe = new NamedPipeClientStream(
            ".",
            IpcChannel.PipeName,
            PipeDirection.InOut,
            PipeOptions.Asynchronous);

        await pipe.ConnectAsync((int)Timeout.TotalMilliseconds, token).ConfigureAwait(false);

        await IpcFraming.WriteAsync(pipe, request, token).ConfigureAwait(false);

        return await IpcFraming.ReadAsync<IpcResponse>(pipe, token).ConfigureAwait(false);
    }
}
