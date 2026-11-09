using SlothCombo.Runtime.Gauges;
using SlothCombo.Combos.PvE.Content;
using SlothCombo.Core;
using SlothCombo.CustomComboNS;
using SlothCombo.CustomComboNS.Functions;
using SlothCombo.Data;
using SlothCombo.Extensions;
using static SlothCombo.CustomComboNS.Functions.CustomComboFunctions;

namespace SlothCombo.Combos.PvE
{
    internal class RPR
    {
        public const byte JobID = 39;

        public const uint
            // Single Target
            切割Slice = 24373,
            增盈切割WaxingSlice = 24374,
            地狱切割InfernalSlice = 24375,
            死亡之影ShadowOfDeath = 24378,
            ShadowOfDeath = 24378,
            灵魂切割SoulSlice = 24380,
            // AoE
            SpinningScythe = 24376,
            NightmareScythe = 24377,
            WhorlOfDeath = 24379,
            SoulScythe = 24381,
            // Unveiled
            绞决Gibbet = 24382,
            缢杀Gallows = 24383,
            Guillotine = 24384,
            UnveiledGibbet = 24390,
            UnveiledGallows = 24391,
            ExecutionersGibbet = 36970,
            ExecutionersGallows = 36971,
            ExecutionersGuillotine = 36972,

            // Reaver
            隐匿挥割BloodStalk = 24389,
            GrimSwathe = 24392,
            暴食Gluttony = 24393,
            // Sacrifice
            神秘环ArcaneCircle = 24405,
            ArcaneCircle = 24405,
            大丰收PlentifulHarvest = 24385,
            // Enshroud
            夜游魂衣Enshroud = 24394,
            Enshroud = 24394,
            团契Communio = 24398,
            LemuresSlice = 24399,
            LemuresScythe = 24400,
            虚无收割VoidReaping = 24395,
            交错收割CrossReaping = 24396,
            GrimReaping = 24397,
            Sacrificium = 36969,
            完人Perfectio = 36973,
            // Miscellaneous
            HellsIngress = 24401,
            HellsEgress = 24402,
            Regress = 24403,
            勾刃Harpe = 24386,
            播魂种Soulsow = 24387,
            收获月HarvestMoon = 24388;

        public static class Buffs
        {
            public const ushort
                妖异之镰SoulReaver = 2587,
                SoulReaver = 2587,
                死亡祭品ImmortalSacrifice = 2592,
                神秘环ArcaneCircle = 2599,
                绞决效果提高EnhancedGibbet = 2588,
                缢杀效果提高EnhancedGallows = 2589,
                虚无收割效果提高EnhancedVoidReaping = 2590,
                交错收割效果提高EnhancedCrossReaping = 2591,
                EnhancedHarpe = 2845,
                夜游魂Enshrouded = 2593,
                Soulsow = 2594,
                Threshold = 2595,
                BloodsownCircle = 2972,
                IdealHost = 3905,
                Oblatio = 3857,
                处刑人Executioner = 3858,
                Executioner = 3858,
                完人预备PerfectioParata = 3860;
        }

        public static class Debuffs
        {
            public const ushort
                死亡烙印DeathsDesign = 2586;
        }
        protected static RPRGauge Gauge = GetJobGauge<RPRGauge>();
        public static class Config
        {
            public static UserInt
                RPR_SoDThreshold = new("RPRSoDThreshold", 0),
                RPR_WoDThreshold = new("RPRWoDThreshold", 1),
                RPR_SoDRefreshRange = new("RPRSoDRefreshRange", 6),
                RPR_Positional = new("RPR_Positional", 0),
                RPR_VariantCure = new("RPRVariantCure"),
                RPR_STSecondWindThreshold = new("RPR_STSecondWindThreshold", 25),
                RPR_STBloodbathThreshold = new("RPR_STBloodbathThreshold", 40),
                RPR_AoESecondWindThreshold = new("RPR_AoESecondWindThreshold", 25),
                RPR_AoEBloodbathThreshold = new("RPR_AoEBloodbathThreshold", 40);

            public static UserBoolArray
                RPR_SoulsowOptions = new("RPR_SoulsowOptions");

            public static UserBool
                RPR_ST_TrueNorth_Moving = new("RPR_ST_TrueNorth_Moving");
        }
        internal class RPR_ST_CustomMode : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.RPR_ST_CustomMode;


            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID is 增盈切割WaxingSlice)
                {
                    if (OnOpenerCustomActionAction(out var customActionActionId))
                    {
                        return customActionActionId;
                    }
                }


                return actionID;
            }
        }

        internal class RPR_ST_SimpleMode : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.RPR_ST_SimpleMode;
            internal static JobHelpers.RPR.RPROpenerLogic RPROpener = new();

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                double enemyHP = GetTargetHPPercent();

                bool trueNorthReady = TargetNeedsPositionals() && ActionReady(All.TrueNorth) &&
                                      !HasEffect(All.Buffs.TrueNorth) && CanDelayedWeave(actionID);
                int PositionalChoice = Config.RPR_Positional;
                float GCD = GetCooldown(切割Slice).CooldownTotal;
                if (actionID is 切割Slice)
                {
                    //Variant Cure
                    if (IsEnabled(CustomComboPreset.RPR_Variant_Cure) && IsEnabled(Variant.VariantCure) && PlayerHealthPercentageHp() <= GetOptionValue(Config.RPR_VariantCure))
                        return Variant.VariantCure;

                    //Variant Rampart
                    if (IsEnabled(CustomComboPreset.RPR_Variant_Rampart) && IsEnabled(Variant.VariantRampart) && IsOffCooldown(Variant.VariantRampart) && CanWeave(actionID))
                        return Variant.VariantRampart;

                    //RPR Opener
                    if (RPROpener.DoFullOpener(ref actionID))
                        return actionID;

                    //Arcane Circle
                    if (CanWeave(ActionWatching.LastWeaponskill) && LevelChecked(神秘环ArcaneCircle) && ((LevelChecked(夜游魂衣Enshroud) && JustUsed(死亡之影ShadowOfDeath) && IsOffCooldown(神秘环ArcaneCircle)) || (!LevelChecked(夜游魂衣Enshroud) && IsOffCooldown(神秘环ArcaneCircle))))
                        return 神秘环ArcaneCircle;

                    //All Weaves
                    if (CanWeave(ActionWatching.LastWeaponskill))
                    {
                        //Arcane Cirlce
                        if (LevelChecked(ArcaneCircle) &&
                            ((LevelChecked(Enshroud) && IsOffCooldown(ArcaneCircle)) ||
                             (!LevelChecked(Enshroud) && IsOffCooldown(ArcaneCircle))))
                            return ArcaneCircle;
                        
                        //Enshroud
                        if (JobHelpers.RPR.RPRHelpers.UseEnshroud(Gauge))
                            return 夜游魂衣Enshroud;

                        //Gluttony/Bloodstalk
                        if (Gauge.Soul >= 50 && !HasEffect(Buffs.夜游魂Enshrouded) && !HasEffect(Buffs.妖异之镰SoulReaver) && !HasEffect(Buffs.处刑人Executioner) && !HasEffect(Buffs.死亡祭品ImmortalSacrifice) && !HasEffect(Buffs.IdealHost) && !HasEffect(Buffs.完人预备PerfectioParata) && !JobHelpers.RPR.RPRHelpers.IsComboExpiring(3) && GetCooldownRemainingTime(神秘环ArcaneCircle) > GCD * 3)
                        {
                            //Gluttony
                            if (ActionReady(暴食Gluttony))
                            {
                                if (trueNorthReady)
                                    return All.TrueNorth;

                                return 暴食Gluttony;
                            }

                            //Bloodstalk
                            if (LevelChecked(隐匿挥割BloodStalk) && (!LevelChecked(暴食Gluttony) || (LevelChecked(暴食Gluttony) && IsOnCooldown(暴食Gluttony) && (Gauge.Soul is 100 || GetCooldownRemainingTime(暴食Gluttony) > GCD * 4))))
                                return OriginalHook(隐匿挥割BloodStalk);
                        }

                        //Enshroud Weaves
                        if (HasEffect(Buffs.夜游魂Enshrouded))
                        {
                            //Sacrificium
                            if (Gauge.LemureShroud is 2 && Gauge.VoidShroud is 1 && HasEffect(Buffs.Oblatio) && LevelChecked(Sacrificium))
                                return OriginalHook(暴食Gluttony);

                            //Lemure's Slice
                            if (Gauge.VoidShroud >= 2 && LevelChecked(LemuresSlice))
                                return OriginalHook(隐匿挥割BloodStalk);
                        }
                    }

                    //Ranged Attacks
                    if (!InMeleeRange() && LevelChecked(勾刃Harpe) && HasBattleTarget()&&
                        !HasEffect(Buffs.处刑人Executioner) && !HasEffect(Buffs.妖异之镰SoulReaver))
                    {
                        //Communio
                        if (HasEffect(Buffs.夜游魂Enshrouded) && Gauge.LemureShroud is 1 && Gauge.VoidShroud is 0 && LevelChecked(团契Communio))
                            return 团契Communio;

                        return (HasEffect(Buffs.Soulsow) && LevelChecked(收获月HarvestMoon))
                            ? 收获月HarvestMoon
                            : 勾刃Harpe;
                    }

                    //Shadow Of Death
                    if (JobHelpers.RPR.RPRHelpers.UseShadowOfDeath())
                        return 死亡之影ShadowOfDeath;

                    // if (TargetHasEffect(Debuffs.死亡烙印DeathsDesign))
                    {
                        //Perfectio
                        if (HasEffect(Buffs.完人预备PerfectioParata) && LevelChecked(完人Perfectio) && !JobHelpers.RPR.RPRHelpers.IsComboExpiring(1))
                            return OriginalHook(团契Communio);

                        //Gibbet/Gallows
                        if (LevelChecked(绞决Gibbet) && !HasEffect(Buffs.夜游魂Enshrouded) && (HasEffect(Buffs.妖异之镰SoulReaver) || HasEffect(Buffs.处刑人Executioner)))
                        {
                            //Gibbet
                            if (HasEffect(Buffs.绞决效果提高EnhancedGibbet)||
                                (PositionalChoice is 1 && !HasEffect(Buffs.绞决效果提高EnhancedGibbet) &&
                                 !HasEffect(Buffs.缢杀效果提高EnhancedGallows)))
                            {
                                if (trueNorthReady && !OnTargetsFlank())
                                    return All.TrueNorth;

                                return OriginalHook(绞决Gibbet);
                            }

                            //Gallows
                            if (HasEffect(Buffs.缢杀效果提高EnhancedGallows) || (!HasEffect(Buffs.绞决效果提高EnhancedGibbet) && !HasEffect(Buffs.缢杀效果提高EnhancedGallows)))
                            {
                              
                                if (trueNorthReady && !OnTargetsRear())
                                    return All.TrueNorth;

                                return OriginalHook(缢杀Gallows);
                            }
                        }

                        //Plentiful Harvest
                        if (LevelChecked(大丰收PlentifulHarvest) && !HasEffect(Buffs.夜游魂Enshrouded) && !HasEffect(Buffs.妖异之镰SoulReaver) && !HasEffect(Buffs.处刑人Executioner) && HasEffect(Buffs.死亡祭品ImmortalSacrifice) && (GetBuffRemainingTime(Buffs.BloodsownCircle) <= 1 || JustUsed(团契Communio)))
                            return 大丰收PlentifulHarvest;

                        //Enshroud Combo
                        if (HasEffect(Buffs.夜游魂Enshrouded))
                        {
                            //Communio
                            if (Gauge.LemureShroud is 1 && Gauge.VoidShroud is 0 && LevelChecked(团契Communio))
                            {
                                return 团契Communio;
                            }

                            //Void Reaping
                            if (HasEffect(Buffs.虚无收割效果提高EnhancedVoidReaping))
                                return OriginalHook(绞决Gibbet);

                            //Cross Reaping
                            if (HasEffect(Buffs.交错收割效果提高EnhancedCrossReaping) || (!HasEffect(Buffs.交错收割效果提高EnhancedCrossReaping) && !HasEffect(Buffs.虚无收割效果提高EnhancedVoidReaping)))
                                return OriginalHook(缢杀Gallows);
                        }

                        //Soul Slice
                        if (Gauge.Soul <= 50 && ActionReady(灵魂切割SoulSlice) && 
                            !JobHelpers.RPR.RPRHelpers.IsComboExpiring(3) && 
                            !HasEffect(Buffs.夜游魂Enshrouded) && 
                            !HasEffect(Buffs.妖异之镰SoulReaver) && 
                            !HasEffect(Buffs.IdealHost) && 
                            !HasEffect(Buffs.处刑人Executioner) && 
                            !HasEffect(Buffs.完人预备PerfectioParata) && 
                            !HasEffect(Buffs.死亡祭品ImmortalSacrifice))
                            return 灵魂切割SoulSlice;
                    }

                    //Healing
                    if (PlayerHealthPercentageHp() <= 25 && ActionReady(All.SecondWind))
                        return All.SecondWind;

                    if (PlayerHealthPercentageHp() <= 40 && ActionReady(All.Bloodbath))
                        return All.Bloodbath;

                    //1-2-3 Combo
                    if (comboTime > 0)
                    {
                        if (lastComboMove == OriginalHook(切割Slice) && LevelChecked(增盈切割WaxingSlice))
                            return OriginalHook(增盈切割WaxingSlice);

                        if (lastComboMove == OriginalHook(增盈切割WaxingSlice) && LevelChecked(地狱切割InfernalSlice))
                            return OriginalHook(地狱切割InfernalSlice);
                    }
                    return OriginalHook(切割Slice);
                }
                return actionID;
            }
        }

        internal class RPR_ST_AdvancedMode : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.RPR_ST_AdvancedMode;
            internal static JobHelpers.RPR.RPROpenerLogic RPROpener = new();

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                RPRGauge? gauge = GetJobGauge<RPRGauge>();
                double enemyHP = GetTargetHPPercent();
                bool trueNorthReady = TargetNeedsPositionals() && ActionReady(All.TrueNorth) && !HasEffect(All.Buffs.TrueNorth) && CanDelayedWeave(actionID);
                bool trueNorthDynReady = trueNorthReady;
                int PositionalChoice = Config.RPR_Positional;
                float GCD = GetCooldown(切割Slice).CooldownTotal;

                
                // Prevent the dynamic true north option from using the last charge
                if (IsEnabled(CustomComboPreset.RPR_ST_TrueNorthDynamic) && IsEnabled(CustomComboPreset.RPR_ST_TrueNorthDynamic_HoldCharge) && GetRemainingCharges(All.TrueNorth) < 2 && trueNorthReady)
                {
                    trueNorthDynReady = false;
                }

                if (actionID is 切割Slice)
                {
                    //Variant Cure
                    if (IsEnabled(CustomComboPreset.RPR_Variant_Cure) && IsEnabled(Variant.VariantCure) && PlayerHealthPercentageHp() <= GetOptionValue(Config.RPR_VariantCure))
                    {
                        return Variant.VariantCure;
                    }

                    //Variant Rampart
                    if (IsEnabled(CustomComboPreset.RPR_Variant_Rampart) && IsEnabled(Variant.VariantRampart) && IsOffCooldown(Variant.VariantRampart) && CanWeave(actionID))
                    {
                        return Variant.VariantRampart;
                    }

                    //RPR Opener
                    if (IsEnabled(CustomComboPreset.RPR_ST_Opener))
                    {
                        if (RPROpener.DoFullOpener(ref actionID))
                            return actionID;
                    }

                 

                    //All Weaves
                    if (CanWeave(ActionWatching.LastWeaponskill))
                    {
                        
                        if (IsEnabled(CustomComboPreset.RPR_ST_CDs))
                        {
                            
                            //Arcane Cirlce
                            if (IsEnabled(CustomComboPreset.RPR_ST_ArcaneCircle) &&
                                LevelChecked(ArcaneCircle) &&
                                ((LevelChecked(夜游魂衣Enshroud) && ArcaneCircle.ActionReady()) ||
                                 (!LevelChecked(夜游魂衣Enshroud) && ArcaneCircle.ActionReady())))
                                return 神秘环ArcaneCircle;
                            
                            //Enshroud
                            if (IsEnabled(CustomComboPreset.RPR_ST_Enshroud) && JobHelpers.RPR.RPRHelpers.UseEnshroud(gauge))
                            {
                                return 夜游魂衣Enshroud;
                            }

                            //Gluttony/Bloodstalk
                            if (gauge.Soul >= 50 
                                && !HasEffect(Buffs.夜游魂Enshrouded)
                                && !HasEffect(Buffs.妖异之镰SoulReaver) 
                                && !HasEffect(Buffs.处刑人Executioner) 
                                && !HasEffect(Buffs.死亡祭品ImmortalSacrifice)
                                && !HasEffect(Buffs.IdealHost) 
                                && !HasEffect(Buffs.完人预备PerfectioParata) 
                                && (GetCooldownRemainingTime(神秘环ArcaneCircle) > GCD * 3 || !LevelChecked(神秘环ArcaneCircle)) 
                                && !JobHelpers.RPR.RPRHelpers.IsComboExpiring(3))
                            {
                                //Gluttony
                                if (IsEnabled(CustomComboPreset.RPR_ST_Gluttony)  && ActionReady(暴食Gluttony))
                                {
                                    if (IsEnabled(CustomComboPreset.RPR_ST_TrueNorthDynamic) && trueNorthReady)
                                        return All.TrueNorth;

                                    return 暴食Gluttony;
                                }

                                //Bloodstalk
                                if (IsEnabled(CustomComboPreset.RPR_ST_Bloodstalk) && LevelChecked(隐匿挥割BloodStalk) && (!LevelChecked(暴食Gluttony) || (LevelChecked(暴食Gluttony) && IsOnCooldown(暴食Gluttony) && (gauge.Soul is 100 || GetCooldownRemainingTime(暴食Gluttony) > GCD * 4))))
                                    return OriginalHook(隐匿挥割BloodStalk);
                            }
                        }

                        //Enshroud Weaves
                        if (HasEffect(Buffs.夜游魂Enshrouded))
                        {
                            //Sacrificium
                            if (IsEnabled(CustomComboPreset.RPR_ST_Sacrificium) && gauge.LemureShroud is 2 && gauge.VoidShroud is 1 && HasEffect(Buffs.Oblatio) && LevelChecked(Sacrificium))
                                return OriginalHook(暴食Gluttony);

                            //Lemure's Slice
                            if (IsEnabled(CustomComboPreset.RPR_ST_Lemure) && gauge.VoidShroud >= 2 && LevelChecked(LemuresSlice))
                                return OriginalHook(隐匿挥割BloodStalk);
                        }
                    }

                    //Ranged Attacks
                    if (IsEnabled(CustomComboPreset.RPR_ST_RangedFiller) && 
                        !InMeleeRange() && LevelChecked(勾刃Harpe) && HasBattleTarget() &&
                        !HasEffect(Buffs.Executioner) && !HasEffect(Buffs.SoulReaver))
                    {
                        //Communio
                        if (HasEffect(Buffs.夜游魂Enshrouded) && gauge.LemureShroud is 1 && gauge.VoidShroud is 0 && LevelChecked(团契Communio))
                        {
                            return 团契Communio;
                        }

                        return (IsEnabled(CustomComboPreset.RPR_ST_RangedFillerHarvestMoon) && HasEffect(Buffs.Soulsow) && LevelChecked(收获月HarvestMoon))
                            ? 收获月HarvestMoon
                            : 勾刃Harpe;
                    }

                    //Shadow Of Death
                    if (IsEnabled(CustomComboPreset.RPR_ST_SoD) && JobHelpers.RPR.RPRHelpers.UseShadowOfDeath() && enemyHP > Config.RPR_SoDThreshold)
                        return 死亡之影ShadowOfDeath;

                    // if (TargetHasEffect(Debuffs.死亡烙印DeathsDesign))
                    {
                        //Perfectio
                        if (IsEnabled(CustomComboPreset.RPR_ST_Perfectio) && HasEffect(Buffs.完人预备PerfectioParata) && LevelChecked(完人Perfectio) && !JobHelpers.RPR.RPRHelpers.IsComboExpiring(1))
                            return OriginalHook(团契Communio);

                        //Gibbet/Gallows
                        if (IsEnabled(CustomComboPreset.RPR_ST_GibbetGallows) && LevelChecked(绞决Gibbet) && !HasEffect(Buffs.夜游魂Enshrouded) && (HasEffect(Buffs.妖异之镰SoulReaver) || HasEffect(Buffs.处刑人Executioner)))
                        {
                            //Gibbet
                            if (HasEffect(Buffs.绞决效果提高EnhancedGibbet) || (PositionalChoice is 1 && !HasEffect(Buffs.绞决效果提高EnhancedGibbet) && !HasEffect(Buffs.缢杀效果提高EnhancedGallows)))
                            {
                                if (IsEnabled(CustomComboPreset.RPR_ST_TrueNorthDynamic) && trueNorthDynReady && !OnTargetsFlank())
                                    return All.TrueNorth;

                                return OriginalHook(绞决Gibbet);
                            }

                            //Gallows
                            if (HasEffect(Buffs.缢杀效果提高EnhancedGallows) || (PositionalChoice is 0 && !HasEffect(Buffs.绞决效果提高EnhancedGibbet) && !HasEffect(Buffs.缢杀效果提高EnhancedGallows)))
                            {
                                if (IsEnabled(CustomComboPreset.RPR_ST_TrueNorthDynamic) && trueNorthDynReady && !OnTargetsRear())
                                    return All.TrueNorth;

                                return OriginalHook(缢杀Gallows);
                            }
                        }

                        //Plentiful Harvest
                        if (IsEnabled(CustomComboPreset.RPR_ST_CDs) && IsEnabled(CustomComboPreset.RPR_ST_PlentifulHarvest) && LevelChecked(大丰收PlentifulHarvest) && !HasEffect(Buffs.夜游魂Enshrouded) && !HasEffect(Buffs.妖异之镰SoulReaver) && !HasEffect(Buffs.处刑人Executioner) && HasEffect(Buffs.死亡祭品ImmortalSacrifice) && (GetBuffRemainingTime(Buffs.BloodsownCircle) <= 1 || JustUsed(团契Communio)))
                            return 大丰收PlentifulHarvest;

                        //Enshroud Combo
                        if (HasEffect(Buffs.夜游魂Enshrouded))
                        {
                            //Communio
                            if (IsEnabled(CustomComboPreset.RPR_ST_Communio) && gauge.LemureShroud is 1 && gauge.VoidShroud is 0 && LevelChecked(团契Communio))
                                return 团契Communio;

                            //Void Reaping
                            if (IsEnabled(CustomComboPreset.RPR_ST_Reaping) && HasEffect(Buffs.虚无收割效果提高EnhancedVoidReaping))
                                return OriginalHook(绞决Gibbet);

                            //Cross Reaping
                            if (IsEnabled(CustomComboPreset.RPR_ST_Reaping) && (HasEffect(Buffs.交错收割效果提高EnhancedCrossReaping) || (!HasEffect(Buffs.交错收割效果提高EnhancedCrossReaping) && !HasEffect(Buffs.虚无收割效果提高EnhancedVoidReaping))))
                                return OriginalHook(缢杀Gallows);
                        }

                        //Soul Slice
                        if (IsEnabled(CustomComboPreset.RPR_ST_SoulSlice) && gauge.Soul <= 50 && ActionReady(灵魂切割SoulSlice) && !JobHelpers.RPR.RPRHelpers.IsComboExpiring(3) && !HasEffect(Buffs.夜游魂Enshrouded) && !HasEffect(Buffs.妖异之镰SoulReaver) && !HasEffect(Buffs.IdealHost) && !HasEffect(Buffs.处刑人Executioner) && !HasEffect(Buffs.完人预备PerfectioParata) && !HasEffect(Buffs.死亡祭品ImmortalSacrifice))
                            return 灵魂切割SoulSlice;
                    }

                    //Healing
                    if (IsEnabled(CustomComboPreset.RPR_ST_ComboHeals))
                    {
                        if (PlayerHealthPercentageHp() <= Config.RPR_STSecondWindThreshold && ActionReady(All.SecondWind))
                            return All.SecondWind;

                        if (PlayerHealthPercentageHp() <= Config.RPR_STBloodbathThreshold && ActionReady(All.Bloodbath))
                            return All.Bloodbath;
                    }

                    //1-2-3 Combo
                    if (comboTime > 0)
                    {
                        if (lastComboMove == OriginalHook(切割Slice) && LevelChecked(增盈切割WaxingSlice))
                            return OriginalHook(增盈切割WaxingSlice);

                        if (lastComboMove == OriginalHook(增盈切割WaxingSlice) && LevelChecked(地狱切割InfernalSlice))
                            return OriginalHook(地狱切割InfernalSlice);
                    }
                    return OriginalHook(切割Slice);
                }
                return actionID;
            }
        }

        internal class RPR_AoE_SimpleMode : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.RPR_AoE_SimpleMode;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                RPRGauge? gauge = GetJobGauge<RPRGauge>();
                float GCD = GetCooldown(SpinningScythe).CooldownTotal;

                if (actionID is SpinningScythe)
                {
                    if (IsEnabled(CustomComboPreset.RPR_Variant_Cure) && IsEnabled(Variant.VariantCure) && PlayerHealthPercentageHp() <= GetOptionValue(Config.RPR_VariantCure))
                        return Variant.VariantCure;

                    if (IsEnabled(CustomComboPreset.RPR_Variant_Rampart) && IsEnabled(Variant.VariantRampart) && IsOffCooldown(Variant.VariantRampart) && CanWeave(actionID))
                        return Variant.VariantRampart;

                    if (LevelChecked(WhorlOfDeath) && GetDebuffRemainingTime(Debuffs.死亡烙印DeathsDesign) < 6 && !HasEffect(Buffs.妖异之镰SoulReaver) && !HasEffect(Buffs.处刑人Executioner))
                        return WhorlOfDeath;

                    // if (TargetHasEffect(Debuffs.死亡烙印DeathsDesign))
                    {
                        if (HasEffect(Buffs.完人预备PerfectioParata) && LevelChecked(完人Perfectio))
                            return OriginalHook(团契Communio);

                        if (HasEffect(Buffs.死亡祭品ImmortalSacrifice) && LevelChecked(大丰收PlentifulHarvest) && !HasEffect(Buffs.妖异之镰SoulReaver) && !HasEffect(Buffs.夜游魂Enshrouded) && !HasEffect(Buffs.处刑人Executioner) && (GetBuffRemainingTime(Buffs.BloodsownCircle) <= 1 || JustUsed(团契Communio)))
                            return 大丰收PlentifulHarvest;

                        if (CanWeave(actionID))
                        {
                            if (LevelChecked(神秘环ArcaneCircle) && ((GetCooldownRemainingTime(神秘环ArcaneCircle) <= GCD + 0.25) || ActionReady(神秘环ArcaneCircle)))
                                return 神秘环ArcaneCircle;

                            if (!HasEffect(Buffs.妖异之镰SoulReaver) && !HasEffect(Buffs.夜游魂Enshrouded) && !HasEffect(Buffs.处刑人Executioner) && ActionReady(夜游魂衣Enshroud) && (gauge.Shroud >= 50 || HasEffect(Buffs.IdealHost)) && !JobHelpers.RPR.RPRHelpers.IsComboExpiring(6))
                                return 夜游魂衣Enshroud;

                            if (LevelChecked(暴食Gluttony) && gauge.Soul >= 50 && !HasEffect(Buffs.夜游魂Enshrouded) && !HasEffect(Buffs.妖异之镰SoulReaver) && !HasEffect(Buffs.死亡祭品ImmortalSacrifice) && ((GetCooldownRemainingTime(暴食Gluttony) <= GetCooldownRemainingTime(切割Slice) + 0.25) || ActionReady(暴食Gluttony)))
                                return 暴食Gluttony;

                            if (LevelChecked(GrimSwathe) && !HasEffect(Buffs.夜游魂Enshrouded) && !HasEffect(Buffs.妖异之镰SoulReaver) && !HasEffect(Buffs.死亡祭品ImmortalSacrifice) && !HasEffect(Buffs.处刑人Executioner) && gauge.Soul >= 50 && (!LevelChecked(暴食Gluttony) || (LevelChecked(暴食Gluttony) && (gauge.Soul is 100 || GetCooldownRemainingTime(暴食Gluttony) > GCD * 5))))
                                return GrimSwathe;
                        }

                        if (!HasEffect(Buffs.夜游魂Enshrouded) && !HasEffect(Buffs.妖异之镰SoulReaver) && !HasEffect(Buffs.处刑人Executioner) && !HasEffect(Buffs.完人预备PerfectioParata) && ActionReady(SoulScythe) && gauge.Soul <= 50)
                            return SoulScythe;

                        if (HasEffect(Buffs.夜游魂Enshrouded))
                        {
                            if (gauge.LemureShroud is 1 && gauge.VoidShroud is 0 && ActionReady(团契Communio))
                                return 团契Communio;

                            if (gauge.LemureShroud is 2 && gauge.VoidShroud is 1 && HasEffect(Buffs.Oblatio))
                                return OriginalHook(暴食Gluttony);

                            if (gauge.VoidShroud >= 2 && LevelChecked(LemuresScythe) && CanWeave(actionID))
                                return OriginalHook(GrimSwathe);

                            if (gauge.LemureShroud > 0)
                                return OriginalHook(Guillotine);
                        }
                    }

                    if (HasEffect(Buffs.妖异之镰SoulReaver)
                        || (HasEffect(Buffs.处刑人Executioner)
                            && !HasEffect(Buffs.夜游魂Enshrouded)
                            && LevelChecked(Guillotine)))
                        return OriginalHook(Guillotine);

                    return lastComboMove == OriginalHook(SpinningScythe) && LevelChecked(NightmareScythe)
                        ? OriginalHook(NightmareScythe)
                        : OriginalHook(SpinningScythe);
                }
                return actionID;
            }
        }

        internal class RPR_AoE_AdvancedMode : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.RPR_AoE_AdvancedMode;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                RPRGauge? gauge = GetJobGauge<RPRGauge>();
                double enemyHP = GetTargetHPPercent();
                float GCD = GetCooldown(SpinningScythe).CooldownTotal;

                if (actionID is SpinningScythe)
                {
                    if (IsEnabled(CustomComboPreset.RPR_Variant_Cure) && IsEnabled(Variant.VariantCure) && PlayerHealthPercentageHp() <= GetOptionValue(Config.RPR_VariantCure))
                        return Variant.VariantCure;

                    if (IsEnabled(CustomComboPreset.RPR_Variant_Rampart) && IsEnabled(Variant.VariantRampart) && IsOffCooldown(Variant.VariantRampart) && CanWeave(actionID))
                        return Variant.VariantRampart;

                    if (IsEnabled(CustomComboPreset.RPR_AoE_WoD) && LevelChecked(WhorlOfDeath) && GetDebuffRemainingTime(Debuffs.死亡烙印DeathsDesign) < 6 && !HasEffect(Buffs.妖异之镰SoulReaver) && enemyHP > Config.RPR_WoDThreshold)
                        return WhorlOfDeath;

                    // if (TargetHasEffect(Debuffs.死亡烙印DeathsDesign))
                    {
                        if (IsEnabled(CustomComboPreset.RPR_AoE_Perfectio) && HasEffect(Buffs.完人预备PerfectioParata) && LevelChecked(完人Perfectio))
                            return OriginalHook(团契Communio);

                        if (IsEnabled(CustomComboPreset.RPR_AoE_CDs))
                        {
                            if (IsEnabled(CustomComboPreset.RPR_AoE_PlentifulHarvest) && HasEffect(Buffs.死亡祭品ImmortalSacrifice) && LevelChecked(大丰收PlentifulHarvest) && !HasEffect(Buffs.妖异之镰SoulReaver) && !HasEffect(Buffs.夜游魂Enshrouded) && (GetBuffRemainingTime(Buffs.BloodsownCircle) <= 1 || JustUsed(团契Communio)))
                                return 大丰收PlentifulHarvest;

                            if (CanWeave(actionID))
                            {
                                if (IsEnabled(CustomComboPreset.RPR_AoE_ArcaneCircle) && LevelChecked(神秘环ArcaneCircle) && ((GetCooldownRemainingTime(神秘环ArcaneCircle) <= GCD + 0.25) || ActionReady(神秘环ArcaneCircle)))
                                    return 神秘环ArcaneCircle;

                                if (IsEnabled(CustomComboPreset.RPR_AoE_Enshroud) && !HasEffect(Buffs.妖异之镰SoulReaver) && !HasEffect(Buffs.夜游魂Enshrouded) && ActionReady(夜游魂衣Enshroud) && (gauge.Shroud >= 50 || HasEffect(Buffs.IdealHost)) && !JobHelpers.RPR.RPRHelpers.IsComboExpiring(6))
                                    return 夜游魂衣Enshroud;

                                if (IsEnabled(CustomComboPreset.RPR_AoE_Gluttony) && LevelChecked(暴食Gluttony) && gauge.Soul >= 50 && !HasEffect(Buffs.夜游魂Enshrouded) && !HasEffect(Buffs.妖异之镰SoulReaver) && !HasEffect(Buffs.死亡祭品ImmortalSacrifice) && ((GetCooldownRemainingTime(暴食Gluttony) <= GetCooldownRemainingTime(切割Slice) + 0.25) || ActionReady(暴食Gluttony)))
                                    return 暴食Gluttony;

                                if (IsEnabled(CustomComboPreset.RPR_AoE_GrimSwathe) && LevelChecked(GrimSwathe) && !HasEffect(Buffs.夜游魂Enshrouded) && !HasEffect(Buffs.妖异之镰SoulReaver) && !HasEffect(Buffs.死亡祭品ImmortalSacrifice) && gauge.Soul >= 50 && (!LevelChecked(暴食Gluttony) || (LevelChecked(暴食Gluttony) && (gauge.Soul is 100 || GetCooldownRemainingTime(暴食Gluttony) > GCD * 5))))
                                    return GrimSwathe;
                            }
                        }

                        if (IsEnabled(CustomComboPreset.RPR_AoE_SoulScythe) && !HasEffect(Buffs.夜游魂Enshrouded) && !HasEffect(Buffs.妖异之镰SoulReaver) && !HasEffect(Buffs.处刑人Executioner) && !HasEffect(Buffs.完人预备PerfectioParata) && ActionReady(SoulScythe) && gauge.Soul <= 50)
                            return SoulScythe;

                        if (HasEffect(Buffs.夜游魂Enshrouded))
                        {
                            if (IsEnabled(CustomComboPreset.RPR_AoE_Communio) && gauge.LemureShroud is 1 && gauge.VoidShroud is 0 && ActionReady(团契Communio))
                                return 团契Communio;

                            if (IsEnabled(CustomComboPreset.RPR_AoE_Sacrificium) && gauge.LemureShroud is 2 && gauge.VoidShroud is 1 && HasEffect(Buffs.Oblatio) && CanWeave(actionID))
                                return OriginalHook(暴食Gluttony);

                            if (IsEnabled(CustomComboPreset.RPR_AoE_Lemure) && gauge.VoidShroud >= 2 && LevelChecked(LemuresScythe) && CanWeave(actionID))
                                return OriginalHook(GrimSwathe);

                            if (IsEnabled(CustomComboPreset.RPR_AoE_Reaping) && gauge.LemureShroud > 0)
                                return OriginalHook(Guillotine);
                        }
                    }

                    if (IsEnabled(CustomComboPreset.RPR_AoE_ComboHeals))
                    {
                        if (PlayerHealthPercentageHp() <= Config.RPR_AoESecondWindThreshold && ActionReady(All.SecondWind))
                            return All.SecondWind;

                        if (PlayerHealthPercentageHp() <= Config.RPR_AoEBloodbathThreshold && ActionReady(All.Bloodbath))
                            return All.Bloodbath;
                    }

                    if (IsEnabled(CustomComboPreset.RPR_AoE_Guillotine)
                        && (HasEffect(Buffs.妖异之镰SoulReaver)
                            || (HasEffect(Buffs.处刑人Executioner)
                                && !HasEffect(Buffs.夜游魂Enshrouded)
                                && LevelChecked(Guillotine))))
                        return OriginalHook(Guillotine);

                    return lastComboMove == OriginalHook(SpinningScythe) && LevelChecked(NightmareScythe)
                        ? OriginalHook(NightmareScythe)
                        : OriginalHook(SpinningScythe);
                }
                return actionID;
            }
        }

        internal class RPR_GluttonyBloodSwathe : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.RPR_GluttonyBloodSwathe;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                RPRGauge? gauge = GetJobGauge<RPRGauge>();
                bool trueNorthReady = TargetNeedsPositionals() && ActionReady(All.TrueNorth) && !HasEffect(All.Buffs.TrueNorth);

                if (actionID is GrimSwathe)
                {
                    if (IsEnabled(CustomComboPreset.RPR_GluttonyBloodSwathe_Enshroud))
                    {
                        if (HasEffect(Buffs.完人预备PerfectioParata) && LevelChecked(完人Perfectio))
                            return OriginalHook(团契Communio);

                        if (HasEffect(Buffs.夜游魂Enshrouded))
                        {
                            if (gauge.LemureShroud == 1 && gauge.VoidShroud == 0 && LevelChecked(团契Communio))
                                return 团契Communio;

                            if (gauge.LemureShroud is 2 && gauge.VoidShroud is 1 && HasEffect(Buffs.Oblatio))
                                return OriginalHook(暴食Gluttony);

                            if (gauge.VoidShroud >= 2 && LevelChecked(LemuresScythe))
                                return OriginalHook(GrimSwathe);

                            if (gauge.LemureShroud > 1)
                                return OriginalHook(Guillotine);
                        }
                    }
                    
                    if (IsEnabled(CustomComboPreset.RPR_GluttonyBloodSwathe_Sacrificium) &&
                        HasEffect(Buffs.夜游魂Enshrouded) && HasEffect(Buffs.Oblatio))
                        return OriginalHook(暴食Gluttony);

                    if (ActionReady(暴食Gluttony) && !HasEffect(Buffs.夜游魂Enshrouded) && !HasEffect(Buffs.妖异之镰SoulReaver))
                        return 暴食Gluttony;

                    if (IsEnabled(CustomComboPreset.RPR_GluttonyBloodSwathe_BloodSwatheCombo) && (HasEffect(Buffs.妖异之镰SoulReaver) || HasEffect(Buffs.处刑人Executioner)) && LevelChecked(Guillotine))
                        return Guillotine;
                }

                if (actionID is 隐匿挥割BloodStalk)
                {
                    if (IsEnabled(CustomComboPreset.RPR_TrueNorthGluttony) && trueNorthReady)
                        return All.TrueNorth;

                    if (IsEnabled(CustomComboPreset.RPR_GluttonyBloodSwathe_Enshroud))
                    {
                        if (HasEffect(Buffs.完人预备PerfectioParata) && LevelChecked(完人Perfectio))
                            return OriginalHook(团契Communio);

                        if (HasEffect(Buffs.夜游魂Enshrouded))
                        {
                            if (gauge.LemureShroud == 1 && gauge.VoidShroud == 0 && LevelChecked(团契Communio))
                                return 团契Communio;

                            if (gauge.LemureShroud is 2 && gauge.VoidShroud is 1 && HasEffect(Buffs.Oblatio))
                                return OriginalHook(暴食Gluttony);

                            if (gauge.VoidShroud >= 2 && LevelChecked(LemuresSlice))
                                return OriginalHook(隐匿挥割BloodStalk);

                            if (HasEffect(Buffs.虚无收割效果提高EnhancedVoidReaping))
                                return OriginalHook(绞决Gibbet);

                            if (HasEffect(Buffs.交错收割效果提高EnhancedCrossReaping) ||
                                (!HasEffect(Buffs.交错收割效果提高EnhancedCrossReaping) && !HasEffect(Buffs.虚无收割效果提高EnhancedVoidReaping)))
                                return OriginalHook(缢杀Gallows);
                        }
                    }

                    if (ActionReady(暴食Gluttony) && !HasEffect(Buffs.夜游魂Enshrouded) && !HasEffect(Buffs.妖异之镰SoulReaver))
                        return 暴食Gluttony;

                    
                    if (IsEnabled(CustomComboPreset.RPR_GluttonyBloodSwathe_Sacrificium) &&
                        HasEffect(Buffs.夜游魂Enshrouded) &&  HasEffect(Buffs.Oblatio))
                        return OriginalHook(暴食Gluttony);
                    
                    if (IsEnabled(CustomComboPreset.RPR_GluttonyBloodSwathe_BloodSwatheCombo) && (HasEffect(Buffs.妖异之镰SoulReaver) || HasEffect(Buffs.处刑人Executioner)) && LevelChecked(绞决Gibbet))
                    {
                        if (HasEffect(Buffs.绞决效果提高EnhancedGibbet))
                            return OriginalHook(绞决Gibbet);

                        if (HasEffect(Buffs.缢杀效果提高EnhancedGallows) || (!HasEffect(Buffs.绞决效果提高EnhancedGibbet) && !HasEffect(Buffs.缢杀效果提高EnhancedGallows)))
                            return OriginalHook(缢杀Gallows);
                    }
                }
                return actionID;
            }
        }

        internal class RPR_ArcaneCirclePlentifulHarvest : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.RPR_ArcaneCirclePlentifulHarvest;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID is 神秘环ArcaneCircle)
                {
                    if (HasEffect(Buffs.死亡祭品ImmortalSacrifice) && LevelChecked(大丰收PlentifulHarvest))
                        return 大丰收PlentifulHarvest;
                }
                return actionID;
            }
        }

        internal class RPR_Regress : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.RPR_Regress;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                return (actionID is HellsEgress or HellsIngress) && FindEffect(Buffs.Threshold)?.RemainingTime <= 9
                    ? Regress
                    : actionID;
            }
        }

        internal class RPR_Soulsow : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.RPR_Soulsow;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                var soulSowOptions = PluginConfiguration.GetCustomBoolArrayValue(Config.RPR_SoulsowOptions);
                bool soulsowReady = LevelChecked(播魂种Soulsow) && !HasEffect(Buffs.Soulsow);

                return (((soulSowOptions.Length > 0) && ((actionID is 勾刃Harpe && soulSowOptions[0]) || (actionID is 切割Slice && soulSowOptions[1]) || (actionID is SpinningScythe && soulSowOptions[2]) || (actionID is 死亡之影ShadowOfDeath && soulSowOptions[3]) || (actionID is 隐匿挥割BloodStalk && soulSowOptions[4])) && soulsowReady && !InCombat()) || (IsEnabled(CustomComboPreset.RPR_Soulsow_Combat) && actionID is 勾刃Harpe && !HasBattleTarget())) ? 播魂种Soulsow : actionID;
            }
        }

        internal class RPR_EnshroudProtection : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.RPR_EnshroudProtection;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                bool trueNorthReady = TargetNeedsPositionals() && ActionReady(All.TrueNorth) && !HasEffect(All.Buffs.TrueNorth);

                if (actionID is 夜游魂衣Enshroud)
                {
                    if (IsEnabled(CustomComboPreset.RPR_TrueNorthEnshroud) && GetBuffStacks(Buffs.妖异之镰SoulReaver) is 2 && trueNorthReady && CanDelayedWeave(切割Slice))
                    {
                        return All.TrueNorth;
                    }



                    if (HasEffect(Buffs.完人预备PerfectioParata) && LevelChecked(完人Perfectio))
                    {
                        return OriginalHook(团契Communio);
                    }
                    
                    RPRGauge gauge = GetJobGauge<RPRGauge>();
                    
                    if (HasEffect(Buffs.夜游魂Enshrouded))
                    {
                        
                        if (gauge.LemureShroud == 1 && gauge.VoidShroud == 0 && LevelChecked(团契Communio))
                            return 团契Communio;

                        if (gauge.LemureShroud is 2 && gauge.VoidShroud is 1 && HasEffect(Buffs.Oblatio))
                            return OriginalHook(暴食Gluttony);

                        if (gauge.VoidShroud >= 2 && LevelChecked(LemuresSlice))
                            return OriginalHook(隐匿挥割BloodStalk);

                        if (HasEffect(Buffs.虚无收割效果提高EnhancedVoidReaping))
                            return OriginalHook(绞决Gibbet);

                        if (HasEffect(Buffs.交错收割效果提高EnhancedCrossReaping) ||
                            (!HasEffect(Buffs.交错收割效果提高EnhancedCrossReaping) && !HasEffect(Buffs.虚无收割效果提高EnhancedVoidReaping)))
                            return OriginalHook(缢杀Gallows);
                    }
                    
                    if (HasEffect(Buffs.妖异之镰SoulReaver))
                    {
                        if (HasEffect(Buffs.绞决效果提高EnhancedGibbet))
                        {
                            return OriginalHook(绞决Gibbet);
                        }

                        if (HasEffect(Buffs.缢杀效果提高EnhancedGallows) || (!HasEffect(Buffs.绞决效果提高EnhancedGibbet) && !HasEffect(Buffs.缢杀效果提高EnhancedGallows)))
                        {
                            return OriginalHook(缢杀Gallows);
                        }
                    }
                }
                return actionID;
            }
        }

        internal class RPR_CommunioOnGGG : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.RPR_CommunioOnGGG;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                RPRGauge? gauge = GetJobGauge<RPRGauge>();

                if (actionID is 绞决Gibbet or 缢杀Gallows && HasEffect(Buffs.夜游魂Enshrouded))
                {
                    if (gauge.LemureShroud == 1 && gauge.VoidShroud == 0 && LevelChecked(团契Communio))
                        return 团契Communio;

                    if (IsEnabled(CustomComboPreset.RPR_LemureOnGGG) && gauge.VoidShroud >= 2 && LevelChecked(LemuresSlice) && CanWeave(actionID))
                        return OriginalHook(隐匿挥割BloodStalk);
                }

                if (actionID is Guillotine && HasEffect(Buffs.夜游魂Enshrouded))
                {
                    if (gauge.LemureShroud == 1 && gauge.VoidShroud == 0 && LevelChecked(团契Communio))
                        return 团契Communio;

                    if (IsEnabled(CustomComboPreset.RPR_LemureOnGGG) && gauge.VoidShroud >= 2 && LevelChecked(LemuresScythe) && CanWeave(actionID))
                        return OriginalHook(GrimSwathe);
                }

                return actionID;
            }
        }

        internal class RPR_EnshroudCommunio : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.RPR_EnshroudCommunio;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID is 夜游魂衣Enshroud)
                {
                    if (HasEffect(Buffs.完人预备PerfectioParata) && LevelChecked(完人Perfectio))
                        return OriginalHook(团契Communio);

                    if (HasEffect(Buffs.夜游魂Enshrouded) && LevelChecked(团契Communio))
                        return 团契Communio;
                }
                return actionID;
            }
        }
    }
}