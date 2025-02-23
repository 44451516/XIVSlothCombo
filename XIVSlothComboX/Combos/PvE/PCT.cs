using Dalamud.Game.ClientState.JobGauge.Types;
using XIVSlothComboX.Combos.JobHelpers;
using XIVSlothComboX.Combos.PvE.Content;
using XIVSlothComboX.Core;
using XIVSlothComboX.CustomComboNS;
using XIVSlothComboX.CustomComboNS.Functions;
using XIVSlothComboX.Data;
using XIVSlothComboX.Extensions;

namespace XIVSlothComboX.Combos.PvE
{
    internal class PCT
    {
        public const byte JobID = 42;

        public const uint
            冰结之蓝青BlizzardinCyan = 34653,
            飞石之纯黄StoneinYellow = 34654,
            BlizzardIIinCyan = 34659,
            ClawMotif = 34666,
            ClawedMuse = 34672,
            CometinBlack = 34663,
            动物彩绘CreatureMotif = 34689,
            火炎之红FireInRed = 34650,
            疾风之绿AeroInGreen = 34651,
            WaterInBlue = 34652,
            FireIIinRed = 34656,
            AeroIIinGreen = 34657,
            HammerMotif = 34668,
            WingedMuse = 34671,
            StrikingMuse = 34674,
            星空构想StarryMuse = 34675,
            HammerStamp = 34678,
            HammerBrush = 34679,
            PolishingHammer = 34680,
            HolyInWhite = 34662,
            StarrySkyMotif = 34669,
            风景彩绘LandscapeMotif = 34691,
            LivingMuse = 35347,
            MawMotif = 34667,
            MogoftheAges = 34676,
            PomMotif = 34664,
            PomMuse = 34670,
            RainbowDrip = 34688,
            RetributionoftheMadeen = 34677,
            风景构想ScenicMuse = 35349,
            Smudge = 34684,
            天星棱光StarPrism = 34681,
            SteelMuse = 35348,
            减色混合SubtractivePalette = 34683,
            StoneIIinYellow = 34660,
            ThunderIIinMagenta = 34661,
            ThunderinMagenta = 34655,
            WaterinBlue = 34652,
            武器彩绘WeaponMotif = 34690,
            WingMotif = 34665;

        public static class Buffs
        {
            public const ushort
                SubtractivePalette = 3674,
                RainbowBright = 3679,
                HammerTime = 3680,
                MonochromeTones = 3691,
                星空构想StarryMuse = 3685,
                绘灵幻景Hyperphantasia = 3688,
                Inspiration = 3689,
                SubtractiveSpectrum = 3690,
                Starstruck = 3681;
        }

        public static class Debuffs
        {
        }

        public static class Config
        {
            public static UserInt
                CombinedAetherhueChoices = new("CombinedAetherhueChoices"),
                PCT_ST_AdvancedMode_LucidOption = new("PCT_ST_AdvancedMode_LucidOption", 6500),
                PCT_AoE_AdvancedMode_HolyinWhiteOption = new("PCT_AoE_AdvancedMode_HolyinWhiteOption", 0),
                PCT_AoE_AdvancedMode_LucidOption = new("PCT_AoE_AdvancedMode_LucidOption", 6500),
                PCT_VariantCure = new("PCT_VariantCure"),
                PCT_ST_CreatureStop = new("PCT_ST_CreatureStop"),
                PCT_AoE_CreatureStop = new("PCT_AoE_CreatureStop"),
                PCT_ST_WeaponStop = new("PCT_ST_WeaponStop"),
                PCT_AoE_WeaponStop = new("PCT_AoE_WeaponStop"),
                PCT_ST_LandscapeStop = new("PCT_ST_LandscapeStop"),
                PCT_AoE_LandscapeStop = new("PCT_AoE_LandscapeStop"),
                PCT_SubtractivePalette = new("PCT_SubtractivePalette",100)
                ;

            public static UserBool
                CombinedMotifsMog = new("CombinedMotifsMog"),
                CombinedMotifsMadeen = new("CombinedMotifsMadeen"),
                CombinedMotifsWeapon = new("CombinedMotifsWeapon");
        }
        internal class PCT_ST_CustomMode : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.PCT_ST_CustomMode;


            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID is All.Sleep)
                {
                    if (OnOpenerCustomActionAction(out var customActionActionId))
                    {
                        return customActionActionId;
                    }
                }


                return actionID;
            }
        }


        internal class PCT_ST_SimpleMode : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.PCT_ST_SimpleMode;
            internal static PCTOpenerLogicLvl100 PCTOpenerLvl100 = new();
            internal static PCTOpenerLogicLvl92 PCTOpenerLvl92 = new();
            internal static PCTOpenerLogicLvl90 PCTOpenerLvl90 = new();
            internal static PCTOpenerLogicLvl80 PCTOpenerLvl80 = new();
            internal static PCTOpenerLogicLvl70 PCTOpenerLvl70 = new();

            protected override uint Invoke(uint actionID, uint lastComboActionID, float comboTime, byte level)
            {
                if (actionID is 火炎之红FireInRed)
                {
                    var gauge = GetJobGauge<PCTGauge>();
                    bool canWeave = CanSpellWeave(ActionWatching.LastSpell) || CanSpellWeave(actionID);

                    // Variant Cure
                    if (IsEnabled(CustomComboPreset.PCT_Variant_Cure) && IsEnabled(Variant.VariantCure) && PlayerHealthPercentageHp() <= GetOptionValue(Config.PCT_VariantCure))
                        return Variant.VariantCure;

                    // Variant Rampart
                    if (IsEnabled(CustomComboPreset.PCT_Variant_Rampart) && IsEnabled(Variant.VariantRampart) && IsOffCooldown(Variant.VariantRampart) && canWeave)
                        return Variant.VariantRampart;

                    // Prepull logic

                    if (!InCombat() || InCombat() && CurrentTarget == null)
                    {
                        if (动物彩绘CreatureMotif.LevelChecked() && !gauge.CreatureMotifDrawn)
                            return OriginalHook(动物彩绘CreatureMotif);
                        if (武器彩绘WeaponMotif.LevelChecked() && !gauge.WeaponMotifDrawn && !HasEffect(Buffs.HammerTime))
                            return OriginalHook(武器彩绘WeaponMotif);
                        
                        if (风景彩绘LandscapeMotif.LevelChecked() && !gauge.LandscapeMotifDrawn && !HasEffect(Buffs.星空构想StarryMuse))
                            return OriginalHook(风景彩绘LandscapeMotif);
                    }

                    // Lvl 100 Opener
                    if (天星棱光StarPrism.LevelChecked())
                    {
                        if (PCTOpenerLvl100.DoFullOpener(ref actionID))
                            return actionID;
                    }
                    // Lvl 92 Opener
                    else if (!天星棱光StarPrism.LevelChecked() && RainbowDrip.LevelChecked())
                    {
                        if (PCTOpenerLvl92.DoFullOpener(ref actionID))
                            return actionID;
                    }
                    // Lvl 90 Opener
                    else if (!天星棱光StarPrism.LevelChecked() && !RainbowDrip.LevelChecked() && CometinBlack.LevelChecked())
                    {
                        if (PCTOpenerLvl90.DoFullOpener(ref actionID))
                            return actionID;
                    }
                    // Lvl 80 Opener
                    else if (!天星棱光StarPrism.LevelChecked() && !CometinBlack.LevelChecked() && HolyInWhite.LevelChecked())
                    {
                        if (PCTOpenerLvl80.DoFullOpener(ref actionID))
                            return actionID;
                    }
                    // Lvl 70 Opener
                    else if (!天星棱光StarPrism.LevelChecked() && !CometinBlack.LevelChecked() && !HolyInWhite.LevelChecked() && 星空构想StarryMuse.LevelChecked())
                    {
                        if (PCTOpenerLvl70.DoFullOpener(ref actionID))
                            return actionID;
                    }

                    // General Weaves
                    if (InCombat() && canWeave)
                    {
                        // ScenicMuse

                        if (风景构想ScenicMuse.LevelChecked() && gauge.LandscapeMotifDrawn && gauge.WeaponMotifDrawn && IsOffCooldown(风景构想ScenicMuse))
                        {
                            return OriginalHook(风景构想ScenicMuse);
                        }

                        // LivingMuse

                        if (LivingMuse.LevelChecked() && gauge.CreatureMotifDrawn && (!(gauge.MooglePortraitReady || gauge.MadeenPortraitReady) || GetRemainingCharges(LivingMuse) == GetMaxCharges(LivingMuse)))
                        {
                            if (HasCharges(OriginalHook(LivingMuse)))
                            {
                                if (!风景构想ScenicMuse.LevelChecked() || GetCooldown(风景构想ScenicMuse).CooldownRemaining > GetCooldownChargeRemainingTime(LivingMuse))
                                {
                                    return OriginalHook(LivingMuse);
                                }
                            }
                        }

                        // SteelMuse

                        if (SteelMuse.LevelChecked() && !HasEffect(Buffs.HammerTime) && gauge.WeaponMotifDrawn && HasCharges(OriginalHook(SteelMuse)) && (GetCooldown(SteelMuse).CooldownRemaining < GetCooldown(风景构想ScenicMuse).CooldownRemaining || GetRemainingCharges(SteelMuse) == GetMaxCharges(SteelMuse) || !风景构想ScenicMuse.LevelChecked()))
                        {
                            return OriginalHook(SteelMuse);
                        }

                        // MogoftheAges

                        if (MogoftheAges.LevelChecked() && (gauge.MooglePortraitReady || gauge.MadeenPortraitReady) && IsOffCooldown(OriginalHook(MogoftheAges)) && (GetCooldownRemainingTime(星空构想StarryMuse) >= 60 || !风景构想ScenicMuse.LevelChecked()))
                        {
                            return OriginalHook(MogoftheAges);
                        }

                        // Swiftcast

                        if (IsMoving() && IsOffCooldown(All.Swiftcast) && All.Swiftcast.LevelChecked() && !HasEffect(Buffs.HammerTime) && gauge.Paint < 1 && (!gauge.CreatureMotifDrawn || !gauge.WeaponMotifDrawn || !gauge.LandscapeMotifDrawn))
                        {
                            return All.Swiftcast;
                        }

                        // SubtractivePalette

                        if (减色混合SubtractivePalette.LevelChecked() && !HasEffect(Buffs.SubtractivePalette) && !HasEffect(Buffs.MonochromeTones))
                        {
                            if (HasEffect(Buffs.SubtractiveSpectrum) || gauge.PalleteGauge >= 50)
                            {
                                return 减色混合SubtractivePalette;
                            }
                        }
                    }

                   


                    // Swiftcast Motifs
                    if (HasEffect(All.Buffs.Swiftcast))
                    {
                        if (!gauge.CreatureMotifDrawn && 动物彩绘CreatureMotif.LevelChecked() && !HasEffect(Buffs.星空构想StarryMuse))
                            return OriginalHook(动物彩绘CreatureMotif);
                        if (!gauge.WeaponMotifDrawn && HammerMotif.LevelChecked() && !HasEffect(Buffs.HammerTime) && !HasEffect(Buffs.星空构想StarryMuse))
                            return OriginalHook(HammerMotif);
                        if (!gauge.LandscapeMotifDrawn && 风景彩绘LandscapeMotif.LevelChecked() && !HasEffect(Buffs.星空构想StarryMuse))
                            return OriginalHook(风景彩绘LandscapeMotif);
                    }
                    
                    
                    

                    // IsMoving logic
                    if (IsMoving() && InCombat())
                    {
                        if (HammerStamp.LevelChecked() && HasEffect(Buffs.HammerTime))
                            return OriginalHook(HammerStamp);

                        if (CometinBlack.LevelChecked() && gauge.Paint >= 1 && HasEffect(Buffs.MonochromeTones))
                            return OriginalHook(CometinBlack);

                        if (HasEffect(Buffs.RainbowBright) || HasEffect(Buffs.RainbowBright) && GetBuffRemainingTime(Buffs.星空构想StarryMuse) <= 3f)
                            return RainbowDrip;

                        if (HolyInWhite.LevelChecked() && gauge.Paint >= 1)
                            return OriginalHook(HolyInWhite);
                    }

                    //Prepare for Burst
                    if (GetCooldownRemainingTime(风景构想ScenicMuse) <= 20)
                    {
                        if (风景彩绘LandscapeMotif.LevelChecked() && !gauge.LandscapeMotifDrawn)
                            return OriginalHook(风景彩绘LandscapeMotif);

                        if (动物彩绘CreatureMotif.LevelChecked() && !gauge.CreatureMotifDrawn)
                            return OriginalHook(动物彩绘CreatureMotif);

                        if (武器彩绘WeaponMotif.LevelChecked() && !gauge.WeaponMotifDrawn && !HasEffect(Buffs.HammerTime))
                            return OriginalHook(武器彩绘WeaponMotif);
                    }
                    
                   

                    // Burst 
                    if (HasEffect(Buffs.星空构想StarryMuse))
                    {

                        if (CometinBlack.LevelChecked() && HasEffect(Buffs.MonochromeTones) && gauge.Paint > 0)
                            return CometinBlack;

                        if (HammerStamp.LevelChecked() && HasEffect(Buffs.HammerTime) && !HasEffect(Buffs.Starstruck))
                            return OriginalHook(HammerStamp);

                        if (HasEffect(Buffs.Starstruck) || HasEffect(Buffs.Starstruck) && GetBuffRemainingTime(Buffs.Starstruck) <= 3f)
                            return 天星棱光StarPrism;

                        if (HasEffect(Buffs.RainbowBright) || HasEffect(Buffs.RainbowBright) && GetBuffRemainingTime(Buffs.星空构想StarryMuse) <= 3f)
                            return RainbowDrip;
                    }

                    if (HasEffect(Buffs.RainbowBright) && !HasEffect(Buffs.星空构想StarryMuse))
                        return RainbowDrip;

                    if (CometinBlack.LevelChecked() && HasEffect(Buffs.MonochromeTones) && gauge.Paint > 0 && GetCooldownRemainingTime(星空构想StarryMuse) > 30f)
                        return OriginalHook(CometinBlack);

                    if (HammerStamp.LevelChecked() && HasEffect(Buffs.HammerTime))
                        return OriginalHook(HammerStamp);

                    if (!HasEffect(Buffs.星空构想StarryMuse))
                    {
                        // LandscapeMotif

                        if (风景彩绘LandscapeMotif.LevelChecked() && !gauge.LandscapeMotifDrawn && GetCooldownRemainingTime(风景构想ScenicMuse) <= 20)
                        {
                            return OriginalHook(风景彩绘LandscapeMotif);
                        }

                        // CreatureMotif

                        if (动物彩绘CreatureMotif.LevelChecked() && !gauge.CreatureMotifDrawn && (HasCharges(LivingMuse) || GetCooldownChargeRemainingTime(LivingMuse) <= 8))
                        {
                            return OriginalHook(动物彩绘CreatureMotif);
                        }

                        // WeaponMotif

                        if (武器彩绘WeaponMotif.LevelChecked() && !HasEffect(Buffs.HammerTime) && !gauge.WeaponMotifDrawn && (HasCharges(SteelMuse) || GetCooldownChargeRemainingTime(SteelMuse) <= 8))
                        {
                            return OriginalHook(武器彩绘WeaponMotif);
                        }
                    }


                    if (All.LucidDreaming.LevelChecked() && ActionReady(All.LucidDreaming) && CanSpellWeave(actionID) && LocalPlayer.CurrentMp <= 6500)
                        return All.LucidDreaming;

                    if (BlizzardIIinCyan.LevelChecked() && HasEffect(Buffs.SubtractivePalette))
                        return OriginalHook(冰结之蓝青BlizzardinCyan);
                }

                return actionID;
            }
        }


        internal class PCT_ST_AdvancedMode : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.PCT_ST_AdvancedMode;
            internal static PCTOpenerLogicLvl100 PCTOpenerLvl100 = new();
            internal static PCTOpenerLogicLvl92 PCTOpenerLvl92 = new();
            internal static PCTOpenerLogicLvl90 PCTOpenerLvl90 = new();
            internal static PCTOpenerLogicLvl80 PCTOpenerLvl80 = new();
            internal static PCTOpenerLogicLvl70 PCTOpenerLvl70 = new();

            protected override uint Invoke(uint actionID, uint lastComboActionID, float comboTime, byte level)
            {
                if (actionID is 火炎之红FireInRed)
                {
                    var gauge = GetJobGauge<PCTGauge>();
                    bool canWeave = CanSpellWeave(ActionWatching.LastSpell) || CanSpellWeave(actionID);
                    int creatureStop = PluginConfiguration.GetCustomIntValue(Config.PCT_ST_CreatureStop);
                    int landscapeStop = PluginConfiguration.GetCustomIntValue(Config.PCT_ST_LandscapeStop);
                    int weaponStop = PluginConfiguration.GetCustomIntValue(Config.PCT_ST_WeaponStop);


                    // Variant Cure
                    if (IsEnabled(CustomComboPreset.PCT_Variant_Cure) && IsEnabled(Variant.VariantCure) && PlayerHealthPercentageHp() <= GetOptionValue(Config.PCT_VariantCure))
                        return Variant.VariantCure;

                    // Variant Rampart
                    if (IsEnabled(CustomComboPreset.PCT_Variant_Rampart) && IsEnabled(Variant.VariantRampart) && IsOffCooldown(Variant.VariantRampart) && canWeave)
                        return Variant.VariantRampart;

                    // Prepull logic
                    if (IsEnabled(CustomComboPreset.PCT_ST_AdvancedMode_PrePullMotifs))
                    {
                        if (!InCombat() || (IsEnabled(CustomComboPreset.PCT_ST_AdvancedMode_NoTargetMotifs) && InCombat() && CurrentTarget == null))
                        {
                            if (动物彩绘CreatureMotif.LevelChecked() && !gauge.CreatureMotifDrawn)
                                return OriginalHook(动物彩绘CreatureMotif);
                            
                            if (武器彩绘WeaponMotif.LevelChecked() && !gauge.WeaponMotifDrawn && !HasEffect(Buffs.HammerTime))
                                return OriginalHook(武器彩绘WeaponMotif);
                            
                            if (风景彩绘LandscapeMotif.LevelChecked() && !gauge.LandscapeMotifDrawn && !HasEffect(Buffs.星空构想StarryMuse))
                                return OriginalHook(风景彩绘LandscapeMotif);
                        }
                    }

                    // Check if Openers are enabled and determine which opener to execute based on current level
                    if (IsEnabled(CustomComboPreset.PCT_ST_Advanced_Openers))
                    {
                        // Lvl 100 Opener
                        if (天星棱光StarPrism.LevelChecked())
                        {
                            if (PCTOpenerLvl100.DoFullOpener(ref actionID))
                                return actionID;
                        }
                        // Lvl 92 Opener
                        else if (!天星棱光StarPrism.LevelChecked() && RainbowDrip.LevelChecked())
                        {
                            if (PCTOpenerLvl92.DoFullOpener(ref actionID))
                                return actionID;
                        }
                        // Lvl 90 Opener
                        else if (!天星棱光StarPrism.LevelChecked() && !RainbowDrip.LevelChecked() && CometinBlack.LevelChecked())
                        {
                            if (PCTOpenerLvl90.DoFullOpener(ref actionID))
                                return actionID;
                        }
                        // Lvl 80 Opener
                        else if (!天星棱光StarPrism.LevelChecked() && !CometinBlack.LevelChecked() && HolyInWhite.LevelChecked())
                        {
                            if (PCTOpenerLvl80.DoFullOpener(ref actionID))
                                return actionID;
                        }
                        // Lvl 70 Opener
                        else if (!天星棱光StarPrism.LevelChecked() && !CometinBlack.LevelChecked() && !HolyInWhite.LevelChecked() && 星空构想StarryMuse.LevelChecked())
                        {
                            if (PCTOpenerLvl70.DoFullOpener(ref actionID))
                                return actionID;
                        }
                    }

                    // General Weaves
                    if (InCombat() && canWeave)
                    {
                        // ScenicMuse
                        if (IsEnabled(CustomComboPreset.PCT_ST_AdvancedMode_ScenicMuse))
                        {
                            if (风景构想ScenicMuse.LevelChecked() && gauge.LandscapeMotifDrawn && gauge.WeaponMotifDrawn && IsOffCooldown(风景构想ScenicMuse))
                            {
                                return OriginalHook(风景构想ScenicMuse);
                            }
                        }

                        // LivingMuse
                        if (IsEnabled(CustomComboPreset.PCT_ST_AdvancedMode_LivingMuse))
                        {
                            if (LivingMuse.LevelChecked() && gauge.CreatureMotifDrawn && (!(gauge.MooglePortraitReady || gauge.MadeenPortraitReady) || GetRemainingCharges(LivingMuse) == GetMaxCharges(LivingMuse)))
                            {
                                if (HasCharges(OriginalHook(LivingMuse)))
                                {
                                    if (!风景构想ScenicMuse.LevelChecked() || GetCooldown(风景构想ScenicMuse).CooldownRemaining > GetCooldownChargeRemainingTime(LivingMuse))
                                    {
                                        return OriginalHook(LivingMuse);
                                    }
                                }
                            }
                        }

                        // SteelMuse
                        if (IsEnabled(CustomComboPreset.PCT_ST_AdvancedMode_SteelMuse))
                        {
                            if (SteelMuse.LevelChecked() && !HasEffect(Buffs.HammerTime) && gauge.WeaponMotifDrawn && HasCharges(OriginalHook(SteelMuse)) && (GetCooldown(SteelMuse).CooldownRemaining < GetCooldown(风景构想ScenicMuse).CooldownRemaining || GetRemainingCharges(SteelMuse) == GetMaxCharges(SteelMuse) || !风景构想ScenicMuse.LevelChecked()))
                            {
                                return OriginalHook(SteelMuse);
                            }
                        }

                        // MogoftheAges
                        if (IsEnabled(CustomComboPreset.PCT_ST_AdvancedMode_MogOfTheAges))
                        {
                            if (MogoftheAges.LevelChecked() && (gauge.MooglePortraitReady || gauge.MadeenPortraitReady) && IsOffCooldown(OriginalHook(MogoftheAges)) && (GetCooldownRemainingTime(星空构想StarryMuse) >= 60 || !风景构想ScenicMuse.LevelChecked()))
                            {
                                return OriginalHook(MogoftheAges);
                            }
                        }

                        // SubtractivePalette 减色混合
                        if (IsEnabled(CustomComboPreset.PCT_ST_AdvancedMode_SubtractivePalette))
                        {
                            if (减色混合SubtractivePalette.LevelChecked() && !HasEffect(Buffs.SubtractivePalette) && !HasEffect(Buffs.MonochromeTones))
                            {
                                if (HasEffect(Buffs.SubtractiveSpectrum) || gauge.PalleteGauge >= Config.PCT_SubtractivePalette)
                                {
                                    return 减色混合SubtractivePalette;
                                }
                            }
                        }
                    }

                    // Swiftcast Motifs
                    if (HasEffect(All.Buffs.Swiftcast))
                    {
                        if (!gauge.CreatureMotifDrawn && 动物彩绘CreatureMotif.LevelChecked() && !HasEffect(Buffs.星空构想StarryMuse) && GetTargetHPPercent() > creatureStop)
                            return OriginalHook(动物彩绘CreatureMotif);
                        
                        if (!gauge.WeaponMotifDrawn && 武器彩绘WeaponMotif.LevelChecked() && !HasEffect(Buffs.HammerTime) && !HasEffect(Buffs.星空构想StarryMuse) && GetTargetHPPercent() > weaponStop)
                            return OriginalHook(武器彩绘WeaponMotif);
                        
                        
                        if (!gauge.LandscapeMotifDrawn && 风景彩绘LandscapeMotif.LevelChecked() && !HasEffect(Buffs.星空构想StarryMuse) && GetTargetHPPercent() > landscapeStop)
                            return OriginalHook(风景彩绘LandscapeMotif);

                    }

                    // IsMoving logic
                    if (IsMoving() && InCombat())
                    {
                        if (IsEnabled(CustomComboPreset.PCT_ST_AdvancedMode_MovementOption_HammerStampCombo) && HammerStamp.LevelChecked() && HasEffect(Buffs.HammerTime))
                            return OriginalHook(HammerStamp);

                        if (IsEnabled(CustomComboPreset.PCT_ST_AdvancedMode_MovementOption_CometinBlack) && CometinBlack.LevelChecked() && gauge.Paint >= 1 && HasEffect(Buffs.MonochromeTones))
                            return OriginalHook(CometinBlack);

                        if (IsEnabled(CustomComboPreset.PCT_ST_AdvancedMode_Burst_RainbowDrip))
                        {
                            if (HasEffect(Buffs.RainbowBright) || HasEffect(Buffs.RainbowBright) && GetBuffRemainingTime(Buffs.星空构想StarryMuse) <= 3f)
                                return RainbowDrip;
                        }

                        if (IsEnabled(CustomComboPreset.PCT_ST_AdvancedMode_MovementOption_HolyInWhite) && HolyInWhite.LevelChecked() && gauge.Paint >= 1)
                            return OriginalHook(HolyInWhite);

                        if (IsEnabled(CustomComboPreset.PCT_ST_AdvancedMode_SwitfcastOption) && ActionReady(All.Swiftcast) && ((LevelChecked(动物彩绘CreatureMotif) && !gauge.CreatureMotifDrawn) || (LevelChecked(武器彩绘WeaponMotif) && !gauge.WeaponMotifDrawn) || (LevelChecked(风景彩绘LandscapeMotif) && !gauge.LandscapeMotifDrawn)))
                            return All.Swiftcast;
                    }

                    //Prepare for Burst
                    if (GetCooldownRemainingTime(风景构想ScenicMuse) <= 20 && !HasEffect(Buffs.绘灵幻景Hyperphantasia))
                    {
                        if (IsEnabled(CustomComboPreset.PCT_ST_AdvancedMode_LandscapeMotif) && 风景彩绘LandscapeMotif.LevelChecked() && !gauge.LandscapeMotifDrawn && GetTargetHPPercent() > landscapeStop)
                            return OriginalHook(风景彩绘LandscapeMotif);

                        if (IsEnabled(CustomComboPreset.PCT_ST_AdvancedMode_CreatureMotif) && 动物彩绘CreatureMotif.LevelChecked() && !gauge.CreatureMotifDrawn && GetTargetHPPercent() > creatureStop)
                            return OriginalHook(动物彩绘CreatureMotif);

                        if (IsEnabled(CustomComboPreset.PCT_ST_AdvancedMode_WeaponMotif) && 武器彩绘WeaponMotif.LevelChecked() && !gauge.WeaponMotifDrawn && !HasEffect(Buffs.HammerTime) && GetTargetHPPercent() > weaponStop)
                            return OriginalHook(武器彩绘WeaponMotif);
                    }
                    
                    
                  

                    // Burst 
                    if (IsEnabled(CustomComboPreset.PCT_ST_AdvancedMode_Burst_Phase) && HasEffect(Buffs.星空构想StarryMuse))
                    {

                        if (IsEnabled(CustomComboPreset.PCT_ST_AdvancedMode_Burst_CometInBlack) && CometinBlack.LevelChecked() && HasEffect(Buffs.MonochromeTones) && gauge.Paint > 0)
                            return CometinBlack;

                        if (IsEnabled(CustomComboPreset.PCT_ST_AdvancedMode_Burst_HammerCombo) && HammerStamp.LevelChecked() && HasEffect(Buffs.HammerTime) && !HasEffect(Buffs.Starstruck))
                            return OriginalHook(HammerStamp);

                        if (IsEnabled(CustomComboPreset.PCT_ST_AdvancedMode_Burst_StarPrism))
                        {
                            if (HasEffect(Buffs.Starstruck) || HasEffect(Buffs.Starstruck) && GetBuffRemainingTime(Buffs.Starstruck) <= 3f)
                                return 天星棱光StarPrism;
                        }

                        if (IsEnabled(CustomComboPreset.PCT_ST_AdvancedMode_Burst_RainbowDrip))
                        {
                            if (HasEffect(Buffs.RainbowBright) || HasEffect(Buffs.RainbowBright) && GetBuffRemainingTime(Buffs.星空构想StarryMuse) <= 3f)
                                return RainbowDrip;
                        }

                    }

                    if (HasEffect(Buffs.RainbowBright) && !HasEffect(Buffs.星空构想StarryMuse))
                        return RainbowDrip;

                    if (IsEnabled(CustomComboPreset.PCT_ST_AdvancedMode_CometinBlack) && CometinBlack.LevelChecked() && HasEffect(Buffs.MonochromeTones) && gauge.Paint > 0 && GetCooldownRemainingTime(星空构想StarryMuse) > 30f)
                        return OriginalHook(CometinBlack);

                    if (IsEnabled(CustomComboPreset.PCT_ST_AdvancedMode_HammerStampCombo) && HammerStamp.LevelChecked() && HasEffect(Buffs.HammerTime))
                        return OriginalHook(HammerStamp);

                    if (!HasEffect(Buffs.星空构想StarryMuse) && !HasEffect(Buffs.绘灵幻景Hyperphantasia))
                    {
                        // LandscapeMotif
                        if (IsEnabled(CustomComboPreset.PCT_ST_AdvancedMode_LandscapeMotif) && GetTargetHPPercent() > landscapeStop)
                        {
                            if (风景彩绘LandscapeMotif.LevelChecked() && !gauge.LandscapeMotifDrawn && GetCooldownRemainingTime(风景构想ScenicMuse) <= 20)
                            {
                                return OriginalHook(风景彩绘LandscapeMotif);
                            }
                        }

                        // CreatureMotif
                        if (IsEnabled(CustomComboPreset.PCT_ST_AdvancedMode_CreatureMotif) && GetTargetHPPercent() > creatureStop)
                        {
                            if (动物彩绘CreatureMotif.LevelChecked() && !gauge.CreatureMotifDrawn && (HasCharges(LivingMuse) || GetCooldownChargeRemainingTime(LivingMuse) <= 8))
                            {
                                return OriginalHook(动物彩绘CreatureMotif);
                            }
                        }

                        // WeaponMotif
                        if (IsEnabled(CustomComboPreset.PCT_ST_AdvancedMode_WeaponMotif) && GetTargetHPPercent() > weaponStop)
                        {
                            if (武器彩绘WeaponMotif.LevelChecked() && !HasEffect(Buffs.HammerTime) && !gauge.WeaponMotifDrawn && (HasCharges(SteelMuse) || GetCooldownChargeRemainingTime(SteelMuse) <= 8))
                            {
                                return OriginalHook(武器彩绘WeaponMotif);
                            }
                        }
                    }
                    

                    if (IsEnabled(CustomComboPreset.PCT_ST_AdvancedMode_LucidDreaming) && All.LucidDreaming.LevelChecked() && ActionReady(All.LucidDreaming) && CanSpellWeave(actionID) && LocalPlayer.CurrentMp <= Config.PCT_ST_AdvancedMode_LucidOption)
                        return All.LucidDreaming;

                    if (IsEnabled(CustomComboPreset.PCT_ST_AdvancedMode_BlizzardInCyan) && BlizzardIIinCyan.LevelChecked() && HasEffect(Buffs.SubtractivePalette))
                        return OriginalHook(冰结之蓝青BlizzardinCyan);
                }

                return actionID;
            }
        }

        internal class PCT_AoE_SimpleMode : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.PCT_AoE_SimpleMode;

            protected override uint Invoke(uint actionID, uint lastComboActionID, float comboTime, byte level)
            {
                if (actionID is FireIIinRed)
                {
                    var gauge = GetJobGauge<PCTGauge>();
                    bool canWeave = CanSpellWeave(ActionWatching.LastSpell);

                    // Variant Cure
                    if (IsEnabled(CustomComboPreset.PCT_Variant_Cure) && IsEnabled(Variant.VariantCure) && PlayerHealthPercentageHp() <= GetOptionValue(Config.PCT_VariantCure))
                        return Variant.VariantCure;

                    // Variant Rampart
                    if (IsEnabled(CustomComboPreset.PCT_Variant_Rampart) && IsEnabled(Variant.VariantRampart) && IsOffCooldown(Variant.VariantRampart) && canWeave)
                        return Variant.VariantRampart;

                    // Prepull logic


                    if (!InCombat() || InCombat() && CurrentTarget == null)
                    {
                        if (动物彩绘CreatureMotif.LevelChecked() && !gauge.CreatureMotifDrawn)
                            return OriginalHook(动物彩绘CreatureMotif);
                        if (武器彩绘WeaponMotif.LevelChecked() && !gauge.WeaponMotifDrawn && !HasEffect(Buffs.HammerTime))
                            return OriginalHook(武器彩绘WeaponMotif);
                        if (风景彩绘LandscapeMotif.LevelChecked() && !gauge.LandscapeMotifDrawn && !HasEffect(Buffs.星空构想StarryMuse))
                            return OriginalHook(风景彩绘LandscapeMotif);
                    }


                    // General Weaves
                    if (InCombat() && canWeave)
                    {
                        // LivingMuse

                        if (LivingMuse.LevelChecked() && gauge.CreatureMotifDrawn && (!(gauge.MooglePortraitReady || gauge.MadeenPortraitReady) || GetRemainingCharges(LivingMuse) == GetMaxCharges(LivingMuse)))
                        {
                            if (HasCharges(OriginalHook(LivingMuse)))
                            {
                                if (!风景构想ScenicMuse.LevelChecked() || GetCooldown(风景构想ScenicMuse).CooldownRemaining > GetCooldownChargeRemainingTime(LivingMuse))
                                {
                                    return OriginalHook(LivingMuse);
                                }
                            }
                        }

                        // ScenicMuse

                        if (风景构想ScenicMuse.LevelChecked() && gauge.LandscapeMotifDrawn && gauge.WeaponMotifDrawn && IsOffCooldown(风景构想ScenicMuse))
                        {
                            return OriginalHook(风景构想ScenicMuse);
                        }

                        // SteelMuse

                        if (SteelMuse.LevelChecked() && !HasEffect(Buffs.HammerTime) && gauge.WeaponMotifDrawn && HasCharges(OriginalHook(SteelMuse)) && (GetCooldown(SteelMuse).CooldownRemaining < GetCooldown(风景构想ScenicMuse).CooldownRemaining || GetRemainingCharges(SteelMuse) == GetMaxCharges(SteelMuse) || !风景构想ScenicMuse.LevelChecked()))
                        {
                            return OriginalHook(SteelMuse);
                        }

                        // MogoftheAges

                        if (MogoftheAges.LevelChecked() && (gauge.MooglePortraitReady || gauge.MadeenPortraitReady) && (IsOffCooldown(OriginalHook(MogoftheAges)) || !风景构想ScenicMuse.LevelChecked()))
                        {
                            return OriginalHook(MogoftheAges);
                        }

                        if (IsMoving() && IsOffCooldown(All.Swiftcast) && All.Swiftcast.LevelChecked() && !HasEffect(Buffs.HammerTime) && gauge.Paint < 1 && (!gauge.CreatureMotifDrawn || !gauge.WeaponMotifDrawn || !gauge.LandscapeMotifDrawn))
                        {
                            return All.Swiftcast;
                        }

                        // Subtractive Palette
                        if (减色混合SubtractivePalette.LevelChecked() && !HasEffect(Buffs.SubtractivePalette) && !HasEffect(Buffs.MonochromeTones))
                        {
                            if (HasEffect(Buffs.SubtractiveSpectrum) || gauge.PalleteGauge >= Config.PCT_SubtractivePalette)
                                return 减色混合SubtractivePalette;
                        }
                    }

                    if (HasEffect(All.Buffs.Swiftcast))
                    {
                        if (!gauge.CreatureMotifDrawn && 动物彩绘CreatureMotif.LevelChecked() && !HasEffect(Buffs.星空构想StarryMuse))
                            return OriginalHook(动物彩绘CreatureMotif);
                        if (!gauge.WeaponMotifDrawn && HammerMotif.LevelChecked() && !HasEffect(Buffs.HammerTime) && !HasEffect(Buffs.星空构想StarryMuse))
                            return OriginalHook(HammerMotif);
                        if (!gauge.LandscapeMotifDrawn && 风景彩绘LandscapeMotif.LevelChecked() && !HasEffect(Buffs.星空构想StarryMuse))
                            return OriginalHook(风景彩绘LandscapeMotif);
                    }

                    if (IsMoving() && InCombat())
                    {
                        if (HammerStamp.LevelChecked() && HasEffect(Buffs.HammerTime))
                            return OriginalHook(HammerStamp);

                        if (CometinBlack.LevelChecked() && gauge.Paint >= 1 && HasEffect(Buffs.MonochromeTones))
                            return OriginalHook(CometinBlack);

                        if (HasEffect(Buffs.RainbowBright) || (HasEffect(Buffs.RainbowBright) && GetBuffRemainingTime(Buffs.星空构想StarryMuse) < 3))
                            return RainbowDrip;

                        if (HolyInWhite.LevelChecked() && gauge.Paint >= 1)
                            return OriginalHook(HolyInWhite);

                    }

                    //Prepare for Burst
                    if (GetCooldownRemainingTime(风景构想ScenicMuse) <= 20)
                    {
                        if (风景彩绘LandscapeMotif.LevelChecked() && !gauge.LandscapeMotifDrawn)
                            return OriginalHook(风景彩绘LandscapeMotif);

                        if (动物彩绘CreatureMotif.LevelChecked() && !gauge.CreatureMotifDrawn)
                            return OriginalHook(动物彩绘CreatureMotif);

                        if (武器彩绘WeaponMotif.LevelChecked() && !gauge.WeaponMotifDrawn && !HasEffect(Buffs.HammerTime))
                            return OriginalHook(武器彩绘WeaponMotif);
                    }

                    // Burst 
                    if (HasEffect(Buffs.星空构想StarryMuse))
                    {
                        // Check for CometInBlack
                        if (CometinBlack.LevelChecked() && HasEffect(Buffs.MonochromeTones) && gauge.Paint > 0)
                            return CometinBlack;

                        // Check for HammerTime 
                        if (HammerStamp.LevelChecked() && HasEffect(Buffs.HammerTime) && !HasEffect(Buffs.Starstruck))
                            return OriginalHook(HammerStamp);

                        // Check for Starstruck
                        if (HasEffect(Buffs.Starstruck) || (HasEffect(Buffs.Starstruck) && GetBuffRemainingTime(Buffs.Starstruck) < 3))
                            return 天星棱光StarPrism;

                        // Check for RainbowBright
                        if (HasEffect(Buffs.RainbowBright) || (HasEffect(Buffs.RainbowBright) && GetBuffRemainingTime(Buffs.星空构想StarryMuse) < 3))
                            return RainbowDrip;
                    }

                    if (HasEffect(Buffs.RainbowBright) && !HasEffect(Buffs.星空构想StarryMuse))
                        return RainbowDrip;

                    if (CometinBlack.LevelChecked() && HasEffect(Buffs.MonochromeTones) && gauge.Paint > 0 && GetCooldownRemainingTime(星空构想StarryMuse) > 60)
                        return OriginalHook(CometinBlack);

                    if (HammerStamp.LevelChecked() && HasEffect(Buffs.HammerTime))
                        return OriginalHook(HammerStamp);

                    if (!HasEffect(Buffs.星空构想StarryMuse))
                    {
                        if (风景彩绘LandscapeMotif.LevelChecked() && !gauge.LandscapeMotifDrawn && GetCooldownRemainingTime(风景构想ScenicMuse) <= 20)
                            return OriginalHook(风景彩绘LandscapeMotif);

                        if (动物彩绘CreatureMotif.LevelChecked() && !gauge.CreatureMotifDrawn && (HasCharges(LivingMuse) || GetCooldownChargeRemainingTime(LivingMuse) <= 8))
                            return OriginalHook(动物彩绘CreatureMotif);

                        if (武器彩绘WeaponMotif.LevelChecked() && !HasEffect(Buffs.HammerTime) && !gauge.WeaponMotifDrawn && (HasCharges(SteelMuse) || GetCooldownChargeRemainingTime(SteelMuse) <= 8))
                            return OriginalHook(武器彩绘WeaponMotif);
                    }
                    //Saves one Charge of White paint for movement/Black paint.
                    if (HolyInWhite.LevelChecked() && gauge.Paint >= 2)
                        return OriginalHook(HolyInWhite);

                    if (All.LucidDreaming.LevelChecked() && ActionReady(All.LucidDreaming) && CanSpellWeave(actionID) && LocalPlayer.CurrentMp <= 6500)
                        return All.LucidDreaming;

                    if (BlizzardIIinCyan.LevelChecked() && HasEffect(Buffs.SubtractivePalette))
                        return OriginalHook(BlizzardIIinCyan);
                }
                return actionID;
            }
        }

        internal class PCT_AoE_AdvancedMode : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.PCT_AoE_AdvancedMode;

            protected override uint Invoke(uint actionID, uint lastComboActionID, float comboTime, byte level)
            {
                if (actionID is FireIIinRed)
                {
                    var gauge = GetJobGauge<PCTGauge>();
                    bool canWeave = CanSpellWeave(ActionWatching.LastSpell);
                    int creatureStop = PluginConfiguration.GetCustomIntValue(Config.PCT_AoE_CreatureStop);
                    int landscapeStop = PluginConfiguration.GetCustomIntValue(Config.PCT_AoE_LandscapeStop);
                    int weaponStop = PluginConfiguration.GetCustomIntValue(Config.PCT_AoE_WeaponStop);

                    // Variant Cure
                    if (IsEnabled(CustomComboPreset.PCT_Variant_Cure) && IsEnabled(Variant.VariantCure) && PlayerHealthPercentageHp() <= GetOptionValue(Config.PCT_VariantCure))
                        return Variant.VariantCure;

                    // Variant Rampart
                    if (IsEnabled(CustomComboPreset.PCT_Variant_Rampart) && IsEnabled(Variant.VariantRampart) && IsOffCooldown(Variant.VariantRampart) && canWeave)
                        return Variant.VariantRampart;

                    // Prepull logic
                    if (IsEnabled(CustomComboPreset.PCT_AoE_AdvancedMode_PrePullMotifs))
                    {
                        if (!InCombat() || (IsEnabled(CustomComboPreset.PCT_AoE_AdvancedMode_NoTargetMotifs) && InCombat() && CurrentTarget == null))
                        {
                            if (动物彩绘CreatureMotif.LevelChecked() && !gauge.CreatureMotifDrawn)
                                return OriginalHook(动物彩绘CreatureMotif);
                            if (武器彩绘WeaponMotif.LevelChecked() && !gauge.WeaponMotifDrawn && !HasEffect(Buffs.HammerTime))
                                return OriginalHook(武器彩绘WeaponMotif);
                            if (风景彩绘LandscapeMotif.LevelChecked() && !gauge.LandscapeMotifDrawn && !HasEffect(Buffs.星空构想StarryMuse))
                                return OriginalHook(风景彩绘LandscapeMotif);
                        }
                    }

                    // General Weaves
                    if (InCombat() && canWeave)
                    {
                        // LivingMuse
                        if (IsEnabled(CustomComboPreset.PCT_AoE_AdvancedMode_LivingMuse))
                        {
                            if (LivingMuse.LevelChecked() && gauge.CreatureMotifDrawn && (!(gauge.MooglePortraitReady || gauge.MadeenPortraitReady) || GetRemainingCharges(LivingMuse) == GetMaxCharges(LivingMuse)))
                            {
                                if (HasCharges(OriginalHook(LivingMuse)))
                                {
                                    if (!风景构想ScenicMuse.LevelChecked() || GetCooldown(风景构想ScenicMuse).CooldownRemaining > GetCooldownChargeRemainingTime(LivingMuse))
                                    {
                                        return OriginalHook(LivingMuse);
                                    }
                                }
                            }
                        }

                        // ScenicMuse
                        if (IsEnabled(CustomComboPreset.PCT_AoE_AdvancedMode_ScenicMuse))
                        {
                            if (风景构想ScenicMuse.LevelChecked() && gauge.LandscapeMotifDrawn && gauge.WeaponMotifDrawn && IsOffCooldown(风景构想ScenicMuse))
                            {
                                return OriginalHook(风景构想ScenicMuse);
                            }
                        }

                        // SteelMuse
                        if (IsEnabled(CustomComboPreset.PCT_AoE_AdvancedMode_SteelMuse))
                        {
                            if (SteelMuse.LevelChecked() && !HasEffect(Buffs.HammerTime) && gauge.WeaponMotifDrawn && HasCharges(OriginalHook(SteelMuse)) && (GetCooldown(SteelMuse).CooldownRemaining < GetCooldown(风景构想ScenicMuse).CooldownRemaining || GetRemainingCharges(SteelMuse) == GetMaxCharges(SteelMuse) || !风景构想ScenicMuse.LevelChecked()))
                            {
                                return OriginalHook(SteelMuse);
                            }
                        }

                        // MogoftheAges
                        if (IsEnabled(CustomComboPreset.PCT_AoE_AdvancedMode_MogOfTheAges))
                        {
                            if (MogoftheAges.LevelChecked() && (gauge.MooglePortraitReady || gauge.MadeenPortraitReady) && (IsOffCooldown(OriginalHook(MogoftheAges)) || !风景构想ScenicMuse.LevelChecked()))
                            {
                                return OriginalHook(MogoftheAges);
                            }
                        }


                        // Subtractive Palette
                        if (IsEnabled(CustomComboPreset.PCT_AoE_AdvancedMode_SubtractivePalette) && 减色混合SubtractivePalette.LevelChecked() && !HasEffect(Buffs.SubtractivePalette) && !HasEffect(Buffs.MonochromeTones))
                        {
                            if (HasEffect(Buffs.SubtractiveSpectrum) || gauge.PalleteGauge >= Config.PCT_SubtractivePalette)
                                return 减色混合SubtractivePalette;
                        }
                    }


                    if (HasEffect(All.Buffs.Swiftcast))
                    {
                        if (!gauge.CreatureMotifDrawn && 动物彩绘CreatureMotif.LevelChecked() && !HasEffect(Buffs.星空构想StarryMuse) && GetTargetHPPercent() > creatureStop)
                            return OriginalHook(动物彩绘CreatureMotif);
                        if (!gauge.WeaponMotifDrawn && HammerMotif.LevelChecked() && !HasEffect(Buffs.HammerTime) && !HasEffect(Buffs.星空构想StarryMuse) && GetTargetHPPercent() > weaponStop)
                            return OriginalHook(HammerMotif);
                        if (!gauge.LandscapeMotifDrawn && 风景彩绘LandscapeMotif.LevelChecked() && !HasEffect(Buffs.星空构想StarryMuse) && GetTargetHPPercent() > landscapeStop)
                            return OriginalHook(风景彩绘LandscapeMotif);
                    }

                    if (IsMoving() && InCombat())
                    {
                        if (IsEnabled(CustomComboPreset.PCT_AoE_AdvancedMode_MovementOption_HammerStampCombo) && HammerStamp.LevelChecked() && HasEffect(Buffs.HammerTime))
                            return OriginalHook(HammerStamp);

                        if (IsEnabled(CustomComboPreset.PCT_AoE_AdvancedMode_MovementOption_CometinBlack) && CometinBlack.LevelChecked() && gauge.Paint >= 1 && HasEffect(Buffs.MonochromeTones))
                            return OriginalHook(CometinBlack);

                        if (IsEnabled(CustomComboPreset.PCT_AoE_AdvancedMode_Burst_RainbowDrip))
                        {
                            if (HasEffect(Buffs.RainbowBright) || (HasEffect(Buffs.RainbowBright) && GetBuffRemainingTime(Buffs.星空构想StarryMuse) < 3))
                                return RainbowDrip;
                        }

                        if (IsEnabled(CustomComboPreset.PCT_AoE_AdvancedMode_MovementOption_HolyInWhite) && HolyInWhite.LevelChecked() && gauge.Paint >= 1)
                            return OriginalHook(HolyInWhite);

                        if (IsEnabled(CustomComboPreset.PCT_AoE_AdvancedMode_SwitfcastOption) && ActionReady(All.Swiftcast) && ((LevelChecked(动物彩绘CreatureMotif) && !gauge.CreatureMotifDrawn) || (LevelChecked(武器彩绘WeaponMotif) && !gauge.WeaponMotifDrawn) || (LevelChecked(风景彩绘LandscapeMotif) && !gauge.LandscapeMotifDrawn)))
                            return All.Swiftcast;
                    }

                    //Prepare for Burst
                    if (GetCooldownRemainingTime(风景构想ScenicMuse) <= 20)
                    {
                        if (IsEnabled(CustomComboPreset.PCT_AoE_AdvancedMode_LandscapeMotif) && 风景彩绘LandscapeMotif.LevelChecked() && !gauge.LandscapeMotifDrawn && GetTargetHPPercent() > landscapeStop)
                            return OriginalHook(风景彩绘LandscapeMotif);

                        if (IsEnabled(CustomComboPreset.PCT_AoE_AdvancedMode_CreatureMotif) && 动物彩绘CreatureMotif.LevelChecked() && !gauge.CreatureMotifDrawn && GetTargetHPPercent() > creatureStop)
                            return OriginalHook(动物彩绘CreatureMotif);

                        if (IsEnabled(CustomComboPreset.PCT_AoE_AdvancedMode_WeaponMotif) && 武器彩绘WeaponMotif.LevelChecked() && !gauge.WeaponMotifDrawn && !HasEffect(Buffs.HammerTime) && GetTargetHPPercent() > weaponStop)
                            return OriginalHook(武器彩绘WeaponMotif);
                    }

                    // Burst 
                    if (IsEnabled(CustomComboPreset.PCT_AoE_AdvancedMode_Burst_Phase) && HasEffect(Buffs.星空构想StarryMuse))
                    {
                        // Check for CometInBlack
                        if (IsEnabled(CustomComboPreset.PCT_AoE_AdvancedMode_Burst_CometInBlack) && CometinBlack.LevelChecked() && HasEffect(Buffs.MonochromeTones) && gauge.Paint > 0)
                            return CometinBlack;

                        // Check for HammerTime 
                        if (IsEnabled(CustomComboPreset.PCT_AoE_AdvancedMode_Burst_HammerCombo) && HammerStamp.LevelChecked() && HasEffect(Buffs.HammerTime) && !HasEffect(Buffs.Starstruck))
                            return OriginalHook(HammerStamp);

                        // Check for Starstruck
                        if (IsEnabled(CustomComboPreset.PCT_AoE_AdvancedMode_Burst_StarPrism))
                        {
                            if (HasEffect(Buffs.Starstruck) || (HasEffect(Buffs.Starstruck) && GetBuffRemainingTime(Buffs.Starstruck) < 3))
                                return 天星棱光StarPrism;
                        }

                        // Check for RainbowBright
                        if (IsEnabled(CustomComboPreset.PCT_AoE_AdvancedMode_Burst_RainbowDrip))
                        {
                            if (HasEffect(Buffs.RainbowBright) || (HasEffect(Buffs.RainbowBright) && GetBuffRemainingTime(Buffs.星空构想StarryMuse) < 3))
                                return RainbowDrip;
                        }
                    }

                    if (IsEnabled(CustomComboPreset.PCT_AoE_AdvancedMode_HolyinWhite) && !HasEffect(Buffs.星空构想StarryMuse) && !HasEffect(Buffs.MonochromeTones))
                    {
                        if (gauge.Paint > Config.PCT_AoE_AdvancedMode_HolyinWhiteOption || (Config.PCT_AoE_AdvancedMode_HolyinWhiteOption == 5 && gauge.Paint == 5 && !HasEffect(Buffs.HammerTime) && (HasEffect(Buffs.RainbowBright) || WasLastSpell(AeroIIinGreen) || WasLastSpell(StoneIIinYellow))))
                            return OriginalHook(HolyInWhite);
                    }

                    if (HasEffect(Buffs.RainbowBright) && !HasEffect(Buffs.星空构想StarryMuse))
                        return RainbowDrip;

                    if (IsEnabled(CustomComboPreset.PCT_AoE_AdvancedMode_CometinBlack) && CometinBlack.LevelChecked() && HasEffect(Buffs.MonochromeTones) && gauge.Paint > 0 && GetCooldownRemainingTime(星空构想StarryMuse) > 60)
                        return OriginalHook(CometinBlack);

                    if (IsEnabled(CustomComboPreset.PCT_AoE_AdvancedMode_HammerStampCombo) && HammerStamp.LevelChecked() && HasEffect(Buffs.HammerTime))
                        return OriginalHook(HammerStamp);


                    if (!HasEffect(Buffs.星空构想StarryMuse))
                    {
                        if (IsEnabled(CustomComboPreset.PCT_AoE_AdvancedMode_LandscapeMotif) && GetTargetHPPercent() > landscapeStop)
                        {
                            if (风景彩绘LandscapeMotif.LevelChecked() && !gauge.LandscapeMotifDrawn && GetCooldownRemainingTime(风景构想ScenicMuse) <= 20)
                                return OriginalHook(风景彩绘LandscapeMotif);
                        }

                        if (IsEnabled(CustomComboPreset.PCT_AoE_AdvancedMode_CreatureMotif) && GetTargetHPPercent() > creatureStop)
                        {
                            if (动物彩绘CreatureMotif.LevelChecked() && !gauge.CreatureMotifDrawn && (HasCharges(LivingMuse) || GetCooldownChargeRemainingTime(LivingMuse) <= 8))
                                return OriginalHook(动物彩绘CreatureMotif);
                        }

                        if (IsEnabled(CustomComboPreset.PCT_AoE_AdvancedMode_WeaponMotif) && GetTargetHPPercent() > weaponStop)
                        {
                            if (武器彩绘WeaponMotif.LevelChecked() && !HasEffect(Buffs.HammerTime) && !gauge.WeaponMotifDrawn && (HasCharges(SteelMuse) || GetCooldownChargeRemainingTime(SteelMuse) <= 8))
                                return OriginalHook(武器彩绘WeaponMotif);
                        }
                    }

                    if (IsEnabled(CustomComboPreset.PCT_AoE_AdvancedMode_LucidDreaming) && All.LucidDreaming.LevelChecked() && ActionReady(All.LucidDreaming) && CanSpellWeave(actionID) && LocalPlayer.CurrentMp <= Config.PCT_ST_AdvancedMode_LucidOption)
                        return All.LucidDreaming;

                    if (IsEnabled(CustomComboPreset.PCT_AoE_AdvancedMode_BlizzardInCyan) && BlizzardIIinCyan.LevelChecked() && HasEffect(Buffs.SubtractivePalette))
                        return OriginalHook(BlizzardIIinCyan);

                }
                return actionID;
            }
        }

        internal class CombinedAetherhues : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.CombinedAetherhues;

            protected override uint Invoke(uint actionID, uint lastComboActionID, float comboTime, byte level)
            {
                int choice = Config.CombinedAetherhueChoices;

                if (actionID == 火炎之红FireInRed && choice is 0 or 1)
                {
                    if (HasEffect(Buffs.SubtractivePalette))
                        return OriginalHook(冰结之蓝青BlizzardinCyan);
                }

                if (actionID == FireIIinRed && choice is 0 or 2)
                {
                    if (HasEffect(Buffs.SubtractivePalette))
                        return OriginalHook(BlizzardIIinCyan);
                }

                return actionID;
            }
        }

        internal class CombinedMotifs : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.CombinedMotifs;

            protected override uint Invoke(uint actionID, uint lastComboActionID, float comboTime, byte level)
            {
                var gauge = GetJobGauge<PCTGauge>();

                if (actionID == 动物彩绘CreatureMotif)
                {
                    if ((Config.CombinedMotifsMog && gauge.MooglePortraitReady) || (Config.CombinedMotifsMadeen && gauge.MadeenPortraitReady) && IsOffCooldown(OriginalHook(MogoftheAges)))
                        return OriginalHook(MogoftheAges);

                    if (gauge.CreatureMotifDrawn)
                        return OriginalHook(LivingMuse);
                }

                if (actionID == 武器彩绘WeaponMotif)
                {
                    if (Config.CombinedMotifsWeapon && HasEffect(Buffs.HammerTime))
                        return OriginalHook(HammerStamp);

                    if (gauge.WeaponMotifDrawn)
                        return OriginalHook(SteelMuse);
                }

                if (actionID == 风景彩绘LandscapeMotif)
                {
                    if (gauge.LandscapeMotifDrawn)
                        return OriginalHook(风景构想ScenicMuse);
                }

                return actionID;
            }
        }

        internal class CombinedPaint : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.CombinedPaint;

            protected override uint Invoke(uint actionID, uint lastComboActionID, float comboTime, byte level)
            {
                if (actionID == HolyInWhite)
                {
                    if (HasEffect(Buffs.MonochromeTones))
                        return CometinBlack;
                }

                return actionID;
            }
        }
        
        
        internal class PCT_ONE_LandscapeMotif : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.PCT_ONE_LandscapeMotif;

            protected override uint Invoke(uint actionID, uint lastComboActionID, float comboTime, byte level)
            {
                if (actionID == 风景彩绘LandscapeMotif)
                {
                    if (HasEffect(Buffs.星空构想StarryMuse))
                    {
                        if (HasEffect(Buffs.Starstruck) || HasEffect(Buffs.Starstruck) && GetBuffRemainingTime(Buffs.Starstruck) <= 3f)
                            return 天星棱光StarPrism;
                    }
                    
                    var gauge = GetJobGauge<PCTGauge>();
                    if (风景彩绘LandscapeMotif.LevelChecked() && !gauge.LandscapeMotifDrawn)
                        return OriginalHook(风景彩绘LandscapeMotif);
                }

                return actionID;
            }
        }
    }
}