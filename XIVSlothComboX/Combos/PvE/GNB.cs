using System.Linq;
using Dalamud.Game.ClientState.JobGauge.Types;
using Dalamud.Game.ClientState.Statuses;
using XIVSlothComboX.Combos.PvE.Content;
using XIVSlothComboX.Core;
using XIVSlothComboX.CustomComboNS;
using XIVSlothComboX.CustomComboNS.Functions;
using XIVSlothComboX.Data;
using XIVSlothComboX.Extensions;
using XIVSlothComboX.Services;

namespace XIVSlothComboX.Combos.PvE
{
    internal static class GNB
    {
        public const byte JobID = 37;

        public static int MaxCartridges(byte level) => level >= 88 ? 3 : 2;


        public static bool 是否使用爆发 = false;

        public const uint 利刃斩KeenEdge = 16137,
            无情NoMercy = 16138,
            残暴弹BrutalShell = 16139,
            恶魔切DemonSlice = 16141,
            迅连斩SolidBarrel = 16145,
            烈牙GnashingFang = 16146,
            SavageClaw = 16147,
            恶魔杀DemonSlaughter = 16149,
            WickedTalon = 16150,
            音速破SonicBreak = 16153,
            续剑Continuation = 16155,
            JugularRip = 16156,
            AbdomenTear = 16157,
            EyeGouge = 16158,
            弓形冲波BowShock = 16159,
            HeartOfLight = 16160,
            爆发击BurstStrike = 16162,
            命运之环FatedCircle = 16163,
            Aurora = 16151,
            倍攻DoubleDown = 25760,
            爆破领域DangerZone = 16144,
            BlastingZone = 16165,
            血壤Bloodfest = 16164,
            超高速Hypervelocity = 25759,
            粗分斩RoughDivide = 16154,
            闪雷弹LightningShot = 16143,
            师心连1FatedBrand = 39636,
            师心连2ReignOfBeasts = 39637,
            师心连3NobleBlood = 39638;

        public static class Buffs
        {
            public const ushort NoMercy = 1831,
                Aurora = 1835,
                //撕喉预备
                ReadyToRip = 1842,
                ReadyToTear = 1843,
                ReadyToGouge = 1844,
                ReadyToBlast = 2686,
                ReadyToReign = 3840;
        }

        public static class Debuffs
        {
            public const ushort BowShock = 1838,
                SonicBreak = 1837;
        }

        public static class Config
        {
            public const string GNB_SkS = "GNB_SkS",
                GNB_START_GCD = "GNB_START_GCD",
                GNB_RoughDivide_HeldCharges = "GNB_RoughDivide_HeldCharges",
                GNB_VariantCure = "GNB_VariantCure";
        }


        internal class GNB_ST_Custom : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.GNB_Advanced_CustomMode;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID is GNB.残暴弹BrutalShell)
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


        internal class GNB_ST_MainCombo : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.GNB_ST_MainCombo;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID is 利刃斩KeenEdge)
                {
                    if (ActionWatching.CombatActions.Count <= 10 && !InCombat())
                    {
                        是否使用爆发 = false;
                    }

                    if (IsEnabled(CustomComboPreset.GNB_START_GCD_直接爆发))
                    {
                        是否使用爆发 = true;
                    }

                    var gauge = GetJobGauge<GNBGauge>();
                    var roughDivideChargesRemaining = PluginConfiguration.GetCustomIntValue(Config.GNB_RoughDivide_HeldCharges);


                    if (IsEnabled(CustomComboPreset.GNB_Variant_Cure)
                        && IsEnabled(Variant.VariantCure)
                        && PlayerHealthPercentageHp() <= GetOptionValue(Config.GNB_VariantCure))
                        return Variant.VariantCure;

                    if (IsEnabled(CustomComboPreset.GNB_ST_RangedUptime) && !InMeleeRange() && LevelChecked(闪雷弹LightningShot) && HasBattleTarget())
                        return 闪雷弹LightningShot;


                    if (IsEnabled(CustomComboPreset.GNB_ST_MainCombo_CooldownsGroup)
                        && IsEnabled(CustomComboPreset.GNB_ST_NoMercy)
                        && ActionReady(无情NoMercy))
                    {
                        if (LevelChecked(爆发击BurstStrike))
                        {
                            if (CanWeave(actionID))
                            {
                                //低等级循环
                                if (MaxCartridges(level) == 2)
                                {
                                    if (CanDelayedWeave(actionID, 1.25, 0.2))
                                    {
                                        if (lastComboMove == (残暴弹BrutalShell))
                                        {
                                            return OriginalHook(无情NoMercy);
                                        }

                                        if (gauge.Ammo >= 1 || (IsEnabled(CustomComboPreset.GNB_ST_Bloodfest) && 血壤Bloodfest.ActionReady()))
                                        {
                                            // 无情好了就用
                                            if (CanDelayedWeave(actionID, 1.25, 0.2)
                                                && ActionWatching.CombatActions.Exists(x
                                                    => x == 无情NoMercy)) // Check RA has been used for opener exception
                                                return 无情NoMercy;
                                        }
                                    }
                                }

                                if (MaxCartridges(level) == 3)
                                {
                                    //修改为3GCD起手
                                    var choice = GetOptionValue(Config.GNB_START_GCD);
                                    switch (choice)
                                    {
                                        case 1:
                                        {
                                            if (IsEnabled(CustomComboPreset.GNB_ST_Bloodfest)
                                                && lastComboMove == (利刃斩KeenEdge)
                                                && gauge.Ammo == 0
                                                && !(HasEffect(Buffs.ReadyToRip)
                                                     || HasEffect(Buffs.ReadyToTear)
                                                     || HasEffect(Buffs.ReadyToGouge)
                                                     || HasEffect(Buffs.ReadyToBlast))
                                                && 血壤Bloodfest.ActionReady())
                                            {
                                                return 血壤Bloodfest;
                                            }


                                            if (gauge.Ammo >= 2 && CanDelayedWeave(actionID, 1.25, 0.2))
                                            {
                                                return 无情NoMercy;
                                            }

                                            if (WasLastAction(血壤Bloodfest)
                                                && CanDelayedWeave(actionID, 1.25, 0.2)) // Check RA has been used for opener exception
                                                return 无情NoMercy;


                                            break;
                                        }

                                        case 2:
                                        {
                                            if (IsEnabled(CustomComboPreset.GNB_ST_Bloodfest)
                                                && !(HasEffect(Buffs.ReadyToRip)
                                                     || HasEffect(Buffs.ReadyToTear)
                                                     || HasEffect(Buffs.ReadyToGouge)
                                                     || HasEffect(Buffs.ReadyToBlast))
                                                && lastComboMove == (残暴弹BrutalShell)
                                                && gauge.Ammo == 0
                                                && 血壤Bloodfest.ActionReady())
                                            {
                                                return 血壤Bloodfest;
                                            }


                                            if (gauge.Ammo >= 2 && CanDelayedWeave(actionID, 1.25, 0.2))
                                            {
                                                return 无情NoMercy;
                                            }

                                            if (WasLastAction(血壤Bloodfest)
                                                && CanDelayedWeave(actionID, 1.25, 0.2)) // Check RA has been used for opener exception
                                                return 无情NoMercy;

                                            break;
                                        }

                                        case 3:
                                        {
                                            if (CanDelayedWeave(actionID, 1.25, 0.2) && WasLastAction(迅连斩SolidBarrel))
                                            {
                                                return 无情NoMercy;
                                            }

                                            break;
                                        }

                                        default:
                                        {
                                            if (lastComboMove == (利刃斩KeenEdge))
                                            {
                                                if (!(HasEffect(Buffs.ReadyToRip)
                                                      || HasEffect(Buffs.ReadyToTear)
                                                      || HasEffect(Buffs.ReadyToGouge)
                                                      || HasEffect(Buffs.ReadyToBlast))
                                                    && IsEnabled(CustomComboPreset.GNB_ST_Bloodfest)
                                                    && gauge.Ammo == 0
                                                    && 血壤Bloodfest.ActionReady())
                                                {
                                                    return 血壤Bloodfest;
                                                }

                                                if (gauge.Ammo >= 2 && CanDelayedWeave(actionID, 1.25, 0.2))
                                                {
                                                    return 无情NoMercy;
                                                }
                                            }

                                            break;
                                        }
                                    }

                                    if (gauge.Ammo >= 3
                                        || (MaxCartridges(level) >= 2 && lastComboMove == 残暴弹BrutalShell)
                                        || (IsEnabled(CustomComboPreset.GNB_ST_Bloodfest) && 血壤Bloodfest.ActionReady()))
                                    {
                                        // 无情好了就用
                                        if (CanDelayedWeave(actionID, 1.25, 0.2)
                                            && ActionWatching.CombatActions.Exists(x
                                                => x == 无情NoMercy)) // Check RA has been used for opener exception
                                            return 无情NoMercy;
                                    }
                                }
                            }
                        }
                    }

                    //oGCDs
                    if (CanSpellWeavePlus(actionID))
                    {
                        Status? sustainedDamage = FindTargetEffect(Variant.Debuffs.SustainedDamage);
                        if (IsEnabled(CustomComboPreset.GNB_Variant_SpiritDart)
                            && IsEnabled(Variant.VariantSpiritDart)
                            && (sustainedDamage is null || sustainedDamage?.RemainingTime <= 3))
                            return Variant.VariantSpiritDart;

                        if (IsEnabled(CustomComboPreset.GNB_Variant_Ultimatum)
                            && IsEnabled(Variant.VariantUltimatum)
                            && IsOffCooldown(Variant.VariantUltimatum))
                            return Variant.VariantUltimatum;

                        if (IsEnabled(CustomComboPreset.GNB_ST_MainCombo_CooldownsGroup))
                        {
                            if (IsEnabled(CustomComboPreset.GNB_ST_Bloodfest)
                                && !(HasEffect(Buffs.ReadyToRip)
                                     || HasEffect(Buffs.ReadyToTear)
                                     || HasEffect(Buffs.ReadyToGouge)
                                     || HasEffect(Buffs.ReadyToBlast))
                                && ActionReady(血壤Bloodfest)
                                && gauge.Ammo is 0
                                && HasEffect(Buffs.NoMercy))
                            {
                                return 血壤Bloodfest;
                            }


                            if (IsEnabled(CustomComboPreset.GNB_ST_BlastingZone) && ActionReady(爆破领域DangerZone))
                            {
                                //Blasting Zone outside of NM
                                if (!HasEffect(Buffs.NoMercy)
                                    && ((GetCooldownRemainingTime(无情NoMercy) > 17 && GetCooldownRemainingTime(无情NoMercy) < 30)
                                        || !LevelChecked(烈牙GnashingFang)))
                                {
                                    return OriginalHook(爆破领域DangerZone);
                                }
                            }

                            //Continuation
                            if (IsEnabled(CustomComboPreset.GNB_ST_Gnashing)
                                && LevelChecked(续剑Continuation)
                                && (HasEffect(Buffs.ReadyToRip) || HasEffect(Buffs.ReadyToTear) || HasEffect(Buffs.ReadyToGouge)))
                            {
                                return OriginalHook(续剑Continuation);
                            }

                            //60s weaves
                            if (HasEffect(Buffs.NoMercy) && 是否使用爆发)
                            {
                                //Post DD
                                if (!WasLastAction(无情NoMercy) && 可否使用能力技爆发())
                                {
                                    if (IsEnabled(CustomComboPreset.GNB_ST_BlastingZone) && ActionReady(爆破领域DangerZone))
                                        return OriginalHook(爆破领域DangerZone);

                                    if (IsEnabled(CustomComboPreset.GNB_ST_BowShock) && ActionReady(弓形冲波BowShock))
                                        return 弓形冲波BowShock;
                                }

                                //Pre DD
                                if (!WasLastAction(无情NoMercy) && !LevelChecked(倍攻DoubleDown) && 可否使用能力技爆发())
                                {
                                    if (IsEnabled(CustomComboPreset.GNB_ST_BowShock) && ActionReady(弓形冲波BowShock))
                                        return 弓形冲波BowShock;

                                    if (IsEnabled(CustomComboPreset.GNB_ST_BlastingZone) && ActionReady(爆破领域DangerZone))
                                    {
                                        // Service.ChatGui.Print($"爆破领域DangerZone5");
                                        return OriginalHook(爆破领域DangerZone);
                                    }
                                }
                            }
                        }
                    }


                    if (IsEnabled(CustomComboPreset.GNB_ST_MainCombo_CooldownsGroup))
                    {
                        // 60s window features
                        if (HasEffect(Buffs.NoMercy))
                        {
                            //低等级循环
                            if (!LevelChecked(倍攻DoubleDown) && 是否使用爆发)
                            {
                                if (主连击_使用子弹连(gauge, level, lastComboMove))
                                {
                                    return OriginalHook(烈牙GnashingFang);
                                }
                            }

                            if (LevelChecked(倍攻DoubleDown) && 是否使用爆发)
                            {
                                if (LevelChecked(倍攻DoubleDown)
                                    && IsEnabled(CustomComboPreset.GNB_ST_DoubleDown)
                                    && gauge.Ammo == 2
                                    && 倍攻是否准备就绪()
                                    && ActionReady(烈牙GnashingFang)
                                    && ActionReady(血壤Bloodfest))
                                {
                                    return 倍攻DoubleDown;
                                }


                                int 先打什么选项 = PluginConfiguration.GetCustomIntValue(Config.GNB_SkS);
                                switch (先打什么选项)
                                {
                                    case 1:
                                    {
                                        if (主连击_使用子弹连(gauge, level, lastComboMove))
                                        {
                                            return OriginalHook(烈牙GnashingFang);
                                        }

                                        if (IsEnabled(CustomComboPreset.GNB_ST_DoubleDown) && 倍攻是否准备就绪() && gauge.Ammo >= 2)
                                        {
                                            return 倍攻DoubleDown;
                                        }

                                        if (IsEnabled(CustomComboPreset.GNB_ST_SonicBreak) && ActionReady(音速破SonicBreak))
                                        {
                                            return 音速破SonicBreak;
                                        }


                                        if (使用师心连(lastComboMove))
                                        {
                                            return OriginalHook(血壤Bloodfest);
                                        }


                                        break;
                                    }

                                    case 2:
                                    {
                                        if (主连击_使用子弹连(gauge, level, lastComboMove))
                                        {
                                            return OriginalHook(烈牙GnashingFang);
                                        }

                                        if (IsEnabled(CustomComboPreset.GNB_ST_SonicBreak) && ActionReady(音速破SonicBreak))
                                        {
                                            return 音速破SonicBreak;
                                        }

                                        if (IsEnabled(CustomComboPreset.GNB_ST_DoubleDown) && 倍攻是否准备就绪() && gauge.Ammo >= 2)
                                        {
                                            return 倍攻DoubleDown;
                                        }

                                        if (使用师心连(lastComboMove))
                                        {
                                            return OriginalHook(血壤Bloodfest);
                                        }


                                        break;
                                    }


                                    case 3:
                                    {
                                        if (IsEnabled(CustomComboPreset.GNB_ST_SonicBreak) && ActionReady(音速破SonicBreak))
                                        {
                                            return 音速破SonicBreak;
                                        }

                                        if (主连击_使用子弹连(gauge, level, lastComboMove))
                                        {
                                            return OriginalHook(烈牙GnashingFang);
                                        }

                                        if (IsEnabled(CustomComboPreset.GNB_ST_DoubleDown) && 倍攻是否准备就绪() && gauge.Ammo >= 2)
                                        {
                                            return 倍攻DoubleDown;
                                        }

                                        if (使用师心连(lastComboMove))
                                        {
                                            return OriginalHook(血壤Bloodfest);
                                        }

                                        break;
                                    }


                                    case 4:
                                    {
                                        if (IsEnabled(CustomComboPreset.GNB_ST_SonicBreak) && ActionReady(音速破SonicBreak))
                                        {
                                            return 音速破SonicBreak;
                                        }

                                        if (IsEnabled(CustomComboPreset.GNB_ST_DoubleDown) && 倍攻是否准备就绪() && gauge.Ammo >= 2)
                                        {
                                            return 倍攻DoubleDown;
                                        }

                                        if (主连击_使用子弹连(gauge, level, lastComboMove))
                                        {
                                            return OriginalHook(烈牙GnashingFang);
                                        }

                                        if (使用师心连(lastComboMove))
                                        {
                                            return OriginalHook(血壤Bloodfest);
                                        }


                                        break;
                                    }


                                    case 5:
                                    {
                                        if (IsEnabled(CustomComboPreset.GNB_ST_DoubleDown) && 倍攻是否准备就绪() && gauge.Ammo >= 2)
                                        {
                                            return 倍攻DoubleDown;
                                        }

                                        if (IsEnabled(CustomComboPreset.GNB_ST_SonicBreak) && ActionReady(音速破SonicBreak))
                                        {
                                            return 音速破SonicBreak;
                                        }

                                        if (主连击_使用子弹连(gauge, level, lastComboMove))
                                        {
                                            return OriginalHook(烈牙GnashingFang);
                                        }

                                        if (使用师心连(lastComboMove))
                                        {
                                            return OriginalHook(血壤Bloodfest);
                                        }


                                        break;
                                    }


                                    case 6:
                                    {
                                        if (IsEnabled(CustomComboPreset.GNB_ST_DoubleDown) && 倍攻是否准备就绪() && gauge.Ammo >= 2)
                                        {
                                            return 倍攻DoubleDown;
                                        }

                                        if (主连击_使用子弹连(gauge, level, lastComboMove))
                                        {
                                            return OriginalHook(烈牙GnashingFang);
                                        }

                                        if (IsEnabled(CustomComboPreset.GNB_ST_SonicBreak) && ActionReady(音速破SonicBreak))
                                        {
                                            return 音速破SonicBreak;
                                        }

                                        if (使用师心连(lastComboMove))
                                        {
                                            return OriginalHook(血壤Bloodfest);
                                        }


                                        break;
                                    }
                                    default:
                                    {
                                        if (主连击_使用子弹连(gauge, level, lastComboMove))
                                        {
                                            return OriginalHook(烈牙GnashingFang);
                                        }

                                        if (IsEnabled(CustomComboPreset.GNB_ST_DoubleDown) && 倍攻是否准备就绪() && gauge.Ammo >= 2)
                                        {
                                            return 倍攻DoubleDown;
                                        }

                                        if (IsEnabled(CustomComboPreset.GNB_ST_SonicBreak) && ActionReady(音速破SonicBreak))
                                        {
                                            return 音速破SonicBreak;
                                        }

                                        if (使用师心连(lastComboMove))
                                        {
                                            return OriginalHook(血壤Bloodfest);
                                        }


                                        break;
                                    }
                                }


                                // if (gauge.AmmoComboStep is 1 or 2)
                                // {
                                //     return OriginalHook(烈牙GnashingFang);
                                // }
                            }
                        }
                        else
                        {
                            if (是否使用爆发)
                            {
                                if (主连击_使用子弹连(gauge, level, lastComboMove))
                                {
                                    return OriginalHook(烈牙GnashingFang);
                                }
                            }
                        }


                        if (!LevelChecked(倍攻DoubleDown))
                        {
                            if (IsEnabled(CustomComboPreset.GNB_ST_SonicBreak)
                                && ActionReady(音速破SonicBreak)
                                && !HasEffect(Buffs.ReadyToRip)
                                && IsOnCooldown(烈牙GnashingFang))
                                return 音速破SonicBreak;

                            //sub level 54 functionality
                            if (IsEnabled(CustomComboPreset.GNB_ST_BlastingZone) && ActionReady(爆破领域DangerZone) && !LevelChecked(音速破SonicBreak))
                            {
                                return OriginalHook(爆破领域DangerZone);
                            }
                        }
                    }

                    //Pre Gnashing Fang stuff
                    if (IsEnabled(CustomComboPreset.GNB_ST_Gnashing) && LevelChecked(烈牙GnashingFang))
                    {
                        if (gauge.Ammo > 0 && 子弹连是否准备就绪() && GetCooldownRemainingTime(无情NoMercy) > 17 && GetCooldownRemainingTime(无情NoMercy) < 35)
                        {
                            return 烈牙GnashingFang;
                        }
                    }

                    if (IsEnabled(CustomComboPreset.GNB_ST_BurstStrike) && IsEnabled(CustomComboPreset.GNB_ST_MainCombo_CooldownsGroup))
                    {
                        if (HasEffect(Buffs.NoMercy) && LevelChecked(爆发击BurstStrike))
                        {
                            if (LevelChecked(超高速Hypervelocity) && HasEffect(Buffs.ReadyToBlast))
                                return 超高速Hypervelocity;

                            if (gauge.AmmoComboStep == 0)
                            {
                                if (gauge.Ammo != 0
                                    && LevelChecked(倍攻DoubleDown)
                                    && GetCooldownRemainingTime(倍攻DoubleDown) > 20
                                    && GetCooldownRemainingTime(烈牙GnashingFang) > 10)
                                    return 爆发击BurstStrike;

                                if (gauge.Ammo != 0 && GetCooldownRemainingTime(烈牙GnashingFang) > 10 && GetCooldownRemainingTime(倍攻DoubleDown) > 40)
                                    return 爆发击BurstStrike;
                            }

                            if (LevelChecked(血壤Bloodfest)
                                && gauge.Ammo != 0
                                && GetCooldownRemainingTime(烈牙GnashingFang) > 10
                                && GetCooldownRemainingTime(血壤Bloodfest) <= GetCooldownRemainingTime(利刃斩KeenEdge))
                                return 爆发击BurstStrike;
                        }

                        //final check if Burst Strike is used right before No Mercy ends
                        if (LevelChecked(超高速Hypervelocity) && HasEffect(Buffs.ReadyToBlast))
                            return 超高速Hypervelocity;
                    }

                    if (gauge.AmmoComboStep is 1 or 2)
                    {
                        return OriginalHook(烈牙GnashingFang);
                    }


                    // Regular 1-2-3 combo with overcap feature
                    if (comboTime > 0)
                    {
                        if (lastComboMove == 残暴弹BrutalShell)
                        {
                            是否使用爆发 = true;
                        }

                        if (lastComboMove == 利刃斩KeenEdge && LevelChecked(残暴弹BrutalShell))
                            return 残暴弹BrutalShell;

                        if (lastComboMove == 残暴弹BrutalShell && LevelChecked(迅连斩SolidBarrel))
                        {
                            if (LevelChecked(超高速Hypervelocity) && HasEffect(Buffs.ReadyToBlast))
                                return 超高速Hypervelocity;

                            if (LevelChecked(爆发击BurstStrike) && gauge.Ammo == MaxCartridges(level))
                                return 爆发击BurstStrike;

                            return 迅连斩SolidBarrel;
                        }
                    }

                    return 利刃斩KeenEdge;
                }

                return actionID;
            }

      
        }

        private static bool 子弹连是否准备就绪()
        {
            if (!CustomComboFunctions.LevelChecked(烈牙GnashingFang))
            {
                return false;
            }

            if (CustomComboFunctions.GetCooldownRemainingTime(烈牙GnashingFang) <= 0
                || (CustomComboFunctions.GetCooldownRemainingTime(烈牙GnashingFang) <= CustomComboFunctions.GetCooldownRemainingTime(利刃斩KeenEdge)
                    && CustomComboFunctions.GetCooldownRemainingTime(利刃斩KeenEdge) < 1.5f))
            {
                return true;
            }

            return false;
        }


        private static bool 倍攻是否准备就绪()
        {
            if (!CustomComboFunctions.LevelChecked(倍攻DoubleDown))
            {
                return false;
            }

            if (CustomComboFunctions.GetTargetDistance() <= 5)
            {
                if (CustomComboFunctions.GetCooldownRemainingTime(倍攻DoubleDown) <= 0
                    || (CustomComboFunctions.GetCooldownRemainingTime(倍攻DoubleDown) <= CustomComboFunctions.GetCooldownRemainingTime(利刃斩KeenEdge)
                        && CustomComboFunctions.GetCooldownRemainingTime(利刃斩KeenEdge) < 1.5f))
                {
                    return true;
                }
            }


            return false;
        }

        private static bool 可否使用能力技爆发()
        {
            if (CustomComboFunctions.IsOnCooldown(烈牙GnashingFang)
                || CustomComboFunctions.IsOnCooldown(倍攻DoubleDown)
                || CustomComboFunctions.IsOnCooldown(音速破SonicBreak))
            {
                return true;
            }

            return false;
        }

        private static bool 主连击_使用子弹连(GNBGauge gauge, byte level, uint lastComboMove)
        {
            if (CustomComboFunctions.IsNotEnabled(CustomComboPreset.GNB_ST_Gnashing))
            {
                return false;
            }

            if (CustomComboFunctions.HasEffect(Buffs.NoMercy))
            {
                //70级循环
                if (MaxCartridges(level) == 2 && 子弹连是否准备就绪() && gauge.Ammo >= 1)
                {
                    return true;
                }

                if (gauge.Ammo == 1 && 子弹连是否准备就绪() && CustomComboFunctions.IsOffCooldown(血壤Bloodfest))
                {
                    return true;
                }

                if (gauge.Ammo == 1 && 子弹连是否准备就绪() && CustomComboFunctions.GetCooldownRemainingTime(倍攻DoubleDown) >= 40)
                {
                    return true;
                }


                if (gauge.Ammo == 3 && 子弹连是否准备就绪())
                {
                    return true;
                }
            }

            if (lastComboMove == 残暴弹BrutalShell && gauge.Ammo >= 2 && 子弹连是否准备就绪())
            {
                return true;
            }

            return false;
        }


        private static bool 子弹连_使用子弹连(GNBGauge gauge, byte level, uint lastComboMove)
        {
            if (gauge.Ammo >= 1 && 子弹连是否准备就绪())
            {
                return true;
            }

            return false;
        }
        
        
        private static bool 使用师心连(uint lastComboMove)
        {
            if (CustomComboFunctions.LevelChecked(师心连1FatedBrand) && CustomComboFunctions.HasEffect(Buffs.ReadyToReign))
            {
                return true;
            }
                
            if (lastComboMove is 师心连1FatedBrand or 师心连2ReignOfBeasts )
            {
                return true;
            }

            return false;
        }


        internal class GNB_GF_Continuation : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.GNB_GF_Continuation;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID == 烈牙GnashingFang)
                {
                    var gauge = GetJobGauge<GNBGauge>();

                    if (IsOffCooldown(无情NoMercy)
                        && CanDelayedWeave(迅连斩SolidBarrel)
                        && GetCooldownRemainingTime(烈牙GnashingFang) < 2.3
                        && IsEnabled(CustomComboPreset.GNB_GF_NoMercy))
                        return 无情NoMercy;

                    if (CanSpellWeavePlus(actionID))
                    {
                        if (IsEnabled(CustomComboPreset.GNB_GF_Cooldowns))
                        {
                            if (ActionReady(爆破领域DangerZone))
                            {
                                //Blasting Zone outside of NM
                                if (!HasEffect(Buffs.NoMercy)
                                    && ((IsOnCooldown(烈牙GnashingFang) && GetCooldownRemainingTime(无情NoMercy) > 17)
                                        || //Post Gnashing Fang
                                        !LevelChecked(烈牙GnashingFang))) //Pre Gnashing Fang
                                    return OriginalHook(爆破领域DangerZone);

                                //Stops DZ Drift
                                if (HasEffect(Buffs.NoMercy) && ((IsOnCooldown(音速破SonicBreak)) || (IsOnCooldown(倍攻DoubleDown))))
                                {
                                    return OriginalHook(爆破领域DangerZone);
                                }
                            }

                            //Continuation
                            if (LevelChecked(续剑Continuation)
                                && (HasEffect(Buffs.ReadyToRip) || HasEffect(Buffs.ReadyToTear) || HasEffect(Buffs.ReadyToGouge)))
                                return OriginalHook(续剑Continuation);

                            //60s weaves
                            if (HasEffect(Buffs.NoMercy))
                            {
                                //Post DD
                                if ((IsOnCooldown(倍攻DoubleDown)) || (IsOnCooldown(音速破SonicBreak)))
                                {
                                    if (ActionReady(爆破领域DangerZone))
                                    {
                                        return OriginalHook(爆破领域DangerZone);
                                    }

                                    if (ActionReady(弓形冲波BowShock))
                                    {
                                        return 弓形冲波BowShock;
                                    }
                                }

                                //Pre DD
                                if (IsOnCooldown(音速破SonicBreak) && !LevelChecked(倍攻DoubleDown))
                                {
                                    if (ActionReady(弓形冲波BowShock))
                                    {
                                        return 弓形冲波BowShock;
                                    }

                                    if (ActionReady(爆破领域DangerZone))
                                    {
                                        return OriginalHook(爆破领域DangerZone);
                                    }
                                }
                            }
                        }

                        if (LevelChecked(续剑Continuation)
                            && (HasEffect(Buffs.ReadyToRip) || HasEffect(Buffs.ReadyToTear) || HasEffect(Buffs.ReadyToGouge)))
                            return OriginalHook(续剑Continuation);
                    }

                    // 60s window features
                    if (GetCooldownRemainingTime(无情NoMercy) > 40 || HasEffect(Buffs.NoMercy))
                    {
                        if (LevelChecked(倍攻DoubleDown)
                            && IsEnabled(CustomComboPreset.GNB_ST_DoubleDown)
                            && gauge.Ammo == 2
                            && 倍攻是否准备就绪()
                            && ActionReady(烈牙GnashingFang)
                            && ActionReady(血壤Bloodfest))
                        {
                            return 倍攻DoubleDown;
                        }

                        if (LevelChecked(倍攻DoubleDown))
                        {
                            int 先打什么选项 = PluginConfiguration.GetCustomIntValue(Config.GNB_SkS);
                            switch (先打什么选项)
                            {
                                case 1:
                                {
                                    if (子弹连_使用子弹连(gauge, level, lastComboMove))
                                    {
                                        return OriginalHook(烈牙GnashingFang);
                                    }

                                    if (IsEnabled(CustomComboPreset.GNB_ST_DoubleDown) && 倍攻是否准备就绪() && gauge.Ammo >= 2)
                                    {
                                        return 倍攻DoubleDown;
                                    }

                                    if (IsEnabled(CustomComboPreset.GNB_ST_SonicBreak) && ActionReady(音速破SonicBreak))
                                    {
                                        return 音速破SonicBreak;
                                    }

                                    if (使用师心连(lastComboMove))
                                    {
                                        return OriginalHook(血壤Bloodfest);
                                    }

                                    break;
                                }

                                case 2:
                                {
                                    if (子弹连_使用子弹连(gauge, level, lastComboMove))
                                        return OriginalHook(烈牙GnashingFang);

                                    if (IsEnabled(CustomComboPreset.GNB_ST_SonicBreak) && ActionReady(音速破SonicBreak))
                                        return 音速破SonicBreak;

                                    if (IsEnabled(CustomComboPreset.GNB_ST_DoubleDown) && 倍攻是否准备就绪() && gauge.Ammo >= 2)
                                        return 倍攻DoubleDown;

                                    if (使用师心连(lastComboMove))
                                    {
                                        return OriginalHook(血壤Bloodfest);
                                    }


                                    break;
                                }


                                case 3:
                                {
                                    if (IsEnabled(CustomComboPreset.GNB_ST_SonicBreak) && ActionReady(音速破SonicBreak))
                                        return 音速破SonicBreak;

                                    if (子弹连_使用子弹连(gauge, level, lastComboMove))
                                        return OriginalHook(烈牙GnashingFang);

                                    if (IsEnabled(CustomComboPreset.GNB_ST_DoubleDown) && 倍攻是否准备就绪() && gauge.Ammo >= 2)
                                        return 倍攻DoubleDown;
                                    
                                    if (使用师心连(lastComboMove))
                                    {
                                        return OriginalHook(血壤Bloodfest);
                                    }


                                    break;
                                }


                                case 4:
                                {
                                    if (IsEnabled(CustomComboPreset.GNB_ST_SonicBreak) && ActionReady(音速破SonicBreak))
                                        return 音速破SonicBreak;

                                    if (IsEnabled(CustomComboPreset.GNB_ST_DoubleDown) && 倍攻是否准备就绪() && gauge.Ammo >= 2)
                                        return 倍攻DoubleDown;

                                    if (子弹连_使用子弹连(gauge, level, lastComboMove))
                                        return OriginalHook(烈牙GnashingFang);

                                    if (使用师心连(lastComboMove))
                                    {
                                        return OriginalHook(血壤Bloodfest);
                                    }


                                    break;
                                }


                                case 5:
                                {
                                    if (IsEnabled(CustomComboPreset.GNB_ST_DoubleDown) && 倍攻是否准备就绪() && gauge.Ammo >= 2)
                                        return 倍攻DoubleDown;

                                    if (IsEnabled(CustomComboPreset.GNB_ST_SonicBreak) && ActionReady(音速破SonicBreak))
                                        return 音速破SonicBreak;

                                    if (子弹连_使用子弹连(gauge, level, lastComboMove))
                                        return OriginalHook(烈牙GnashingFang);

                                    if (使用师心连(lastComboMove))
                                    {
                                        return OriginalHook(血壤Bloodfest);
                                    }


                                    break;
                                }


                                case 6:
                                {
                                    if (IsEnabled(CustomComboPreset.GNB_ST_DoubleDown) && 倍攻是否准备就绪() && gauge.Ammo >= 2)
                                        return 倍攻DoubleDown;

                                    if (子弹连_使用子弹连(gauge, level, lastComboMove))
                                        return 烈牙GnashingFang;

                                    if (IsEnabled(CustomComboPreset.GNB_ST_SonicBreak) && ActionReady(音速破SonicBreak))
                                        return 音速破SonicBreak;

                                    if (使用师心连(lastComboMove))
                                    {
                                        return OriginalHook(血壤Bloodfest);
                                    }


                                    break;
                                }

                                default:
                                {
                                    if (子弹连_使用子弹连(gauge, level, lastComboMove))
                                        return OriginalHook(烈牙GnashingFang);

                                    if (IsEnabled(CustomComboPreset.GNB_ST_DoubleDown) && 倍攻是否准备就绪() && gauge.Ammo >= 2)
                                        return 倍攻DoubleDown;

                                    if (IsEnabled(CustomComboPreset.GNB_ST_SonicBreak) && ActionReady(音速破SonicBreak))
                                        return 音速破SonicBreak;

                                    if (使用师心连(lastComboMove))
                                    {
                                        return OriginalHook(血壤Bloodfest);
                                    }


                                    break;
                                }
                            }
                        }


                        if (!LevelChecked(倍攻DoubleDown) && IsEnabled(CustomComboPreset.GNB_GF_Cooldowns) && CanSpellWeavePlus(actionID))
                        {
                            if (ActionReady(音速破SonicBreak) && !HasEffect(Buffs.ReadyToRip) && IsOnCooldown(烈牙GnashingFang))
                                return 音速破SonicBreak;

                            //sub level 54 functionality
                            if (ActionReady(爆破领域DangerZone) && !LevelChecked(音速破SonicBreak))
                            {
                                return OriginalHook(爆破领域DangerZone);
                            }
                        }
                    }

                    if ((gauge.AmmoComboStep == 0 && 子弹连是否准备就绪()))
                        return OriginalHook(烈牙GnashingFang);

                    if (IsEnabled(CustomComboPreset.GNB_GF_Cooldowns))
                    {
                        //final check if Burst Strike is used right before No Mercy ends
                        if (LevelChecked(超高速Hypervelocity) && HasEffect(Buffs.ReadyToBlast))
                            return 超高速Hypervelocity;

                        if (HasEffect(Buffs.NoMercy) && LevelChecked(爆发击BurstStrike))
                        {
                            if (gauge.AmmoComboStep == 0)
                            {
                                if (LevelChecked(超高速Hypervelocity) && HasEffect(Buffs.ReadyToBlast))
                                    return 超高速Hypervelocity;

                                if (gauge.Ammo != 0
                                    && LevelChecked(倍攻DoubleDown)
                                    && GetCooldownRemainingTime(倍攻DoubleDown) > 20
                                    && GetCooldownRemainingTime(烈牙GnashingFang) > 10)
                                    return 爆发击BurstStrike;

                                if (gauge.Ammo != 0 && GetCooldownRemainingTime(烈牙GnashingFang) > 10 && GetCooldownRemainingTime(倍攻DoubleDown) > 40)
                                    return 爆发击BurstStrike;
                            }

                            if (LevelChecked(血壤Bloodfest)
                                && gauge.Ammo != 0
                                && GetCooldownRemainingTime(烈牙GnashingFang) > 10
                                && GetCooldownRemainingTime(血壤Bloodfest) <= GetCooldownRemainingTime(利刃斩KeenEdge))
                                return 爆发击BurstStrike;
                        }

                        //final check if Burst Strike is used right before No Mercy ends
                        if (LevelChecked(超高速Hypervelocity) && HasEffect(Buffs.ReadyToBlast))
                            return 超高速Hypervelocity;

                        if (gauge.AmmoComboStep is 1 or 2)
                        {
                            return OriginalHook(烈牙GnashingFang);
                        }
                    }
                }

                return actionID;
            }
        }


        internal class GNB_BS : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.GNB_BS;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID is 爆发击BurstStrike)
                {
                    var gauge = GetJobGauge<GNBGauge>();

                    if (IsEnabled(CustomComboPreset.GNB_BS_Continuation) && HasEffect(Buffs.ReadyToBlast) && LevelChecked(超高速Hypervelocity))
                        return 超高速Hypervelocity;

                    if (IsEnabled(CustomComboPreset.GNB_BS_Bloodfest)
                        && gauge.Ammo is 0
                        && LevelChecked(血壤Bloodfest)
                        && !HasEffect(Buffs.ReadyToBlast))
                        return 血壤Bloodfest;

                    if (IsEnabled(CustomComboPreset.GNB_BS_DoubleDown)
                        && HasEffect(Buffs.NoMercy)
                        && GetCooldownRemainingTime(倍攻DoubleDown) < 2
                        && gauge.Ammo >= 2
                        && LevelChecked(倍攻DoubleDown))

                        return 倍攻DoubleDown;
                }

                return actionID;
            }
        }


        internal class GNB_AoE_MainCombo : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.GNB_AoE_MainCombo;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID == 恶魔切DemonSlice)
                {
                    var gauge = GetJobGauge<GNBGauge>();

                    if (IsEnabled(CustomComboPreset.GNB_Variant_Cure)
                        && IsEnabled(Variant.VariantCure)
                        && PlayerHealthPercentageHp() <= GetOptionValue(Config.GNB_VariantCure))
                        return Variant.VariantCure;

                    if (InCombat())
                    {
                        if (CanWeave(actionID))
                        {
                            Status? sustainedDamage = FindTargetEffect(Variant.Debuffs.SustainedDamage);
                            if (IsEnabled(CustomComboPreset.GNB_Variant_SpiritDart)
                                && IsEnabled(Variant.VariantSpiritDart)
                                && (sustainedDamage is null || sustainedDamage?.RemainingTime <= 3))
                                return Variant.VariantSpiritDart;

                            if (IsEnabled(CustomComboPreset.GNB_Variant_Ultimatum)
                                && IsEnabled(Variant.VariantUltimatum)
                                && IsOffCooldown(Variant.VariantUltimatum))
                                return Variant.VariantUltimatum;

                            if (IsEnabled(CustomComboPreset.GNB_AoE_NoMercy) && ActionReady(无情NoMercy))
                                return 无情NoMercy;

                            if (IsEnabled(CustomComboPreset.GNB_AoE_BowShock) && ActionReady(弓形冲波BowShock))
                                return 弓形冲波BowShock;

                            if (IsEnabled(CustomComboPreset.GNB_AOE_DangerZone) && ActionReady(爆破领域DangerZone))
                                return OriginalHook(爆破领域DangerZone);

                            if (IsEnabled(CustomComboPreset.GNB_AoE_Bloodfest) && gauge.Ammo == 0 && ActionReady(血壤Bloodfest))
                                return 血壤Bloodfest;
                        }

                        if (IsEnabled(CustomComboPreset.GNB_AOE_SonicBreak) && ActionReady(音速破SonicBreak))
                            return 音速破SonicBreak;

                        if (IsEnabled(CustomComboPreset.GNB_AoE_DoubleDown) && gauge.Ammo >= 2 && ActionReady(倍攻DoubleDown))
                            return 倍攻DoubleDown;


                        if (IsEnabled(CustomComboPreset.GNB_AoE_Bloodfest)
                            && gauge.Ammo != 0
                            && GetCooldownRemainingTime(血壤Bloodfest) < 6
                            && LevelChecked(命运之环FatedCircle))
                            return 命运之环FatedCircle;
                    }

                    if (comboTime > 0 && lastComboMove == 恶魔切DemonSlice && LevelChecked(恶魔杀DemonSlaughter))
                    {
                        return (IsEnabled(CustomComboPreset.GNB_AOE_Overcap) && LevelChecked(命运之环FatedCircle) && gauge.Ammo == MaxCartridges(level))
                            ? 命运之环FatedCircle
                            : 恶魔杀DemonSlaughter;
                    }

                    return 恶魔切DemonSlice;
                }

                return actionID;
            }
        }


        internal class GNB_AuroraProtection : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.GNB_AuroraProtection;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                return (actionID is Aurora && HasEffect(Buffs.Aurora)) ? WAR.NascentFlash : actionID;
            }
        }
    }
}