using System;
using System.Net;
using System.Threading.Tasks;
using XIVSlothComboX.WebUI.Contracts;

namespace XIVSlothComboX.WebUI.Handlers;

/// <summary> Serves game icons as PNG images. </summary>
internal static class IconHandler
{
    internal static async Task HandleAsync(HttpListenerContext context, MainThreadDispatcher dispatcher, string[] segments)
    {
        if (segments.Length < 3 || !uint.TryParse(segments[2], out var iconId))
        {
            await WebUIServer.WriteJsonAsync(context, 400, ApiResult.Failure("BAD_REQUEST", "缺少图标 id")).ConfigureAwait(false);
            return;
        }

        var hd = !string.Equals(context.Request.QueryString["hd"], "0", StringComparison.Ordinal);
        var png = await dispatcher.RunAsync(() => IconService.GetPng(iconId, hd)).ConfigureAwait(false);

        if (png == null)
        {
            await WebUIServer.WriteJsonAsync(context, 404, ApiResult.Failure("ICON_NOT_FOUND", $"找不到图标 {iconId}")).ConfigureAwait(false);
            return;
        }

        context.Response.StatusCode = 200;
        context.Response.ContentType = "image/png";
        context.Response.Headers.Add("Cache-Control", "public, max-age=86400");
        await context.Response.OutputStream.WriteAsync(png, 0, png.Length).ConfigureAwait(false);
        context.Response.Close();
    }
}
