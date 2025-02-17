using System;
using System.Collections.Generic;
using ECommons.GameHelpers;
using FFXIVClientStructs.FFXIV.Client.Game;
using FFXIVClientStructs.FFXIV.Common.Math;
using XIVSlothComboX.Combos.PvE;
using XIVSlothComboX.Core;
using XIVSlothComboX.Data;
using XIVSlothComboX.Extensions;
using XIVSlothComboX.Services;

namespace XIVSlothComboX.CustomComboNS.Functions
{
    internal abstract partial class CustomComboFunctions
    {
        public const string UseCustomTimeKeyIndex = "UseCustomTimeKey";
        public const string UseCustomTimeKeyUseItem = "UseCustomTimeKeyIsUseItem";

        public static CustomTimeline? _CustomTimeline;

        public static bool IsUseItem = false;


        public static readonly List<CustomAction> 药品轴 = new();

        public static readonly List<CustomAction> 序列轴 = new();

        public static readonly List<CustomAction> 时间轴 = new();

        public static readonly List<CustomAction> 地面轴 = new();
        public static readonly List<CustomAction> 强制插入轴 = new();


        public static readonly List<CustomAction> 整个轴 = new();

        public static uint CustomTimelineHookId = 0;


        public static void InitCustomTimeline()
        {
            IsUseItem = PluginConfiguration.GetCustomBoolValue(UseCustomTimeKeyUseItem);

            var customIntValue = PluginConfiguration.GetCustomIntValue(UseCustomTimeKeyIndex, -1);
            if (customIntValue != -1)
            {
                foreach (var customTimeline in PluginConfiguration.CustomTimelineList)
                {
                    if (customTimeline.Index == customIntValue)
                    {
                        LoadCustomTime(customTimeline);
                    }
                }
            }
        }

        public static void LoadCustomTime(int index)
        {
            foreach (CustomTimeline tCustomTimeline in PluginConfiguration.CustomTimelineList)
            {
                if (tCustomTimeline.Index == index)
                {
                    LoadCustomTime(tCustomTimeline, true);
                    return;
                }
            }
        }

        public static void LoadCustomTime(CustomTimeline tCustomAction, bool init = false)
        {
            ResetCustomTime();

            if (init != true)
            {
                PluginConfiguration.SetCustomIntValue(UseCustomTimeKeyIndex, tCustomAction.Index);
            }

            _CustomTimeline = tCustomAction;


            foreach (var customAction in tCustomAction.ActionList)
            {
                整个轴.Add(customAction);

                switch (customAction.CustomActionType)
                {
                    case CustomType.序列:
                    {
                        序列轴.Add(customAction);
                        break;
                    }

                    case CustomType.时间:
                    {
                        时间轴.Add(customAction);
                        break;
                    }

                    case CustomType.药品:
                    {
                        药品轴.Add(customAction);
                        break;
                    }

                    case CustomType.地面:
                    {
                        地面轴.Add(customAction);
                        break;
                    }
                    case CustomType.强制插入:
                    {
                        强制插入轴.Add(customAction);
                        break;
                    }
                }
            }

            if (init != true)
            {
                Service.Configuration.Save();
            }
        }


        public static void ResetCustomTime()
        {
            PluginConfiguration.SetCustomIntValue(UseCustomTimeKeyIndex, -1);
            _CustomTimeline = null;
            时间轴.Clear();
            序列轴.Clear();
            药品轴.Clear();
            地面轴.Clear();
            整个轴.Clear();
            ActionWatching.CustomList.Clear();
            Service.Configuration.Save();
        }

        public static CustomAction? CustomTimelineFindBy时间轴(uint actionId)
        {
            var seconds = CombatEngageDuration().TotalSeconds;
            foreach (var customAction in 时间轴)
            {
                if (customAction.ActionId == actionId)
                {
                    if (customAction.UseTimeStart < seconds && seconds < customAction.UseTimeEnd)
                    {
                        return customAction;
                    }
                }
            }

            return null;
        }

        public static CustomAction? CustomTimelineFindBy地面轴(uint actionId)
        {
            double? seconds = -9999d;

            if (InCombat())
            {
                seconds = CombatEngageDuration().TotalSeconds;
            }
            else
            {
                var timeRemaining = Countdown.TimeRemaining();
                if (timeRemaining != null)
                {
                    seconds = -timeRemaining;
                }
            }


            foreach (var customAction in 地面轴)
            {
                if (customAction.ActionId == actionId)
                {
                    if (customAction.UseTimeStart < seconds && seconds < customAction.UseTimeEnd)
                    {
                        return customAction;
                    }
                }
            }

            return null;
        }


        public static bool CustomTimelineIsEnable()
        {
            if (LocalPlayer == null)
                return false;

            if (_CustomTimeline == null)
            {
                return false;
            }

            if (LocalPlayer.ClassJob.RowId == _CustomTimeline.JobId)
            {
                return true;
            }

            return false;
        }


        public static unsafe bool Useitem(uint itemId)
        {
            if (HasEffect(All.Buffs.生还))
            {
                return false;
            }
            
            uint a4 = 65535;
            if (IsUseItem == false)
                return false;

            if (ActionManager.Instance()->GetActionStatus(ActionType.Item, itemId + 1000000) != 0)
                return false;

            if (InventoryManager.Instance()->GetInventoryItemCount(itemId, true) > 0)
            {
                return ActionManager.Instance()->UseAction(ActionType.Item, itemId + 1000000, LocalPlayer.GameObjectId, a4);
            }

            return ActionManager.Instance()->UseAction(ActionType.Item, itemId, LocalPlayer.GameObjectId, a4);
        }

        public static unsafe void Use地面技能(CustomAction customAction)
        {
            if (HasEffect(All.Buffs.生还))
            {
                return ;
            }
            
            if (customAction.ActionId.ActionReady())
            {
                System.Numerics.Vector3 vector3 = new Vector3();
                vector3.X = customAction.Vector3.X;
                vector3.Y = customAction.Vector3.Y;
                vector3.Z = customAction.Vector3.Z;

                ActionManager.Instance()->UseActionLocation(ActionType.Action, customAction.ActionId, 3758096384, &vector3, 0);
            }
        }


        public static unsafe void AutoUseAction(uint ActionId)
        {
            if (HasEffect(All.Buffs.生还) == true)
            {
                return;
            }

            if (ActionId.ActionReady())
            {
                ActionManager.Instance()->UseAction(ActionType.Action, ActionId);
            }
        }

        public static bool OnOpenerCustomActionAction(out uint customActionActionId)
        {
            customActionActionId = All.Repose;
            if (CustomTimelineIsEnable())
            {
                double? seconds = -9999d;

                if (InCombat())
                {
                    seconds = CombatEngageDuration().TotalSeconds;
                }
                else
                {
                    var timeRemaining = Countdown.TimeRemaining();
                    if (timeRemaining != null)
                    {
                        seconds = -timeRemaining;
                    }
                }

                foreach (var customAction in 药品轴)
                {
                    if (customAction.UseTimeStart < seconds && seconds < customAction.UseTimeEnd)
                    {
                        if (CanSpellWeavePlus(All.飞斧,0.5d))
                        {
                            Useitem(customAction.ActionId);
                        }
                    }
                }

                foreach (var customAction in 地面轴)
                {
                    if (customAction.UseTimeStart < seconds && seconds < customAction.UseTimeEnd)
                    {
                        if (CanSpellWeavePlus(All.飞斧,0.5d))
                        {
                            Use地面技能(customAction);
                        }
                    }
                }

                foreach (var customAction in 时间轴)
                {
                    if (customAction.ActionId.ActionReady() && customAction.UseTimeStart < seconds && seconds < customAction.UseTimeEnd)
                    {
                        switch (customAction.CustomActionTypeSub)
                        {
                            case CustomTypeSub.默认:
                            {
                                if (CanSpellWeavePlus(All.飞斧,0.5d))
                                {
                                    // Service.ChatGui.PrintError($"1 {customAction.ActionId} {HasEffect(All.Buffs.生还)}");
                                    {
                                        // Service.ChatGui.PrintError($"22222 {customAction.ActionId}");
                                        AutoUseAction(customAction.ActionId);
                                    }
                                }

                               
                                break;
                            }

                            case CustomTypeSub.强制:
                            {
                                {
                                    AutoUseAction(customAction.ActionId);
                                }
                                break;
                            }

                            case CustomTypeSub.能力技窗口:
                            {
                                if (CanSpellWeavePlus(All.飞斧))
                                {
                                    customActionActionId = customAction.ActionId;
                                    return true;
                                }
                                break;
                            }

                            case CustomTypeSub.能力技窗口后半:
                            {
                                if (CanDelayedWeavePlus(All.飞斧))
                                {
                                    customActionActionId = customAction.ActionId;
                                    return true;
                                }
                                break;
                            }

                            case CustomTypeSub.长期替换:
                            {
                                customActionActionId = customAction.ActionId;
                                return true;
                            }
                        }


                    }
                }

                int index = ActionWatching.CustomList.Count;
                if (index < 序列轴.Count)
                {
                    byte customActionTypeSub = 序列轴[index].CustomActionTypeSub;
                    var newActionId = 序列轴[index].ActionId;
                    switch (customActionTypeSub)
                    {
                        case CustomTypeSub.默认:
                        {
                            customActionActionId = newActionId;
                            return true;
                        }

                        case CustomTypeSub.能力技窗口:
                        {
                            if (CanSpellWeavePlus(All.飞斧))
                            {
                                customActionActionId = newActionId;
                                return true;
                            }
                            break;
                        }

                        case CustomTypeSub.能力技窗口后半:
                        {
                            if (CanSpellWeavePlus(All.飞斧))
                            {
                                customActionActionId = newActionId;
                                return true;
                            }
                            break;
                        }
                    }
                }
            }
            return false;
        }

        // public static long DateTimeToLongTimeStamp(DateTime dateTime)
        // {
        //     return (long)(dateTime.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        // }
    }
}