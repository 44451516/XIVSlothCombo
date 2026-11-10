using System;
using System.Collections.Generic;

namespace SlothCombo.Abstractions;

/// <summary>
/// The game services the combo logic depends on.
/// </summary>
/// <remarks>
/// <para>
/// The legacy plugin reached for the Dalamud service locator
/// (<c>Service.ObjectTable</c>, <c>Service.Condition</c>, ...) and for the
/// ECommons shortcut (<c>Svc.Data</c>, <c>Svc.Condition</c>, ...). Both are
/// replaced by this single interface so the combo logic can run on the
/// standalone runtime without Dalamud being present at all.
/// </para>
/// <para>
/// Implementations must be safe to call from the game's main thread only,
/// with the single exception of <see cref="IDataManager"/>, which may be
/// called from any thread as long as it does its own synchronisation.
/// </para>
/// </remarks>
public interface IGameServices
{
    /// <summary>Gets the object table of the game.</summary>
    IObjectTable ObjectTable { get; }

    /// <summary>Gets the condition state of the player.</summary>
    IConditionService Condition { get; }

    /// <summary>Gets the target information of the game.</summary>
    ITargetService Targets { get; }

    /// <summary>Gets the party state of the game.</summary>
    IPartyService Party { get; }

    /// <summary>Gets the static game data (Excel sheets).</summary>
    IDataManager Data { get; }

    /// <summary>Gets the chat output helper.</summary>
    IChatService Chat { get; }

    /// <summary>Gets the logging helper.</summary>
    IComboLogger Log { get; }

    /// <summary>Gets the client state of the game.</summary>
    IClientStateService ClientState { get; }
}

/// <summary>Access to the objects currently known to the game.</summary>
public interface IObjectTable : IEnumerable<IGameObject>
{
    /// <summary>Gets the local player, or <c>null</c> when not in the world.</summary>
    IGameObject? LocalPlayer { get; }

    /// <summary>Gets every object currently in the game world.</summary>
    IEnumerable<IGameObject> Objects { get; }
}

/// <summary>Access to the condition flags of the player.</summary>
public interface IConditionService
{
    /// <summary>Gets a value indicating whether the given condition is active.</summary>
    bool this[ConditionFlag flag] { get; }

    /// <summary>Raised when a condition of the player changes.</summary>
    event Action<ConditionFlag, bool>? ConditionChange;
}

/// <summary>Access to the current target of the player.</summary>
public interface ITargetService
{
    /// <summary>Gets or sets the hard target, or <c>null</c>.</summary>
    IGameObject? Target { get; set; }

    /// <summary>Gets the soft target, or <c>null</c>.</summary>
    IGameObject? SoftTarget { get; }

    /// <summary>Gets the focus target, or <c>null</c>.</summary>
    IGameObject? FocusTarget { get; }

    /// <summary>Gets the mouse over target, or <c>null</c>.</summary>
    IGameObject? MouseOverTarget { get; }
}

/// <summary>Access to the party of the player.</summary>
public interface IPartyService
{
    /// <summary>Gets the id of the party, or zero when the player is alone.</summary>
    long PartyId { get; }

    /// <summary>Gets the number of party members.</summary>
    int Length { get; }

    /// <summary>Gets a value indicating whether the party is an alliance.</summary>
    bool IsAlliance { get; }

    /// <summary>Gets the party member at the given slot index, or <c>null</c>.</summary>
    IPartyMember? GetSlot(int index);
}

/// <summary>A single member of the party.</summary>
public interface IPartyMember
{
    /// <summary>Gets the entity id of the member.</summary>
    uint EntityId { get; }

    /// <summary>Gets the content id of the member, or zero when it is unknown.</summary>
    ulong ContentId { get; }

    /// <summary>Gets the name of the member.</summary>
    string Name { get; }

    /// <summary>Gets the level of the member.</summary>
    byte Level { get; }

    /// <summary>Gets the row id of the class job of the member.</summary>
    uint ClassJobRowId { get; }

    /// <summary>Gets the current hit points of the member.</summary>
    uint CurrentHp { get; }

    /// <summary>Gets the maximum hit points of the member.</summary>
    uint MaxHp { get; }

    /// <summary>Gets the current mana of the member.</summary>
    ushort CurrentMp { get; }

    /// <summary>Gets the maximum mana of the member.</summary>
    ushort MaxMp { get; }

    /// <summary>Gets the position of the member.</summary>
    System.Numerics.Vector3 Position { get; }

    /// <summary>Gets the game object of the member, or <c>null</c> when it is not loaded.</summary>
    IGameObject? GameObject { get; }
}

/// <summary>Chat output.</summary>
public interface IChatService
{
    /// <summary>Writes a line to the chat log.</summary>
    void Print(string message);

    /// <summary>Writes an error line to the chat log.</summary>
    void PrintError(string message);
}

/// <summary>Logging used by the combo logic.</summary>
public interface IComboLogger
{
    /// <summary>Writes a verbose message.</summary>
    void Verbose(string message);

    /// <summary>Writes a debug message.</summary>
    void Debug(string message);

    /// <summary>Writes an informational message.</summary>
    void Information(string message);

    /// <summary>Writes a warning message.</summary>
    void Warning(string message);

    /// <summary>Writes an error message.</summary>
    void Error(string message);

    /// <summary>Writes an error message with an exception.</summary>
    void Error(Exception exception, string message);
}

/// <summary>High level state of the game client.</summary>
public interface IClientStateService
{
    /// <summary>Gets a value indicating whether the player is in PvP.</summary>
    bool IsPvP { get; }

    /// <summary>Gets the language of the client.</summary>
    ClientLanguage ClientLanguage { get; }

    /// <summary>Gets the id of the territory the player is in.</summary>
    uint TerritoryType { get; }

    /// <summary>Gets the local player, or <c>null</c> when not in the world.</summary>
    IGameObject? LocalPlayer { get; }

    /// <summary>Raised when the player moves into another territory.</summary>
    event Action<uint>? TerritoryChanged;
}

/// <summary>The language of the game client.</summary>
public enum ClientLanguage
{
    /// <summary>Unknown language.</summary>
    Unknown,

    /// <summary>Japanese.</summary>
    Ja,

    /// <summary>English.</summary>
    En,

    /// <summary>German.</summary>
    De,

    /// <summary>French.</summary>
    Fr,

    /// <summary>Chinese.</summary>
    Zh,

    /// <summary>Korean.</summary>
    Ko,
}
