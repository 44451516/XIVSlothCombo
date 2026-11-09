using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using SlothCombo.Combos.JobHelpers;
using SlothCombo.Core;
using SlothCombo.Data;
using SlothCombo.Generated;
using SlothCombo.Runtime.Configuration;
using SlothCombo.Runtime.Gauges;
using SlothCombo.Runtime.Hooking;
using SlothCombo.Runtime.Ipc;
using SlothCombo.Runtime.Logging;
using SlothCombo.Runtime.Services;

namespace SlothCombo.Runtime.Bootstrap;

/// <summary>
/// Starts the runtime inside the game process.
/// </summary>
/// <remarks>
/// <para>
/// The legacy plugin was created by Dalamud, which provided the services, the
/// update loop and the plugin configuration. The runtime provides all of that
/// itself, so the entry point of the plugin is replaced by this class.
/// </para>
/// <para>
/// The order of the steps matters. The addresses of the game are resolved
/// before any detour is installed, because a detour patches the very bytes a
/// signature is matched against.
/// </para>
/// </remarks>
public static class SlothComboRuntime
{
    private static FrameworkTickHook? _tickHook;
    private static GameThreadDispatcher? _dispatcher;
    private static IpcServer? _ipcServer;

    /// <summary>
    /// Starts the runtime.
    /// </summary>
    /// <param name="options">The settings of the runtime.</param>
    /// <param name="error">The reason the runtime could not be started.</param>
    /// <returns><c>true</c> when the runtime is running.</returns>
    public static bool Start(RuntimeOptions options, out string error)
    {
        ArgumentNullException.ThrowIfNull(options);

        error = string.Empty;

        try
        {
            Directory.CreateDirectory(options.DataFolder);

            // The log is the first thing that exists, because everything that
            // can fail afterwards reports through it.
            var logger = new FileComboLogger(options.LogFolder);
            Service.PluginLog = logger;
            Svc.Log = logger;

            logger.Information("===== X I V S L O T H C O M B O =====");
            logger.Information($"Data folder: {options.DataFolder}");

            ResolveAddresses(logger, options);

            var scanner = new SignatureScanner();
            var hookFactory = new ReloadedHookFactory();
            var framework = new FrameworkService();

            Service.SigScanner = scanner;
            Svc.SigScanner = scanner;
            Service.GameInteropProvider = hookFactory;
            Service.Framework = framework;
            Svc.Framework = framework;

            var objectTable = new ObjectTableService();
            var condition = new ConditionService(framework);
            var targets = new TargetService();
            var party = new PartyService(objectTable);
            var clientState = new ClientStateService(objectTable);
            var addons = new AddonService();
            var chat = new ChatService();
            var gauges = new JobGaugeService();

            Service.ObjectTable = objectTable;
            Svc.Objects = objectTable;
            Service.Condition = condition;
            Svc.Condition = condition;
            Service.TargetManager = targets;
            Service.ClientState = clientState;
            Svc.ClientState = clientState;
            Service.PartyList = party;
            Svc.Party = party;
            Svc.GameGui = addons;
            Service.ChatGui = chat;
            Service.JobGauges = gauges;
            Svc.Gauges = gauges;
            Service.BuddyList = new BuddyListService();

            var data = new LuminaDataService(ResolveGameInstallPath(options));
            Service.DataManager = data;
            Svc.Data = data;

            var configStore = new ConfigStore(options.ConfigurationPath);
            Service.Interface = new PluginInterface(configStore);
            Service.Configuration = configStore.Load<PluginConfiguration>();

            Service.Address = new PluginAddressResolver();
            Service.Address.Setup(scanner);

            PresetStorage.Init();
            ConsumableChecker.Init();

            Service.ComboCache = new CustomComboCache();
            Service.IconReplacer = new IconReplacer();

            ActionWatching.Enable();
            AST.Init();

            // The tick hook goes last: from here on the runtime reacts to the
            // game on its own schedule.
            _tickHook = new FrameworkTickHook(hookFactory, framework);

            // The program outside of the game asks the runtime to do the work
            // that needs the game, so the runtime opens a pipe for it.
            _dispatcher = new GameThreadDispatcher(framework);
            _dispatcher.Start();

            _ipcServer = new IpcServer(_dispatcher);
            _ipcServer.Start();

            logger.Information("The runtime is running.");
            return true;
        }
        catch (Exception ex)
        {
            error = ex.ToString();

            try
            {
                Service.PluginLog?.Error(ex, "The runtime could not be started.");
            }
            catch (Exception)
            {
                // The log is the thing that failed, so there is nothing left
                // to report the failure with.
            }

            return false;
        }
    }

    /// <summary>
    /// Reports that the managed part of the runtime is loaded.
    /// </summary>
    /// <remarks>
    /// The boot library calls this to find out whether the runtime came up.
    /// It does nothing else, so it is safe to call at any time.
    /// </remarks>
    /// <returns>Zero, always.</returns>
    [UnmanagedCallersOnly]
    public static int Ping() => 0;

    /// <summary>
    /// Starts the runtime from the boot library.
    /// </summary>
    /// <remarks>
    /// The boot library is loaded into the game by the injector, which cannot
    /// hand over any managed arguments. The two paths are passed as plain
    /// pointers instead.
    /// </remarks>
    /// <param name="dataFolderUtf16">The folder the runtime keeps its data in.</param>
    /// <param name="gameInstallPathUtf16">The installation directory of the game.</param>
    /// <returns>Zero when the runtime is running.</returns>
    [UnmanagedCallersOnly]
    public static int Boot(IntPtr dataFolderUtf16, IntPtr gameInstallPathUtf16)
    {
        var dataFolder = Marshal.PtrToStringUni(dataFolderUtf16);
        var gameInstallPath = Marshal.PtrToStringUni(gameInstallPathUtf16);

        var options = new RuntimeOptions
        {
            DataFolder = string.IsNullOrEmpty(dataFolder) ? new RuntimeOptions().DataFolder : dataFolder,
            GameInstallPath = string.IsNullOrWhiteSpace(gameInstallPath) ? null : gameInstallPath,
        };

        if (Start(options, out var error))
        {
            return 0;
        }

        try
        {
            var path = Path.Combine(options.DataFolder, "boot-error.txt");
            Directory.CreateDirectory(options.DataFolder);
            File.WriteAllText(path, error);
        }
        catch (Exception)
        {
            // The details are a convenience, the return value is what counts.
        }

        return 1;
    }

    /// <summary>
    /// Stops the runtime and removes the detours.
    /// </summary>
    public static void Stop()
    {
        _ipcServer?.Dispose();
        _ipcServer = null;

        _dispatcher?.Stop();
        _dispatcher = null;

        _tickHook?.Dispose();
        _tickHook = null;

        ActionWatching.Dispose();
        Service.IconReplacer?.Dispose();
        Service.ComboCache?.Dispose();
    }

    /// <summary>
    /// Resolves the addresses of the game.
    /// </summary>
    /// <remarks>
    /// A few addresses of the struct library do not resolve on every build of
    /// the game. Those are reported but do not stop the runtime, because the
    /// features that need them are the only ones that break.
    /// </remarks>
    private static void ResolveAddresses(FileComboLogger logger, RuntimeOptions options)
    {
        var cachePath = Path.Combine(options.DataFolder, SignatureResolver.CacheFileName);
        if (SignatureResolver.Resolve(cachePath, out var unresolved))
        {
            logger.Information("All addresses of the game were resolved.");
            return;
        }

        logger.Warning($"{unresolved.Count} addresses of the game could not be resolved.");
        foreach (var name in unresolved)
        {
            logger.Warning($"Unresolved address: {name}");
        }
    }

    /// <summary>
    /// Works out the installation directory of the game.
    /// </summary>
    private static string ResolveGameInstallPath(RuntimeOptions options)
    {
        if (!string.IsNullOrWhiteSpace(options.GameInstallPath))
        {
            return options.GameInstallPath;
        }

        var detected = LuminaDataService.TryFindGameInstallPath();
        if (string.IsNullOrEmpty(detected))
        {
            throw new InvalidOperationException(
                "The installation directory of the game could not be found. " +
                "It is needed to read the static game data.");
        }

        return detected;
    }
}
