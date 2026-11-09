using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading;

namespace SlothCombo.Boot;

/// <summary>
/// Starts the managed runtime inside the game process.
/// </summary>
/// <remarks>
/// <para>
/// The game is a plain native program, so there is no runtime in it that
/// could host the managed part of this project. This library is injected into
/// the game, and it loads the runtime itself before it hands control over to
/// the managed assembly.
/// </para>
/// <para>
/// The library is built ahead of time, so it can be loaded into a process
/// that has nothing but the game in it.
/// </para>
/// <para>
/// The entry point is an export rather than a module initializer, because the
/// runtime of a library that is built ahead of time only starts when one of
/// its functions is called.
/// </para>
/// </remarks>
internal static unsafe class BootEntry
{
    /// <summary>The delegate that loads an assembly and returns a function of it.</summary>
    private const int LoadAssemblyAndGetFunctionPointer = 5;

    /// <summary>The name of the export the injector calls.</summary>
    public const string ExportName = "SlothComboBoot";

    /// <summary>The name of the export the loader of the system calls.</summary>
    public const string Rundll32ExportName = "SlothComboBootRundll32";

    /// <summary>
    /// Starts the boot.
    /// </summary>
    /// <remarks>
    /// The injector calls this as the entry point of a thread, so the shape of
    /// the function is the one the system expects of such an entry point.
    /// </remarks>
    /// <param name="reserved">Not used.</param>
    /// <returns>Zero when the runtime was started.</returns>
    [UnmanagedCallersOnly(EntryPoint = ExportName)]
    public static int SlothComboBoot(IntPtr reserved) => Run();

    /// <summary>
    /// Starts the boot for the loader of the system.
    /// </summary>
    /// <remarks>
    /// The loader of the system calls the exports of a library with a fixed
    /// shape. This export has that shape, which makes it possible to start
    /// the runtime without an injector.
    /// </remarks>
    /// <param name="window">Not used.</param>
    /// <param name="instance">Not used.</param>
    /// <param name="commandLine">Not used.</param>
    /// <param name="showCommand">Not used.</param>
    /// <returns>Zero when the runtime was started.</returns>
    [UnmanagedCallersOnly(EntryPoint = Rundll32ExportName)]
    public static int SlothComboBootRundll32(IntPtr window, IntPtr instance, IntPtr commandLine, int showCommand) => Run();

    /// <summary>
    /// Loads the runtime and starts the managed part.
    /// </summary>
    private static int Run()
    {
        BootConfig? config = null;

        try
        {
            config = BootConfig.Load();
            if (config is null)
            {
                WriteStatus(null, "failed", $"The settings file '{BootConfig.FilePath}' is missing.");
                return 1;
            }

            return Start(config);
        }
        catch (Exception ex)
        {
            // The entry point is called from native code, so an exception
            // that leaves it would take the game down with it.
            WriteStatus(config, "failed", ex.ToString());
            return 1;
        }
    }

    /// <summary>
    /// Loads the runtime and starts the managed part.
    /// </summary>
    private static int Start(BootConfig config)
    {
        WriteStatus(config, "starting", "The runtime is being loaded.");

        var hostFxr = LoadHostFxr(config.HostFxrPath);
        if (hostFxr == IntPtr.Zero)
        {
            throw new InvalidOperationException(
                $"The library '{config.HostFxrPath}' could not be loaded.");
        }

        var runtimeConfig = Utf16(config.RuntimeConfigPath);
        var assembly = Utf16(config.AssemblyPath);
        var typeName = Utf16(config.TypeName);
        var methodName = Utf16(config.MethodName);

        try
        {
            var initialize = (delegate* unmanaged<char*, void*, IntPtr*, int>)
                NativeLibrary.GetExport(hostFxr, "hostfxr_initialize_for_runtime_config");

            var getDelegate = (delegate* unmanaged<IntPtr, int, IntPtr*, int>)
                NativeLibrary.GetExport(hostFxr, "hostfxr_get_runtime_delegate");

            IntPtr context;
            var result = initialize(runtimeConfig, null, &context);
            if (result is not (0 or 1))
            {
                throw new InvalidOperationException(
                    $"The runtime could not be prepared (0x{result:X8}).");
            }

            IntPtr loader;
            result = getDelegate(context, LoadAssemblyAndGetFunctionPointer, &loader);
            if (result < 0)
            {
                throw new InvalidOperationException(
                    $"The loader of the runtime could not be read (0x{result:X8}).");
            }

            // The loader of the runtime takes the path of the assembly, the
            // type that holds the entry point, the name of the entry point,
            // the kind of the entry point, a place that is not used and the
            // place the pointer of the entry point is written to.
            var load = (delegate* unmanaged<char*, char*, char*, char*, IntPtr, IntPtr*, int>)loader;

            IntPtr entryPoint;
            result = load(assembly, typeName, methodName, (char*)-1, IntPtr.Zero, &entryPoint);
            if (result < 0 || entryPoint == IntPtr.Zero)
            {
                throw new InvalidOperationException(
                    $"The managed runtime could not be loaded (0x{result:X8}).");
            }

            var entry = (delegate* unmanaged<IntPtr, IntPtr, int>)entryPoint;

            var dataFolder = Utf16(config.DataFolder);
            var gameInstallPath = Utf16(config.GameInstallPath);

            int exitCode;
            try
            {
                exitCode = entry((IntPtr)dataFolder, (IntPtr)gameInstallPath);
            }
            finally
            {
                Free(dataFolder);
                Free(gameInstallPath);
            }

            if (exitCode != 0)
            {
                WriteStatus(config, "failed", $"The managed runtime refused to start ({exitCode}).");
                return 1;
            }

            WriteStatus(config, "running", "The runtime is running.");
            return 0;
        }
        finally
        {
            Free(runtimeConfig);
            Free(assembly);
            Free(typeName);
            Free(methodName);
        }
    }

    /// <summary>
    /// Loads the library that hosts the runtime.
    /// </summary>
    /// <remarks>
    /// The library is taken from the place the host program installed it in.
    /// It works out where the runtime lives from its own place, so the game
    /// uses the very same runtime as the host program.
    /// </remarks>
    private static IntPtr LoadHostFxr(string path)
    {
        if (string.IsNullOrEmpty(path) || !File.Exists(path))
        {
            return IntPtr.Zero;
        }

        return NativeLibrary.Load(path);
    }

    /// <summary>
    /// Copies a string into unmanaged memory.
    /// </summary>
    private static char* Utf16(string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return null;
        }

        return (char*)Marshal.StringToHGlobalUni(value);
    }

    /// <summary>
    /// Releases a string that was copied into unmanaged memory.
    /// </summary>
    private static void Free(char* value)
    {
        if (value is not null)
        {
            Marshal.FreeHGlobal((IntPtr)value);
        }
    }

    /// <summary>
    /// Reports the progress of the boot.
    /// </summary>
    /// <remarks>
    /// The report is written to the place the host program asked for. When
    /// that place cannot be written to, the report goes to the temporary
    /// folder of the machine instead, so that a failure is never silent.
    /// </remarks>
    private static void WriteStatus(BootConfig? config, string state, string message)
    {
        var path = config is null || string.IsNullOrEmpty(config.StatusPath)
            ? Path.Combine(BootConfig.Folder, "boot-status.json")
            : config.StatusPath;

        if (TryWriteStatus(path, state, message))
        {
            return;
        }

        TryWriteStatus(Path.Combine(Path.GetTempPath(), "SlothCombo.Boot.status.json"), state, message);
    }

    /// <summary>
    /// Writes the state of the boot to the given file.
    /// </summary>
    private static bool TryWriteStatus(string path, string state, string message)
    {
        try
        {
            var directory = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var status = new BootStatus
            {
                State = state,
                Message = message,
                Timestamp = DateTime.UtcNow,
            };

            File.WriteAllText(path, JsonSerializer.Serialize(status, BootJsonContext.Default.BootStatus));
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
