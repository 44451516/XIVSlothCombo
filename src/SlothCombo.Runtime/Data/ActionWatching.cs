using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using SlothCombo.Abstractions;

using SlothCombo.Runtime.Hooking;
using SlothCombo.Runtime.Services;

using SlothCombo.Interop;
using SlothCombo.Interop.Game;
using SlothCombo.Abstractions.GameData;
using SlothCombo.Combos.PvE;
using SlothCombo.Core;
using SlothCombo.CustomComboNS.Functions;
using ActionRow = SlothCombo.Abstractions.GameData.ActionRow;
using AST = SlothCombo.Combos.PvE.AST;
using StatusRow = SlothCombo.Abstractions.GameData.StatusRow;
using TraitRow = SlothCombo.Abstractions.GameData.TraitRow;
using ItemRow = SlothCombo.Abstractions.GameData.ItemRow;
using AozActionRow = SlothCombo.Abstractions.GameData.AozActionRow;
using AozActionTransientRow = SlothCombo.Abstractions.GameData.AozActionTransientRow;
using Vector3Struct = System.Numerics.Vector3;

namespace SlothCombo.Data
{
    public static class ActionWatching
    {
        internal static Dictionary<uint, ActionRow> ActionSheet = Svc.Data.GetTable<ActionRow>()
            .Rows.Where(i => i.RowId is not 7).ToDictionary(i => i.RowId, i => i);

        internal static Dictionary<uint, StatusRow> StatusSheet =
            Svc.Data.GetTable<StatusRow>().Rows.ToDictionary(i => i.RowId, i => i);

        internal static Dictionary<uint, TraitRow> TraitSheet = Svc.Data.GetTable<TraitRow>()
            .Rows
            .Where(i => i.ClassJobCategory != 0) //All player traits are assigned to a category. Chocobo and other garbage lacks this, thus excluded.
            .ToDictionary(i => i.RowId, i => i);


        internal static Dictionary<uint, ItemRow> ItemsSheet =
            Svc.Data.GetTable<ItemRow>().Rows.ToDictionary(i => i.RowId, i => i);


        internal static readonly Dictionary<uint, long> ChargeTimestamps = [];
        internal static readonly Dictionary<uint, long> ActionTimestamps = [];
        private static readonly Dictionary<string, List<uint>> statusCache = new();

        internal static readonly List<uint> CombatActions = new();
        // internal static readonly List<uint> 特殊起手Actions = new();

        /**
         * 这个记录用的编辑
         */
        internal static readonly List<CustomAction> TimelineList = [];

        /**
         * 当前使用的时间轴
         * 用于对比时间序列
         */
        internal static readonly List<uint> CustomList = new();


        [UnmanagedFunctionPointer(CallingConvention.Winapi)]
        private delegate bool UseActionLocationDelegate
        (
            IntPtr actionManager,
            uint actionType,
            uint actionID,
            ulong targetedActorID,
            IntPtr vectorLocation,
            uint param,
            byte a7
        );

        private static readonly INativeHook<UseActionLocationDelegate>? UseActionLocationHook;

        private static bool UseActionLocationDetour
        (
            IntPtr actionManager,
            uint actionType,
            uint actionId,
            ulong targetedActorID,
            IntPtr vectorLocation,
            uint param,
            byte a7
        )
        {
            try
            {
                TrackUseActionLocation(actionType, actionId, ref targetedActorID, vectorLocation);
            }
            catch (Exception ex)
            {
                DetourGuard.Report(nameof(UseActionLocationDetour), ex);
            }

            return UseActionLocationHook!.Original(actionManager, actionType, actionId, targetedActorID,
                vectorLocation, param, a7);
        }

        private static void TrackUseActionLocation(uint actionType, uint actionId, ref ulong targetedActorID,
            IntPtr vectorLocation)
        {
            if (vectorLocation == IntPtr.Zero)
            {
                return;
            }

            if (CustomComboFunctions.CustomTimelineIsEnable())
            {
                if (actionType == (byte)SlothCombo.Interop.Game.ActionType.Action)
                {
                    CustomAction? customAction = CustomComboFunctions.CustomTimelineFindBy时间轴(actionId);

                    if (customAction != null)
                    {
                        int targetType = customAction.TargetType;
                        if (targetType != -1)
                        {
                            var gameObject = CustomComboFunctions.GetPartySlot(targetType);
                            if (gameObject != null)
                            {
                                targetedActorID = gameObject.GameObjectId;
                            }
                        }
                    }
                    else
                    {
                        if (CustomComboFunctions._CustomTimeline != null)
                        {
                            foreach (var tCustomAction in CustomComboFunctions._CustomTimeline.ActionList)
                            {
                                if (tCustomAction.CustomActionType == CustomType.序列)
                                {
                                    if (tCustomAction.ActionId == actionId)
                                    {
                                        CustomList.Add(actionId);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // Service.ChatGui.PrintError($"UseActionLocationDetour{GetActionName(actionId)} - {actionType}");

            Vector3Struct vector3 = Marshal.PtrToStructure<Vector3Struct>(vectorLocation);


            bool isSkip = vector3 is { X: 0, Y: 0, Z: 0 };

            if (isSkip == false)
            {
                //用于记录
                {
                    var customAction = new CustomAction();
                    customAction.ActionId = actionId;
                    var totalSeconds = CustomComboFunctions.CombatEngageDuration().TotalSeconds;

                    if (CustomComboFunctions.InCombat())
                    {
                        customAction.UseTimeStart = totalSeconds;
                        customAction.UseTimeEnd = totalSeconds + 0.5f;
                    }
                    else
                    {
                        var timeRemaining = Countdown.TimeRemaining();
                        if (timeRemaining != null)
                        {
                            customAction.UseTimeStart = (double)-timeRemaining;
                            customAction.UseTimeEnd = (double)-timeRemaining + 0.5f;
                        }
                        else
                        {
                            customAction.UseTimeStart = -1;
                        }
                    }


                    if (ActionSheet.ContainsKey(actionId))
                    {
                        {
                            customAction.CustomActionType = CustomType.地面;
                            customAction.Vector3.X = vector3.X;
                            customAction.Vector3.Y = vector3.Y;
                            customAction.Vector3.Z = vector3.Z;
                        }
                    }


                    TimelineList.Add(customAction);
                }


                // Service.ChatGui.PrintError(
                //     $"UseActionLocationDetour{GetActionName(actionId)}-{actionId}-{actionType}-{targetedActorID} {param}");
            }
        }

        [UnmanagedFunctionPointer(CallingConvention.Winapi)]
        private unsafe delegate void ReceiveActionEffectDelegate(uint casterEntityId, Character* casterPtr,
            System.Numerics.Vector3* targetPos, ActionEffectHandler.Header* header,
            ActionEffectHandler.TargetEffects* effects, GameObjectId* targetEntityIds);

        private static readonly INativeHook<ReceiveActionEffectDelegate>? ReceiveActionEffectHook;


        private unsafe static void ReceiveActionEffectDetour(uint casterEntityId, Character* casterPtr,
            System.Numerics.Vector3* targetPos, ActionEffectHandler.Header* header,
            ActionEffectHandler.TargetEffects* effects, GameObjectId* targetEntityIds)
        {
            // The game is served first and unconditionally, so nothing that
            // happens below can keep it from processing its own effect.
            ReceiveActionEffectHook!.Original(casterEntityId, casterPtr, targetPos, header, effects, targetEntityIds);

            try
            {
                TrackReceivedActionEffect(casterEntityId, header);
            }
            catch (Exception ex)
            {
                DetourGuard.Report(nameof(ReceiveActionEffectDetour), ex);
            }
        }

        private static unsafe void TrackReceivedActionEffect(uint casterEntityId, ActionEffectHandler.Header* header)
        {
            if (!CustomComboFunctions.InCombat())
            {
                CombatActions.Clear();
            }

            // if (MCHOpenerLogic.isInit && MCHOpenerLogic.currentState == OpenerState.InOpener)
            // {
            //     特殊起手Actions.Clear();
            //     MCHOpenerLogic.isInit = false;
            // }

            if (ActionType is 13 or 2)
                return;

            if (header is null || ActionSheet.ContainsKey(header->ActionId) == false)
            {
                return;
            }

            var localPlayer = Service.ObjectTable?.LocalPlayer;

            if (header->ActionId != 7 && header->ActionId != 8 &&
                localPlayer is not null && casterEntityId == localPlayer.GameObjectId)
            {
                ActionSheet.TryGetValue(header->ActionId, out var sheet);
                {
                    switch (sheet.ActionCategory)
                    {
                        case 2: //Spell
                            LastSpell = header->ActionId;
                            break;
                        case 3: //Weaponskill
                            LastWeaponskill = header->ActionId;
                            break;
                        case 4: //Ability
                            LastAbility = header->ActionId;
                            break;
                    }
                }
                TimeLastActionUsed = DateTime.Now;

                LastAction = header->ActionId;
                CombatActions.Add(header->ActionId);
                // 特殊起手Actions.Add(header.ActionId);


                if (Service.Configuration.EnabledOutputLog)
                    OutputLog();
            }
        }

        [UnmanagedFunctionPointer(CallingConvention.Winapi)]
        private delegate void SendActionDelegate
        (
            ulong targetObjectId,
            byte actionType,
            uint actionId,
            ushort sequence,
            long a5,
            long a6,
            long a7,
            long a8,
            long a9);

        private static readonly INativeHook<SendActionDelegate>? SendActionHook;

        private static unsafe void SendActionDetour
        (
            ulong targetObjectId,
            byte actionType,
            uint actionId,
            ushort sequence,
            long a5,
            long a6,
            long a7,
            long a8,
            long a9)
        {
            try
            {
                TrackSentAction(ref targetObjectId, actionType, actionId);
            }
            catch (Exception ex)
            {
                DetourGuard.Report(nameof(SendActionDetour), ex);
            }

            // The action goes out exactly once, whatever the bookkeeping did.
            SendActionHook!.Original(targetObjectId, actionType, actionId, sequence, a5, a6, a7, a8, a9);

            try
            {
                TimeLastActionUsed = DateTime.Now;
                ActionType = actionType;

                UpdateHelpers(actionId);
            }
            catch (Exception ex)
            {
                DetourGuard.Report(nameof(SendActionDetour), ex);
            }
        }

        private static void TrackSentAction(ref ulong targetObjectId, byte actionType, uint actionId)
        {
            // Service.ChatGui.PrintError($"1 {actionType} - {actionId}");
            if (actionType == (byte)SlothCombo.Interop.Game.ActionType.Action)
            {
                var customAction = new CustomAction();
                customAction.ActionId = actionId;
                var totalSeconds = CustomComboFunctions.CombatEngageDuration().TotalSeconds;

                if (CustomComboFunctions.InCombat())
                {
                    customAction.UseTimeStart = totalSeconds;
                    customAction.UseTimeEnd = totalSeconds + 0.5f;
                }
                else
                {
                    var timeRemaining = Countdown.TimeRemaining();
                    if (timeRemaining != null)
                    {
                        customAction.UseTimeStart = (double)-timeRemaining;
                        customAction.UseTimeEnd = (double)-timeRemaining + 0.5f;
                    }
                    else
                    {
                        customAction.UseTimeStart = -1;
                    }
                }

                if (ActionSheet.ContainsKey(actionId))
                {
                    var actionByActionSheet = ActionSheet[actionId];

                    if (actionByActionSheet.CanTargetParty)
                    {
                        customAction.CustomActionType = CustomType.时间;
                        customAction.TargetType = CustomComboFunctions.getPartyIndex(targetObjectId);
                    }
                }

                // Service.ChatGui.PrintError($"2 {actionType} - {actionId}");
                //为啥不换UseAction？？？
                TimelineList.Add(customAction);
            }

            if (actionType == 1 && CustomComboFunctions.GetMaxCharges(actionId) > 0)
            {
                ChargeTimestamps[actionId] = Environment.TickCount64;
            }

            if (actionType == 1)
            {
                ActionTimestamps[actionId] = Environment.TickCount64;
            }

            CheckForChangedTarget(actionId, ref targetObjectId, actionType);
        }

        private static void UpdateHelpers(uint actionId)
        {
            if (actionId is NIN.Ten or NIN.Chi or NIN.Jin or NIN.TenCombo or NIN.ChiCombo or NIN.JinCombo)
                Combos.JobHelpers.NIN.NINHelper.InMudra = true;
            else
                Combos.JobHelpers.NIN.NINHelper.InMudra = false;
        }

        private static unsafe void CheckForChangedTarget(uint actionId, ref ulong targetObjectId, byte actionType)
        {
            // Service.ChatGui.PrintError($"{targetObjectId} -> {actionId} ->{actionType}");
            // if (CustomComboFunctions.CustomTimelineIsEnable())
            // {
            //     if (actionType == (byte)SlothCombo.Interop.Game.ActionType.Action)
            //     {
            //         CustomAction? customAction = CustomComboFunctions.CustomTimelineFindBy时间轴(actionId);
            //
            //         if (customAction != null)
            //         {
            //             int targetType = customAction.TargetType;
            //             if (targetType != -1)
            //             {
            //                 var gameObject = CustomComboFunctions.GetPartySlot(targetType);
            //                 if (gameObject != null)
            //                 {
            //                     targetObjectId = gameObject.GameObjectId;
            //                 }
            //             }
            //         }
            //         else
            //         {
            //             CustomList.Add(actionId);
            //         }
            //     }
            // }


            if (actionId is AST.Balance or AST.Bole or AST.Ewer or AST.Arrow or AST.Spire or AST.Spear &&
                Combos.JobHelpers.AST.AST_QuickTargetCards.SelectedRandomMember is not null && !OutOfRange(actionId,
                    Service.ObjectTable.LocalPlayer!, Combos.JobHelpers.AST.AST_QuickTargetCards.SelectedRandomMember))
            {
                int targetOptions = AST.Config.AST_QuickTarget_Override;

                switch (targetOptions)
                {
                    case 0:
                        targetObjectId = Combos.JobHelpers.AST.AST_QuickTargetCards.SelectedRandomMember.GameObjectId;
                        break;
                    case 1:
                        if (CustomComboFunctions.HasFriendlyTarget())
                            targetObjectId = Service.ObjectTable.LocalPlayer.TargetObject.GameObjectId;
                        else
                            targetObjectId = Combos.JobHelpers.AST.AST_QuickTargetCards.SelectedRandomMember
                                .GameObjectId;
                        break;
                    case 2:
                        if (CustomComboFunctions.GetHealTarget(true, true) is not null)
                            targetObjectId = CustomComboFunctions.GetHealTarget(true, true).GameObjectId;
                        else
                            targetObjectId = Combos.JobHelpers.AST.AST_QuickTargetCards.SelectedRandomMember
                                .GameObjectId;
                        break;
                }
            }
        }

        // public static unsafe bool OutOfRange(uint actionId, GameObject* source, GameObject* target)
        public static unsafe bool OutOfRange(uint actionId, IGameObject source, IGameObject target)
        {
            // return ActionManager.GetActionInRangeOrLoS(actionId, source, target) is 566;
            return ActionManager.GetActionInRangeOrLoS(actionId, (GameObject*)source.Address, (GameObject*)target.Address) is 566;
        }


        /// <summary>
        /// Returns the amount of time since an action was last used.
        /// </summary>
        /// <param name="actionId"></param>
        /// <returns>Time in milliseconds if found, else -1.</returns>
        public static float TimeSinceActionUsed(uint actionId)
        {
            if (ActionTimestamps.ContainsKey(actionId))
                return Environment.TickCount64 - ActionTimestamps[actionId];

            return -1f;
        }

        public static uint WhichOfTheseActionsWasLast(params uint[] actions)
        {
            if (CombatActions.Count == 0) return 0;

            int currentLastIndex = 0;
            foreach (var action in actions)
            {
                if (CombatActions.Any(x => x == action))
                {
                    int index = CombatActions.LastIndexOf(action);

                    if (index > currentLastIndex)
                        currentLastIndex = index;
                }
            }

            return CombatActions[currentLastIndex];
        }

        public static int HowManyTimesUsedAfterAnotherAction(uint lastUsedIDToCheck, uint idToCheckAgainst)
        {
            if (CombatActions.Count < 2) return 0;
            if (WhichOfTheseActionsWasLast(lastUsedIDToCheck, idToCheckAgainst) != lastUsedIDToCheck) return 0;

            int startingIndex = CombatActions.LastIndexOf(idToCheckAgainst);
            if (startingIndex == -1) return 0;

            int count = 0;
            for (int i = startingIndex + 1; i < CombatActions.Count; i++)
            {
                if (CombatActions[i] == lastUsedIDToCheck) count++;
            }

            return count;
        }


        public static bool HasDoubleWeaved()
        {
            if (CombatActions.Count < 2)
                return false;

            var lastAction = CombatActions.Last();
            var secondLastAction = CombatActions[^2];

            return (GetAttackType(lastAction) == GetAttackType(secondLastAction) &&
                    GetAttackType(lastAction) == ActionAttackType.Ability);
        }

        public static bool WasLast2ActionsAbilities()
        {
            if (CombatActions.Count < 2) return false;

            var lastAction = CombatActions.Last();
            var secondLastAction = CombatActions[^2];

            return (GetAttackType(lastAction) == GetAttackType(secondLastAction) &&
                    GetAttackType(lastAction) == ActionAttackType.Ability);
        }


        public static int NumberOfGcdsUsed =>
            CombatActions.Count(x =>
                GetAttackType(x) == ActionAttackType.Weaponskill || GetAttackType(x) == ActionAttackType.Spell);

        public static uint LastAction { get; set; } = 0;
        public static uint ActionType { get; set; } = 0;
        public static uint LastWeaponskill { get; set; } = 0;
        public static uint LastAbility { get; set; } = 0;
        public static uint LastSpell { get; set; } = 0;

        public static TimeSpan TimeSinceLastAction => DateTime.Now - TimeLastActionUsed;

        private static DateTime TimeLastActionUsed { get; set; } = DateTime.Now;

        public static void OutputLog()
        {
            Service.ChatGui.Print($"You just used: {GetActionName(LastAction)}-{LastAction}");
        }


        static unsafe ActionWatching()
        {
            // ReceiveActionEffectHook ??= Service.GameInteropProvider.HookFromSignature<ReceiveActionEffectDelegate>(HookAddress.ReceiveActionEffect,ReceiveActionEffectDetour);
            ReceiveActionEffectHook ??=
                Service.GameInteropProvider.HookFromAddress<ReceiveActionEffectDelegate>(
                    HookAddress.ReceiveActionEffect, ReceiveActionEffectDetour);

            SendActionHook ??=
                Service.GameInteropProvider.HookFromSignature<SendActionDelegate>(HookAddress.SendAction,
                    SendActionDetour);


            //E8 ?? ?? ?? ?? 3C 01 0F 85 ?? ?? ?? ?? EB 46
            //E8 ?? ?? ?? ?? 41 3A C5 0F 85 ?? ?? ?? ?? ?? ??

            UseActionLocationHook ??=
                Service.GameInteropProvider.HookFromAddress<UseActionLocationDelegate>(HookAddress.UseActionLocation,
                    UseActionLocationDetour);


            Service.PluginLog.Debug($"{nameof(ReceiveActionEffectHook)}         0x{ReceiveActionEffectHook.Address:X}");
            Service.PluginLog.Debug($"{nameof(SendActionHook)}                  0x{SendActionHook.Address:X}");
            Service.PluginLog.Debug($"{nameof(UseActionLocationHook)}           0x{UseActionLocationHook.Address:X}");
        }


        private static void ResetActions(ConditionFlag flag, bool value)
        {
            if (flag == ConditionFlag.InCombat && !value)
            {
                CombatActions.Clear();
                ActionTimestamps.Clear();
                LastAbility = 0;
                LastAction = 0;
                LastWeaponskill = 0;
                LastSpell = 0;
            }
        }


        public static void Enable()
        {
            ReceiveActionEffectHook?.Enable();
            SendActionHook?.Enable();
            UseActionLocationHook?.Enable();

            Service.Condition.ConditionChange += ResetActions;
            Service.ClientState.TerritoryChanged += TerritoryChangedEvent;
        }


        public static void Dispose()
        {
            Disable();
            ReceiveActionEffectHook?.Dispose();
            SendActionHook?.Dispose();
            UseActionLocationHook?.Dispose();
        }

        public static void Disable()
        {
            ReceiveActionEffectHook?.Disable();
            SendActionHook?.Disable();
            UseActionLocationHook?.Disable();

            if (Service.Condition is not null)
            {
                Service.Condition.ConditionChange -= ResetActions;
            }

            if (Service.ClientState is not null)
            {
                Service.ClientState.TerritoryChanged -= TerritoryChangedEvent;
            }
        }

        private static void TerritoryChangedEvent(uint obj)
        {
            TimelineList.Clear();
            CustomList.Clear();
        }

        public static int GetLevel(uint id) =>
            ActionSheet.TryGetValue(id, out var action) && action.ClassJobCategory != 0 ? action.ClassJobLevel : 255;

        public static float GetActionCastTime(uint id) =>
            ActionSheet.TryGetValue(id, out var action) ? action.Cast100ms / (float)10 : 0;

        public static int GetActionRange(uint id) =>
            ActionSheet.TryGetValue(id, out var action)
                ? action.Range
                : -2; // 0 & -1 are valid numbers. -2 is our failure code for InActionRange

        public static int GetActionEffectRange(uint id) =>
            ActionSheet.TryGetValue(id, out var action) ? action.EffectRange : -1;

        public static int GetTraitLevel(uint id) => TraitSheet.TryGetValue(id, out var trait) ? trait.Level : 255;

        public static string GetActionName(uint id) =>
            ActionSheet.TryGetValue(id, out var action) ? action.Name : "UNKNOWN ABILITY";

        public static string GetItemName(uint id) =>
            ItemsSheet.TryGetValue(id, out var item) ? item.Name : "UNKNOWN ITEM";

        public static string GetStatusName(uint id) =>
            StatusSheet.TryGetValue(id, out var status) ? status.Name : "Unknown Status";


        public static string GetBLUIndex(uint id)
        {
            var aozKey = Service.DataManager.GetTable<AozActionRow>().Rows
                .First(x => x.Action == id).RowId;
            var index = Service.DataManager.GetTable<AozActionTransientRow>().GetRow(aozKey)?.Number ?? 0;

            return $"#{index} ";
        }

        public static List<uint>? GetStatusesByName(string status)
        {
            if (statusCache.TryGetValue(status, out List<uint>? list))
                return list;


            return statusCache.TryAdd
            (
                status,
                StatusSheet
                    .Where(x => x.Value.Name.Equals(status, StringComparison.CurrentCultureIgnoreCase))
                    .Select(x => x.Key).ToList()
            )
                ? statusCache[status]
                : null;
        }

        public static ActionAttackType GetAttackType(uint id)
        {
            if (!ActionSheet.TryGetValue(id, out var action)) return ActionAttackType.Unknown;

            return action.ActionCategory switch
            {
                2 => ActionAttackType.Spell,
                3 => ActionAttackType.Weaponskill,
                4 => ActionAttackType.Ability,
                _ => ActionAttackType.Unknown
            };
        }

        public enum ActionAttackType
        {
            Ability,
            Spell,
            Weaponskill,
            Unknown
        }
    }

    internal static unsafe class ActionManagerHelper
    {
        private static readonly IntPtr actionMgrPtr;
        internal static IntPtr FpUseAction => (IntPtr)GameFunctions.Get(GameAddresses.UseAction);
        internal static IntPtr FpUseActionLocation => (IntPtr)GameFunctions.Get(GameAddresses.UseActionLocation);
        internal static IntPtr CheckActionResources => (IntPtr)GameFunctions.Get(GameAddresses.CheckActionResources);

        public static ushort CurrentSeq =>
            actionMgrPtr != IntPtr.Zero ? (ushort)Marshal.ReadInt16(actionMgrPtr + 0x110) : (ushort)0;

        public static ushort LastRecievedSeq =>
            actionMgrPtr != IntPtr.Zero ? (ushort)Marshal.ReadInt16(actionMgrPtr + 0x112) : (ushort)0;

        public static bool IsCasting => actionMgrPtr != IntPtr.Zero && Marshal.ReadByte(actionMgrPtr + 0x28) != 0;

        public static uint CastingActionId =>
            actionMgrPtr != IntPtr.Zero ? (uint)Marshal.ReadInt32(actionMgrPtr + 0x24) : 0u;

        public static uint CastTargetObjectId =>
            actionMgrPtr != IntPtr.Zero ? (uint)Marshal.ReadInt32(actionMgrPtr + 0x38) : 0u;

        static ActionManagerHelper() => actionMgrPtr = (IntPtr)ActionManager.Instance();
    }
}
