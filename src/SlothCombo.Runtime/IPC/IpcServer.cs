using System;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;
using SlothCombo.Abstractions;
using SlothCombo.Runtime.Services;
using SlothCombo.Runtime.WebUI;
using SlothCombo.Runtime.WebUI.Contracts;

namespace SlothCombo.Runtime.Ipc;

/// <summary>
/// The end of the connection between the program and the runtime inside the game.
/// </summary>
/// <remarks>
/// <para>
/// The program that shows the interface runs outside of the game and asks the
/// runtime inside the game to do the work. The two talk over a pipe of the
/// machine.
/// </para>
/// <para>
/// The work itself has to happen on the thread of the game, so every request
/// is handed to the dispatcher before it is answered.
/// </para>
/// </remarks>
internal sealed class IpcServer : IDisposable
{
    private readonly GameThreadDispatcher _dispatcher;
    private readonly CancellationTokenSource _cancellation = new();
    private Task? _acceptLoop;

    /// <summary>Creates the server for the given dispatcher.</summary>
    /// <param name="dispatcher">The way onto the thread of the game.</param>
    public IpcServer(GameThreadDispatcher dispatcher) => _dispatcher = dispatcher;

    /// <summary>Starts listening for the program.</summary>
    public void Start()
    {
        if (_acceptLoop is not null)
        {
            return;
        }

        _acceptLoop = Task.Run(() => AcceptLoopAsync(_cancellation.Token));
    }

    /// <summary>Stops listening and closes the pipe.</summary>
    public void Dispose()
    {
        _cancellation.Cancel();
        _cancellation.Dispose();
    }

    /// <summary>
    /// Waits for the program and answers what it asks.
    /// </summary>
    private async Task AcceptLoopAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            NamedPipeServerStream pipe;
            try
            {
                pipe = new NamedPipeServerStream(
                    IpcChannel.PipeName,
                    PipeDirection.InOut,
                    1,
                    PipeTransmissionMode.Byte,
                    PipeOptions.Asynchronous);
            }
            catch (Exception ex)
            {
                Service.PluginLog?.Warning($"The pipe of the interface could not be opened: {ex.Message}");
                return;
            }

            try
            {
                await pipe.WaitForConnectionAsync(token).ConfigureAwait(false);
                await HandleAsync(pipe, token).ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                return;
            }
            catch (Exception ex)
            {
                Service.PluginLog?.Debug($"The connection of the interface failed: {ex.Message}");
            }
            finally
            {
                await pipe.DisposeAsync().ConfigureAwait(false);
            }
        }
    }

    /// <summary>
    /// Answers a single request.
    /// </summary>
    private async Task HandleAsync(NamedPipeServerStream pipe, CancellationToken token)
    {
        IpcRequest request;
        try
        {
            request = await IpcFraming.ReadAsync<IpcRequest>(pipe, token).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Service.PluginLog?.Debug($"The request of the interface could not be read: {ex.Message}");
            return;
        }

        var response = await AnswerAsync(request).ConfigureAwait(false);

        try
        {
            await IpcFraming.WriteAsync(pipe, response, token).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Service.PluginLog?.Debug($"The answer of the interface could not be written: {ex.Message}");
        }
    }

    /// <summary>
    /// Works out the answer to a request.
    /// </summary>
    private async Task<IpcResponse> AnswerAsync(IpcRequest request)
    {
        var apiRequest = new ApiRequest
        {
            Method = request.Method,
            Path = request.Path,
            Query = request.Query,
            Body = request.Body,
            Port = request.Port,
        };

        var apiResponse = new ApiResponse();

        try
        {
            await ApiRouter.HandleAsync(apiRequest, apiResponse, _dispatcher).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Service.PluginLog?.Error(ex, "The request of the interface failed.");
            apiResponse.SetJson(500, ApiResult.Failure("INTERNAL_ERROR", ex.Message));
        }

        var response = new IpcResponse
        {
            Status = apiResponse.Status,
            ContentType = apiResponse.ContentType,
            Body = apiResponse.Body,
        };

        if (apiResponse.Bytes is not null)
        {
            response.Base64 = Convert.ToBase64String(apiResponse.Bytes);
        }

        return response;
    }
}
