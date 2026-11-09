using System;
using System.Collections.Generic;
using SlothCombo.Abstractions;

namespace SlothCombo.Runtime.Services;

/// <summary>
/// The update loop of the game, driven by the runtime.
/// </summary>
/// <remarks>
/// The legacy plugin received its tick from the Dalamud framework. The runtime
/// raises the same event from its own hook on the game loop, so the combo
/// logic does not have to know where the tick comes from.
/// </remarks>
internal sealed class FrameworkService : IFramework
{
    private readonly Queue<FrameworkUpdateDelegate> _tickActions = [];
    private readonly object _lock = new();
    private volatile bool _suspended;

    /// <inheritdoc/>
    public event FrameworkUpdateDelegate? Update;

    /// <summary>
    /// Stops raising the update event.
    /// </summary>
    /// <remarks>
    /// The runtime is about to remove the hooks and let go of every service,
    /// so the frame that is running right now must not reach any of them.
    /// </remarks>
    public void Suspend()
    {
        _suspended = true;
        Update = null;

        lock (_lock)
        {
            _tickActions.Clear();
        }
    }

    /// <inheritdoc/>
    public void RunOnTick(FrameworkUpdateDelegate action)
    {
        ArgumentNullException.ThrowIfNull(action);

        lock (_lock)
        {
            _tickActions.Enqueue(action);
        }
    }

    /// <summary>
    /// Raises the update event.
    /// </summary>
    /// <remarks>
    /// This is called from the hook on the game loop, so it runs on the main
    /// thread of the game. A handler that throws is logged and skipped, so a
    /// broken combo cannot take the game down with it.
    /// </remarks>
    public void RaiseUpdate()
    {
        if (_suspended)
        {
            return;
        }

        var handlers = Update;
        if (handlers is not null)
        {
            foreach (var handler in handlers.GetInvocationList())
            {
                try
                {
                    ((FrameworkUpdateDelegate)handler)(this);
                }
                catch (Exception ex)
                {
                    Service.PluginLog?.Error(ex, "An update handler of the runtime failed.");
                }
            }
        }

        FrameworkUpdateDelegate[] pending;
        lock (_lock)
        {
            if (_tickActions.Count == 0)
            {
                return;
            }

            pending = _tickActions.ToArray();
            _tickActions.Clear();
        }

        foreach (var action in pending)
        {
            try
            {
                action(this);
            }
            catch (Exception ex)
            {
                Service.PluginLog?.Error(ex, "A tick action of the runtime failed.");
            }
        }
    }
}
