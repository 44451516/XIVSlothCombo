using System;
using System.Collections.Generic;
using System.Numerics;

namespace SlothCombo.Abstractions;

/// <summary>
/// An object of the game world.
/// </summary>
/// <remarks>
/// The surface of this interface is the one the combo logic relies on. It is
/// deliberately shaped after the object interface of the legacy plugin so the
/// rotation code keeps reading the same way after the move off Dalamud.
/// </remarks>
public interface IGameObject
{
    /// <summary>Gets the entity id of the object.</summary>
    ulong GameObjectId { get; }

    /// <summary>Gets the object id of the object.</summary>
    uint ObjectId { get; }

    /// <summary>Gets the base id of the object, which identifies its kind.</summary>
    uint BaseId { get; }

    /// <summary>Gets the name of the object.</summary>
    string Name { get; }

    /// <summary>Gets the level of the object.</summary>
    byte Level { get; }

    /// <summary>Gets the class job of the object.</summary>
    RowRef ClassJob { get; }

    /// <summary>Gets the position of the object.</summary>
    Vector3 Position { get; }

    /// <summary>Gets the rotation of the object.</summary>
    float Rotation { get; }

    /// <summary>Gets the horizontal distance from the player to the object.</summary>
    byte YalmDistanceX { get; }

    /// <summary>Gets the entity id of the target of the object.</summary>
    ulong TargetObjectId { get; }

    /// <summary>Gets the target of the object, or <c>null</c> when there is none.</summary>
    IGameObject? TargetObject { get; }

    /// <summary>Gets the current hit points of the object.</summary>
    uint CurrentHp { get; }

    /// <summary>Gets the maximum hit points of the object.</summary>
    uint MaxHp { get; }

    /// <summary>Gets the current mana of the object.</summary>
    uint CurrentMp { get; }

    /// <summary>Gets the maximum mana of the object.</summary>
    uint MaxMp { get; }

    /// <summary>Gets a value indicating whether the object is casting.</summary>
    bool IsCasting { get; }

    /// <summary>Gets a value indicating whether the cast of the object can be interrupted.</summary>
    bool IsCastInterruptible { get; }

    /// <summary>Gets the action the object is casting.</summary>
    uint CastActionId { get; }

    /// <summary>Gets the elapsed cast time of the object.</summary>
    float CurrentCastTime { get; }

    /// <summary>Gets the total cast time of the action the object is casting.</summary>
    float TotalCastTime { get; }

    /// <summary>Gets a value indicating whether the object is dead.</summary>
    bool IsDead { get; }

    /// <summary>Gets a value indicating whether the object can be targeted.</summary>
    bool IsTargetable { get; }

    /// <summary>Gets a value indicating whether the object is hostile.</summary>
    bool IsHostile { get; }

    /// <summary>Gets the hit box radius of the object.</summary>
    float HitboxRadius { get; }

    /// <summary>Gets the status effects on the object.</summary>
    IReadOnlyList<IStatus> StatusList { get; }

    /// <summary>Gets the id of the mount the object rides.</summary>
    uint CurrentMountId { get; }

    /// <summary>Gets the mount the object rides, or <c>null</c> when it is on foot.</summary>
    IGameObject? CurrentMount { get; }

    /// <summary>Gets a value indicating whether the object is the player.</summary>
    bool IsPlayerCharacter { get; }

    /// <summary>Gets the kind of the object.</summary>
    ObjectKind ObjectKind { get; }

    /// <summary>Gets the address of the native object.</summary>
    IntPtr Address { get; }
}

/// <summary>
/// An object that can fight, and therefore carries resources, a cast state and
/// a status list.
/// </summary>
public interface IBattleChara : IGameObject
{
}

/// <summary>
/// An enemy of the player.
/// </summary>
public interface IBattleNpc : IGameObject
{
    /// <summary>Gets the name id of the enemy.</summary>
    uint NameId { get; }

    /// <summary>Gets the kind of the enemy.</summary>
    BattleNpcSubKind BattleNpcKind { get; }
}

/// <summary>
/// The kind of an enemy of the player.
/// </summary>
/// <remarks>
/// The numbering matches the one of the game so the mapping stays a plain
/// cast at runtime.
/// </remarks>
public enum BattleNpcSubKind
{
    /// <summary>No kind.</summary>
    None = 0,

    /// <summary>A part of an enemy.</summary>
    BNpcPart = 1,

    /// <summary>A pet.</summary>
    Pet = 2,

    /// <summary>A buddy.</summary>
    Buddy = 3,

    /// <summary>A player.</summary>
    Player = 4,

    /// <summary>An enemy that fights.</summary>
    Combatant = 5,

    /// <summary>A race chocobo.</summary>
    RaceChocobo = 6,

    /// <summary>A love minion.</summary>
    LovmMinion = 7,

    /// <summary>A member of the party of the enemies.</summary>
    NpcPartyMember = 9,
}

/// <summary>
/// A reference to a row of an excel sheet.
/// </summary>
/// <param name="RowId">The row id.</param>
public readonly record struct RowRef(uint RowId);

/// <summary>
/// The kind of an object in the game world.
/// </summary>
/// <remarks>
/// The numbering matches the one of the game so the mapping stays a plain
/// cast at runtime.
/// </remarks>
public enum ObjectKind
{
    /// <summary>An unknown object.</summary>
    None = 0,

    /// <summary>A player character.</summary>
    Pc = 1,

    /// <summary>An enemy.</summary>
    BattleNpc = 2,

    /// <summary>An event object.</summary>
    EventNpc = 3,

    /// <summary>A treasure.</summary>
    Treasure = 4,

    /// <summary>A gathering point.</summary>
    GatheringPoint = 6,

    /// <summary>A mount.</summary>
    Mount = 8,

    /// <summary>A companion or pet.</summary>
    Companion = 9,

    /// <summary>An ornament.</summary>
    Ornament = 15,
}
