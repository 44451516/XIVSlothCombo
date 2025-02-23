using Dalamud.Game.ClientState.JobGauge.Types;
using ECommons.DalamudServices;
using XIVSlothComboX.Combos.PvE.Content;
using XIVSlothComboX.Core;
using XIVSlothComboX.CustomComboNS;
using XIVSlothComboX.CustomComboNS.Functions;
using XIVSlothComboX.Data;
using XIVSlothComboX.Extensions;
using static XIVSlothComboX.Combos.JobHelpers.MCHHelpers;
namespace XIVSlothComboX.Combos.PvE
{
    internal class MCH
    {
        public const byte JobID = 31;


        public const uint
            CleanShot = 2873,
            HeatedCleanShot = 7413,
            SplitShot = 2866,
            HeatedSplitShot = 7411,
            SlugShot = 2868,
            HeatedSlugShot = 7412,
            GaussRound = 2874,
            Ricochet = 2890,
            整备Reassemble = 2876,
            Reassemble = 2876,
            钻头Drill = 16498,
            Drill = 16498,
            HotShot = 2872,
            AirAnchor = 16500,
            Hypercharge = 17209,
            热冲击HeatBlast = 7410,
            HeatBlast = 7410,
            SpreadShot = 2870,
            Scattergun = 25786,
            AutoCrossbow = 16497,
            RookAutoturret = 2864,
            RookOverdrive = 7415,
            AutomatonQueen = 16501,
            QueenOverdrive = 16502,
            Tactician = 16889,
            ChainSaw = 25788,
            Chainsaw = 25788,
            BioBlaster = 16499,
            BarrelStabilizer = 7414,
            Wildfire = 2878,
            Dismantle = 2887,
            Flamethrower = 7418,
            BlazingShot = 36978,
            DoubleCheck = 36979,
            CheckMate = 36980,
            Excavator = 36981,
            FullMetalField = 36982;

        public static class Buffs
        {
            public const ushort
                Reassembled = 851,
                Tactician = 1951,
                Wildfire = 1946,
                Overheated = 2688,
                Flamethrower = 1205,
                Hypercharged = 3864,
                ExcavatorReady = 3865,
                FullMetalMachinist = 3866;
        }

        public static class Debuffs
        {
            public const ushort
                Dismantled = 2887,
                Bioblaster = 1866;
        }


        public static class Traits
        {
            public const ushort
                EnhancedMultiWeapon = 605;
        }

        public static class Config
        {
            public static UserInt
                MCH_ST_SecondWindThreshold = new("MCH_ST_SecondWindThreshold", 25),
                MCH_AoE_SecondWindThreshold = new("MCH_AoE_SecondWindThreshold", 25),
                MCH_VariantCure = new("MCH_VariantCure"),
                MCH_ST_TurretUsage = new("MCH_ST_Adv_TurretGauge"),
                MCH_AoE_TurretUsage = new("MCH_AoE_TurretUsage"),
                MCH_ST_ReassemblePool = new("MCH_ST_ReassemblePool", 0),
                MCH_AoE_ReassemblePool = new("MCH_AoE_ReassemblePool", 0),
                MCH_ST_WildfireHP = new("MCH_ST_WildfireHP", 0),
                MCH_ST_HyperchargeHP = new("MCH_ST_HyperchargeHP", 0),
                MCH_ST_QueenOverDrive = new("MCH_ST_QueenOverDrive", 100);

            public static UserBoolArray
                MCH_ST_Reassembled = new("MCH_ST_Reassembled"),
                MCH_AoE_Reassembled = new("MCH_AoE_Reassembled");

            public static UserBool
                MCH_AoE_Hypercharge = new("MCH_AoE_Hypercharge");
        }

        internal class MCH_ST_Custom : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.MCH_ST_CustomMode;


            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID is SlugShot or HeatedSlugShot)
                {
                    if (OnOpenerCustomActionAction(out var customActionActionId))
                    {
                        return customActionActionId;
                    }
                }


                return actionID;
            }
        }

        internal class MCH_ST_AdvancedMode : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.MCH_ST_AdvancedMode;
            internal static MCHOpenerLogic100 MCHOpener100 = new();
            internal static MCHOpenerLogic96 MCHOpener96 = new();
            internal static MCHOpenerLogic92 MCHOpener92 = new();
            internal static MCHOpenerLogic90 MCHOpener90 = new();
            

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                MCHGauge? gauge = GetJobGauge<MCHGauge>();
                bool interruptReady = ActionReady(All.HeadGraze) && CanInterruptEnemy() && CanDelayedWeave(actionID);
                float heatblastRC = GetCooldown(热冲击HeatBlast).CooldownTotal;
                bool drillCD = !LevelChecked(钻头Drill) || (!TraitLevelChecked(Traits.EnhancedMultiWeapon) && GetCooldownRemainingTime(钻头Drill) > heatblastRC * 6) || (TraitLevelChecked(Traits.EnhancedMultiWeapon) && GetRemainingCharges(钻头Drill) < GetMaxCharges(钻头Drill) && GetCooldownRemainingTime(钻头Drill) > heatblastRC * 6);
                bool anchorCD = !LevelChecked(AirAnchor) || (LevelChecked(AirAnchor) && GetCooldownRemainingTime(AirAnchor) > heatblastRC * 6);
                bool sawCD = !LevelChecked(ChainSaw) || (LevelChecked(ChainSaw) && GetCooldownRemainingTime(ChainSaw) > heatblastRC * 6);
                float GCD = GetCooldown(OriginalHook(SplitShot)).CooldownTotal;

                if (actionID is SplitShot or HeatedSplitShot)
                {
                    if (IsEnabled(CustomComboPreset.MCH_Variant_Cure) && IsEnabled(Variant.VariantCure) && PlayerHealthPercentageHp() <= Config.MCH_VariantCure)
                        return Variant.VariantCure;

                    if (IsEnabled(CustomComboPreset.MCH_Variant_Rampart) && IsEnabled(Variant.VariantRampart) && IsOffCooldown(Variant.VariantRampart) && CanWeave(actionID))
                        return Variant.VariantRampart;

                    // Opener for MCH 90
                    if (IsEnabled(CustomComboPreset.MCH_ST_Adv_Opener) && !LevelChecked(CheckMate))
                    {
                        if (MCHOpener90.DoFullOpener(ref actionID))
                            return actionID;
                    }

                    // Opener for MCH 92
                    if (IsEnabled(CustomComboPreset.MCH_ST_Adv_Opener) && LevelChecked(CheckMate) && !LevelChecked(Excavator))
                    {
                        if (MCHOpener92.DoFullOpener(ref actionID))
                            return actionID;
                    }

                    // Opener for MCH 96
                    if (IsEnabled(CustomComboPreset.MCH_ST_Adv_Opener) && LevelChecked(Excavator) && !LevelChecked(FullMetalField))
                    {
                        if (MCHOpener96.DoFullOpener(ref actionID))
                            return actionID;
                    }

                    // Opener for MCH 100
                    if (IsEnabled(CustomComboPreset.MCH_ST_Adv_Opener) && LevelChecked(FullMetalField))
                    {
                        if (MCHOpener100.DoFullOpener(ref actionID))
                            return actionID;
                    }

                    // Interrupt
                    if (IsEnabled(CustomComboPreset.MCH_ST_Adv_Interrupt) && interruptReady)
                        return All.HeadGraze;

                    if (IsEnabled(CustomComboPreset.MCH_ST_Adv_QueenOverdrive) && gauge.IsRobotActive && GetTargetHP() <= Config.MCH_ST_QueenOverDrive * 10000 && CanWeave(actionID) && ActionReady(OriginalHook(RookOverdrive)))
                        return OriginalHook(RookOverdrive);

                    // Wildfire
                    if (IsEnabled(CustomComboPreset.MCH_ST_Adv_WildFire) && WasLastAbility(Hypercharge) && CanWeave(actionID) && ActionReady(Wildfire) && GetTargetHP() >= Config.MCH_ST_WildfireHP * 10000)
                        return Wildfire;

                    // BarrelStabilizer
                    if (IsEnabled(CustomComboPreset.MCH_ST_Adv_Stabilizer) && !gauge.IsOverheated && CanWeave(actionID) && ActionReady(BarrelStabilizer))
                        return BarrelStabilizer;

                    if (IsEnabled(CustomComboPreset.MCH_ST_Adv_Hypercharge) && CanWeave(actionID) && (gauge.Heat >= 50 || HasEffect(Buffs.Hypercharged)) && LevelChecked(Hypercharge) && !gauge.IsOverheated && GetTargetHP() >= Config.MCH_ST_HyperchargeHP * 10000)
                    {
                        //Protection & ensures Hyper charged is double weaved with WF during reopener
                        if ((LevelChecked(FullMetalField) && WasLastWeaponskill(FullMetalField) && (GetCooldownRemainingTime(Wildfire) < GCD || ActionReady(Wildfire))) || ((!LevelChecked(FullMetalField)) && ActionReady(Wildfire)) || !LevelChecked(Wildfire))
                            return Hypercharge;

                        if (drillCD && anchorCD && sawCD && ((GetCooldownRemainingTime(Wildfire) > 40 && LevelChecked(Wildfire)) || !LevelChecked(Wildfire)))
                            return Hypercharge;
                    }

                    //Full Metal Field
                    if (IsEnabled(CustomComboPreset.MCH_ST_Adv_Stabilizer_FullMetalField) && HasEffect(Buffs.FullMetalMachinist) && (GetCooldownRemainingTime(Wildfire) <= GCD || ActionReady(Wildfire) || GetBuffRemainingTime(Buffs.FullMetalMachinist) <= 6) && LevelChecked(FullMetalField))
                        return OriginalHook(BarrelStabilizer);

                    //Heatblast, Gauss, Rico
                    if (IsEnabled(CustomComboPreset.MCH_ST_Adv_GaussRicochet) && CanWeave(actionID) && WasLastWeaponskill(OriginalHook(热冲击HeatBlast)) && (ActionWatching.GetAttackType(ActionWatching.LastAction) != ActionWatching.ActionAttackType.Ability))
                    {
                        if (ActionReady(OriginalHook(GaussRound)) && GetRemainingCharges(OriginalHook(GaussRound)) >= GetRemainingCharges(OriginalHook(Ricochet)))
                            return OriginalHook(GaussRound);

                        if (ActionReady(OriginalHook(Ricochet)) && GetRemainingCharges(OriginalHook(Ricochet)) > GetRemainingCharges(OriginalHook(GaussRound)))
                            return OriginalHook(Ricochet);
                    }

                    if (IsEnabled(CustomComboPreset.MCH_ST_Adv_Heatblast) && gauge.IsOverheated && LevelChecked(OriginalHook(热冲击HeatBlast)))
                        return OriginalHook(热冲击HeatBlast);

                    //Queen
                    //queen 优雅的使用机器人
                    if (IsEnabled(CustomComboPreset.MCH_Adv_TurretQueen)
                        && Config.MCH_ST_TurretUsage == 1
                        && !WasLastAction(Wildfire)
                        && CanSpellWeavePlus(actionID)
                        // && !gauge.IsOverheated
                        && OriginalHook(RookAutoturret).ActionReady()
                        && !gauge.IsRobotActive)
                    {
                        if (gauge.Battery >= 80)
                        {
                            var 空气锚快好了 = GetCooldownRemainingTime(OriginalHook(AirAnchor)) <= 3 || ActionReady(OriginalHook(AirAnchor));

                            if (空气锚快好了)
                                return OriginalHook(RookAutoturret);

                            var 回转飞锯ChainSaw快好了 = GetCooldownRemainingTime(ChainSaw) <= 3 || ActionReady(OriginalHook(ChainSaw));

                            if (回转飞锯ChainSaw快好了)
                                return OriginalHook(RookAutoturret);

                        }

                        if (gauge.Battery >= 90)
                        {
                            if (lastComboMove is SlugShot or HeatedSlugShot)
                            {
                                return OriginalHook(RookAutoturret);
                            }
                        }

                        if (gauge.Battery >= 90)
                        {
                            return OriginalHook(RookAutoturret);
                        }
                    }

                    //机器人好了就用
                    if (IsEnabled(CustomComboPreset.MCH_Adv_TurretQueen)
                        && Config.MCH_ST_TurretUsage == 0
                        && !WasLastAction(Wildfire)
                        && !gauge.IsRobotActive
                        && CanSpellWeavePlus(actionID)
                        && OriginalHook(RookAutoturret).ActionReady()
                        && gauge.Battery >= 50)
                    {
                        if (HasEffect(Buffs.Wildfire) && gauge.Heat < 50)
                        {
                            return OriginalHook(RookAutoturret);
                        }

                        if (!HasEffect(Buffs.Wildfire) && gauge.Battery >= 50)
                        {
                            return OriginalHook(RookAutoturret);
                        }

                    }

                    //gauss and ricochet outside HC
                    if (IsEnabled(CustomComboPreset.MCH_ST_Adv_GaussRicochet) && CanWeave(actionID) && !gauge.IsOverheated && (WasLastWeaponskill(OriginalHook(AirAnchor)) || WasLastWeaponskill(ChainSaw) || WasLastWeaponskill(钻头Drill) || WasLastWeaponskill(Excavator)) && !ActionWatching.HasDoubleWeaved())
                    {
                        if (ActionReady(OriginalHook(GaussRound)) && !WasLastAbility(OriginalHook(GaussRound)))
                            return OriginalHook(GaussRound);

                        if (ActionReady(OriginalHook(Ricochet)) && !WasLastAbility(OriginalHook(Ricochet)))
                            return OriginalHook(Ricochet);
                    }

                    //三件套
                    if (ReassembledTools(ref actionID, gauge) && !gauge.IsOverheated)
                        return actionID;

                    // healing
                    if (IsEnabled(CustomComboPreset.MCH_ST_Adv_SecondWind) && CanWeave(actionID) && PlayerHealthPercentageHp() <= Config.MCH_ST_SecondWindThreshold && ActionReady(All.SecondWind))
                        return All.SecondWind;

                    //1-2-3 Combo
                    if (comboTime > 0)
                    {
                        if (lastComboMove is SplitShot && LevelChecked(OriginalHook(SlugShot)))
                            return OriginalHook(SlugShot);

                        if (IsEnabled(CustomComboPreset.MCH_ST_Adv_Reassemble) && Config.MCH_ST_Reassembled[4] && lastComboMove == OriginalHook(SlugShot) && !LevelChecked(钻头Drill) && !HasEffect(Buffs.Reassembled) && ActionReady(整备Reassemble))
                            return 整备Reassemble;

                        if (lastComboMove is SlugShot && LevelChecked(OriginalHook(CleanShot)))
                            return OriginalHook(CleanShot);
                    }
                    return OriginalHook(SplitShot);
                }
                return actionID;
            }

            private static bool ReassembledTools(ref uint actionID, MCHGauge gauge)
            {
                bool battery = Svc.Gauges.Get<MCHGauge>().Battery >= 100;
                bool reassembledExcavator = (IsEnabled(CustomComboPreset.MCH_ST_Adv_Reassemble) && Config.MCH_ST_Reassembled[0] && (HasEffect(Buffs.Reassembled) || !HasEffect(Buffs.Reassembled))) || (IsEnabled(CustomComboPreset.MCH_ST_Adv_Reassemble) && !Config.MCH_ST_Reassembled[0] && !HasEffect(Buffs.Reassembled)) || (!HasEffect(Buffs.Reassembled) && GetRemainingCharges(整备Reassemble) <= Config.MCH_ST_ReassemblePool) || (!IsEnabled(CustomComboPreset.MCH_ST_Adv_Reassemble));
                bool reassembledChainsaw = (IsEnabled(CustomComboPreset.MCH_ST_Adv_Reassemble) && Config.MCH_ST_Reassembled[1] && (HasEffect(Buffs.Reassembled) || !HasEffect(Buffs.Reassembled))) || (IsEnabled(CustomComboPreset.MCH_ST_Adv_Reassemble) && !Config.MCH_ST_Reassembled[1] && !HasEffect(Buffs.Reassembled)) || (!HasEffect(Buffs.Reassembled) && GetRemainingCharges(整备Reassemble) <= Config.MCH_ST_ReassemblePool) || (!IsEnabled(CustomComboPreset.MCH_ST_Adv_Reassemble));
                bool reassembledAnchor = (IsEnabled(CustomComboPreset.MCH_ST_Adv_Reassemble) && Config.MCH_ST_Reassembled[2] && (HasEffect(Buffs.Reassembled) || !HasEffect(Buffs.Reassembled))) || (IsEnabled(CustomComboPreset.MCH_ST_Adv_Reassemble) && !Config.MCH_ST_Reassembled[2] && !HasEffect(Buffs.Reassembled)) || (!HasEffect(Buffs.Reassembled) && GetRemainingCharges(整备Reassemble) <= Config.MCH_ST_ReassemblePool) || (!IsEnabled(CustomComboPreset.MCH_ST_Adv_Reassemble));
                bool reassembledDrill = (IsEnabled(CustomComboPreset.MCH_ST_Adv_Reassemble) && Config.MCH_ST_Reassembled[3] && (HasEffect(Buffs.Reassembled) || !HasEffect(Buffs.Reassembled))) || (IsEnabled(CustomComboPreset.MCH_ST_Adv_Reassemble) && !Config.MCH_ST_Reassembled[3] && !HasEffect(Buffs.Reassembled)) || (!HasEffect(Buffs.Reassembled) && GetRemainingCharges(整备Reassemble) <= Config.MCH_ST_ReassemblePool) || (!IsEnabled(CustomComboPreset.MCH_ST_Adv_Reassemble));


                // TOOLS!! Chainsaw Drill Air Anchor Excavator
                if (IsEnabled(CustomComboPreset.MCH_ST_Adv_Reassemble)
                    && !gauge.IsOverheated
                    && !WasLastWeaponskill(OriginalHook(热冲击HeatBlast))
                    && !HasEffect(Buffs.Reassembled)
                    && ActionReady(整备Reassemble)
                    && (CanWeave(actionID) || !InCombat())
                    && GetRemainingCharges(整备Reassemble) > Config.MCH_ST_ReassemblePool
                    && ((Config.MCH_ST_Reassembled[0] && LevelChecked(Excavator) && HasEffect(Buffs.ExcavatorReady) && !battery)
                        || (Config.MCH_ST_Reassembled[1] && LevelChecked(ChainSaw) && (!LevelChecked(Excavator) || !Config.MCH_ST_Reassembled[0]) && ((GetCooldownRemainingTime(ChainSaw) <= GetCooldownRemainingTime(OriginalHook(SplitShot)) + 0.25) || ActionReady(ChainSaw)) && !battery)
                        || (Config.MCH_ST_Reassembled[2] && LevelChecked(AirAnchor) && ((GetCooldownRemainingTime(AirAnchor) <= GetCooldownRemainingTime(OriginalHook(SplitShot)) + 0.25) || ActionReady(AirAnchor)) && !battery)))
                {
                    actionID = 整备Reassemble;
                    return true;
                }

                if (IsEnabled(CustomComboPreset.MCH_ST_Adv_Excavator) && reassembledExcavator && LevelChecked(OriginalHook(ChainSaw)) && !battery && HasEffect(Buffs.ExcavatorReady))
                {
                    actionID = OriginalHook(ChainSaw);
                    return true;
                }

                if (IsEnabled(CustomComboPreset.MCH_ST_Adv_Chainsaw) && reassembledChainsaw && LevelChecked(ChainSaw) && !battery && ((GetCooldownRemainingTime(ChainSaw) <= GetCooldownRemainingTime(OriginalHook(SplitShot)) + 0.25) || ActionReady(ChainSaw)))
                {
                    actionID = ChainSaw;
                    return true;
                }

                if (IsEnabled(CustomComboPreset.MCH_ST_Adv_AirAnchor) && reassembledAnchor && LevelChecked(OriginalHook(AirAnchor)) && !battery && ((GetCooldownRemainingTime(OriginalHook(AirAnchor)) <= GetCooldownRemainingTime(OriginalHook(SplitShot)) + 0.25) || ActionReady(OriginalHook(AirAnchor))))
                {
                    actionID = OriginalHook(AirAnchor);
                    return true;
                }

                bool 钻头是否可以使用 = false;

                if (GetMaxCharges(钻头Drill) > 1)
                {
                    钻头是否可以使用 = GetCooldownRemainingTime(钻头Drill) - GetCooldownRemainingTime(SplitShot) < GetCooldown(钻头Drill).单次计时器 + 0.5f;
                }
                else
                {
                    钻头是否可以使用 = GetCooldownRemainingTime(钻头Drill) - GetCooldownRemainingTime(SplitShot) <= 0;
                    
                }


                if (IsEnabled(CustomComboPreset.MCH_ST_Adv_Drill) && reassembledDrill && LevelChecked(钻头Drill) && 钻头是否可以使用)
                {

                    if (GetCooldownRemainingTime(Wildfire) is >= 20 or <= 10 && !WasLastWeaponskill(钻头Drill))
                    {
                        if (IsEnabled(CustomComboPreset.MCH_ST_Adv_Reassemble)
                            && !gauge.IsOverheated
                            && !WasLastWeaponskill(OriginalHook(热冲击HeatBlast))
                            && !HasEffect(Buffs.Reassembled)
                            && ActionReady(整备Reassemble)
                            && (CanWeave(actionID) || !InCombat())
                            && Config.MCH_ST_Reassembled[3]
                            && GetRemainingCharges(整备Reassemble) > Config.MCH_ST_ReassemblePool)
                        {
                            actionID = 整备Reassemble;
                            return true;
                        }

                        actionID = 钻头Drill;
                        return true;
                    }

                    if (GetCooldownRemainingTime(钻头Drill) - GetCooldownRemainingTime(SplitShot) < 1.5f)
                    {
                        if (IsEnabled(CustomComboPreset.MCH_ST_Adv_Reassemble)
                            && !gauge.IsOverheated
                            && !WasLastWeaponskill(OriginalHook(热冲击HeatBlast))
                            && !HasEffect(Buffs.Reassembled)
                            && ActionReady(整备Reassemble)
                            && (CanWeave(actionID) || !InCombat())
                            && Config.MCH_ST_Reassembled[3]
                            && GetRemainingCharges(整备Reassemble) > Config.MCH_ST_ReassemblePool)
                        {
                            actionID = 整备Reassemble;
                            return true;
                        }

                        actionID = 钻头Drill;
                        return true;
                    }

                    return false;
                }
                return false;
            }
        }

        internal class MCH_AoE_AdvancedMode : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.MCH_AoE_AdvancedMode;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID is SpreadShot or Scattergun)
                {
                    MCHGauge? gauge = GetJobGauge<MCHGauge>();
                    float GCD = GetCooldown(OriginalHook(SpreadShot)).CooldownTotal;
                    bool reassembledScattergun = IsEnabled(CustomComboPreset.MCH_AoE_Adv_Reassemble) && Config.MCH_AoE_Reassembled[0] && HasEffect(Buffs.Reassembled);
                    bool reassembledCrossbow = (IsEnabled(CustomComboPreset.MCH_AoE_Adv_Reassemble) && Config.MCH_AoE_Reassembled[1] && HasEffect(Buffs.Reassembled)) || (IsEnabled(CustomComboPreset.MCH_AoE_Adv_Reassemble) && !Config.MCH_AoE_Reassembled[1] && !HasEffect(Buffs.Reassembled)) || (!IsEnabled(CustomComboPreset.MCH_AoE_Adv_Reassemble));
                    bool reassembledChainsaw = (IsEnabled(CustomComboPreset.MCH_AoE_Adv_Reassemble) && Config.MCH_AoE_Reassembled[2] && HasEffect(Buffs.Reassembled)) || (IsEnabled(CustomComboPreset.MCH_AoE_Adv_Reassemble) && !Config.MCH_AoE_Reassembled[2] && !HasEffect(Buffs.Reassembled)) || (!HasEffect(Buffs.Reassembled) && GetRemainingCharges(整备Reassemble) <= Config.MCH_AoE_ReassemblePool) || (!IsEnabled(CustomComboPreset.MCH_AoE_Adv_Reassemble));
                    bool reassembledExcavator = (IsEnabled(CustomComboPreset.MCH_AoE_Adv_Reassemble) && Config.MCH_AoE_Reassembled[3] && HasEffect(Buffs.Reassembled)) || (IsEnabled(CustomComboPreset.MCH_AoE_Adv_Reassemble) && !Config.MCH_AoE_Reassembled[3] && !HasEffect(Buffs.Reassembled)) || (!HasEffect(Buffs.Reassembled) && GetRemainingCharges(整备Reassemble) <= Config.MCH_AoE_ReassemblePool) || (!IsEnabled(CustomComboPreset.MCH_AoE_Adv_Reassemble));

                    if (IsEnabled(CustomComboPreset.MCH_Variant_Cure) && IsEnabled(Variant.VariantCure) && PlayerHealthPercentageHp() <= GetOptionValue(Config.MCH_VariantCure))
                        return Variant.VariantCure;

                    if (HasEffect(Buffs.Flamethrower) || JustUsed(Flamethrower))
                        return OriginalHook(11);

                    if (IsEnabled(CustomComboPreset.MCH_Variant_Rampart) && IsEnabled(Variant.VariantRampart) && IsOffCooldown(Variant.VariantRampart) && CanWeave(actionID))
                        return Variant.VariantRampart;

                    //Full Metal Field
                    if (IsEnabled(CustomComboPreset.MCH_AoE_Adv_Stabilizer_FullMetalField) && HasEffect(Buffs.FullMetalMachinist) && LevelChecked(FullMetalField))
                        return OriginalHook(BarrelStabilizer);

                    // BarrelStabilizer
                    if (IsEnabled(CustomComboPreset.MCH_AoE_Adv_Stabilizer) && !gauge.IsOverheated && CanWeave(actionID) && ActionReady(BarrelStabilizer))
                        return BarrelStabilizer;

                    if (IsEnabled(CustomComboPreset.MCH_AoE_Adv_Bioblaster) && ActionReady(BioBlaster) && !TargetHasEffect(Debuffs.Bioblaster))
                        return OriginalHook(BioBlaster);

                    if (IsEnabled(CustomComboPreset.MCH_AoE_Adv_FlameThrower) && ActionReady(Flamethrower) && !IsMoving())
                        return OriginalHook(Flamethrower);

                    if (IsEnabled(CustomComboPreset.MCH_AoE_Adv_Queen) && !gauge.IsOverheated)
                    {
                        if (gauge.Battery >= Config.MCH_AoE_TurretUsage)
                            return OriginalHook(RookAutoturret);
                    }

                    // Hypercharge        
                    if (IsEnabled(CustomComboPreset.MCH_AoE_Adv_Hypercharge) && (gauge.Heat >= 50 || HasEffect(Buffs.Hypercharged)) && LevelChecked(Hypercharge) && LevelChecked(AutoCrossbow) && !gauge.IsOverheated && ((BioBlaster.LevelChecked() && GetCooldownRemainingTime(BioBlaster) > 10) || !BioBlaster.LevelChecked() || IsNotEnabled(CustomComboPreset.MCH_AoE_Adv_Bioblaster)) && ((Flamethrower.LevelChecked() && GetCooldownRemainingTime(Flamethrower) > 10) || !Flamethrower.LevelChecked() || IsNotEnabled(CustomComboPreset.MCH_AoE_Adv_FlameThrower)))
                        return Hypercharge;

                    //AutoCrossbow, Gauss, Rico
                    if (IsEnabled(CustomComboPreset.MCH_AoE_Adv_GaussRicochet) && !Config.MCH_AoE_Hypercharge && CanWeave(actionID) && WasLastWeaponskill(OriginalHook(AutoCrossbow)) && (ActionWatching.GetAttackType(ActionWatching.LastAction) != ActionWatching.ActionAttackType.Ability))
                    {
                        if (ActionReady(OriginalHook(GaussRound)) && GetRemainingCharges(OriginalHook(GaussRound)) >= GetRemainingCharges(OriginalHook(Ricochet)))
                            return OriginalHook(GaussRound);

                        if (ActionReady(OriginalHook(Ricochet)) && GetRemainingCharges(OriginalHook(Ricochet)) > GetRemainingCharges(OriginalHook(GaussRound)))
                            return OriginalHook(Ricochet);
                    }

                    if (gauge.IsOverheated && AutoCrossbow.LevelChecked())
                        return OriginalHook(AutoCrossbow);

                    //gauss and ricochet outside HC
                    if (IsEnabled(CustomComboPreset.MCH_AoE_Adv_GaussRicochet) && Config.MCH_AoE_Hypercharge && CanWeave(actionID) && !gauge.IsOverheated)
                    {
                        if (ActionReady(OriginalHook(GaussRound)) && !WasLastAbility(OriginalHook(GaussRound)))
                            return OriginalHook(GaussRound);

                        if (ActionReady(OriginalHook(Ricochet)) && !WasLastAbility(OriginalHook(Ricochet)))
                            return OriginalHook(Ricochet);
                    }

                    if (IsEnabled(CustomComboPreset.MCH_AoE_Adv_Reassemble) && !HasEffect(Buffs.Wildfire) && !HasEffect(Buffs.Reassembled) && HasCharges(整备Reassemble) && GetRemainingCharges(整备Reassemble) > Config.MCH_AoE_ReassemblePool && ((Config.MCH_AoE_Reassembled[0] && Scattergun.LevelChecked()) || (gauge.IsOverheated && Config.MCH_AoE_Reassembled[1] && AutoCrossbow.LevelChecked()) || (GetCooldownRemainingTime(ChainSaw) < 1 && Config.MCH_AoE_Reassembled[2] && ChainSaw.LevelChecked()) || (GetCooldownRemainingTime(OriginalHook(ChainSaw)) < 1 && Config.MCH_AoE_Reassembled[3] && Excavator.LevelChecked())))
                        return 整备Reassemble;

                    if (IsEnabled(CustomComboPreset.MCH_AoE_Adv_Excavator) && reassembledExcavator && LevelChecked(Excavator) && HasEffect(Buffs.ExcavatorReady))
                        return OriginalHook(ChainSaw);

                    if (IsEnabled(CustomComboPreset.MCH_AoE_Adv_Chainsaw) && reassembledChainsaw && LevelChecked(ChainSaw) && ((GetCooldownRemainingTime(ChainSaw) <= GCD + 0.25) || ActionReady(ChainSaw)))
                        return ChainSaw;

                    if (reassembledScattergun)
                        return OriginalHook(Scattergun);

                    if (reassembledCrossbow && LevelChecked(AutoCrossbow) && gauge.IsOverheated)
                        return AutoCrossbow;

                    if (IsEnabled(CustomComboPreset.MCH_AoE_Adv_SecondWind))
                    {
                        if (PlayerHealthPercentageHp() <= Config.MCH_AoE_SecondWindThreshold && ActionReady(All.SecondWind))
                            return All.SecondWind;
                    }
                }
                return actionID;
            }
        }

        internal class MCH_HeatblastGaussRicochet : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.MCH_Heatblast;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                MCHGauge? gauge = GetJobGauge<MCHGauge>();

                if (actionID is 热冲击HeatBlast or BlazingShot)
                {
                    if (IsEnabled(CustomComboPreset.MCH_Heatblast_AutoBarrel) && ActionReady(BarrelStabilizer) && !gauge.IsOverheated)
                        return BarrelStabilizer;

                    if (IsEnabled(CustomComboPreset.MCH_Heatblast_Wildfire) && ActionReady(Wildfire) && WasLastAbility(Hypercharge))
                        return Wildfire;

                    if (!gauge.IsOverheated && LevelChecked(Hypercharge) && (gauge.Heat >= 50 || HasEffect(Buffs.Hypercharged)))
                        return Hypercharge;

                    //Heatblast, Gauss, Rico
                    if (IsEnabled(CustomComboPreset.MCH_Heatblast_GaussRound) && CanWeave(actionID) && WasLastWeaponskill(OriginalHook(热冲击HeatBlast)) && (ActionWatching.GetAttackType(ActionWatching.LastAction) != ActionWatching.ActionAttackType.Ability))
                    {
                        if (ActionReady(OriginalHook(GaussRound)) && GetRemainingCharges(OriginalHook(GaussRound)) >= GetRemainingCharges(OriginalHook(Ricochet)))
                            return OriginalHook(GaussRound);

                        if (ActionReady(OriginalHook(Ricochet)) && GetRemainingCharges(OriginalHook(Ricochet)) > GetRemainingCharges(OriginalHook(GaussRound)))
                            return OriginalHook(Ricochet);
                    }

                    if (gauge.IsOverheated && LevelChecked(OriginalHook(热冲击HeatBlast)))
                        return OriginalHook(热冲击HeatBlast);
                }
                return actionID;
            }
        }

        internal class MCH_AutoCrossbowGaussRicochet : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.MCH_AutoCrossbow;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                MCHGauge? gauge = GetJobGauge<MCHGauge>();

                if (actionID is AutoCrossbow)
                {
                    if (IsEnabled(CustomComboPreset.MCH_AutoCrossbow_AutoBarrel) && ActionReady(BarrelStabilizer) && !gauge.IsOverheated)
                        return BarrelStabilizer;

                    if (!gauge.IsOverheated && LevelChecked(Hypercharge) && (gauge.Heat >= 50 || HasEffect(Buffs.Hypercharged)))
                        return Hypercharge;

                    //Autocrossbow, Gauss, Rico
                    if (IsEnabled(CustomComboPreset.MCH_AutoCrossbow_GaussRound) && CanWeave(actionID) && WasLastWeaponskill(OriginalHook(AutoCrossbow)) && (ActionWatching.GetAttackType(ActionWatching.LastAction) != ActionWatching.ActionAttackType.Ability))
                    {
                        if (ActionReady(OriginalHook(GaussRound)) && GetRemainingCharges(OriginalHook(GaussRound)) >= GetRemainingCharges(OriginalHook(Ricochet)))
                            return OriginalHook(GaussRound);

                        if (ActionReady(OriginalHook(Ricochet)) && GetRemainingCharges(OriginalHook(Ricochet)) > GetRemainingCharges(OriginalHook(GaussRound)))
                            return OriginalHook(Ricochet);
                    }

                    if (gauge.IsOverheated && LevelChecked(OriginalHook(AutoCrossbow)))
                        return OriginalHook(AutoCrossbow);
                }
                return actionID;
            }
        }

        internal class MCH_GaussRoundRicochet : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.MCH_GaussRoundRicochet;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {

                if (actionID is GaussRound or Ricochet or CheckMate or DoubleCheck)
                {
                    {
                        if (ActionReady(OriginalHook(GaussRound)) && GetRemainingCharges(OriginalHook(GaussRound)) >= GetRemainingCharges(OriginalHook(Ricochet)))
                            return OriginalHook(GaussRound);

                        if (ActionReady(OriginalHook(Ricochet)) && GetRemainingCharges(OriginalHook(Ricochet)) > GetRemainingCharges(OriginalHook(GaussRound)))
                            return OriginalHook(Ricochet);
                    }
                }

                return actionID;
            }
        }

        internal class MCH_Overdrive : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.MCH_Overdrive;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                MCHGauge? gauge = GetJobGauge<MCHGauge>();

                if (actionID is RookAutoturret or AutomatonQueen && gauge.IsRobotActive)
                    return OriginalHook(QueenOverdrive);

                return actionID;
            }
        }

        internal class MCH_HotShotDrillChainsawExcavator : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.MCH_HotShotDrillChainsawExcavator;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID is 钻头Drill or HotShot or AirAnchor or ChainSaw)
                {
                    if (LevelChecked(Excavator) && HasEffect(Buffs.ExcavatorReady))
                        return CalcBestAction(actionID, Excavator, ChainSaw, AirAnchor, 钻头Drill);

                    if (LevelChecked(ChainSaw))
                        return CalcBestAction(actionID, ChainSaw, AirAnchor, 钻头Drill);

                    if (LevelChecked(AirAnchor))
                        return CalcBestAction(actionID, AirAnchor, 钻头Drill);

                    if (LevelChecked(钻头Drill))
                        return CalcBestAction(actionID, 钻头Drill, HotShot);

                    return HotShot;
                }
                return actionID;
            }
        }

        internal class MCH_DismantleTactician : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.MCH_DismantleTactician;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID is Dismantle
                    && (IsOnCooldown(Dismantle) || !LevelChecked(Dismantle))
                    && ActionReady(Tactician)
                    && !HasEffect(Buffs.Tactician))
                    return Tactician;

                return actionID;
            }
        }

        internal class All_PRanged_Dismantle : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.All_PRanged_Dismantle;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID is Dismantle && TargetHasEffectAny(Debuffs.Dismantled) && IsOffCooldown(Dismantle))
                    return OriginalHook(11);

                return actionID;
            }
        }
    }
}