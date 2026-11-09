using System;

using SlothCombo.Runtime.WebUI;
using SlothCombo.Runtime.Services;

using SlothCombo.Runtime.WebUI.Contracts;

namespace SlothCombo.Runtime.WebUI.Handlers;

/// <summary> Serves game icons as PNG images. </summary>
internal static class IconHandler
{
    internal static async Task HandleAsync(ApiRequest apiRequest, ApiResponse response, GameThreadDispatcher dispatcher)
    {
        if (apiRequest.Segments.Length < 3 || !uint.TryParse(apiRequest.Segments[2], out var iconId))
        {
            response.SetJson(400, ApiResult.Failure("BAD_REQUEST", "缺少图标 id"));
            return;
        }

        var hd = !string.Equals(apiRequest.QueryValue("hd"), "0", StringComparison.Ordinal);
        var png = await dispatcher.RunAsync(() => IconService.GetPng(iconId, hd)).ConfigureAwait(false);

        if (png == null)
        {
            response.SetJson(404, ApiResult.Failure("ICON_NOT_FOUND", $"找不到图标 {iconId}"));
            return;
        }

        response.SetBytes(200, "image/png", png);
    }
}
