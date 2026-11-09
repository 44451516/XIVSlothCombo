using SlothCombo.Abstractions;
using SlothCombo.Runtime.Gauges;

namespace SlothCombo.Runtime.Services;

/// <summary>
/// The shortcut to the services of the runtime.
/// </summary>
/// <remarks>
/// This is the replacement for the ECommons service shortcut. The combo code
/// was written against it, so keeping the same shape keeps the rotation files
/// readable while the runtime owns the actual services.
/// </remarks>
internal static class Svc
{
    /// <summary>Gets or sets the log of the runtime.</summary>
    public static IComboLogger Log { get; set; } = null!;

    /// <summary>Gets or sets the static game data.</summary>
    public static IDataManager Data { get; set; } = null!;

    /// <summary>Gets or sets the condition flags of the player.</summary>
    public static IConditionService Condition { get; set; } = null!;

    /// <summary>Gets or sets the in-game addons.</summary>
    public static IAddonService GameGui { get; set; } = null!;

    /// <summary>Gets or sets the object table of the game.</summary>
    public static IObjectTable Objects { get; set; } = null!;

    /// <summary>Gets or sets the client state of the game.</summary>
    public static IClientStateService ClientState { get; set; } = null!;

    /// <summary>Gets or sets the update loop of the game.</summary>
    public static IFramework Framework { get; set; } = null!;

    /// <summary>Gets or sets the party state of the game.</summary>
    public static IPartyService Party { get; set; } = null!;

    /// <summary>Gets or sets the signature scanner of the runtime.</summary>
    public static ISignatureScanner SigScanner { get; set; } = null!;

    /// <summary>Gets or sets the job gauges of the player.</summary>
    public static JobGaugeService Gauges { get; set; } = null!;
}
