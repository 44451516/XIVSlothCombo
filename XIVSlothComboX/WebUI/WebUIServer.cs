using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dalamud.Plugin.Services;
using Newtonsoft.Json;
using XIVSlothComboX.Services;
using XIVSlothComboX.WebUI.Contracts;
using XIVSlothComboX.WebUI.Handlers;

namespace XIVSlothComboX.WebUI;

/// <summary>
/// Serves the Vue settings page and the JSON API from a local HTTP server that only
/// listens on the loopback interface.
/// </summary>
internal sealed class WebUIServer : IDisposable
{
    private const string ResourcePrefix = "WebUI.wwwroot.";

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
    private readonly MainThreadDispatcher _dispatcher;
    private readonly CancellationTokenSource _cancellation = new();
    private Task? _acceptLoop;
    private string? _infoFilePath;

    /// <summary> Creates the server and binds it to a free loopback port. </summary>
    public WebUIServer(IFramework framework)
    {
        _dispatcher = new MainThreadDispatcher(framework);
        _auth = new WebUIAuth(FindFreePort());
        _listener.Prefixes.Add($"http://127.0.0.1:{_auth.Port}/");
    }

    /// <summary> Port the server listens on. </summary>
    public int Port => _auth.Port;

    /// <summary> URL that opens the settings page in the default browser. </summary>
    public string Url => _auth.Url;

    /// <summary> Session token that every request has to carry. </summary>
    public string Token => _auth.Token;

    /// <summary> Name of the file that describes the running server to other tools. </summary>
    private const string InfoFileName = "sloth-webui.json";

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
                // Port is taken, try another one.
            }
        }

        throw new InvalidOperationException("Unable to find a free port for the web UI.");
    }

    /// <summary> Starts listening for requests. </summary>
    public void Start()
    {
        if (_acceptLoop != null) return;

        _dispatcher.Start();
        _listener.Start();
        _acceptLoop = Task.Run(() => AcceptLoopAsync(_cancellation.Token));
        WriteInfoFile();
        Service.PluginLog.Information($"XIVSlothComboX web UI listening on {_auth.Url}");
    }

    /// <summary> Stops the server and releases the port. </summary>
    public void Dispose()
    {
        try
        {
            _cancellation.Cancel();
            if (_listener.IsListening) _listener.Stop();
            _listener.Close();
        }
        catch (Exception ex)
        {
            Service.PluginLog.Debug($"Failed to stop the web UI server: {ex.Message}");
        }

        _dispatcher.Stop();
        _cancellation.Dispose();
        IconService.ClearCache();
        DeleteInfoFile();
    }

    /// <summary>
    /// Writes the port and the token of the running server into the plugin's
    /// configuration folder, so that the frontend development server can find it.
    /// </summary>
    private void WriteInfoFile()
    {
        try
        {
            var directory = Service.Interface.ConfigDirectory;
            if (directory == null) return;

            directory.Create();
            var path = Path.Combine(directory.FullName, InfoFileName);
            var json = JsonConvert.SerializeObject(new
            {
                port = _auth.Port,
                token = _auth.Token,
                url = _auth.Url,
            });

            File.WriteAllText(path, json);
            _infoFilePath = path;
        }
        catch (Exception ex)
        {
            Service.PluginLog.Debug($"Failed to write the web UI info file: {ex.Message}");
        }
    }

    /// <summary> Removes the info file again. </summary>
    private void DeleteInfoFile()
    {
        if (_infoFilePath == null) return;

        try
        {
            if (File.Exists(_infoFilePath)) File.Delete(_infoFilePath);
        }
        catch (Exception ex)
        {
            Service.PluginLog.Debug($"Failed to remove the web UI info file: {ex.Message}");
        }

        _infoFilePath = null;
    }

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
            catch (Exception ex)
            {
                Service.PluginLog.Debug($"Web UI accept loop error: {ex.Message}");
                break;
            }

            _ = Task.Run(() => HandleAsync(context), CancellationToken.None);
        }
    }

    private async Task HandleAsync(HttpListenerContext context)
    {
        try
        {
            var path = context.Request.Url?.AbsolutePath ?? "/";
            var token = context.Request.Headers["X-Sloth-Token"];
            if (string.IsNullOrEmpty(token)) token = context.Request.QueryString["t"];
            var origin = context.Request.Headers["Origin"];
            var referer = context.Request.Headers["Referer"];

            if (!_auth.Validate(token, origin, referer))
            {
                await WriteJsonAsync(context, 401, ApiResult.Failure("UNAUTHORIZED", "会话已失效，请在游戏内重新打开设置页")).ConfigureAwait(false);
                return;
            }

            if (path.StartsWith("/api/", StringComparison.Ordinal))
            {
                await HandleApiAsync(context, path).ConfigureAwait(false);
                return;
            }

            await ServeStaticAsync(context, path).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Service.PluginLog.Error(ex, "Web UI request failed");
            try
            {
                await WriteJsonAsync(context, 500, ApiResult.Failure("INTERNAL_ERROR", ex.Message)).ConfigureAwait(false);
            }
            catch
            {
                // The response was already sent or the client is gone.
            }
        }
    }

    private async Task HandleApiAsync(HttpListenerContext context, string path)
    {
        var method = context.Request.HttpMethod;
        var segments = path.Trim('/').Split('/');

        switch (segments[1])
        {
            case "health":
                await WriteJsonAsync(context, 200, ApiResult.Success(new
                {
                    version = PluginVersion,
                    port = _auth.Port,
                })).ConfigureAwait(false);
                return;

            case "meta":
                await WriteJsonAsync(context, 200, ApiResult.Success(await _dispatcher.RunAsync(() =>
                {
                    var meta = new MetaDto { Version = PluginVersion, Port = _auth.Port };
                    meta.Jobs.AddRange(PresetService.BuildJobs());
                    meta.Presets.AddRange(PresetService.BuildPresetNodes());
#if DEBUG
                    meta.Debug = true;
#endif
                    return meta;
                }))).ConfigureAwait(false);
                return;

            case "presets":
                await PresetHandler.HandleAsync(context, _dispatcher, segments).ConfigureAwait(false);
                return;

            case "value":
                await ValueHandler.HandleAsync(context, _dispatcher).ConfigureAwait(false);
                return;

            case "config":
                await ConfigHandler.HandleAsync(context, _dispatcher).ConfigureAwait(false);
                return;

            case "timeline":
                await TimelineHandler.HandleAsync(context, _dispatcher, segments).ConfigureAwait(false);
                return;

            case "icon":
                await IconHandler.HandleAsync(context, _dispatcher, segments).ConfigureAwait(false);
                return;

            case "debug":
                await DebugHandler.HandleAsync(context, _dispatcher, segments).ConfigureAwait(false);
                return;

            default:
                await WriteJsonAsync(context, 404, ApiResult.Failure("NOT_FOUND", $"未知接口 {path}")).ConfigureAwait(false);
                return;
        }
    }

    private static string PluginVersion =>
        System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "0.0.0.0";

    private static async Task ServeStaticAsync(HttpListenerContext context, string path)
    {
        var relative = path.TrimStart('/');
        if (string.IsNullOrEmpty(relative)) relative = "index.html";

        var fileName = relative.Contains('/') ? relative[(relative.LastIndexOf('/') + 1)..] : relative;
        var bytes = LoadResource(fileName);
        if (bytes == null)
        {
            await WriteJsonAsync(context, 404, ApiResult.Failure("NOT_FOUND", $"找不到 {relative}")).ConfigureAwait(false);
            return;
        }

        var extension = Path.GetExtension(fileName);
        var contentType = ContentTypes.TryGetValue(extension, out var type) ? type : "application/octet-stream";

        context.Response.StatusCode = 200;
        context.Response.ContentType = contentType;
        context.Response.Headers.Add("Cache-Control", "no-cache");
        if (extension.Equals(".html", StringComparison.OrdinalIgnoreCase))
        {
            context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'; img-src 'self' data:; style-src 'self' 'unsafe-inline'; script-src 'self'; connect-src 'self'; font-src 'self' data:; object-src 'none'; base-uri 'none'");
            context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
        }

        await context.Response.OutputStream.WriteAsync(bytes, 0, bytes.Length).ConfigureAwait(false);
        context.Response.Close();
    }

    private static byte[]? LoadResource(string fileName)
    {
        var assembly = System.Reflection.Assembly.GetExecutingAssembly();
        var resourceName = ResourcePrefix + fileName;

        using (var stream = assembly.GetManifestResourceStream(resourceName))
        {
            if (stream != null)
            {
                using var memory = new MemoryStream();
                stream.CopyTo(memory);
                return memory.ToArray();
            }
        }

        // Debug builds may read the files straight from disk.
        var onDisk = Path.Combine(AppContext.BaseDirectory, "wwwroot", fileName);
        if (File.Exists(onDisk)) return File.ReadAllBytes(onDisk);

        return null;
    }

    internal static async Task WriteJsonAsync(HttpListenerContext context, int statusCode, ApiResult result)
    {
        var json = JsonConvert.SerializeObject(result);
        var bytes = Encoding.UTF8.GetBytes(json);

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json; charset=utf-8";
        context.Response.Headers.Add("Cache-Control", "no-store");
        await context.Response.OutputStream.WriteAsync(bytes, 0, bytes.Length).ConfigureAwait(false);
        context.Response.Close();
    }

    internal static async Task<string> ReadBodyAsync(HttpListenerContext context)
    {
        using var reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding ?? Encoding.UTF8);
        return await reader.ReadToEndAsync().ConfigureAwait(false);
    }
}
