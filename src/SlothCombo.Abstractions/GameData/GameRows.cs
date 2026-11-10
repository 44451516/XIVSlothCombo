using System.Text.Json.Serialization;

namespace SlothCombo.Abstractions.GameData;

/// <summary>The base of every row of a table of the game.</summary>
public abstract class GameRow
{
    /// <summary>Gets or sets the number of the row.</summary>
    [JsonPropertyName("id")]
    public uint RowId { get; set; }
}

/// <summary>A row of the table of the actions.</summary>
/// <remarks>
/// The row carries only the fields the program has a use for. The rest of
/// the table is left out, so the data that is carried along stays small.
/// </remarks>
public sealed class ActionRow : GameRow
{
    /// <summary>Gets or sets the name of the action.</summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>Gets or sets the number of the picture of the action.</summary>
    [JsonPropertyName("icon")]
    public ushort Icon { get; set; }

    /// <summary>Gets or sets the number of the row of the kind of the action.</summary>
    [JsonPropertyName("actionCategory")]
    public uint ActionCategory { get; set; }

    /// <summary>Gets or sets the number of the row of the class of the action.</summary>
    [JsonPropertyName("classJob")]
    public uint ClassJob { get; set; }

    /// <summary>Gets or sets the level of the class the action belongs to.</summary>
    [JsonPropertyName("classJobLevel")]
    public byte ClassJobLevel { get; set; }

    /// <summary>Gets or sets the distance the action reaches.</summary>
    [JsonPropertyName("range")]
    public sbyte Range { get; set; }

    /// <summary>Gets or sets the distance the effect of the action reaches.</summary>
    [JsonPropertyName("effectRange")]
    public byte EffectRange { get; set; }

    /// <summary>Gets or sets the time the cast takes, in tenths of a second.</summary>
    [JsonPropertyName("cast100ms")]
    public ushort Cast100ms { get; set; }

    /// <summary>Gets or sets the time the action takes to be used again.</summary>
    [JsonPropertyName("recast100ms")]
    public ushort Recast100ms { get; set; }

    /// <summary>Gets or sets the group of the cooldown of the action.</summary>
    [JsonPropertyName("cooldownGroup")]
    public byte CooldownGroup { get; set; }

    /// <summary>Gets or sets the number of the charges of the action.</summary>
    [JsonPropertyName("maxCharges")]
    public byte MaxCharges { get; set; }

    /// <summary>Gets or sets the number of the task that unlocks the action.</summary>
    [JsonPropertyName("unlockLink")]
    public uint UnlockLink { get; set; }

    /// <summary>Gets or sets the category of the class the action belongs to.</summary>
    [JsonPropertyName("classJobCategory")]
    public uint ClassJobCategory { get; set; }

    /// <summary>Gets or sets a value indicating whether the action is of a fight against other players.</summary>
    [JsonPropertyName("isPvP")]
    public bool IsPvP { get; set; }

    /// <summary>Gets or sets a value indicating whether the action belongs to a player.</summary>
    [JsonPropertyName("isPlayerAction")]
    public bool IsPlayerAction { get; set; }

    /// <summary>Gets or sets a value indicating whether the action can be used on a party member.</summary>
    [JsonPropertyName("canTargetParty")]
    public bool CanTargetParty { get; set; }
}

/// <summary>A row of the table of the kinds of the actions.</summary>
public sealed class ActionCategoryRow : GameRow
{
    /// <summary>Gets or sets the name of the kind of the action.</summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}

/// <summary>A row of the table of the status effects.</summary>
public sealed class StatusRow : GameRow
{
    /// <summary>Gets or sets the name of the status.</summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>Gets or sets the number of the picture of the status.</summary>
    [JsonPropertyName("icon")]
    public uint Icon { get; set; }

    /// <summary>Gets or sets a value indicating whether the status can be dispelled.</summary>
    [JsonPropertyName("canDispel")]
    public bool CanDispel { get; set; }
}

/// <summary>A row of the table of the items.</summary>
public sealed class ItemRow : GameRow
{
    /// <summary>Gets or sets the name of the item.</summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>Gets or sets the number of the picture of the item.</summary>
    [JsonPropertyName("icon")]
    public ushort Icon { get; set; }

    /// <summary>Gets or sets the number of the row of the action of the item.</summary>
    [JsonPropertyName("itemAction")]
    public uint ItemAction { get; set; }

    /// <summary>Gets or sets the category of the item in the interface.</summary>
    [JsonPropertyName("itemUICategory")]
    public uint ItemUICategory { get; set; }
}

/// <summary>A row of the table of the actions of the items.</summary>
public sealed class ItemActionRow : GameRow
{
    /// <summary>Gets or sets the number of the action of the item.</summary>
    [JsonPropertyName("action")]
    public uint Action { get; set; }

    /// <summary>Gets or sets the values that belong to the action of the item.</summary>
    [JsonPropertyName("data")]
    public ushort[] Data { get; set; } = [];

    /// <summary>Gets or sets the values that belong to the action of the better item.</summary>
    [JsonPropertyName("dataHQ")]
    public ushort[] DataHq { get; set; } = [];
}

/// <summary>A row of the table of the classes.</summary>
public sealed class ClassJobRow : GameRow
{
    /// <summary>Gets or sets the name of the class.</summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>Gets or sets the short name of the class.</summary>
    [JsonPropertyName("abbreviation")]
    public string Abbreviation { get; set; } = string.Empty;

    /// <summary>Gets or sets the name of the class in English.</summary>
    [JsonPropertyName("nameEnglish")]
    public string NameEnglish { get; set; } = string.Empty;

    /// <summary>Gets or sets the number of the category of the class.</summary>
    [JsonPropertyName("classJobCategory")]
    public uint ClassJobCategory { get; set; }

    /// <summary>Gets or sets the role of the class.</summary>
    [JsonPropertyName("role")]
    public byte Role { get; set; }
}

/// <summary>A row of the table of the kinds of the classes.</summary>
public sealed class ClassJobCategoryRow : GameRow
{
    /// <summary>Gets or sets the name of the kind of the class.</summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}

/// <summary>A row of the table of the traits.</summary>
public sealed class TraitRow : GameRow
{
    /// <summary>Gets or sets the name of the trait.</summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>Gets or sets the number of the picture of the trait.</summary>
    [JsonPropertyName("icon")]
    public uint Icon { get; set; }

    /// <summary>Gets or sets the level the trait is learnt at.</summary>
    [JsonPropertyName("level")]
    public byte Level { get; set; }

    /// <summary>Gets or sets the category of the class the trait belongs to.</summary>
    [JsonPropertyName("classJobCategory")]
    public uint ClassJobCategory { get; set; }
}

/// <summary>A row of the table of the tasks.</summary>
public sealed class QuestRow : GameRow
{
    /// <summary>Gets or sets the name of the task.</summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}

/// <summary>A row of the table of the areas.</summary>
public sealed class TerritoryTypeRow : GameRow
{
    /// <summary>Gets or sets the number of the row of the name of the area.</summary>
    [JsonPropertyName("placeName")]
    public uint PlaceName { get; set; }
}

/// <summary>A row of the table of the names of the places.</summary>
public sealed class PlaceNameRow : GameRow
{
    /// <summary>Gets or sets the name of the place.</summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}

/// <summary>A row of the table of the lines of the log.</summary>
public sealed class LogMessageRow : GameRow
{
    /// <summary>Gets or sets the text of the line.</summary>
    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;
}

/// <summary>A row of the table of the enemies.</summary>
public sealed class BNpcBaseRow : GameRow
{
    /// <summary>Gets or sets a value indicating whether the enemy turns towards the player.</summary>
    [JsonPropertyName("isOmnidirectional")]
    public bool IsOmnidirectional { get; set; }
}

/// <summary>A row of the table of the actions of the blue mage.</summary>
public sealed class AozActionRow : GameRow
{
    /// <summary>Gets or sets the number of the action.</summary>
    [JsonPropertyName("action")]
    public uint Action { get; set; }
}

/// <summary>A row of the table of the numbers of the actions of the blue mage.</summary>
public sealed class AozActionTransientRow : GameRow
{
    /// <summary>Gets or sets the number the action has in the list.</summary>
    [JsonPropertyName("number")]
    public byte Number { get; set; }
}

/// <summary>A row of the table of the food.</summary>
public sealed class ItemFoodRow : GameRow
{
    /// <summary>Gets or sets the share of the experience the food gives.</summary>
    [JsonPropertyName("expBonusPercent")]
    public uint ExpBonusPercent { get; set; }

    /// <summary>Gets or sets the numbers of the parameters the food gives.</summary>
    [JsonPropertyName("baseParam")]
    public uint[] BaseParam { get; set; } = [];
}

/// <summary>A row of the table of the items of the events.</summary>
public sealed class EventItemRow : GameRow
{
    /// <summary>Gets or sets the name of the item.</summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>Gets or sets the number of the picture of the item.</summary>
    [JsonPropertyName("icon")]
    public ushort Icon { get; set; }

    /// <summary>Gets or sets the number of the action of the item.</summary>
    [JsonPropertyName("action")]
    public uint Action { get; set; }
}
