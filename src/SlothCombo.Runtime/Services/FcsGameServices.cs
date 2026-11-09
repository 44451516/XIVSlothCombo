using System;
using System.Collections.Generic;
using System.Numerics;
using FFXIVClientStructs.FFXIV.Client.Game;
using FFXIVClientStructs.FFXIV.Client.Game.Character;
using FFXIVClientStructs.FFXIV.Client.Game.Control;
using FFXIVClientStructs.FFXIV.Client.Game.Object;
using SlothCombo.Abstractions;
using FcsGameObject = FFXIVClientStructs.FFXIV.Client.Game.Object.GameObject;
using FcsObjectKind = FFXIVClientStructs.FFXIV.Client.Game.Object.ObjectKind;
using FcsTargetableFlags = FFXIVClientStructs.FFXIV.Client.Game.Object.ObjectTargetableFlags;
using FcsBattleNpcSubKind = FFXIVClientStructs.FFXIV.Client.Game.Object.BattleNpcSubKind;

namespace SlothCombo.Runtime.Services;

/// <summary>
/// Looks up game objects by their entity id.
/// </summary>
/// <remarks>
/// The object table hands out live wrappers, but a few places need to find an
/// object that is only known by its id, for example the target of a character.
/// This helper keeps that lookup in one place so the wrappers do not have to
/// know about each other.
/// </remarks>
internal static unsafe class GameObjectLookup
{
    /// <summary>Finds the object with the given entity id, or <c>null</c>.</summary>
    public static IGameObject? Find(ulong entityId)
    {
        if (entityId == 0)
        {
            return null;
        }

        var manager = ClientObjectManager.Instance();
        if (manager is null)
        {
            return null;
        }

        foreach (var entry in manager->BattleCharas)
        {
            var chara = entry.BattleChara;
            if (chara is null)
            {
                continue;
            }

            if (chara->Character.GameObject.EntityId == entityId)
            {
                return new GameObjectAdapter(chara);
            }
        }

        return null;
    }
}

/// <summary>
/// Wraps a native battle character so the combo logic can stay free of struct library types.
/// </summary>
/// <remarks>
/// The struct library splits the character data across two levels: the shared
/// game object data lives on <c>GameObject</c> while the character specific
/// data (level, job, resources, cast state) lives on <c>Character</c>. A battle
/// character embeds both, so the adapter reads through both.
/// </remarks>
internal sealed unsafe class GameObjectAdapter : IBattleChara
{
    private readonly BattleChara* _pointer;

    /// <summary>Wraps the given native battle character.</summary>
    public GameObjectAdapter(BattleChara* pointer) => _pointer = pointer;

    private Character Character => _pointer->Character;

    private FcsGameObject GameObject => _pointer->Character.GameObject;

    /// <inheritdoc/>
    public ulong GameObjectId => GameObject.EntityId;

    /// <inheritdoc/>
    public uint ObjectId => GameObject.EntityId;

    /// <inheritdoc/>
    public uint BaseId => GameObject.BaseId;

    /// <inheritdoc/>
    public byte Level => Character.Level;

    /// <inheritdoc/>
    public RowRef ClassJob => new(Character.ClassJob);

    /// <inheritdoc/>
    public string Name => GameObject.NameString ?? string.Empty;

    /// <inheritdoc/>
    public Vector3 Position =>
        new(GameObject.Position.X, GameObject.Position.Y, GameObject.Position.Z);

    /// <inheritdoc/>
    public float Rotation => GameObject.Rotation;

    /// <inheritdoc/>
    public byte YalmDistanceX => GameObject.CurrentDistance;

    /// <inheritdoc/>
    public ulong TargetObjectId => Character.TargetId.Id;

    /// <inheritdoc/>
    public IGameObject? TargetObject => GameObjectLookup.Find(TargetObjectId);

    /// <inheritdoc/>
    public uint CurrentHp => Character.Health;

    /// <inheritdoc/>
    public uint MaxHp => Character.MaxHealth;

    /// <inheritdoc/>
    public uint CurrentMp => Character.Mana;

    /// <inheritdoc/>
    public uint MaxMp => Character.MaxMana;

    /// <inheritdoc/>
    public bool IsCasting => _pointer->CastInfo.IsCasting;

    /// <inheritdoc/>
    public bool IsCastInterruptible => _pointer->CastInfo.Interruptible;

    /// <inheritdoc/>
    public uint CastActionId => _pointer->CastInfo.ActionId;

    /// <inheritdoc/>
    public float CurrentCastTime => _pointer->CastInfo.CurrentCastTime;

    /// <inheritdoc/>
    public float TotalCastTime => _pointer->CastInfo.TotalCastTime;

    /// <inheritdoc/>
    public bool IsDead => Character.Health == 0;

    /// <inheritdoc/>
    public bool IsTargetable =>
        (GameObject.TargetableStatus & FcsTargetableFlags.IsTargetable) != 0;

    /// <inheritdoc/>
    public bool IsHostile => Character.IsHostile;

    /// <inheritdoc/>
    public float HitboxRadius => GameObject.HitboxRadius;

    /// <inheritdoc/>
    public IReadOnlyList<IStatus> StatusList
    {
        get
        {
            var statusManager = _pointer->StatusManager;
            var count = statusManager.NumValidStatuses;
            var result = new List<IStatus>(count);
            for (var index = 0; index < count; index++)
            {
                result.Add(new StatusAdapter(statusManager.Status[index]));
            }

            return result;
        }
    }

    /// <inheritdoc/>
    public uint CurrentMountId => Character.Mount.MountId;

    /// <inheritdoc/>
    public IGameObject? CurrentMount
    {
        get
        {
            var mountedIds = Character.Mount.MountedEntityIds;
            foreach (var entityId in mountedIds)
            {
                var found = GameObjectLookup.Find(entityId);
                if (found is not null)
                {
                    return found;
                }
            }

            return null;
        }
    }

    /// <inheritdoc/>
    public bool IsPlayerCharacter => false;

    /// <inheritdoc/>
    public SlothCombo.Abstractions.ObjectKind ObjectKind => (SlothCombo.Abstractions.ObjectKind)(int)GameObject.ObjectKind;
    /// <inheritdoc/>
    public IntPtr Address => (IntPtr)_pointer;
}

/// <summary>
/// A status effect backed by the native status of a character.
/// </summary>
internal readonly struct StatusAdapter : IStatus
{
    private readonly Status _status;

    /// <summary>Wraps the given native status.</summary>
    public StatusAdapter(Status status) => _status = status;

    /// <inheritdoc/>
    public uint StatusId => _status.StatusId;

    /// <inheritdoc/>
    public float RemainingTime => _status.RemainingTime;

    /// <inheritdoc/>
    public ushort Param => _status.Param;

    /// <inheritdoc/>
    public ulong SourceId => _status.SourceObject.Id;

    /// <inheritdoc/>
    public string Name => StatusSheetLookup.GetName(_status.StatusId);
}

/// <summary>
/// An enemy of the player.
/// </summary>
/// <remarks>
/// The combo logic tells the enemies apart from the other objects of the
/// world, so the enemies get their own type.
/// </remarks>
internal sealed unsafe class BattleNpcAdapter : IBattleNpc
{
    private readonly BattleChara* _pointer;

    /// <summary>Wraps the given native battle character.</summary>
    public BattleNpcAdapter(BattleChara* pointer) => _pointer = pointer;

    private Character Character => _pointer->Character;

    private FcsGameObject GameObject => _pointer->Character.GameObject;

    /// <inheritdoc/>
    public ulong GameObjectId => GameObject.EntityId;

    /// <inheritdoc/>
    public uint ObjectId => GameObject.EntityId;

    /// <inheritdoc/>
    public uint BaseId => GameObject.BaseId;

    /// <inheritdoc/>
    public string Name => GameObject.NameString ?? string.Empty;

    /// <inheritdoc/>
    public byte Level => Character.Level;

    /// <inheritdoc/>
    public RowRef ClassJob => new(Character.ClassJob);

    /// <inheritdoc/>
    public Vector3 Position =>
        new(GameObject.Position.X, GameObject.Position.Y, GameObject.Position.Z);

    /// <inheritdoc/>
    public float Rotation => GameObject.Rotation;

    /// <inheritdoc/>
    public byte YalmDistanceX => GameObject.CurrentDistance;

    /// <inheritdoc/>
    public ulong TargetObjectId => Character.TargetId.Id;

    /// <inheritdoc/>
    public IGameObject? TargetObject => GameObjectLookup.Find(TargetObjectId);

    /// <inheritdoc/>
    public uint CurrentHp => Character.Health;

    /// <inheritdoc/>
    public uint MaxHp => Character.MaxHealth;

    /// <inheritdoc/>
    public uint CurrentMp => Character.Mana;

    /// <inheritdoc/>
    public uint MaxMp => Character.MaxMana;

    /// <inheritdoc/>
    public bool IsCasting => _pointer->CastInfo.IsCasting;

    /// <inheritdoc/>
    public bool IsCastInterruptible => _pointer->CastInfo.Interruptible;

    /// <inheritdoc/>
    public uint CastActionId => _pointer->CastInfo.ActionId;

    /// <inheritdoc/>
    public float CurrentCastTime => _pointer->CastInfo.CurrentCastTime;

    /// <inheritdoc/>
    public float TotalCastTime => _pointer->CastInfo.TotalCastTime;

    /// <inheritdoc/>
    public bool IsDead => Character.Health == 0;

    /// <inheritdoc/>
    public bool IsTargetable =>
        (GameObject.TargetableStatus & FcsTargetableFlags.IsTargetable) != 0;

    /// <inheritdoc/>
    public bool IsHostile => Character.IsHostile;

    /// <inheritdoc/>
    public float HitboxRadius => GameObject.HitboxRadius;

    /// <inheritdoc/>
    public IReadOnlyList<IStatus> StatusList => new GameObjectAdapter(_pointer).StatusList;

    /// <inheritdoc/>
    public uint CurrentMountId => Character.Mount.MountId;

    /// <inheritdoc/>
    public IGameObject? CurrentMount => new GameObjectAdapter(_pointer).CurrentMount;

    /// <inheritdoc/>
    public bool IsPlayerCharacter => false;

    /// <inheritdoc/>
    public SlothCombo.Abstractions.ObjectKind ObjectKind =>
        (SlothCombo.Abstractions.ObjectKind)(int)GameObject.ObjectKind;

    /// <inheritdoc/>
    public uint NameId => Character.NameId;

    /// <inheritdoc/>
    public SlothCombo.Abstractions.BattleNpcSubKind BattleNpcKind =>
        (SlothCombo.Abstractions.BattleNpcSubKind)(int)GameObject.BattleNpcSubKind;

    /// <inheritdoc/>
    public IntPtr Address => (IntPtr)_pointer;
}

/// <summary>
/// The object table backed by the struct library.
/// </summary>
internal sealed unsafe class ObjectTableService : IObjectTable
{
    /// <inheritdoc/>
    public IGameObject? LocalPlayer
    {
        get
        {
            var manager = ClientObjectManager.Instance();
            if (manager is null)
            {
                return null;
            }

            // The local player is the first entry of the battle character array.
            var battleCharas = manager->BattleCharas;
            if (battleCharas.Length == 0)
            {
                return null;
            }

            var player = battleCharas[0].BattleChara;
            return player is null ? null : new GameObjectAdapter(player);
        }
    }

    /// <inheritdoc/>
    public IEnumerable<IGameObject> Objects
    {
        get
        {
            var manager = ClientObjectManager.Instance();
            if (manager is null)
            {
                return [];
            }

            var result = new List<IGameObject>();
            foreach (var entry in manager->BattleCharas)
            {
                var pointer = entry.BattleChara;
                if (pointer is not null)
                {
                    result.Add(new GameObjectAdapter(pointer));
                }
            }

            return result;
        }
    }

    /// <inheritdoc/>
    public IEnumerator<IGameObject> GetEnumerator() => Objects.GetEnumerator();

    /// <inheritdoc/>
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        => Objects.GetEnumerator();
}

/// <summary>
/// The condition flags backed by the struct library.
/// </summary>
/// <remarks>
/// The struct library models every condition as its own boolean field rather
/// than as a bit mask, so the lookup is a direct field read.
/// </remarks>
internal sealed unsafe class ConditionService : IConditionService, IDisposable
{
    /// <summary>The conditions the runtime keeps an eye on.</summary>
    private static readonly ConditionFlag[] TrackedFlags =
    [
        ConditionFlag.InCombat,
        ConditionFlag.BoundByDuty,
        ConditionFlag.BetweenAreas,
        ConditionFlag.Occupied,
        ConditionFlag.Occupied30,
        ConditionFlag.Occupied33,
        ConditionFlag.Occupied38,
        ConditionFlag.Occupied39,
        ConditionFlag.OccupiedInCutSceneEvent,
        ConditionFlag.OccupiedInEvent,
        ConditionFlag.OccupiedInQuestEvent,
        ConditionFlag.OccupiedSummoningBell,
        ConditionFlag.Diving,
        ConditionFlag.Fishing,
        ConditionFlag.Unconscious,
    ];

    private readonly Dictionary<ConditionFlag, bool> _lastValues = [];
    private readonly IFramework _framework;
    private readonly FrameworkUpdateDelegate _onUpdate;

    /// <summary>Creates the condition service over the given update loop.</summary>
    /// <param name="framework">The update loop of the game.</param>
    public ConditionService(IFramework framework)
    {
        ArgumentNullException.ThrowIfNull(framework);

        _framework = framework;
        _onUpdate = _ => Update();

        // The game does not raise an event when a condition changes, so the
        // change is detected by comparing the state on every update.
        _framework.Update += _onUpdate;
    }

    /// <summary>Stops listening to the update loop.</summary>
    public void Dispose() => _framework.Update -= _onUpdate;

    /// <inheritdoc/>
    public event Action<ConditionFlag, bool>? ConditionChange;

    /// <inheritdoc/>
    public bool this[ConditionFlag flag]
    {
        get
        {
            var conditions = FFXIVClientStructs.FFXIV.Client.Game.Conditions.Instance();
            if (conditions is null)
            {
                return false;
            }

            return flag switch
            {
                ConditionFlag.InCombat => conditions->InCombat,
                ConditionFlag.BoundByDuty => conditions->BoundByDuty,
                ConditionFlag.BetweenAreas => conditions->BetweenAreas,
                ConditionFlag.Occupied => conditions->Occupied,
                ConditionFlag.Occupied30 => conditions->Occupied30,
                ConditionFlag.Occupied33 => conditions->Occupied33,
                ConditionFlag.Occupied38 => conditions->Occupied38,
                ConditionFlag.Occupied39 => conditions->Occupied39,
                ConditionFlag.OccupiedInCutSceneEvent => conditions->OccupiedInCutSceneEvent,
                ConditionFlag.OccupiedInEvent => conditions->OccupiedInEvent,
                ConditionFlag.OccupiedInQuestEvent => conditions->OccupiedInQuestEvent,
                ConditionFlag.OccupiedSummoningBell => conditions->OccupiedSummoningBell,
                ConditionFlag.Diving => conditions->Diving,
                ConditionFlag.Fishing => conditions->Fishing,
                ConditionFlag.Unconscious => conditions->Dead,
                _ => false,
            };
        }
    }

    /// <summary>Raises the change event for every condition that changed.</summary>
    private void Update()
    {
        foreach (var flag in TrackedFlags)
        {
            var value = this[flag];
            if (_lastValues.TryGetValue(flag, out var last) && last != value)
            {
                ConditionChange?.Invoke(flag, value);
            }

            _lastValues[flag] = value;
        }
    }
}

/// <summary>
/// The target information backed by the struct library.
/// </summary>
internal sealed unsafe class TargetService : ITargetService
{
    /// <inheritdoc/>
    public IGameObject? Target
    {
        get => Wrap(TargetSystem.Instance()->GetTargetObject());
        set
        {
            if (value is null)
            {
                return;
            }

            TargetSystem.Instance()->SetHardTarget((FcsGameObject*)value.Address, false, false, 0);
        }
    }

    /// <inheritdoc/>
    public IGameObject? SoftTarget => Wrap(TargetSystem.Instance()->GetSoftTarget());

    /// <inheritdoc/>
    public IGameObject? FocusTarget => Wrap(TargetSystem.Instance()->GetHardTarget());

    /// <inheritdoc/>
    public IGameObject? MouseOverTarget => Wrap(TargetSystem.Instance()->GetMouseOverObject(0, 0));

    private static IGameObject? Wrap(FcsGameObject* pointer)
    {
        if (pointer is null)
        {
            return null;
        }

        // The target system hands out plain game objects, but the combo logic
        // expects the richer battle character view. A battle character starts
        // with the shared game object data, so the pointer can be reinterpreted
        // when the object really is a character.
        if (pointer->ObjectKind is FcsObjectKind.Pc or FcsObjectKind.BattleNpc or FcsObjectKind.EventNpc)
        {
            return new GameObjectAdapter((BattleChara*)pointer);
        }

        return null;
    }
}
