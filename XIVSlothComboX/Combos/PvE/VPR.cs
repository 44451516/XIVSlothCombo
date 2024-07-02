
using XIVSlothComboX.Combos.JobHelpers;
using XIVSlothComboX.CustomComboNS;
using XIVSlothComboX.CustomComboNS.Functions;
using XIVSlothComboX.Data;

namespace XIVSlothComboX.Combos.PvE
{
    internal class VPR
    {
        public const byte JobID = 41;

        public const uint
            DreadFangs = 34607,
            DreadMaw = 34615,
            Dreadwinder = 34620,
            HuntersCoil = 34621,
            HuntersDen = 34624,
            HuntersSnap = 39166,
            PitofDread = 34623,
            RattlingCoil = 39189,
            Reawaken = 34626,
            SerpentsIre = 34647,
            SerpentsTail = 35920,
            Slither = 34646,
            SteelFangs = 34606,
            SteelMaw = 34614,
            SwiftskinsCoil = 34622,
            SwiftskinsDen = 34625,
            Twinblood = 35922,
            Twinfang = 35921,
            UncoiledFury = 34633,
            WrithingSnap = 34632,
            SwiftskinsSting = 34609,
            TwinfangBite = 34636,
            TwinbloodBite = 34637,
            UncoiledTwinfang = 34644,
            UncoiledTwinblood = 34645,
            HindstingStrike = 34612,
            DeathRattle = 34634,
            HuntersSting = 34608,
            HindsbaneFang = 34613,
            FlankstingStrike = 34610,
            FlanksbaneFang = 34611,
            HuntersBite = 34616,
            JaggedMaw = 34618,
            SwiftskinsBite = 34617,
            BloodiedMaw = 34619,
            FirstGeneration = 34627,
            FirstLegacy = 34640,
            SecondGeneration = 34628,
            SecondLegacy = 34641,
            ThirdGeneration = 34629,
            ThirdLegacy = 34642,
            FourthGeneration = 34630,
            FourthLegacy = 34643,
            Ouroboros = 34631;

        public static class Buffs
        {
            public const ushort
                FellhuntersVenom = 3659,
                FellskinsVenom = 3660,
                FlanksbaneVenom = 3646,
                FlankstungVenom = 3645,
                HindstungVenom = 3647,
                HindsbaneVenom = 3648,
                GrimhuntersVenom = 3649,
                GrimskinsVenom = 3650,
                HuntersVenom = 3657,
                SwiftskinsVenom = 3658,
                HuntersInstinct = 3668,
                Swiftscaled = 3669,
                Reawakened = 3670,
                ReadyToReawaken = 3671,
                PoisedForTwinfang = 3665,
                PoisedForTwinblood = 3666;
        }

        public static class Debuffs
        {
            public const ushort
                NoxiousGnash = 3667;
        }

        public static class Traits
        {
            public const uint
                EnhancedVipersRattle = 530,
                EnhancedSerpentsLineage = 533,
                SerpentsLegacy = 534;
        }

        public static class Config
        {
            public static UserInt
                VPR_NoxiousRefreshRange = new("VPR_NoxiousRefreshRange"),
                VPR_ST_SecondWind_Threshold = new("VPR_ST_SecondWindThreshold"),
                VPR_ST_Bloodbath_Threshold = new("VPR_ST_BloodbathThreshold"),
                VPR_AoE_SecondWind_Threshold = new("VPR_AoE_SecondWindThreshold"),
                VPR_AoE_Bloodbath_Threshold = new("VPR_AoE_BloodbathThreshold");
        }

        internal class VPR_ST_SimpleMode : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.VPR_ST_SimpleMode;
            internal static VPROpenerLogic VPROpener = new();

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                var gauge = new TmpVPRGauge();
                bool trueNorthReady = TargetNeedsPositionals() && ActionReady(All.TrueNorth) && !HasEffect(All.Buffs.TrueNorth) && !IsMoving && CanWeave(actionID);
                int NoxiousRefreshRange = Config.VPR_NoxiousRefreshRange;

                if (actionID is SteelFangs)
                {
                    // Opener for VPR
                    if (VPROpener.DoFullOpener(ref actionID, false))
                        return actionID;

                    if (LevelChecked(WrithingSnap) && !InMeleeRange() && HasBattleTarget())
                        return WrithingSnap;

                    //Overcap protection
                    if (HasCharges(Dreadwinder) &&
                        ((gauge.RattlingCoilStacks is 3 && TraitLevelChecked(Traits.EnhancedVipersRattle)) ||
                        (gauge.RattlingCoilStacks is 2 && !TraitLevelChecked(Traits.EnhancedVipersRattle))))
                        return UncoiledFury;

                    //Serpents Ire usage
                    if (ActionReady(SerpentsIre) && CanWeave(actionID))
                        return SerpentsIre;

                    //Reawaken combo
                    if (HasEffect(Buffs.Reawakened))
                    {
                        if (!TraitLevelChecked(Traits.EnhancedSerpentsLineage))
                        {
                            if (gauge.AnguineTribute is 4)
                                return OriginalHook(SteelFangs);

                            if (gauge.AnguineTribute is 3)
                                return OriginalHook(DreadFangs);

                            if (gauge.AnguineTribute is 2)
                                return OriginalHook(HuntersCoil);

                            if (gauge.AnguineTribute is 1)
                                return OriginalHook(SwiftskinsCoil);
                        }

                        if (TraitLevelChecked(Traits.EnhancedSerpentsLineage))
                        {
                            //Legacy weaves
                            if (TraitLevelChecked(Traits.EnhancedSerpentsLineage) && CanWeave(actionID) &&
                                (WasLastAction(OriginalHook(SteelFangs)) || WasLastAction(OriginalHook(DreadFangs)) ||
                                WasLastAction(OriginalHook(HuntersCoil)) || WasLastAction(OriginalHook(SwiftskinsCoil))))
                                return OriginalHook(SerpentsTail);

                            if (gauge.AnguineTribute is 5)
                                return OriginalHook(SteelFangs);

                            if (gauge.AnguineTribute is 4)
                                return OriginalHook(DreadFangs);

                            if (gauge.AnguineTribute is 3)
                                return OriginalHook(HuntersCoil);

                            if (gauge.AnguineTribute is 2)
                                return OriginalHook(SwiftskinsCoil);

                            if (gauge.AnguineTribute is 1)
                                return OriginalHook(Reawaken);
                        }
                    }

                    // Uncoiled Fury usage
                    if (LevelChecked(UncoiledFury) && gauge.HasRattlingCoilStack())
                        return UncoiledFury;

                    // Uncoiled follow-ups
                    if (WasLastWeaponskill(UncoiledFury))
                    {
                        if (HasEffect(Buffs.PoisedForTwinfang))
                            return OriginalHook(Twinfang);

                        if (HasEffect(Buffs.PoisedForTwinblood))
                            return OriginalHook(Twinblood);
                    }

                    // Dreadwinder usage
                    // Dreadwinder combo
                    if (WasLastWeaponskill(HuntersCoil) || WasLastWeaponskill(SwiftskinsCoil))
                    {
                        if (HasEffect(Buffs.HuntersVenom))
                            return OriginalHook(Twinfang);

                        if (HasEffect(Buffs.SwiftskinsVenom))
                            return OriginalHook(Twinblood);
                    }

                    if (WasLastWeaponskill(SwiftskinsCoil))
                        return HuntersCoil;

                    if (gauge.DreadwinderReady && !WasLastWeaponskill(SwiftskinsCoil))
                        return SwiftskinsCoil;



                    //1-2-3 (4-5-6) Combo
                    if (comboTime > 0 && !HasEffect(Buffs.Reawakened))
                    {
                        if (lastComboMove is DreadFangs or SteelFangs)
                        {
                            if (HasEffect(Buffs.FlankstungVenom) || HasEffect(Buffs.FlanksbaneVenom))
                                return OriginalHook(SteelFangs);

                            if (HasEffect(Buffs.HindstungVenom) || HasEffect(Buffs.HindsbaneVenom))
                                return OriginalHook(DreadFangs);
                        }

                        if (lastComboMove is HuntersSting or SwiftskinsSting)
                        {
                            if (HasEffect(Buffs.FlankstungVenom) || HasEffect(Buffs.HindstungVenom))
                            {
                                if (trueNorthReady && !OnTargetsRear() && HasEffect(Buffs.HindstungVenom))
                                    return All.TrueNorth;

                                else if (trueNorthReady && !OnTargetsFlank() && HasEffect(Buffs.FlankstungVenom))
                                    return All.TrueNorth;

                                return OriginalHook(SteelFangs);
                            }

                            if (HasEffect(Buffs.FlanksbaneVenom) || HasEffect(Buffs.HindsbaneVenom))
                            {
                                if (trueNorthReady && !OnTargetsRear() && HasEffect(Buffs.HindsbaneVenom))
                                    return All.TrueNorth;

                                else if (trueNorthReady && !OnTargetsFlank() && HasEffect(Buffs.FlanksbaneVenom))
                                    return All.TrueNorth;

                                return OriginalHook(DreadFangs);
                            }
                        }

                        if (lastComboMove is HindstingStrike or HindsbaneFang or FlankstingStrike or FlanksbaneFang)
                        {
                            if (CanWeave(actionID) && LevelChecked(SerpentsTail) &&
                                (WasLastAction(HindstingStrike) || WasLastAction(HindsbaneFang) ||
                                WasLastAction(FlankstingStrike) || WasLastAction(FlanksbaneFang)))
                                return OriginalHook(SerpentsTail);

                            //Reawakend usage
                            if (HasEffect(Buffs.ReadyToReawaken) || gauge.SerpentsOfferings >= 50)
                                return Reawaken;

                            if (ActionReady(Dreadwinder))
                                return Dreadwinder;

                            if (GetBuffRemainingTime(Buffs.Swiftscaled) < 10 ||
                                (GetDebuffRemainingTime(Debuffs.NoxiousGnash) <= NoxiousRefreshRange))
                                return OriginalHook(DreadFangs);

                            if (GetBuffRemainingTime(Buffs.HuntersInstinct) < 10 ||
                                (GetDebuffRemainingTime(Debuffs.NoxiousGnash) > NoxiousRefreshRange))
                                return OriginalHook(SteelFangs);
                        }
                    }
                    return OriginalHook(DreadFangs);
                }
                return actionID;
            }
        }

        internal class VPR_ST_AdvancedMode : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.VPR_ST_AdvancedMode;
            internal static VPROpenerLogic VPROpener = new();

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                var gauge = new TmpVPRGauge();
                bool trueNorthReady = TargetNeedsPositionals() && ActionReady(All.TrueNorth) && !HasEffect(All.Buffs.TrueNorth) && !IsMoving && CanWeave(actionID);
                int NoxiousRefreshRange = Config.VPR_NoxiousRefreshRange;

                if (actionID is SteelFangs)
                {
                    // Opener for VPR
                    if (IsEnabled(CustomComboPreset.VPR_ST_Opener))
                    {
                        if (VPROpener.DoFullOpener(ref actionID, false))
                            return actionID;
                    }

                    if (IsEnabled(CustomComboPreset.VPR_ST_RangedUptime) &&
                        LevelChecked(WrithingSnap) && !InMeleeRange() && HasBattleTarget())
                        return WrithingSnap;


                    if (IsEnabled(CustomComboPreset.VPR_ST_CDs))
                    {
                        //Overcap protection
                        if (IsEnabled(CustomComboPreset.VPR_ST_UncoiledFury) &&
                            HasCharges(Dreadwinder) &&
                            ((gauge.RattlingCoilStacks is 3 && TraitLevelChecked(Traits.EnhancedVipersRattle)) ||
                            (gauge.RattlingCoilStacks is 2 && !TraitLevelChecked(Traits.EnhancedVipersRattle))))
                            return UncoiledFury;

                        //Serpents Ire usage
                        if (IsEnabled(CustomComboPreset.VPR_ST_SerpentsIre) &&
                            ActionReady(SerpentsIre) && CanWeave(actionID))
                            return SerpentsIre;

                        //Reawaken combo
                        if (IsEnabled(CustomComboPreset.VPR_ST_ReawakenCombo) &&
                            HasEffect(Buffs.Reawakened))
                        {
                            if (!TraitLevelChecked(Traits.EnhancedSerpentsLineage))
                            {
                                if (gauge.AnguineTribute is 4)
                                    return OriginalHook(SteelFangs);

                                if (gauge.AnguineTribute is 3)
                                    return OriginalHook(DreadFangs);

                                if (gauge.AnguineTribute is 2)
                                    return OriginalHook(HuntersCoil);

                                if (gauge.AnguineTribute is 1)
                                    return OriginalHook(SwiftskinsCoil);
                            }

                            if (TraitLevelChecked(Traits.EnhancedSerpentsLineage))
                            {
                                //Legacy weaves
                                if (TraitLevelChecked(Traits.EnhancedSerpentsLineage) && CanWeave(actionID) &&
                                    (WasLastAction(OriginalHook(SteelFangs)) || WasLastAction(OriginalHook(DreadFangs)) ||
                                    WasLastAction(OriginalHook(HuntersCoil)) || WasLastAction(OriginalHook(SwiftskinsCoil))))
                                    return OriginalHook(SerpentsTail);

                                if (gauge.AnguineTribute is 5)
                                    return OriginalHook(SteelFangs);

                                if (gauge.AnguineTribute is 4)
                                    return OriginalHook(DreadFangs);

                                if (gauge.AnguineTribute is 3)
                                    return OriginalHook(HuntersCoil);

                                if (gauge.AnguineTribute is 2)
                                    return OriginalHook(SwiftskinsCoil);

                                if (gauge.AnguineTribute is 1)
                                    return OriginalHook(Reawaken);
                            }
                        }

                        // Uncoiled Fury usage
                        if (IsEnabled(CustomComboPreset.VPR_ST_UncoiledFury))
                        {
                            
                            if (LevelChecked(UncoiledFury) && gauge.HasRattlingCoilStack())
                                return UncoiledFury;

                            // Uncoiled follow-ups
                            if (IsEnabled(CustomComboPreset.VPR_ST_UncoiledFuryCombo))
                            {
                                if (WasLastWeaponskill(UncoiledFury))
                                {
                                    if (HasEffect(Buffs.PoisedForTwinfang))
                                        return OriginalHook(Twinfang);

                                    if (HasEffect(Buffs.PoisedForTwinblood))
                                        return OriginalHook(Twinblood);
                                }
                            }
                        }

                        // Dreadwinder usage
                        if (IsEnabled(CustomComboPreset.VPR_ST_DreadwinderCombo))
                        {
                            // Dreadwinder combo
                            if (WasLastWeaponskill(HuntersCoil) || WasLastWeaponskill(SwiftskinsCoil))
                            {
                                if (HasEffect(Buffs.HuntersVenom))
                                    return OriginalHook(Twinfang);

                                if (HasEffect(Buffs.SwiftskinsVenom))
                                    return OriginalHook(Twinblood);
                            }

                            if (WasLastWeaponskill(SwiftskinsCoil))
                                return HuntersCoil;

                            if (gauge.DreadwinderReady && !WasLastWeaponskill(SwiftskinsCoil))
                                return SwiftskinsCoil;
                        }
                    }

                    // healing
                    if (IsEnabled(CustomComboPreset.VPR_ST_ComboHeals))
                    {
                        if (PlayerHealthPercentageHp() <= Config.VPR_ST_SecondWind_Threshold && ActionReady(All.SecondWind))
                            return All.SecondWind;

                        if (PlayerHealthPercentageHp() <= Config.VPR_ST_Bloodbath_Threshold && ActionReady(All.Bloodbath))
                            return All.Bloodbath;
                    }

                    //1-2-3 (4-5-6) Combo
                    if (comboTime > 0 && !HasEffect(Buffs.Reawakened))
                    {
                        if (lastComboMove is DreadFangs or SteelFangs)
                        {
                            if (HasEffect(Buffs.FlankstungVenom) || HasEffect(Buffs.FlanksbaneVenom))
                                return OriginalHook(SteelFangs);

                            if (HasEffect(Buffs.HindstungVenom) || HasEffect(Buffs.HindsbaneVenom))
                                return OriginalHook(DreadFangs);
                        }

                        if (lastComboMove is HuntersSting or SwiftskinsSting)
                        {
                            if (HasEffect(Buffs.FlankstungVenom) || HasEffect(Buffs.HindstungVenom))
                            {
                                if (IsEnabled(CustomComboPreset.VPR_TrueNorthDynamic) &&
                                    trueNorthReady && !OnTargetsRear() && HasEffect(Buffs.HindstungVenom))
                                    return All.TrueNorth;

                                else if (IsEnabled(CustomComboPreset.VPR_TrueNorthDynamic) &&
                                    trueNorthReady && !OnTargetsFlank() && HasEffect(Buffs.FlankstungVenom))
                                    return All.TrueNorth;

                                return OriginalHook(SteelFangs);
                            }

                            if (HasEffect(Buffs.FlanksbaneVenom) || HasEffect(Buffs.HindsbaneVenom))
                            {
                                if (IsEnabled(CustomComboPreset.VPR_TrueNorthDynamic) &&
                                    trueNorthReady && !OnTargetsRear() && HasEffect(Buffs.HindsbaneVenom))
                                    return All.TrueNorth;

                                else if (IsEnabled(CustomComboPreset.VPR_TrueNorthDynamic) &&
                                    trueNorthReady && !OnTargetsFlank() && HasEffect(Buffs.FlanksbaneVenom))
                                    return All.TrueNorth;

                                return OriginalHook(DreadFangs);
                            }
                        }

                        if (lastComboMove is HindstingStrike or HindsbaneFang or FlankstingStrike or FlanksbaneFang)
                        {
                            if (IsEnabled(CustomComboPreset.VPR_ST_SerpentsTail) &&
                                CanWeave(actionID) && LevelChecked(SerpentsTail) &&
                                (WasLastAction(HindstingStrike) || WasLastAction(HindsbaneFang) ||
                                WasLastAction(FlankstingStrike) || WasLastAction(FlanksbaneFang)))
                                return OriginalHook(SerpentsTail);

                            //Reawakend usage
                            if (IsEnabled(CustomComboPreset.VPR_ST_Reawaken) &&
                                (HasEffect(Buffs.ReadyToReawaken) || gauge.SerpentsOfferings >= 50))
                                return Reawaken;

                            if (IsEnabled(CustomComboPreset.VPR_ST_Dreadwinder) &&
                                ActionReady(Dreadwinder))
                                return Dreadwinder;

                            if (GetBuffRemainingTime(Buffs.Swiftscaled) < 10 ||
                                (GetDebuffRemainingTime(Debuffs.NoxiousGnash) <= NoxiousRefreshRange))
                                return OriginalHook(DreadFangs);

                            if (GetBuffRemainingTime(Buffs.HuntersInstinct) < 10 ||
                                (GetDebuffRemainingTime(Debuffs.NoxiousGnash) > NoxiousRefreshRange))
                                return OriginalHook(SteelFangs);
                        }
                    }
                    return OriginalHook(DreadFangs);
                }
                return actionID;
            }
        }

        internal class VPR_AoE_Simplemode : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.VPR_AoE_SimpleMode;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                var gauge = new TmpVPRGauge();
                int NoxiousRefreshRange = Config.VPR_NoxiousRefreshRange;

                if (actionID is SteelMaw)
                {
                    //Overcap protection
                    if (HasCharges(Dreadwinder) &&
                        ((gauge.RattlingCoilStacks is 3 && TraitLevelChecked(Traits.EnhancedVipersRattle)) ||
                        (gauge.RattlingCoilStacks is 2 && !TraitLevelChecked(Traits.EnhancedVipersRattle))))
                        return UncoiledFury;

                    //Serpents Ire usage
                    if (ActionReady(SerpentsIre) && CanWeave(actionID))
                        return SerpentsIre;

                    if (HasEffect(Buffs.Reawakened))
                    {
                        if (!TraitLevelChecked(Traits.EnhancedSerpentsLineage))
                        {
                            if (gauge.AnguineTribute is 4)
                                return OriginalHook(SteelMaw);

                            if (gauge.AnguineTribute is 3)
                                return OriginalHook(DreadMaw);

                            if (gauge.AnguineTribute is 2)
                                return OriginalHook(HuntersDen);

                            if (gauge.AnguineTribute is 1)
                                return OriginalHook(SwiftskinsDen);
                        }

                        if (TraitLevelChecked(Traits.EnhancedSerpentsLineage))
                        {
                            //Legacy weaves
                            if (TraitLevelChecked(Traits.EnhancedSerpentsLineage) && CanWeave(actionID) &&
                                (WasLastAction(OriginalHook(SteelMaw)) || WasLastAction(OriginalHook(DreadMaw)) ||
                                WasLastAction(OriginalHook(HuntersDen)) || WasLastAction(OriginalHook(SwiftskinsDen))))
                                return OriginalHook(SerpentsTail);

                            if (gauge.AnguineTribute is 5)
                                return OriginalHook(SteelMaw);

                            if (gauge.AnguineTribute is 4)
                                return OriginalHook(DreadMaw);

                            if (gauge.AnguineTribute is 3)
                                return OriginalHook(HuntersDen);

                            if (gauge.AnguineTribute is 2)
                                return OriginalHook(SwiftskinsDen);

                            if (gauge.AnguineTribute is 1)
                                return OriginalHook(Reawaken);
                        }
                    }

                    // Uncoiled Fury usage

                    if (LevelChecked(UncoiledFury) && gauge.HasRattlingCoilStack())
                        return UncoiledFury;

                    // Uncoiled follow-ups

                    if (WasLastWeaponskill(UncoiledFury))
                    {
                        if (HasEffect(Buffs.PoisedForTwinfang))
                            return OriginalHook(Twinfang);

                        if (HasEffect(Buffs.PoisedForTwinblood))
                            return OriginalHook(Twinblood);
                    }

                    // Dreadwinder usage
                    // Dreadwinder combo
                    if (WasLastWeaponskill(HuntersDen) || WasLastWeaponskill(SwiftskinsDen))
                    {
                        if (HasEffect(Buffs.FellhuntersVenom))
                            return OriginalHook(Twinfang);

                        if (HasEffect(Buffs.FellskinsVenom))
                            return OriginalHook(Twinblood);
                    }
                    if (WasLastWeaponskill(SwiftskinsDen))
                        return HuntersDen;

                    if (gauge.PitOfDreadReady && !WasLastWeaponskill(SwiftskinsDen))
                        return SwiftskinsDen;

                    //1-2-3 (4-5-6) Combo
                    if (comboTime > 0 && !HasEffect(Buffs.Reawakened))
                    {
                        if (lastComboMove is DreadMaw or SteelMaw)
                        {
                            if (HasEffect(Buffs.GrimhuntersVenom))
                                return OriginalHook(SteelMaw);

                            if (HasEffect(Buffs.GrimskinsVenom))
                                return OriginalHook(DreadMaw);
                        }

                        if (lastComboMove is HuntersBite or SwiftskinsBite)
                        {
                            if (HasEffect(Buffs.GrimhuntersVenom))
                                return OriginalHook(SteelMaw);

                            if (HasEffect(Buffs.GrimskinsVenom))
                                return OriginalHook(DreadMaw);
                        }

                        if (lastComboMove is JaggedMaw or BloodiedMaw)
                        {
                            if (CanWeave(actionID) && ActionReady(SerpentsTail) &&
                               (WasLastAction(JaggedMaw) || WasLastAction(BloodiedMaw)))
                                return OriginalHook(SerpentsTail);

                            //Reawakend usage
                            if (HasEffect(Buffs.ReadyToReawaken) || gauge.SerpentsOfferings >= 50)
                                return Reawaken;

                            if (ActionReady(PitofDread))
                                return PitofDread;

                            if (GetBuffRemainingTime(Buffs.Swiftscaled) < 10 ||
                                (GetDebuffRemainingTime(Debuffs.NoxiousGnash) <= NoxiousRefreshRange))
                                return OriginalHook(DreadMaw);

                            if (GetBuffRemainingTime(Buffs.HuntersInstinct) < 10 ||
                                (GetDebuffRemainingTime(Debuffs.NoxiousGnash) > NoxiousRefreshRange))
                                return OriginalHook(SteelMaw);
                        }
                    }
                    return OriginalHook(DreadMaw);
                }
                return actionID;
            }
        }

        internal class VPR_AoE_AdvancedMode : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.VPR_AoE_AdvancedMode;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                var gauge = new TmpVPRGauge();
                int NoxiousRefreshRange = Config.VPR_NoxiousRefreshRange;

                if (actionID is SteelMaw)
                {

                    if (IsEnabled(CustomComboPreset.VPR_AoE_CDs))
                    {
                        //Overcap protection
                        if (IsEnabled(CustomComboPreset.VPR_AoE_UncoiledFury) &&
                            HasCharges(Dreadwinder) &&
                            ((gauge.RattlingCoilStacks is 3 && TraitLevelChecked(Traits.EnhancedVipersRattle)) ||
                            (gauge.RattlingCoilStacks is 2 && !TraitLevelChecked(Traits.EnhancedVipersRattle))))
                            return UncoiledFury;

                        //Serpents Ire usage
                        if (IsEnabled(CustomComboPreset.VPR_AoE_SerpentsIre) &&
                            ActionReady(SerpentsIre) && CanWeave(actionID))
                            return SerpentsIre;

                        if (IsEnabled(CustomComboPreset.VPR_AoE_ReawakenCombo) &&
                                 HasEffect(Buffs.Reawakened))
                        {
                            if (!TraitLevelChecked(Traits.EnhancedSerpentsLineage))
                            {
                                if (gauge.AnguineTribute is 4)
                                    return OriginalHook(SteelMaw);

                                if (gauge.AnguineTribute is 3)
                                    return OriginalHook(DreadMaw);

                                if (gauge.AnguineTribute is 2)
                                    return OriginalHook(HuntersDen);

                                if (gauge.AnguineTribute is 1)
                                    return OriginalHook(SwiftskinsDen);
                            }

                            if (TraitLevelChecked(Traits.EnhancedSerpentsLineage))
                            {
                                //Legacy weaves
                                if (TraitLevelChecked(Traits.EnhancedSerpentsLineage) && CanWeave(actionID) &&
                                    (WasLastAction(OriginalHook(SteelMaw)) || WasLastAction(OriginalHook(DreadMaw)) ||
                                    WasLastAction(OriginalHook(HuntersDen)) || WasLastAction(OriginalHook(SwiftskinsDen))))
                                    return OriginalHook(SerpentsTail);

                                if (gauge.AnguineTribute is 5)
                                    return OriginalHook(SteelMaw);

                                if (gauge.AnguineTribute is 4)
                                    return OriginalHook(DreadMaw);

                                if (gauge.AnguineTribute is 3)
                                    return OriginalHook(HuntersDen);

                                if (gauge.AnguineTribute is 2)
                                    return OriginalHook(SwiftskinsDen);

                                if (gauge.AnguineTribute is 1)
                                    return OriginalHook(Reawaken);
                            }
                        }

                        // Uncoiled Fury usage
                        if (IsEnabled(CustomComboPreset.VPR_AoE_UncoiledFury))
                        {
                            if (LevelChecked(UncoiledFury) && gauge.HasRattlingCoilStack())
                                return UncoiledFury;

                            // Uncoiled follow-ups
                            if (IsEnabled(CustomComboPreset.VPR_AoE_UncoiledFuryCombo))
                            {
                                if (WasLastWeaponskill(UncoiledFury))
                                {
                                    if (HasEffect(Buffs.PoisedForTwinfang))
                                        return OriginalHook(Twinfang);

                                    if (HasEffect(Buffs.PoisedForTwinblood))
                                        return OriginalHook(Twinblood);
                                }
                            }
                        }

                        // Dreadwinder usage
                        if (IsEnabled(CustomComboPreset.VPR_AoE_PitOfDreadCombo))
                        {
                            // Dreadwinder combo
                            if (WasLastWeaponskill(HuntersDen) || WasLastWeaponskill(SwiftskinsDen))
                            {
                                if (HasEffect(Buffs.FellhuntersVenom))
                                    return OriginalHook(Twinfang);

                                if (HasEffect(Buffs.FellskinsVenom))
                                    return OriginalHook(Twinblood);
                            }
                            if (WasLastWeaponskill(SwiftskinsDen))
                                return HuntersDen;

                            if (gauge.PitOfDreadReady && !WasLastWeaponskill(SwiftskinsDen))
                                return SwiftskinsDen;
                        }
                    }

                    // healing
                    if (IsEnabled(CustomComboPreset.VPR_AoE_ComboHeals))
                    {
                        if (PlayerHealthPercentageHp() <= Config.VPR_AoE_SecondWind_Threshold && ActionReady(All.SecondWind))
                            return All.SecondWind;

                        if (PlayerHealthPercentageHp() <= Config.VPR_AoE_Bloodbath_Threshold && ActionReady(All.Bloodbath))
                            return All.Bloodbath;
                    }

                    //1-2-3 (4-5-6) Combo
                    if (comboTime > 0 && !HasEffect(Buffs.Reawakened))
                    {
                        if (lastComboMove is DreadMaw or SteelMaw)
                        {
                            if (HasEffect(Buffs.GrimhuntersVenom))
                                return OriginalHook(SteelMaw);

                            if (HasEffect(Buffs.GrimskinsVenom))
                                return OriginalHook(DreadMaw);
                        }

                        if (lastComboMove is HuntersBite or SwiftskinsBite)
                        {
                            if (HasEffect(Buffs.GrimhuntersVenom))
                                return OriginalHook(SteelMaw);

                            if (HasEffect(Buffs.GrimskinsVenom))
                                return OriginalHook(DreadMaw);
                        }

                        if (lastComboMove is JaggedMaw or BloodiedMaw)
                        {
                            if (IsEnabled(CustomComboPreset.VPR_AoE_SerpentsTail) &&
                               CanWeave(actionID) && ActionReady(SerpentsTail) &&
                               (WasLastAction(JaggedMaw) || WasLastAction(BloodiedMaw)))
                                return OriginalHook(SerpentsTail);

                            //Reawakend usage
                            if (IsEnabled(CustomComboPreset.VPR_AoE_Reawaken) &&
                                (HasEffect(Buffs.ReadyToReawaken) || gauge.SerpentsOfferings >= 50))
                                return Reawaken;

                            if (IsEnabled(CustomComboPreset.VPR_AoE_PitOfDread) &&
                                ActionReady(PitofDread))
                                return PitofDread;

                            if (GetBuffRemainingTime(Buffs.Swiftscaled) < 10 ||
                                (GetDebuffRemainingTime(Debuffs.NoxiousGnash) <= NoxiousRefreshRange))
                                return OriginalHook(DreadMaw);

                            if (GetBuffRemainingTime(Buffs.HuntersInstinct) < 10 ||
                                (GetDebuffRemainingTime(Debuffs.NoxiousGnash) > NoxiousRefreshRange))
                                return OriginalHook(SteelMaw);
                        }
                    }
                    return OriginalHook(DreadMaw);
                }
                return actionID;
            }
        }

        internal class VPR_DreadwinderCoils : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.VPR_DreadwinderCoils;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                var gauge = new TmpVPRGauge();

                if (actionID is Dreadwinder)
                {
                    if (WasLastWeaponskill(HuntersCoil) || WasLastWeaponskill(SwiftskinsCoil))
                    {
                        if (HasEffect(Buffs.HuntersVenom))
                            return OriginalHook(Twinfang);

                        if (HasEffect(Buffs.SwiftskinsVenom))
                            return OriginalHook(Twinblood);
                    }

                    if (WasLastWeaponskill(SwiftskinsCoil))
                        return HuntersCoil;

                    if (gauge.DreadwinderReady && !WasLastWeaponskill(SwiftskinsCoil))
                        return SwiftskinsCoil;
                }
                return actionID;
            }
        }

        internal class VPR_PitOfDreadDens : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.VPR_PitOfDreadDens;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                var gauge = new TmpVPRGauge();

                if (actionID is PitofDread)
                {
                    if (WasLastWeaponskill(HuntersDen) || WasLastWeaponskill(SwiftskinsDen))
                    {
                        if (HasEffect(Buffs.FellhuntersVenom))
                            return OriginalHook(Twinfang);

                        if (HasEffect(Buffs.FellskinsVenom))
                            return OriginalHook(Twinblood);
                    }
                    if (WasLastWeaponskill(SwiftskinsDen))
                        return HuntersDen;

                    if (gauge.PitOfDreadReady && !WasLastWeaponskill(SwiftskinsDen))
                        return SwiftskinsDen;
                }
                return actionID;
            }
        }

        internal class VPR_UncoiledTwins : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.VPR_UncoiledTwins;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID is UncoiledFury)
                {
                    if (HasEffect(Buffs.PoisedForTwinfang))
                        return OriginalHook(Twinfang);

                    if (HasEffect(Buffs.PoisedForTwinblood))
                        return OriginalHook(Twinblood);
                }
                return actionID;
            }
        }

        internal class VPR_ReawakenLegacy : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.VPR_ReawakenLegacy;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                var gauge = new TmpVPRGauge();
                if (actionID is Reawaken && HasEffect(Buffs.Reawakened))
                {
                    if (!TraitLevelChecked(Traits.EnhancedSerpentsLineage))
                    {
                        if (gauge.AnguineTribute is 4)
                            return OriginalHook(SteelFangs);

                        if (gauge.AnguineTribute is 3)
                            return OriginalHook(DreadFangs);

                        if (gauge.AnguineTribute is 2)
                            return OriginalHook(HuntersCoil);

                        if (gauge.AnguineTribute is 1)
                            return OriginalHook(SwiftskinsCoil);
                    }

                    if (TraitLevelChecked(Traits.EnhancedSerpentsLineage))
                    {
                        //Legacy weaves
                        if (TraitLevelChecked(Traits.EnhancedSerpentsLineage) && CanWeave(actionID) &&
                            (WasLastAction(OriginalHook(SteelFangs)) || WasLastAction(OriginalHook(DreadFangs)) ||
                            WasLastAction(OriginalHook(HuntersCoil)) || WasLastAction(OriginalHook(SwiftskinsCoil))))
                            return OriginalHook(SerpentsTail);

                        if (gauge.AnguineTribute is 5)
                            return OriginalHook(SteelFangs);

                        if (gauge.AnguineTribute is 4)
                            return OriginalHook(DreadFangs);

                        if (gauge.AnguineTribute is 3)
                            return OriginalHook(HuntersCoil);

                        if (gauge.AnguineTribute is 2)
                            return OriginalHook(SwiftskinsCoil);

                        if (gauge.AnguineTribute is 1)
                            return OriginalHook(Reawaken);
                    }
                }
                return actionID;
            }
        }
    }
}