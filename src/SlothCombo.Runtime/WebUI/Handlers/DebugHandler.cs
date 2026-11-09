using System;
using System.Collections.Generic;
using System.Linq;
using FFXIVClientStructs.FFXIV.Client.Game;
using SlothCombo.CustomComboNS.Functions;
using SlothCombo.Data;
using SlothCombo.Runtime.Services;
using Action = Lumina.Excel.Sheets.Action;
using Status = SlothCombo.Abstractions.IStatus;

using SlothCombo.Runtime.WebUI;

using SlothCombo.Runtime.WebUI.Contracts;

namespace SlothCombo.Runtime.WebUI.Handlers;

/// <summary> Serves the diagnostics page. Only available in debug builds. </summary>
internal static class DebugHandler
{
    private static readonly uint[] StatusBlacklist = { 360, 361, 362, 363, 364, 365, 366, 367, 368 };

    internal static async Task HandleAsync(ApiRequest apiRequest, ApiResponse response, GameThreadDispatcher dispatcher)
    {
#if !DEBUG
        response.SetJson(404, ApiResult.Failure("NOT_FOUND", "调试页面仅在调试版本中可用"));
#else
        var section = apiRequest.Segments.Length >= 3 ? apiRequest.Segments[2].ToLowerInvariant() : "overview";

        switch (section)
        {
            case "overview":
            {
                var payload = await dispatcher.RunAsync(BuildOverview).ConfigureAwait(false);
                response.SetJson(200, ApiResult.Success(payload));
                return;
            }

            case "actions":
            {
                var payload = await dispatcher.RunAsync(BuildActionList).ConfigureAwait(false);
                response.SetJson(200, ApiResult.Success(payload));
                return;
            }

            case "action":
            {
                if (apiRequest.Segments.Length < 4 || !uint.TryParse(apiRequest.Segments[3], out var actionId))
                {
                    response.SetJson(400, ApiResult.Failure("BAD_REQUEST", "缺少技能 id"));
                    return;
                }

                var payload = await dispatcher.RunAsync(() => BuildActionDetail(actionId)).ConfigureAwait(false);
                response.SetJson(200, ApiResult.Success(payload));
                return;
            }

            default:
                response.SetJson(404, ApiResult.Failure("NOT_FOUND", $"未知调试接口 {section}"));
                return;
        }
#endif
    }

#if DEBUG
    private static object BuildOverview()
    {
        var localPlayer = Service.ObjectTable.LocalPlayer;
        if (localPlayer == null) return new { loggedIn = false };

        var target = localPlayer.TargetObject;
        var isBattleChara = target is IBattleChara;

        return new
        {
            loggedIn = true,
            statuses = BuildStatuses(localPlayer.StatusList, false),
            targetStatuses = target is IBattleChara chara ? BuildStatuses(chara.StatusList, true) : new List<object>(),
            player = new
            {
                job = localPlayer.ClassJob.Value.NameEnglish,
                jobId = localPlayer.ClassJob.RowId,
                zone = Service.DataManager.GetExcelSheet<Lumina.Excel.Sheets.TerritoryType>()?.FirstOrDefault(x => x.RowId == Service.ClientState.TerritoryType).PlaceName.Value.Name,
                zoneId = Service.ClientState.TerritoryType,
                inPvP = CustomComboFunctions.InPvP(),
                inCombat = CustomComboFunctions.InCombat(),
                hitboxRadius = localPlayer.HitboxRadius,
            },
            target = new
            {
                objectId = target?.GameObjectId,
                objectKind = target?.ObjectKind.ToString(),
                dataId = target?.BaseId,
                isBattleChara = isBattleChara,
                isPlayerCharacter = target is IPlayerCharacter,
                distance = Math.Round(CustomComboFunctions.GetTargetDistance(), 2),
                hitboxRadius = target?.HitboxRadius,
                inMeleeRange = CustomComboFunctions.InMeleeRange(),
                relativePosition = CustomComboFunctions.AngleToTarget() switch
                {
                    2 => "Rear",
                    1 or 3 => "Flank",
                    4 => "Front",
                    _ => string.Empty,
                },
                health = isBattleChara ? $"{CustomComboFunctions.EnemyHealthCurrentHp():N0} / {CustomComboFunctions.EnemyHealthMaxHp():N0} ({Math.Round(CustomComboFunctions.GetTargetHPPercent(), 2)}%)" : null,
            },
            lastAction = new
            {
                action = ActionWatching.LastAction == 0 ? string.Empty : $"{ActionWatching.GetActionName(ActionWatching.LastAction)} (ID: {ActionWatching.LastAction})",
                cost = CustomComboFunctions.GetResourceCost(ActionWatching.LastAction),
                type = ActionWatching.GetAttackType(ActionWatching.LastAction),
                weaponskill = ActionWatching.GetActionName(ActionWatching.LastWeaponskill),
                spell = ActionWatching.GetActionName(ActionWatching.LastSpell),
                ability = ActionWatching.GetActionName(ActionWatching.LastAbility),
                comboTimer = $"{CustomComboFunctions.ComboTimer:F1}",
                comboAction = CustomComboFunctions.ComboAction == 0 ? string.Empty : $"{ActionWatching.GetActionName(CustomComboFunctions.ComboAction)} (ID: {CustomComboFunctions.ComboAction})",
                castTime = $"{localPlayer.CurrentCastTime:F2} / {localPlayer.TotalCastTime:F2}",
                castAction = localPlayer.CastActionId == 0 ? string.Empty : $"{ActionWatching.GetActionName(localPlayer.CastActionId)} (ID: {localPlayer.CastActionId})",
            },
            party = new
            {
                partyId = Service.PartyList.PartyId,
                size = Service.PartyList.Length,
                members = Enumerable.Range(1, 8)
                    .Select(slot => CustomComboFunctions.GetPartySlot(slot))
                    .Where(member => member is not null)
                    .Select(member => new
                    {
                        name = member!.Name,
                        job = (member as IPlayerCharacter)?.ClassJob.Value.Abbreviation ?? string.Empty,
                    })
                    .ToList(),
            },
            bluSpells = Service.Configuration.ActiveBLUSpells.Select(x => ActionWatching.GetActionName(x)).OrderBy(x => x).ToList(),
        };
    }

    private static List<object> BuildStatuses(IEnumerable<Status> statuses, bool isTarget)
    {
        var list = new List<object>();
        foreach (var status in statuses)
        {
            var name = ActionWatching.GetStatusName(status.StatusId);
            var remaining = isTarget
                ? CustomComboFunctions.GetDebuffRemainingTime((ushort)status.StatusId, false)
                : CustomComboFunctions.GetBuffRemainingTime((ushort)status.StatusId, false);

            string duration = string.Empty;
            if (remaining != 0 && !StatusBlacklist.Contains(status.StatusId))
            {
                duration = remaining >= 60 ? $"{(int)(remaining / 60)}m" : $"{remaining:F1}s";
            }

            list.Add(new
            {
                source = status.SourceObject?.Name,
                name = string.IsNullOrEmpty(name) ? status.StatusId.ToString() : name,
                id = status.StatusId,
                duration,
            });
        }

        return list;
    }

    private static object BuildActionList()
    {
        var jobId = Service.ClientState?.ClassJobRowId ?? 0;
        var classId = CustomComboFunctions.JobIDs.JobToClass(jobId);

        var actions = Service.DataManager.GetExcelSheet<Action>()
            .Where(x => x.IsPlayerAction && (x.ClassJob.RowId == classId || x.ClassJob.RowId == jobId))
            .OrderBy(x => x.ClassJobLevel)
            .Select(x => new
            {
                id = x.RowId,
                name = x.Name.ToString(),
                level = x.ClassJobLevel,
                isPvp = x.IsPvP,
            })
            .ToList();

        return new { actions };
    }

    private static unsafe object BuildActionDetail(uint actionId)
    {
        var sheet = Service.DataManager.GetExcelSheet<Action>();
        if (!sheet.TryGetRow(actionId, out var action)) return new { found = false };

        var actionStatus = ActionManager.Instance()->GetActionStatus(ActionType.Action, actionId);

        return new
        {
            found = true,
            id = action.RowId,
            name = action.Name.ToString(),
            level = action.ClassJobLevel,
            isPvp = action.IsPvP,
            iconId = action.Icon,
            status = actionStatus.ToString(),
            statusText = Service.DataManager.GetExcelSheet<Lumina.Excel.Sheets.LogMessage>().GetRow(actionStatus).Text.ExtractText(),
            actionType = action.ActionCategory.Value.Name.ToString(),
            quest = action.UnlockLink.RowId == 0 ? null : Service.DataManager.GetExcelSheet<Lumina.Excel.Sheets.Quest>().GetRow(action.UnlockLink.RowId).Name.ExtractText(),
            baseRecast = $"{action.Recast100ms / 10f}s",
            maxCharges = action.MaxCharges,
            timeSinceLastUse = ActionWatching.ActionTimestamps.ContainsKey(actionId)
                ? $"{(Environment.TickCount64 - ActionWatching.ActionTimestamps[actionId]) / 1000f:F2}"
                : null,
        };
    }
#endif
}
