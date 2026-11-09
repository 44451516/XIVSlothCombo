using System;

namespace SlothCombo.Abstractions;

/// <summary>
/// The subset of the Dalamud condition flags the combo logic relies on.
/// </summary>
/// <remarks>
/// <para>
/// The values are the ordinal values of
/// <c>Dalamud.Game.ClientState.Conditions.ConditionFlag</c>, so the legacy
/// plugin can move between the two representations without a translation
/// table. The upstream enum is a plain ordinal list rather than a bit mask,
/// which is why these values are small consecutive numbers.
/// </para>
/// </remarks>
public enum ConditionFlag
{
    /// <summary>No condition.</summary>
    None = 0,

    /// <summary>Normal conditions.</summary>
    NormalConditions = 1,

    /// <summary>The player is unconscious.</summary>
    Unconscious = 2,

    /// <summary>The player is emoting.</summary>
    Emoting = 3,

    /// <summary>The player is mounted.</summary>
    Mounted = 4,

    /// <summary>The player is crafting.</summary>
    Crafting = 5,

    /// <summary>The player is gathering.</summary>
    Gathering = 6,

    /// <summary>The player is melding materia.</summary>
    MeldingMateria = 7,

    /// <summary>The player is occupied in a general action.</summary>
    Occupied = 25,

    /// <summary>The player is in combat.</summary>
    InCombat = 26,

    /// <summary>The player is casting.</summary>
    Casting = 27,

    /// <summary>The player is occupied in a crafting related action.</summary>
    Occupied30 = 30,

    /// <summary>The player is occupied in an event.</summary>
    OccupiedInEvent = 31,

    /// <summary>The player is occupied in a quest.</summary>
    OccupiedInQuestEvent = 32,

    /// <summary>The player is occupied in a gathering related action.</summary>
    Occupied33 = 33,

    /// <summary>The player is bound by the current duty.</summary>
    BoundByDuty = 34,

    /// <summary>The player is occupied in a cut scene.</summary>
    OccupiedInCutSceneEvent = 35,

    /// <summary>The player is occupied in a fishing related action.</summary>
    Occupied38 = 38,

    /// <summary>The player is occupied in another timed action.</summary>
    Occupied39 = 39,

    /// <summary>The player is fishing.</summary>
    Fishing = 43,

    /// <summary>The player is between areas.</summary>
    BetweenAreas = 45,

    /// <summary>The player is occupied summoning a bell.</summary>
    OccupiedSummoningBell = 50,

    /// <summary>The player is diving.</summary>
    Diving = 81,
}
