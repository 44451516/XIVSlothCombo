using Dalamud.Game.ClientState.JobGauge.Types;
using Dalamud.Game.ClientState.Objects.Types;
using Dalamud.Game.ClientState.Statuses;
using XIVSlothComboX.Combos.PvE.Content;
using XIVSlothComboX.Combos.PvP;
using XIVSlothComboX.Core;
using XIVSlothComboX.CustomComboNS;
using XIVSlothComboX.CustomComboNS.Functions;
using XIVSlothComboX.Data;
using XIVSlothComboX.Extensions;
using static XIVSlothComboX.CustomComboNS.Functions.CustomComboFunctions;

namespace XIVSlothComboX.Combos.PvE
{
    internal static class DRK
    {
        public const byte JobID = 32;

        public const int 血乱学习等级 = 68;

        public const uint
            HardSlash = 3617,
            Unleash = 3621,
            //吸收斩
            SyphonStrike = 3623,
            Souleater = 3632,
            腐秽大地SaltedEarth = 3639,
            AbyssalDrain = 3641,
            精雕怒斩CarveAndSpit = 3643,
            //血乱
            血乱Delirium = 7390,
            释放Quietus = 7391,
            //Bloodspiller
            血溅Bloodspiller = 7392,
            血溅3 = 36930,
            血溅2 = 36929,
            血溅1 = 36928,
            //暗黑波动
            暗黑波动FloodOfDarkness = 16466,
            //暗黑锋
            暗黑锋EdgeOfDarkness = 16467,
            刚魂StalwartSoul = 16468,
            FloodOfShadow = 16469,
            EdgeOfShadow = 16470,
            //弗雷
            LivingShadow = 16472,
            蔑视厌恶Disesteem = 36932,
            SaltAndDarkness = 25755,
            Oblation = 25754,
            Shadowbringer暗影使者 = 25757,
            // Shadowbringer = 29738,
            Plunge = 3640,
            //BloodWeapon
            嗜血BloodWeapon = 3625,
            Unmend = 3624;

        public static class Buffs
        {
            public const ushort
                //嗜血
                嗜血BloodWeapon = 742,
                Darkside = 751,
                BlackestNight = 1178,
                //血乱
                血乱Delirium1 = 1972,
                血乱Delirium2 = 3836,
                Scorn = 3837,
                SaltedEarth = 749;
        }

        public static int 血乱层数()
        {
            if (GetBuffStacks(Buffs.血乱Delirium1) > 0)
            {
                return GetBuffStacks(Buffs.血乱Delirium1);
            }

            if (GetBuffStacks(Buffs.血乱Delirium2) > 0)
            {
                return GetBuffStacks(Buffs.血乱Delirium2);
            }

            return 0;
        }

        public static class Debuffs
        {
            public const ushort Placeholder = 1;
        }

        public static class Config
        {
            public static UserInt
                DRK_MPManagement = new("DrkMPManagement", 3000),
                DRK_VariantCure = new("DRKVariantCure", 50),
                DRK_Burs_HP = new("DRK_Burs_HP", 30),
                DRK_KeepPlungeCharges = new("DrkKeepPlungeCharges", 0);

            public static UserFloat
                DRK_Burst_Delay = new("DRK_Burst_Delay", 5f),
                DRK_LivingShadow_Delay = new("DRK_LivingShadow_Delay", 0f),
                DRK_Burst_Delay_GCD = new("DRK_Burst_Delay_GCD", 7.5f);
        }


        internal class DRK_ST_Custom : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.DRK_Advanced_CustomMode;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID is DRK.SyphonStrike)
                {
                    if (OnOpenerCustomActionAction(out var customActionActionId))
                    {
                        return customActionActionId;
                    }
                }


                return actionID;
            }
        }


        /**
         * 7.0 起手
         * 1-爆发药、暗影峰1
         * 2-弗雷、血乱
         * 3-暗影使者1、精雕细琢
         * 1-腐秽大地1、暗影峰
         * 血溅-暗影峰2、暗影使者2
         * 血溅-暗影峰3、腐秽大地2
         * 血溅-暗影峰4
         * 血溅-暗影峰5
         */
        internal class DRK_SouleaterCombo : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.DRK_SouleaterCombo;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID == Souleater)
                {
                    // var plungeChargesRemaining = PluginConfiguration.GetCustomIntValue(Config.DRK_KeepPlungeCharges);
                    var gauge = GetJobGauge<DRKGauge>();
                    var mpRemaining = PluginConfiguration.GetCustomIntValue(Config.DRK_MPManagement);

                    if (IsEnabled(CustomComboPreset.DRK_Variant_Cure) && IsEnabled(Variant.VariantCure) && PlayerHealthPercentageHp() <= GetOptionValue(Config.DRK_VariantCure))
                        return Variant.VariantCure;

                    if (IsEnabled(CustomComboPreset.DRK_RangedUptime) && LevelChecked(Unmend) && !InMeleeRange() && HasBattleTarget())
                        return Unmend;

                    var burstDelay = PluginConfiguration.GetCustomFloatValue(Config.DRK_Burst_Delay);
                    var burstDelayGCD = PluginConfiguration.GetCustomFloatValue(Config.DRK_Burst_Delay_GCD);
                    var DRK_LivingShadow_Delay = PluginConfiguration.GetCustomFloatValue(Config.DRK_LivingShadow_Delay);
                    var Burs_HP = PluginConfiguration.GetCustomIntValue(Config.DRK_Burs_HP) * 10000;
                    var combatTotalSeconds = CombatEngageDuration().TotalSeconds;

                    // if (InCombat())
                    {
                        // oGCDs
                        // if (CanDelayedWeavePlus(actionID))
                        if (CanWeave(actionID))
                        {
                            Status? sustainedDamage = FindTargetEffect(Variant.Debuffs.SustainedDamage);
                            if (IsEnabled(CustomComboPreset.DRK_Variant_SpiritDart) && IsEnabled(Variant.VariantSpiritDart) && (sustainedDamage is null || sustainedDamage?.RemainingTime <= 3))
                                return Variant.VariantSpiritDart;

                            if (IsEnabled(CustomComboPreset.DRK_Variant_Ultimatum) && IsEnabled(Variant.VariantUltimatum) && IsOffCooldown(Variant.VariantUltimatum))
                                return Variant.VariantUltimatum;

                            //Mana Features
                            if (IsEnabled(CustomComboPreset.DRK_ManaOvercap))
                            {
                                //这里要改一下 改成根据120的逻辑
                                if ((combatTotalSeconds < 7 && gauge.DarksideTimeRemaining == 0) || combatTotalSeconds >= 6)
                                {
                                    if (CanDelayedWeavePlus(actionID))
                                    {
                                        //泄蓝
                                        if (IsEnabled(CustomComboPreset.DRK_EoSPooling))
                                        {
                                            if (GetCooldownRemainingTime(血乱Delirium) >= 40
                                                && (gauge.HasDarkArts || LocalPlayer.CurrentMp > (mpRemaining + 3000))
                                                && !WasLastAction(OriginalHook(暗黑锋EdgeOfDarkness))
                                                && LevelChecked(暗黑锋EdgeOfDarkness))
                                            {
                                                return OriginalHook(暗黑锋EdgeOfDarkness);
                                            }
                                        }


                                        //续buff
                                        if (gauge.DarksideTimeRemaining < 10 * 1000 && LocalPlayer.CurrentMp >= 3000)
                                        {
                                            if (LevelChecked(暗黑锋EdgeOfDarkness))
                                                return OriginalHook(暗黑锋EdgeOfDarkness);

                                            if (LevelChecked(暗黑波动FloodOfDarkness) && !LevelChecked(暗黑锋EdgeOfDarkness))
                                                return 暗黑波动FloodOfDarkness;
                                        }


                                        if (LocalPlayer.CurrentMp >= 9800)
                                        {
                                            if (LevelChecked(暗黑锋EdgeOfDarkness))
                                                return OriginalHook(暗黑锋EdgeOfDarkness);

                                            if (LevelChecked(暗黑波动FloodOfDarkness) && !LevelChecked(暗黑锋EdgeOfDarkness))
                                                return 暗黑波动FloodOfDarkness;
                                        }

                                        if (comboTime > 0 && lastComboMove == HardSlash && LocalPlayer.CurrentMp >= 9200)
                                        {
                                            if (LevelChecked(暗黑锋EdgeOfDarkness))
                                                return OriginalHook(暗黑锋EdgeOfDarkness);

                                            if (LevelChecked(暗黑波动FloodOfDarkness) && !LevelChecked(暗黑锋EdgeOfDarkness))
                                                return 暗黑波动FloodOfDarkness;
                                        }

                                        if (comboTime > 0 && lastComboMove == HardSlash && GetBuffStacks(Buffs.嗜血BloodWeapon) > 0 && LocalPlayer.CurrentMp > 8600)
                                        {
                                            if (LevelChecked(暗黑锋EdgeOfDarkness))
                                                return OriginalHook(暗黑锋EdgeOfDarkness);

                                            if (LevelChecked(暗黑波动FloodOfDarkness) && !LevelChecked(暗黑锋EdgeOfDarkness))
                                                return 暗黑波动FloodOfDarkness;
                                        }

                                        if (GetBuffStacks(Buffs.嗜血BloodWeapon) > 0 && LocalPlayer.CurrentMp >= 9200)
                                        {
                                            if (LevelChecked(暗黑锋EdgeOfDarkness))
                                                return OriginalHook(暗黑锋EdgeOfDarkness);

                                            if (LevelChecked(暗黑波动FloodOfDarkness) && !LevelChecked(暗黑锋EdgeOfDarkness))
                                                return 暗黑波动FloodOfDarkness;
                                        }
                                    }
                                }
                            }

                            //oGCD Features
                            if (gauge.DarksideTimeRemaining > 1)
                            {
                                if (IsEnabled(CustomComboPreset.DRK_MainComboCDs_Group))
                                {
                                    if (IsEnabled(CustomComboPreset.DRK_BloodWeapon) && 嗜血BloodWeapon.ActionReady() && level < 血乱学习等级)
                                    {
                                        return 嗜血BloodWeapon;
                                    }

                                    if (IsEnabled(CustomComboPreset.DRK_LivingShadow) && LivingShadow.ActionReady() && combatTotalSeconds > DRK_LivingShadow_Delay)
                                    {
                                        return LivingShadow;
                                    }

                                    if (IsEnabled(CustomComboPreset.DRK_Delirium) && 血乱Delirium.ActionReady() && gauge.Blood <= 70 && combatTotalSeconds > burstDelay)
                                    {
                                        return 血乱Delirium.OriginalHook();
                                    }

                                    if (IsEnabled(CustomComboPreset.DRK_SaltedEarth) && LevelChecked(腐秽大地SaltedEarth) && combatTotalSeconds > burstDelay)
                                    {
                                        if ((IsOffCooldown(腐秽大地SaltedEarth) && !HasEffect(Buffs.SaltedEarth))
                                            || //Salted Earth
                                            (HasEffect(Buffs.SaltedEarth) && IsOffCooldown(SaltAndDarkness) && IsOnCooldown(腐秽大地SaltedEarth) && LevelChecked(SaltAndDarkness)) && GetBuffRemainingTime(Buffs.SaltedEarth) > 0 && GetBuffRemainingTime(Buffs.SaltedEarth) < 9) //Salt and Darkness
                                            return OriginalHook(腐秽大地SaltedEarth);
                                    }

                                    if (LevelChecked(Shadowbringer暗影使者)
                                        && IsEnabled(CustomComboPreset.DRK_Shadowbringer)
                                        && !WasLastAction(Shadowbringer暗影使者)
                                        && combatTotalSeconds > burstDelay)
                                    {
                                        if (gauge.DarksideTimeRemaining > 0 && GetRemainingCharges(Shadowbringer暗影使者) > 0)
                                        {
                                            if (IsNotEnabled(CustomComboPreset.DRK_ShadowbringerBurst))
                                            {
                                                return Shadowbringer暗影使者;
                                            }

                                            if (IsEnabled(CustomComboPreset.DRK_ShadowbringerBurst))
                                            {
                                                
                                                if (gauge.ShadowTimeRemaining is > 0 and <= 10_000)
                                                {
                                                    return 0;
                                                }
                                            }
                                        }
                                    }


                                    if (IsEnabled(CustomComboPreset.DRK_CarveAndSpit) && 精雕怒斩CarveAndSpit.ActionReady() && combatTotalSeconds > burstDelay)
                                    {
                                        if (LivingShadow.LevelChecked())
                                        {
                                            if (GetCooldownRemainingTime(LivingShadow) > 0)
                                            {
                                                return 精雕怒斩CarveAndSpit;
                                            }
                                        }
                                        else
                                        {
                                            return 精雕怒斩CarveAndSpit;
                                        }
                                    }
                                }
                            }
                        }


                        if (IsEnabled(CustomComboPreset.DRK_MainComboCDs_Group))
                        {
                            if (IsEnabled(CustomComboPreset.DRK_蔑视厌恶))
                            {
                                if (GetBuffRemainingTime(Buffs.Scorn) is > 0 and <= 4)
                                {
                                    return 蔑视厌恶Disesteem;
                                }
                            }

                            //Delirium Features
                            if (LevelChecked(血乱Delirium) && IsEnabled(CustomComboPreset.DRK_Bloodspiller))
                            {
                                if (gauge.Blood >= 50 || 血乱层数() > 0)
                                {
                                    if (CurrentTarget is IBattleChara battleChara)
                                    {
                                        if (battleChara.CurrentHp < Burs_HP)
                                        {
                                            return 血溅Bloodspiller.OriginalHook();
                                        }
                                    }
                                }
                                
                                if (gauge.Blood >= 70 && 血乱层数() >= 3)
                                {
                                    //再不打要溢出了
                                    return 血溅Bloodspiller.OriginalHook();
                                }
                                
                                if (gauge.Blood >= 80 && 血乱层数() >= 2)
                                {
                                    //再不打要溢出了
                                    return 血溅Bloodspiller.OriginalHook();
                                }

                                //对团辅
                                if (GetCooldownRemainingTime(LivingShadow) >= 40)
                                {
                                    if (RaidBuff.爆发期())
                                    {
                                        if (gauge.Blood >= 50 || 血乱层数() > 0)
                                        {
                                            return 血溅Bloodspiller.OriginalHook();
                                        }
                                    }
                                }


                                // 不用对团辅直接放
                                if (GetCooldownRemainingTime(血乱Delirium) >= 40 && GetCooldownRemainingTime(LivingShadow) is >= 40 and <= 70)
                                {
                                    if (gauge.Blood >= 50 || 血乱层数() > 0)
                                    {
                                        return 血溅Bloodspiller.OriginalHook();
                                    }
                                }


                                //防止暗血溢出
                                if (comboTime > 0 && lastComboMove == SyphonStrike && gauge.Blood >= 60 && GetBuffStacks(Buffs.嗜血BloodWeapon) > 0 && 血乱层数() > 0)
                                {
                                    return 血溅Bloodspiller.OriginalHook();
                                }


                                if (level < 血乱学习等级)
                                {
                                    if (gauge.Blood == 100 && (GetCooldownRemainingTime(嗜血BloodWeapon) is >= 0 and < 5))
                                    {
                                        return 血溅Bloodspiller.OriginalHook();
                                    }

                                    //防止延后血乱
                                    if (gauge.Blood >= 60 && GetCooldownRemainingTime(嗜血BloodWeapon) is >= 0 and < 5 && GetCooldownRemainingTime(LivingShadow) is > 7.5f)
                                    {
                                        return 血溅Bloodspiller.OriginalHook();
                                    }
                                }
                                else
                                {
                                    if (gauge.Blood == 100 && (GetCooldownRemainingTime(血乱Delirium) is >= 0 and < 5))
                                    {
                                        return 血溅Bloodspiller.OriginalHook();
                                    }

                                    //防止延后血乱
                                    if (gauge.Blood >= 60 && GetCooldownRemainingTime(血乱Delirium) is >= 0 and < 5 && GetCooldownRemainingTime(LivingShadow) is > 7.5f)
                                    {
                                        return 血溅Bloodspiller.OriginalHook();
                                    }
                                }


                                //防止血溅没有打完
                                if (血乱层数() > 0 && GetCooldownRemainingTime(血乱Delirium) + GetCooldownRemainingTime(血溅Bloodspiller) <= 50.2f)
                                {
                                    return 血溅Bloodspiller.OriginalHook();
                                }

                                //Regular Delirium
                                if (血乱层数() > 0 && IsNotEnabled(CustomComboPreset.DRK_DelayedBloodspiller))
                                {
                                    return 血溅Bloodspiller.OriginalHook();
                                }

                                //已经用了直接打完吧
                                if (IsEnabled(CustomComboPreset.DRK_DelayedBloodspiller) && 血乱层数() > 0 && IsOnCooldown(血乱Delirium) && GetBuffStacks(Buffs.嗜血BloodWeapon) < 2)
                                {
                                    return 血溅Bloodspiller.OriginalHook();
                                }
                            }
                        }
                        
                        if (IsEnabled(CustomComboPreset.DRK_蔑视厌恶))
                        {
                            if (蔑视厌恶Disesteem.LevelChecked()  && RaidBuff.爆发期() && HasEffect(Buffs.Scorn))
                            {
                                return 蔑视厌恶Disesteem;
                            }
                        }

                        // 1-2-3 combo
                        if (comboTime > 0)
                        {
                            if (lastComboMove == HardSlash && LevelChecked(SyphonStrike))
                                return SyphonStrike;

                            if (lastComboMove == SyphonStrike && LevelChecked(Souleater))
                            {
                                if (IsEnabled(CustomComboPreset.DRK_BloodGaugeOvercap) && LevelChecked(血溅Bloodspiller.OriginalHook()) && gauge.Blood >= 90)
                                {
                                    return 血溅Bloodspiller.OriginalHook();
                                }


                                return Souleater;
                            }
                        }
                    }

                    return HardSlash;
                }

                return actionID;
            }
        }


        internal class DRK_StalwartSoulCombo : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.DRK_StalwartSoulCombo;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID == 刚魂StalwartSoul)
                {
                    var gauge = GetJobGauge<DRKGauge>();

                    if (IsEnabled(CustomComboPreset.DRK_Variant_Cure) && IsEnabled(Variant.VariantCure) && PlayerHealthPercentageHp() <= GetOptionValue(Config.DRK_VariantCure))
                        return Variant.VariantCure;


                    if (CanWeave(actionID))
                    {
                        Status? sustainedDamage = FindTargetEffect(Variant.Debuffs.SustainedDamage);
                        if (IsEnabled(CustomComboPreset.DRK_Variant_SpiritDart) && IsEnabled(Variant.VariantSpiritDart) && (sustainedDamage is null || sustainedDamage?.RemainingTime <= 3))
                            return Variant.VariantSpiritDart;

                        if (IsEnabled(CustomComboPreset.DRK_Variant_Ultimatum) && IsEnabled(Variant.VariantUltimatum) && IsOffCooldown(Variant.VariantUltimatum))
                            return Variant.VariantUltimatum;

                        if (IsEnabled(CustomComboPreset.DRK_AoE_ManaOvercap) && LevelChecked(暗黑波动FloodOfDarkness) && (gauge.HasDarkArts || LocalPlayer.CurrentMp > 8500 || (gauge.DarksideTimeRemaining < 10 && LocalPlayer.CurrentMp >= 3000)))
                            return OriginalHook(暗黑波动FloodOfDarkness);

                        if (gauge.DarksideTimeRemaining > 1)
                        {
                            if (IsEnabled(CustomComboPreset.DRK_AoE_BloodWeapon) && 嗜血BloodWeapon.ActionReady() && level < 血乱学习等级)
                                return 嗜血BloodWeapon;

                            if (IsEnabled(CustomComboPreset.DRK_AoE_Delirium) && IsOffCooldown(血乱Delirium) && LevelChecked(血乱Delirium))
                                return 血乱Delirium;

                            if (IsEnabled(CustomComboPreset.DRK_AoE_LivingShadow) && LivingShadow.ActionReady())
                                return LivingShadow;

                            if (IsEnabled(CustomComboPreset.DRK_AoE_SaltedEarth) && LevelChecked(腐秽大地SaltedEarth))
                            {
                                if ((IsOffCooldown(腐秽大地SaltedEarth) && !HasEffect(Buffs.SaltedEarth))
                                    || //Salted Earth
                                    (HasEffect(Buffs.SaltedEarth) && IsOffCooldown(SaltAndDarkness) && IsOnCooldown(腐秽大地SaltedEarth) && LevelChecked(SaltAndDarkness))) //Salt and Darkness
                                    return OriginalHook(腐秽大地SaltedEarth);
                            }

                            if (IsEnabled(CustomComboPreset.DRK_AoE_AbyssalDrain) && LevelChecked(AbyssalDrain) && IsOffCooldown(AbyssalDrain) && PlayerHealthPercentageHp() <= 60)
                                return AbyssalDrain;

                            if (IsEnabled(CustomComboPreset.DRK_AoE_Shadowbringer) && LevelChecked(Shadowbringer暗影使者) && GetRemainingCharges(Shadowbringer暗影使者) > 0)
                                return Shadowbringer暗影使者;
                        }
                    }

                    if (IsEnabled(CustomComboPreset.DRK_Delirium))
                    {
                        if (LevelChecked(血乱Delirium) && 血乱层数() > 0 && gauge.DarksideTimeRemaining > 0)
                            return 释放Quietus.OriginalHook();
                    }


                    if (蔑视厌恶Disesteem.LevelChecked() && HasEffect(Buffs.Scorn))
                    {
                        return 蔑视厌恶Disesteem;
                    }

                    if (comboTime > 0)
                    {
                        if (lastComboMove == Unleash && LevelChecked(刚魂StalwartSoul))
                        {
                            if (IsEnabled(CustomComboPreset.DRK_Overcap) && gauge.Blood >= 90 && LevelChecked(释放Quietus.OriginalHook()))
                                return 释放Quietus.OriginalHook();

                            return 刚魂StalwartSoul;
                        }
                    }

                    return Unleash;
                }

                return actionID;
            }
        }

        internal class DRK_oGCD : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.DRK_oGCD;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                var gauge = GetJobGauge<DRKGauge>();

                if (actionID == 精雕怒斩CarveAndSpit || actionID == AbyssalDrain)
                {
                    if (LivingShadow.ActionReady())
                        return LivingShadow;

                    if (IsOffCooldown(腐秽大地SaltedEarth) && LevelChecked(腐秽大地SaltedEarth))
                        return 腐秽大地SaltedEarth;

                    if (IsOffCooldown(精雕怒斩CarveAndSpit) && LevelChecked(AbyssalDrain))
                        return actionID;

                    if (IsOffCooldown(SaltAndDarkness) && HasEffect(Buffs.SaltedEarth) && LevelChecked(SaltAndDarkness))
                        return SaltAndDarkness;

                    if (IsEnabled(CustomComboPreset.DRK_Shadowbringer_oGCD) && GetCooldownRemainingTime(Shadowbringer暗影使者) < 60 && LevelChecked(Shadowbringer暗影使者) && gauge.DarksideTimeRemaining > 0)
                        return Shadowbringer暗影使者;
                }

                return actionID;
            }
        }
    }
}