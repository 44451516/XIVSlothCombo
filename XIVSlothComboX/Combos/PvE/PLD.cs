﻿using Dalamud.Game.ClientState.JobGauge.Types;
using Dalamud.Game.ClientState.Statuses;
using System.Linq;
using XIVSlothComboX.Combos.PvE.Content;
using XIVSlothComboX.CustomComboNS;
using XIVSlothComboX.CustomComboNS.Functions;
using XIVSlothComboX.Data;
using XIVSlothComboX.Extensions;
using XIVSlothComboX.Services;

namespace XIVSlothComboX.Combos.PvE
{
    internal static class PLD
    {
        public const byte ClassID = 1;
        public const byte JobID = 19;

        public const float CooldownThreshold = 0.5f;

        public const uint 先锋剑FastBlade = 9,
            暴乱剑RiotBlade = 15,
            ShieldBash = 16,
            战女神之怒RageOfHalone = 21,
            厄运流转CircleOfScorn = 23,
            调停ShieldLob = 24,
            深奥之灵SpiritsWithin = 29,
            沥血剑GoringBlade = 3538,
            王权剑RoyalAuthority = 3539,
            全蚀斩TotalEclipse = 7381,
            安魂祈祷Requiescat = 7383,
            圣灵HolySpirit = 7384,
            日珥斩Prominence = 16457,
            圣环HolyCircle = 16458,
            大保健连击Confiteor = 16459,
            偿赎剑Expiacion = 25747,
            BladeOfFaith = 25748,
            BladeOfTruth = 25749,
            BladeOfValor = 25750,
            战逃反应FightOrFlight = 20,
            赎罪剑Atonement = 16460,
            调停Intervene = 16461,
            盾阵Sheltron = 3542;

        public static class Buffs
        {
            public const ushort Requiescat = 1368,
                忠义之剑SwordOath = 1902,
                FightOrFlight = 76,
                ConfiteorReady = 3019,
                DivineMight = 2673,
                HolySheltron = 2674,
                Sheltron = 1856;
        }

        public static class Debuffs
        {
            public const ushort BladeOfValor = 2721,
                GoringBlade = 725;
        }

        private static PLDGauge Gauge => CustomComboFunctions.GetJobGauge<PLDGauge>();

        public static class Config
        {
            public static UserInt PLD_Intervene_HoldCharges = new("PLDKeepInterveneCharges"),
                PLD_VariantCure = new("PLD_VariantCure"),
                PLD_RequiescatOption = new("PLD_RequiescatOption"),
                PLD_SpiritsWithinOption = new("PLD_SpiritsWithinOption"),
                PLD_SheltronOption = new("PLD_SheltronOption"),
                PLD_FOF_GCD = new("PLD_FOF_GCD");

            public static UserBool PLD_Intervene_MeleeOnly = new("PLD_Intervene_MeleeOnly");
        }

        internal class PLD_ST_SimpleMode : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.PLD_ST_SimpleMode;

            protected override uint Invoke(uint actionID, uint lastComboActionID, float comboTime, byte level)
            {
                if (actionID is 先锋剑FastBlade)
                {

                    if (IsEnabled(CustomComboPreset.PLD_Variant_Cure)
                        && IsEnabled(Variant.VariantCure)
                        && PlayerHealthPercentageHp() <= Config.PLD_VariantCure)
                        return Variant.VariantCure;

                    if (HasBattleTarget())
                    {

                        if (IsEnabled(CustomComboPreset.PLD_ST_AdvancedMode_HolySpirit)
                            && GetBuffRemainingTime(Buffs.DivineMight) > 0f
                            && GetBuffRemainingTime(Buffs.DivineMight) <= 2.5f
                            && GetResourceCost(圣灵HolySpirit) <= LocalPlayer.CurrentMp)
                            return 圣灵HolySpirit;

                        if (!InMeleeRange() && 圣灵HolySpirit.LevelChecked() && GetResourceCost(圣灵HolySpirit) <= LocalPlayer.CurrentMp && !IsMoving)
                            return 圣灵HolySpirit;


                        if (CanSpellWeave(actionID))
                        {
                            if (ActionReady(战逃反应FightOrFlight))
                            {

                                if (!ActionWatching.CombatActions.Exists(x => x == 战逃反应FightOrFlight))
                                {
                                    if (lastComboActionID == 暴乱剑RiotBlade)
                                    {
                                        return OriginalHook(战逃反应FightOrFlight);
                                    }
                                }

                                if (HasEffect(Buffs.DivineMight) && lastComboActionID == 暴乱剑RiotBlade)
                                {
                                    return OriginalHook(战逃反应FightOrFlight);
                                }

                                if (HasEffect(Buffs.DivineMight) && GetBuffStacks(Buffs.忠义之剑SwordOath) >= 2)
                                {
                                    return OriginalHook(战逃反应FightOrFlight);
                                }

                            }


                            if (IsEnabled(CustomComboPreset.PLD_ST_AdvancedMode_Requiescat)
                                && WasLastAbility(战逃反应FightOrFlight)
                                && ActionReady(安魂祈祷Requiescat))
                            {
                                return 安魂祈祷Requiescat;
                            }

                            if (IsEnabled(CustomComboPreset.PLD_ST_AdvancedMode_Requiescat)
                                && HasEffect(Buffs.FightOrFlight)
                                && ActionReady(安魂祈祷Requiescat))
                            {
                                return 安魂祈祷Requiescat;
                            }


                            if (!InMeleeRange() && 调停ShieldLob.LevelChecked())
                                return 调停ShieldLob;
                        }

                        if (CanWeave(actionID))
                        {
                            Status? sustainedDamage = FindTargetEffect(Variant.Debuffs.SustainedDamage);

                            if (IsEnabled(CustomComboPreset.PLD_Variant_SpiritDart)
                                && IsEnabled(Variant.VariantSpiritDart)
                                && (sustainedDamage is null || sustainedDamage?.RemainingTime <= 3))
                                return Variant.VariantSpiritDart;

                            if (IsEnabled(CustomComboPreset.PLD_Variant_Ultimatum)
                                && IsEnabled(Variant.VariantUltimatum)
                                && IsOffCooldown(Variant.VariantUltimatum))
                                return Variant.VariantUltimatum;
                        }



                        if (HasEffect(Buffs.FightOrFlight))
                        {
                            if (CanSpellWeavePlus(actionID) && !WasLastAbility(战逃反应FightOrFlight))
                            {
                                if (InMeleeRange())
                                {
                                    if (ActionReady(厄运流转CircleOfScorn))
                                        return 厄运流转CircleOfScorn;

                                    if (ActionReady(OriginalHook(深奥之灵SpiritsWithin)))
                                        return OriginalHook(深奥之灵SpiritsWithin);
                                }

                                if (OriginalHook(调停Intervene).LevelChecked()
                                    && !WasLastAction(调停Intervene)
                                    && GetRemainingCharges(调停Intervene) > Config.PLD_Intervene_HoldCharges
                                    && GetCooldownRemainingTime(厄运流转CircleOfScorn) > 3
                                    && GetCooldownRemainingTime(OriginalHook(厄运流转CircleOfScorn)) > 3
                                    && ((Config.PLD_Intervene_MeleeOnly && InMeleeRange()) || (!Config.PLD_Intervene_MeleeOnly)))
                                    return OriginalHook(调停Intervene);
                            }

                            if (ActionReady(沥血剑GoringBlade) && InMeleeRange())
                                return 沥血剑GoringBlade;

                            if (HasEffect(Buffs.Requiescat))
                            {
                                // Confiteor & Blades
                                if (HasEffect(Buffs.ConfiteorReady)
                                    || (BladeOfFaith.LevelChecked()
                                        && OriginalHook(大保健连击Confiteor) != 大保健连击Confiteor
                                        && GetResourceCost(大保健连击Confiteor) <= LocalPlayer.CurrentMp))
                                    return OriginalHook(大保健连击Confiteor);

                                // HS when Confiteor not unlocked or Confiteor used
                                if (GetResourceCost(圣灵HolySpirit) <= LocalPlayer.CurrentMp)
                                    return 圣灵HolySpirit;
                            }

                            if (HasEffect(Buffs.DivineMight) && GetResourceCost(圣灵HolySpirit) <= LocalPlayer.CurrentMp)
                                return 圣灵HolySpirit;

                            //学习了圣灵
                            if (lastComboActionID is 暴乱剑RiotBlade && 圣环HolyCircle.LevelChecked())
                            {
                                return OriginalHook(战女神之怒RageOfHalone);
                            }

                            if (HasEffect(Buffs.忠义之剑SwordOath))
                                return 赎罪剑Atonement;
                        }

                        // 非爆发期
                        if (CanSpellWeavePlus(actionID)
                            && !WasLastAction(战逃反应FightOrFlight)
                            && GetCooldownRemainingTime(战逃反应FightOrFlight) >= 15
                            && InMeleeRange())
                        {
                            if (ActionReady(厄运流转CircleOfScorn))
                                return 厄运流转CircleOfScorn;

                            if (ActionReady(OriginalHook(深奥之灵SpiritsWithin)))
                                return OriginalHook(深奥之灵SpiritsWithin);
                        }

                        // Goring on cooldown (burst features disabled)
                        if (IsEnabled(CustomComboPreset.PLD_ST_AdvancedMode_GoringBlade)
                            && ActionReady(沥血剑GoringBlade)
                            && IsNotEnabled(CustomComboPreset.PLD_ST_AdvancedMode_FoF))
                            return 沥血剑GoringBlade;

                        // Confiteor & Blades
                        if ((大保健连击Confiteor.LevelChecked() && HasEffect(Buffs.ConfiteorReady))
                            || (BladeOfFaith.LevelChecked()
                                && HasEffect(Buffs.Requiescat)
                                && OriginalHook(大保健连击Confiteor) != 大保健连击Confiteor
                                && GetResourceCost(大保健连击Confiteor) <= LocalPlayer.CurrentMp))
                            return OriginalHook(大保健连击Confiteor);

                        //Req HS 安魂祈祷下面直接使用圣灵
                        if (HasEffect(Buffs.Requiescat) && GetResourceCost(圣灵HolySpirit) <= LocalPlayer.CurrentMp)
                            return 圣灵HolySpirit;

                        if (GetCooldownRemainingTime(战逃反应FightOrFlight) >= 15 && RaidBuff.爆发期())
                        {
                            if (HasEffect(Buffs.DivineMight) && GetResourceCost(圣灵HolySpirit) <= LocalPlayer.CurrentMp)
                                return 圣灵HolySpirit;

                            if (HasEffect(Buffs.忠义之剑SwordOath))
                                return 赎罪剑Atonement;
                        }


                        // Base combo
                        if (comboTime > 0)
                        {
                            if (lastComboActionID is 先锋剑FastBlade && 暴乱剑RiotBlade.LevelChecked())
                                return 暴乱剑RiotBlade;

                            if (lastComboActionID is 暴乱剑RiotBlade && 战女神之怒RageOfHalone.LevelChecked())
                            {
                                if (HasEffect(Buffs.忠义之剑SwordOath) && InMeleeRange())
                                    return 赎罪剑Atonement;

                                if ((HasEffect(Buffs.DivineMight) && GetResourceCost(圣灵HolySpirit) <= LocalPlayer.CurrentMp))
                                {
                                    return 圣灵HolySpirit;
                                }

                                return OriginalHook(战女神之怒RageOfHalone);
                            }
                        }

                    }
                }

                return actionID;
            }
        }

        internal class PLD_AoE_SimpleMode : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.PLD_AoE_SimpleMode;

            protected override uint Invoke(uint actionID, uint lastComboActionID, float comboTime, byte level)
            {
                if (actionID is 全蚀斩TotalEclipse)
                {
                    if (IsEnabled(CustomComboPreset.PLD_Variant_Cure)
                        && IsEnabled(Variant.VariantCure)
                        && PlayerHealthPercentageHp() <= Config.PLD_VariantCure)
                        return Variant.VariantCure;

                    if (CanWeave(actionID))
                    {
                        Status? sustainedDamage = FindTargetEffect(Variant.Debuffs.SustainedDamage);

                        if (IsEnabled(CustomComboPreset.PLD_Variant_SpiritDart)
                            && IsEnabled(Variant.VariantSpiritDart)
                            && (sustainedDamage is null || sustainedDamage?.RemainingTime <= 3))
                            return Variant.VariantSpiritDart;

                        if (IsEnabled(CustomComboPreset.PLD_Variant_Ultimatum)
                            && IsEnabled(Variant.VariantUltimatum)
                            && IsOffCooldown(Variant.VariantUltimatum))
                            return Variant.VariantUltimatum;
                    }

                    if (ActionReady(安魂祈祷Requiescat))
                    {
                        if (WasLastAbility(战逃反应FightOrFlight))
                        {
                            return 安魂祈祷Requiescat;
                        }

                        if (HasEffect(Buffs.FightOrFlight))
                        {
                            return 安魂祈祷Requiescat;
                        }
                    }


                    // Actions under FoF burst
                    if (HasEffect(Buffs.FightOrFlight))
                    {
                        if (CanWeave(actionID))
                        {
                            if (ActionReady(厄运流转CircleOfScorn))
                                return 厄运流转CircleOfScorn;

                            if (ActionReady(OriginalHook(深奥之灵SpiritsWithin)))
                                return OriginalHook(深奥之灵SpiritsWithin);
                        }

                        if (HasEffect(Buffs.Requiescat))
                        {
                            if ((HasEffect(Buffs.ConfiteorReady) || BladeOfFaith.LevelChecked())
                                && GetResourceCost(大保健连击Confiteor) <= LocalPlayer.CurrentMp)
                                return OriginalHook(大保健连击Confiteor);

                            if (圣环HolyCircle.LevelChecked() && GetResourceCost(圣环HolyCircle) <= LocalPlayer.CurrentMp)
                                return 圣环HolyCircle;

                            if (圣灵HolySpirit.LevelChecked() && GetResourceCost(圣灵HolySpirit) <= LocalPlayer.CurrentMp)
                                return 圣灵HolySpirit;
                        }

                        if (HasEffect(Buffs.DivineMight) && GetResourceCost(圣环HolyCircle) <= LocalPlayer.CurrentMp && 圣环HolyCircle.LevelChecked())
                            return 圣环HolyCircle;
                    }

                    if (comboTime > 0 && lastComboActionID is 全蚀斩TotalEclipse && 日珥斩Prominence.LevelChecked())
                        return 日珥斩Prominence;

                    if (CanSpellWeavePlus(actionID))
                    {
                        if (ActionReady(战逃反应FightOrFlight))
                            return 战逃反应FightOrFlight;

                        if (!WasLastAbility(战逃反应FightOrFlight) && IsOnCooldown(战逃反应FightOrFlight))
                        {
                            if (ActionReady(厄运流转CircleOfScorn))
                                return 厄运流转CircleOfScorn;

                            if (ActionReady(OriginalHook(深奥之灵SpiritsWithin)))
                                return OriginalHook(深奥之灵SpiritsWithin);
                        }
                    }

                    if ((HasEffect(Buffs.DivineMight) || HasEffect(Buffs.Requiescat))
                        && GetResourceCost(圣环HolyCircle) <= LocalPlayer.CurrentMp
                        && LevelChecked(圣环HolyCircle))
                        return 圣环HolyCircle;

                    return actionID;
                }

                return actionID;
            }
        }

        internal class PLD_ST_AdvancedMode : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.PLD_ST_AdvancedMode;

            protected override uint Invoke(uint actionID, uint lastComboActionID, float comboTime, byte level)
            {
                if (actionID is 先锋剑FastBlade)
                {
                    if (IsEnabled(CustomComboPreset.PLD_Variant_Cure)
                        && IsEnabled(Variant.VariantCure)
                        && PlayerHealthPercentageHp() <= Config.PLD_VariantCure)
                        return Variant.VariantCure;

                    if (HasBattleTarget())
                    {

                        if (IsEnabled(CustomComboPreset.PLD_ST_AdvancedMode_HolySpirit)
                            && GetBuffRemainingTime(Buffs.DivineMight) > 0f
                            && GetBuffRemainingTime(Buffs.DivineMight) <= 2.5f
                            && HasEffect(Buffs.Requiescat)
                            && GetResourceCost(圣灵HolySpirit) <= LocalPlayer.CurrentMp)
                            return 圣灵HolySpirit;

                        if (!InMeleeRange() && !HasEffect(Buffs.Requiescat))
                        {
                            if (IsEnabled(CustomComboPreset.PLD_ST_AdvancedMode_ShieldLob)
                                && 调停ShieldLob.LevelChecked()
                                && ((圣灵HolySpirit.LevelChecked() && GetResourceCost(圣灵HolySpirit) > LocalPlayer.CurrentMp)
                                    || (!圣灵HolySpirit.LevelChecked())
                                    || IsMoving))
                                return 调停ShieldLob;
                        }


                        if (CanDelayedWeavePlus(actionID, 2.5f, 0.1f))
                        {
                            if (IsEnabled(CustomComboPreset.PLD_ST_AdvancedMode_FoF) && ActionReady(战逃反应FightOrFlight))
                            {
                                if (GetBuffRemainingTime(Buffs.DivineMight) >= 14 && lastComboActionID == 暴乱剑RiotBlade && comboTime >= 16)
                                {
                                    return OriginalHook(战逃反应FightOrFlight);
                                }

                                if (GetBuffRemainingTime(Buffs.DivineMight) >= 14 && GetBuffStacks(Buffs.忠义之剑SwordOath) >= 2)
                                {
                                    return OriginalHook(战逃反应FightOrFlight);
                                }


                                if (lastComboActionID == 暴乱剑RiotBlade && ActionWatching.CombatActions.Exists(x => x == 战逃反应FightOrFlight))
                                {
                                    return OriginalHook(战逃反应FightOrFlight);
                                }



                                var choice = GetOptionValue(Config.PLD_FOF_GCD);
                                if (!ActionWatching.CombatActions.Exists(x => x == 战逃反应FightOrFlight))
                                {
                                    switch (choice)
                                    {
                                        case 1:
                                        {
                                            if (lastComboActionID == 暴乱剑RiotBlade)
                                            {
                                                return OriginalHook(战逃反应FightOrFlight);
                                            }
                                            break;
                                        }

                                        case 2:
                                        {
                                            if (WasLastAction(王权剑RoyalAuthority))
                                            {
                                                return OriginalHook(战逃反应FightOrFlight);
                                            }
                                            break;
                                        }
                                        default:
                                        {
                                            if (WasLastAction(王权剑RoyalAuthority))
                                            {
                                                return OriginalHook(战逃反应FightOrFlight);
                                            }
                                            break;
                                        }
                                    }
                                }
                            }

                            if (ActionReady(安魂祈祷Requiescat))
                            {
                                if (IsEnabled(CustomComboPreset.PLD_ST_AdvancedMode_Requiescat) && WasLastAbility(战逃反应FightOrFlight))
                                {
                                    return 安魂祈祷Requiescat;
                                }

                                if (IsEnabled(CustomComboPreset.PLD_ST_AdvancedMode_Requiescat) && HasEffect(Buffs.FightOrFlight))
                                {
                                    return 安魂祈祷Requiescat;
                                }
                            }



                            Status? sustainedDamage = FindTargetEffect(Variant.Debuffs.SustainedDamage);

                            if (IsEnabled(CustomComboPreset.PLD_Variant_SpiritDart)
                                && IsEnabled(Variant.VariantSpiritDart)
                                && (sustainedDamage is null || sustainedDamage?.RemainingTime <= 3))
                                return Variant.VariantSpiritDart;

                            if (IsEnabled(CustomComboPreset.PLD_Variant_Ultimatum)
                                && IsEnabled(Variant.VariantUltimatum)
                                && IsOffCooldown(Variant.VariantUltimatum))
                                return Variant.VariantUltimatum;

                            // (Holy) Sheltron overcap protection
                            if (IsEnabled(CustomComboPreset.PLD_ST_AdvancedMode_Sheltron)
                                && 盾阵Sheltron.LevelChecked()
                                && !HasEffect(Buffs.Sheltron)
                                && !HasEffect(Buffs.HolySheltron)
                                && Gauge.OathGauge >= Config.PLD_SheltronOption)
                                return OriginalHook(盾阵Sheltron);
                        }

                        if (IsEnabled(CustomComboPreset.PLD_ST_AdvancedMode_FoF) && HasEffect(Buffs.FightOrFlight))
                        {
                            if (CanSpellWeavePlus(actionID) && !WasLastAbility(战逃反应FightOrFlight))
                            {
                                if (InMeleeRange() && GetCooldownRemainingTime(安魂祈祷Requiescat) > 40)
                                {
                                    if (IsEnabled(CustomComboPreset.PLD_ST_AdvancedMode_CircleOfScorn) && ActionReady(厄运流转CircleOfScorn))
                                        return 厄运流转CircleOfScorn;

                                    if (IsEnabled(CustomComboPreset.PLD_ST_AdvancedMode_SpiritsWithin)
                                        && ActionReady(OriginalHook(深奥之灵SpiritsWithin)))
                                        return OriginalHook(深奥之灵SpiritsWithin);
                                }

                                if (IsEnabled(CustomComboPreset.PLD_ST_AdvancedMode_Intervene)
                                    && OriginalHook(调停Intervene).LevelChecked()
                                    && !WasLastAction(调停Intervene)
                                    && GetRemainingCharges(调停Intervene) > Config.PLD_Intervene_HoldCharges
                                    && GetCooldownRemainingTime(厄运流转CircleOfScorn) > 3
                                    && GetCooldownRemainingTime(OriginalHook(厄运流转CircleOfScorn)) > 3
                                    && ((Config.PLD_Intervene_MeleeOnly && InMeleeRange()) || (!Config.PLD_Intervene_MeleeOnly)))
                                    return OriginalHook(调停Intervene);
                            }

                            if (IsEnabled(CustomComboPreset.PLD_ST_AdvancedMode_GoringBlade) && ActionReady(沥血剑GoringBlade) && InMeleeRange())
                                return 沥血剑GoringBlade;

                            if (HasEffect(Buffs.Requiescat))
                            {
                                // Confiteor & Blades
                                if ((IsEnabled(CustomComboPreset.PLD_ST_AdvancedMode_Confiteor) && HasEffect(Buffs.ConfiteorReady))
                                    || (IsEnabled(CustomComboPreset.PLD_ST_AdvancedMode_Blades)
                                        && BladeOfFaith.LevelChecked()
                                        && OriginalHook(大保健连击Confiteor) != 大保健连击Confiteor
                                        && GetResourceCost(大保健连击Confiteor) <= LocalPlayer.CurrentMp))
                                    return OriginalHook(大保健连击Confiteor);

                                // HS when Confiteor not unlocked or Confiteor used
                                if (IsEnabled(CustomComboPreset.PLD_ST_AdvancedMode_HolySpirit)
                                    && GetResourceCost(圣灵HolySpirit) <= LocalPlayer.CurrentMp)
                                    return 圣灵HolySpirit;
                            }

                            if (IsEnabled(CustomComboPreset.PLD_ST_AdvancedMode_HolySpirit)
                                && HasEffect(Buffs.DivineMight)
                                && GetResourceCost(圣灵HolySpirit) <= LocalPlayer.CurrentMp)
                                return 圣灵HolySpirit;

                            if (IsEnabled(CustomComboPreset.PLD_ST_AdvancedMode_Atonement) && HasEffect(Buffs.忠义之剑SwordOath))
                                return 赎罪剑Atonement;

                            //学习了圣灵
                            if (lastComboActionID is 暴乱剑RiotBlade && 圣环HolyCircle.LevelChecked())
                            {
                                return OriginalHook(战女神之怒RageOfHalone);
                            }

                            if (IsEnabled(CustomComboPreset.PLD_ST_AdvancedMode_Atonement) && HasEffect(Buffs.忠义之剑SwordOath))
                                return 赎罪剑Atonement;
                        }

                        if (GetCooldownRemainingTime(战逃反应FightOrFlight) >= 15 && RaidBuff.爆发期())
                        {
                            if (IsEnabled(CustomComboPreset.PLD_ST_AdvancedMode_HolySpirit)
                                && HasEffect(Buffs.DivineMight)
                                && GetResourceCost(圣灵HolySpirit) <= LocalPlayer.CurrentMp)
                                return 圣灵HolySpirit;

                            if (IsEnabled(CustomComboPreset.PLD_ST_AdvancedMode_Atonement) && HasEffect(Buffs.忠义之剑SwordOath))
                                return 赎罪剑Atonement;
                        }

                        // 没用启动战逃反应
                        if (CanSpellWeavePlus(actionID, 0.3f)
                            && (!WasLastAction(战逃反应FightOrFlight) && GetCooldownRemainingTime(战逃反应FightOrFlight) >= 15
                                || IsNotEnabled(CustomComboPreset.PLD_ST_AdvancedMode_FoF))
                            && InMeleeRange())
                        {
                            if (IsEnabled(CustomComboPreset.PLD_ST_AdvancedMode_CircleOfScorn) && ActionReady(厄运流转CircleOfScorn))
                                return 厄运流转CircleOfScorn;

                            if (IsEnabled(CustomComboPreset.PLD_ST_AdvancedMode_SpiritsWithin) && ActionReady(OriginalHook(深奥之灵SpiritsWithin)))
                                return OriginalHook(深奥之灵SpiritsWithin);
                        }

                        // Goring on cooldown (burst features disabled)
                        if (IsEnabled(CustomComboPreset.PLD_ST_AdvancedMode_GoringBlade)
                            && ActionReady(沥血剑GoringBlade)
                            && IsNotEnabled(CustomComboPreset.PLD_ST_AdvancedMode_FoF))
                            return 沥血剑GoringBlade;

                        // Confiteor & Blades
                        if (((IsEnabled(CustomComboPreset.PLD_ST_AdvancedMode_Confiteor)
                              && 大保健连击Confiteor.LevelChecked()
                              && HasEffect(Buffs.ConfiteorReady))
                             || (IsEnabled(CustomComboPreset.PLD_ST_AdvancedMode_Blades)
                                 && BladeOfFaith.LevelChecked()
                                 && HasEffect(Buffs.Requiescat)
                                 && OriginalHook(大保健连击Confiteor) != 大保健连击Confiteor
                                 && GetResourceCost(大保健连击Confiteor) <= LocalPlayer.CurrentMp)))
                            return OriginalHook(大保健连击Confiteor);

                        //Req HS 安魂祈祷下面直接使用圣灵
                        if (IsEnabled(CustomComboPreset.PLD_ST_AdvancedMode_HolySpirit)
                            && HasEffect(Buffs.Requiescat)
                            && GetResourceCost(圣灵HolySpirit) <= LocalPlayer.CurrentMp)
                            return 圣灵HolySpirit;



                        // Base combo
                        if (comboTime > 0)
                        {
                            if (lastComboActionID is 先锋剑FastBlade && 暴乱剑RiotBlade.LevelChecked())
                                return 暴乱剑RiotBlade;

                            if (lastComboActionID is 暴乱剑RiotBlade && 战女神之怒RageOfHalone.LevelChecked())
                            {
                                if (IsEnabled(CustomComboPreset.PLD_ST_AdvancedMode_Atonement) && HasEffect(Buffs.忠义之剑SwordOath) && InMeleeRange())
                                    return 赎罪剑Atonement;

                                if ((IsEnabled(CustomComboPreset.PLD_ST_AdvancedMode_HolySpirit)
                                     && HasEffect(Buffs.DivineMight)
                                     && GetResourceCost(圣灵HolySpirit) <= LocalPlayer.CurrentMp))
                                {
                                    return 圣灵HolySpirit;
                                }

                                return OriginalHook(战女神之怒RageOfHalone);
                            }
                        }
                    }
                }

                return actionID;
            }
        }

        internal class PLD_AoE_AdvancedMode : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.PLD_AoE_AdvancedMode;

            protected override uint Invoke(uint actionID, uint lastComboActionID, float comboTime, byte level)
            {
                if (actionID is 全蚀斩TotalEclipse)
                {
                    if (IsEnabled(CustomComboPreset.PLD_Variant_Cure)
                        && IsEnabled(Variant.VariantCure)
                        && PlayerHealthPercentageHp() <= Config.PLD_VariantCure)
                        return Variant.VariantCure;

                    if (CanSpellWeavePlus(actionID))
                    {
                        Status? sustainedDamage = FindTargetEffect(Variant.Debuffs.SustainedDamage);

                        if (IsEnabled(CustomComboPreset.PLD_Variant_SpiritDart)
                            && IsEnabled(Variant.VariantSpiritDart)
                            && (sustainedDamage is null || sustainedDamage?.RemainingTime <= 3))
                            return Variant.VariantSpiritDart;

                        if (IsEnabled(CustomComboPreset.PLD_Variant_Ultimatum)
                            && IsEnabled(Variant.VariantUltimatum)
                            && IsOffCooldown(Variant.VariantUltimatum))
                            return Variant.VariantUltimatum;

                        // (Holy) Sheltron overcap protection
                        if (IsEnabled(CustomComboPreset.PLD_AoE_AdvancedMode_Sheltron)
                            && 盾阵Sheltron.LevelChecked()
                            && !HasEffect(Buffs.Sheltron)
                            && !HasEffect(Buffs.HolySheltron)
                            && Gauge.OathGauge >= Config.PLD_SheltronOption)
                            return OriginalHook(盾阵Sheltron);
                    }

                    // Requiescat inside burst (checking for FoF buff causes a late weave and can misalign over long fights with some ping)

                    if (ActionReady(安魂祈祷Requiescat))
                    {
                        if (IsEnabled(CustomComboPreset.PLD_AoE_AdvancedMode_Requiescat) && WasLastAction(战逃反应FightOrFlight))
                        {
                            return 安魂祈祷Requiescat;
                        }

                        if (IsEnabled(CustomComboPreset.PLD_AoE_AdvancedMode_Requiescat) && HasEffect(Buffs.FightOrFlight))
                        {
                            return 安魂祈祷Requiescat;
                        }
                    }



                    // Actions under FoF burst
                    if (IsEnabled(CustomComboPreset.PLD_AoE_AdvancedMode_FoF) && HasEffect(Buffs.FightOrFlight))
                    {
                        if (CanSpellWeavePlus(actionID) && !WasLastAbility(战逃反应FightOrFlight))
                        {
                            if (IsEnabled(CustomComboPreset.PLD_AoE_AdvancedMode_CircleOfScorn) && ActionReady(厄运流转CircleOfScorn))
                                return 厄运流转CircleOfScorn;

                            if (IsEnabled(CustomComboPreset.PLD_AoE_AdvancedMode_SpiritsWithin) && ActionReady(OriginalHook(深奥之灵SpiritsWithin)))
                                return OriginalHook(深奥之灵SpiritsWithin);
                        }

                        if (HasEffect(Buffs.Requiescat))
                        {
                            // Confiteor & Blades
                            if ((IsEnabled(CustomComboPreset.PLD_AoE_AdvancedMode_Confiteor) && HasEffect(Buffs.ConfiteorReady))
                                || (IsEnabled(CustomComboPreset.PLD_AoE_AdvancedMode_Blades)
                                    && BladeOfFaith.LevelChecked()
                                    && OriginalHook(大保健连击Confiteor) != 大保健连击Confiteor
                                    && GetResourceCost(OriginalHook(大保健连击Confiteor)) <= LocalPlayer.CurrentMp))
                                return OriginalHook(大保健连击Confiteor);

                            // HC when Confiteor not unlocked
                            if (IsEnabled(CustomComboPreset.PLD_AoE_AdvancedMode_HolyCircle)
                                && GetResourceCost(圣环HolyCircle) <= LocalPlayer.CurrentMp
                                && LevelChecked(圣环HolyCircle))
                                return 圣环HolyCircle;

                        }

                        // HC under DM/Req
                        if (IsEnabled(CustomComboPreset.PLD_AoE_AdvancedMode_HolyCircle)
                            && (HasEffect(Buffs.DivineMight) || HasEffect(Buffs.Requiescat))
                            && GetResourceCost(圣环HolyCircle) <= LocalPlayer.CurrentMp
                            && 圣环HolyCircle.LevelChecked())
                            return 圣环HolyCircle;
                    }

                    if (comboTime > 0 && lastComboActionID is 全蚀斩TotalEclipse && 日珥斩Prominence.LevelChecked())
                        return 日珥斩Prominence;

                    if (CanSpellWeavePlus(actionID))
                    {
                        // FoF (Starts burst)
                        if (IsEnabled(CustomComboPreset.PLD_AoE_AdvancedMode_FoF) && ActionReady(战逃反应FightOrFlight))
                            return 战逃反应FightOrFlight;

                        // Usage outside of burst (desync for Req, 30s windows for CoS/SW)
                        if ((!WasLastAction(战逃反应FightOrFlight) && GetCooldownRemainingTime(战逃反应FightOrFlight) >= 15
                             || IsNotEnabled(CustomComboPreset.PLD_AoE_AdvancedMode_FoF))
                            && !ActionWatching.WasLast2ActionsAbilities())
                        {
                            if (IsEnabled(CustomComboPreset.PLD_AoE_AdvancedMode_Requiescat) && ActionReady(安魂祈祷Requiescat))
                                return 安魂祈祷Requiescat;

                            if (IsEnabled(CustomComboPreset.PLD_AoE_AdvancedMode_CircleOfScorn) && ActionReady(厄运流转CircleOfScorn))
                                return 厄运流转CircleOfScorn;

                            if (IsEnabled(CustomComboPreset.PLD_AoE_AdvancedMode_SpiritsWithin) && ActionReady(OriginalHook(深奥之灵SpiritsWithin)))
                                return OriginalHook(深奥之灵SpiritsWithin);
                        }
                    }

                    // Confiteor & Blades
                    if (((IsEnabled(CustomComboPreset.PLD_AoE_AdvancedMode_Confiteor)
                          && 大保健连击Confiteor.LevelChecked()
                          && HasEffect(Buffs.ConfiteorReady))
                         || (IsEnabled(CustomComboPreset.PLD_AoE_AdvancedMode_Blades)
                             && BladeOfFaith.LevelChecked()
                             && HasEffect(Buffs.Requiescat)
                             && OriginalHook(大保健连击Confiteor) != 大保健连击Confiteor
                             && GetResourceCost(OriginalHook(大保健连击Confiteor)) <= LocalPlayer.CurrentMp))
                        && IsNotEnabled(CustomComboPreset.PLD_AoE_AdvancedMode_FoF))
                        return OriginalHook(大保健连击Confiteor);

                    // HS under DM (outside of burst)
                    if (IsEnabled(CustomComboPreset.PLD_AoE_AdvancedMode_HolyCircle)
                        && HasEffect(Buffs.DivineMight)
                        && GetResourceCost(圣环HolyCircle) <= LocalPlayer.CurrentMp
                        && LevelChecked(圣环HolyCircle))
                        return 圣环HolyCircle;

                    return actionID;
                }

                return actionID;
            }
        }

        internal class PLD_Requiescat_Confiteor : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.PLD_Requiescat_Options;

            protected override uint Invoke(uint actionID, uint lastComboActionID, float comboTime, byte level)
            {
                if (actionID is 安魂祈祷Requiescat)
                {
                    int choice = Config.PLD_RequiescatOption;

                    if ((choice is 1 || choice is 3)
                        && HasEffect(Buffs.ConfiteorReady)
                        && 大保健连击Confiteor.LevelChecked()
                        && GetResourceCost(大保健连击Confiteor) <= LocalPlayer.CurrentMp)
                        return OriginalHook(大保健连击Confiteor);

                    if (HasEffect(Buffs.Requiescat))
                    {
                        if ((choice is 2 || choice is 3)
                            && OriginalHook(大保健连击Confiteor) != 大保健连击Confiteor
                            && BladeOfFaith.LevelChecked()
                            && GetResourceCost(大保健连击Confiteor) <= LocalPlayer.CurrentMp)
                            return OriginalHook(大保健连击Confiteor);

                        if (choice is 4 && 圣灵HolySpirit.LevelChecked() && GetResourceCost(圣灵HolySpirit) <= LocalPlayer.CurrentMp)
                            return 圣灵HolySpirit;

                        if (choice is 5 && 圣环HolyCircle.LevelChecked() && GetResourceCost(圣环HolyCircle) <= LocalPlayer.CurrentMp)
                            return 圣环HolyCircle;
                    }
                }

                return actionID;
            }
        }

        internal class PLD_CircleOfScorn : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.PLD_SpiritsWithin;

            protected override uint Invoke(uint actionID, uint lastComboActionID, float comboTime, byte level)
            {
                if ((actionID == 深奥之灵SpiritsWithin || actionID == 偿赎剑Expiacion) && ActionReady(厄运流转CircleOfScorn))
                {
                    if (IsOffCooldown(OriginalHook(深奥之灵SpiritsWithin)))
                    {
                        int choice = Config.PLD_SpiritsWithinOption;

                        switch (choice)
                        {
                            case 1: return 厄运流转CircleOfScorn;
                            case 2: return OriginalHook(深奥之灵SpiritsWithin);
                        }
                    }

                    return 厄运流转CircleOfScorn;
                }

                return actionID;
            }
        }
    }
}
