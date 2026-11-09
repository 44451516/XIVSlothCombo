using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading;
using SlothCombo.Host.Ipc;
using SlothCombo.Host.Injection;
using SlothCombo.Host.WebUI;

namespace SlothCombo.Host;

/// <summary>
/// The entry point of the program.
/// </summary>
/// <remarks>
/// The program finds the game, loads the runtime into it, serves the page of
/// the settings and hands the work that needs the game to the runtime.
/// Everything else happens inside the game.
/// </remarks>
internal static class Program
{
    /// <summary>How long the boot is given to report its state.</summary>
    private static readonly TimeSpan BootTimeout = TimeSpan.FromSeconds(60);

    /// <summary>The name of the file that describes the running page.</summary>
    private const string InfoFileName = "sloth-webui.json";

    /// <summary>The page of the settings.</summary>
    private static WebUIServer? _server;

    private static int Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        try
        {
            var options = HostOptions.Parse(args);
            if (options.ShowHelp)
            {
                HostOptions.WriteHelp();
                return 0;
            }

            return Run(options);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"The program stopped: {ex.Message}");
            return 1;
        }
    }

    /// <summary>
    /// Loads the runtime into the game.
    /// </summary>
    private static int Run(HostOptions options)
    {
        var baseFolder = AppContext.BaseDirectory;
        var bootLibrary = Path.Combine(baseFolder, "SlothCombo.Boot.dll");

        if (!File.Exists(bootLibrary))
        {
            Console.Error.WriteLine($"The boot library is missing: {bootLibrary}");
            return 1;
        }

        var hostFxr = FindHostFxr(baseFolder);
        if (hostFxr.Length == 0)
        {
            Console.Error.WriteLine($"The library that hosts the runtime is missing under '{baseFolder}'.");
            return 1;
        }

        var runtimeFolder = Path.Combine(baseFolder, "game");
        var runtimeConfig = Path.Combine(runtimeFolder, "SlothCombo.Runtime.runtimeconfig.json");
        var assembly = Path.Combine(runtimeFolder, "SlothCombo.Runtime.dll");

        if (!File.Exists(runtimeConfig) || !File.Exists(assembly))
        {
            Console.Error.WriteLine($"The managed part of the runtime is missing under '{runtimeFolder}'.");
            return 1;
        }

        var process = options.ProcessId is { } pid
            ? TryGetProcess(pid)
            : GameProcessLocator.Find();

        if (process is null)
        {
            Console.Error.WriteLine("The game is not running. Start it and try again.");
            return 1;
        }

        var dataFolder = options.DataFolder ?? GetDefaultDataFolder();
        Directory.CreateDirectory(dataFolder);

        var statusPath = Path.Combine(dataFolder, "boot-status.json");
        TryDelete(statusPath);

        var settings = new BootSettings
        {
            HostFxrPath = hostFxr,
            RuntimeConfigPath = runtimeConfig,
            AssemblyPath = assembly,
            TypeName = "SlothCombo.Runtime.Bootstrap.SlothComboRuntime, SlothCombo.Runtime",
            MethodName = "Boot",
            DataFolder = dataFolder,
            GameInstallPath = options.GameInstallPath ?? FindGameDirectory(process),
            StatusPath = statusPath,
        };

        BootSettings.Write(settings);

        Console.WriteLine($"Injecting the runtime into the game (process {process.Id})...");
        DllInjector.Inject(process.Id, bootLibrary, "SlothComboBoot");

        var status = WaitForStatus(statusPath);
        switch (status)
        {
            case "running":
                Console.WriteLine("The runtime is running.");
                Console.WriteLine($"Log: {Path.Combine(dataFolder, "logs")}");

                StartInterface(dataFolder, options);
                WaitForGame(process);
                return 0;

            case "failed":
                Console.Error.WriteLine("The runtime could not be started.");
                Console.Error.WriteLine(ReadError(dataFolder));
                return 1;

            default:
                Console.Error.WriteLine("The game did not report back in time.");
                return 1;
        }
    }

    /// <summary>
    /// Starts the page of the settings.
    /// </summary>
    /// <remarks>
    /// The page is served by the program itself. The work that needs the game
    /// is handed to the runtime inside the game over a pipe.
    /// </remarks>
    private static void StartInterface(string dataFolder, HostOptions options)
    {
        var wwwroot = Path.Combine(AppContext.BaseDirectory, "wwwroot");
        var server = new WebUIServer(new IpcClient(), wwwroot);
        server.Start();

        _server = server;

        WriteInfoFile(dataFolder, server);

        Console.WriteLine($"The settings page is at {server.Url}");

        if (!options.NoBrowser)
        {
            OpenBrowser(server.Url);
        }
    }

    /// <summary>
    /// Writes the port and the token of the page into the folder of the data,
    /// so that the tools around it can find them.
    /// </summary>
    private static void WriteInfoFile(string dataFolder, WebUIServer server)
    {
        try
        {
            Directory.CreateDirectory(dataFolder);

            var json = JsonSerializer.Serialize(new
            {
                port = server.Port,
                token = server.Token,
                url = server.Url,
            });

            File.WriteAllText(Path.Combine(dataFolder, InfoFileName), json);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"The description of the page could not be written: {ex.Message}");
        }
    }

    /// <summary>
    /// Opens the page of the settings in the browser of the machine.
    /// </summary>
    private static void OpenBrowser(string url)
    {
        try
        {
            using var process = Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"The browser could not be opened: {ex.Message}");
        }
    }

    /// <summary>
    /// Stays with the program until the game is closed.
    /// </summary>
    private static void WaitForGame(Process process)
    {
        Console.WriteLine("The program stays with the game. Press Ctrl+C to close it.");

        while (!process.HasExited)
        {
            Thread.Sleep(500);
        }
    }

    /// <summary>
    /// Finds the folder the game is installed in.
    /// </summary>
    /// <remarks>
    /// The client lives in the folder of the game, and the data of the game
    /// sits next to it. The folder is taken from the program of the game, so
    /// it is right no matter where the game was installed.
    /// </remarks>
    private static string FindGameDirectory(Process process)
    {
        try
        {
            var path = process.MainModule?.FileName;
            if (string.IsNullOrEmpty(path))
            {
                return string.Empty;
            }

            var directory = Path.GetDirectoryName(path);
            return string.IsNullOrEmpty(directory) ? string.Empty : directory;
        }
        catch (Exception)
        {
            // The program of the game cannot always be read. The runtime
            // looks for the folder on its own when it is not given.
            return string.Empty;
        }
    }

    /// <summary>
    /// Finds the library that hosts the runtime.
    /// </summary>
    /// <remarks>
    /// The library works out where the runtime lives from its own place, so
    /// it has to sit in the folder the runtime was installed in, under the
    /// place the host of the runtime looks for it as well.
    /// </remarks>
    private static string FindHostFxr(string baseFolder)
    {
        var folder = Path.Combine(baseFolder, "host", "fxr");
        if (!Directory.Exists(folder))
        {
            return string.Empty;
        }

        var newest = Directory
            .GetDirectories(folder)
            .Select(path => (Path: path, Version: ParseVersion(Path.GetFileName(path))))
            .Where(entry => entry.Version is not null)
            .OrderByDescending(entry => entry.Version)
            .FirstOrDefault();

        if (newest.Path is null)
        {
            return string.Empty;
        }

        var library = Path.Combine(newest.Path, "hostfxr.dll");
        return File.Exists(library) ? library : string.Empty;
    }

    /// <summary>
    /// Reads the version out of the name of a folder.
    /// </summary>
    private static Version? ParseVersion(string name) => Version.TryParse(name, out var version) ? version : null;

    /// <summary>
    /// Waits for the boot library to report its state.
    /// </summary>
    private static string? WaitForStatus(string path)
    {
        var deadline = DateTime.UtcNow + BootTimeout;

        while (DateTime.UtcNow < deadline)
        {
            var status = BootStatus.Read(path);
            if (status is not null && !string.IsNullOrEmpty(status.State))
            {
                return status.State;
            }

            Thread.Sleep(200);
        }

        return null;
    }

    /// <summary>
    /// Reads the details of a failure that the runtime reported.
    /// </summary>
    private static string ReadError(string dataFolder)
    {
        var path = Path.Combine(dataFolder, "boot-error.txt");
        try
        {
            return File.Exists(path) ? File.ReadAllText(path) : "No details were reported.";
        }
        catch (Exception)
        {
            return "No details were reported.";
        }
    }

    /// <summary>
    /// Gets the process with the given id, if it is still running.
    /// </summary>
    private static Process? TryGetProcess(int id)
    {
        try
        {
            return Process.GetProcessById(id);
        }
        catch (Exception)
        {
            return null;
        }
    }

    /// <summary>
    /// Deletes a file, ignoring the failures.
    /// </summary>
    private static void TryDelete(string path)
    {
        try
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        catch (Exception)
        {
            // A file that cannot be deleted is overwritten by the boot anyway.
        }
    }

    /// <summary>
    /// Gets the folder the program keeps its data in.
    /// </summary>
    private static string GetDefaultDataFolder()
    {
        var root = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        return Path.Combine(root, "XIVSlothComboX");
    }
}
