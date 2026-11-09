using System;
using System.IO;
using System.Net;
using System.Net.Http;
using SlothCombo.Host.Ipc;
using SlothCombo.Host.WebUI;

namespace SlothCombo.Tests;

/// <summary>
/// The tests of the page of the settings.
/// </summary>
/// <remarks>
/// The browser loads the script and the stylesheet of the page itself, without
/// the token of the session. The test starts the server on a page of its own
/// and checks that those files are served and that the work which touches the
/// game is still refused without the token.
/// </remarks>
internal static class WebUIContentTest
{
    /// <summary>
    /// Serves the files of the page without the token and guards the game.
    /// </summary>
    public static void ServesFilesAndGuardsTheGame()
    {
        var folder = Path.Combine(Path.GetTempPath(), "slothcombo-tests", "web");
        var assets = Path.Combine(folder, "assets");
        Directory.CreateDirectory(assets);
        File.WriteAllText(Path.Combine(folder, "index.html"), "<!doctype html><title>settings</title>");
        File.WriteAllText(Path.Combine(assets, "app.js"), "export const ready = true;");

        using var server = new WebUIServer(new IpcClient(), folder);
        try
        {
            server.Start();
        }
        catch (HttpListenerException)
        {
            throw new SkipException("The loopback interface cannot be listened on in this session.");
        }

        using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(10) };
        var root = $"http://127.0.0.1:{server.Port}";

        Assert.Equal(HttpStatusCode.OK, Get(client, $"{root}/").StatusCode);
        Assert.Equal(HttpStatusCode.OK, Get(client, $"{root}/assets/app.js").StatusCode);
        Assert.Equal(HttpStatusCode.OK, Get(client, $"{root}/?t={server.Token}").StatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, Get(client, $"{root}/api/status").StatusCode);
    }

    private static HttpResponseMessage Get(HttpClient client, string url)
        => client.GetAsync(url).GetAwaiter().GetResult();
}