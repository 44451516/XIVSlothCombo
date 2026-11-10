using System;
using System.Runtime.InteropServices;
using SlothCombo.Interop.UI;
using SlothCombo.Abstractions;
using SlothCombo.Runtime.Services;

namespace SlothCombo.Runtime.Hooking;

/// <summary>
/// Drives the update loop of the runtime from the game loop.
/// </summary>
/// <remarks>
/// <para>
/// The legacy plugin received its tick from the Dalamud framework. The game
/// keeps its own loop in the virtual table of the framework, so the runtime
/// hooks that entry instead of depending on a host that no longer exists.
/// </para>
/// <para>
/// The detour runs on the main thread of the game, which is the thread every
/// service of the runtime expects to be called from.
/// </para>
/// </remarks>
internal sealed unsafe class FrameworkTickHook : IDisposable
{
    /// <summary>
    /// The shape of the tick of the game loop.
    /// </summary>
    /// <remarks>
    /// The game is built for the Windows x64 calling convention, which is what
    /// the standard convention of the platform resolves to.
    /// </remarks>
    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    private delegate bool TickDelegate(Framework* framework);

    private readonly INativeHook<TickDelegate> _hook;
    private readonly FrameworkService _framework;

    /// <summary>Installs the hook on the tick of the game loop.</summary>
    /// <param name="hookFactory">The factory that creates the detour.</param>
    /// <param name="framework">The update loop of the runtime.</param>
    public FrameworkTickHook(IHookFactory hookFactory, FrameworkService framework)
    {
        ArgumentNullException.ThrowIfNull(hookFactory);
        ArgumentNullException.ThrowIfNull(framework);

        _framework = framework;

        var table = Framework.StaticVirtualTablePointer;
        if (table is null)
        {
            throw new InvalidOperationException("The virtual table of the game loop could not be read.");
        }

        _hook = hookFactory.HookFromAddress<TickDelegate>((IntPtr)table->Tick, TickDetour);
        _hook.Enable();
    }

    /// <inheritdoc/>
    public void Dispose() => _hook.Dispose();

    /// <summary>
    /// Raises the update event and lets the game continue.
    /// </summary>
    private bool TickDetour(Framework* framework)
    {
        try
        {
            _framework.RaiseUpdate();
        }
        catch (Exception ex)
        {
            DetourGuard.Report(nameof(TickDetour), ex);
        }

        return _hook.Original(framework);
    }
}
