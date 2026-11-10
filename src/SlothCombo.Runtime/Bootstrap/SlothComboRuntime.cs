using System;
using System.Collections.Generic;
using System.IO;
using SlothCombo.Abstractions;
using SlothCombo.Combos.JobHelpers;
using SlothCombo.Core;
using SlothCombo.Data;
using SlothCombo.Runtime.Configuration;
using SlothCombo.Runtime.Gauges;
using SlothCombo.Runtime.Hooking;
using SlothCombo.Runtime.Ipc;
using SlothCombo.Runtime.Logging;
using SlothCombo.Runtime.Services;

namespace SlothCombo.Runtime.Bootstrap;

/// <summary>
/// Starts and stops the runtime inside the game process.
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
/// <para>
/// The runtime is loaded into a context that can be unloaded, so a newer
/// build can take its place without restarting the game. That only works if
/// the stop leaves nothing behind, which is why it undoes every single thing
/// the start did.
/// </para>
/// </remarks>
public static class SlothComboRuntime
{
    private static FrameworkTickHook? _tickHook;
    private static GameThreadDispatcher? _dispatcher;
    private static IpcServer? _ipcServer;
    private static FrameworkService? _framework;
    private static ConditionService? _condition;
    private static int _stoppedOnGameThread;
    private static bool _running;

    /// <summary>Gets a value indicating whether the runtime is running.</summary>
    public static bool IsRunning => _running;

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

        if (_running)
        {
            return true;
        }

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

            var scanner = new SignatureScanner();
            ResolveAddresses(logger, options, scanner);

            var hookFactory = new ReloadedHookFactory();
            var framework = new FrameworkService();
            _framework = framework;

            Service.SigScanner = scanner;
            Svc.SigScanner = scanner;
            Service.GameInteropProvider = hookFactory;
            Service.Framework = framework;
            Svc.Framework = framework;

            var objectTable = new ObjectTableService();
            var condition = new ConditionService(framework);
            _condition = condition;
            var targets = new TargetService();
            var party = new PartyService(objectTable);
            var clientState = new ClientStateService(objectTable);
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
            Service.ChatGui = chat;
            Service.JobGauges = gauges;
            Svc.Gauges = gauges;
            Service.BuddyList = new BuddyListService();

            var data = new GameDataService(GameDataService.ResolveRootFolder(), clientState.ClientLanguage);
            Service.DataManager = data;
            Svc.Data = data;

            var configStore = new ConfigStore(options.ConfigurationPath);
            Service.Interface = new PluginInterface(configStore);
            Service.Configuration = configStore.Load<PluginConfiguration>();

            Service.Address = new PluginAddressResolver();
            Service.Address.Setup(scanner);

            PresetStorage.Init();

            Service.ComboCache = new CustomComboCache();
            Service.IconReplacer = new IconReplacer();

            ActionWatching.Enable();
            AST.Init();

            // The tick hook goes last: from here on the runtime reacts to the
            // game on its own schedule.
            Interlocked.Exchange(ref _stoppedOnGameThread, 0);
            _tickHook = new FrameworkTickHook(hookFactory, framework);

            // The program outside of the game asks the runtime to do the work
            // that needs the game, so the runtime opens a pipe for it.
            _dispatcher = new GameThreadDispatcher(framework);
            _dispatcher.Start();

            _ipcServer = new IpcServer(_dispatcher);
            _ipcServer.Start();

            _running = true;

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

            // A start that fails halfway leaves hooks and a pipe behind; the
            // same teardown the loader uses takes them down again.
            StopManaged();

            return false;
        }
    }

    /// <summary>
    /// Stops the runtime and removes everything it installed in the game.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This runs right before a newer build takes the place of this one, so
    /// it has to leave nothing behind: a hook that still points into this
    /// assembly, a pipe that still holds the name, or a log file that is
    /// still open would break the runtime that follows.
    /// </para>
    /// <para>
    /// The steps are independent, so one failing does not stop the rest, and
    /// the method can be called at any time and more than once.
    /// </para>
    /// </remarks>
    public static void StopManaged()
    {
        var ipc = _ipcServer;
        _ipcServer = null;

        if (ipc is not null)
        {
            // The pipe goes first: a request that is being answered right now
            // still needs the game thread, and the game thread is about to be
            // taken by the shutdown.
            try
            {
                ipc.StopAsync(TimeSpan.FromSeconds(5)).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Log(ex, "The pipe of the interface could not be closed.");
            }
        }

        var dispatcher = _dispatcher;

        if (dispatcher is not null && _tickHook is not null)
        {
            try
            {
                // The game thread is the only thread that runs the detours,
                // so the detours are taken down from there.
                if (!dispatcher.RunAsync(StopOnGameThread).Wait(TimeSpan.FromSeconds(30)))
                {
                    Service.PluginLog?.Warning(
                        "The game thread did not take the shutdown in time; it is done without it.");
                    StopOnGameThread();
                }
            }
            catch (Exception ex)
            {
                Log(ex, "The game thread could not run the shutdown.");
                StopOnGameThread();
            }
        }
        else
        {
            // The start never reached the point where the game thread ran
            // this assembly, so there is nothing that needs to wait for it.
            StopOnGameThread();
        }

        _dispatcher = null;
        _tickHook = null;

        DisposeServices();

        _running = false;
    }

    /// <summary>
    /// Undoes everything the start did on the game thread.
    /// </summary>
    private static void StopOnGameThread()
    {
        if (Interlocked.Exchange(ref _stoppedOnGameThread, 1) != 0)
        {
            return;
        }

        // The update event is stopped before anything that listens to it goes
        // away, so no listener is called while it is being torn down.
        Try("The update loop could not be stopped.", () => _framework?.Suspend());

        Try("The condition service could not be stopped.", () => _condition?.Dispose());

        Try("The card helper could not be stopped.", AST.Dispose);

        Try("The action watching could not be stopped.", ActionWatching.Dispose);

        Try("The icon replacer could not be stopped.", () => Service.IconReplacer?.Dispose());

        Try("The combo cache could not be disposed.", () => Service.ComboCache?.Dispose());

        // The detour of the update loop is the one that is running this very
        // call, so it goes last. Taking it down only restores the original
        // bytes of the game function; the trampoline stays alive, which is
        // what the detour returns through on its way out.
        Try("The update loop hook could not be removed.", () => _tickHook?.Dispose());

        Try("The dispatcher could not be stopped.", () => _dispatcher?.Stop());
    }

    /// <summary>
    /// Lets go of the services that hold files or static state.
    /// </summary>
    private static void DisposeServices()
    {
        _framework = null;
        _condition = null;

        if (Service.DataManager is IDisposable data)
        {
            Try("The static game data could not be closed.", data.Dispose);
        }

        Try("The names of the statuses could not be released.", StatusSheetLookup.Unregister);

        if (Service.PluginLog is IDisposable log)
        {
            Try("The log could not be closed.", log.Dispose);
        }

        Service.Reset();
        Svc.Reset();
        SignatureScanner.Reset();
    }

    /// <summary>
    /// Runs a step of the shutdown and reports a failure without stopping the
    /// remaining steps.
    /// </summary>
    private static void Try(string message, Action action)
    {
        try
        {
            action();
        }
        catch (Exception ex)
        {
            Log(ex, message);
        }
    }

    /// <summary>
    /// Reports a failure of the shutdown.
    /// </summary>
    private static void Log(Exception ex, string message)
    {
        try
        {
            Service.PluginLog?.Error(ex, message);
        }
        catch (Exception)
        {
            // The log is already gone; there is nothing left to report with.
        }
    }

    /// <summary>
    /// Starts the runtime from the loader.
    /// </summary>
    /// <remarks>
    /// The loader of the program keeps the runtime in a context that can be
    /// unloaded, and reaches this entry point through reflection. The paths
    /// arrive as plain strings because the caller has no typed reference to
    /// this assembly.
    /// </remarks>
    /// <param name="dataFolder">The folder the runtime keeps its data in.</param>
    /// <param name="gameInstallPath">The installation directory of the game.</param>
    /// <returns>Zero when the runtime is running.</returns>
    public static int BootManaged(string? dataFolder, string? gameInstallPath)
    {
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
    /// Resolves the addresses of the game.
    /// </summary>
    /// <remarks>
    /// A few addresses of the game do not resolve on every build of it. Those
    /// are reported but do not stop the runtime, because the features that need
    /// them are the only ones that break.
    /// </remarks>
    /// <param name="logger">The logger the result is reported to.</param>
    /// <param name="options">The options of the runtime.</param>
    /// <param name="scanner">The scanner that looks for the patterns.</param>
    private static void ResolveAddresses(FileComboLogger logger, RuntimeOptions options, ISignatureScanner scanner)
    {
        var cachePath = Path.Combine(options.DataFolder, SignatureResolver.CacheFileName);
        if (SignatureResolver.Resolve(cachePath, scanner, out var unresolved))
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
}
