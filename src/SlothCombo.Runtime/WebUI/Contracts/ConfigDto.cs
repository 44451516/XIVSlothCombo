using System.Collections.Generic;
using Newtonsoft.Json;

namespace SlothCombo.Runtime.WebUI.Contracts;

/// <summary> Snapshot of the plugin's top level configuration. </summary>
public sealed class ConfigDto
{
    /// <summary> Prints every used action to the chat box. </summary>
    [JsonProperty("enabledOutputLog")]
    public bool EnabledOutputLog { get; set; }

    /// <summary> Prints state changes of features to the chat box. </summary>
    [JsonProperty("setOutChat")]
    public bool SetOutChat { get; set; }

    /// <summary> Hides features conflicting with an enabled one. </summary>
    [JsonProperty("hideConflictedCombos")]
    public bool HideConflictedCombos { get; set; }

    /// <summary> Hides the options of disabled features. </summary>
    [JsonProperty("hideChildren")]
    public bool HideChildren { get; set; }

    /// <summary> Offset applied to the melee range check. </summary>
    [JsonProperty("meleeOffset")]
    public double MeleeOffset { get; set; }

    /// <summary> Colour used for the target highlight. </summary>
    [JsonProperty("targetHighlightColor")]
    public ColorDto TargetHighlightColor { get; set; } = new();

    /// <summary> Hides the login message. </summary>
    [JsonProperty("hideMessageOfTheDay")]
    public bool HideMessageOfTheDay { get; set; }

    /// <summary> Whether the special event naming is active. </summary>
    [JsonProperty("specialEvent")]
    public bool SpecialEvent { get; set; }

    /// <summary> Delay applied to movement. </summary>
    [JsonProperty("movementLeeway")]
    public float MovementLeeway { get; set; }

    /// <summary> Enables the performance mode. </summary>
    [JsonProperty("performanceMode")]
    public bool PerformanceMode { get; set; }

    /// <summary> Whether the recommended settings page was already shown. </summary>
    [JsonProperty("recommendedSettingsViewed")]
    public bool RecommendedSettingsViewed { get; set; }

    /// <summary> Blue mage spells the player has learnt. </summary>
    [JsonProperty("activeBluSpells")]
    public List<uint> ActiveBluSpells { get; set; } = new();
}

/// <summary> RGBA colour used by the target highlight. </summary>
public sealed class ColorDto
{
    /// <summary> Red channel. </summary>
    [JsonProperty("r")]
    public float R { get; set; }

    /// <summary> Green channel. </summary>
    [JsonProperty("g")]
    public float G { get; set; }

    /// <summary> Blue channel. </summary>
    [JsonProperty("b")]
    public float B { get; set; }

    /// <summary> Alpha channel. </summary>
    [JsonProperty("a")]
    public float A { get; set; }
}
