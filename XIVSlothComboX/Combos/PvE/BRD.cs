using System;
using Dalamud.Game.ClientState.JobGauge.Enums;
using Dalamud.Game.ClientState.JobGauge.Types;
using Dalamud.Game.ClientState.Statuses;
using XIVSlothComboX.Combos.PvE.Content;
using XIVSlothComboX.Core;
using XIVSlothComboX.CustomComboNS;
using XIVSlothComboX.CustomComboNS.Functions;
using XIVSlothComboX.Data;
using XIVSlothComboX.Extensions;

namespace XIVSlothComboX.Combos.PvE
{
    internal static class BRD
    {
        public const byte ClassID = 5;
        public const byte JobID = 23;

        public const uint
            HeavyShot = 97,
            StraightShot = 98,
            毒咬箭VenomousBite = 100,
            猛者强击RagingStrikes = 101,
            QuickNock = 106,
            纷乱箭Barrage = 107,
            Bloodletter = 110,
            风蚀箭Windbite = 113,
            贤者的叙事谣MagesBallad = 114,
            军神的赞美歌ArmysPaeon = 116,
            RainOfDeath = 117,
            战斗之声BattleVoice = 118,
            九天连箭EmpyrealArrow = 3558,
            放浪神的小步舞曲WanderersMinuet = 3559,
            伶牙俐齿IronJaws = 3560,
            Sidewinder = 3562,
            完美音调PitchPerfect = 7404,
            Troubadour = 7405,
            CausticBite = 7406,
            Stormbite = 7407,
            RefulgentArrow = 7409,
            BurstShot = 16495,
            ApexArrow = 16496,
            Shadowbite = 16494,
            Ladonsbite = 25783,
            BlastArrow = 25784,
            光明神的最终乐章RadiantFinale = 25785,
            WideVolley = 36974,
            HeartbreakShot = 36975,
            共鸣箭ResonantArrow = 36976,
            光明神的返场余音RadiantEncore = 36977;

        public static class Buffs
        {
            public const ushort
                强者猛击RagingStrikes = 125,
                纷乱箭 = 128,
                MagesBallad = 135,
                ArmysPaeon = 137,
                战斗之声BattleVoice = 141,
                WanderersMinuet = 865,
                Troubadour = 1934,
                BlastArrowReady = 2692,
                RadiantFinale = 2722,
                ShadowbiteReady = 3002,
                HawksEye = 3861,
                ResonantArrowReady = 3862,
                RadiantEncoreReady = 3863;
        }

        public static class Debuffs
        {
            public const ushort
                毒VenomousBite = 124,
                风Windbite = 129,
                烈毒咬箭CausticBite = 1200,
                Stormbite = 1201;
        }

        public static class Config
        {
            public const string
                BRD_RagingJawsRenewTime = "ragingJawsRenewTime",
                BRD_NoWasteHPPercentage = "noWasteHpPercentage",
                BRD_AoENoWasteHPPercentage = "AoENoWasteHpPercentage",
                BRD_STSecondWindThreshold = "BRD_STSecondWindThreshold",
                BRD_AoESecondWindThreshold = "BRD_AoESecondWindThreshold",
                BRD_VariantCure = "BRD_VariantCure";
        }

        internal static class Traits
        {
            internal const ushort
                EnhancedBloodletter = 445;
        }

        #region Song status

        internal static bool SongIsNotNone(Song        value) => value != Song.NONE;
        internal static bool SongIsNone(Song           value) => value == Song.NONE;
        internal static bool SongIsWandererMinuet(Song value) => value == Song.WANDERER;

        #endregion

        internal class BRD_ST_CustomMode : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BRD_ST_CustomMode;


            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID is StraightShot)
                {
                    if (OnOpenerCustomActionAction(out var customActionActionId))
                    {
                        return customActionActionId;
                    }
                }


                return actionID;
            }
        }

        // Replace HS/BS with SS/RA when procced, Apex feature added. 
        internal class BRD_StraightShotUpgrade : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BRD_StraightShotUpgrade;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID is HeavyShot or BurstShot)
                {

                    if (IsEnabled(CustomComboPreset.BRD_DoTMaintainance))
                    {
                        bool venomous = TargetHasEffect(Debuffs.毒VenomousBite);
                        bool windbite = TargetHasEffect(Debuffs.风Windbite);
                        bool caustic = TargetHasEffect(Debuffs.烈毒咬箭CausticBite);
                        bool stormbite = TargetHasEffect(Debuffs.Stormbite);
                        float venomRemaining = GetDebuffRemainingTime(Debuffs.毒VenomousBite);
                        float windRemaining = GetDebuffRemainingTime(Debuffs.风Windbite);
                        float causticRemaining = GetDebuffRemainingTime(Debuffs.烈毒咬箭CausticBite);
                        float stormRemaining = GetDebuffRemainingTime(Debuffs.Stormbite);

                        if (InCombat())
                        {
                            if (LevelChecked(伶牙俐齿IronJaws) && ((venomous && venomRemaining < 4) || (caustic && causticRemaining < 4)) || (windbite && windRemaining < 4) || (stormbite && stormRemaining < 4))
                                return 伶牙俐齿IronJaws;
                            if (!LevelChecked(伶牙俐齿IronJaws) && venomous && venomRemaining < 4)
                                return 毒咬箭VenomousBite;
                            if (!LevelChecked(伶牙俐齿IronJaws) && windbite && windRemaining < 4)
                                return 风蚀箭Windbite;
                        }
                    }

                    if (IsEnabled(CustomComboPreset.BRD_ApexST))
                    {
                        BRDGauge? gauge = GetJobGauge<BRDGauge>();

                        if (LevelChecked(ApexArrow) && gauge.SoulVoice == 100)
                            return ApexArrow;
                        if (LevelChecked(BlastArrow) && HasEffect(Buffs.BlastArrowReady))
                            return BlastArrow;
                    }

                    if (HasEffect(Buffs.HawksEye) || HasEffect(Buffs.纷乱箭))
                        return OriginalHook(StraightShot);
                }

                return actionID;
            }
        }

        internal class BRD_IronJaws : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BRD_IronJaws;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID is 伶牙俐齿IronJaws)
                {

                    if (!LevelChecked(伶牙俐齿IronJaws))
                    {
                        Status? venomous = FindTargetEffect(Debuffs.毒VenomousBite);
                        Status? windbite = FindTargetEffect(Debuffs.风Windbite);
                        float venomRemaining = GetDebuffRemainingTime(Debuffs.毒VenomousBite);
                        float windRemaining = GetDebuffRemainingTime(Debuffs.风Windbite);

                        if (venomous is not null && windbite is not null)
                        {
                            if (LevelChecked(毒咬箭VenomousBite) && venomRemaining < windRemaining)
                                return 毒咬箭VenomousBite;
                            if (LevelChecked(风蚀箭Windbite))
                                return 风蚀箭Windbite;
                        }

                        if (LevelChecked(毒咬箭VenomousBite) && (!LevelChecked(风蚀箭Windbite) || windbite is not null))
                            return 毒咬箭VenomousBite;
                        if (LevelChecked(风蚀箭Windbite))
                            return 风蚀箭Windbite;
                    }

                    if (!LevelChecked(Stormbite))
                    {
                        bool venomous = TargetHasEffect(Debuffs.毒VenomousBite);
                        bool windbite = TargetHasEffect(Debuffs.风Windbite);

                        if (LevelChecked(伶牙俐齿IronJaws) && venomous && windbite)
                            return 伶牙俐齿IronJaws;
                        if (LevelChecked(毒咬箭VenomousBite) && windbite)
                            return 毒咬箭VenomousBite;
                        if (LevelChecked(风蚀箭Windbite))
                            return 风蚀箭Windbite;
                    }

                    bool caustic = TargetHasEffect(Debuffs.烈毒咬箭CausticBite);
                    bool stormbite = TargetHasEffect(Debuffs.Stormbite);

                    if (LevelChecked(伶牙俐齿IronJaws) && caustic && stormbite)
                        return 伶牙俐齿IronJaws;
                    if (LevelChecked(CausticBite) && stormbite)
                        return CausticBite;
                    if (LevelChecked(Stormbite))
                        return Stormbite;

                    if (IsEnabled(CustomComboPreset.BRD_IronJawsApex) && LevelChecked(ApexArrow))
                    {
                        BRDGauge? gauge = GetJobGauge<BRDGauge>();

                        if (LevelChecked(BlastArrow) && HasEffect(Buffs.BlastArrowReady))
                            return BlastArrow;
                        if (gauge.SoulVoice == 100 && IsOffCooldown(ApexArrow))
                            return ApexArrow;
                    }
                }

                return actionID;
            }
        }

        internal class BRD_IronJaws_Alternate : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BRD_IronJaws_Alternate;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID is 伶牙俐齿IronJaws)
                {
                    if (!LevelChecked(伶牙俐齿IronJaws))
                    {
                        Status? venomous = FindTargetEffect(Debuffs.毒VenomousBite);
                        Status? windbite = FindTargetEffect(Debuffs.风Windbite);

                        if (venomous is not null && windbite is not null)
                        {
                            float venomRemaining = GetDebuffRemainingTime(Debuffs.毒VenomousBite);
                            float windRemaining = GetDebuffRemainingTime(Debuffs.风Windbite);

                            if (LevelChecked(毒咬箭VenomousBite) && venomRemaining < windRemaining)
                                return 毒咬箭VenomousBite;
                            if (LevelChecked(风蚀箭Windbite))
                                return 风蚀箭Windbite;
                        }

                        if (LevelChecked(毒咬箭VenomousBite) && (!LevelChecked(风蚀箭Windbite) || windbite is not null))
                            return 毒咬箭VenomousBite;
                        if (LevelChecked(风蚀箭Windbite))
                            return 风蚀箭Windbite;
                    }

                    if (!LevelChecked(Stormbite))
                    {
                        bool venomous = TargetHasEffect(Debuffs.毒VenomousBite);
                        bool windbite = TargetHasEffect(Debuffs.风Windbite);
                        float venomRemaining = GetDebuffRemainingTime(Debuffs.毒VenomousBite);
                        float windRemaining = GetDebuffRemainingTime(Debuffs.风Windbite);

                        if (LevelChecked(伶牙俐齿IronJaws) && venomous && windbite && (venomRemaining < 4 || windRemaining < 4))
                            return 伶牙俐齿IronJaws;
                        if (LevelChecked(毒咬箭VenomousBite) && windbite)
                            return 毒咬箭VenomousBite;
                        if (LevelChecked(风蚀箭Windbite))
                            return 风蚀箭Windbite;
                    }

                    bool caustic = TargetHasEffect(Debuffs.烈毒咬箭CausticBite);
                    bool stormbite = TargetHasEffect(Debuffs.Stormbite);
                    float causticRemaining = GetDebuffRemainingTime(Debuffs.烈毒咬箭CausticBite);
                    float stormRemaining = GetDebuffRemainingTime(Debuffs.Stormbite);

                    if (LevelChecked(伶牙俐齿IronJaws) && caustic && stormbite && (causticRemaining < 4 || stormRemaining < 4))
                        return 伶牙俐齿IronJaws;
                    if (LevelChecked(CausticBite) && stormbite)
                        return CausticBite;
                    if (LevelChecked(Stormbite))
                        return Stormbite;
                }

                return actionID;
            }
        }

        internal class BRD_AoE_oGCD : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BRD_AoE_oGCD;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID is RainOfDeath)
                {
                    BRDGauge? gauge = GetJobGauge<BRDGauge>();
                    bool songWanderer = gauge.Song == Song.WANDERER;

                    if (LevelChecked(放浪神的小步舞曲WanderersMinuet) && songWanderer && gauge.Repertoire == 3)
                        return OriginalHook(放浪神的小步舞曲WanderersMinuet);
                    if (ActionReady(九天连箭EmpyrealArrow))
                        return 九天连箭EmpyrealArrow;
                    if (ActionReady(RainOfDeath))
                        return RainOfDeath;
                    if (ActionReady(Sidewinder))
                        return Sidewinder;
                }

                return actionID;
            }
        }

        internal class BRD_AoE_AdvMode : CustomCombo
        {
            internal static bool inOpener = false;
            internal static bool openerFinished = false;

            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BRD_AoE_AdvMode;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID is Ladonsbite or QuickNock)
                {
                    BRDGauge? gauge = GetJobGauge<BRDGauge>();
                    bool canWeave = CanWeave(actionID);
                    bool canWeaveBuffs = CanWeave(actionID, 0.6);
                    bool canWeaveDelayed = CanDelayedWeave(actionID, 0.9);
                    int songTimerInSeconds = gauge.SongTimer / 1000;
                    bool songNone = gauge.Song == Song.NONE;
                    bool songWanderer = gauge.Song == Song.WANDERER;
                    bool songMage = gauge.Song == Song.MAGE;
                    bool songArmy = gauge.Song == Song.ARMY;
                    int targetHPThreshold = PluginConfiguration.GetCustomIntValue(Config.BRD_AoENoWasteHPPercentage);
                    bool isEnemyHealthHigh = !IsEnabled(CustomComboPreset.BRD_AoE_Adv_NoWaste) || GetTargetHPPercent() > targetHPThreshold;

                    if (IsEnabled(CustomComboPreset.BRD_Variant_Cure) && IsEnabled(Variant.VariantCure) && PlayerHealthPercentageHp() <= GetOptionValue(Config.BRD_VariantCure))
                        return Variant.VariantCure;

                    if (IsEnabled(CustomComboPreset.BRD_Variant_Rampart) && IsEnabled(Variant.VariantRampart) && IsOffCooldown(Variant.VariantRampart) && canWeave)
                        return Variant.VariantRampart;

                    if (IsEnabled(CustomComboPreset.BRD_AoE_Adv_Songs) && canWeave)
                    {

                        // Limit optimisation to when you are high enough level to benefit from it.
                        if (LevelChecked(放浪神的小步舞曲WanderersMinuet))
                        {
                            if (canWeave)
                            {
                                if (songNone)
                                {
                                    // Logic to determine first song
                                    if (ActionReady(放浪神的小步舞曲WanderersMinuet) && !(JustUsed(贤者的叙事谣MagesBallad) || JustUsed(军神的赞美歌ArmysPaeon)))
                                        return 放浪神的小步舞曲WanderersMinuet;
                                    if (ActionReady(贤者的叙事谣MagesBallad) && !(JustUsed(放浪神的小步舞曲WanderersMinuet) || JustUsed(军神的赞美歌ArmysPaeon)))
                                        return 贤者的叙事谣MagesBallad;
                                    if (ActionReady(军神的赞美歌ArmysPaeon) && !(JustUsed(贤者的叙事谣MagesBallad) || JustUsed(放浪神的小步舞曲WanderersMinuet)))
                                        return 军神的赞美歌ArmysPaeon;
                                }

                                if (songWanderer)
                                {
                                    if (songTimerInSeconds <= 3 && gauge.Repertoire > 0) // Spend any repertoire before switching to next song
                                        return OriginalHook(完美音调PitchPerfect);
                                    if (songTimerInSeconds <= 3 && ActionReady(贤者的叙事谣MagesBallad)) // Move to Mage's Ballad if <= 3 seconds left on song
                                        return 贤者的叙事谣MagesBallad;
                                }

                                if (songMage)
                                {

                                    // Move to Army's Paeon if < 3 seconds left on song
                                    if (songTimerInSeconds <= 3 && ActionReady(军神的赞美歌ArmysPaeon))
                                    {
                                        // Special case for Empyreal Arrow: it must be cast before you change to it to avoid drift!
                                        if (ActionReady(九天连箭EmpyrealArrow))
                                            return 九天连箭EmpyrealArrow;

                                        return 军神的赞美歌ArmysPaeon;
                                    }
                                }
                            }

                            if (songArmy && canWeaveDelayed)
                            {
                                // Move to Wanderer's Minuet if <= 12 seconds left on song or WM off CD and have 4 repertoires of AP
                                if (songTimerInSeconds <= 12 || (ActionReady(放浪神的小步舞曲WanderersMinuet) && gauge.Repertoire == 4))
                                    return 放浪神的小步舞曲WanderersMinuet;
                            }
                        }
                        else if (songTimerInSeconds <= 3 && canWeave)
                        {
                            if (ActionReady(贤者的叙事谣MagesBallad))
                                return 贤者的叙事谣MagesBallad;
                            if (ActionReady(军神的赞美歌ArmysPaeon))
                                return 军神的赞美歌ArmysPaeon;
                        }
                    }

                    if (IsEnabled(CustomComboPreset.BRD_AoE_Adv_Buffs) && (!songNone || !LevelChecked(贤者的叙事谣MagesBallad)) && isEnemyHealthHigh)
                    {
                        float battleVoiceCD = GetCooldownRemainingTime(战斗之声BattleVoice);
                        float ragingCD = GetCooldownRemainingTime(猛者强击RagingStrikes);

                        if (canWeaveDelayed
                            && ActionReady(光明神的最终乐章RadiantFinale)
                            && (Array.TrueForAll(gauge.Coda, SongIsNotNone) || Array.Exists(gauge.Coda, SongIsWandererMinuet))
                            && (battleVoiceCD < 3 || ActionReady(战斗之声BattleVoice))
                            && (ragingCD < 3 || ActionReady(猛者强击RagingStrikes)))
                            return 光明神的最终乐章RadiantFinale;

                        if (canWeaveBuffs && ActionReady(战斗之声BattleVoice) && (HasEffect(Buffs.RadiantFinale) || !LevelChecked(光明神的最终乐章RadiantFinale)))
                            return 战斗之声BattleVoice;

                        if (canWeaveBuffs && ActionReady(猛者强击RagingStrikes) && (HasEffect(Buffs.RadiantFinale) || !LevelChecked(光明神的最终乐章RadiantFinale)))
                            return 猛者强击RagingStrikes;

                        if (canWeaveBuffs && ActionReady(纷乱箭Barrage) && HasEffect(Buffs.强者猛击RagingStrikes))
                        {
                            if (LevelChecked(光明神的最终乐章RadiantFinale) && HasEffect(Buffs.RadiantFinale))
                                return 纷乱箭Barrage;
                            else if (LevelChecked(战斗之声BattleVoice) && HasEffect(Buffs.战斗之声BattleVoice))
                                return 纷乱箭Barrage;
                            else if (!LevelChecked(战斗之声BattleVoice) && HasEffect(Buffs.强者猛击RagingStrikes))
                                return 纷乱箭Barrage;

                        }
                    }


                    if (canWeave && IsEnabled(CustomComboPreset.BRD_AoE_Adv_oGCD))
                    {
                        float battleVoiceCD = GetCooldownRemainingTime(战斗之声BattleVoice);
                        float 九天连箭empyrealCD = GetCooldownRemainingTime(九天连箭EmpyrealArrow);
                        float ragingCD = GetCooldownRemainingTime(猛者强击RagingStrikes);
                        float radiantCD = GetCooldownRemainingTime(光明神的最终乐章RadiantFinale);

                        if (ActionReady(九天连箭EmpyrealArrow))
                            return 九天连箭EmpyrealArrow;

                        if (LevelChecked(完美音调PitchPerfect) && songWanderer && (gauge.Repertoire == 3 || (LevelChecked(九天连箭EmpyrealArrow) && gauge.Repertoire == 2 && 九天连箭empyrealCD < 2)))
                            return OriginalHook(完美音调PitchPerfect);

                        if (ActionReady(Sidewinder))
                        {
                            if (songWanderer)
                            {
                                if ((HasEffect(Buffs.强者猛击RagingStrikes) || ragingCD > 10) && (HasEffect(Buffs.战斗之声BattleVoice) || battleVoiceCD > 10) && (HasEffect(Buffs.RadiantFinale) || radiantCD > 10 || !LevelChecked(光明神的最终乐章RadiantFinale)))
                                    return Sidewinder;
                            }
                            else return Sidewinder;

                        }


                        if (LevelChecked(RainOfDeath) && !WasLastAction(RainOfDeath) && (九天连箭empyrealCD > 1 || !LevelChecked(九天连箭EmpyrealArrow)))
                        {
                            uint rainOfDeathCharges = LevelChecked(RainOfDeath) ? GetRemainingCharges(RainOfDeath) : 0;

                            if (IsEnabled(CustomComboPreset.BRD_AoE_Pooling) && LevelChecked(放浪神的小步舞曲WanderersMinuet) && TraitLevelChecked(Traits.EnhancedBloodletter))
                            {
                                if (songWanderer)
                                {
                                    if (((HasEffect(Buffs.强者猛击RagingStrikes) || ragingCD > 10) && (HasEffect(Buffs.战斗之声BattleVoice) || battleVoiceCD > 10 || !LevelChecked(战斗之声BattleVoice)) && (HasEffect(Buffs.RadiantFinale) || radiantCD > 10 || !LevelChecked(光明神的最终乐章RadiantFinale)) && rainOfDeathCharges > 0) || rainOfDeathCharges > 2)
                                        return OriginalHook(RainOfDeath);
                                }

                                if (songArmy && (rainOfDeathCharges == 3 || ((gauge.SongTimer / 1000) > 30 && rainOfDeathCharges > 0)))
                                    return OriginalHook(RainOfDeath);
                                if (songMage && rainOfDeathCharges > 0)
                                    return OriginalHook(RainOfDeath);
                                if (songNone && rainOfDeathCharges == 3)
                                    return OriginalHook(RainOfDeath);
                            }
                            else if (rainOfDeathCharges > 0)
                                return OriginalHook(RainOfDeath);
                        }

                        // healing - please move if not appropriate priority
                        if (IsEnabled(CustomComboPreset.BRD_AoE_SecondWind))
                        {
                            if (PlayerHealthPercentageHp() <= PluginConfiguration.GetCustomIntValue(Config.BRD_AoESecondWindThreshold) && ActionReady(All.SecondWind))
                                return All.SecondWind;
                        }
                    }

                    //Moved Below ogcds as it was preventing them from happening. 
                    if (HasEffect(Buffs.RadiantEncoreReady) && !JustUsed(光明神的最终乐章RadiantFinale) && GetCooldownElapsed(战斗之声BattleVoice) >= 4.2f && IsEnabled(CustomComboPreset.BRD_AoE_BuffsEncore))
                        return OriginalHook(光明神的返场余音RadiantEncore);

                    bool wideVolleyReady = LevelChecked(WideVolley) && (HasEffect(Buffs.HawksEye) || HasEffect(Buffs.纷乱箭));
                    bool blastArrowReady = LevelChecked(BlastArrow) && HasEffect(Buffs.BlastArrowReady);
                    bool resonantArrowReady = LevelChecked(共鸣箭ResonantArrow) && HasEffect(Buffs.ResonantArrowReady);

                    if (wideVolleyReady)
                        return OriginalHook(WideVolley);
                    if (LevelChecked(ApexArrow) && gauge.SoulVoice == 100 && IsEnabled(CustomComboPreset.BRD_Aoe_ApexArrow))
                        return ApexArrow;
                    if (blastArrowReady && IsEnabled(CustomComboPreset.BRD_Aoe_ApexArrow))
                        return BlastArrow;
                    if (resonantArrowReady && IsEnabled(CustomComboPreset.BRD_AoE_BuffsResonant))
                        return 共鸣箭ResonantArrow;
                    if (HasEffect(Buffs.RadiantEncoreReady) && IsEnabled(CustomComboPreset.BRD_AoE_BuffsEncore))
                        return OriginalHook(光明神的返场余音RadiantEncore);

                }

                return actionID;
            }
        }

        internal class BRD_ST_oGCD : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BRD_ST_oGCD;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID is Bloodletter or HeartbreakShot)
                {
                    BRDGauge? gauge = GetJobGauge<BRDGauge>();
                    bool songArmy = gauge.Song == Song.ARMY;
                    bool songWanderer = gauge.Song == Song.WANDERER;
                    bool minuetReady = LevelChecked(放浪神的小步舞曲WanderersMinuet) && IsOffCooldown(放浪神的小步舞曲WanderersMinuet);
                    bool balladReady = LevelChecked(贤者的叙事谣MagesBallad) && IsOffCooldown(贤者的叙事谣MagesBallad);
                    bool paeonReady = LevelChecked(军神的赞美歌ArmysPaeon) && IsOffCooldown(军神的赞美歌ArmysPaeon);

                    if (gauge.SongTimer < 1 || songArmy)
                    {
                        if (minuetReady)
                            return 放浪神的小步舞曲WanderersMinuet;
                        if (balladReady)
                            return 贤者的叙事谣MagesBallad;
                        if (paeonReady)
                            return 军神的赞美歌ArmysPaeon;
                    }

                    if (songWanderer && gauge.Repertoire == 3)
                        return OriginalHook(完美音调PitchPerfect);
                    if (ActionReady(九天连箭EmpyrealArrow))
                        return 九天连箭EmpyrealArrow;
                    if (ActionReady(Sidewinder))
                        return Sidewinder;
                    if (ActionReady(Bloodletter))
                        return OriginalHook(Bloodletter);

                }

                return actionID;
            }
        }

        internal class BRD_AoE_Combo : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BRD_AoE_Combo;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID is QuickNock or Ladonsbite)
                {
                    if (IsEnabled(CustomComboPreset.BRD_Apex))
                    {
                        BRDGauge? gauge = GetJobGauge<BRDGauge>();
                        bool blastReady = LevelChecked(BlastArrow) && HasEffect(Buffs.BlastArrowReady);

                        if (LevelChecked(ApexArrow) && gauge.SoulVoice == 100)
                            return ApexArrow;
                        if (blastReady)
                            return BlastArrow;
                    }

                    bool wideVolleyReady = LevelChecked(WideVolley) && HasEffect(Buffs.HawksEye);

                    if (IsEnabled(CustomComboPreset.BRD_AoE_Combo) && wideVolleyReady)
                        return OriginalHook(WideVolley);
                }

                return actionID;
            }
        }

        internal class BRD_ST_AdvMode : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BRD_ST_AdvMode;
            internal static bool inOpener = false;

            /// <summary>
            /// 这个好像没什么卵用
            /// </summary>
            internal static bool openerFinished = false;

            internal static byte step = 0;
            internal static byte subStep = 0;
            internal static bool usedStraightShotReady = false;
            internal static bool usedPitchPerfect = false;

            internal delegate bool DotRecast(int value);

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID is HeavyShot or BurstShot)
                {
                    BRDGauge? gauge = GetJobGauge<BRDGauge>();
                    bool canWeave = CanWeave(actionID);
                    bool canWeaveBuffs = CanWeave(actionID, 0.6);
                    bool canWeaveDelayed = CanDelayedWeave(actionID, 0.9);
                    bool songNone = gauge.Song == Song.NONE;
                    bool songWanderer = gauge.Song == Song.WANDERER;
                    bool songMage = gauge.Song == Song.MAGE;
                    bool songArmy = gauge.Song == Song.ARMY;
                    bool canInterrupt = CanInterruptEnemy() && IsOffCooldown(All.HeadGraze);
                    int targetHPThreshold = PluginConfiguration.GetCustomIntValue(Config.BRD_NoWasteHPPercentage);
                    bool isEnemyHealthHigh = !IsEnabled(CustomComboPreset.BRD_Adv_NoWaste) || GetTargetHPPercent() > targetHPThreshold;

                    if (!InCombat() && (inOpener || openerFinished))
                    {
                        openerFinished = false;
                    }

                    if (IsEnabled(CustomComboPreset.BRD_Adv_Interrupt) && canInterrupt)
                        return All.HeadGraze;

                    if (IsEnabled(CustomComboPreset.BRD_Variant_Cure) && IsEnabled(Variant.VariantCure) && PlayerHealthPercentageHp() <= GetOptionValue(Config.BRD_VariantCure))
                        return Variant.VariantCure;

                    if (IsEnabled(CustomComboPreset.BRD_Variant_Rampart) && IsEnabled(Variant.VariantRampart) && IsOffCooldown(Variant.VariantRampart) && canWeave)
                        return Variant.VariantRampart;

                    if (IsEnabled(CustomComboPreset.BRD_Adv_Song) && isEnemyHealthHigh)
                    {
                        int songTimerInSeconds = gauge.SongTimer / 1000;

                        // Limit optimisation to when you are high enough level to benefit from it.
                        if (LevelChecked(放浪神的小步舞曲WanderersMinuet))
                        {
                            // 43s of Wanderer's Minute, ~36s of Mage's Ballad, and ~43s of Army's Paeon    
                            bool minuetReady = IsOffCooldown(放浪神的小步舞曲WanderersMinuet);
                            bool balladReady = IsOffCooldown(贤者的叙事谣MagesBallad);
                            bool paeonReady = IsOffCooldown(军神的赞美歌ArmysPaeon);


                            if (canWeave)
                            {
                                if (songNone)
                                {
                                    // Logic to determine first song
                                    if (minuetReady && !(JustUsed(贤者的叙事谣MagesBallad) || JustUsed(军神的赞美歌ArmysPaeon)))
                                        return 放浪神的小步舞曲WanderersMinuet;
                                    if (balladReady && !(JustUsed(放浪神的小步舞曲WanderersMinuet) || JustUsed(军神的赞美歌ArmysPaeon)))
                                        return 贤者的叙事谣MagesBallad;
                                    if (paeonReady && !(JustUsed(贤者的叙事谣MagesBallad) || JustUsed(放浪神的小步舞曲WanderersMinuet)))
                                        return 军神的赞美歌ArmysPaeon;
                                }

                                if (songWanderer)
                                {
                                    if (songTimerInSeconds <= 3 && gauge.Repertoire > 0) // Spend any repertoire before switching to next song
                                        return OriginalHook(完美音调PitchPerfect);

                                    if (songTimerInSeconds <= 3 && balladReady) // Move to Mage's Ballad if <= 3 seconds left on song
                                        return 贤者的叙事谣MagesBallad;
                                }

                                if (songMage)
                                {
                                    // Move to Army's Paeon if <= 3 seconds left on song
                                    if (songTimerInSeconds <= 3 && paeonReady)
                                    {
                                        return 军神的赞美歌ArmysPaeon;
                                    }
                                }
                            }

                            if (songArmy && canWeaveDelayed)
                            {
                                // Move to Wanderer's Minuet if <= 12 seconds left on song or WM off CD and have 4 repertoires of AP
                                if (songTimerInSeconds <= 12 || (minuetReady && gauge.Repertoire == 4))
                                    return 放浪神的小步舞曲WanderersMinuet;
                            }
                        }
                        else if (songTimerInSeconds <= 3 && canWeave)
                        {
                            bool balladReady = LevelChecked(贤者的叙事谣MagesBallad) && IsOffCooldown(贤者的叙事谣MagesBallad);
                            bool paeonReady = LevelChecked(军神的赞美歌ArmysPaeon) && IsOffCooldown(军神的赞美歌ArmysPaeon);

                            if (balladReady)
                                return 贤者的叙事谣MagesBallad;
                            if (paeonReady)
                                return 军神的赞美歌ArmysPaeon;
                        }
                    }

                    if (IsEnabled(CustomComboPreset.BRD_Adv_Buffs) && (!songNone || !LevelChecked(贤者的叙事谣MagesBallad)) && isEnemyHealthHigh)
                    {
                        bool radiantReady = LevelChecked(光明神的最终乐章RadiantFinale) && IsOffCooldown(光明神的最终乐章RadiantFinale);
                        bool ragingReady = LevelChecked(猛者强击RagingStrikes) && IsOffCooldown(猛者强击RagingStrikes);
                        bool battleVoiceReady = LevelChecked(战斗之声BattleVoice) && IsOffCooldown(战斗之声BattleVoice);
                        bool barrageReady = LevelChecked(纷乱箭Barrage) && IsOffCooldown(纷乱箭Barrage);
                        float battleVoiceCD = GetCooldownRemainingTime(战斗之声BattleVoice);
                        float ragingCD = GetCooldownRemainingTime(猛者强击RagingStrikes);

                        if (canWeaveDelayed
                            && IsEnabled(CustomComboPreset.BRD_Adv_BuffsRadiant)
                            && radiantReady
                            && (Array.TrueForAll(gauge.Coda, SongIsNotNone) || Array.Exists(gauge.Coda, SongIsWandererMinuet))
                            && (battleVoiceCD < 3 || ActionReady(战斗之声BattleVoice))
                            && (ragingCD < 3 || ActionReady(猛者强击RagingStrikes)))
                            return 光明神的最终乐章RadiantFinale;

                        if (canWeaveBuffs && battleVoiceReady && (HasEffect(Buffs.RadiantFinale) || !LevelChecked(光明神的最终乐章RadiantFinale)))
                            return 战斗之声BattleVoice;

                        if (canWeaveBuffs && ragingReady && (HasEffect(Buffs.RadiantFinale) || !LevelChecked(光明神的最终乐章RadiantFinale)))
                            return 猛者强击RagingStrikes;

                        //removed requirement to not have hawks eye, it is better to maybe lose 60 potency than allow it to drift a 1000 potency gain out of the window
                        if (canWeaveBuffs && barrageReady && HasEffect(Buffs.强者猛击RagingStrikes))
                        {
                            if (LevelChecked(光明神的最终乐章RadiantFinale) && HasEffect(Buffs.RadiantFinale))
                                return 纷乱箭Barrage;
                            else if (LevelChecked(战斗之声BattleVoice) && HasEffect(Buffs.战斗之声BattleVoice))
                                return 纷乱箭Barrage;
                            else if (!LevelChecked(战斗之声BattleVoice) && HasEffect(Buffs.强者猛击RagingStrikes))
                                return 纷乱箭Barrage;
                        }
                    }

                    if (canWeave && IsEnabled(CustomComboPreset.BRD_ST_Adv_oGCD))
                    {
                        float battleVoiceCD = GetCooldownRemainingTime(战斗之声BattleVoice);
                        float empyrealCD = GetCooldownRemainingTime(九天连箭EmpyrealArrow);
                        float ragingCD = GetCooldownRemainingTime(猛者强击RagingStrikes);
                        float radiantCD = GetCooldownRemainingTime(光明神的最终乐章RadiantFinale);


                        if (LevelChecked(完美音调PitchPerfect) && songWanderer && (gauge.Repertoire == 3 || (LevelChecked(九天连箭EmpyrealArrow) && gauge.Repertoire == 2 && empyrealCD < 2)))
                            return OriginalHook(完美音调PitchPerfect);

                        if (ActionReady(Sidewinder))
                        {
                            if (IsEnabled(CustomComboPreset.BRD_Adv_Pooling))
                            {
                                if (songWanderer)
                                {
                                    if ((HasEffect(Buffs.强者猛击RagingStrikes) || ragingCD > 10) && (HasEffect(Buffs.战斗之声BattleVoice) || battleVoiceCD > 10) && (HasEffect(Buffs.RadiantFinale) || radiantCD > 10 || !LevelChecked(光明神的最终乐章RadiantFinale)))
                                        return Sidewinder;
                                }
                                else return Sidewinder;
                            }
                            else return Sidewinder;
                        }


                        if (ActionReady(Bloodletter) && !(WasLastAction(Bloodletter) || WasLastAction(HeartbreakShot)) && (empyrealCD > 1 || !LevelChecked(九天连箭EmpyrealArrow)))
                        {
                            uint bloodletterCharges = GetRemainingCharges(Bloodletter);

                            if (IsEnabled(CustomComboPreset.BRD_Adv_Pooling) && LevelChecked(放浪神的小步舞曲WanderersMinuet) && TraitLevelChecked(Traits.EnhancedBloodletter))
                            {
                                if (songWanderer)
                                {
                                    if (((HasEffect(Buffs.强者猛击RagingStrikes) || ragingCD > 10) && (HasEffect(Buffs.战斗之声BattleVoice) || battleVoiceCD > 10 || !LevelChecked(战斗之声BattleVoice)) && (HasEffect(Buffs.RadiantFinale) || radiantCD > 10 || !LevelChecked(光明神的最终乐章RadiantFinale)) && bloodletterCharges > 0) || bloodletterCharges > 2)
                                        return OriginalHook(Bloodletter);
                                }

                                if (songArmy && (bloodletterCharges == 3 || ((gauge.SongTimer / 1000) > 30 && bloodletterCharges > 0)))
                                    return OriginalHook(Bloodletter);
                                if (songMage && bloodletterCharges > 0)
                                    return OriginalHook(Bloodletter);
                                if (songNone && bloodletterCharges == 3)
                                    return OriginalHook(Bloodletter);
                            }
                            else if (bloodletterCharges > 0)
                                return OriginalHook(Bloodletter);
                        }

                        // healing - please move if not appropriate priority
                        if (IsEnabled(CustomComboPreset.BRD_ST_SecondWind))
                        {
                            if (PlayerHealthPercentageHp() <= PluginConfiguration.GetCustomIntValue(Config.BRD_STSecondWindThreshold) && ActionReady(All.SecondWind))
                                return All.SecondWind;
                        }

                        if (ActionReady(九天连箭EmpyrealArrow) && HasEffect(Buffs.战斗之声BattleVoice) && HasEffect(Buffs.强者猛击RagingStrikes))
                        {
                            return 九天连箭EmpyrealArrow;
                        }

                        if (ActionReady(九天连箭EmpyrealArrow) && GetCooldownRemainingTime(战斗之声BattleVoice) >= 10)
                        {
                            return 九天连箭EmpyrealArrow;
                        }

                    }
                    //Moved below weaves bc roobert says it is blocking his weaves from happening
                    if (HasEffect(Buffs.RadiantEncoreReady) && !JustUsed(光明神的最终乐章RadiantFinale) && GetCooldownElapsed(光明神的最终乐章RadiantFinale) >= 4.2f && IsEnabled(CustomComboPreset.BRD_Adv_BuffsEncore))
                        return OriginalHook(光明神的返场余音RadiantEncore);

                    if (isEnemyHealthHigh)
                    {
                        bool venomous = TargetHasEffect(Debuffs.毒VenomousBite);
                        bool windbite = TargetHasEffect(Debuffs.风Windbite);
                        bool caustic = TargetHasEffect(Debuffs.烈毒咬箭CausticBite);
                        bool stormbite = TargetHasEffect(Debuffs.Stormbite);
                        float venomRemaining = GetDebuffRemainingTime(Debuffs.毒VenomousBite);
                        float windRemaining = GetDebuffRemainingTime(Debuffs.风Windbite);
                        float causticRemaining = GetDebuffRemainingTime(Debuffs.烈毒咬箭CausticBite);
                        float stormRemaining = GetDebuffRemainingTime(Debuffs.Stormbite);
                        float ragingStrikesDuration = GetBuffRemainingTime(Buffs.强者猛击RagingStrikes);
                        float radiantFinaleDuration = GetBuffRemainingTime(Buffs.RadiantFinale);
                        int ragingJawsRenewTime = PluginConfiguration.GetCustomIntValue(Config.BRD_RagingJawsRenewTime);

                        DotRecast poisonRecast = delegate(int duration) { return (venomous && venomRemaining < duration) || (caustic && causticRemaining < duration); };

                        DotRecast windRecast = delegate(int duration) { return (windbite && windRemaining < duration) || (stormbite && stormRemaining < duration); };

                        if (IsEnabled(CustomComboPreset.BRD_Adv_DoT))
                        {

                            if (LevelChecked(Stormbite) && !stormbite)
                                return Stormbite;
                            if (LevelChecked(CausticBite) && !caustic)
                                return CausticBite;
                            if (LevelChecked(风蚀箭Windbite) && !windbite && !LevelChecked(Stormbite))
                                return 风蚀箭Windbite;
                            if (LevelChecked(毒咬箭VenomousBite) && !venomous && !LevelChecked(CausticBite))
                                return 毒咬箭VenomousBite;


                            if (!LevelChecked(伶牙俐齿IronJaws))
                            {
                                if (windbite && windRemaining < 4)
                                {
                                    openerFinished = true;
                                    return 风蚀箭Windbite;
                                }

                                if (venomous && venomRemaining < 4)
                                {
                                    openerFinished = true;
                                    return 毒咬箭VenomousBite;
                                }
                            }
                        }

                        if (ActionReady(伶牙俐齿IronJaws) && IsEnabled(CustomComboPreset.BRD_Adv_IronJaws) && poisonRecast(4) && windRecast(4))
                        {
                            openerFinished = true;
                            return 伶牙俐齿IronJaws;
                        }


                        if (ActionReady(伶牙俐齿IronJaws) && IsEnabled(CustomComboPreset.BRD_Adv_RagingJaws) && HasEffect(Buffs.强者猛击RagingStrikes) && !WasLastAction(伶牙俐齿IronJaws) && ragingStrikesDuration < ragingJawsRenewTime && poisonRecast(35) && windRecast(35))
                        {
                            openerFinished = true;
                            return 伶牙俐齿IronJaws;
                        }
                    }

                    if (IsEnabled(CustomComboPreset.BRD_ST_ApexArrow))
                    {
                        if (LevelChecked(BlastArrow) && HasEffect(Buffs.BlastArrowReady))
                            return BlastArrow;

                        if (LevelChecked(ApexArrow))
                        {
                            int songTimerInSeconds = gauge.SongTimer / 1000;

                            if (songMage && gauge.SoulVoice == 100)
                                return ApexArrow;
                            if (songMage && gauge.SoulVoice >= 80 && songTimerInSeconds > 18 && songTimerInSeconds < 22)
                                return ApexArrow;
                            if (songWanderer && HasEffect(Buffs.强者猛击RagingStrikes) && HasEffect(Buffs.战斗之声BattleVoice) && (HasEffect(Buffs.RadiantFinale) || !LevelChecked(光明神的最终乐章RadiantFinale)) && gauge.SoulVoice >= 80)
                                return ApexArrow;
                        }
                    }

                    if (HasEffect(Buffs.HawksEye) || HasEffect(Buffs.纷乱箭))
                        return OriginalHook(StraightShot);

                    if (HasEffect(Buffs.ResonantArrowReady) && IsEnabled(CustomComboPreset.BRD_Adv_BuffsResonant))
                        return 共鸣箭ResonantArrow;

                }

                return actionID;
            }
        }

        internal class BRD_Buffs : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BRD_Buffs;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID is 纷乱箭Barrage)
                {
                    if (ActionReady(猛者强击RagingStrikes))
                        return 猛者强击RagingStrikes;
                    if (ActionReady(战斗之声BattleVoice))
                        return 战斗之声BattleVoice;
                }

                return actionID;
            }
        }

        internal class BRD_OneButtonSongs : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BRD_OneButtonSongs;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID is 放浪神的小步舞曲WanderersMinuet)
                {
                    // Doesn't display the lowest cooldown song if they have been used out of order and are all on cooldown.
                    BRDGauge? gauge = GetJobGauge<BRDGauge>();
                    int songTimerInSeconds = gauge.SongTimer / 1000;

                    if (ActionReady(放浪神的小步舞曲WanderersMinuet) || (gauge.Song == Song.WANDERER && songTimerInSeconds > 11))
                        return 放浪神的小步舞曲WanderersMinuet;

                    if (ActionReady(贤者的叙事谣MagesBallad) || (gauge.Song == Song.MAGE && songTimerInSeconds > 2))
                        return 贤者的叙事谣MagesBallad;

                    if (ActionReady(军神的赞美歌ArmysPaeon) || (gauge.Song == Song.ARMY && songTimerInSeconds > 2))
                        return 军神的赞美歌ArmysPaeon;

                }

                return actionID;
            }
        }

        internal class BRD_AoE_SimpleMode : CustomCombo
        {
            internal static bool inOpener = false;
            internal static bool openerFinished = false;

            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BRD_AoE_SimpleMode;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID is Ladonsbite or QuickNock)
                {
                    BRDGauge? gauge = GetJobGauge<BRDGauge>();
                    bool canWeave = CanWeave(actionID);
                    bool canWeaveBuffs = CanWeave(actionID, 0.6);
                    bool canWeaveDelayed = CanDelayedWeave(actionID, 0.9);
                    int songTimerInSeconds = gauge.SongTimer / 1000;
                    bool songNone = gauge.Song == Song.NONE;
                    bool songWanderer = gauge.Song == Song.WANDERER;
                    bool songMage = gauge.Song == Song.MAGE;
                    bool songArmy = gauge.Song == Song.ARMY;
                    int targetHPThreshold = PluginConfiguration.GetCustomIntValue(Config.BRD_AoENoWasteHPPercentage);
                    bool isEnemyHealthHigh = GetTargetHPPercent() > 1;

                    if (IsEnabled(Variant.VariantCure) && PlayerHealthPercentageHp() <= 50)
                        return Variant.VariantCure;

                    if (IsEnabled(Variant.VariantRampart) && IsOffCooldown(Variant.VariantRampart) && canWeave)
                        return Variant.VariantRampart;

                    if (canWeave)
                    {
                        // Limit optimisation to when you are high enough level to benefit from it.
                        if (LevelChecked(放浪神的小步舞曲WanderersMinuet))
                        {
                            if (canWeave)
                            {
                                if (songNone)
                                {
                                    // Logic to determine first song
                                    if (ActionReady(放浪神的小步舞曲WanderersMinuet) && !(JustUsed(贤者的叙事谣MagesBallad) || JustUsed(军神的赞美歌ArmysPaeon)))
                                        return 放浪神的小步舞曲WanderersMinuet;
                                    if (ActionReady(贤者的叙事谣MagesBallad) && !(JustUsed(放浪神的小步舞曲WanderersMinuet) || JustUsed(军神的赞美歌ArmysPaeon)))
                                        return 贤者的叙事谣MagesBallad;
                                    if (ActionReady(军神的赞美歌ArmysPaeon) && !(JustUsed(贤者的叙事谣MagesBallad) || JustUsed(放浪神的小步舞曲WanderersMinuet)))
                                        return 军神的赞美歌ArmysPaeon;
                                }

                                if (songWanderer)
                                {
                                    if (songTimerInSeconds <= 3 && gauge.Repertoire > 0) // Spend any repertoire before switching to next song
                                        return OriginalHook(完美音调PitchPerfect);
                                    if (songTimerInSeconds <= 3 && ActionReady(贤者的叙事谣MagesBallad)) // Move to Mage's Ballad if <= 3 seconds left on song
                                        return 贤者的叙事谣MagesBallad;
                                }

                                if (songMage)
                                {

                                    // Move to Army's Paeon if < 3 seconds left on song
                                    if (songTimerInSeconds <= 3 && ActionReady(军神的赞美歌ArmysPaeon))
                                    {
                                        // Special case for Empyreal Arrow: it must be cast before you change to it to avoid drift!
                                        if (ActionReady(九天连箭EmpyrealArrow))
                                            return 九天连箭EmpyrealArrow;

                                        return 军神的赞美歌ArmysPaeon;
                                    }
                                }
                            }

                            if (songArmy && canWeaveDelayed)
                            {
                                // Move to Wanderer's Minuet if <= 12 seconds left on song or WM off CD and have 4 repertoires of AP
                                if (songTimerInSeconds <= 12 || (ActionReady(放浪神的小步舞曲WanderersMinuet) && gauge.Repertoire == 4))
                                    return 放浪神的小步舞曲WanderersMinuet;
                            }
                        }
                        else if (songTimerInSeconds <= 3 && canWeave)
                        {
                            if (ActionReady(贤者的叙事谣MagesBallad))
                                return 贤者的叙事谣MagesBallad;
                            if (ActionReady(军神的赞美歌ArmysPaeon))
                                return 军神的赞美歌ArmysPaeon;
                        }
                    }

                    if ((!songNone || !LevelChecked(贤者的叙事谣MagesBallad)) && isEnemyHealthHigh)
                    {
                        float battleVoiceCD = GetCooldownRemainingTime(战斗之声BattleVoice);
                        float ragingCD = GetCooldownRemainingTime(猛者强击RagingStrikes);

                        if (canWeaveDelayed
                            && ActionReady(光明神的最终乐章RadiantFinale)
                            && (Array.TrueForAll(gauge.Coda, SongIsNotNone) || Array.Exists(gauge.Coda, SongIsWandererMinuet))
                            && (battleVoiceCD < 3 || ActionReady(战斗之声BattleVoice))
                            && (ragingCD < 3 || ActionReady(猛者强击RagingStrikes)))
                            return 光明神的最终乐章RadiantFinale;

                        if (canWeaveBuffs && ActionReady(战斗之声BattleVoice) && (HasEffect(Buffs.RadiantFinale) || !LevelChecked(光明神的最终乐章RadiantFinale)))
                            return 战斗之声BattleVoice;

                        if (canWeaveBuffs && ActionReady(猛者强击RagingStrikes) && (HasEffect(Buffs.RadiantFinale) || !LevelChecked(光明神的最终乐章RadiantFinale)))
                            return 猛者强击RagingStrikes;

                        if (canWeaveBuffs && ActionReady(纷乱箭Barrage) && HasEffect(Buffs.强者猛击RagingStrikes))
                        {
                            if (LevelChecked(光明神的最终乐章RadiantFinale) && HasEffect(Buffs.RadiantFinale))
                                return 纷乱箭Barrage;
                            else if (LevelChecked(战斗之声BattleVoice) && HasEffect(Buffs.战斗之声BattleVoice))
                                return 纷乱箭Barrage;
                            else if (!LevelChecked(战斗之声BattleVoice) && HasEffect(Buffs.强者猛击RagingStrikes))
                                return 纷乱箭Barrage;

                        }
                    }


                    if (canWeave)
                    {
                        float battleVoiceCD = GetCooldownRemainingTime(战斗之声BattleVoice);
                        float empyrealCD = GetCooldownRemainingTime(九天连箭EmpyrealArrow);
                        float ragingCD = GetCooldownRemainingTime(猛者强击RagingStrikes);
                        float radiantCD = GetCooldownRemainingTime(光明神的最终乐章RadiantFinale);

                        if (ActionReady(九天连箭EmpyrealArrow))
                            return 九天连箭EmpyrealArrow;

                        if (LevelChecked(完美音调PitchPerfect) && songWanderer && (gauge.Repertoire == 3 || (LevelChecked(九天连箭EmpyrealArrow) && gauge.Repertoire == 2 && empyrealCD < 2)))
                            return OriginalHook(完美音调PitchPerfect);

                        if (ActionReady(Sidewinder))
                        {
                            if (songWanderer)
                            {
                                if ((HasEffect(Buffs.强者猛击RagingStrikes) || ragingCD > 10) && (HasEffect(Buffs.战斗之声BattleVoice) || battleVoiceCD > 10) && (HasEffect(Buffs.RadiantFinale) || radiantCD > 10 || !LevelChecked(光明神的最终乐章RadiantFinale)))
                                    return Sidewinder;

                            }
                            else return Sidewinder;
                        }

                        // if (LevelChecked(RainOfDeath) && (empyrealCD > 1 || !LevelChecked(九天连箭EmpyrealArrow)))
                        if (LevelChecked(RainOfDeath) && !WasLastAction(RainOfDeath) && (empyrealCD > 1 || !LevelChecked(九天连箭EmpyrealArrow)))
                        {
                            uint rainOfDeathCharges = LevelChecked(RainOfDeath) ? GetRemainingCharges(RainOfDeath) : 0;

                            if (LevelChecked(放浪神的小步舞曲WanderersMinuet) && TraitLevelChecked(Traits.EnhancedBloodletter))
                            {
                                if (songWanderer)
                                {
                                    if (((HasEffect(Buffs.强者猛击RagingStrikes) || ragingCD > 10) && (HasEffect(Buffs.战斗之声BattleVoice) || battleVoiceCD > 10 || !LevelChecked(战斗之声BattleVoice)) && (HasEffect(Buffs.RadiantFinale) || radiantCD > 10 || !LevelChecked(光明神的最终乐章RadiantFinale)) && rainOfDeathCharges > 0) || rainOfDeathCharges > 2)
                                        return OriginalHook(RainOfDeath);
                                }

                                if (songArmy && (rainOfDeathCharges == 3 || ((gauge.SongTimer / 1000) > 30 && rainOfDeathCharges > 0)))
                                    return OriginalHook(RainOfDeath);
                                if (songMage && rainOfDeathCharges > 0)
                                    return OriginalHook(RainOfDeath);
                                if (songNone && rainOfDeathCharges == 3)
                                    return OriginalHook(RainOfDeath);
                            }
                            else if (rainOfDeathCharges > 0)
                                return OriginalHook(RainOfDeath);
                        }
                        //Moved Below ogcds as it was preventing them from happening. 
                        if (HasEffect(Buffs.RadiantEncoreReady) && !JustUsed(光明神的最终乐章RadiantFinale) && GetCooldownElapsed(战斗之声BattleVoice) >= 4.2f)
                            return OriginalHook(光明神的返场余音RadiantEncore);

                        // healing - please move if not appropriate priority

                        if (PlayerHealthPercentageHp() <= 40 && ActionReady(All.SecondWind))
                            return All.SecondWind;

                    }

                    bool wideVolleyReady = LevelChecked(WideVolley) && (HasEffect(Buffs.HawksEye) || HasEffect(Buffs.纷乱箭));
                    bool blastArrowReady = LevelChecked(BlastArrow) && HasEffect(Buffs.BlastArrowReady);
                    bool resonantArrowReady = LevelChecked(共鸣箭ResonantArrow) && HasEffect(Buffs.ResonantArrowReady);

                    if (wideVolleyReady)
                        return OriginalHook(WideVolley);
                    if (LevelChecked(ApexArrow) && gauge.SoulVoice == 100)
                        return ApexArrow;
                    if (blastArrowReady)
                        return BlastArrow;
                    if (resonantArrowReady)
                        return 共鸣箭ResonantArrow;
                    if (HasEffect(Buffs.RadiantEncoreReady))
                        return OriginalHook(光明神的返场余音RadiantEncore);

                }

                return actionID;
            }
        }

        internal class BRD_ST_SimpleMode : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BRD_ST_SimpleMode;
            internal static bool inOpener = false;
            internal static bool openerFinished = false;
            internal static byte step = 0;
            internal static byte subStep = 0;
            internal static bool usedStraightShotReady = false;
            internal static bool usedPitchPerfect = false;

            internal delegate bool DotRecast(int value);

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID is HeavyShot or BurstShot)
                {
                    BRDGauge? gauge = GetJobGauge<BRDGauge>();
                    bool canWeave = CanWeave(actionID);
                    bool canWeaveBuffs = CanWeave(actionID, 0.6);
                    bool canWeaveDelayed = CanDelayedWeave(actionID, 0.9);
                    bool songNone = gauge.Song == Song.NONE;
                    bool songWanderer = gauge.Song == Song.WANDERER;
                    bool songMage = gauge.Song == Song.MAGE;
                    bool songArmy = gauge.Song == Song.ARMY;
                    bool canInterrupt = CanInterruptEnemy() && IsOffCooldown(All.HeadGraze);
                    bool isEnemyHealthHigh = GetTargetHPPercent() > 1;

                    if (!InCombat() && (inOpener || openerFinished))
                    {
                        openerFinished = false;
                    }

                    if (canInterrupt)
                        return All.HeadGraze;

                    if (IsEnabled(Variant.VariantCure) && PlayerHealthPercentageHp() <= 50)
                        return Variant.VariantCure;

                    if (IsEnabled(Variant.VariantRampart) && IsOffCooldown(Variant.VariantRampart) && canWeave)
                        return Variant.VariantRampart;

                    if (isEnemyHealthHigh)
                    {
                        int songTimerInSeconds = gauge.SongTimer / 1000;

                        // Limit optimisation to when you are high enough level to benefit from it.
                        if (LevelChecked(放浪神的小步舞曲WanderersMinuet))
                        {
                            // 43s of Wanderer's Minute, ~36s of Mage's Ballad, and ~43s of Army's Paeon    
                            bool minuetReady = IsOffCooldown(放浪神的小步舞曲WanderersMinuet);
                            bool balladReady = IsOffCooldown(贤者的叙事谣MagesBallad);
                            bool paeonReady = IsOffCooldown(军神的赞美歌ArmysPaeon);

                            if (ActionReady(九天连箭EmpyrealArrow) && JustUsed(放浪神的小步舞曲WanderersMinuet))
                                return 九天连箭EmpyrealArrow;

                            if (canWeave)
                            {
                                if (songNone)
                                {
                                    // Logic to determine first song
                                    if (minuetReady && !(JustUsed(贤者的叙事谣MagesBallad) || JustUsed(军神的赞美歌ArmysPaeon)))
                                        return 放浪神的小步舞曲WanderersMinuet;
                                    if (balladReady && !(JustUsed(放浪神的小步舞曲WanderersMinuet) || JustUsed(军神的赞美歌ArmysPaeon)))
                                        return 贤者的叙事谣MagesBallad;
                                    if (paeonReady && !(JustUsed(贤者的叙事谣MagesBallad) || JustUsed(放浪神的小步舞曲WanderersMinuet)))
                                        return 军神的赞美歌ArmysPaeon;
                                }

                                if (songWanderer)
                                {
                                    if (songTimerInSeconds <= 3 && gauge.Repertoire > 0) // Spend any repertoire before switching to next song
                                        return OriginalHook(完美音调PitchPerfect);
                                    if (songTimerInSeconds <= 3 && balladReady) // Move to Mage's Ballad if <= 3 seconds left on song
                                        return 贤者的叙事谣MagesBallad;
                                }

                                if (songMage)
                                {

                                    // Move to Army's Paeon if <= 3 seconds left on song
                                    if (songTimerInSeconds <= 3 && paeonReady)
                                    {
                                        // Special case for Empyreal Arrow: it must be cast before you change to it to avoid drift!
                                        if (ActionReady(九天连箭EmpyrealArrow))
                                            return 九天连箭EmpyrealArrow;

                                        return 军神的赞美歌ArmysPaeon;
                                    }
                                }
                            }

                            if (songArmy && canWeaveDelayed)
                            {
                                // Move to Wanderer's Minuet if <= 12 seconds left on song or WM off CD and have 4 repertoires of AP
                                if (songTimerInSeconds <= 12 || (minuetReady && gauge.Repertoire == 4))
                                    return 放浪神的小步舞曲WanderersMinuet;
                            }
                        }
                        else if (songTimerInSeconds <= 3 && canWeave)
                        {
                            bool balladReady = LevelChecked(贤者的叙事谣MagesBallad) && IsOffCooldown(贤者的叙事谣MagesBallad);
                            bool paeonReady = LevelChecked(军神的赞美歌ArmysPaeon) && IsOffCooldown(军神的赞美歌ArmysPaeon);

                            if (balladReady)
                                return 贤者的叙事谣MagesBallad;
                            if (paeonReady)
                                return 军神的赞美歌ArmysPaeon;
                        }
                    }

                    if ((!songNone || !LevelChecked(贤者的叙事谣MagesBallad)) && isEnemyHealthHigh)
                    {
                        bool radiantReady = LevelChecked(光明神的最终乐章RadiantFinale) && IsOffCooldown(光明神的最终乐章RadiantFinale);
                        bool ragingReady = LevelChecked(猛者强击RagingStrikes) && IsOffCooldown(猛者强击RagingStrikes);
                        bool battleVoiceReady = LevelChecked(战斗之声BattleVoice) && IsOffCooldown(战斗之声BattleVoice);
                        bool barrageReady = LevelChecked(纷乱箭Barrage) && IsOffCooldown(纷乱箭Barrage);
                        float battleVoiceCD = GetCooldownRemainingTime(战斗之声BattleVoice);
                        float ragingCD = GetCooldownRemainingTime(猛者强击RagingStrikes);

                        if (canWeaveDelayed
                            && radiantReady
                            && (Array.TrueForAll(gauge.Coda, SongIsNotNone) || Array.Exists(gauge.Coda, SongIsWandererMinuet))
                            && (battleVoiceCD < 3 || ActionReady(战斗之声BattleVoice))
                            && (ragingCD < 3 || ActionReady(猛者强击RagingStrikes)))
                            return 光明神的最终乐章RadiantFinale;

                        if (canWeaveBuffs && battleVoiceReady && (HasEffect(Buffs.RadiantFinale) || !LevelChecked(光明神的最终乐章RadiantFinale)))
                            return 战斗之声BattleVoice;

                        if (canWeaveBuffs && ragingReady && (HasEffect(Buffs.RadiantFinale) || !LevelChecked(光明神的最终乐章RadiantFinale)))
                            return 猛者强击RagingStrikes;

                        //removed requirement to not have hawks eye, it is better to maybe lose 60 potency than allow it to drift a 1000 potency gain out of the window
                        if (canWeaveBuffs && barrageReady && HasEffect(Buffs.强者猛击RagingStrikes))
                        {
                            if (LevelChecked(光明神的最终乐章RadiantFinale) && HasEffect(Buffs.RadiantFinale))
                                return 纷乱箭Barrage;
                            else if (LevelChecked(战斗之声BattleVoice) && HasEffect(Buffs.战斗之声BattleVoice))
                                return 纷乱箭Barrage;
                            else if (!LevelChecked(战斗之声BattleVoice) && HasEffect(Buffs.强者猛击RagingStrikes))
                                return 纷乱箭Barrage;
                        }
                    }

                    if (canWeave)
                    {
                        float battleVoiceCD = GetCooldownRemainingTime(战斗之声BattleVoice);
                        float empyrealCD = GetCooldownRemainingTime(九天连箭EmpyrealArrow);
                        float ragingCD = GetCooldownRemainingTime(猛者强击RagingStrikes);
                        float radiantCD = GetCooldownRemainingTime(光明神的最终乐章RadiantFinale);

                        if (ActionReady(九天连箭EmpyrealArrow))
                            return 九天连箭EmpyrealArrow;

                        if (LevelChecked(完美音调PitchPerfect) && songWanderer && (gauge.Repertoire == 3 || (LevelChecked(九天连箭EmpyrealArrow) && gauge.Repertoire == 2 && empyrealCD < 2)))
                            return OriginalHook(完美音调PitchPerfect);

                        if (ActionReady(Sidewinder))
                        {
                            if (songWanderer)
                            {
                                if ((HasEffect(Buffs.强者猛击RagingStrikes) || ragingCD > 10) && (HasEffect(Buffs.战斗之声BattleVoice) || battleVoiceCD > 10) && (HasEffect(Buffs.RadiantFinale) || radiantCD > 10 || !LevelChecked(光明神的最终乐章RadiantFinale)))
                                    return Sidewinder;
                            }
                            else return Sidewinder;
                        }


                        if (ActionReady(Bloodletter) && !(WasLastAction(Bloodletter) || WasLastAction(HeartbreakShot)) && (empyrealCD > 1 || !LevelChecked(九天连箭EmpyrealArrow)))
                        {
                            uint bloodletterCharges = GetRemainingCharges(Bloodletter);

                            if (LevelChecked(放浪神的小步舞曲WanderersMinuet) && TraitLevelChecked(Traits.EnhancedBloodletter))
                            {
                                if (songWanderer)
                                {
                                    if (((HasEffect(Buffs.强者猛击RagingStrikes) || ragingCD > 10) && (HasEffect(Buffs.战斗之声BattleVoice) || battleVoiceCD > 10 || !LevelChecked(战斗之声BattleVoice)) && (HasEffect(Buffs.RadiantFinale) || radiantCD > 10 || !LevelChecked(光明神的最终乐章RadiantFinale)) && bloodletterCharges > 0) || bloodletterCharges > 2)
                                        return OriginalHook(Bloodletter);
                                }

                                if (songArmy && (bloodletterCharges == 3 || ((gauge.SongTimer / 1000) > 30 && bloodletterCharges > 0)))
                                    return OriginalHook(Bloodletter);
                                if (songMage && bloodletterCharges > 0)
                                    return OriginalHook(Bloodletter);
                                if (songNone && bloodletterCharges == 3)
                                    return OriginalHook(Bloodletter);
                            }
                            else if (bloodletterCharges > 0)
                                return OriginalHook(Bloodletter);
                        }

                        // healing - please move if not appropriate priority

                        if (PlayerHealthPercentageHp() <= 40 && ActionReady(All.SecondWind))
                            return All.SecondWind;

                    }

                    //Moved below weaves bc roobert says it is blocking his weaves from happening
                    if (HasEffect(Buffs.RadiantEncoreReady) && !JustUsed(光明神的最终乐章RadiantFinale) && GetCooldownElapsed(战斗之声BattleVoice) >= 4.2f)
                        return OriginalHook(光明神的返场余音RadiantEncore);

                    if (isEnemyHealthHigh)
                    {
                        bool venomous = TargetHasEffect(Debuffs.毒VenomousBite);
                        bool windbite = TargetHasEffect(Debuffs.风Windbite);
                        bool caustic = TargetHasEffect(Debuffs.烈毒咬箭CausticBite);
                        bool stormbite = TargetHasEffect(Debuffs.Stormbite);
                        float venomRemaining = GetDebuffRemainingTime(Debuffs.毒VenomousBite);
                        float windRemaining = GetDebuffRemainingTime(Debuffs.风Windbite);
                        float causticRemaining = GetDebuffRemainingTime(Debuffs.烈毒咬箭CausticBite);
                        float stormRemaining = GetDebuffRemainingTime(Debuffs.Stormbite);
                        float ragingStrikesDuration = GetBuffRemainingTime(Buffs.强者猛击RagingStrikes);
                        float radiantFinaleDuration = GetBuffRemainingTime(Buffs.RadiantFinale);
                        int ragingJawsRenewTime = 5;

                        DotRecast poisonRecast = delegate(int duration) { return (venomous && venomRemaining < duration) || (caustic && causticRemaining < duration); };

                        DotRecast windRecast = delegate(int duration) { return (windbite && windRemaining < duration) || (stormbite && stormRemaining < duration); };

                        if (ActionReady(伶牙俐齿IronJaws) && HasEffect(Buffs.强者猛击RagingStrikes) && !WasLastAction(伶牙俐齿IronJaws) && ragingStrikesDuration < ragingJawsRenewTime && poisonRecast(35) && windRecast(35))
                        {
                            openerFinished = true;
                            return 伶牙俐齿IronJaws;
                        }

                        if (LevelChecked(Stormbite) && !stormbite)
                            return Stormbite;
                        if (LevelChecked(CausticBite) && !caustic)
                            return CausticBite;
                        if (LevelChecked(风蚀箭Windbite) && !windbite && !LevelChecked(Stormbite))
                            return 风蚀箭Windbite;
                        if (LevelChecked(毒咬箭VenomousBite) && !venomous && !LevelChecked(CausticBite))
                            return 毒咬箭VenomousBite;

                        if (ActionReady(伶牙俐齿IronJaws) && poisonRecast(4) && windRecast(4))
                        {
                            openerFinished = true;
                            return 伶牙俐齿IronJaws;
                        }
                        if (!LevelChecked(伶牙俐齿IronJaws))
                        {
                            if (windbite && windRemaining < 4)
                            {
                                openerFinished = true;
                                return 风蚀箭Windbite;
                            }

                            if (venomous && venomRemaining < 4)
                            {
                                openerFinished = true;
                                return 毒咬箭VenomousBite;
                            }
                        }

                    }

                    if (LevelChecked(BlastArrow) && HasEffect(Buffs.BlastArrowReady))
                        return BlastArrow;

                    if (LevelChecked(ApexArrow))
                    {
                        int songTimerInSeconds = gauge.SongTimer / 1000;

                        if (songMage && gauge.SoulVoice == 100)
                            return ApexArrow;
                        if (songMage && gauge.SoulVoice >= 80 && songTimerInSeconds > 18 && songTimerInSeconds < 22)
                            return ApexArrow;
                        if (songWanderer && HasEffect(Buffs.强者猛击RagingStrikes) && HasEffect(Buffs.战斗之声BattleVoice) && (HasEffect(Buffs.RadiantFinale) || !LevelChecked(光明神的最终乐章RadiantFinale)) && gauge.SoulVoice >= 80)
                            return ApexArrow;
                    }
                    if (HasEffect(Buffs.HawksEye) || HasEffect(Buffs.纷乱箭))
                        return OriginalHook(StraightShot);

                    if (HasEffect(Buffs.ResonantArrowReady))
                        return 共鸣箭ResonantArrow;
                }
                return actionID;
            }
        }
    }
}