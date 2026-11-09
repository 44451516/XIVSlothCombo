using System;
using System.Reflection;
using System.Threading.Tasks;
using SlothCombo.Runtime.Services;
using SlothCombo.Runtime.WebUI.Contracts;
using SlothCombo.Runtime.WebUI.Handlers;

namespace SlothCombo.Runtime.WebUI;

/// <summary>
/// Hands a request of the interface to the part that answers it.
/// </summary>
internal static class ApiRouter
{
    /// <summary>
    /// Answers a request of the interface.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="response">The answer.</param>
    /// <param name="dispatcher">The way onto the thread of the game.</param>
    public static async Task HandleAsync(ApiRequest request, ApiResponse response, GameThreadDispatcher dispatcher)
    {
        var segments = request.Segments;
        var section = segments.Length > 1 ? segments[1] : string.Empty;

        switch (section)
        {
            case "health":
                response.SetJson(200, ApiResult.Success(new
                {
                    version = Version,
                    port = request.Port,
                }));
                return;

            case "meta":
                response.SetJson(200, ApiResult.Success(await dispatcher.RunAsync(() =>
                {
                    var meta = new MetaDto { Version = Version, Port = request.Port };
                    meta.Jobs.AddRange(PresetService.BuildJobs());
                    meta.Presets.AddRange(PresetService.BuildPresetNodes());
                    return meta;
                })));
                return;

            case "presets":
                await PresetHandler.HandleAsync(request, response, dispatcher).ConfigureAwait(false);
                return;

            case "value":
                await ValueHandler.HandleAsync(request, response, dispatcher).ConfigureAwait(false);
                return;

            case "config":
                await ConfigHandler.HandleAsync(request, response, dispatcher).ConfigureAwait(false);
                return;

            case "timeline":
                await TimelineHandler.HandleAsync(request, response, dispatcher).ConfigureAwait(false);
                return;

            case "icon":
                await IconHandler.HandleAsync(request, response, dispatcher).ConfigureAwait(false);
                return;

            case "debug":
                await DebugHandler.HandleAsync(request, response, dispatcher).ConfigureAwait(false);
                return;

            default:
                response.SetJson(404, ApiResult.Failure("NOT_FOUND", $"未知接口 {request.Path}"));
                return;
        }
    }

    /// <summary>Gets the version of the runtime.</summary>
    public static string Version =>
        Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "0.0.0.0";
}
