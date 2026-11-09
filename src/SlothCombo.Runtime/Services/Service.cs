using SlothCombo.Abstractions;
using SlothCombo.Core;
using SlothCombo.Data;
using SlothCombo.Runtime.Gauges;

namespace SlothCombo.Runtime.Services;

/// <summary>
/// The services the combo logic reaches for.
/// </summary>
/// <remarks>
/// This is the replacement for the service locator of the legacy plugin. The
/// plugin used the Dalamud service locator and the ECommons shortcut at the
/// same time; both are replaced by this single class, which is filled in by
/// the runtime when it starts up.
/// </remarks>
internal static class Service
{
    /// <summary>Gets or sets the object table of the game.</summary>
    public static IObjectTable ObjectTable { get; set; } = null!;

    /// <summary>Gets or sets the condition flags of the player.</summary>
    public static IConditionService Condition { get; set; } = null!;

    /// <summary>Gets or sets the chat output of the game.</summary>
    public static IChatService ChatGui { get; set; } = null!;

    /// <summary>Gets or sets the client state of the game.</summary>
    public static IClientStateService ClientState { get; set; } = null!;

    /// <summary>Gets or sets the log of the runtime.</summary>
    public static IComboLogger PluginLog { get; set; } = null!;

    /// <summary>Gets or sets the target information of the game.</summary>
    public static ITargetService TargetManager { get; set; } = null!;

    /// <summary>Gets or sets the hook factory of the runtime.</summary>
    public static IHookFactory GameInteropProvider { get; set; } = null!;

    /// <summary>Gets or sets the update loop of the game.</summary>
    public static IFramework Framework { get; set; } = null!;

    /// <summary>Gets or sets the signature scanner of the runtime.</summary>
    public static ISignatureScanner SigScanner { get; set; } = null!;

    /// <summary>Gets or sets the job gauges of the player.</summary>
    public static JobGaugeService JobGauges { get; set; } = null!;

    /// <summary>Gets or sets the static game data.</summary>
    public static IDataManager DataManager { get; set; } = null!;

    /// <summary>Gets or sets the pets of the player.</summary>
    public static IBuddyList BuddyList { get; set; } = null!;

    /// <summary>Gets or sets the party state of the game.</summary>
    public static IPartyService PartyList { get; set; } = null!;

    /// <summary>Gets or sets the interface to the outside world.</summary>
    public static IPluginInterface Interface { get; set; } = null!;

    /// <summary>Gets or sets the configuration of the plugin.</summary>
    public static PluginConfiguration Configuration { get; set; } = null!;

    /// <summary>Gets or sets the address resolver of the runtime.</summary>
    public static PluginAddressResolver Address { get; set; } = null!;

    /// <summary>Gets or sets the cache of the combo logic.</summary>
    public static CustomComboCache ComboCache { get; set; } = null!;

    /// <summary>Gets or sets the icon replacer of the runtime.</summary>
    public static IconReplacer IconReplacer { get; set; } = null!;
}
