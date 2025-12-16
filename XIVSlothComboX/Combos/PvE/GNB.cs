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
using Status = Dalamud.Game.ClientState.Statuses.IStatus;

namespace XIVSlothComboX.Combos.PvE
{
    internal static class GNB
    {
        public const byte JobID = 37;

        public static int MaxCartridges(byte level) => level >= 88 ? 3 : 2;


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
                          命运之印 = 36936,
                          师心连1FatedBrand = 36937,
                          师心连2ReignOfBeasts = 36938,
                          师心连3NobleBlood = 36939;

        public static class Buffs
        {
            public const ushort NoMercy = 1831,
                                Aurora = 1835,
                                //撕喉预备
                                子弹连1ReadyToRip = 1842,
                                //子弹连
                                子弹连2ReadyToTear = 1843,
                                子弹连3ReadyToGouge = 1844,
                                //音速破
                                ReadyToBreak = 3886,
                                //超高速
                                超高速ReadyToBlast = 2686,
                                ReadyToReign = 3840,
                                ReadyToRaze命运之印预备 = 3839;
        }

        public static class Debuffs
        {
            public const ushort BowShock = 1838,
                                SonicBreak = 1837;
        }

        public static class Config
        {
            public static UserFloat
                GNB_Burst_Delay = new("GNB_Burst_Delay", 1.9f);

            public static UserInt
                GNB_SkS = new("GNB_SkS", 1),
                GNB_VariantCure = new("GNB_VariantCure");

            public const string
                // GNB_SkS = "GNB_SkS",
                GNB_START_GCD = "GNB_START_GCD",
                GNB_RoughDivide_HeldCharges = "GNB_RoughDivide_HeldCharges"
                // GNB_Burst_Delay = "GNB_Burst_Delay",
                // GNB_VariantCure = "GNB_VariantCure"
                ;
        }


        internal class GNB_ST_Custom : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.GNB_Advanced_CustomMode;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID is GNB.残暴弹BrutalShell)
                {
                    if (OnOpenerCustomActionAction(out var customActionActionId))
                    {
                        return customActionActionId;
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
                    var burstDelay = PluginConfiguration.GetCustomFloatValue(Config.GNB_Burst_Delay);
                    var combatTotalSeconds = CombatEngageDuration().TotalSeconds;

                    var gnbGauge = GetJobGauge<GNBGauge>();

                    if (IsEnabled(CustomComboPreset.GNB_Variant_Cure)
                        && IsEnabled(Variant.VariantCure)
                        && PlayerHealthPercentageHp() <= GetOptionValue(Config.GNB_VariantCure))
                    {
                        return Variant.VariantCure;
                    }


                    if (IsEnabled(CustomComboPreset.GNB_ST_RangedUptime) && !InMeleeRange() && LevelChecked(闪雷弹LightningShot) && HasBattleTarget())
                    {
                        return 闪雷弹LightningShot;
                    }


                    //oGCDs
                    if (CanSpellWeavePlus(actionID, 0.4f))
                    {
                        Status? sustainedDamage = FindTargetEffect(Variant.Debuffs.SustainedDamage);
                        if (IsEnabled(CustomComboPreset.GNB_Variant_SpiritDart)
                            && IsEnabled(Variant.VariantSpiritDart)
                            && (sustainedDamage is null || sustainedDamage?.RemainingTime <= 3))
                        {
                            return Variant.VariantSpiritDart;
                        }


                        if (IsEnabled(CustomComboPreset.GNB_Variant_Ultimatum)
                            && IsEnabled(Variant.VariantUltimatum)
                            && IsOffCooldown(Variant.VariantUltimatum))
                        {
                            return Variant.VariantUltimatum;
                        }

                        if (IsEnabled(CustomComboPreset.GNB_ST_MainCombo_CooldownsGroup))
                        {
                            if (无情的使用条件(lastComboMove, level, gnbGauge, burstDelay))
                            {
                                return 无情NoMercy;
                            }

                            if (血壤的使用条件(gnbGauge, combatTotalSeconds, burstDelay))
                            {
                                return 血壤Bloodfest;
                            }


                            if (CanDelayedWeavePlus(actionID, 1.25f, 0.4f) && LevelChecked(超高速Hypervelocity) && HasEffect(Buffs.超高速ReadyToBlast))
                            {
                                return 超高速Hypervelocity;
                            }

                            //Continuation
                            if (IsEnabled(CustomComboPreset.GNB_ST_Gnashing)
                                && LevelChecked(续剑Continuation)
                                && (HasEffect(Buffs.子弹连1ReadyToRip) || HasEffect(Buffs.子弹连2ReadyToTear) || HasEffect(Buffs.子弹连3ReadyToGouge)))
                            {
                                return OriginalHook(续剑Continuation);
                            }

                            //非无情期间 30秒
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


                            //60s weaves
                            if (HasEffect(Buffs.NoMercy))
                            {
                                if (IsEnabled(CustomComboPreset.GNB_ST_BlastingZone) && ActionReady(爆破领域DangerZone))
                                {
                                    return OriginalHook(爆破领域DangerZone);
                                }

                                if (IsEnabled(CustomComboPreset.GNB_ST_BowShock) && ActionReady(弓形冲波BowShock))
                                {
                                    return 弓形冲波BowShock;
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
                            if (!LevelChecked(倍攻DoubleDown))
                            {
                                if (主连击_使用子弹连(gnbGauge, level, lastComboMove))
                                {
                                    return OriginalHook(烈牙GnashingFang);
                                }
                                
                                if (是否使用音速破(ref actionID))
                                {
                                    return actionID;
                                }

                            }
                            else if (LevelChecked(倍攻DoubleDown))
                            {
                                int 先打什么选项 = PluginConfiguration.GetCustomIntValue(Config.GNB_SkS);
                                switch (先打什么选项)
                                {
                                    case 1:
                                    {
                                        if (主连击_使用子弹连(gnbGauge, level, lastComboMove))
                                        {
                                            return OriginalHook(烈牙GnashingFang);
                                        }

                                        if (IsEnabled(CustomComboPreset.GNB_ST_DoubleDown) && 倍攻是否准备就绪() && gnbGauge.Ammo >= 1)
                                        {
                                            return 倍攻DoubleDown;
                                        }

                                        if (使用师心连(lastComboMove))
                                        {
                                            return OriginalHook(血壤Bloodfest);
                                        }

                                        if (是否使用音速破(ref actionID))
                                        {
                                            return actionID;
                                        }


                                        break;
                                    }

                                    case 2:
                                    {
                                        if (主连击_使用子弹连(gnbGauge, level, lastComboMove))
                                        {
                                            return OriginalHook(烈牙GnashingFang);
                                        }

                                        if (IsEnabled(CustomComboPreset.GNB_ST_DoubleDown) && 倍攻是否准备就绪() && gnbGauge.Ammo >= 1)
                                        {
                                            return 倍攻DoubleDown;
                                        }

                                        if (使用师心连(lastComboMove))
                                        {
                                            return OriginalHook(血壤Bloodfest);
                                        }


                                        if (是否使用音速破(ref actionID))
                                        {
                                            return actionID;
                                        }
                                        break;
                                    }


                                    case 3:
                                    {
                                        if (使用师心连(lastComboMove))
                                        {
                                            return OriginalHook(血壤Bloodfest);
                                        }

                                        if (主连击_使用子弹连(gnbGauge, level, lastComboMove))
                                        {
                                            return OriginalHook(烈牙GnashingFang);
                                        }

                                        if (IsEnabled(CustomComboPreset.GNB_ST_DoubleDown) && 倍攻是否准备就绪() && gnbGauge.Ammo >= 1)
                                        {
                                            return 倍攻DoubleDown;
                                        }

                                        if (是否使用音速破(ref actionID))
                                        {
                                            return actionID;
                                        }

                                        break;
                                    }


                                    case 4:
                                    {
                                        if (使用师心连(lastComboMove))
                                        {
                                            return OriginalHook(血壤Bloodfest);
                                        }

                                        if (IsEnabled(CustomComboPreset.GNB_ST_DoubleDown) && 倍攻是否准备就绪() && gnbGauge.Ammo >= 1)
                                        {
                                            return 倍攻DoubleDown;
                                        }

                                        if (主连击_使用子弹连(gnbGauge, level, lastComboMove))
                                        {
                                            return OriginalHook(烈牙GnashingFang);
                                        }

                                        if (是否使用音速破(ref actionID))
                                        {
                                            return actionID;
                                        }

                                        break;
                                    }


                                    case 5:
                                    {
                                        if (IsEnabled(CustomComboPreset.GNB_ST_DoubleDown) && 倍攻是否准备就绪() && gnbGauge.Ammo >= 1)
                                        {
                                            return 倍攻DoubleDown;
                                        }

                                        if (使用师心连(lastComboMove))
                                        {
                                            return OriginalHook(血壤Bloodfest);
                                        }


                                        if (主连击_使用子弹连(gnbGauge, level, lastComboMove))
                                        {
                                            return OriginalHook(烈牙GnashingFang);
                                        }

                                        if (是否使用音速破(ref actionID))
                                        {
                                            return actionID;
                                        }
                                        break;
                                    }


                                    case 6:
                                    {
                                        if (IsEnabled(CustomComboPreset.GNB_ST_DoubleDown) && 倍攻是否准备就绪() && gnbGauge.Ammo >= 1)
                                        {
                                            return 倍攻DoubleDown;
                                        }

                                        if (主连击_使用子弹连(gnbGauge, level, lastComboMove))
                                        {
                                            return OriginalHook(烈牙GnashingFang);
                                        }


                                        if (使用师心连(lastComboMove))
                                        {
                                            return OriginalHook(血壤Bloodfest);
                                        }


                                        if (是否使用音速破(ref actionID))
                                        {
                                            return actionID;
                                        }


                                        break;
                                    }
                                    default:
                                    {
                                        if (IsEnabled(CustomComboPreset.GNB_ST_DoubleDown) && 倍攻是否准备就绪() && gnbGauge.Ammo >= 1)
                                        {
                                            return 倍攻DoubleDown;
                                        }

                                        if (主连击_使用子弹连(gnbGauge, level, lastComboMove))
                                        {
                                            return OriginalHook(烈牙GnashingFang);
                                        }


                                        if (使用师心连(lastComboMove))
                                        {
                                            return OriginalHook(血壤Bloodfest);
                                        }


                                        if (是否使用音速破(ref actionID))
                                        {
                                            return actionID;
                                        }

                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (IsEnabled(CustomComboPreset.GNB_ST_Gnashing))
                            {
                                if (主连击_使用子弹连(gnbGauge, level, lastComboMove))
                                {
                                    return OriginalHook(烈牙GnashingFang);
                                }


                                if (gnbGauge.Ammo > 0 && 子弹连是否准备就绪() && GetCooldownRemainingTime(无情NoMercy) > 17 && GetCooldownRemainingTime(无情NoMercy) < 35)
                                {
                                    return 烈牙GnashingFang;
                                }
                            }
                        }
                    }

                    if (gnbGauge.AmmoComboStep is 3 or 4)
                    {
                        return 血壤Bloodfest.OriginalHook();
                    }


                    if (使用师心连许剑())
                    {
                        return 血壤Bloodfest.OriginalHook();
                    }


                    if (gnbGauge.AmmoComboStep is 1 or 2)
                    {
                        return OriginalHook(烈牙GnashingFang);
                    }


                    if (IsEnabled(CustomComboPreset.GNB_ST_BurstStrike) && IsEnabled(CustomComboPreset.GNB_ST_MainCombo_CooldownsGroup))
                    {
                        if (HasEffect(Buffs.NoMercy) && LevelChecked(爆发击BurstStrike))
                        {
                            if (gnbGauge.AmmoComboStep == 0)
                            {
                                if (gnbGauge.Ammo != 0
                                    && LevelChecked(倍攻DoubleDown)
                                    && GetCooldownRemainingTime(倍攻DoubleDown) > 20
                                    && GetCooldownRemainingTime(烈牙GnashingFang) > 10)
                                {
                                    return 爆发击BurstStrike;
                                }

                                if (gnbGauge.Ammo != 0 && GetCooldownRemainingTime(烈牙GnashingFang) > 7.5f && GetCooldownRemainingTime(倍攻DoubleDown) > 40)
                                {
                                    return 爆发击BurstStrike;
                                }
                                
                            }

                            if (LevelChecked(血壤Bloodfest)
                                && gnbGauge.Ammo != 0
                                && GetCooldownRemainingTime(烈牙GnashingFang) > 10
                                && GetCooldownRemainingTime(血壤Bloodfest) <= GetCooldownRemainingTime(利刃斩KeenEdge))
                                return 爆发击BurstStrike;
                        }
                        
                        
                       

                        if (师心连1FatedBrand.LevelChecked())
                        {
                            
                            if (gnbGauge.Ammo != 0 && GetCooldownRemainingTime(血壤Bloodfest)  <=10f && GetCooldownRemainingTime(无情NoMercy) <= 10f )
                            {
                                return 爆发击BurstStrike;
                            }
                            
                            // if (GetCooldownRemainingTime(血壤Bloodfest) < 7 && gnbGauge.Ammo != 0 && GetCooldownRemainingTime(无情NoMercy) > 2.4f)
                            // {
                            //     return 爆发击BurstStrike;
                            // }
                            //
                            // if (GetCooldownRemainingTime(血壤Bloodfest) < 7 && gnbGauge.Ammo != 0 && GetCooldownRemainingTime(无情NoMercy) < 2.4f && lastComboMove == 残暴弹BrutalShell)
                            // {
                            //     return 爆发击BurstStrike;
                            // }
                        }
                    }

                    // Regular 1-2-3 combo with overcap feature
                    if (comboTime > 0)
                    {
                        if (lastComboMove == 利刃斩KeenEdge && LevelChecked(残暴弹BrutalShell))
                            return 残暴弹BrutalShell;

                        if (lastComboMove == 残暴弹BrutalShell && LevelChecked(迅连斩SolidBarrel))
                        {
                            if (LevelChecked(超高速Hypervelocity) && HasEffect(Buffs.超高速ReadyToBlast))
                                return 超高速Hypervelocity;

                            if (LevelChecked(爆发击BurstStrike) && gnbGauge.Ammo == MaxCartridges(level))
                                return 爆发击BurstStrike;

                            return 迅连斩SolidBarrel;
                        }
                    }

                    return 利刃斩KeenEdge;
                }

                return actionID;
            }

            private static bool 是否使用音速破(ref uint actionID)
            {

                if (音速破SonicBreak.LevelChecked() && HasEffect(Buffs.ReadyToBreak))
                {
                    if (GetCooldownRemainingTime(倍攻DoubleDown) > 45)
                    {
                        actionID = 音速破SonicBreak.OriginalHook();
                        return true;
                    }
                    
                    if (HasEffect(Buffs.NoMercy) && GetCooldownRemainingTime(烈牙GnashingFang) < 2.5)
                    {
                        actionID = 音速破SonicBreak.OriginalHook();
                        return true;
                    }


                }
                return false;
            }

            private static bool 血壤的使用条件(GNBGauge gnbGauge, double combatTotalSeconds, float burstDelay)
            {
                if (IsNotEnabled(CustomComboPreset.GNB_ST_Bloodfest))
                {
                    return false;
                }

                if (gnbGauge.Ammo != 0)
                {
                    return false;
                }

                if (!ActionReady(血壤Bloodfest))
                {
                    return false;
                }

                if (combatTotalSeconds < burstDelay)
                {
                    return false;
                }


                if (HasEffect(Buffs.子弹连1ReadyToRip)
                    || HasEffect(Buffs.子弹连2ReadyToTear)
                    || HasEffect(Buffs.子弹连3ReadyToGouge)
                    || HasEffect(Buffs.超高速ReadyToBlast))
                {
                    return false;
                }


                if (ActionReady(血壤Bloodfest))
                {
                    return true;
                }

                return false;
            }

            private static bool 无情的使用条件(uint lastComboMove, byte level, GNBGauge gnbGauge, float burstDelay)
            {
                if (IsEnabled(CustomComboPreset.GNB_ST_MainCombo_CooldownsGroup)
                    && IsEnabled(CustomComboPreset.GNB_ST_NoMercy)
                    && ActionReady(无情NoMercy))
                {
                    var combatTotalSeconds = CombatEngageDuration().TotalSeconds;

                    if (combatTotalSeconds < burstDelay)
                    {
                        return false;
                    }

                    if (!LevelChecked(倍攻DoubleDown))
                    {
                        if (gnbGauge.Ammo >= 1)
                        {
                            return true;
                        }
                    }

                    if (WasLastAction(血壤Bloodfest))
                    {
                        return true;
                    }

                    if (GetCooldownRemainingTime(血壤Bloodfest) == 0)
                    {
                        return true;
                    }


                    if (gnbGauge.Ammo >= 2)
                    {
                        return true;
                    }

                    if (gnbGauge.Ammo >= 1 && lastComboMove == 残暴弹BrutalShell)
                    {
                        if (子弹连是否准备就绪())
                        {
                            return true;
                        }

                        if (gnbGauge.AmmoComboStep is 1 or 2)
                        {
                            return true;
                        }
                    }

                    if (gnbGauge.Ammo >= 1 && lastComboMove == 残暴弹BrutalShell)
                    {
                        if (gnbGauge.AmmoComboStep is 1 or 2)
                        {
                            return true;
                        }
                    }

                    if (gnbGauge.Ammo >= 1 && 子弹连是否准备就绪() && 血壤Bloodfest.ActionReady())
                    {
                        if (gnbGauge.AmmoComboStep is 0)
                        {
                            return true;
                        }
                    }


                    if (gnbGauge.AmmoComboStep is 1 or 2 && GetCooldownRemainingTime(血壤Bloodfest) < 10)
                    {
                        return true;
                    }

                    if (gnbGauge.AmmoComboStep is 1 && gnbGauge.Ammo >= 1)
                    {
                        return true;
                    }

                }

                return false;
            }

            public static bool 子弹连是否准备就绪()
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


            public static bool 主连击_使用子弹连(GNBGauge gauge, byte level, uint lastComboMove)
            {
                if (CustomComboFunctions.IsNotEnabled(CustomComboPreset.GNB_ST_Gnashing))
                {
                    return false;
                }

                if (OriginalHook(GNB.血壤Bloodfest) is 师心连2ReignOfBeasts or 师心连3NobleBlood)
                {
                    return false;
                }

                if (子弹连是否准备就绪() == false)
                {
                    return false;
                }


                if (HasEffect(Buffs.NoMercy) || GetCooldownRemainingTime(无情NoMercy) >= 40)
                {
                    if (!LevelChecked(倍攻DoubleDown))
                    {
                        if (gauge.Ammo >= 1)
                        {
                            return true;
                        }
                    }

                    // Service.ChatGui.PrintError($"111");

                    if (gauge.Ammo == 1 && IsOffCooldown(血壤Bloodfest))
                    {
                        return true;
                    }

                    if (gauge.Ammo == 1 && GetCooldownRemainingTime(倍攻DoubleDown) >= 40)
                    {
                        return true;
                    }

                    // Service.ChatGui.PrintError($"222");


                    if (gauge.Ammo == 3)
                    {
                        // Service.ChatGui.PrintError($"333");
                        return true;
                    }
                }

                if (gauge.Ammo == 3 && GetCooldownRemainingTime(无情NoMercy) <= GetCooldownRemainingTime(利刃斩KeenEdge))
                {
                    return true;
                }


                if (lastComboMove == 残暴弹BrutalShell && gauge.Ammo >= 2 && GetCooldownRemainingTime(无情NoMercy) < 1.78f)
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
                var gauge = GetJobGauge<GNBGauge>();
                if (gauge.AmmoComboStep is 1 or 2)
                {
                    return false;
                }


                if (OriginalHook(GNB.血壤Bloodfest) is 师心连1FatedBrand or 师心连2ReignOfBeasts or 师心连3NobleBlood)
                {
                    return true;
                }

                return false;
            }


            private static bool 使用师心连许剑()
            {

                if (OriginalHook(GNB.血壤Bloodfest) is 师心连2ReignOfBeasts or 师心连3NobleBlood)
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
                                    if (HasEffect(Buffs.NoMercy) && IsOnCooldown(倍攻DoubleDown))
                                    {
                                        return OriginalHook(爆破领域DangerZone);
                                    }
                                }

                                //Continuation
                                if (LevelChecked(续剑Continuation)
                                    && (HasEffect(Buffs.子弹连1ReadyToRip) || HasEffect(Buffs.子弹连2ReadyToTear) || HasEffect(Buffs.子弹连3ReadyToGouge)))
                                    return OriginalHook(续剑Continuation);

                                //60s weaves
                                if (HasEffect(Buffs.NoMercy))
                                {
                                    //Post DD
                                    if ((IsOnCooldown(倍攻DoubleDown)))
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
                                && (HasEffect(Buffs.子弹连1ReadyToRip) || HasEffect(Buffs.子弹连2ReadyToTear) || HasEffect(Buffs.子弹连3ReadyToGouge)))
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


                                        if (使用师心连(lastComboMove))
                                        {
                                            return OriginalHook(血壤Bloodfest);
                                        }


                                        if (音速破SonicBreak.LevelChecked() && GetCooldownRemainingTime(倍攻DoubleDown) > 50 && HasEffect(Buffs.ReadyToBreak))
                                        {
                                            return 音速破SonicBreak;
                                        }

                                        break;
                                    }

                                    case 2:
                                    {
                                        if (子弹连_使用子弹连(gauge, level, lastComboMove))
                                            return OriginalHook(烈牙GnashingFang);

                                        if (使用师心连(lastComboMove))
                                        {
                                            return OriginalHook(血壤Bloodfest);
                                        }


                                        if (IsEnabled(CustomComboPreset.GNB_ST_DoubleDown) && 倍攻是否准备就绪() && gauge.Ammo >= 2)
                                            return 倍攻DoubleDown;


                                        if (音速破SonicBreak.LevelChecked() && GetCooldownRemainingTime(倍攻DoubleDown) > 50 && HasEffect(Buffs.ReadyToBreak))
                                        {
                                            return 音速破SonicBreak;
                                        }


                                        break;
                                    }


                                    case 3:
                                    {
                                        if (使用师心连(lastComboMove))
                                        {
                                            return OriginalHook(血壤Bloodfest);
                                        }

                                        if (子弹连_使用子弹连(gauge, level, lastComboMove))
                                            return OriginalHook(烈牙GnashingFang);

                                        if (IsEnabled(CustomComboPreset.GNB_ST_DoubleDown) && 倍攻是否准备就绪() && gauge.Ammo >= 2)
                                            return 倍攻DoubleDown;


                                        if (音速破SonicBreak.LevelChecked() && GetCooldownRemainingTime(倍攻DoubleDown) > 50 && HasEffect(Buffs.ReadyToBreak))
                                        {
                                            return 音速破SonicBreak;
                                        }

                                        break;
                                    }


                                    case 4:
                                    {
                                        if (使用师心连(lastComboMove))
                                        {
                                            return OriginalHook(血壤Bloodfest);
                                        }

                                        if (IsEnabled(CustomComboPreset.GNB_ST_DoubleDown) && 倍攻是否准备就绪() && gauge.Ammo >= 2)
                                            return 倍攻DoubleDown;

                                        if (子弹连_使用子弹连(gauge, level, lastComboMove))
                                            return OriginalHook(烈牙GnashingFang);

                                        if (音速破SonicBreak.LevelChecked() && GetCooldownRemainingTime(倍攻DoubleDown) > 50 && HasEffect(Buffs.ReadyToBreak))
                                        {
                                            return 音速破SonicBreak;
                                        }


                                        break;
                                    }


                                    case 5:
                                    {
                                        if (IsEnabled(CustomComboPreset.GNB_ST_DoubleDown) && 倍攻是否准备就绪() && gauge.Ammo >= 2)
                                            return 倍攻DoubleDown;

                                        if (使用师心连(lastComboMove))
                                        {
                                            return OriginalHook(血壤Bloodfest);
                                        }


                                        if (子弹连_使用子弹连(gauge, level, lastComboMove))
                                            return OriginalHook(烈牙GnashingFang);


                                        if (音速破SonicBreak.LevelChecked() && GetCooldownRemainingTime(倍攻DoubleDown) > 50 && HasEffect(Buffs.ReadyToBreak))
                                        {
                                            return 音速破SonicBreak;
                                        }

                                        break;
                                    }


                                    case 6:
                                    {
                                        if (IsEnabled(CustomComboPreset.GNB_ST_DoubleDown) && 倍攻是否准备就绪() && gauge.Ammo >= 2)
                                            return 倍攻DoubleDown;

                                        if (子弹连_使用子弹连(gauge, level, lastComboMove))
                                            return 烈牙GnashingFang;


                                        if (使用师心连(lastComboMove))
                                        {
                                            return OriginalHook(血壤Bloodfest);
                                        }

                                        if (音速破SonicBreak.LevelChecked() && GetCooldownRemainingTime(倍攻DoubleDown) > 50 && HasEffect(Buffs.ReadyToBreak))
                                        {
                                            return 音速破SonicBreak;
                                        }

                                        break;
                                    }

                                    default:
                                    {
                                        if (子弹连_使用子弹连(gauge, level, lastComboMove))
                                            return OriginalHook(烈牙GnashingFang);

                                        if (IsEnabled(CustomComboPreset.GNB_ST_DoubleDown) && 倍攻是否准备就绪() && gauge.Ammo >= 2)
                                            return 倍攻DoubleDown;


                                        if (使用师心连(lastComboMove))
                                        {
                                            return OriginalHook(血壤Bloodfest);
                                        }

                                        if (音速破SonicBreak.LevelChecked() && GetCooldownRemainingTime(倍攻DoubleDown) > 50 && HasEffect(Buffs.ReadyToBreak))
                                        {
                                            return 音速破SonicBreak;
                                        }


                                        break;
                                    }
                                }
                            }


                            if (!LevelChecked(倍攻DoubleDown) && IsEnabled(CustomComboPreset.GNB_GF_Cooldowns) && CanSpellWeavePlus(actionID))
                            {
                                if (ActionReady(音速破SonicBreak) && !HasEffect(Buffs.子弹连1ReadyToRip) && IsOnCooldown(烈牙GnashingFang))
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
                            if (LevelChecked(超高速Hypervelocity) && HasEffect(Buffs.超高速ReadyToBlast))
                                return 超高速Hypervelocity;

                            if (HasEffect(Buffs.NoMercy) && LevelChecked(爆发击BurstStrike))
                            {
                                if (gauge.AmmoComboStep == 0)
                                {
                                    if (LevelChecked(超高速Hypervelocity) && HasEffect(Buffs.超高速ReadyToBlast))
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
                            if (LevelChecked(超高速Hypervelocity) && HasEffect(Buffs.超高速ReadyToBlast))
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

                        if (IsEnabled(CustomComboPreset.GNB_BS_Continuation) && HasEffect(Buffs.超高速ReadyToBlast) && LevelChecked(超高速Hypervelocity))
                            return 超高速Hypervelocity;

                        if (IsEnabled(CustomComboPreset.GNB_BS_Bloodfest)
                            && gauge.Ammo is 0
                            && LevelChecked(血壤Bloodfest)
                            && !HasEffect(Buffs.超高速ReadyToBlast))
                            return 血壤Bloodfest;

                        if (IsEnabled(CustomComboPreset.GNB_BS_DoubleDown)
                            && HasEffect(Buffs.NoMercy)
                            && 倍攻是否准备就绪()
                            && gauge.Ammo >= 2)
                            return 倍攻DoubleDown;

                        if (IsEnabled(CustomComboPreset.GNB_BS_ReignOfBeasts) && 使用师心连(lastComboMove))
                        {
                            return OriginalHook(血壤Bloodfest);
                        }
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
                            if (CanDelayedWeavePlus(actionID))
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

                                if (LevelChecked(续剑Continuation) && (HasEffect(Buffs.ReadyToRaze命运之印预备)))
                                {
                                    return OriginalHook(续剑Continuation);
                                }
                            }


                            if (IsEnabled(CustomComboPreset.GNB_AoE_DoubleDown) && gauge.Ammo >= 2 && ActionReady(倍攻DoubleDown) && ActionReady(无情NoMercy) == false)
                            {
                                return 倍攻DoubleDown;
                            }

                            if (IsEnabled(CustomComboPreset.GNB_AOE_SonicBreak) && HasEffect(Buffs.ReadyToBreak) && 倍攻是否准备就绪() == false)
                            {
                                return 音速破SonicBreak;
                            }


                            if (IsEnabled(CustomComboPreset.GNB_AOE_ReignOfBeasts) && 使用师心连(actionID))
                            {
                                return OriginalHook(血壤Bloodfest);
                            }


                            if (IsEnabled(CustomComboPreset.GNB_AoE_Bloodfest))
                            {
                                if (gauge.Ammo != 0
                                    && GetCooldownRemainingTime(血壤Bloodfest) < 6
                                    && lastComboMove == 恶魔切DemonSlice
                                    && LevelChecked(命运之环FatedCircle))
                                {
                                    return 命运之环FatedCircle;
                                }
                            }
                        }

                        if (comboTime > 0 && lastComboMove == 恶魔切DemonSlice)
                        {
                            if (IsEnabled(CustomComboPreset.GNB_AOE_Overcap) && LevelChecked(命运之环FatedCircle) && gauge.Ammo == MaxCartridges(level))
                            {
                                return 命运之环FatedCircle;
                            }

                            if (恶魔杀DemonSlaughter.LevelChecked())
                            {
                                return 恶魔杀DemonSlaughter;
                            }
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
            
            
            internal class GNB_FatedCircle : CustomCombo
            {
                protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.GNB_FatedCircle;

                protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
                {
                    if (actionID == 命运之环FatedCircle)
                    {
                        if (CanSpellWeavePlus(actionID))
                        {
                            if (LevelChecked(命运之印) && (HasEffect(Buffs.ReadyToRaze命运之印预备)))
                            {
                                return OriginalHook(续剑Continuation);
                            }
                        }
                       
                        return 命运之环FatedCircle;
                    }
                    return actionID;
                }
            }
        }
    }
}