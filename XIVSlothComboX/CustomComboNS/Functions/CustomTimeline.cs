using System;
using System.Collections.Generic;
using ECommons.GameHelpers;
using FFXIVClientStructs.FFXIV.Client.Game;
using XIVSlothComboX.Core;
using XIVSlothComboX.Data;
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
                    LoadCustomTime(tCustomTimeline);
                    return;
                }
            }
        }

        public static void LoadCustomTime(CustomTimeline tCustomAction)
        {
            ResetCustomTime();

            PluginConfiguration.SetCustomIntValue(UseCustomTimeKeyIndex, tCustomAction.Index);
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
                }
            }
            Service.Configuration.Save();
        }


        public static void ResetCustomTime()
        {
            PluginConfiguration.SetCustomIntValue(UseCustomTimeKeyIndex, -1);
            _CustomTimeline = null;
            时间轴.Clear();
            序列轴.Clear();
            药品轴.Clear();
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


        public static bool CustomTimelineIsEnable()
        {
            if (LocalPlayer == null)
                return false;

            if (_CustomTimeline == null)
            {
                return false;
            }

            if (LocalPlayer.ClassJob.Id == _CustomTimeline.JobId)
            {
                return true;
            }

            return false;
        }


        public static unsafe bool Useitem(uint itemId)
        {
            uint a4 = 65535;

            if (IsUseItem == false)
                return false;

            if (ActionManager.Instance()->GetActionStatus(ActionType.Item, itemId + 1000000) != 0)
                return false;

            if (ActionManager.Instance()->GetActionStatus(ActionType.Item, itemId) != 0)
                return false;

            if (InventoryManager.Instance()->GetInventoryItemCount(itemId, true) > 0)
            {
                return ActionManager.Instance()->UseAction(ActionType.Item, itemId + 1000000, LocalPlayer.ObjectId, a4);
            }

            return ActionManager.Instance()->UseAction(ActionType.Item, itemId, LocalPlayer.ObjectId, a4);
        }

        // public static long DateTimeToLongTimeStamp(DateTime dateTime)
        // {
        //     return (long)(dateTime.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        // }
    }
}