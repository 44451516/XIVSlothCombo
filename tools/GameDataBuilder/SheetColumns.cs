using System.Text.Json;

namespace SlothCombo.GameDataBuilder;

/// <summary>The columns of a sheet that are carried over into the tables.</summary>
/// <remarks>
/// Only the columns the program has a use for are carried over. The sheets of
/// the game hold many more, and carrying them along would make the tables
/// many times bigger for nothing.
/// </remarks>
internal static class SheetColumns
{
    /// <summary>The columns of the sheet of the actions.</summary>
    public static readonly string[] Action =
    [
        "Name",
        "Icon",
        "ActionCategory",
        "ClassJob",
        "ClassJobLevel",
        "Range",
        "EffectRange",
        "Cast100ms",
        "Recast100ms",
        "CooldownGroup",
        "MaxCharges",
        "UnlockLink",
        "ClassJobCategory",
        "IsPvP",
        "IsPlayerAction",
        "CanTargetParty",
        "CanTargetSelf",
        "CanTargetHostile",
        "CanTargetAlly",
        "TargetArea",
    ];

    /// <summary>The columns of the sheet of the kinds of the actions.</summary>
    public static readonly string[] ActionCategory =
    [
        "Name",
    ];

    /// <summary>The columns of the sheet of the status effects.</summary>
    public static readonly string[] Status =
    [
        "Name",
        "Icon",
        "CanDispel",
        "MaxStacks",
    ];

    /// <summary>The columns of the sheet of the items.</summary>
    public static readonly string[] Item =
    [
        "Name",
        "Icon",
        "ItemAction",
        "ItemUICategory",
    ];

    /// <summary>The columns of the sheet of the actions of the items.</summary>
    public static readonly string[] ItemAction =
    [
        "Action",
        "Data[0]",
        "DataHQ[0]",
    ];

    /// <summary>The columns of the sheet of the classes.</summary>
    public static readonly string[] ClassJob =
    [
        "Name",
        "Abbreviation",
        "NameEnglish",
        "ClassJobCategory",
        "Role",
    ];

    /// <summary>The columns of the sheet of the kinds of the classes.</summary>
    public static readonly string[] ClassJobCategory =
    [
        "Name",
    ];

    /// <summary>The columns of the sheet of the traits.</summary>
    public static readonly string[] Trait =
    [
        "Name",
        "Icon",
        "Level",
        "ClassJobCategory",
    ];

    /// <summary>The columns of the sheet of the tasks.</summary>
    public static readonly string[] Quest =
    [
        "Name",
    ];

    /// <summary>The columns of the sheet of the areas.</summary>
    public static readonly string[] TerritoryType =
    [
        "PlaceName",
    ];

    /// <summary>The columns of the sheet of the names of the places.</summary>
    public static readonly string[] PlaceName =
    [
        "Name",
    ];

    /// <summary>The columns of the sheet of the lines of the log.</summary>
    public static readonly string[] LogMessage =
    [
        "Text",
    ];

    /// <summary>The columns of the sheet of the enemies.</summary>
    public static readonly string[] BNpcBase =
    [
        "IsOmnidirectional",
    ];

    /// <summary>The columns of the sheet of the actions of the blue mage.</summary>
    public static readonly string[] AozAction =
    [
        "Action",
    ];

    /// <summary>The columns of the sheet of the numbers of the blue mage.</summary>
    public static readonly string[] AozActionTransient =
    [
        "Number",
    ];

    /// <summary>The columns of the sheet of the food.</summary>
    /// <remarks>
    /// The food carries a handful of values for every one of its entries.
    /// Only the entries of the parameters are of interest, so only those are
    /// carried over.
    /// </remarks>
    public static readonly string[] ItemFood =
    [
        "EXPBonusPercent",
        "BaseParam[0]",
    ];

    /// <summary>The columns of the sheet of the items of the events.</summary>
    public static readonly string[] EventItem =
    [
        "Name",
        "Icon",
        "Action",
    ];
}

/// <summary>The name of a sheet and the columns that are carried over.</summary>
/// <param name="Name">The name of the sheet.</param>
/// <param name="Columns">The columns that are carried over.</param>
/// <param name="GatheredJobs">
/// The name of the field the classes of a kind are gathered into. The sheet of
/// the kinds of the classes holds one column per class, so the classes of a
/// kind are the columns of the row that are set. The field is only written
/// when it is set, so it is empty for the sheets that do not gather it.
/// </param>
internal readonly record struct SheetPlan(string Name, string[] Columns, string? GatheredJobs = null);
