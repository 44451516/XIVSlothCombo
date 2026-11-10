using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SlothCombo.Abstractions;
using SlothCombo.Host.Ipc;

namespace SlothCombo.Host.WebUI;

/// <summary>
/// Serves the page of the settings and hands the work to the runtime in the game.
/// </summary>
/// <remarks>
/// <para>
/// The page of the settings and the server that shows it live outside of the
/// game. The server only listens on the loopback interface; the files of the
/// page are the same for every session, but every request that touches the
/// game has to carry the token of the session.
/// </para>
/// <para>
/// The work that needs the game is handed to the runtime inside the game over
/// a pipe, and the answer of the runtime is passed on unchanged.
/// </para>
/// </remarks>
internal sealed class WebUIServer : IDisposable
{
    private static readonly Dictionary<string, string> ContentTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        [".html"] = "text/html; charset=utf-8",
        [".js"] = "text/javascript; charset=utf-8",
        [".mjs"] = "text/javascript; charset=utf-8",
        [".css"] = "text/css; charset=utf-8",
        [".json"] = "application/json; charset=utf-8",
        [".png"] = "image/png",
        [".jpg"] = "image/jpeg",
        [".jpeg"] = "image/jpeg",
        [".gif"] = "image/gif",
        [".svg"] = "image/svg+xml",
        [".ico"] = "image/x-icon",
        [".woff"] = "font/woff",
        [".woff2"] = "font/woff2",
        [".ttf"] = "font/ttf",
        [".map"] = "application/json; charset=utf-8",
    };

    private readonly HttpListener _listener = new();
    private readonly WebUIAuth _auth;
    private readonly IpcClient _client;
    private readonly string _wwwroot;
    private readonly CancellationTokenSource _cancellation = new();
    private Task? _acceptLoop;

    /// <summary>Creates the server.</summary>
    /// <param name="client">The way to the runtime inside the game.</param>
    /// <param name="wwwroot">The folder that holds the page of the settings.</param>
    public WebUIServer(IpcClient client, string wwwroot)
    {
        _client = client;
        _wwwroot = wwwroot;

        _auth = new WebUIAuth(FindFreePort());
        _listener.Prefixes.Add($"http://127.0.0.1:{_auth.Port}/");
    }

    /// <summary>Port the server listens on.</summary>
    public int Port => _auth.Port;

    /// <summary>URL that opens the settings page in the default browser.</summary>
    public string Url => _auth.Url;

    /// <summary>Session token that every request has to carry.</summary>
    public string Token => _auth.Token;

    /// <summary>Starts listening for requests.</summary>
    public void Start()
    {
        if (_acceptLoop is not null)
        {
            return;
        }

        _listener.Start();
        _acceptLoop = Task.Run(() => AcceptLoopAsync(_cancellation.Token));
    }

    /// <summary>Stops the server and releases the port.</summary>
    public void Dispose()
    {
        try
        {
            _cancellation.Cancel();
            if (_listener.IsListening)
            {
                _listener.Stop();
            }

            _listener.Close();
        }
        catch (Exception)
        {
            // The port is released when the listener is closed.
        }

        _cancellation.Dispose();
    }

    /// <summary>
    /// Finds a port that nothing listens on.
    /// </summary>
    private static int FindFreePort()
    {
        for (var attempt = 0; attempt < 10; attempt++)
        {
            var port = Random.Shared.Next(20000, 60000);
            try
            {
                var probe = new TcpListener(IPAddress.Loopback, port);
                probe.Start();
                probe.Stop();
                return port;
            }
            catch (SocketException)
            {
                // The port is taken, so another one is tried.
            }
        }

        throw new InvalidOperationException("Unable to find a free port for the web UI.");
    }

    /// <summary>
    /// Waits for requests and answers them.
    /// </summary>
    private async Task AcceptLoopAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            HttpListenerContext context;
            try
            {
                context = await _listener.GetContextAsync().ConfigureAwait(false);
            }
            catch (Exception) when (token.IsCancellationRequested)
            {
                break;
            }
            catch (Exception)
            {
                break;
            }

            _ = Task.Run(() => HandleAsync(context), CancellationToken.None);
        }
    }

    /// <summary>
    /// Answers a single request.
    /// </summary>
    private async Task HandleAsync(HttpListenerContext context)
    {
        try
        {
            var path = context.Request.Url?.AbsolutePath ?? "/";

            if (NeedsToken(path))
            {
                var token = context.Request.Headers["X-Sloth-Token"];
                if (string.IsNullOrEmpty(token))
                {
                    token = context.Request.QueryString["t"];
                }

                var origin = context.Request.Headers["Origin"];
                var referer = context.Request.Headers["Referer"];

                if (!_auth.Validate(token, origin, referer))
                {
                    await WriteJsonAsync(context, 401, "{\"ok\":false,\"error\":{\"code\":\"UNAUTHORIZED\",\"message\":\"会话已失效，请重新打开设置页\"}}").ConfigureAwait(false);
                    return;
                }

                await HandleApiAsync(context, path).ConfigureAwait(false);
                return;
            }

            await ServeStaticAsync(context, path).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            try
            {
                await WriteJsonAsync(context, 500, $"{{\"ok\":false,\"error\":{{\"code\":\"INTERNAL_ERROR\",\"message\":\"{ex.Message}\"}}}}").ConfigureAwait(false);
            }
            catch (Exception)
            {
                // The answer was already sent or the client is gone.
            }
        }
    }

    /// <summary>
    /// Whether a request has to prove that it belongs to the session.
    /// </summary>
    /// <remarks>
    /// The page and its files are the same for every session, so the browser
    /// can load them like any other page. Only the work of the interface
    /// touches the game, so only the interface has to carry the token.
    /// </remarks>
    internal static bool NeedsToken(string path) => path.StartsWith("/api/", StringComparison.Ordinal);

    /// <summary>
    /// Hands the request to the runtime inside the game and passes the answer on.
    /// </summary>
    private async Task HandleApiAsync(HttpListenerContext context, string path)
    {
        var request = new IpcRequest
        {
            Method = context.Request.HttpMethod,
            Path = path,
            Query = context.Request.Url?.Query ?? string.Empty,
            Body = await ReadBodyAsync(context).ConfigureAwait(false),
            Port = _auth.Port,
        };

        IpcResponse response;
        try
        {
            response = await _client.SendAsync(request, _cancellation.Token).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            await WriteJsonAsync(context, 503, $"{{\"ok\":false,\"error\":{{\"code\":\"GAME_NOT_READY\",\"message\":\"游戏内的运行时没有应答：{ex.Message}\"}}}}").ConfigureAwait(false);
            return;
        }

        context.Response.StatusCode = response.Status;
        context.Response.ContentType = response.ContentType;

        if (response.Base64 is not null)
        {
            var bytes = Convert.FromBase64String(response.Base64);
            context.Response.Headers.Add("Cache-Control", "public, max-age=86400");
            await context.Response.OutputStream.WriteAsync(bytes).ConfigureAwait(false);
        }
        else
        {
            var bytes = Encoding.UTF8.GetBytes(response.Body);
            context.Response.Headers.Add("Cache-Control", "no-store");
            await context.Response.OutputStream.WriteAsync(bytes).ConfigureAwait(false);
        }

        context.Response.Close();
    }

    /// <summary>
    /// Serves a file of the page of the settings.
    /// </summary>
    private async Task ServeStaticAsync(HttpListenerContext context, string path)
    {
        var relative = path.TrimStart('/');
        if (string.IsNullOrEmpty(relative))
        {
            relative = "index.html";
        }

        var file = ResolveFile(relative);
        if (file is null)
        {
            await WriteJsonAsync(context, 404, $"{{\"ok\":false,\"error\":{{\"code\":\"NOT_FOUND\",\"message\":\"找不到 {relative}\"}}}}").ConfigureAwait(false);
            return;
        }

        var extension = Path.GetExtension(file);
        var contentType = ContentTypes.TryGetValue(extension, out var type) ? type : "application/octet-stream";

        context.Response.StatusCode = 200;
        context.Response.ContentType = contentType;
        context.Response.Headers.Add("Cache-Control", "no-cache");
        if (extension.Equals(".html", StringComparison.OrdinalIgnoreCase))
        {
            // The pictures of the game are taken from the hosts that serve the
            // game data, so they have to be allowed to be loaded.
            context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'; img-src 'self' data: https://cafemaker.wakingsands.com https://xivapi.com; style-src 'self' 'unsafe-inline'; script-src 'self'; connect-src 'self'; font-src 'self' data:; object-src 'none'; base-uri 'none'");
            context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
        }

        var bytes = await File.ReadAllBytesAsync(file).ConfigureAwait(false);
        await context.Response.OutputStream.WriteAsync(bytes).ConfigureAwait(false);
        context.Response.Close();
    }

    /// <summary>
    /// Works out which file of the page of the settings a path belongs to.
    /// </summary>
    /// <remarks>
    /// The path may not leave the folder of the page, so a path that climbs out
    /// of it is refused.
    /// </remarks>
    private string? ResolveFile(string relative)
    {
        if (relative.Contains("..", StringComparison.Ordinal))
        {
            return null;
        }

        var root = Path.GetFullPath(_wwwroot);
        var file = Path.GetFullPath(Path.Combine(root, relative.Replace('/', Path.DirectorySeparatorChar)));

        if (!file.StartsWith(root, StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        return File.Exists(file) ? file : null;
    }

    /// <summary>
    /// Reads the body of a request.
    /// </summary>
    private static async Task<string> ReadBodyAsync(HttpListenerContext context)
    {
        if (!context.Request.HasEntityBody)
        {
            return string.Empty;
        }

        using var reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding ?? Encoding.UTF8);
        return await reader.ReadToEndAsync().ConfigureAwait(false);
    }

    /// <summary>
    /// Writes an answer that is already in the shape the page expects.
    /// </summary>
    private static async Task WriteJsonAsync(HttpListenerContext context, int statusCode, string json)
    {
        var bytes = Encoding.UTF8.GetBytes(json);

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json; charset=utf-8";
        context.Response.Headers.Add("Cache-Control", "no-store");
        await context.Response.OutputStream.WriteAsync(bytes).ConfigureAwait(false);
        context.Response.Close();
    }
}
