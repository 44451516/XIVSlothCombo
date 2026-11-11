using System;
using System.Runtime.InteropServices;

namespace SlothCombo.Interop.Game;

/// <summary>The conditions of the world.</summary>
/// <remarks>
/// The game keeps the conditions as a run of flags, one byte each. The
/// program reads the ones it is interested in.
/// </remarks>
[StructLayout(LayoutKind.Explicit, Size = 0x70)]
public unsafe struct Conditions
{
    /// <summary>The flags of the conditions.</summary>
    [FieldOffset(0x00)]
    public fixed byte Flags[112];

    /// <summary>Gets the manager of the conditions.</summary>
    /// <returns>The manager, or <c>null</c> when the game is not ready.</returns>
    public static Conditions* Instance() => (Conditions*)GameFunctions.Get(GameAddresses.Conditions);

    /// <summary>Gets the flag at the given place.</summary>
    /// <param name="index">The place of the flag.</param>
    /// <returns>The value of the flag.</returns>
    public bool this[int index] => Flags[index] != 0;

    /// <summary>Gets a value indicating whether the player is dead.</summary>
    public bool Dead => this[2];

    /// <summary>Gets a value indicating whether the player is busy.</summary>
    public bool Occupied => this[25];

    /// <summary>Gets a value indicating whether the player is in combat.</summary>
    public bool InCombat => this[26];

    /// <summary>Gets a value indicating whether the player is busy in a light event.</summary>
    public bool Occupied30 => this[30];

    /// <summary>Gets a value indicating whether the player is busy in an event.</summary>
    public bool OccupiedInEvent => this[31];

    /// <summary>Gets a value indicating whether the player is busy in a task event.</summary>
    public bool OccupiedInQuestEvent => this[32];

    /// <summary>Gets a value indicating whether the player is busy in a priority event.</summary>
    public bool Occupied33 => this[33];

    /// <summary>Gets a value indicating whether the player is bound to a duty.</summary>
    public bool BoundByDuty => this[34];

    /// <summary>Gets a value indicating whether the player is in a cutscene event.</summary>
    public bool OccupiedInCutSceneEvent => this[35];

    /// <summary>Gets a value indicating whether the player is busy in another way.</summary>
    public bool Occupied38 => this[38];

    /// <summary>Gets a value indicating whether the player is busy in yet another way.</summary>
    public bool Occupied39 => this[39];

    /// <summary>Gets a value indicating whether the player is fishing.</summary>
    public bool Fishing => this[43];

    /// <summary>Gets a value indicating whether the player is between areas.</summary>
    public bool BetweenAreas => this[45];

    /// <summary>Gets a value indicating whether the player is at the summoning bell.</summary>
    public bool OccupiedSummoningBell => this[50];

    /// <summary>Gets a value indicating whether the player is diving.</summary>
    public bool Diving => this[81];

    /// <summary>Gets a value indicating whether the player is on a mount.</summary>
    public bool Mounted => this[4];

    /// <summary>Gets a value indicating whether the player is crafting.</summary>
    public bool Crafting => this[5];

    /// <summary>Gets a value indicating whether the player is gathering.</summary>
    public bool Gathering => this[6];

    /// <summary>Gets a value indicating whether the player is a passenger of a mount.</summary>
    public bool RidingPillion => this[10];

    /// <summary>Gets a value indicating whether the player is between areas a second way.</summary>
    public bool BetweenAreas51 => this[51];

    /// <summary>Gets a value indicating whether the player is logging out.</summary>
    public bool LoggingOut => this[53];

    /// <summary>Gets a value indicating whether the player watches a cutscene.</summary>
    public bool WatchingCutscene => this[58];

    /// <summary>Gets a value indicating whether the player is in flight.</summary>
    public bool InFlight => this[77];

    /// <summary>Gets a value indicating whether the player watches a cutscene in battle.</summary>
    public bool WatchingCutscene78 => this[78];

    /// <summary>Gets a value indicating whether the player is being moved by the game.</summary>
    public bool BeingMoved => this[70];

    /// <summary>Gets a value indicating whether the player is playing a role.</summary>
    public bool RolePlaying => this[90];

    /// <summary>Gets a value indicating whether the duty recorder is playing.</summary>
    public bool DutyRecorderPlayback => this[86];

    /// <summary>Gets a value indicating whether the player pilots a mech.</summary>
    public bool PilotingMech => this[102];
}

/// <summary>The objects the player has its eyes on.</summary>
[StructLayout(LayoutKind.Explicit, Size = 0x6EF0)]
public unsafe struct TargetSystem
{
    /// <summary>The object the player has its eyes on.</summary>
    [FieldOffset(0x80)]
    public GameObject* Target;

    /// <summary>The object the player has its eyes on for the moment.</summary>
    [FieldOffset(0x88)]
    public GameObject* SoftTarget;

    /// <summary>The object under the mouse.</summary>
    [FieldOffset(0xD0)]
    public GameObject* MouseOverTarget;

    /// <summary>The object the player keeps its eyes on.</summary>
    [FieldOffset(0xF8)]
    public GameObject* FocusTarget;

    /// <summary>Gets the manager of the targets.</summary>
    /// <returns>The manager, or <c>null</c> when the game is not ready.</returns>
    public static TargetSystem* Instance() => (TargetSystem*)GameFunctions.Get(GameAddresses.TargetSystem);

    /// <summary>
    /// Gets the object the player has its eyes on.
    /// </summary>
    /// <remarks>
    /// The game resolves the hard target and the soft target into one object,
    /// and the first one that is set wins.
    /// </remarks>
    /// <returns>The object, or <c>null</c> when there is none.</returns>
    public GameObject* GetTargetObject()
    {
        var function = (delegate* unmanaged<TargetSystem*, GameObject*>)
            GameFunctions.Get(GameAddresses.TargetSystemGetTargetObject);

        return function(StructPtr.Of(ref this));
    }

    /// <summary>Gets the object the player has its eyes on for the moment.</summary>
    /// <returns>The object, or <c>null</c> when there is none.</returns>
    public GameObject* GetSoftTarget()
    {
        var function = (delegate* unmanaged<TargetSystem*, GameObject*>)
            GameFunctions.Get(GameAddresses.TargetSystemGetSoftTarget);

        return function(StructPtr.Of(ref this));
    }

    /// <summary>Gets the object the player keeps its eyes on.</summary>
    /// <returns>The object, or <c>null</c> when there is none.</returns>
    public GameObject* GetHardTarget()
    {
        var function = (delegate* unmanaged<TargetSystem*, GameObject*>)
            GameFunctions.Get(GameAddresses.TargetSystemGetHardTarget);

        return function(StructPtr.Of(ref this));
    }

    /// <summary>Points the player at the given object.</summary>
    /// <param name="target">The object to point at.</param>
    /// <param name="ignoreTargetModes">Whether the modes of the target are ignored.</param>
    /// <param name="a4">Whether the change is made quietly.</param>
    /// <param name="a5">An extra number of the change.</param>
    /// <returns><c>true</c> when the target was changed.</returns>
    public bool SetHardTarget(GameObject* target, bool ignoreTargetModes = false, bool a4 = false, int a5 = 0)
    {
        var function = (delegate* unmanaged<TargetSystem*, GameObject*, bool, bool, int, bool>)
            GameFunctions.Get(GameAddresses.TargetSystemSetHardTarget);

        return function(StructPtr.Of(ref this), target, ignoreTargetModes, a4, a5);
    }
}

/// <summary>The main state of the game.</summary>
[StructLayout(LayoutKind.Explicit, Size = 0x4170)]
public unsafe struct GameMain
{
    /// <summary>The gauges of the jobs.</summary>
    [FieldOffset(0x0B28)]
    public Gauges.JobGaugeManager JobGaugeManager;

    /// <summary>The area the player is in.</summary>
    [FieldOffset(0x4108)]
    public uint CurrentTerritoryTypeId;

    /// <summary>Gets the main state of the game.</summary>
    /// <returns>The state, or <c>null</c> when the game is not ready.</returns>
    public static GameMain* Instance() => (GameMain*)GameFunctions.Get(GameAddresses.GameMain);

    /// <summary>Tells whether the player is in a battle against other players.</summary>
    /// <returns><c>true</c> when the player is in a battle against other players.</returns>
    public static bool IsInPvPArea()
    {
        var function = (delegate* unmanaged<bool>)GameFunctions.Get(GameAddresses.IsInPvPArea);
        return function();
    }

    /// <summary>Tells whether the player is in a battle against other players inside an instance.</summary>
    /// <returns><c>true</c> when the player is in such an instance.</returns>
    public static bool IsInPvPInstance()
    {
        var function = (delegate* unmanaged<bool>)GameFunctions.Get(GameAddresses.IsInPvPInstance);
        return function();
    }
}

/// <summary>The kind of a container of items.</summary>
public enum InventoryType : uint
{
    /// <summary>The items the player carries.</summary>
    Inventory1 = 0,

    /// <summary>The items the player carries for the saddler.</summary>
    Inventory2 = 1,

    /// <summary>The items the player carries for the armourer.</summary>
    Inventory3 = 2,

    /// <summary>The items the player carries for the weaponsmith.</summary>
    Inventory4 = 3,

    /// <summary>The items the player has equipped.</summary>
    EquippedItems = 1000,

    /// <summary>The money of the player.</summary>
    Currency = 2000,

    /// <summary>The crystals of the player.</summary>
    Crystals = 2001,

    /// <summary>The items of a retainer.</summary>
    RetainerPage1 = 10000,

    /// <summary>No container at all.</summary>
    Invalid = 9999,
}

/// <summary>An item in a container.</summary>
[StructLayout(LayoutKind.Explicit, Size = 0x48)]
public struct InventoryItem
{
    /// <summary>The kind of the container the item is in.</summary>
    [FieldOffset(0x08)]
    public InventoryType Container;

    /// <summary>The place of the item in the container.</summary>
    [FieldOffset(0x0C)]
    public short Slot;

    /// <summary>The number of the item.</summary>
    [FieldOffset(0x10)]
    public uint ItemId;

    /// <summary>The number of the items.</summary>
    [FieldOffset(0x14)]
    public int Quantity;

    /// <summary>The number of the refining of the item, or of its collectables.</summary>
    [FieldOffset(0x18)]
    public ushort SpiritbondOrCollectability;

    /// <summary>The state of the item.</summary>
    [FieldOffset(0x1A)]
    public ushort Condition;
}

/// <summary>A container of items.</summary>
[StructLayout(LayoutKind.Explicit, Size = 0x20)]
public unsafe struct InventoryContainer
{
    /// <summary>The items in the container.</summary>
    [FieldOffset(0x08)]
    public InventoryItem* Items;

    /// <summary>The kind of the container.</summary>
    [FieldOffset(0x10)]
    public InventoryType Type;

    /// <summary>The number of places in the container.</summary>
    [FieldOffset(0x14)]
    public int Size;

    /// <summary>Gets the item at the given place.</summary>
    /// <param name="index">The place of the item.</param>
    /// <returns>The item.</returns>
    public InventoryItem this[int index] => Items[index];
}

/// <summary>The items the player carries.</summary>
[StructLayout(LayoutKind.Explicit, Size = 0x3730)]
public unsafe struct InventoryManager
{
    /// <summary>The containers of the items.</summary>
    [FieldOffset(0x1E08)]
    public InventoryContainer* Inventories;

    /// <summary>The kind of the container of the equipped items.</summary>
    [FieldOffset(0x1E10)]
    public InventoryType EquippedItemsInventoryType;

    /// <summary>Gets the manager of the items.</summary>
    /// <returns>The manager, or <c>null</c> when the game is not ready.</returns>
    public static InventoryManager* Instance()
        => (InventoryManager*)GameFunctions.Get(GameAddresses.InventoryManager);

    /// <summary>Gives the container of the given kind.</summary>
    /// <param name="inventoryType">The kind of the container.</param>
    /// <returns>The container, or <c>null</c> when there is none.</returns>
    public InventoryContainer* GetInventoryContainer(InventoryType inventoryType)
    {
        var function = (delegate* unmanaged<InventoryManager*, InventoryType, InventoryContainer*>)
            GameFunctions.Get(GameAddresses.GetInventoryContainer);

        return function(StructPtr.Of(ref this), inventoryType);
    }

    /// <summary>Gives the number of the items the player carries.</summary>
    /// <param name="itemId">The number of the item.</param>
    /// <param name="isHq">Whether the better item is counted.</param>
    /// <param name="checkEquipped">Whether the equipped items are counted.</param>
    /// <param name="checkArmory">Whether the armoury is counted.</param>
    /// <param name="minCollectability">The number of collectables the item needs.</param>
    /// <returns>The number of the items.</returns>
    public int GetInventoryItemCount(
        uint itemId,
        bool isHq = false,
        bool checkEquipped = true,
        bool checkArmory = true,
        short minCollectability = 0)
    {
        var function = (delegate* unmanaged<InventoryManager*, uint, bool, bool, bool, short, int>)
            GameFunctions.Get(GameAddresses.GetInventoryItemCount);

        return function(StructPtr.Of(ref this), itemId, isHq, checkEquipped, checkArmory, minCollectability);
    }
}

/// <summary>A member of the group of the player.</summary>
[StructLayout(LayoutKind.Explicit, Size = 0x490)]
public unsafe struct PartyMember
{
    /// <summary>The place of the member in the world.</summary>
    [FieldOffset(0x3E0)]
    public GameVector3 Position;

    /// <summary>The number of the account of the member.</summary>
    [FieldOffset(0x3F0)]
    public ulong AccountId;

    /// <summary>The number that tells the identity of the member.</summary>
    [FieldOffset(0x3F8)]
    public ulong ContentId;

    /// <summary>The number that tells which object the member is.</summary>
    [FieldOffset(0x400)]
    public uint EntityId;

    /// <summary>The hit points of the member.</summary>
    [FieldOffset(0x40C)]
    public uint CurrentHP;

    /// <summary>The hit points the member can hold.</summary>
    [FieldOffset(0x410)]
    public uint MaxHP;

    /// <summary>The mana of the member.</summary>
    [FieldOffset(0x414)]
    public ushort CurrentMP;

    /// <summary>The mana the member can hold.</summary>
    [FieldOffset(0x416)]
    public ushort MaxMP;

    /// <summary>The number of bytes the name of the member takes.</summary>
    private const int NameByteCount = 64;

    /// <summary>The name of the member, as a run of bytes that end with a zero.</summary>
    [FieldOffset(0x41C)]
    public fixed byte NameBytes[NameByteCount];

    /// <summary>The class of the member.</summary>
    [FieldOffset(0x469)]
    public byte ClassJob;

    /// <summary>The level of the member.</summary>
    [FieldOffset(0x46A)]
    public byte Level;

    /// <summary>Gets the name of the member.</summary>
    public string NameString
    {
        get
        {
            fixed (byte* bytes = NameBytes)
            {
                var length = 0;
                while (length < NameByteCount && bytes[length] != 0)
                {
                    length++;
                }

                return System.Text.Encoding.UTF8.GetString(bytes, length);
            }
        }
    }
}

/// <summary>The group of the player.</summary>
[StructLayout(LayoutKind.Explicit, Size = 0x7FF0)]
public unsafe struct Group
{
    /// <summary>The members of the party.</summary>
    [FieldOffset(0x0000)]
    public unsafe PartyMember* PartyMembers;

    /// <summary>The members of the alliance.</summary>
    [FieldOffset(0x2480)]
    public unsafe PartyMember* AllianceMembers;

    /// <summary>The number of the party.</summary>
    [FieldOffset(0x7FC8)]
    public long PartyId;

    /// <summary>The number of the members of the group.</summary>
    [FieldOffset(0x7FDC)]
    public byte MemberCount;

    /// <summary>The flags of the alliance.</summary>
    [FieldOffset(0x7FE1)]
    public byte AllianceFlags;

    /// <summary>Gets a value indicating whether the group is an alliance.</summary>
    public bool IsAlliance => (AllianceFlags & 1) != 0;
}

/// <summary>The manager of the groups of the world.</summary>
[StructLayout(LayoutKind.Explicit, Size = 0x10000)]
public unsafe struct GroupManager
{
    /// <summary>The group the player is in.</summary>
    [FieldOffset(0x20)]
    public Group MainGroup;

    /// <summary>Gets the manager of the groups.</summary>
    /// <returns>The manager, or <c>null</c> when the game is not ready.</returns>
    public static GroupManager* Instance() => (GroupManager*)GameFunctions.Get(GameAddresses.GroupManager);
}

/// <summary>The state of the interface of the game.</summary>
[StructLayout(LayoutKind.Explicit, Size = 0x1A400)]
public unsafe struct UIState
{
    /// <summary>Gets the state of the interface.</summary>
    /// <returns>The state, or <c>null</c> when the game is not ready.</returns>
    public static UIState* Instance() => (UIState*)GameFunctions.Get(GameAddresses.UIState);

    /// <summary>Tells whether a link of a task is unlocked.</summary>
    /// <param name="unlockLinkOrQuestId">The number of the link or of the task.</param>
    /// <param name="minQuestProgression">The progress the task needs at least.</param>
    /// <param name="a4">Whether the flag of the unlock is respected.</param>
    /// <returns><c>true</c> when the link is unlocked.</returns>
    public bool IsUnlockLinkUnlockedOrQuestCompleted(uint unlockLinkOrQuestId, byte minQuestProgression = 0, bool a4 = true)
    {
        var function = (delegate* unmanaged<UIState*, uint, byte, bool, bool>)
            GameFunctions.Get(GameAddresses.IsUnlockLinkUnlockedOrQuestCompleted);

        return function(StructPtr.Of(ref this), unlockLinkOrQuestId, minQuestProgression, a4);
    }
}
