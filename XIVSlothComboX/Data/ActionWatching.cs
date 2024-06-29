using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Dalamud.Game.ClientState.Conditions;
using Dalamud.Hooking;
using FFXIVClientStructs.FFXIV.Client.Game;
using FFXIVClientStructs.FFXIV.Client.Game.Object;
using FFXIVClientStructs.FFXIV.Common.Math;
using InteropGenerator.Runtime;
using Lumina.Excel.GeneratedSheets;
using XIVSlothComboX.Combos.JobHelpers;
using XIVSlothComboX.Combos.JobHelpers.Enums;
using XIVSlothComboX.Core;
using XIVSlothComboX.CustomComboNS.Functions;
using XIVSlothComboX.Services;
using Action = Lumina.Excel.GeneratedSheets.Action;
using AST = XIVSlothComboX.Combos.PvE.AST;
using Vector3Struct = FFXIVClientStructs.FFXIV.Common.Math.Vector3;

namespace XIVSlothComboX.Data
{
    public static class ActionWatching
    {
        internal static Dictionary<uint, Lumina.Excel.GeneratedSheets.Action> ActionSheet =
            Service.DataManager.GetExcelSheet<Lumina.Excel.GeneratedSheets.Action>()!
                .Where(i => i.RowId is not 7)
                .ToDictionary(i => i.RowId, i => i);

        internal static Dictionary<uint, Lumina.Excel.GeneratedSheets.Status> StatusSheet =
            Service.DataManager.GetExcelSheet<Lumina.Excel.GeneratedSheets.Status>()!
                .ToDictionary(i => i.RowId, i => i);

        internal static Dictionary<uint, Item> ItemsSheet =
            Service.DataManager.GetExcelSheet<Item>()!
                .ToDictionary(i => i.RowId, i => i);


        internal static Dictionary<uint, Trait> TraitSheet = Service.DataManager.GetExcelSheet<Trait>()!
            .Where(i =>
                i.ClassJobCategory is not null) //All player traits are assigned to a category. Chocobo and other garbage lacks this, thus excluded.
            .ToDictionary(i => i.RowId, i => i);

        internal static Dictionary<uint, BNpcBase> BNpcSheet = Service.DataManager.GetExcelSheet<BNpcBase>()!
            .ToDictionary(i => i.RowId, i => i);

        private static readonly Dictionary<string, List<uint>> statusCache = new();

        internal static readonly List<uint> CombatActions = new();
        internal static readonly List<uint> 特殊起手Actions = new();

        /**
         * 这个记录用的编辑
         */
        internal static readonly List<CustomAction> TimelineList = new();

        /**
         * 当前使用的时间轴
         * 用于对比时间序列
         */
        internal static readonly List<uint> CustomList = new();


        private delegate byte UseActionLocationDelegate(IntPtr actionManager, uint actionType, uint actionID, long targetedActorID,
            IntPtr vectorLocation, uint param);

        private static readonly Hook<UseActionLocationDelegate>? UseActionLocationHook;

        private static byte UseActionLocationDetour(IntPtr actionManager, uint actionType, uint actionId, long targetedActorID, IntPtr vectorLocation,
            uint param)
        {
            // Service.ChatGui.PrintError($"UseActionLocationDetour{GetActionName(actionID)}");
            // Service.ChatGui.PrintError($"UseActionLocationDetour{GetActionName(actionID)}");

            //
            Vector3 vector3 = (Vector3)Marshal.PtrToStructure<Vector3Struct>(vectorLocation);
            
            
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


            var ret = UseActionLocationHook.Original(actionManager, actionType, actionId, targetedActorID, vectorLocation, param);

            return ret;
        }

        private delegate void ReceiveActionEffectDelegate(ulong sourceObjectId, IntPtr sourceActor, IntPtr position, IntPtr effectHeader,
            IntPtr effectArray, IntPtr effectTrail);

        private static readonly Hook<ReceiveActionEffectDelegate>? ReceiveActionEffectHook;

        private static void ReceiveActionEffectDetour(ulong sourceObjectId, IntPtr sourceActor, IntPtr position, IntPtr effectHeader,
            IntPtr effectArray, IntPtr effectTrail)
        {
            if (!CustomComboFunctions.InCombat())
            {
                CombatActions.Clear();
            }

            if (MCHOpenerLogic.isInit && MCHOpenerLogic.currentState == OpenerState.InOpener)
            {
                特殊起手Actions.Clear();
                MCHOpenerLogic.isInit = false;
            }

            ReceiveActionEffectHook!.Original(sourceObjectId, sourceActor, position, effectHeader, effectArray, effectTrail);
            ActionEffectHeader header = Marshal.PtrToStructure<ActionEffectHeader>(effectHeader);

            if (ActionType is 13 or 2) return;
            if (header.ActionId != 7 &&
                header.ActionId != 8 &&
                sourceObjectId == Service.ClientState.LocalPlayer.GameObjectId)
            {
                TimeLastActionUsed = DateTime.Now;
                LastActionUseCount++;
                if (header.ActionId != LastAction)
                {
                    LastActionUseCount = 1;
                }

                LastAction = header.ActionId;

                ActionSheet.TryGetValue(header.ActionId, out var sheet);
                if (sheet != null)
                {
                    switch (sheet.ActionCategory.Value.RowId)
                    {
                        case 2: //Spell
                            LastSpell = header.ActionId;
                            break;
                        case 3: //Weaponskill
                            LastWeaponskill = header.ActionId;
                            break;
                        case 4: //Ability
                            LastAbility = header.ActionId;
                            break;
                    }
                }

                CombatActions.Add(header.ActionId);
                特殊起手Actions.Add(header.ActionId);


                if (Service.Configuration.EnabledOutputLog)
                    OutputLog();
            }
        }

        private delegate void SendActionDelegate(ulong targetObjectId, byte actionType, uint actionId, ushort sequence, long a5, long a6, long a7,
            long a8, long a9);

        private static readonly Hook<SendActionDelegate>? SendActionHook;

        private static unsafe void SendActionDetour(ulong targetObjectId, byte actionType, uint actionId, ushort sequence, long a5, long a6, long a7,
            long a8, long a9)
        {
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
                    Action actionByActionSheet = ActionSheet[actionId];

                    if (actionByActionSheet.CanTargetParty)
                    {
                        customAction.CustomActionType = CustomType.时间;
                        customAction.TargetType = CustomComboFunctions.getPartyIndex(targetObjectId);
                    }
                }

                //为啥不换UseAction？？？
                TimelineList.Add(customAction);
            }


            try
            {
                CheckForChangedTarget(actionId, ref targetObjectId);
                SendActionHook!.Original(targetObjectId, actionType, actionId, sequence, a5, a6, a7, a8, a9);
                TimeLastActionUsed = DateTime.Now;
                ActionType = actionType;
            }
            catch (Exception ex)
            {
                // Dalamud.Logging.PluginLog.Error(ex, "SendActionDetour");
                SendActionHook!.Original(targetObjectId, actionType, actionId, sequence, a5, a6, a7, a8, a9);
            }
        }

        private static unsafe void CheckForChangedTarget(uint actionId, ref ulong targetObjectId)
        {
            // Service.ChatGui.PrintError($"[CheckForChangedTarget]");


            if (CustomComboFunctions.CustomTimelineIsEnable())
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
                            // targetObjectId = gameObject.ObjectId;
                            targetObjectId = gameObject.GameObjectId;
                        }
                    }
                }
                else
                {
                    CustomList.Add(actionId);
                }
            }


            if (actionId is AST.Balance or AST.Bole or AST.Ewer or AST.Arrow or AST.Spire or AST.Spear &&
                Combos.JobHelpers.AST.AST_QuickTargetCards.SelectedRandomMember is not null &&
                !OutOfRange(actionId, (GameObject*)Service.ClientState.LocalPlayer.Address,
                    (GameObject*)Combos.JobHelpers.AST.AST_QuickTargetCards.SelectedRandomMember.Address))
            {
                int targetOptions = AST.Config.AST_QuickTarget_Override;

                switch (targetOptions)
                {
                    case 0:
                        targetObjectId = Combos.JobHelpers.AST.AST_QuickTargetCards.SelectedRandomMember.GameObjectId;
                        break;
                    case 1:
                        if (CustomComboFunctions.HasFriendlyTarget())
                            targetObjectId = Service.ClientState.LocalPlayer.TargetObject.GameObjectId;
                        else
                            targetObjectId = Combos.JobHelpers.AST.AST_QuickTargetCards.SelectedRandomMember.GameObjectId;
                        break;
                    case 2:
                        if (CustomComboFunctions.GetHealTarget(true, true) is not null)
                            targetObjectId = CustomComboFunctions.GetHealTarget(true, true).GameObjectId;
                        else
                            targetObjectId = Combos.JobHelpers.AST.AST_QuickTargetCards.SelectedRandomMember.GameObjectId;
                        break;
                }
            }
        }

        public static unsafe bool OutOfRange(uint actionId, GameObject* source, GameObject* target)
        {
            return ActionManager.GetActionInRangeOrLoS(actionId, source, target) is 566;
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

        public static bool WasLast2ActionsAbilities()
        {
            if (CombatActions.Count < 2) return false;
            var lastAction = CombatActions.Last();
            var secondLastAction = CombatActions[^2];

            return (GetAttackType(lastAction) == GetAttackType(secondLastAction) && GetAttackType(lastAction) == ActionAttackType.Ability);
        }


        public static int NumberOfGcdsUsed =>
            CombatActions.Count(x => GetAttackType(x) == ActionAttackType.Weaponskill || GetAttackType(x) == ActionAttackType.Spell);

        public static uint LastAction { get; set; } = 0;
        public static int LastActionUseCount { get; set; } = 0;
        public static uint ActionType { get; set; } = 0;
        public static uint LastWeaponskill { get; set; } = 0;
        public static uint LastAbility { get; set; } = 0;
        public static uint LastSpell { get; set; } = 0;

        public static TimeSpan TimeSinceLastAction => DateTime.Now - TimeLastActionUsed;

        private static DateTime TimeLastActionUsed { get; set; } = DateTime.Now;

        public static void OutputLog()
        {
            Service.ChatGui.Print($"You just used: {GetActionName(LastAction)} x{LastActionUseCount}");
        }

        public static void Dispose()
        {
            ReceiveActionEffectHook?.Dispose();
            SendActionHook?.Dispose();
            UseActionLocationHook?.Dispose();
        }

        static unsafe ActionWatching()
        {
            ReceiveActionEffectHook ??=
                Service.GameInteropProvider.HookFromSignature<ReceiveActionEffectDelegate>(HookAddress.ReceiveActionEffect,
                    ReceiveActionEffectDetour);

            SendActionHook ??= Service.GameInteropProvider.HookFromSignature<SendActionDelegate>(HookAddress.SendAction, SendActionDetour);

            
            //E8 ?? ?? ?? ?? 3C 01 0F 85 ?? ?? ?? ?? EB 46
            //E8 ?? ?? ?? ?? 41 3A C5 0F 85 ?? ?? ?? ?? ?? ??
            
            UseActionLocationHook ??=
                Service.GameInteropProvider.HookFromSignature<UseActionLocationDelegate>(HookAddress.UseActionLocation,
                    UseActionLocationDetour);

            
            // UseActionLocationHook ??=
            //     Service.GameInteropProvider.HookFromAddress<UseActionLocationDelegate>(ActionManagerHelper.FpUseActionLocation,
            //         UseActionLocationDetour);

            Service.PluginLog.Error($"{nameof(ReceiveActionEffectHook)}         0x{ReceiveActionEffectHook.Address:X}");
            Service.PluginLog.Error($"{nameof(SendActionHook)}                  0x{SendActionHook.Address:X}");
            Service.PluginLog.Error($"{nameof(UseActionLocationHook)}           0x{UseActionLocationHook.Address:X}");
        }


        public static void Enable()
        {
            ReceiveActionEffectHook?.Enable();
            SendActionHook?.Enable();
            UseActionLocationHook?.Enable();


            Service.Condition.ConditionChange += ResetActions;
            Service.ClientState.TerritoryChanged += TerritoryChangedEvent;
        }

        private static void ResetActions(ConditionFlag flag, bool value)
        {
            if (flag == ConditionFlag.InCombat && !value)
            {
                CombatActions.Clear();
                LastAbility = 0;
                LastAction = 0;
                LastWeaponskill = 0;
                LastSpell = 0;
            }
        }

        public static void Disable()
        {
            ReceiveActionEffectHook.Disable();
            SendActionHook?.Disable();
            Service.Condition.ConditionChange -= ResetActions;
            Service.ClientState.TerritoryChanged -= TerritoryChangedEvent;
        }

        private static void TerritoryChangedEvent(ushort obj)
        {
            TimelineList.Clear();
            CustomList.Clear();
        }

        public static int GetLevel(uint id) =>
            ActionSheet.TryGetValue(id, out var action) && action.ClassJobCategory is not null ? action.ClassJobLevel : 255;

        public static float GetActionCastTime(uint id) => ActionSheet.TryGetValue(id, out var action) ? action.Cast100ms / (float)10 : 0;

        public static int GetActionRange(uint id) =>
            ActionSheet.TryGetValue(id, out var action) ? action.Range : -2; // 0 & -1 are valid numbers. -2 is our failure code for InActionRange

        public static int GetActionEffectRange(uint id) => ActionSheet.TryGetValue(id, out var action) ? action.EffectRange : -1;
        public static int GetTraitLevel(uint id) => TraitSheet.TryGetValue(id, out var trait) ? trait.Level : 255;
        public static string GetActionName(uint id) => ActionSheet.TryGetValue(id, out var action) ? (string)action.Name : "UNKNOWN ABILITY";
        public static string GetItemName(uint id) => ItemsSheet.TryGetValue(id, out var item) ? (string)item.Name : "UNKNOWN ITEM";
        public static string GetStatusName(uint id) => StatusSheet.TryGetValue(id, out var status) ? (string)status.Name : "Unknown Status";

        public static List<uint>? GetStatusesByName(string status)
        {
            if (statusCache.TryGetValue(status, out List<uint>? list))
                return list;

            

            return statusCache.TryAdd(status,
                StatusSheet.Where(x => x.Value.Name.ToString().Equals(status, StringComparison.CurrentCultureIgnoreCase)).Select(x => x.Key).ToList())
                ? statusCache[status]
                : null;
            
            
        }

        public static ActionAttackType GetAttackType(uint id)
        {
            if (!ActionSheet.TryGetValue(id, out var action)) return ActionAttackType.Unknown;

            return action.ActionCategory.Row switch
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
        internal static IntPtr FpUseAction => (IntPtr)ActionManager.Addresses.UseAction.Value;
        internal static IntPtr FpUseActionLocation => (IntPtr)ActionManager.Addresses.UseActionLocation.Value;
        internal static IntPtr CheckActionResources => (IntPtr)ActionManager.Addresses.CheckActionResources.Value;
        public static ushort CurrentSeq => actionMgrPtr != IntPtr.Zero ? (ushort)Marshal.ReadInt16(actionMgrPtr + 0x110) : (ushort)0;
        public static ushort LastRecievedSeq => actionMgrPtr != IntPtr.Zero ? (ushort)Marshal.ReadInt16(actionMgrPtr + 0x112) : (ushort)0;
        public static bool IsCasting => actionMgrPtr != IntPtr.Zero && Marshal.ReadByte(actionMgrPtr + 0x28) != 0;
        public static uint CastingActionId => actionMgrPtr != IntPtr.Zero ? (uint)Marshal.ReadInt32(actionMgrPtr + 0x24) : 0u;
        public static uint CastTargetObjectId => actionMgrPtr != IntPtr.Zero ? (uint)Marshal.ReadInt32(actionMgrPtr + 0x38) : 0u;
        static ActionManagerHelper() => actionMgrPtr = (IntPtr)ActionManager.Instance();
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct ActionEffectHeader
    {
        [FieldOffset(0x0)] public long TargetObjectId;
        [FieldOffset(0x8)] public uint ActionId;
        [FieldOffset(0x14)] public uint UnkObjectId;
        [FieldOffset(0x18)] public ushort Sequence;
        [FieldOffset(0x1A)] public ushort Unk_1A;
        [FieldOffset(0X1C)] public ushort AnimationId;
        [FieldOffset(0X1F)] public byte Type;
        [FieldOffset(0x21)] public byte TargetCount;
    }
}