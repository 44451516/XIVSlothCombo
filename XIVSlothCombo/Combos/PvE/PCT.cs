﻿using XIVSlothCombo.CustomComboNS;
using XIVSlothCombo.CustomComboNS.Functions;
using XIVSlothCombo.Data;

namespace XIVSlothCombo.Combos.PvE
{
    internal class PCT
    {
        public const byte JobID = 42;

        public const uint
            AdventofChocobastion = 39215,
            AeroinGreen = 39192,
            BlizzardinCyan = 34653,
            BlizzardIIinCyan = 34659,
            ClawMotif = 34666,
            CometinBlack = 39199,
            CreatureMotif = 34689,
            FireInRed = 34650,
            FireIIinRed = 34656,
            HammerStamp = 34678,
            LandscapeMotif = 34691,
            LivingMuse = 35347,
            MawMotif = 34667,
            MogoftheAges = 34676,
            PomMotif = 34664,
            PomMuse = 34670,
            RainbowDrip = 34688,
            ReleaseSubtractivePalette = 39214,
            RetributionoftheMadeen1 = 34677,
            RetributionoftheMadeen2 = 39783,
            ScenicMuse = 35349,
            Smudge1 = 34684,
            Smudge2 = 39210,
            StarPrism = 34681,
            SteelMuse = 35348,
            SubtractivePalette = 39213,
            ThunderIIinMagenta = 34661,
            ThunderinMagenta1 = 34655,
            ThunderinMagenta2 = 39196,
            WaterinBlue = 34652,
            WeaponMotif = 34690,
            WingMotif = 34665;

        public static class Buffs
        {
            public const ushort
                SubtractivePalette = 3674,
                HammerTime = 3680;
        }

        public static class Debuffs
        {

        }

        public static class Config
        {
            public static UserInt
                CombinedAetherhueChoices = new("CombinedAetherhueChoices");

            public static UserBool
                CombinedMotifsMog = new("CombinedMotifsMog"),
                CombinedMotifsWeapon = new("CombinedMotifsWeapon");
        }

        internal class CombinedAetherhues : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.CombinedAetherhues;

            protected override uint Invoke(uint actionID, uint lastComboActionID, float comboTime, byte level)
            {
                int choice = Config.CombinedAetherhueChoices;

                if (actionID == FireInRed && choice is 0 or 1)
                {
                    if (HasEffect(Buffs.SubtractivePalette))
                        return OriginalHook(BlizzardinCyan);
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
                var gauge = new TmpPCTGauge();

                if (actionID == CreatureMotif)
                {
                    if (Config.CombinedMotifsMog && gauge.MooglePortraitReady && IsOffCooldown(OriginalHook(MogoftheAges)))
                        return OriginalHook(MogoftheAges);

                    if (gauge.CreatureMotifDrawn)
                        return OriginalHook(LivingMuse);
                }

                if (actionID == WeaponMotif)
                {
                    if (Config.CombinedMotifsWeapon && HasEffect(Buffs.HammerTime))
                        return OriginalHook(HammerStamp);

                    if (gauge.WeaponMotifDrawn)
                        return OriginalHook(SteelMuse);
                }

                if (actionID == LandscapeMotif)
                {
                    if (gauge.LandscapeMotifDrawn)
                        return OriginalHook(ScenicMuse);
                }

                return actionID;
            }
        }
    }
}
