using System;
using System.Collections.Generic;
using System.Linq;
using Dalamud.Game.ClientState.JobGauge.Types;
using ECommons.DalamudServices;
using Newtonsoft.Json;
using XIVSlothComboX.Combos.JobHelpers;
using XIVSlothComboX.Combos.PvE.Content;
using XIVSlothComboX.Core;
using XIVSlothComboX.CustomComboNS;
using XIVSlothComboX.CustomComboNS.Functions;
using XIVSlothComboX.Data;
using XIVSlothComboX.Extensions;
using XIVSlothComboX.Services;


namespace XIVSlothComboX.Combos.PvE
{
    internal class MCH
    {
        public const byte JobID = 31;
        public static int MaxCartridges(byte level) => level >= 74 ? 2 : 1;

        internal const uint 狙击弹CleanShot = 2873,
            热狙击弹HeatedCleanShot = 7413,
            分裂弹SplitShot = 2866,
            热分裂弹HeatedSplitShot = 7411,
            独头弹SlugShot = 2868,
            虹吸弹GaussRound = 2874,
            弹射Ricochet = 2890,
            热独头弹HeatedSlugshot = 7412,
            钻头Drill = 16498,
            热弹HotShot = 2872,
            整备Reassemble = 2876,
            // 250 获取不到先拿散弹枪的ID
            整备ReassembleV2 = 25786,
            空气锚AirAnchor = 16500,
            超荷Hypercharge = 17209,
            热冲击HeatBlast = 7410,
            散射SpreadShot = 2870,
            霰弹枪Scattergun = 25786,
            自动弩AutoCrossbow = 16497,
            车式浮空炮塔RookAutoturret = 2864,
            超档车式炮塔RookOverdrive = 7415,
            后式自走人偶AutomatonQueen = 16501,
            超档后式人偶QueenOverdrive = 16502,
            策动Tactician = 16889,
            回转飞锯ChainSaw = 25788,
            BioBlaster = 16499,
            枪管加热BarrelStabilizer = 7414,
            野火Wildfire = 2878,
            武装解除Dismantle = 2887,
            火焰喷射器Flamethrower = 7418;


        internal static class Buffs
        {
            internal const ushort 整备Reassembled = 851,
                策动Tactician = 1951,
                野火Wildfire = 1946,
                过热Overheated = 2688,
                火焰喷射器Flamethrower = 1205;
        }

        internal static class Debuffs
        {
            internal const ushort Dismantled = 2887;
        }

        internal static class Config
        {
            public static UserInt MCH_ST_SecondWindThreshold = new("MCH_ST_SecondWindThreshold"),
                MCH_AoE_SecondWindThreshold = new("MCH_AoE_SecondWindThreshold"),
                MCH_ST_RotationSelection = new("MCH_ST_RotationSelection"),
                MCH_VariantCure = new("MCH_VariantCure"),
                MCH_ST_TurretUsage = new("MCH_ST_Adv_TurretGauge"),
                MCH_AoE_TurretUsage = new("MCH_AoE_TurretUsage");

            public static UserBoolArray MCH_ST_Reassembled = new("MCH_ST_Reassembled"),
                MCH_AoE_Reassembled = new("MCH_AoE_Reassembled");

            public static UserBool MCH_AoE_Hypercharge = new("MCH_AoE_Hypercharge");
        }

        internal static class Levels
        {
            internal const byte SlugShot = 2,
                Hotshot = 4,
                GaussRound = 15,
                CleanShot = 26,
                Hypercharge = 30,
                HeatBlast = 35,
                RookOverdrive = 40,
                Wildfire = 45,
                Ricochet = 50,
                Drill = 58,
                AirAnchor = 76,
                AutoCrossbow = 52,
                HeatedSplitShot = 54,
                Tactician = 56,
                HeatedSlugshot = 60,
                HeatedCleanShot = 64,
                BioBlaster = 72,
                ChargedActionMastery = 74,
                QueenOverdrive = 80,
                Scattergun = 82,
                BarrelStabilizer = 66,
                ChainSaw = 90,
                Dismantle = 62;
        }

        internal class MCH_ST_SimpleMode : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.MCH_ST_SimpleMode;

            internal static MCHOpenerLogic MCHOpener = new();

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                MCHGauge? gauge = GetJobGauge<MCHGauge>();
                float wildfireCDTime = GetCooldownRemainingTime(野火Wildfire);
                bool interruptReady = ActionReady(All.伤头HeadGraze) && CanInterruptEnemy();

                // if (actionID is 分裂弹SplitShot or 热分裂弹HeatedSplitShot )
                if (actionID is 分裂弹SplitShot or 热分裂弹HeatedSplitShot)
                {
                    if (IsEnabled(CustomComboPreset.MCH_Variant_Cure)
                        && IsEnabled(Variant.VariantCure)
                        && PlayerHealthPercentageHp() <= Config.MCH_VariantCure)
                        return Variant.VariantCure;

                    if (IsEnabled(CustomComboPreset.MCH_Variant_Rampart)
                        && IsEnabled(Variant.VariantRampart)
                        && IsOffCooldown(Variant.VariantRampart)
                        && CanWeave(actionID))
                        return Variant.VariantRampart;


                    // Opener for MCH
                    if (MCHOpener.DoFullOpener(ref actionID, false))
                    {
                        return OriginalHook(actionID);
                    }


                    // Interrupt
                    if (interruptReady)
                        return All.伤头HeadGraze;

                    // Wildfire
                    if ((gauge.Heat >= 50 || WasLastAbility(超荷Hypercharge)) && ActionReady(野火Wildfire)) //these try to ensure the correct loops
                    {
                        if (CanDelayedWeave(actionID))
                        {
                            if (!gauge.IsOverheated && WasLastWeaponskill(空气锚AirAnchor)) //WF EVEN BURST
                                return 野火Wildfire;

                            else if (gauge.IsOverheated && WasLastWeaponskill(热冲击HeatBlast))
                                return 野火Wildfire;
                        }
                    }

                    // BarrelStabilizer use
                    if (CanWeave(actionID) && gauge.Heat <= 55 && ActionReady(枪管加热BarrelStabilizer))
                        return 枪管加热BarrelStabilizer;

                    //queen
                    if (CanWeave(actionID) && !gauge.IsOverheated && LevelChecked(OriginalHook(车式浮空炮塔RookAutoturret)) && !gauge.IsRobotActive)
                    {
                        if (level >= 90)
                        {
                            // First condition
                            if (gauge.Battery is 50 && CombatEngageDuration().TotalSeconds > 61 && CombatEngageDuration().TotalSeconds < 68)
                                return OriginalHook(车式浮空炮塔RookAutoturret);

                            // Second condition
                            if (gauge.Battery is 100
                                && gauge.LastSummonBatteryPower == 50
                                && (GetCooldownRemainingTime(空气锚AirAnchor) <= 3 || ActionReady(空气锚AirAnchor)))
                                return OriginalHook(车式浮空炮塔RookAutoturret);

                            // Third condition
                            if (gauge.LastSummonBatteryPower == 100 && gauge.Battery >= 90)
                                return OriginalHook(车式浮空炮塔RookAutoturret);

                            // Fourth condition
                            else if (gauge.LastSummonBatteryPower == 90 && wildfireCDTime < 70 && wildfireCDTime > 45 && gauge.Battery >= 90)
                                return OriginalHook(车式浮空炮塔RookAutoturret);

                            // Fifth condition
                            else if (gauge.LastSummonBatteryPower != 50
                                     && (wildfireCDTime <= 4 || (ActionReady(空气锚AirAnchor) && ActionReady(野火Wildfire))))
                                return OriginalHook(车式浮空炮塔RookAutoturret);
                        }
                        else if (LevelChecked(超档车式炮塔RookOverdrive) && gauge.Battery >= 50)
                            return OriginalHook(车式浮空炮塔RookAutoturret);
                    }

                    if (CanWeave(actionID) && gauge.Heat >= 50 && LevelChecked(超荷Hypercharge) && !gauge.IsOverheated)
                    {
                        //Protection & ensures Hyper charged is double weaved with WF during reopener
                        if (HasEffect(Buffs.野火Wildfire) || !LevelChecked(野火Wildfire))
                            return 超荷Hypercharge;

                        if (LevelChecked(钻头Drill) && GetCooldownRemainingTime(钻头Drill) >= 8)
                        {
                            if (LevelChecked(空气锚AirAnchor) && GetCooldownRemainingTime(空气锚AirAnchor) >= 8)
                            {
                                if (LevelChecked(回转飞锯ChainSaw) && GetCooldownRemainingTime(回转飞锯ChainSaw) >= 8)
                                {
                                    if (UseHyperchargeStandard(gauge, wildfireCDTime))
                                        return 超荷Hypercharge;
                                }

                                else if (!LevelChecked(回转飞锯ChainSaw))
                                {
                                    if (UseHyperchargeStandard(gauge, wildfireCDTime))
                                        return 超荷Hypercharge;
                                }
                            }

                            else if (!LevelChecked(空气锚AirAnchor))
                            {
                                if (UseHyperchargeStandard(gauge, wildfireCDTime))
                                    return 超荷Hypercharge;
                            }
                        }

                        else if (!LevelChecked(钻头Drill))
                        {
                            if (UseHyperchargeStandard(gauge, wildfireCDTime))
                                return 超荷Hypercharge;
                        }
                    }

                    //Heatblast, Gauss, Rico
                    if (gauge.IsOverheated && LevelChecked(热冲击HeatBlast))
                    {
                        if (LastPreAction == (热冲击HeatBlast) && CanSpellWeavePlus(actionID))
                        {
                            if (ActionReady(虹吸弹GaussRound) && GetRemainingCharges(虹吸弹GaussRound) >= GetRemainingCharges(弹射Ricochet))
                                return 虹吸弹GaussRound;

                            if (ActionReady(弹射Ricochet) && GetRemainingCharges(弹射Ricochet) >= GetRemainingCharges(虹吸弹GaussRound))
                                return 弹射Ricochet;
                        }

                        return 热冲击HeatBlast;
                    }

                    // OGCD's
                    if (CanWeave(actionID)
                        && !HasEffect(Buffs.野火Wildfire)
                        && !HasEffect(Buffs.整备Reassembled)
                        && HasCharges(整备Reassemble)
                        && ((LevelChecked(回转飞锯ChainSaw) && GetCooldownRemainingTime(回转飞锯ChainSaw) < 1)
                            || ActionReady(回转飞锯ChainSaw)
                            || (LevelChecked(空气锚AirAnchor) && GetCooldownRemainingTime(空气锚AirAnchor) < 1)
                            || ActionReady(空气锚AirAnchor)
                            || (!LevelChecked(空气锚AirAnchor) && LevelChecked(钻头Drill) && (GetCooldownRemainingTime(钻头Drill) < 1))
                            || ActionReady(钻头Drill)))
                        return 整备Reassemble;

                    if (!HasEffect(Buffs.野火Wildfire)
                        && ((LevelChecked(回转飞锯ChainSaw) && GetCooldownRemainingTime(回转飞锯ChainSaw) < 1.2) || ActionReady(回转飞锯ChainSaw))
                        && !HasEffect(Buffs.整备Reassembled)
                        && HasCharges(整备Reassemble))
                        return 整备Reassemble;

                    if ((LevelChecked(回转飞锯ChainSaw) && GetCooldownRemainingTime(回转飞锯ChainSaw) < 1) || ActionReady(回转飞锯ChainSaw))
                        return 回转飞锯ChainSaw;

                    if ((LevelChecked(空气锚AirAnchor) && GetCooldownRemainingTime(空气锚AirAnchor) < 1)
                        || (!LevelChecked(空气锚AirAnchor) && ActionReady(热弹HotShot))
                        || ActionReady(空气锚AirAnchor))
                        return OriginalHook(空气锚AirAnchor);

                    if ((LevelChecked(钻头Drill) && GetCooldownRemainingTime(钻头Drill) < 1) || ActionReady(钻头Drill))
                        return 钻头Drill;

                    //gauss and ricochet overcap protection
                    if (CanWeave(actionID) && !gauge.IsOverheated && !HasEffect(Buffs.野火Wildfire))
                    {
                        if (ActionReady(虹吸弹GaussRound) && GetRemainingCharges(虹吸弹GaussRound) >= GetMaxCharges(虹吸弹GaussRound))
                            return 虹吸弹GaussRound;

                        if (ActionReady(弹射Ricochet) && GetRemainingCharges(弹射Ricochet) >= GetMaxCharges(弹射Ricochet))
                            return 弹射Ricochet;
                    }


                    // healing
                    if (CanWeave(actionID) && PlayerHealthPercentageHp() <= 20 && ActionReady(All.内丹SecondWind))
                        return All.内丹SecondWind;

                    //1-2-3 Combo
                    if (comboTime > 0)
                    {
                        if (lastComboMove is 分裂弹SplitShot && LevelChecked(OriginalHook(独头弹SlugShot)))
                            return OriginalHook(独头弹SlugShot);

                        if (lastComboMove is 独头弹SlugShot && LevelChecked(OriginalHook(狙击弹CleanShot)))
                            return (!LevelChecked(钻头Drill) && !HasEffect(Buffs.整备Reassembled) && HasCharges(整备Reassemble))
                                ? 整备Reassemble
                                : OriginalHook(狙击弹CleanShot);
                    }

                    return OriginalHook(分裂弹SplitShot);
                }

                return actionID;
            }

            private bool UseHyperchargeStandard(MCHGauge gauge, float wildfireCDTime)
            {
                // i really do not remember why i put > 70 here for heat, and im afraid if i remove it itll break it lol
                if (CombatEngageDuration().Minutes == 0
                    && (gauge.Heat > 70 || CombatEngageDuration().Seconds <= 30)
                    && !WasLastWeaponskill(OriginalHook(狙击弹CleanShot)))
                    return true;

                if (CombatEngageDuration().Minutes > 0)
                {
                    if (CombatEngageDuration().Minutes % 2 == 1 && gauge.Heat >= 90)
                        return true;

                    if (CombatEngageDuration().Minutes % 2 == 0)
                        return true;
                }

                return false;
            }
        }


        internal class MCH_ST_Custom : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.MCH_ST_CustomMode;


            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID is 独头弹SlugShot or 热独头弹HeatedSlugshot)
                {
                    if (CustomTimelineIsEnable())
                    {
                        var seconds = CombatEngageDuration().TotalSeconds;

                        foreach (var customAction in 药品轴)
                        {
                            if (customAction.UseTimeStart < seconds && seconds < customAction.UseTimeEnd)
                            {
                                Useitem(customAction.ActionId);
                            }
                        }


                        foreach (var customAction in 时间轴)
                        {
                            if (customAction.ActionId.ActionReady() && customAction.UseTimeStart < seconds && seconds < customAction.UseTimeEnd)
                            {
                                return customAction.ActionId;
                            }
                        }


                        int index = ActionWatching.CustomList.Count;
                        if (index < 序列轴.Count)
                        {
                            var newActionId = 序列轴[index].ActionId;
                            return newActionId;
                        }
                    }
                }


                return actionID;
            }
        }


        /***
         * 单体循环
         */
        internal class MCH_ST_AdvancedMode : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.MCH_ST_AdvancedMode;
            internal static MCHOpenerLogic MCHOpener = new();

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                float wildfireCDTime = GetCooldownRemainingTime(野火Wildfire);
                MCHGauge? gauge = GetJobGauge<MCHGauge>();
                // int rotationSelection = Config.MCH_ST_RotationSelection;
                bool interruptReady = ActionReady(All.伤头HeadGraze) && CanInterruptEnemy();


                if (actionID is 分裂弹SplitShot or 热分裂弹HeatedSplitShot)
                {
                    if (IsEnabled(CustomComboPreset.MCH_Variant_Cure)
                        && IsEnabled(Variant.VariantCure)
                        && PlayerHealthPercentageHp() <= Config.MCH_VariantCure)
                        return Variant.VariantCure;

                    if (IsEnabled(CustomComboPreset.MCH_Variant_Rampart)
                        && IsEnabled(Variant.VariantRampart)
                        && IsOffCooldown(Variant.VariantRampart)
                        && CanWeave(actionID))
                        return Variant.VariantRampart;

                    // Opener for MCH 90级起手
                    if (IsEnabled(CustomComboPreset.MCH_ST_Adv_Opener))
                    {
                        if (MCHOpener.DoFullOpener(ref actionID, false))
                            return actionID;
                    }

                    //Standard Rotation
                    // if (rotationSelection is 0)
                    {
                        // Interrupt 自动伤头
                        if (IsEnabled(CustomComboPreset.MCH_ST_Adv_Interrupt) && interruptReady)
                            return All.伤头HeadGraze;


                        //这里一般是爆发药 加入
                        if (IsEnabled(CustomComboPreset.MCH_ST_Adv_Reassembled)
                            && !gauge.IsOverheated
                            && gauge.Heat >= 50
                            && !WasLastAction(热冲击HeatBlast)
                            && 回转飞锯ChainSaw.GCDActionReady(actionID, 2.15f)
                            && GetCooldownRemainingTime(野火Wildfire) <= 1.1f
                            && CanSpellWeavePlus(actionID)
                            && GetCooldownRemainingTime(MCH.弹射Ricochet) >= 5
                            && GetCooldownRemainingTime(MCH.虹吸弹GaussRound) >= 5
                            && !HasEffect(Buffs.整备Reassembled)
                            && !HasEffect(Buffs.野火Wildfire)
                            && 整备Reassemble.ActionReady())
                        {
                            return 整备Reassemble;
                        }


                        // Wildfire
                        if (IsEnabled(CustomComboPreset.MCH_ST_Adv_WildFire))
                        {
                            if (gauge.Heat >= 50 && ActionReady(野火Wildfire) && CanSpellWeavePlus(actionID))
                            {
                                //钻头都没学 直接用吧
                                if (!LevelChecked(钻头Drill))
                                {
                                    // Service.ChatGui.Print($"我用的野火1");
                                    return 野火Wildfire;
                                }


                                if (GetCooldownRemainingTime(回转飞锯ChainSaw) < 1.5f
                                    && GetCooldownRemainingTime(空气锚AirAnchor) > 8
                                    && GetCooldownRemainingTime(钻头Drill) > 8
                                    && CanDelayedWeavePlus(actionID))
                                {
                                    return 野火Wildfire;
                                }


                                if (三件套最小冷却Time() > 7.5)
                                {
                                    if (GetCooldownRemainingTime(超荷Hypercharge) > 0.6f)
                                    {
                                        if (CanDelayedWeavePlus(actionID))
                                        {
                                            //起手延后使用野火 起码再3大剑之后
                                            if (LevelChecked(回转飞锯ChainSaw))
                                            {
                                                return 野火Wildfire;
                                            }

                                            if (LevelChecked(钻头Drill) && !LevelChecked(空气锚AirAnchor))
                                            {
                                                return 野火Wildfire;
                                            }

                                            if (LevelChecked(空气锚AirAnchor) && !LevelChecked(回转飞锯ChainSaw))
                                            {
                                                return 野火Wildfire;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //起手延后使用野火 起码再3大剑之后
                                        if (LevelChecked(回转飞锯ChainSaw))
                                        {
                                            return 野火Wildfire;
                                        }

                                        if (LevelChecked(钻头Drill) && !LevelChecked(空气锚AirAnchor))
                                        {
                                            return 野火Wildfire;
                                        }

                                        if (LevelChecked(空气锚AirAnchor) && !LevelChecked(回转飞锯ChainSaw))
                                        {
                                            return 野火Wildfire;
                                        }
                                    }
                                }
                            }
                        }

                        //queen 优雅的使用机器人
                        if (IsEnabled(CustomComboPreset.MCH_Adv_TurretQueen)
                            && Config.MCH_ST_TurretUsage == 1
                            && !WasLastAction(野火Wildfire)
                            && CanSpellWeavePlus(actionID)
                            // && !gauge.IsOverheated
                            && OriginalHook(车式浮空炮塔RookAutoturret).ActionReady()
                            && !gauge.IsRobotActive)
                        {
                            if (gauge.Battery >= 80)
                            {
                                var 空气锚快好了 = GetCooldownRemainingTime(OriginalHook(空气锚AirAnchor)) <= 3 || ActionReady(OriginalHook(空气锚AirAnchor));

                                if (空气锚快好了)
                                    return OriginalHook(车式浮空炮塔RookAutoturret);

                                var 回转飞锯ChainSaw快好了 = GetCooldownRemainingTime(回转飞锯ChainSaw) <= 3 || ActionReady(OriginalHook(回转飞锯ChainSaw));

                                if (回转飞锯ChainSaw快好了)
                                    return OriginalHook(车式浮空炮塔RookAutoturret);
                            }

                            if (gauge.Battery >= 90)
                            {
                                if (lastComboMove is 独头弹SlugShot or 热独头弹HeatedSlugshot)
                                {
                                    return OriginalHook(车式浮空炮塔RookAutoturret);
                                }
                            }

                            if (gauge.Battery >= 90)
                            {
                                return OriginalHook(车式浮空炮塔RookAutoturret);
                            }
                        }

                        //机器人好了就用
                        if (IsEnabled(CustomComboPreset.MCH_Adv_TurretQueen)
                            && Config.MCH_ST_TurretUsage == 0
                            && !WasLastAction(野火Wildfire)
                            && !gauge.IsRobotActive
                            && CanSpellWeavePlus(actionID)
                            && OriginalHook(车式浮空炮塔RookAutoturret).ActionReady()
                            && gauge.Battery >= 50)
                        {
                            if (HasEffect(Buffs.野火Wildfire) && gauge.Heat < 50)
                            {
                                return OriginalHook(车式浮空炮塔RookAutoturret);
                            }

                            if (!HasEffect(Buffs.野火Wildfire) && gauge.Battery >= 50)
                            {
                                return OriginalHook(车式浮空炮塔RookAutoturret);
                            }
                        }


                        //超荷判断
                        if (IsEnabled(CustomComboPreset.MCH_ST_Adv_Hypercharge)
                            && CanSpellWeavePlus(actionID)
                            && gauge.Heat >= 50
                            && 超荷Hypercharge.ActionReady()
                            && !gauge.IsOverheated)
                        {
                            if (HasEffect(Buffs.野火Wildfire) || !LevelChecked(野火Wildfire))
                            {
                                return 超荷Hypercharge;
                            }
                        }


                        //超荷判断
                        if (IsEnabled(CustomComboPreset.MCH_ST_Adv_Hypercharge)
                            && CanSpellWeavePlus(actionID)
                            && gauge.Heat >= 50
                            && 超荷Hypercharge.ActionReady()
                            && GetCooldownRemainingTime(MCH.虹吸弹GaussRound) > 50
                            && GetCooldownRemainingTime(MCH.弹射Ricochet) > 50
                            && !gauge.IsOverheated)
                        {
                            if (LevelChecked(钻头Drill) && GetCooldownRemainingTime(钻头Drill) > 7.5)
                            {
                                if (LevelChecked(空气锚AirAnchor) && GetCooldownRemainingTime(空气锚AirAnchor) > 7.5)
                                {
                                    if (LevelChecked(回转飞锯ChainSaw) && GetCooldownRemainingTime(回转飞锯ChainSaw) > 7.5)
                                    {
                                        if (UseHyperchargeDelayedTools(gauge, wildfireCDTime))
                                            return 超荷Hypercharge;
                                    }

                                    else if (!LevelChecked(回转飞锯ChainSaw))
                                    {
                                        if (UseHyperchargeDelayedTools(gauge, wildfireCDTime))
                                            return 超荷Hypercharge;
                                    }
                                }

                                else if (!LevelChecked(空气锚AirAnchor))
                                {
                                    if (UseHyperchargeDelayedTools(gauge, wildfireCDTime))
                                        return 超荷Hypercharge;
                                }
                            }
                            else if (!LevelChecked(钻头Drill))
                            {
                                if (UseHyperchargeDelayedTools(gauge, wildfireCDTime))
                                    return 超荷Hypercharge;
                            }
                        }


                        // BarrelStabilizer use
                        if (IsEnabled(CustomComboPreset.MCH_ST_Adv_Stabilizer)
                            && CanDelayedWeavePlus(actionID, 1.5)
                            && gauge.Heat <= 55
                            && GetRemainingCharges(MCH.虹吸弹GaussRound) != GetMaxCharges(虹吸弹GaussRound)
                            && GetRemainingCharges(MCH.弹射Ricochet) != GetMaxCharges(弹射Ricochet)
                            && ActionReady(枪管加热BarrelStabilizer))
                        {
                            return 枪管加热BarrelStabilizer;
                        }


                        // Service.ChatGui.Print($"整备Reassemble3");

                        // TOOLS!! ChainSaw Drill Air Anchor
                        if (IsEnabled(CustomComboPreset.MCH_ST_Adv_Reassembled)
                            && !gauge.IsOverheated
                            && wildfireCDTime > 60
                            && wildfireCDTime > GetCooldownRemainingTime(整备Reassemble)
                            && GetCooldownRemainingTime(MCH.弹射Ricochet) >= 5
                            && GetCooldownRemainingTime(MCH.虹吸弹GaussRound) >= 5
                            && CanSpellWeavePlus(actionID)
                            && !HasEffect(Buffs.整备Reassembled)
                            && HasCharges(整备Reassemble)
                            && 整备使用条件())
                        {
                            if (HasEffect(RaidBuff.强化药))
                            {
                                return 整备Reassemble;
                            }

                            if (!HasEffect(Buffs.野火Wildfire))
                            {
                                return 整备Reassemble;
                            }

                            if (HasEffect(Buffs.野火Wildfire) && FindEffect(Buffs.野火Wildfire)?.RemainingTime < 2f)
                            {
                                return 整备Reassemble;
                            }
                        }


                        if (IsEnabled(CustomComboPreset.MCH_ST_Adv_GaussRicochet) && LevelChecked(热冲击HeatBlast))
                        {
                            if (CanSpellWeavePlus(actionID))
                            {
                                //超荷状态下
                                if (gauge.IsOverheated)
                                {
                                    if (WasLastAction(热冲击HeatBlast)
                                        && GetCooldownRemainingTime(虹吸弹GaussRound) >= GetCooldownRemainingTime(弹射Ricochet)
                                        && 弹射Ricochet.ActionReady())
                                    {
                                        return 弹射Ricochet;
                                    }

                                    if (WasLastAction(热冲击HeatBlast)
                                        && GetCooldownRemainingTime(弹射Ricochet) >= GetCooldownRemainingTime(虹吸弹GaussRound)
                                        && 虹吸弹GaussRound.ActionReady())
                                    {
                                        return 虹吸弹GaussRound;
                                    }
                                }

                                // if (!gauge.IsOverheated && !HasEffect(Buffs.野火Wildfire))
                                if (!gauge.IsOverheated)
                                {
                                    if (WasLastAction(热冲击HeatBlast)
                                        && GetCooldownRemainingTime(弹射Ricochet) >= GetCooldownRemainingTime(虹吸弹GaussRound)
                                        && 虹吸弹GaussRound.ActionReady())
                                    {
                                        return 虹吸弹GaussRound;
                                    }

                                    if (WasLastAction(热冲击HeatBlast)
                                        && GetCooldownRemainingTime(虹吸弹GaussRound) >= GetCooldownRemainingTime(弹射Ricochet)
                                        && 弹射Ricochet.ActionReady())
                                    {
                                        return 弹射Ricochet;
                                    }


                                    if (!HasEffect(Buffs.野火Wildfire) && !WasLastAction(野火Wildfire))
                                    {
                                        if (!WasLastAction(虹吸弹GaussRound)
                                            && 虹吸弹GaussRound.ActionReady()
                                            && GetCooldownRemainingTime(MCH.虹吸弹GaussRound) >= 0
                                            && GetCooldownRemainingTime(MCH.虹吸弹GaussRound) <= 61)
                                        {
                                            return 虹吸弹GaussRound;
                                        }

                                        if (!WasLastAction(弹射Ricochet)
                                            && 弹射Ricochet.ActionReady()
                                            && GetCooldownRemainingTime(MCH.弹射Ricochet) >= 0
                                            && GetCooldownRemainingTime(MCH.弹射Ricochet) <= 61)
                                        {
                                            return 弹射Ricochet;
                                        }


                                        if (wildfireCDTime >= 40 && RaidBuff.爆发期())
                                        {
                                            if (!WasLastAction(虹吸弹GaussRound) && 虹吸弹GaussRound.ActionReady())
                                            {
                                                return 虹吸弹GaussRound;
                                            }

                                            if (!WasLastAction(弹射Ricochet) && 弹射Ricochet.ActionReady())
                                            {
                                                return 弹射Ricochet;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (IsEnabled(CustomComboPreset.MCH_ST_Adv_HeatBlast))
                        {
                            if (gauge.IsOverheated && LevelChecked(热冲击HeatBlast))
                            {
                                return 热冲击HeatBlast;
                            }
                        }


                        if (IsEnabled(CustomComboPreset.MCH_ST_Adv_Reassembled)
                            && MaxCartridges(level) == 1
                            && HasCharges(整备Reassemble))
                        {
                            if (整备使用条件())
                            {
                                if (InCombat())
                                {
                                    if (CanSpellWeavePlus(actionID))
                                    {
                                        return 整备Reassemble;
                                    }
                                }
                                else
                                {
                                    return 整备Reassemble;
                                }
                            }
                        }


                        //三大件
                        if (ReassembledTools(ref actionID))
                            return actionID;
                    }

                    // healing
                    if (IsEnabled(CustomComboPreset.MCH_ST_Adv_SecondWind)
                        && CanSpellWeavePlus(actionID, 0.6)
                        && PlayerHealthPercentageHp() <= Config.MCH_ST_SecondWindThreshold
                        && ActionReady(All.内丹SecondWind))
                    {
                        return All.内丹SecondWind;
                    }

                    //1-2-3 Combo
                    if (comboTime > 0)
                    {
                        if (lastComboMove is 分裂弹SplitShot or 热分裂弹HeatedSplitShot && LevelChecked(OriginalHook(独头弹SlugShot)))
                            return OriginalHook(独头弹SlugShot);

                        if (lastComboMove is 独头弹SlugShot or 热独头弹HeatedSlugshot && LevelChecked(OriginalHook(狙击弹CleanShot)))
                            return OriginalHook(狙击弹CleanShot);
                    }

                    return OriginalHook(分裂弹SplitShot);
                    // 这个也是可以的
                    // return 热分裂弹HeatedSplitShot; 
                }

                return actionID;
            }

            private static bool 整备使用条件()
            {
                return OriginalHook(热弹HotShot).GCDActionReady(狙击弹CleanShot)
                       || 钻头Drill.GCDActionReady(狙击弹CleanShot)
                       || 回转飞锯ChainSaw.GCDActionReady(狙击弹CleanShot);
            }


            private float 三件套最小冷却Time()
            {
                var GCDCooldownRemainingTime = GetCooldownRemainingTime(狙击弹CleanShot);

                var 钻头DrillCooldownRemainingTime = (LevelChecked(钻头Drill)) ? GetCooldownRemainingTime(钻头Drill) : 999;

                var 空气锚AirAnchorCooldownRemainingTime = (LevelChecked(空气锚AirAnchor)) ? GetCooldownRemainingTime(空气锚AirAnchor) : 9999;

                var 回转飞锯ChainSawDrillCooldownRemainingTime = (LevelChecked(回转飞锯ChainSaw)) ? GetCooldownRemainingTime(回转飞锯ChainSaw) : 9999;


                return Math.Min(Math.Min(钻头DrillCooldownRemainingTime, 空气锚AirAnchorCooldownRemainingTime), 回转飞锯ChainSawDrillCooldownRemainingTime);
            }

            private bool ReassembledTools(ref uint actionId)
            {
                // bool reassembledAnchor = (Config.MCH_ST_Reassembled[0] && HasEffect(Buffs.整备Reassembled))
                //                          || (!Config.MCH_ST_Reassembled[0] && !HasEffect(Buffs.整备Reassembled))
                //                          || (!HasEffect(Buffs.整备Reassembled) && GetRemainingCharges(整备Reassemble) == 0);
                //
                // bool reassembledDrill = (Config.MCH_ST_Reassembled[1] && HasEffect(Buffs.整备Reassembled))
                //                         || (!Config.MCH_ST_Reassembled[1] && !HasEffect(Buffs.整备Reassembled))
                //                         || (!HasEffect(Buffs.整备Reassembled) && GetRemainingCharges(整备Reassemble) == 0);
                //
                // bool reassembledChainsaw = (Config.MCH_ST_Reassembled[2] && HasEffect(Buffs.整备Reassembled))
                //                            || (!Config.MCH_ST_Reassembled[2] && !HasEffect(Buffs.整备Reassembled))
                //                            || (!HasEffect(Buffs.整备Reassembled)
                //                                && GetRemainingCharges(整备Reassemble) == 0);


                bool reassembledAnchor = (Config.MCH_ST_Reassembled[0]) && OriginalHook(热弹HotShot).GCDActionReady(狙击弹CleanShot);

                bool reassembledDrill = (Config.MCH_ST_Reassembled[1]) && 钻头Drill.GCDActionReady(狙击弹CleanShot);

                bool reassembledChainsaw = (Config.MCH_ST_Reassembled[2]) && 回转飞锯ChainSaw.GCDActionReady(狙击弹CleanShot);


                if (IsEnabled(CustomComboPreset.MCH_ST_Adv_AirAnchor) && reassembledAnchor)
                {
                    actionId = OriginalHook(空气锚AirAnchor);
                    return true;
                }


                if (IsEnabled(CustomComboPreset.MCH_ST_Adv_Drill) && reassembledDrill)
                {
                    actionId = 钻头Drill;
                    return true;
                }


                if (IsEnabled(CustomComboPreset.MCH_ST_Adv_ChainSaw) && reassembledChainsaw)
                {
                    actionId = 回转飞锯ChainSaw;
                    return true;
                }


                return false;
            }

            private bool UseHyperchargeDelayedTools(MCHGauge gauge, float wildfireCDTime)
            {
                if (!LevelChecked(超荷Hypercharge))
                {
                    return false;
                }

                if (野火Wildfire.ActionReady())
                {
                    return false;
                }


                if (gauge.Heat >= 50)
                {
                    if (wildfireCDTime is >= 0.5f and <= 33)
                    {
                        if (GetCooldownRemainingTime(枪管加热BarrelStabilizer) < wildfireCDTime)
                        {
                            return true;
                        }

                        return false;
                    }


                    if (wildfireCDTime >= 60)
                    {
                        return true;
                    }

                    if (gauge.Heat >= 50)
                    {
                        return true;
                    }
                }

                return false;
            }

            private bool UseHypercharge123Tools(MCHGauge gauge, float wildfireCDTime)
            {
                if (CombatEngageDuration().Minutes == 0
                    && (gauge.Heat >= 60 || CombatEngageDuration().Seconds <= 30)
                    && !WasLastWeaponskill(OriginalHook(狙击弹CleanShot)))
                    return true;

                if (CombatEngageDuration().Minutes > 0)
                {
                    if (gauge.Heat >= 50 && GetCooldownRemainingTime(回转飞锯ChainSaw) <= 1 && (wildfireCDTime <= 4 || IsOffCooldown(野火Wildfire)))
                        return true;

                    if (gauge.Heat >= 50 && wildfireCDTime <= 38 && wildfireCDTime >= 4)
                        return false;

                    if (gauge.Heat >= 55)
                        return true;

                    if (gauge.Heat >= 50 && wildfireCDTime >= 99)
                        return true;
                }

                return false;
            }

            private bool UseHyperchargeEarlyRotation(MCHGauge gauge, float wildfireCDTime)
            {
                //起手超荷
                if (CombatEngageDuration().Minutes == 0
                    && (gauge.Heat >= 50 || CombatEngageDuration().Seconds <= 30)
                    && WasLastWeaponskill(热分裂弹HeatedSplitShot))
                    return true;

                if (CombatEngageDuration().Minutes > 0)
                {
                    if (gauge.Heat >= 50 && wildfireCDTime <= 36 && wildfireCDTime >= 1)
                        return false;

                    if (gauge.Heat >= 60)
                        return true;

                    if (gauge.Heat >= 50 && wildfireCDTime >= 99)
                        return true;
                }

                return false;
            }
        }

        internal class MCH_AoE_SimpleMode : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.MCH_AoE_SimpleMode;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID is 散射SpreadShot)
                {
                    MCHGauge? gauge = GetJobGauge<MCHGauge>();

                    if (IsEnabled(CustomComboPreset.MCH_Variant_Cure)
                        && IsEnabled(Variant.VariantCure)
                        && PlayerHealthPercentageHp() <= GetOptionValue(Config.MCH_VariantCure))
                        return Variant.VariantCure;

                    if (IsEnabled(CustomComboPreset.MCH_Variant_Rampart)
                        && IsEnabled(Variant.VariantRampart)
                        && IsOffCooldown(Variant.VariantRampart)
                        && CanWeave(actionID))
                        return Variant.VariantRampart;

                    if (!gauge.IsOverheated)
                    {
                        if (gauge.Battery == 100)
                            return OriginalHook(车式浮空炮塔RookAutoturret);
                    }

                    //gauss and ricochet overcap protection
                    if (CanWeave(actionID) && !gauge.IsOverheated)
                    {
                        if (ActionReady(虹吸弹GaussRound) && GetRemainingCharges(虹吸弹GaussRound) >= GetMaxCharges(虹吸弹GaussRound))
                            return 虹吸弹GaussRound;

                        if (ActionReady(弹射Ricochet) && GetRemainingCharges(弹射Ricochet) >= GetMaxCharges(弹射Ricochet))
                            return 弹射Ricochet;
                    }

                    // Hypercharge        
                    if (gauge.Heat >= 50 && LevelChecked(超荷Hypercharge) && !gauge.IsOverheated)
                        return 超荷Hypercharge;

                    //Heatblast, Gauss, Rico
                    if (gauge.IsOverheated && LevelChecked(自动弩AutoCrossbow))
                    {
                        if (WasLastAction(自动弩AutoCrossbow) && CanWeave(actionID))
                        {
                            if (ActionReady(虹吸弹GaussRound) && GetRemainingCharges(虹吸弹GaussRound) >= GetRemainingCharges(弹射Ricochet))
                                return 虹吸弹GaussRound;

                            if (ActionReady(弹射Ricochet) && GetRemainingCharges(弹射Ricochet) >= GetRemainingCharges(虹吸弹GaussRound))
                                return 弹射Ricochet;
                        }

                        return 自动弩AutoCrossbow;
                    }

                    if (ActionReady(BioBlaster) && !HasEffect(Buffs.过热Overheated) && IsEnabled(CustomComboPreset.MCH_AoE_Adv_Bioblaster))
                        return BioBlaster;

                    if (CanWeave(actionID, 0.6) && PlayerHealthPercentageHp() <= 20 && ActionReady(All.内丹SecondWind))
                        return All.内丹SecondWind;
                }

                return actionID;
            }
        }

        internal class MCH_AoE_AdvancedMode : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.MCH_AoE_AdvancedMode;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID is 散射SpreadShot)
                {
                    MCHGauge? gauge = GetJobGauge<MCHGauge>();
                    bool reassembledScattergun = (Config.MCH_AoE_Reassembled[0] && HasEffect(Buffs.整备Reassembled));
                    bool reassembledCrossbow = (Config.MCH_AoE_Reassembled[1] && HasEffect(Buffs.整备Reassembled))
                                               || (!Config.MCH_AoE_Reassembled[1] && !HasEffect(Buffs.整备Reassembled));
                    bool reassembledChainsaw = (Config.MCH_AoE_Reassembled[2] && HasEffect(Buffs.整备Reassembled))
                                               || (!Config.MCH_AoE_Reassembled[2] && !HasEffect(Buffs.整备Reassembled))
                                               || (!HasEffect(Buffs.整备Reassembled) && GetRemainingCharges(整备Reassemble) == 0);


                    if (IsEnabled(CustomComboPreset.MCH_Variant_Cure)
                        && IsEnabled(Variant.VariantCure)
                        && PlayerHealthPercentageHp() <= GetOptionValue(Config.MCH_VariantCure))
                        return Variant.VariantCure;

                    if (HasEffect(Buffs.火焰喷射器Flamethrower) || JustUsed(火焰喷射器Flamethrower))
                        return OriginalHook(11);

                    if (IsEnabled(CustomComboPreset.MCH_Variant_Rampart)
                        && IsEnabled(Variant.VariantRampart)
                        && IsOffCooldown(Variant.VariantRampart)
                        && CanWeave(actionID))
                        return Variant.VariantRampart;

                    if (IsEnabled(CustomComboPreset.MCH_AoE_Adv_Reassemble)
                        && !HasEffect(Buffs.野火Wildfire)
                        && !HasEffect(Buffs.整备Reassembled)
                        && HasCharges(整备Reassemble)
                        && ((Config.MCH_AoE_Reassembled[0] && 霰弹枪Scattergun.LevelChecked())
                            || (gauge.IsOverheated && Config.MCH_AoE_Reassembled[1] && 自动弩AutoCrossbow.LevelChecked())
                            || (GetCooldownRemainingTime(OriginalHook(回转飞锯ChainSaw)) < 1
                                && Config.MCH_AoE_Reassembled[2]
                                && 回转飞锯ChainSaw.LevelChecked())))
                        return 整备Reassemble;

                    if (IsEnabled(CustomComboPreset.MCH_AoE_Adv_Chainsaw)
                        && reassembledChainsaw
                        && ((LevelChecked(回转飞锯ChainSaw) && GetCooldownRemainingTime(回转飞锯ChainSaw) < 1) || ActionReady(回转飞锯ChainSaw)))
                        return 回转飞锯ChainSaw;

                    if (reassembledScattergun)
                        return OriginalHook(霰弹枪Scattergun);

                    if (reassembledCrossbow && LevelChecked(自动弩AutoCrossbow) && gauge.IsOverheated)
                        return 自动弩AutoCrossbow;

                    if (IsEnabled(CustomComboPreset.MCH_AoE_Adv_Bioblaster) && ActionReady(BioBlaster))
                        return OriginalHook(BioBlaster);

                    if (IsEnabled(CustomComboPreset.MCH_AoE_Adv_FlameThrower) && ActionReady(火焰喷射器Flamethrower) && !IsMoving)
                        return OriginalHook(火焰喷射器Flamethrower);

                    if (IsEnabled(CustomComboPreset.MCH_AoE_Adv_Queen) && !gauge.IsOverheated)
                    {
                        if (gauge.Battery >= Config.MCH_AoE_TurretUsage)
                            return OriginalHook(车式浮空炮塔RookAutoturret);
                    }

                    // Hypercharge        
                    if (IsEnabled(CustomComboPreset.MCH_AoE_Adv_Hypercharge)
                        && gauge.Heat >= 50
                        && LevelChecked(超荷Hypercharge)
                        && LevelChecked(自动弩AutoCrossbow)
                        && !gauge.IsOverheated
                        && ((BioBlaster.LevelChecked() && GetCooldownRemainingTime(BioBlaster) > 10)
                            || !BioBlaster.LevelChecked()
                            || IsNotEnabled(CustomComboPreset.MCH_AoE_Adv_Bioblaster))
                        && ((火焰喷射器Flamethrower.LevelChecked() && GetCooldownRemainingTime(火焰喷射器Flamethrower) > 10)
                            || !火焰喷射器Flamethrower.LevelChecked()
                            || IsNotEnabled(CustomComboPreset.MCH_AoE_Adv_FlameThrower)))
                        return 超荷Hypercharge;

                    //Heatblast, Gauss, Rico
                    if (IsEnabled(CustomComboPreset.MCH_AoE_Adv_GaussRicochet)
                        && CanWeave(actionID)
                        && (Config.MCH_AoE_Hypercharge || (!Config.MCH_AoE_Hypercharge && gauge.IsOverheated)))
                    {
                        if ((WasLastAction(散射SpreadShot) || WasLastAction(自动弩AutoCrossbow) || Config.MCH_AoE_Hypercharge)
                            && ActionWatching.GetAttackType(ActionWatching.LastAction) != ActionWatching.ActionAttackType.Ability)
                        {
                            if (GetRemainingCharges(弹射Ricochet) > 0)
                                return 弹射Ricochet;

                            if (GetRemainingCharges(虹吸弹GaussRound) > 0)
                                return 虹吸弹GaussRound;
                        }
                    }

                    if (gauge.IsOverheated && 自动弩AutoCrossbow.LevelChecked())
                        return OriginalHook(自动弩AutoCrossbow);

                    if (IsEnabled(CustomComboPreset.MCH_AoE_Adv_SecondWind) && CanWeave(actionID, 0.6))
                    {
                        if (PlayerHealthPercentageHp() <= Config.MCH_AoE_SecondWindThreshold && ActionReady(All.内丹SecondWind))
                            return All.内丹SecondWind;
                    }
                }

                return actionID;
            }
        }

        internal class MCH_HeatblastGaussRicochet : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.MCH_HeatblastGaussRicochet;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                MCHGauge? gauge = GetJobGauge<MCHGauge>();
                if (actionID is 热冲击HeatBlast)
                {
                    if (IsEnabled(CustomComboPreset.MCH_AutoCrossbowGaussRicochet_AutoBarrel)
                        && ActionReady(枪管加热BarrelStabilizer)
                        && gauge.Heat < 50
                        && !HasEffect(Buffs.过热Overheated))
                        return 枪管加热BarrelStabilizer;

                    if (IsEnabled(CustomComboPreset.MCH_ST_Wildfire) && ActionReady(超荷Hypercharge) && ActionReady(野火Wildfire) && gauge.Heat >= 50)
                        return 野火Wildfire;

                    if (!HasEffect(Buffs.过热Overheated) && LevelChecked(超荷Hypercharge))
                        return 超荷Hypercharge;

                    if (GetCooldownRemainingTime(热冲击HeatBlast) < 0.7 && LevelChecked(热冲击HeatBlast)) // Prioritize Heat Blast
                        return 热冲击HeatBlast;

                    if (!LevelChecked(弹射Ricochet))
                        return 虹吸弹GaussRound;

                    if (GetCooldownRemainingTime(虹吸弹GaussRound) < GetCooldownRemainingTime(弹射Ricochet))
                        return 虹吸弹GaussRound;
                    return 弹射Ricochet;
                }

                return actionID;
            }
        }

        internal class MCH_GaussRoundRicochet : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.MCH_GaussRoundRicochet;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID is 虹吸弹GaussRound or 弹射Ricochet)

                {
                    var gaussCharges = GetRemainingCharges(虹吸弹GaussRound);
                    var ricochetCharges = GetRemainingCharges(弹射Ricochet);

                    // Prioritize the original if both are off cooldown

                    if (!LevelChecked(弹射Ricochet))
                        return 虹吸弹GaussRound;

                    if (IsOffCooldown(虹吸弹GaussRound) && IsOffCooldown(弹射Ricochet))
                        return actionID;

                    if ((gaussCharges >= ricochetCharges || level < Levels.Ricochet) && level >= Levels.GaussRound)
                        return 虹吸弹GaussRound;
                    else if (ricochetCharges > 0 && level >= Levels.Ricochet)
                        return 弹射Ricochet;
                }

                return actionID;
            }
        }

        internal class MCH_Overdrive : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.MCH_Overdrive;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID is 车式浮空炮塔RookAutoturret or 后式自走人偶AutomatonQueen)
                {
                    MCHGauge? gauge = GetJobGauge<MCHGauge>();
                    if (gauge.IsRobotActive)
                        return OriginalHook(超档后式人偶QueenOverdrive);
                }

                return actionID;
            }
        }

        internal class MCH_HotShotDrillChainSaw : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.MCH_HotShotDrillChainSaw;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID == 钻头Drill || actionID == 热弹HotShot || actionID == 空气锚AirAnchor)
                {
                    if (LevelChecked(回转飞锯ChainSaw))
                        return CalcBestAction(actionID, 回转飞锯ChainSaw, 空气锚AirAnchor, 钻头Drill);

                    if (LevelChecked(空气锚AirAnchor))
                        return CalcBestAction(actionID, 空气锚AirAnchor, 钻头Drill);

                    if (LevelChecked(钻头Drill))
                        return CalcBestAction(actionID, 钻头Drill, 热弹HotShot);

                    return 热弹HotShot;
                }

                return actionID;
            }
        }

        internal class MCH_DismantleTactician : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.MCH_DismantleTactician;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID is 武装解除Dismantle
                    && (IsOnCooldown(武装解除Dismantle) || !LevelChecked(武装解除Dismantle))
                    && ActionReady(策动Tactician)
                    && !HasEffect(Buffs.策动Tactician))
                    return 策动Tactician;

                return actionID;
            }
        }

        internal class MCH_AutoCrossbowGaussRicochet : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.MCH_AutoCrossbowGaussRicochet;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID is 自动弩AutoCrossbow)
                {
                    var heatBlastCD = GetCooldown(热冲击HeatBlast);
                    var gaussCD = GetCooldown(虹吸弹GaussRound);
                    var ricochetCD = GetCooldown(弹射Ricochet);
                    MCHGauge? gauge = GetJobGauge<MCHGauge>();

                    if (IsEnabled(CustomComboPreset.MCH_AutoCrossbowGaussRicochet_AutoBarrel)
                        && ActionReady(枪管加热BarrelStabilizer)
                        && gauge.Heat < 50
                        && !HasEffect(Buffs.过热Overheated)) return 枪管加热BarrelStabilizer;

                    if (!HasEffect(Buffs.过热Overheated) && ActionReady(超荷Hypercharge))
                        return 超荷Hypercharge;
                    if (heatBlastCD.CooldownRemaining < 0.7 && LevelChecked(自动弩AutoCrossbow)) // prioritize autocrossbow
                        return 自动弩AutoCrossbow;
                    if (!LevelChecked(弹射Ricochet))
                        return 虹吸弹GaussRound;
                    if (gaussCD.CooldownRemaining < ricochetCD.CooldownRemaining)
                        return 虹吸弹GaussRound;
                    else
                        return 弹射Ricochet;
                }

                return actionID;
            }
        }


        internal class All_PRanged_Dismantle : CustomCombo

        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.All_PRanged_Dismantle;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID is 武装解除Dismantle)
                    if (TargetHasEffectAny(Debuffs.Dismantled) && IsOffCooldown(武装解除Dismantle))
                        return BLM.Fire;

                return actionID;
            }
        }
    }
}