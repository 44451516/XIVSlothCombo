using System;
using System.Linq;
using System.Numerics;
using SlothCombo.Interop.Game;
using SlothCombo.Interop.UI;
using SlothCombo.Abstractions;

namespace SlothCombo.Runtime.Services;

/// <summary>
/// The party state backed by the struct library.
/// </summary>
/// <remarks>
/// The game keeps the party in the group manager, where every slot holds a
/// snapshot of the member rather than a pointer to the live character. The
/// snapshot is what the party list addon reads from as well, so it is the
/// right source for the party state.
/// </remarks>
internal sealed unsafe class PartyService : IPartyService
{
    /// <summary>The number of members a single group can hold.</summary>
    private const int GroupLength = 8;

    private readonly IObjectTable _objectTable;

    /// <summary>Creates the party service over the given object table.</summary>
    public PartyService(IObjectTable objectTable) => _objectTable = objectTable;

    /// <inheritdoc/>
    public long PartyId
    {
        get
        {
            var group = GetGroup();
            return group is null ? 0 : group->PartyId;
        }
    }

    /// <inheritdoc/>
    public int Length
    {
        get
        {
            var group = GetGroup();
            return group is null ? 0 : group->MemberCount;
        }
    }

    /// <inheritdoc/>
    public bool IsAlliance
    {
        get
        {
            var group = GetGroup();
            return group is not null && group->IsAlliance;
        }
    }

    /// <inheritdoc/>
    public IPartyMember? GetSlot(int index)
    {
        var group = GetGroup();
        if (group is null || index < 0 || index >= group->MemberCount)
        {
            return null;
        }

        // An alliance shares one group per alliance team, so the full roster
        // lives in the alliance list once the group outgrows a single party.
        var member = group->MemberCount > GroupLength
            ? group->AllianceMembers[index]
            : group->PartyMembers[index];

        return new PartyMemberAdapter(member, _objectTable);
    }

    /// <summary>Gets the group of the player, or <c>null</c> when there is none.</summary>
    private static Group* GetGroup()
    {
        var manager = GroupManager.Instance();
        return manager is null ? null : &manager->MainGroup;
    }
}

/// <summary>
/// A party member backed by the snapshot the game keeps in the group manager.
/// </summary>
internal sealed class PartyMemberAdapter : IPartyMember
{
    private readonly PartyMember _member;
    private readonly IObjectTable _objectTable;

    /// <summary>Wraps the given party member snapshot.</summary>
    public PartyMemberAdapter(PartyMember member, IObjectTable objectTable)
    {
        _member = member;
        _objectTable = objectTable;
    }

    /// <inheritdoc/>
    public uint EntityId => _member.EntityId;

    /// <inheritdoc/>
    public ulong ContentId => _member.ContentId;

    /// <inheritdoc/>
    public string Name => _member.NameString ?? string.Empty;

    /// <inheritdoc/>
    public byte Level => _member.Level;

    /// <inheritdoc/>
    public uint ClassJobRowId => _member.ClassJob;

    /// <inheritdoc/>
    public uint CurrentHp => _member.CurrentHP;

    /// <inheritdoc/>
    public uint MaxHp => _member.MaxHP;

    /// <inheritdoc/>
    public ushort CurrentMp => _member.CurrentMP;

    /// <inheritdoc/>
    public ushort MaxMp => _member.MaxMP;

    /// <inheritdoc/>
    public Vector3 Position =>
        new(_member.Position.X, _member.Position.Y, _member.Position.Z);

    /// <inheritdoc/>
    public IGameObject? GameObject
    {
        get
        {
            var entityId = _member.EntityId;
            if (entityId == 0)
            {
                return null;
            }

            return _objectTable.Objects.FirstOrDefault(x => x.GameObjectId == entityId);
        }
    }
}

/// <summary>
/// The high level client state backed by the struct library.
/// </summary>
internal sealed unsafe class ClientStateService : IClientStateService
{
    private readonly IObjectTable _objectTable;
    private uint _territoryType;

    /// <summary>Creates the client state over the given object table.</summary>
    /// <param name="objectTable">The object table of the game.</param>
    public ClientStateService(IObjectTable objectTable) => _objectTable = objectTable;

    /// <inheritdoc/>
    public event Action<uint>? TerritoryChanged;

    /// <inheritdoc/>
    public bool IsPvP => GameMain.IsInPvPArea();

    /// <inheritdoc/>
    public uint TerritoryType
    {
        get
        {
            var gameMain = GameMain.Instance();
            return gameMain is null ? 0 : gameMain->CurrentTerritoryTypeId;
        }
    }

    /// <inheritdoc/>
    public IGameObject? LocalPlayer => _objectTable.LocalPlayer;

    /// <inheritdoc/>
    public ClientLanguage ClientLanguage
    {
        get
        {
            var framework = Framework.Instance();
            if (framework is null)
            {
                return ClientLanguage.Unknown;
            }

            // The client language byte uses the same numbering as the social
            // list language filter of the game: japanese, english, german,
            // french, chinese and korean.
            return framework->ClientLanguage switch
            {
                0 => ClientLanguage.Ja,
                1 => ClientLanguage.En,
                2 => ClientLanguage.De,
                3 => ClientLanguage.Fr,
                4 or 5 => ClientLanguage.Zh,
                6 => ClientLanguage.Ko,
                _ => ClientLanguage.Unknown,
            };
        }
    }

    /// <summary>
    /// Checks the territory of the player and raises the change event.
    /// </summary>
    /// <remarks>
    /// This is called from the update loop of the runtime, which is the only
    /// place where the game tells the plugin that the zone changed.
    /// </remarks>
    public void UpdateTerritoryType()
    {
        var territoryType = TerritoryType;
        if (territoryType == _territoryType)
        {
            return;
        }

        _territoryType = territoryType;
        TerritoryChanged?.Invoke(territoryType);
    }
}

/// <summary>
/// The party state and the client state combined into the service facade.
/// </summary>
internal sealed class RuntimeGameServices : IGameServices
{
    /// <summary>Creates the facade over the given parts.</summary>
    public RuntimeGameServices(
        IObjectTable objectTable,
        IConditionService condition,
        ITargetService targets,
        IPartyService party,
        IDataManager data,
        IChatService chat,
        IComboLogger log,
        IClientStateService clientState)
    {
        ObjectTable = objectTable;
        Condition = condition;
        Targets = targets;
        Party = party;
        Data = data;
        Chat = chat;
        Log = log;
        ClientState = clientState;
    }

    /// <inheritdoc/>
    public IObjectTable ObjectTable { get; }

    /// <inheritdoc/>
    public IConditionService Condition { get; }

    /// <inheritdoc/>
    public ITargetService Targets { get; }

    /// <inheritdoc/>
    public IPartyService Party { get; }

    /// <inheritdoc/>
    public IDataManager Data { get; }

    /// <inheritdoc/>
    public IChatService Chat { get; }

    /// <inheritdoc/>
    public IComboLogger Log { get; }

    /// <inheritdoc/>
    public IClientStateService ClientState { get; }
}
