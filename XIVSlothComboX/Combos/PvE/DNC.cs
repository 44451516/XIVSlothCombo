using Dalamud.Game.ClientState.JobGauge.Types;
using XIVSlothComboX.Combos.JobHelpers;
using XIVSlothComboX.Combos.PvE.Content;
using XIVSlothComboX.Core;
using XIVSlothComboX.CustomComboNS;
using XIVSlothComboX.CustomComboNS.Functions;
using XIVSlothComboX.Extensions;
using XIVSlothComboX.Services;

namespace XIVSlothComboX.Combos.PvE
{
    internal class DNC
    {
        public const byte JobID = 38;

        public const uint
            // Single Target
            瀑泻Cascade = 15989,
            喷泉Fountain = 15990,
            逆瀑泻ReverseCascade = 15991,
            坠喷泉Fountainfall = 15992,
            流星舞StarfallDance = 25792,
            // AoE
            风车Windmill = 15993,
            落刃雨Bladeshower = 15994,
            升风车RisingWindmill = 15995,
            落血雨Bloodshower = 15996,
            提拉纳Tillana = 25790,
            // Dancing
            标准舞步StandardStep = 15997,
            技巧舞步TechnicalStep = 15998,
            StandardFinish0 = 16003,
            StandardFinish1 = 16191,
            双色标准舞步结束StandardFinish2 = 16192,
            TechnicalFinish0 = 16004,
            TechnicalFinish1 = 16193,
            TechnicalFinish2 = 16194,
            TechnicalFinish3 = 16195,
            四色技巧舞步结束TechnicalFinish4 = 16196,
            四色技巧舞步结束TechnicalFinish4_0 = 33218,
            // Fan Dances
            扇舞序FanDance1 = 16007,
            扇舞破FanDance2 = 16008,
            扇舞急FanDance3 = 16009,
            扇舞终FanDance4 = 25791,
            // Other
            速行Peloton = 7557,
            剑舞SaberDance = 16005,
            EnAvant = 16010,
            进攻之探戈Devilment = 16011,
            防守之桑巴ShieldSamba = 16012,
            百花争艳Flourish = 16013,
            即兴表演Improvisation = 16014,
            治疗之华尔兹CuringWaltz = 16015,
            //7.0 等待技能ID
            最后一舞LastDance = 36983,
            舞步终结 = 4584,
            晓之舞DanceOfTheDawn = 36985,
            _ = 0;

        public static class Buffs
        {
            public const ushort
                // Flourishing & Silken (Procs)
                FlourishingCascade = 1814,
                FlourishingFountain = 1815,
                FlourishingWindmill = 1816,
                FlourishingShower = 1817,
                FlourishingFanDance = 2021,
                对称投掷SilkenSymmetry = 2693,
                非对称投掷SilkenFlow = 2694,
                提拉纳预备FlourishingFinish = 2698,
                流星舞预备FlourishingStarfall = 2700,
                对称投掷_百花争艳FlourishingSymmetry = 3017,
                非对称投掷_百花争艳FlourishingFlow = 3018,
                // Dances
                标准舞步预备StandardStep = 1818,
                技巧舞步预备TechnicalStep = 1819,
                StandardFinish = 1821,
                技巧舞步结束TechnicalFinish = 1822,
                // Fan Dances
                扇舞_急预备ThreeFoldFanDance = 1820,
                扇舞_终FourFoldFanDance = 2699,
                // Other
                速行Peloton = 1199,
                防守之桑巴ShieldSamba = 1826,
                //7.0 等待BUFFid
                最后一舞预备PRE = 1826,
                舞步终结预备 = 1826,
                晓之舞预备 = 3869,
                _ = 0;
        }

        /*
        public static class Debuffs
        {
            public const short placeholder = 0;
        }
        */

        public static class Config
        {
            public static UserInt DNC_VariantCure = new("DNC_VariantCure");

            public const string DNCEspritThreshold_ST = "DNCEspritThreshold_ST"; // Single target Esprit threshold
            public const string DNCEspritThreshold_AoE = "DNCEspritThreshold_AoE"; // AoE Esprit threshold

            #region Simple ST Sliders

            public const string DNCSimpleSSBurstPercent = "DNCSimpleSSBurstPercent"; // Standard Step    target HP% threshold
            public const string DNCSimpleTSBurstPercent = "DNCSimpleTSBurstPercent"; // Technical Step   target HP% threshold
            public const string DNCSimpleFeatherBurstPercent = "DNCSimpleFeatherBurstPercent"; // Feather burst    target HP% threshold
            public const string DNCSimplePanicHealWaltzPercent = "DNCSimplePanicHealWaltzPercent"; // Curing Waltz     player HP% threshold
            public const string DNCSimplePanicHealWindPercent = "DNCSimplePanicHealWindPercent"; // Second Wind      player HP% threshold

            #endregion

            #region Simple AoE Sliders

            public const string DNCSimpleSSAoEBurstPercent = "DNCSimpleSSAoEBurstPercent"; // Standard Step    target HP% threshold
            public const string DNCSimpleTSAoEBurstPercent = "DNCSimpleTSAoEBurstPercent"; // Technical Step   target HP% threshold
            public const string DNCSimpleAoEPanicHealWaltzPercent = "DNCSimpleAoEPanicHealWaltzPercent"; // Curing Waltz     player HP% threshold 
            public const string DNCSimpleAoEPanicHealWindPercent = "DNCSimpleAoEPanicHealWindPercent"; // Second Wind      player HP% threshold

            #endregion
        }

       

       
        

        /*
         * 单体模块
         */
        internal class DNC_DT_SimpleMode : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.DNC_DT_SimpleMode;

            internal static DNCOpenerLogic _DNCOpenerLogic = new();

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID is 瀑泻Cascade)
                {
                    if (IsEnabled(CustomComboPreset.DNC_Variant_Rampart)
                        && IsEnabled(Variant.VariantCure)
                        && PlayerHealthPercentageHp() <= Config.DNC_VariantCure)
                        return Variant.VariantCure;

                    if (IsEnabled(CustomComboPreset.DNC_Variant_Rampart)
                        && IsEnabled(Variant.VariantRampart)
                        && IsOffCooldown(Variant.VariantRampart)
                        && CanWeave(actionID))
                        return Variant.VariantRampart;

                    #region Types

                    DNCGauge? gauge = GetJobGauge<DNCGauge>();
                    bool canWeave = CanSpellWeavePlus(actionID);
                    bool flow = HasEffect(Buffs.非对称投掷SilkenFlow) || HasEffect(Buffs.非对称投掷_百花争艳FlourishingFlow);
                    bool symmetry = HasEffect(Buffs.对称投掷SilkenSymmetry) || HasEffect(Buffs.对称投掷_百花争艳FlourishingSymmetry);
                    float techBurstTimer = GetBuffRemainingTime(Buffs.技巧舞步结束TechnicalFinish);

                    float 技巧舞步TechnicalStepCD倒计时 = GetCooldownRemainingTime(技巧舞步TechnicalStep);

                    bool techBurst = HasEffect(Buffs.技巧舞步结束TechnicalFinish);
                    bool standardStepReady = 标准舞步StandardStep.ActionReady();
                    bool technicalStepReady = 技巧舞步TechnicalStep.ActionReady();

                    #endregion


                    // Opener for DNC 自动跳舞
                    if (IsEnabled(CustomComboPreset.DNC_DT_Simple_AUTO_SS))
                    {
                        if (_DNCOpenerLogic.DoFullOpener(ref actionID))
                        {
                            return actionID;
                        }
                    }


                    // Simple DT Standard Steps & Fill Feature
                    if (HasEffect(Buffs.标准舞步预备StandardStep))
                        // if (HasEffect(Buffs.标准舞步StandardStep) && IsEnabled(CustomComboPreset.DNC_DT_Simple_SS))
                        return gauge.CompletedSteps < 2 ? gauge.NextStep : 双色标准舞步结束StandardFinish2;

                    // Simple DT Tech Steps & Fill Feature
                    if (HasEffect(Buffs.技巧舞步预备TechnicalStep))
                        // if (HasEffect(Buffs.技巧舞步TechnicalStep) && IsEnabled(CustomComboPreset.DNC_DT_Simple_TS))
                        return gauge.CompletedSteps < 4 ? gauge.NextStep : 四色技巧舞步结束TechnicalFinish4;


                    // Devilment & Flourish
                    if (canWeave)
                    {
                        bool flourishReady = InCombat()
                                             && 百花争艳Flourish.ActionReady()
                                             && !HasEffect(Buffs.扇舞_急预备ThreeFoldFanDance)
                                             && !HasEffect(Buffs.扇舞_终FourFoldFanDance)
                                             && !HasEffect(Buffs.对称投掷_百花争艳FlourishingSymmetry)
                                             && !HasEffect(Buffs.非对称投掷_百花争艳FlourishingFlow);

                        bool devilmentReady = 进攻之探戈Devilment.ActionReady() && IsEnabled(CustomComboPreset.DNC_DT_Simple_Devilment);

                        if (devilmentReady && (techBurst || !LevelChecked(技巧舞步TechnicalStep)))
                            return 进攻之探戈Devilment;

                        if (WasLastAction(四色技巧舞步结束TechnicalFinish4))
                            return 进攻之探戈Devilment;


                        if (IsEnabled(CustomComboPreset.DNC_DT_Simple_Flourish) && flourishReady && CanDelayedWeavePlus(actionID, 1.5, 0.5f))
                            return 百花争艳Flourish;
                    }

                    if (canWeave)
                    {
                        // 低等级循环
                        if (!LevelChecked(扇舞急FanDance3) && gauge.Feathers > 0)
                        {
                            return 扇舞序FanDance1;
                        }


                        if (LevelChecked(扇舞破FanDance2))
                        {
                            int minFeathers = LevelChecked(技巧舞步TechnicalStep) ? (GetCooldownRemainingTime(技巧舞步TechnicalStep) < 5f ? 4 : 3) : 0;

                            if (LevelChecked(技巧舞步TechnicalStep))
                            {
                                if (!WasLastAction(四色技巧舞步结束TechnicalFinish4)
                                    && GetCooldownRemainingTime(进攻之探戈Devilment) > 0
                                    && GetCooldownRemainingTime(进攻之探戈Devilment) < 118)
                                {
                                    if (IsEnabled(CustomComboPreset.DNC_DT_Simple_TS) && GetCooldownRemainingTime(技巧舞步TechnicalStep) > 3)
                                    {
                                        if (HasEffect(Buffs.扇舞_急预备ThreeFoldFanDance))
                                            return 扇舞急FanDance3;

                                        if (HasEffect(Buffs.扇舞_终FourFoldFanDance))
                                            return 扇舞终FanDance4;

                                        if (!HasEffect(Buffs.扇舞_急预备ThreeFoldFanDance) && !WasLastAction(提拉纳Tillana))
                                        {
                                            if (gauge.Feathers > 0)
                                            {
                                                if (gauge.Feathers > minFeathers || (HasEffect(Buffs.技巧舞步结束TechnicalFinish)))
                                                {
                                                    return 扇舞序FanDance1;
                                                }

                                                if (RaidBuff.爆发期() && gauge.Feathers > 0)
                                                {
                                                    if (LevelChecked(技巧舞步TechnicalStep))
                                                    {
                                                        if (技巧舞步TechnicalStepCD倒计时 >= 40)
                                                        {
                                                            return 扇舞序FanDance1;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        return 扇舞序FanDance1;
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    if (HasEffect(Buffs.扇舞_急预备ThreeFoldFanDance) && IsNotEnabled(CustomComboPreset.DNC_DT_Simple_TS))
                                        return 扇舞急FanDance3;
                                }
                            }
                            else
                            {
                                if (HasEffect(Buffs.扇舞_急预备ThreeFoldFanDance))
                                    return 扇舞急FanDance3;
                            }
                        }
                    }

                    if (standardStepReady && IsEnabled(CustomComboPreset.DNC_DT_Simple_SS) && !HasEffect(Buffs.StandardFinish))
                    {
                        return 标准舞步StandardStep;
                    }


                    // Simple DT Tech (activates dance with no target, or when target is over HP% threshold)
                    if (IsEnabled(CustomComboPreset.DNC_DT_Simple_TS) && technicalStepReady && !HasEffect(Buffs.标准舞步预备StandardStep))
                    {
                        return 技巧舞步TechnicalStep;
                    }

                    if (LevelChecked(剑舞SaberDance) && IsEnabled(CustomComboPreset.DNC_DT_Simple_SaberDance))
                    {
                        if (gauge.Esprit >= 50)
                        {
                            if (晓之舞DanceOfTheDawn.LevelChecked() && HasEffect(Buffs.晓之舞预备))
                                return 晓之舞DanceOfTheDawn;
                        }

                        if (IsEnabled(CustomComboPreset.DNC_DT_Simple_SaberDance_1))
                        {
                            if (gauge.Esprit >= 70 && 剑舞SaberDance.GCDActionPreReady(技巧舞步TechnicalStep))
                            {
                                return 剑舞SaberDance;
                            }
                        }

                        if ((gauge.Esprit >= 85 || (techBurst && gauge.Esprit >= 50)))
                        {
                            return 剑舞SaberDance;
                        }

                        if (技巧舞步TechnicalStepCD倒计时 >= 40 && RaidBuff.爆发期() && gauge.Esprit >= 50)
                        {
                            return 剑舞SaberDance;
                        }
                    }

                    if (IsEnabled(CustomComboPreset.DNC_DT_Simple_最后一舞))
                    {
                        if (最后一舞LastDance.LevelChecked() && HasEffect(Buffs.最后一舞预备PRE))
                            return 最后一舞LastDance;
                    }

                    if (IsEnabled(CustomComboPreset.DNC_DT_Simple_舞步终结))
                    {
                        if (舞步终结.LevelChecked() && HasEffect(Buffs.舞步终结预备))
                            return 舞步终结;
                    }

                    if (LevelChecked(喷泉Fountain) && lastComboMove is 瀑泻Cascade && comboTime is < 2 and > 0)
                        return 喷泉Fountain;

                    if (HasEffect(Buffs.提拉纳预备FlourishingFinish))
                        return 提拉纳Tillana;

                    if (HasEffect(Buffs.流星舞预备FlourishingStarfall))
                        return 流星舞StarfallDance;

                    // Simple DT Standard (activates dance with no target, or when target is over HP% threshold)
                    // limited to 3 times in 120 seconds
                    // bool burstProtected = (LevelChecked(技巧舞步TechnicalStep) && IsEnabled(CustomComboPreset.DNC_DT_Simple_TS))
                    //     ? ((技巧舞步TechnicalStepCD倒计时 < 90 && 技巧舞步TechnicalStepCD倒计时 > 5)
                    //        || (techBurstTimer > 5 && !canWeave && !flow && !symmetry && comboTime <= 0))
                    //     : true;

                    // bool flourishProtected = (LevelChecked(百花争艳Flourish) && IsEnabled(CustomComboPreset.DNC_DT_Simple_Flourish))
                    //     ? (GetCooldownRemainingTime(百花争艳Flourish) > 4)
                    //     : true;

               

                    if (LevelChecked(坠喷泉Fountainfall) && flow)
                        return 坠喷泉Fountainfall;

                    if (LevelChecked(逆瀑泻ReverseCascade) && symmetry)
                        return 逆瀑泻ReverseCascade;

                    if (LevelChecked(喷泉Fountain) && lastComboMove is 瀑泻Cascade && comboTime > 0)
                        return 喷泉Fountain;
                }

                return actionID;
            }
        }

        /***
         * AOE模块
         */
        internal class DNC_AoE_SimpleMode : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.DNC_AoE_SimpleMode;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID is 风车Windmill)
                {
                    if (IsEnabled(CustomComboPreset.DNC_Variant_Rampart)
                        && IsEnabled(Variant.VariantCure)
                        && PlayerHealthPercentageHp() <= Config.DNC_VariantCure)
                        return Variant.VariantCure;

                    if (IsEnabled(CustomComboPreset.DNC_Variant_Rampart)
                        && IsEnabled(Variant.VariantRampart)
                        && IsOffCooldown(Variant.VariantRampart)
                        && CanWeave(actionID))
                        return Variant.VariantRampart;

                    #region Types

                    DNCGauge? gauge = GetJobGauge<DNCGauge>();
                    bool canWeave = CanWeave(actionID);
                    bool flow = HasEffect(Buffs.非对称投掷SilkenFlow) || HasEffect(Buffs.非对称投掷_百花争艳FlourishingFlow);
                    bool symmetry = HasEffect(Buffs.对称投掷SilkenSymmetry) || HasEffect(Buffs.对称投掷_百花争艳FlourishingSymmetry);
                    float techBurstTimer = GetBuffRemainingTime(Buffs.技巧舞步结束TechnicalFinish);
                    bool techBurst = HasEffect(Buffs.技巧舞步结束TechnicalFinish);
                    bool improvisationReady = LevelChecked(即兴表演Improvisation) && IsOffCooldown(即兴表演Improvisation);
                    bool standardStepReady = LevelChecked(标准舞步StandardStep) && IsOffCooldown(标准舞步StandardStep);
                    bool technicalStepReady = LevelChecked(技巧舞步TechnicalStep) && IsOffCooldown(技巧舞步TechnicalStep);
                    bool interruptable = CanInterruptEnemy() && IsOffCooldown(All.伤头HeadGraze) && LevelChecked(All.伤头HeadGraze);
                    int standardStepBurstThreshold = PluginConfiguration.GetCustomIntValue(Config.DNCSimpleSSAoEBurstPercent);
                    int technicalStepBurstThreshold = PluginConfiguration.GetCustomIntValue(Config.DNCSimpleTSAoEBurstPercent);

                    #endregion

                    // Simple AoE Standard Steps & Fill Feature
                    if (HasEffect(Buffs.标准舞步预备StandardStep)
                        && (IsEnabled(CustomComboPreset.DNC_AoE_Simple_SS) || IsEnabled(CustomComboPreset.DNC_AoE_Simple_StandardFill)))
                        return gauge.CompletedSteps < 2 ? gauge.NextStep : 双色标准舞步结束StandardFinish2;

                    // Simple AoE Tech Steps & Fill Feature
                    if (HasEffect(Buffs.技巧舞步预备TechnicalStep)
                        && (IsEnabled(CustomComboPreset.DNC_AoE_Simple_TS) || IsEnabled(CustomComboPreset.DNC_AoE_Simple_TechFill)))
                        return gauge.CompletedSteps < 4 ? gauge.NextStep : 四色技巧舞步结束TechnicalFinish4;

                    if (IsEnabled(CustomComboPreset.DNC_AoE_Simple_Interrupt) && interruptable)
                        return All.伤头HeadGraze;

                    // Simple AoE Standard (activates dance with no target, or when target is over HP% threshold)
                    if ((!HasTarget() || GetTargetHPPercent() > standardStepBurstThreshold)
                        && IsEnabled(CustomComboPreset.DNC_AoE_Simple_SS)
                        && standardStepReady
                        && ((!HasEffect(Buffs.技巧舞步预备TechnicalStep) && !techBurst) || techBurstTimer > 5))
                        return 标准舞步StandardStep;

                    // Simple AoE Tech (activates dance with no target, or when target is over HP% threshold)
                    if ((!HasTarget() || GetTargetHPPercent() > technicalStepBurstThreshold)
                        && IsEnabled(CustomComboPreset.DNC_AoE_Simple_TS)
                        && technicalStepReady
                        && !HasEffect(Buffs.标准舞步预备StandardStep))
                        return 技巧舞步TechnicalStep;

                    if (canWeave)
                    {
                        bool flourishReady = LevelChecked(百花争艳Flourish)
                                             && IsOffCooldown(百花争艳Flourish)
                                             && !HasEffect(Buffs.扇舞_急预备ThreeFoldFanDance)
                                             && !HasEffect(Buffs.扇舞_终FourFoldFanDance)
                                             && !HasEffect(Buffs.对称投掷_百花争艳FlourishingSymmetry)
                                             && !HasEffect(Buffs.非对称投掷_百花争艳FlourishingFlow);
                        bool devilmentReady = LevelChecked(进攻之探戈Devilment) && IsOffCooldown(进攻之探戈Devilment);

                        // Simple AoE Tech Devilment
                        if (IsEnabled(CustomComboPreset.DNC_AoE_Simple_Devilment)
                            && devilmentReady
                            && (HasEffect(Buffs.技巧舞步结束TechnicalFinish) || !LevelChecked(技巧舞步TechnicalStep)))
                            return 进攻之探戈Devilment;
                        if (IsEnabled(CustomComboPreset.DNC_AoE_Simple_Flourish) && flourishReady)
                            return 百花争艳Flourish;
                    }

                    if (LevelChecked(剑舞SaberDance) && (gauge.Esprit >= 85 || (techBurst && gauge.Esprit > 50)))
                        return 剑舞SaberDance;

                    if (canWeave)
                    {
                        // Feathers
                        if (LevelChecked(扇舞序FanDance1) && IsEnabled(CustomComboPreset.DNC_AoE_Simple_Feathers))
                        {
                            // Pooling
                            int minFeathers = IsEnabled(CustomComboPreset.DNC_AoE_Simple_FeatherPooling) && LevelChecked(技巧舞步TechnicalStep) ? 3 : 0;

                            if (HasEffect(Buffs.扇舞_急预备ThreeFoldFanDance))
                                return 扇舞急FanDance3;

                            if (LevelChecked(扇舞破FanDance2) && (gauge.Feathers > minFeathers || (techBurst && gauge.Feathers > 0)))
                                return 扇舞破FanDance2;
                        }

                        if (HasEffect(Buffs.扇舞_终FourFoldFanDance))
                            return 扇舞终FanDance4;

                        // Panic Heals
                        if (IsEnabled(CustomComboPreset.DNC_AoE_Simple_PanicHeals))
                        {
                            bool curingWaltzReady = LevelChecked(治疗之华尔兹CuringWaltz) && IsOffCooldown(治疗之华尔兹CuringWaltz);
                            bool secondWindReady = LevelChecked(All.内丹SecondWind) && IsOffCooldown(All.内丹SecondWind);
                            int waltzThreshold = PluginConfiguration.GetCustomIntValue(Config.DNCSimpleAoEPanicHealWaltzPercent);
                            int secondWindThreshold = PluginConfiguration.GetCustomIntValue(Config.DNCSimpleAoEPanicHealWindPercent);

                            if (PlayerHealthPercentageHp() < waltzThreshold && curingWaltzReady)
                                return 治疗之华尔兹CuringWaltz;
                            if (PlayerHealthPercentageHp() < secondWindThreshold && secondWindReady)
                                return All.内丹SecondWind;
                        }

                        if (IsEnabled(CustomComboPreset.DNC_AoE_Simple_Improvisation) && improvisationReady)
                            return 即兴表演Improvisation;
                    }

                    // Simple AoE combos and burst attacks
                    if (LevelChecked(落刃雨Bladeshower) && lastComboMove is 风车Windmill && comboTime is < 2 and > 0)
                        return 落刃雨Bladeshower;

                    if (HasEffect(Buffs.提拉纳预备FlourishingFinish))
                        return 提拉纳Tillana;
                    if (HasEffect(Buffs.流星舞预备FlourishingStarfall))
                        return 流星舞StarfallDance;

                    if (LevelChecked(落血雨Bloodshower) && flow)
                        return 落血雨Bloodshower;
                    if (LevelChecked(升风车RisingWindmill) && symmetry)
                        return 升风车RisingWindmill;
                    if (LevelChecked(落刃雨Bladeshower) && lastComboMove is 风车Windmill && comboTime > 0)
                        return 落刃雨Bladeshower;
                }

                return actionID;
            }
        }
        
        
        /***
         * 一键跳舞
         */
        internal class DNC_DanceStepCombo : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.DNC_DanceStepCombo;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                DNCGauge? gauge = GetJobGauge<DNCGauge>();

                // Standard Step
                if (actionID is 标准舞步StandardStep && gauge.IsDancing && HasEffect(Buffs.标准舞步预备StandardStep))
                    return gauge.CompletedSteps < 2 ? gauge.NextStep : 双色标准舞步结束StandardFinish2;

                // Technical Step
                if ((actionID is 技巧舞步TechnicalStep) && gauge.IsDancing && HasEffect(Buffs.技巧舞步预备TechnicalStep))
                    return gauge.CompletedSteps < 4 ? gauge.NextStep : 四色技巧舞步结束TechnicalFinish4;

                return actionID;
            }
        }
    }
}