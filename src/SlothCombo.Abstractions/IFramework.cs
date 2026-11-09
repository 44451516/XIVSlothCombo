using System;

namespace SlothCombo.Abstractions;

/// <summary>
/// The callback the framework raises on every update of the game client.
/// </summary>
/// <param name="framework">The framework that raised the event.</param>
public delegate void FrameworkUpdateDelegate(IFramework framework);

/// <summary>
/// The update loop of the game client.
/// </summary>
/// <remarks>
/// The legacy plugin received its tick from the Dalamud framework. The
/// standalone runtime raises the same event from its own hook on the game
/// loop, so the combo logic does not need to know where the tick comes from.
/// </remarks>
public interface IFramework
{
    /// <summary>Raised on every update of the game client.</summary>
    event FrameworkUpdateDelegate? Update;

    /// <summary>Schedules the given callback for the next update.</summary>
    /// <param name="action">The callback to run.</param>
    void RunOnTick(FrameworkUpdateDelegate action);
}
