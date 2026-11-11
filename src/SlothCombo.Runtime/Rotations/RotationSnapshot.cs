using System.Globalization;
using System.Reflection;
using SlothCombo.Abstractions;
using SlothCombo.Interop.Game;
using SlothCombo.Runtime.Gauges;
using SlothCombo.Runtime.Services;
using StatusRow = SlothCombo.Abstractions.GameData.StatusRow;

namespace SlothCombo.Runtime.Rotations;

internal sealed unsafe class RotationSnapshot
{
    private readonly IGameObject _player;
    private readonly IGameObject? _target;
    private readonly Dictionary<string, object> _values = new(StringComparer.Ordinal);
    private readonly Dictionary<string, IStatus[]> _statuses = new(StringComparer.Ordinal);

    public RotationSnapshot(IGameObject player, uint lastAction, float? countdown)
    {
        _player = player;
        _target = Service.TargetManager.Target;
        var manager = ActionManager.Instance();
        if (manager is null)
            throw new RotationUnavailableException("action-manager-unavailable");
        _values["player.level"] = (double)player.Level;
        _values["player.hpPercent"] = Hp(player);
        _values["player.mp"] = (double)player.CurrentMp;
        _values["player.moving"] = RotationNativeAdapter.Moving();
        _values["player.inCombat"] = Service.Condition[ConditionFlag.InCombat];
        _values["combo.action"] = (double)manager->Combo.Action;
        _values["combo.remaining"] = (double)Math.Max(0, manager->Combo.Timer);
        _values["lastAction"] = (double)lastAction;
        _values["party.count"] = (double)Math.Max(1, Math.Min(8, Service.PartyList.Length));
        var party = RotationNativeAdapter.Party(player).Where(member => !member.IsDead && member.MaxHp > 0).ToArray();
        if (party.Length > 0)
            _values["party.lowestHp"] = party.Min(Hp);
        if (countdown is float remaining)
            _values["countdown.remaining"] = (double)remaining;
    }

    public static double Hp(IGameObject member)
        => member.MaxHp == 0 ? throw new RotationUnavailableException("hp-unavailable")
            : Math.Clamp(100d * member.CurrentHp / member.MaxHp, 0, 100);

    public object Read(string path)
    {
        if (_values.TryGetValue(path, out var cached))
            return Checked(path, cached);
        object value;
        var parts = path.Split('.');
        if (path is "target.hpPercent" or "target.distance")
        {
            if (_target is null || _target.Address == IntPtr.Zero)
                throw new RotationUnavailableException("target-unavailable");
            value = path == "target.hpPercent" ? Hp(_target) : (double)Math.Max(0,
                System.Numerics.Vector3.Distance(_player.Position, _target.Position) -
                _player.HitboxRadius - _target.HitboxRadius);
            return _values[path] = Checked(path, value);
        }
        if (parts.Length == 3 && parts[0] == "action" && uint.TryParse(parts[1], out var action))
        {
            var state = RotationNativeAdapter.ActionState(action, _target?.GameObjectId ?? _player.GameObjectId);
            _values[$"action.{action}.ready"] = state.Ready;
            _values[$"action.{action}.cooldown"] = state.Cooldown;
            _values[$"action.{action}.charges"] = (double)state.Charges;
            return _values.TryGetValue(path, out var result) ? Checked(path, result)
                : throw new RotationUnavailableException($"unsupported-field:{path}");
        }
        if (parts.Length == 4 && parts[0] == "status" && parts[1] is "self" or "target" &&
            uint.TryParse(parts[2], out var statusId))
        {
            var owner = parts[1] == "self" ? _player : _target;
            if (owner is null)
                throw new RotationUnavailableException("status-target-unavailable");
            if (!_statuses.TryGetValue(parts[1], out var statuses))
                _statuses[parts[1]] = statuses = owner.StatusList.ToArray();
            var effects = statuses.Where(status => status.StatusId == statusId).ToArray();
            // Prefer the local player's copy when several sources apply the same status.
            var selected = effects.OrderByDescending(status => status.SourceId == _player.GameObjectId)
                .ThenByDescending(status => Math.Abs(status.RemainingTime)).FirstOrDefault();
            value = parts[3] switch
            {
                "remaining" => selected is null ? 0d : Math.Abs((double)selected.RemainingTime),
                "mine" => effects.Any(status => status.SourceId == _player.GameObjectId),
                "stacks" => Stacks(statusId, selected),
                _ => throw new RotationUnavailableException($"unsupported-field:{path}"),
            };
        }
        else if (parts.Length == 2 && parts[0] == "gauge")
            value = Gauge(parts[1]);
        else if (path == "target.enemyCount")
        {
            if (_target is null || !_target.IsHostile || _target.IsDead || !_target.IsTargetable ||
                !float.IsFinite(_target.Position.X) || !float.IsFinite(_target.Position.Y) ||
                !float.IsFinite(_target.Position.Z))
                throw new RotationUnavailableException("enemy-count-current-enemy-unavailable");
            value = (double)Service.ObjectTable.Objects
                .Where(enemy => enemy.IsHostile && !enemy.IsDead && enemy.IsTargetable && enemy.Address != IntPtr.Zero &&
                    System.Numerics.Vector3.Distance(enemy.Position, _target.Position) <= 5f)
                .Select(enemy => enemy.GameObjectId).Distinct().Count();
        }
        else
            throw new RotationUnavailableException($"unavailable-field:{path}");
        return _values[path] = Checked(path, value);
    }

    private static object Checked(string path, object value)
        => value is double number && !double.IsFinite(number)
            ? throw new RotationUnavailableException($"non-finite-field:{path}") : value;

    public void ObserveLastAction(uint action) => _values["lastAction"] = (double)action;

    private static double Stacks(uint id, IStatus? status)
    {
        if (status is null)
            return 0;
        if (!Service.DataManager.GetTable<StatusRow>().TryGetRow(id, out var row))
            throw new RotationUnavailableException($"status-sheet-unavailable:{id}");
        if (row.MaxStacks == 0)
            return 0;
        return status.Param <= row.MaxStacks ? status.Param
            : throw new RotationUnavailableException($"status-stack-encoding-unavailable:{id}");
    }

    private object Gauge(string name)
    {
        if (SlothCombo.Interop.GameFunctions.Get(SlothCombo.Interop.GameAddresses.JobGaugeManager) == 0 ||
            Service.JobGauges.Address == IntPtr.Zero)
            throw new RotationUnavailableException("gauge-unavailable");
        JobGaugeBase gauge = _player.ClassJob.RowId switch
        {
            19 => Service.JobGauges.Get<PLDGauge>(),
            20 => Service.JobGauges.Get<MNKGauge>(),
            21 => Service.JobGauges.Get<WARGauge>(),
            22 => Service.JobGauges.Get<DRGGauge>(),
            23 => Service.JobGauges.Get<BRDGauge>(),
            24 => Service.JobGauges.Get<WHMGauge>(),
            25 => Service.JobGauges.Get<BLMGauge>(),
            27 => Service.JobGauges.Get<SMNGauge>(),
            28 => Service.JobGauges.Get<SCHGauge>(),
            30 => Service.JobGauges.Get<NINGauge>(),
            31 => Service.JobGauges.Get<MCHGauge>(),
            32 => Service.JobGauges.Get<DRKGauge>(),
            33 => Service.JobGauges.Get<ASTGauge>(),
            34 => Service.JobGauges.Get<SAMGauge>(),
            35 => Service.JobGauges.Get<RDMGauge>(),
            37 => Service.JobGauges.Get<GNBGauge>(),
            38 => Service.JobGauges.Get<DNCGauge>(),
            39 => Service.JobGauges.Get<RPRGauge>(),
            40 => Service.JobGauges.Get<SGEGauge>(),
            41 => Service.JobGauges.Get<VPRGauge>(),
            42 => Service.JobGauges.Get<PCTGauge>(),
            _ => throw new RotationUnavailableException("gauge-job-unavailable"),
        };
        var property = gauge.GetType().GetProperty(name, BindingFlags.Public | BindingFlags.Instance);
        if (property is null || property.DeclaringType == typeof(JobGaugeBase) || property.GetIndexParameters().Length != 0)
            throw new RotationUnavailableException($"gauge-field-unavailable:{name}");
        var raw = property.GetValue(gauge);
        if (raw is bool flag)
            return flag;
        if (raw is byte or sbyte or short or ushort or int or uint or long or ulong or float or double or Enum)
            return Convert.ToDouble(raw, CultureInfo.InvariantCulture);
        throw new RotationUnavailableException($"gauge-type-unavailable:{name}");
    }
}
