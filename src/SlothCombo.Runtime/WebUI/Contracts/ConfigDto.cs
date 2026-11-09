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

    /// <summary> Automatically refines materia. </summary>
    [JsonProperty("autoRefine")]
    public bool AutoRefine { get; set; }

    /// <summary> Restricts automatic refining to Alexander. </summary>
    [JsonProperty("onlyRefineAlexander")]
    public bool OnlyRefineAlexander { get; set; }

    /// <summary> Automatically eats the configured food. </summary>
    [JsonProperty("autoFood")]
    public bool AutoFood { get; set; }

    /// <summary> Item id of the food eaten automatically. </summary>
    [JsonProperty("autoFoodId")]
    public uint AutoFoodId { get; set; }

    /// <summary> Whether the automatically eaten food must be the high quality one. </summary>
    [JsonProperty("requiredFoodHq")]
    public bool RequiredFoodHq { get; set; }

    /// <summary> Automatically uses the refining potion. </summary>
    [JsonProperty("autoRefinePotion")]
    public bool AutoRefinePotion { get; set; }

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

    /// <summary> Foods that can be selected for automatic consumption. </summary>
    [JsonProperty("foods")]
    public List<FoodDto> Foods { get; set; } = new();
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

/// <summary> A food item that can be consumed automatically. </summary>
public sealed class FoodDto
{
    /// <summary> Item id. </summary>
    [JsonProperty("id")]
    public uint Id { get; set; }

    /// <summary> Item name. </summary>
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary> Whether the player currently carries the regular quality item. </summary>
    [JsonProperty("inInventory")]
    public bool InInventory { get; set; }

    /// <summary> Whether the player currently carries the high quality item. </summary>
    [JsonProperty("inInventoryHq")]
    public bool InInventoryHq { get; set; }
}
