using System.Numerics;
using System.Text.Json.Nodes;
using FFXIVClientStructs.FFXIV.Client.Game;
using FFXIVClientStructs.FFXIV.Client.Game.Character;
using FFXIVClientStructs.FFXIV.Client.Game.Control;
using FFXIVClientStructs.FFXIV.Client.Game.Group;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using FFXIVClientStructs.FFXIV.Client.UI.Misc;
using FFXIVClientStructs.FFXIV.Common.Component.BGCollision;
using SlothCombo.Abstractions;
using SlothCombo.Runtime.Services;
using NativeObject = FFXIVClientStructs.FFXIV.Client.Game.Object.GameObject;
using SheetAction = Lumina.Excel.Sheets.Action;
using GameFramework = FFXIVClientStructs.FFXIV.Client.System.Framework.Framework;

namespace SlothCombo.Runtime.Rotations;

internal sealed record RotationSubmission(uint RequestedId, uint AdjustedId, ulong TargetId, Vector3? Ground);

internal static unsafe class RotationNativeAdapter
{
    public static bool Available =>
        ActionManager.Addresses.Instance.Value != 0 &&
        ActionManager.Addresses.GetAdjustedActionId.Value != 0 &&
        ActionManager.Addresses.GetActionStatus.Value != 0 &&
        ActionManager.Addresses.UseAction.Value != 0 &&
        ActionManager.Addresses.GetActionInRangeOrLoS.Value != 0 &&
        ActionManager.Addresses.CanUseActionOnTarget.Value != 0 &&
        ActionManager.Addresses.GetAdjustedCastTime.Value != 0 &&
        ActionManager.Addresses.GetCurrentCharges.Value != 0 &&
        ActionManager.Addresses.GetRecastTime.Value != 0 &&
        ActionManager.Addresses.GetRecastTimeElapsed.Value != 0 &&
        CharacterManager.Addresses.Instance.Value != 0 &&
        Conditions.Addresses.Instance.Value != 0 &&
        TargetSystem.Addresses.Instance.Value != 0 &&
        TargetSystem.Addresses.GetTargetObject.Value != 0 &&
        GroupManager.Addresses.Instance.Value != 0 &&
        GameMain.Addresses.Instance.Value != 0 &&
        GameMain.Addresses.IsInPvPArea.Value != 0;

    public static bool AgentsAvailable =>
        GameFramework.Addresses.Instance.Value != 0 &&
        GameFramework.Addresses.GetUIModule.Value != 0 &&
        AgentModule.Addresses.GetAgentByInternalId.Value != 0;

    public static bool GroundAvailable => Available &&
        ActionManager.Addresses.UseActionLocation.Value != 0 &&
        BGCollisionModule.Addresses.RaycastMaterialFilter.Value != 0 &&
        GameFramework.Addresses.Instance.Value != 0;

    public static bool PartySlotsAvailable => AgentsAvailable &&
        PronounModule.Addresses.ResolvePlaceholder.Value != 0;

    public static string? UnsafeReason(IGameObject? player)
    {
        if (!Available)
            return "native-action-api-unavailable";
        if (player is null || player.Address == IntPtr.Zero)
            return "player-unavailable";
        var game = GameMain.Instance();
        if (game is null || game->CurrentTerritoryTypeId == 0)
            return "territory-unavailable";
        if (TargetSystem.Instance() is null || GroupManager.Instance() is null)
            return "target-or-party-manager-unavailable";
        if (Conditions.Addresses.Instance.Value == 0)
            return "native-conditions-unavailable";
        var flags = Conditions.Instance();
        if (flags is null)
            return "conditions-unavailable";
        if (player.IsDead || flags->Dead)
            return "player-dead";
        if (flags->BetweenAreas || flags->BetweenAreas51 || flags->LoggingOut)
            return "zone-or-logout";
        if (flags->Occupied || flags->Occupied30 || flags->OccupiedInEvent || flags->OccupiedInQuestEvent ||
            flags->Occupied33 || flags->OccupiedInCutSceneEvent || flags->Occupied38 || flags->Occupied39 ||
            flags->OccupiedSummoningBell || flags->WatchingCutscene || flags->WatchingCutscene78 ||
            flags->Crafting || flags->Gathering || flags->Mounted || flags->RidingPillion || flags->InFlight ||
            flags->BeingMoved || flags->RolePlaying || flags->PilotingMech || flags->DutyRecorderPlayback)
            return "player-occupied";
        if (Service.ClientState.IsPvP)
            return "pvp-unavailable";
        return ActionManager.Instance() is null ? "action-manager-unavailable" : null;
    }

    public static bool Busy(IGameObject player)
    {
        var manager = ActionManager.Instance();
        if (manager is not null && (!float.IsFinite(manager->AnimationLock) ||
            !float.IsFinite(manager->CastTimeTotal) || manager->AnimationLock < 0 || manager->CastTimeTotal < 0))
            throw new RotationUnavailableException("invalid-native-action-state");
        return manager is null || player.IsCasting || manager->CastTimeTotal > 0 ||
            manager->AnimationLock > 0.01f || manager->ActionQueued || manager->AreaTargetingActionId != 0;
    }

    public static bool Moving()
    {
        var agent = (AgentMap*)GetAgent(AgentId.Map);
        if (agent is null)
            throw new RotationUnavailableException("movement-unavailable");
        return agent->IsPlayerMoving;
    }

    public static float? Countdown()
    {
        var agent = (AgentCountDownSettingDialog*)GetAgent(AgentId.CountDownSettingDialog);
        if (agent is null)
            throw new RotationUnavailableException("countdown-unavailable");
        if (!agent->Active)
            return null;
        if (!float.IsFinite(agent->TimeRemaining) || agent->TimeRemaining is < 0 or > 120)
            throw new RotationUnavailableException("invalid-countdown");
        return agent->TimeRemaining;
    }

    public static (uint Id, double Cooldown, uint Charges, bool Ready) ActionState(uint requested, ulong target)
    {
        if (!Available)
            throw new RotationUnavailableException("native-action-api-unavailable");
        var manager = ActionManager.Instance();
        if (manager is null)
            throw new RotationUnavailableException("action-manager-unavailable");
        var id = manager->GetAdjustedActionId(requested);
        if (id == 0 || !Service.DataManager.GetExcelSheet<SheetAction>().TryGetRow(id, out _))
            throw new RotationUnavailableException($"action-unavailable:{requested}");
        var total = manager->GetRecastTime(ActionType.Action, id);
        var elapsed = manager->GetRecastTimeElapsed(ActionType.Action, id);
        if (!float.IsFinite(total) || !float.IsFinite(elapsed))
            throw new RotationUnavailableException("invalid-cooldown");
        return (id, Math.Max(0, total - elapsed), manager->GetCurrentCharges(id),
            manager->GetActionStatus(ActionType.Action, id, target) == 0);
    }

    public static bool TryPrepare(JsonObject action, IGameObject player, out RotationSubmission? submission, out string reason)
    {
        submission = null;
        reason = "";
        var requested = action["actionId"]!.GetValue<uint>();
        if (!Available)
        {
            reason = "native-action-api-unavailable";
            return false;
        }
        var manager = ActionManager.Instance();
        if (manager is null)
        {
            reason = "action-manager-unavailable";
            return false;
        }
        var sheet = Service.DataManager.GetExcelSheet<SheetAction>();
        if (!sheet.TryGetRow(requested, out var requestedRow) ||
            !requestedRow.IsPlayerAction || requestedRow.IsPvP ||
            requestedRow.ClassJobLevel > player.Level || !requestedRow.ClassJobCategory.IsValid ||
            requestedRow.ActionCategory.RowId is not (2 or 3 or 4))
        {
            reason = $"requested-action-not-available-for-player:{requested}";
            return false;
        }
        var category = requestedRow.ClassJobCategory.Value;
        var job = Service.DataManager.GetExcelSheet<Lumina.Excel.Sheets.ClassJob>().GetRow(player.ClassJob.RowId);
        var abbreviation = job.Abbreviation.ToString();
        var membership = typeof(Lumina.Excel.Sheets.ClassJobCategory).GetProperty(abbreviation);
        if (membership?.PropertyType != typeof(bool) || membership.GetValue(category) is not true)
        {
            reason = "action-job-mismatch";
            return false;
        }
        if (requestedRow.UnlockLink.RowId != 0)
        {
            if (UIState.Addresses.Instance.Value == 0 || UIState.Addresses.IsUnlockLinkUnlockedOrQuestCompleted.Value == 0)
            {
                reason = "action-unlock-api-unavailable";
                return false;
            }
            var ui = UIState.Instance();
            if (ui is null || !ui->IsUnlockLinkUnlockedOrQuestCompleted(requestedRow.UnlockLink.RowId))
            {
                reason = "action-not-unlocked";
                return false;
            }
        }
        var id = manager->GetAdjustedActionId(requested);
        // Only native mappings from a verified player action may enter non-player-marked variant rows.
        if (id == 0 || !sheet.TryGetRow(id, out var row) || row.IsPvP)
        {
            reason = "adjusted-action-unavailable";
            return false;
        }
        var selector = RotationJson.Object(action["target"]);
        var ground = RotationJson.Text(action, "ground", "none");
        var target = SelectTarget(selector, player, ground == "none" ? candidate =>
            candidate.IsTargetable && candidate.Address != IntPtr.Zero && Finite(candidate.Position) &&
            ActionManager.CanUseActionOnTarget(id, (NativeObject*)candidate.Address) &&
            ActionManager.GetActionInRangeOrLoS(id, (NativeObject*)player.Address, (NativeObject*)candidate.Address) == 0 : null);
        if (target is null)
        {
            reason = "target-unavailable";
            return false;
        }
        if (!Finite(player.Position) || !Finite(target.Position))
        {
            reason = "target-position-unavailable";
            return false;
        }
        var current = Service.TargetManager.Target;
        var currentEnemy = current is not null && current.IsHostile && !current.IsDead && current.IsTargetable;
        if (requestedRow.CanTargetHostile || row.CanTargetHostile || target.IsHostile)
        {
            if (!currentEnemy || current!.GameObjectId != target.GameObjectId ||
                RotationJson.Text(selector, "kind") != "current")
            {
                reason = "offense-requires-current-enemy";
                return false;
            }
        }
        if (row.TargetArea)
        {
            // Ground damage is not classified reliably by target flags. Never anchor it to an arbitrary party member.
            if (!currentEnemy || (ground == "target" && target.GameObjectId != current!.GameObjectId) ||
                (ground == "self" && target.GameObjectId != player.GameObjectId))
            {
                reason = "ground-requires-current-enemy-or-self-anchor";
                return false;
            }
        }
        if (row.CanTargetSelf && !row.CanTargetParty && !row.CanTargetAlly && !row.TargetArea && !currentEnemy)
        {
            reason = "unclassified-self-action-requires-current-enemy";
            return false;
        }
        if (row.TargetArea != (ground != "none"))
        {
            reason = "ground-mode-mismatch";
            return false;
        }
        var targetId = target.GameObjectId;
        Vector3? position = null;
        if (ground != "none")
        {
            if (ActionManager.Addresses.UseActionLocation.Value == 0 ||
                BGCollisionModule.Addresses.RaycastMaterialFilter.Value == 0 ||
                GameFramework.Addresses.Instance.Value == 0)
            {
                reason = "ground-native-api-unavailable";
                return false;
            }
            var framework = GameFramework.Instance();
            if (framework is null || framework->BGCollisionModule is null ||
                framework->BGCollisionModule->ShuttingDown || framework->BGCollisionModule->SceneManager is null)
            {
                reason = "ground-collision-scene-unavailable";
                return false;
            }
            var anchor = ground == "self" ? player : target;
            var origin = anchor.Position + Vector3.UnitY * 0.5f;
            if (!BGCollisionModule.RaycastMaterialFilter(origin, -Vector3.UnitY, out var hit, 2) ||
                !Finite(hit.Point) || Math.Abs(hit.Point.Y - anchor.Position.Y) > 1.5f ||
                Vector3.Distance(player.Position, hit.Point) > row.Range)
            {
                reason = "ground-placement-unavailable";
                return false;
            }
            var eye = player.Position + Vector3.UnitY;
            var vector = hit.Point + Vector3.UnitY * 0.1f - eye;
            var distance = vector.Length();
            if (distance > 0.1f && BGCollisionModule.RaycastMaterialFilter(
                    eye, Vector3.Normalize(vector), out _, Math.Max(0, distance - 0.1f)))
            {
                reason = "ground-line-of-sight-blocked";
                return false;
            }
            position = hit.Point;
            targetId = 0xE0000000;
        }
        else
        {
            if (!target.IsTargetable || target.Address == IntPtr.Zero ||
                !ActionManager.CanUseActionOnTarget(id, (NativeObject*)target.Address) ||
                ActionManager.GetActionInRangeOrLoS(id, (NativeObject*)player.Address, (NativeObject*)target.Address) != 0)
            {
                reason = "native-target-rejected";
                return false;
            }
        }
        var status = manager->GetActionStatus(ActionType.Action, id, targetId);
        if (status != 0)
        {
            reason = $"native-action-status:{status}";
            return false;
        }
        if (Moving() && ActionManager.GetAdjustedCastTime(ActionType.Action, id) > 0)
        {
            reason = "moving-cast";
            return false;
        }
        submission = new RotationSubmission(requested, id, targetId, position);
        return true;
    }

    public static bool Submit(RotationSubmission submission)
    {
        var manager = ActionManager.Instance();
        if (manager is null)
            return false;
        if (submission.Ground is Vector3 location)
            return manager->UseActionLocation(ActionType.Action, submission.AdjustedId, submission.TargetId, &location);
        // Macro mode prevents native queueing, so Stop never leaves an engine-owned queued action.
        return manager->UseAction(ActionType.Action, submission.AdjustedId, submission.TargetId,
            mode: ActionManager.UseActionMode.Macro);
    }

    private static bool Finite(Vector3 value)
        => float.IsFinite(value.X) && float.IsFinite(value.Y) && float.IsFinite(value.Z);

    public static int CastMilliseconds(uint id) => Math.Max(0, ActionManager.GetAdjustedCastTime(ActionType.Action, id));

    private static AgentInterface* GetAgent(AgentId id)
    {
        if (!AgentsAvailable)
            throw new RotationUnavailableException("agent-native-api-unavailable");
        var framework = GameFramework.Instance();
        if (framework is null)
            throw new RotationUnavailableException("framework-unavailable");
        var ui = framework->GetUIModule();
        if (ui is null)
            throw new RotationUnavailableException("ui-module-unavailable");
        if (*(nint*)ui == 0 || ((nint*)(*(nint*)ui))[37] == 0)
            throw new RotationUnavailableException("agent-module-vtable-unavailable");
        var module = ui->GetAgentModule();
        if (module is null || module->Initialized == 0)
            throw new RotationUnavailableException("agent-module-unavailable");
        return module->GetAgentByInternalId(id);
    }

    internal static IGameObject? SelectTarget(JsonObject selector, IGameObject player, Func<IGameObject, bool>? valid = null)
    {
        var kind = RotationJson.Text(selector, "kind");
        var dead = RotationJson.Flag(selector, "dead");
        IGameObject? target = kind switch
        {
            "current" => Service.TargetManager.Target,
            "self" => player,
            "partySlot" => PartySlot(selector["slot"]!.GetValue<int>()),
            "lowestHp" or "role" => Party(player)
                .Where(member => member.IsDead == dead && member.IsTargetable && (valid is null || valid(member)) &&
                    (kind != "role" || Role(member.ClassJob.RowId) == RotationJson.Text(selector, "role")))
                .OrderBy(member => RotationSnapshot.Hp(member)).ThenBy(member => member.GameObjectId).FirstOrDefault(),
            _ => null,
        };
        return target is not null && target.IsDead == dead ? target : null;
    }

    private static IGameObject? PartySlot(int slot)
    {
        if (slot is < 1 or > 8 || !PartySlotsAvailable)
            throw new RotationUnavailableException("party-slot-native-api-unavailable");
        var framework = GameFramework.Instance();
        if (framework is null)
            throw new RotationUnavailableException("framework-unavailable");
        var ui = framework->GetUIModule();
        if (ui is null || *(nint*)ui == 0 || ((nint*)(*(nint*)ui))[10] == 0)
            throw new RotationUnavailableException("pronoun-module-unavailable");
        var pronouns = ui->GetPronounModule();
        if (pronouns is null)
            throw new RotationUnavailableException("pronoun-module-unavailable");
        var pointer = pronouns->ResolvePlaceholder($"<{slot}>", 0, 0);
        if (pointer is null)
            return null;
        var address = (IntPtr)pointer;
        return Service.ObjectTable.Objects.FirstOrDefault(member => member.Address == address && !member.IsHostile);
    }

    internal static IEnumerable<IGameObject> Party(IGameObject player)
    {
        if (Service.PartyList.Length == 0)
        {
            yield return player;
            yield break;
        }
        for (var slot = 0; slot < Math.Min(8, Service.PartyList.Length); slot++)
            if (Service.PartyList.GetSlot(slot)?.GameObject is { } member)
                yield return member;
    }

    private static string Role(uint job)
    {
        if (!Service.DataManager.GetExcelSheet<Lumina.Excel.Sheets.ClassJob>().TryGetRow(job, out var row))
            return "";
        return row.Role switch { 1 => "tank", 4 => "healer", 2 or 3 => "dps", _ => "" };
    }
}
