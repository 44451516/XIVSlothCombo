using System;
using System.Runtime.InteropServices;

namespace SlothCombo.Interop.Game;

/// <summary>A point in the world.</summary>
/// <remarks>
/// The game keeps four bytes behind the three numbers of the point, so the
/// structure is bigger than the three numbers it holds.
/// </remarks>
[StructLayout(LayoutKind.Explicit, Size = 0x10)]
public struct GameVector3
{
    /// <summary>The distance along the first axis.</summary>
    [FieldOffset(0x00)]
    public float X;

    /// <summary>The distance along the second axis.</summary>
    [FieldOffset(0x04)]
    public float Y;

    /// <summary>The distance along the third axis.</summary>
    [FieldOffset(0x08)]
    public float Z;
}

/// <summary>The kind of an object of the world.</summary>
public enum ObjectKind : byte
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

    /// <summary>An aetheryte.</summary>
    Aetheryte = 5,

    /// <summary>A gathering point.</summary>
    GatheringPoint = 6,

    /// <summary>An object of an event.</summary>
    EventObj = 7,

    /// <summary>A mount.</summary>
    Mount = 8,

    /// <summary>A companion or pet.</summary>
    Companion = 9,

    /// <summary>A retainer.</summary>
    Retainer = 10,

    /// <summary>An object of an area.</summary>
    AreaObject = 11,

    /// <summary>An object of a housing event.</summary>
    HousingEventObject = 12,

    /// <summary>An object of a cutscene.</summary>
    Cutscene = 13,

    /// <summary>An object of a reaction event.</summary>
    ReactionEventObject = 14,

    /// <summary>An ornament.</summary>
    Ornament = 15,

    /// <summary>A stand of cards.</summary>
    CardStand = 16,

    /// <summary>A mount that is followed.</summary>
    FollowMount = 17,
}

/// <summary>The kind of a part of an enemy.</summary>
public enum BattleNpcSubKind : byte
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

/// <summary>What the player may do with an object.</summary>
[Flags]
public enum ObjectTargetableFlags : byte
{
    /// <summary>Nothing.</summary>
    Unk0 = 1 << 0,

    /// <summary>The object may be targeted.</summary>
    IsTargetable = 1 << 1,

    /// <summary>Nothing.</summary>
    Unk1 = 1 << 2,

    /// <summary>The object is ready to be drawn.</summary>
    ReadyToDraw = 1 << 6,
}

/// <summary>The identifier of an object of the world.</summary>
/// <remarks>
/// The identifier is made of two parts: the number that tells which object it
/// is, and the kind of that number. A companion of the player, for example,
/// is numbered by the number of its owner.
/// </remarks>
[StructLayout(LayoutKind.Explicit, Size = 0x08)]
public struct GameObjectId
{
    /// <summary>The number of the object as a wide number.</summary>
    [FieldOffset(0x00)]
    public ulong Id;

    /// <summary>The number of the object.</summary>
    [FieldOffset(0x00)]
    public uint ObjectId;

    /// <summary>The kind of the number.</summary>
    [FieldOffset(0x04)]
    public byte Type;

    /// <summary>Gets the identifier as a wide number.</summary>
    public static implicit operator ulong(GameObjectId id) => id.Id;

    /// <summary>Gets the identifier of the given wide number.</summary>
    public static implicit operator GameObjectId(ulong id) => new() { Id = id };

    /// <summary>Gets a value indicating whether the two identifiers are the same.</summary>
    public static bool operator ==(GameObjectId left, GameObjectId right) => left.Id == right.Id;

    /// <summary>Gets a value indicating whether the two identifiers are not the same.</summary>
    public static bool operator !=(GameObjectId left, GameObjectId right) => left.Id != right.Id;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is GameObjectId other && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => Id.GetHashCode();
}

/// <summary>The virtual functions of an object of the world.</summary>
[StructLayout(LayoutKind.Explicit)]
public unsafe struct GameObjectVirtualTable
{
    /// <summary>The function that frees the object.</summary>
    [FieldOffset(0x00)]
    public delegate* unmanaged<GameObject*, GameObject*> Dtor;

    /// <summary>The function that gives the identifier of the object.</summary>
    [FieldOffset(0x08)]
    public delegate* unmanaged<GameObject*, GameObjectId> GetGameObjectId;
}

/// <summary>An object of the world.</summary>
/// <remarks>
/// <para>
/// Every object of the world starts with this structure. The objects that
/// fight add their own data behind it, so a pointer to an object of the world
/// can be read as a pointer to a character when the kind of the object says so.
/// </para>
/// <para>
/// The name of the object is stored as a run of bytes that end with a zero.
/// It is read here as a string, which stops at that zero.
/// </para>
/// </remarks>
[StructLayout(LayoutKind.Explicit, Size = 0x1A0)]
public unsafe struct GameObject
{
    /// <summary>The table of the virtual functions of the object.</summary>
    [FieldOffset(0x00)]
    public GameObjectVirtualTable* VirtualTable;

    /// <summary>The number of bytes the name of the object takes.</summary>
    private const int NameByteCount = 64;

    /// <summary>The name of the object, as a run of bytes that end with a zero.</summary>
    [FieldOffset(0x30)]
    public fixed byte NameBytes[NameByteCount];

    /// <summary>The number that tells which object this is.</summary>
    [FieldOffset(0x78)]
    public uint EntityId;

    /// <summary>The number that tells what kind of object this is.</summary>
    [FieldOffset(0x84)]
    public uint BaseId;

    /// <summary>The kind of the object.</summary>
    [FieldOffset(0x90)]
    public ObjectKind ObjectKind;

    /// <summary>The kind of the enemy, when the object is an enemy.</summary>
    [FieldOffset(0x91)]
    public BattleNpcSubKind BattleNpcSubKind;

    /// <summary>The distance from the player to the object.</summary>
    [FieldOffset(0x94)]
    public byte CurrentDistance;

    /// <summary>What the player may do with the object.</summary>
    [FieldOffset(0x9A)]
    public ObjectTargetableFlags TargetableStatus;

    /// <summary>The place of the object in the world.</summary>
    [FieldOffset(0xB0)]
    public GameVector3 Position;

    /// <summary>The turn of the object.</summary>
    [FieldOffset(0xC0)]
    public float Rotation;

    /// <summary>The radius of the box around the object.</summary>
    [FieldOffset(0xD0)]
    public float HitboxRadius;

    /// <summary>Gets the name of the object.</summary>
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

    /// <summary>
    /// Gets the identifier of the object.
    /// </summary>
    /// <remarks>
    /// The identifier is not simply the number of the object: a companion is
    /// numbered by the number of its owner. The game keeps the identifier in
    /// the virtual table of the object, so it is asked for there.
    /// </remarks>
    /// <returns>The identifier of the object.</returns>
    public GameObjectId GetGameObjectId()
    {
        if (VirtualTable is null)
        {
            return new GameObjectId { ObjectId = EntityId };
        }

        return VirtualTable->GetGameObjectId(StructPtr.Of(ref this));
    }
}
