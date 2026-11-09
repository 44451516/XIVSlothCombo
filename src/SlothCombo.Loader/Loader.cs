using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Loader;
using System.Threading;

namespace SlothCombo.Loader;

/// <summary>
/// Loads the managed part of the runtime into the game and replaces it when a
/// newer build is available.
/// </summary>
/// <remarks>
/// <para>
/// The library that starts the runtime of the system hands an assembly to the
/// default load context of the process, and that context lives until the
/// process ends. An assembly that is loaded that way keeps its code and its
/// files for the rest of the session, so every change of the runtime would
/// need a restart of the game.
/// </para>
/// <para>
/// This loader is the one assembly that is loaded that way. It keeps the
/// runtime in a collectible context of its own, so a new build can take the
/// place of the old one without restarting the game: the old runtime is
/// stopped, its context is unloaded, and the assembly of the new build is
/// loaded into a fresh context.
/// </para>
/// <para>
/// The runtime is only ever reached through reflection. A typed reference
/// would load the runtime next to the loader, in the context that can never
/// be unloaded, and would defeat the whole point.
/// </para>
/// </remarks>
public static class Loader
{
    private const string RuntimeAssemblyName = "SlothCombo.Runtime";
    private const string RuntimeTypeName = "SlothCombo.Runtime.Bootstrap.SlothComboRuntime";
    private const string BootMethodName = "BootManaged";
    private const string StopMethodName = "StopManaged";

    private static readonly object Gate = new();

    private static AssemblyLoadContext? _context;
    private static Func<string?, string?, int>? _boot;
    private static Action? _stop;
    private static string? _folder;

    /// <summary>
    /// Starts the runtime, or replaces the one that is already running.
    /// </summary>
    /// <param name="runtimePathUtf16">The path of the assembly of the runtime.</param>
    /// <param name="dataFolderUtf16">The folder the runtime keeps its data in.</param>
    /// <param name="gameInstallPathUtf16">The installation directory of the game.</param>
    /// <returns>Zero when the runtime is running.</returns>
    /// <remarks>
    /// The boot library calls this through the function pointer it reads from
    /// the assembly, which is why the paths arrive as plain pointers.
    /// </remarks>
    [UnmanagedCallersOnly]
    public static int Boot(IntPtr runtimePathUtf16, IntPtr dataFolderUtf16, IntPtr gameInstallPathUtf16)
    {
        var runtimePath = FromNative(runtimePathUtf16);
        var dataFolder = FromNative(dataFolderUtf16);
        var gameInstallPath = FromNative(gameInstallPathUtf16);

        try
        {
            lock (Gate)
            {
                // A runtime that sits in the default context was started by an
                // older build of this program, which had no way of replacing
                // it. Starting a second one would install every hook twice.
                if (_context is null && FindRunningRuntimeInDefaultContext() is { } reason)
                {
                    return Fail(dataFolder, reason);
                }

                return BootCore(runtimePath, dataFolder, gameInstallPath);
            }
        }
        catch (Exception ex)
        {
            return Fail(dataFolder, ex.ToString());
        }
    }

    /// <summary>
    /// The work of the entry point, reachable from the tests.
    /// </summary>
    /// <param name="runtimePath">The path of the assembly of the runtime.</param>
    /// <param name="dataFolder">The folder the runtime keeps its data in.</param>
    /// <param name="gameInstallPath">The installation directory of the game.</param>
    /// <returns>Zero when the runtime is running.</returns>
    internal static int BootCore(string runtimePath, string? dataFolder, string? gameInstallPath)
    {
        lock (Gate)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(runtimePath) || !File.Exists(runtimePath))
                {
                    return Fail(dataFolder, $"The assembly of the runtime was not found: '{runtimePath}'.");
                }

                var folder = Path.GetDirectoryName(Path.GetFullPath(runtimePath)) ?? string.Empty;

                if (_context is not null && _boot is not null &&
                    string.Equals(folder, _folder, StringComparison.OrdinalIgnoreCase))
                {
                    Log($"The runtime of '{folder}' is already running.");
                    return 0;
                }

                if (_context is not null)
                {
                    Log($"A new build was found. '{folder}' replaces '{_folder}'.");
                    UnloadCurrent();
                }

                return Load(runtimePath, folder, dataFolder, gameInstallPath);
            }
            catch (Exception ex)
            {
                UnloadCurrent();
                return Fail(dataFolder, ex.ToString());
            }
        }
    }

    /// <summary>
    /// Loads the runtime of the given build and starts it.
    /// </summary>
    private static int Load(string runtimePath, string folder, string? dataFolder, string? gameInstallPath)
    {
        var fullPath = Path.GetFullPath(runtimePath);
        var context = new RuntimeLoadContext(fullPath, $"SlothCombo.Runtime.{Path.GetFileName(folder)}");

        Assembly assembly;
        try
        {
            assembly = context.LoadFromAssemblyPath(fullPath);
        }
        catch (Exception)
        {
            context.Unload();
            throw;
        }

        var type = assembly.GetType(RuntimeTypeName, throwOnError: false)
            ?? throw new InvalidOperationException($"The type '{RuntimeTypeName}' was not found in '{runtimePath}'.");

        var bootMethod = type.GetMethod(BootMethodName, BindingFlags.Public | BindingFlags.Static)
            ?? throw new InvalidOperationException($"The entry point '{BootMethodName}' was not found in '{runtimePath}'.");
        var stopMethod = type.GetMethod(StopMethodName, BindingFlags.Public | BindingFlags.Static)
            ?? throw new InvalidOperationException($"The exit point '{StopMethodName}' was not found in '{runtimePath}'.");

        var boot = (Func<string?, string?, int>)bootMethod.CreateDelegate(typeof(Func<string?, string?, int>));
        var stop = (Action)stopMethod.CreateDelegate(typeof(Action));

        // The state is published before the start, so that a failure inside
        // the start still tears the context down through the same path.
        _context = context;
        _boot = boot;
        _stop = stop;
        _folder = folder;

        Log($"Starting the runtime of '{folder}'.");

        var result = boot(dataFolder, gameInstallPath);
        if (result != 0)
        {
            Log($"The runtime of '{folder}' refused to start ({result}).");
            UnloadCurrent();
            return result;
        }

        Log($"The runtime of '{folder}' is running.");
        return 0;
    }

    /// <summary>
    /// Stops the running runtime and unloads its context.
    /// </summary>
    private static void UnloadCurrent()
    {
        InvokeStop();

        if (_context is null)
        {
            return;
        }

        var witness = new WeakReference(_context);
        _context.Unload();
        _context = null;

        // The unload only starts the work; the context is collected on one of
        // the next passes of the collector. The forced passes take the wait
        // from "whenever" to "right away", which is what releases the files
        // of the build. A replacement is a thing the user asked for, so a
        // short pause here is cheaper than a file that stays locked.
        for (var attempt = 0; attempt < 20 && witness.IsAlive; attempt++)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Thread.Sleep(50);
        }

        Log(witness.IsAlive
            ? "The old runtime is still in memory; a later collection finishes the unload."
            : "The old runtime was unloaded and its files are free again.");
    }

    /// <summary>
    /// Lets the old runtime take its hooks and its threads down.
    /// </summary>
    private static void InvokeStop()
    {
        var stop = _stop;

        _stop = null;
        _boot = null;
        _folder = null;

        if (stop is null)
        {
            return;
        }

        try
        {
            stop();
        }
        catch (Exception ex)
        {
            Log($"The runtime reported an error while stopping: {ex}");
        }
    }

    /// <summary>
    /// Looks for a runtime that an older build left in the default context.
    /// </summary>
    /// <returns>The reason the boot has to be refused, or <c>null</c>.</returns>
    /// <remarks>
    /// A build of the runtime that was loaded before this loader existed has
    /// no way of being stopped from here, and its hooks cannot be removed.
    /// A build that is not running is harmless and is left alone.
    /// </remarks>
    private static string? FindRunningRuntimeInDefaultContext()
    {
        foreach (var assembly in AssemblyLoadContext.Default.Assemblies)
        {
            if (!string.Equals(assembly.GetName().Name, RuntimeAssemblyName, StringComparison.Ordinal))
            {
                continue;
            }

            var type = assembly.GetType(RuntimeTypeName, throwOnError: false);
            var running = type?.GetProperty("IsRunning", BindingFlags.Public | BindingFlags.Static);

            if (running is null || running.GetValue(null) is true)
            {
                return "An older build of the runtime is still running in this game session. " +
                       "Restart the game once to finish this update.";
            }
        }

        return null;
    }

    /// <summary>
    /// Reports a failure the way the program outside of the game reads it.
    /// </summary>
    private static int Fail(string? dataFolder, string message)
    {
        Log($"The boot failed: {message}");

        if (!string.IsNullOrWhiteSpace(dataFolder))
        {
            try
            {
                Directory.CreateDirectory(dataFolder);
                File.WriteAllText(Path.Combine(dataFolder, "boot-error.txt"), message);
            }
            catch (Exception)
            {
                // The log file is the only place left to report the failure.
            }
        }

        return 1;
    }

    /// <summary>
    /// Reads a string that was handed over by the boot library.
    /// </summary>
    private static string FromNative(IntPtr value) =>
        value == IntPtr.Zero ? string.Empty : Marshal.PtrToStringUni(value) ?? string.Empty;

    /// <summary>
    /// Appends a line to the log of the loader.
    /// </summary>
    /// <remarks>
    /// The log is the only trace a replacement leaves behind, and it is the
    /// one thing that makes "did the old build really go away" answerable
    /// after the fact.
    /// </remarks>
    private static void Log(string message)
    {
        try
        {
            var folder = Path.Combine(Path.GetTempPath(), "XIVSlothCombo");
            Directory.CreateDirectory(folder);
            File.AppendAllText(
                Path.Combine(folder, "loader.log"),
                $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} {message}{Environment.NewLine}");
        }
        catch (Exception)
        {
            // The log must never take the boot down with it.
        }
    }
}
