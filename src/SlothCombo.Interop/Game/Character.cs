using System;
using System.Runtime.InteropServices;

namespace SlothCombo.Interop.Game;

/// <summary>The data every character of the world carries.</summary>
/// <remarks>
/// The data is laid out behind the data of the object of the world, so the
/// numbers below are the ones inside the structure on its own.
/// </remarks>
[StructLayout(LayoutKind.Explicit, Size = 0x50)]
public unsafe struct CharacterData
{
    /// <summary>The table of the virtual functions of the character.</summary>
    [FieldOffset(0x00)]
    public void* VTable;

    /// <summary>The hit points of the character.</summary>
    [FieldOffset(0x0C)]
    public uint Health;

    /// <summary>The hit points the character can hold.</summary>
    [FieldOffset(0x10)]
    public uint MaxHealth;

    /// <summary>The mana of the character.</summary>
    [FieldOffset(0x14)]
    public uint Mana;

    /// <summary>The mana the character can hold.</summary>
    [FieldOffset(0x18)]
    public uint MaxMana;

    /// <summary>The class of the character.</summary>
    [FieldOffset(0x2A)]
    public byte ClassJob;

    /// <summary>The level of the character.</summary>
    [FieldOffset(0x2B)]
    public byte Level;

    /// <summary>The value of the shield of the character.</summary>
    [FieldOffset(0x2E)]
    public byte ShieldValue;

    /// <summary>The flags that tell whether the character fights the player.</summary>
    [FieldOffset(0x34)]
    public byte Flags;

    /// <summary>Gets a value indicating whether the character is hostile.</summary>
    public bool IsHostile => BitField.GetBit(Flags, 0);

    /// <summary>Gets a value indicating whether the character is in combat.</summary>
    public bool InCombat => BitField.GetBit(Flags, 1);
}

/// <summary>The mount a character rides.</summary>
[StructLayout(LayoutKind.Explicit, Size = 0x68)]
public unsafe struct MountContainer
{
    /// <summary>The number of the places the container keeps an entity in.</summary>
    public const int MountedEntityIdCount = 7;

    /// <summary>The object of the mount itself.</summary>
    [FieldOffset(0x10)]
    public Character* MountObject;

    /// <summary>The number of the mount.</summary>
    [FieldOffset(0x18)]
    public ushort MountId;

    /// <summary>The numbers of the entities that ride along.</summary>
    [FieldOffset(0x24)]
    public uint* MountedEntityIds;
}

/// <summary>The kind of a companion.</summary>
public enum CompanionType
{
    /// <summary>No kind.</summary>
    None = 0,

    /// <summary>A companion.</summary>
    Companion = 1,

    /// <summary>A mount that is followed.</summary>
    FollowMount = 2,
}

/// <summary>The companion of a character.</summary>
[StructLayout(LayoutKind.Explicit, Size = 0x20)]
public unsafe struct CompanionContainer
{
    /// <summary>The companion itself.</summary>
    [FieldOffset(0x10)]
    public Companion* CompanionObject;

    /// <summary>The number of the companion.</summary>
    [FieldOffset(0x18)]
    public ushort CompanionId;
}

/// <summary>A companion of a character.</summary>
/// <remarks>
/// A companion is a character with its own data behind it. Only the kind of
/// the companion is of interest to the program.
/// </remarks>
[StructLayout(LayoutKind.Explicit, Size = 0x2470)]
public unsafe struct Companion
{
    /// <summary>The kind of the companion.</summary>
    [FieldOffset(0x2370)]
    public CompanionType CompanionType;
}

/// <summary>The data of a character of the world.</summary>
/// <remarks>
/// The structure continues the data of the object of the world, so the
/// numbers below are the ones of the whole character. A character that
/// fights adds the data of the fight behind this structure.
/// </remarks>
[StructLayout(LayoutKind.Explicit, Size = 0x2370)]
public unsafe struct Character
{
    /// <summary>The data of the object of the world.</summary>
    [FieldOffset(0x0000)]
    public GameObject GameObject;

    /// <summary>The data every character carries.</summary>
    [FieldOffset(0x01A0)]
    public CharacterData CharacterData;

    /// <summary>The mount the character rides.</summary>
    [FieldOffset(0x0670)]
    public MountContainer Mount;

    /// <summary>The companion of the character.</summary>
    [FieldOffset(0x06D8)]
    public CompanionContainer CompanionData;

    /// <summary>The flags that tell who the character is related to.</summary>
    [FieldOffset(0x1CE2)]
    public byte RelationFlags;

    /// <summary>The object of the companion.</summary>
    [FieldOffset(0x22F8)]
    public Companion* CompanionObject;

    /// <summary>The object the character has its eyes on.</summary>
    [FieldOffset(0x2308)]
    public GameObjectId TargetId;

    /// <summary>The number that tells the name of the character.</summary>
    [FieldOffset(0x2338)]
    public uint NameId;

    /// <summary>Gets the hit points of the character.</summary>
    public uint Health => CharacterData.Health;

    /// <summary>Gets the hit points the character can hold.</summary>
    public uint MaxHealth => CharacterData.MaxHealth;

    /// <summary>Gets the mana of the character.</summary>
    public uint Mana => CharacterData.Mana;

    /// <summary>Gets the mana the character can hold.</summary>
    public uint MaxMana => CharacterData.MaxMana;

    /// <summary>Gets the class of the character.</summary>
    public byte ClassJob => CharacterData.ClassJob;

    /// <summary>Gets the level of the character.</summary>
    public byte Level => CharacterData.Level;

    /// <summary>Gets the value of the shield of the character.</summary>
    public byte ShieldValue => CharacterData.ShieldValue;

    /// <summary>Gets a value indicating whether the character is hostile.</summary>
    public bool IsHostile => CharacterData.IsHostile;

    /// <summary>Gets a value indicating whether the character is in combat.</summary>
    public bool InCombat => CharacterData.InCombat;

    /// <summary>Gets a value indicating whether the character is in the party.</summary>
    public bool IsPartyMember => BitField.GetBit(RelationFlags, 0);

    /// <summary>Gets a value indicating whether the character is in the alliance.</summary>
    public bool IsAllianceMember => BitField.GetBit(RelationFlags, 1);

    /// <summary>Gets a value indicating whether the character is a friend.</summary>
    public bool IsFriend => BitField.GetBit(RelationFlags, 2);
}

/// <summary>The state of a cast of a character.</summary>
[StructLayout(LayoutKind.Explicit, Size = 0x170)]
public struct CastInfo
{
    /// <summary>The flags of the cast.</summary>
    [FieldOffset(0x00)]
    public byte Flags;

    /// <summary>The kind of the action that is cast.</summary>
    [FieldOffset(0x01)]
    public byte ActionType;

    /// <summary>The action that is cast.</summary>
    [FieldOffset(0x04)]
    public uint ActionId;

    /// <summary>The object the cast is aimed at.</summary>
    [FieldOffset(0x10)]
    public GameObjectId TargetId;

    /// <summary>The time the cast has taken so far.</summary>
    [FieldOffset(0x34)]
    public float CurrentCastTime;

    /// <summary>The time the cast takes.</summary>
    [FieldOffset(0x3C)]
    public float TotalCastTime;

    /// <summary>Gets a value indicating whether the character is casting.</summary>
    public bool IsCasting => BitField.GetBit(Flags, 0);

    /// <summary>Gets a value indicating whether the cast can be interrupted.</summary>
    public bool Interruptible => BitField.GetBit(Flags, 1);
}

/// <summary>A status effect on a character.</summary>
[StructLayout(LayoutKind.Explicit, Size = 0x10)]
public struct Status
{
    /// <summary>The number of the status.</summary>
    [FieldOffset(0x00)]
    public ushort StatusId;

    /// <summary>The value of the status, which is the number of the stacks for a harm.</summary>
    [FieldOffset(0x02)]
    public ushort Param;

    /// <summary>The time the status is left.</summary>
    [FieldOffset(0x04)]
    public float RemainingTime;

    /// <summary>The object that gave the status.</summary>
    [FieldOffset(0x08)]
    public GameObjectId SourceObject;
}

/// <summary>The status effects on a character.</summary>
[StructLayout(LayoutKind.Explicit, Size = 0x3E0)]
public unsafe struct StatusManager
{
    /// <summary>The character the status effects belong to.</summary>
    [FieldOffset(0x00)]
    public Character* Owner;

    /// <summary>The status effects, of which the first ones are the ones that are active.</summary>
    [FieldOffset(0x08)]
    public Status* Status;

    /// <summary>The number of the status effects that are active.</summary>
    [FieldOffset(0x3D8)]
    public byte NumValidStatuses;
}

/// <summary>A character that can fight.</summary>
/// <remarks>
/// The structure continues the data of the character, so the numbers below are
/// the ones of the whole character.
/// </remarks>
[StructLayout(LayoutKind.Explicit, Size = 0x3810)]
public unsafe struct BattleChara
{
    /// <summary>The data of the character.</summary>
    [FieldOffset(0x0000)]
    public Character Character;

    /// <summary>The status effects of the character.</summary>
    [FieldOffset(0x23B0)]
    public StatusManager StatusManager;

    /// <summary>The state of the cast of the character.</summary>
    [FieldOffset(0x2790)]
    public CastInfo CastInfo;
}

/// <summary>The characters of the world.</summary>
[StructLayout(LayoutKind.Explicit, Size = 0x3A8)]
public unsafe struct CharacterManager
{
    /// <summary>The number of the places the manager keeps a character in.</summary>
    public const int BattleCharaCount = 100;

    /// <summary>The characters, of which the first one is the player.</summary>
    [FieldOffset(0x50)]
    public BattleChara** BattleCharas;

    /// <summary>Gets the manager of the characters.</summary>
    /// <returns>The manager, or <c>null</c> when the game is not ready.</returns>
    public static CharacterManager* Instance()
        => (CharacterManager*)GameFunctions.Get(GameAddresses.CharacterManager);
}

/// <summary>An entry of the table of the objects of the client.</summary>
[StructLayout(LayoutKind.Explicit, Size = 0x10)]
public unsafe struct BattleCharaEntry
{
    /// <summary>The character itself.</summary>
    [FieldOffset(0x00)]
    public BattleChara* BattleChara;

    /// <summary>The kind of the object.</summary>
    [FieldOffset(0x08)]
    public ObjectKind ObjectKind;

    /// <summary>The place of the object in the table.</summary>
    [FieldOffset(0x0C)]
    public byte Index;
}

/// <summary>The objects the client makes on its own.</summary>
[StructLayout(LayoutKind.Explicit, Size = 0xFA0)]
public unsafe struct ClientObjectManager
{
    /// <summary>The number of the places the manager keeps a character in.</summary>
    public const int BattleCharaEntryCount = 249;

    /// <summary>The characters, of which the first one is the player.</summary>
    [FieldOffset(0x10)]
    public BattleCharaEntry* BattleCharas;

    /// <summary>Gets the manager of the objects of the client.</summary>
    /// <returns>The manager, or <c>null</c> when the game is not ready.</returns>
    public static ClientObjectManager* Instance()
        => (ClientObjectManager*)GameFunctions.Get(GameAddresses.ClientObjectManager);
}
