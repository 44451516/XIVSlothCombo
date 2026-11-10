using SlothCombo.Runtime.Gauges;
using SlothCombo.Runtime.Services;
using SlothCombo.Interop.Game;
using SlothCombo.Combos.JobHelpers.Enums;
using SlothCombo.Combos.PvE;
using SlothCombo.CustomComboNS.Functions;
using SlothCombo.Data;
using static SlothCombo.Combos.PvE.RPR;
using static SlothCombo.CustomComboNS.Functions.CustomComboFunctions;

namespace SlothCombo.Combos.JobHelpers
{
    internal class RPR
    {
        internal class RPROpenerLogic
        {
            private static bool HasCooldowns()
            {
                if (GetRemainingCharges(灵魂切割SoulSlice) < 2)
                    return false;

                if (!ActionReady(神秘环ArcaneCircle))
                    return false;

                if (!ActionReady(暴食Gluttony))
                    return false;

                return true;
            }

            private static uint OpenerLevel => 100;

            public uint PrePullStep = 0;

            public uint OpenerStep = 1;

            public static bool LevelChecked => CustomComboFunctions.LocalPlayer.Level >= OpenerLevel;

            private static bool CanOpener => HasCooldowns() && LevelChecked;

            private OpenerState currentState = OpenerState.PrePull;

            public OpenerState CurrentState
            {
                get { return currentState; }
                set
                {
                    if (value != currentState)
                    {
                        if (value == OpenerState.PrePull)
                        {
                            Svc.Log.Debug($"Entered PrePull Opener");
                        }
                        if (value == OpenerState.InOpener) OpenerStep = 1;
                        if (value == OpenerState.OpenerFinished || value == OpenerState.FailedOpener)
                        {
                            if (value == OpenerState.FailedOpener)
                                Svc.Log.Information($"Opener Failed at step {OpenerStep}");

                            ResetOpener();
                        }
                        if (value == OpenerState.OpenerFinished) Svc.Log.Information("Opener Finished");

                        currentState = value;
                    }
                }
            }

            private bool DoPrePullSteps(ref uint actionID)
            {
                if (!LevelChecked) return false;

                if (CanOpener && PrePullStep == 0)
                {
                    PrePullStep = 1;
                }

                if (!HasCooldowns())
                {
                    PrePullStep = 0;
                }

                if (CurrentState == OpenerState.PrePull && PrePullStep > 0)
                {
                    if (WasLastAction(死亡之影ShadowOfDeath) && PrePullStep == 1) CurrentState = OpenerState.InOpener;
                    else if (PrePullStep == 1) actionID = 死亡之影ShadowOfDeath;

                    if (ActionWatching.CombatActions.Count > 2 && InCombat())
                        CurrentState = OpenerState.FailedOpener;

                    return true;
                }

                PrePullStep = 0;
                return false;
            }

            private bool DoOpener(ref uint actionID)
            {
                if (!LevelChecked) return false;

                if (currentState == OpenerState.InOpener)
                {
                    if (WasLastAction(灵魂切割SoulSlice) && OpenerStep == 1) OpenerStep++;
                    else if (OpenerStep == 1) actionID = 灵魂切割SoulSlice;

                    if (WasLastAction(神秘环ArcaneCircle) && OpenerStep == 2) OpenerStep++;
                    else if (OpenerStep == 2) actionID = 神秘环ArcaneCircle;

                    if (WasLastAction(暴食Gluttony) && OpenerStep == 3) OpenerStep++;
                    else if (OpenerStep == 3) actionID = 暴食Gluttony;

                    if (WasLastAction(ExecutionersGibbet) && OpenerStep == 4) OpenerStep++;
                    else if (OpenerStep == 4) actionID = ExecutionersGibbet;

                    if (WasLastAction(ExecutionersGallows) && OpenerStep == 5) OpenerStep++;
                    else if (OpenerStep == 5) actionID = ExecutionersGallows;

                    if (WasLastAction(大丰收PlentifulHarvest) && OpenerStep == 6) OpenerStep++;
                    else if (OpenerStep == 6) actionID = 大丰收PlentifulHarvest;

                    if (WasLastAction(夜游魂衣Enshroud) && OpenerStep == 7) OpenerStep++;
                    else if (OpenerStep == 7) actionID = 夜游魂衣Enshroud;

                    if (WasLastAction(虚无收割VoidReaping) && OpenerStep == 8) OpenerStep++;
                    else if (OpenerStep == 8) actionID = 虚无收割VoidReaping;

                    if (WasLastAction(Sacrificium) && OpenerStep == 9) OpenerStep++;
                    else if (OpenerStep == 9) actionID = Sacrificium;

                    if (WasLastAction(交错收割CrossReaping) && OpenerStep == 10) OpenerStep++;
                    else if (OpenerStep == 10) actionID = 交错收割CrossReaping;

                    if (WasLastAction(LemuresSlice) && OpenerStep == 11) OpenerStep++;
                    else if (OpenerStep == 11) actionID = LemuresSlice;

                    if (WasLastAction(虚无收割VoidReaping) && OpenerStep == 12) OpenerStep++;
                    else if (OpenerStep == 12) actionID = 虚无收割VoidReaping;

                    if (WasLastAction(交错收割CrossReaping) && OpenerStep == 13) OpenerStep++;
                    else if (OpenerStep == 13) actionID = 交错收割CrossReaping;

                    if (WasLastAction(LemuresSlice) && OpenerStep == 14) OpenerStep++;
                    else if (OpenerStep == 14) actionID = LemuresSlice;

                    if (WasLastAction(团契Communio) && OpenerStep == 15) OpenerStep++;
                    else if (OpenerStep == 15) actionID = 团契Communio;

                    if (WasLastAction(完人Perfectio) && OpenerStep == 16) OpenerStep++;
                    else if (OpenerStep == 16) actionID = 完人Perfectio;

                    if (WasLastAction(灵魂切割SoulSlice) && OpenerStep == 17) OpenerStep++;
                    else if (OpenerStep == 17) actionID = 灵魂切割SoulSlice;

                    if (WasLastAction(UnveiledGibbet) && OpenerStep == 18) OpenerStep++;
                    else if (OpenerStep == 18) actionID = UnveiledGibbet;

                    if (WasLastAction(绞决Gibbet) && OpenerStep == 19) OpenerStep++;
                    else if (OpenerStep == 19) actionID = 绞决Gibbet;

                    if (WasLastAction(死亡之影ShadowOfDeath) && OpenerStep == 20) OpenerStep++;
                    else if (OpenerStep == 20) actionID = 死亡之影ShadowOfDeath;

                    if (WasLastAction(切割Slice) && OpenerStep == 21) CurrentState = OpenerState.OpenerFinished;
                    else if (OpenerStep == 21) actionID = 切割Slice;

                    if (ActionWatching.TimeSinceLastAction.TotalSeconds >= 5)
                        CurrentState = OpenerState.FailedOpener;

                    if (((actionID == 灵魂切割SoulSlice && GetRemainingCharges(灵魂切割SoulSlice) == 0) || (actionID == 神秘环ArcaneCircle && IsOnCooldown(神秘环ArcaneCircle)) || (actionID == 暴食Gluttony && IsOnCooldown(暴食Gluttony))) && ActionWatching.TimeSinceLastAction.TotalSeconds >= 3)
                    {
                        CurrentState = OpenerState.FailedOpener;
                        return false;
                    }
                    return true;
                }
                return false;
            }

            private void ResetOpener()
            {
                PrePullStep = 0;
                OpenerStep = 0;
            }

            public bool DoFullOpener(ref uint actionID)
            {
                if (!LevelChecked)
                    return false;

                if (CurrentState == OpenerState.PrePull)
                    if (DoPrePullSteps(ref actionID))
                        return true;

                if (CurrentState == OpenerState.InOpener)
                {
                    if (DoOpener(ref actionID))
                        return true;
                }

                if (!InCombat())
                {
                    ResetOpener();
                    CurrentState = OpenerState.PrePull;
                }
                return false;
            }
        }

        internal class RPRHelpers
        {
            public unsafe static bool IsComboExpiring(float Times)
            {
                float GCD = GetCooldown(切割Slice).CooldownTotal * Times;

                if (ActionManager.Instance()->Combo.Timer != 0 && ActionManager.Instance()->Combo.Timer < GCD)
                    return true;

                else return false;
            }

            public static bool IsBuffExpiring(float Times)
            {
                float GCD = GetCooldown(切割Slice).CooldownTotal * Times;

                if ((HasEffect(Buffs.缢杀效果提高EnhancedGallows) && GetBuffRemainingTime(Buffs.缢杀效果提高EnhancedGallows) < GCD) || (HasEffect(Buffs.绞决效果提高EnhancedGibbet) && GetBuffRemainingTime(Buffs.绞决效果提高EnhancedGibbet) < GCD))
                    return true;

                else return false;
            }

            public static bool IsDebuffExpiring(float Times)
            {
                float GCD = GetCooldown(切割Slice).CooldownTotal * Times;

                if (TargetHasEffect(Debuffs.死亡烙印DeathsDesign) && GetDebuffRemainingTime(Debuffs.死亡烙印DeathsDesign) < GCD)
                    return true;

                else return false;
            }

            public static bool UseEnshroud(RPRGauge gauge)
            {
                float GCD = GetCooldown(切割Slice).CooldownTotal;

                if (LevelChecked(夜游魂衣Enshroud) && (gauge.Shroud >= 50 || HasEffect(Buffs.IdealHost)) && !HasEffect(Buffs.妖异之镰SoulReaver) && !HasEffect(Buffs.处刑人Executioner) && !HasEffect(Buffs.完人预备PerfectioParata) && !HasEffect(Buffs.夜游魂Enshrouded))
                {
                    // Before Plentiful Harvest 
                    if (!LevelChecked(大丰收PlentifulHarvest))
                        return true;

                    // Shroud in Arcane Circle 
                    if (HasEffect(Buffs.神秘环ArcaneCircle))
                        return true;

                    // Prep for double Enshroud
                    if (LevelChecked(大丰收PlentifulHarvest) && GetCooldownRemainingTime(神秘环ArcaneCircle) <= (GCD * 2) + 1.5)
                        return true;

                    //2nd part of Double Enshroud
                    // if (LevelChecked(大丰收PlentifulHarvest) && WasLastWeaponskill(大丰收PlentifulHarvest))
                    if (LevelChecked(大丰收PlentifulHarvest) && JustUsed(大丰收PlentifulHarvest, 5))
                        return true;

                    //Natural Odd Minute Shrouds
                    if (!HasEffect(Buffs.神秘环ArcaneCircle) && !IsDebuffExpiring(5) && GetCooldownRemainingTime(神秘环ArcaneCircle) is >= 50 and <= 65)
                        return true;

                    // Correction for 2 min windows 
                    if (!HasEffect(Buffs.神秘环ArcaneCircle) && !IsDebuffExpiring(5) && gauge.Soul >= 90)
                        return true;
                }
                return false;
            }

            public static bool UseShadowOfDeath()
            {
                float GCD = GetCooldown(切割Slice).CooldownTotal;

                if (LevelChecked(死亡之影ShadowOfDeath)
                    && !HasEffect(Buffs.妖异之镰SoulReaver)
                    && !HasEffect(Buffs.处刑人Executioner)
                    // && !HasEffect(Buffs.完人预备PerfectioParata) 
                    // && !HasEffect(Buffs.死亡祭品ImmortalSacrifice) 
                    && !IsComboExpiring(3)
                    // && !JustUsed(死亡之影ShadowOfDeath)
                    )
                {
                    if (GetDebuffRemainingTime(Debuffs.死亡烙印DeathsDesign) > 30)
                    {
                        return false;
                    }
                    //神秘纹快好了 打
                    if (LevelChecked(大丰收PlentifulHarvest)
                        && HasEffect(Buffs.夜游魂Enshrouded)
                        && GetCooldownRemainingTime(神秘环ArcaneCircle) <= (GCD * 2) + 1.5
                        && JustUsed(夜游魂衣Enshroud))
                        return true;

                    if (LevelChecked(大丰收PlentifulHarvest)
                        && HasEffect(Buffs.夜游魂Enshrouded)
                        && ((GetCooldownRemainingTime(神秘环ArcaneCircle) <= GCD) || IsOffCooldown(神秘环ArcaneCircle))
                        && (JustUsed(虚无收割VoidReaping) || JustUsed(交错收割CrossReaping)))
                        return true;

                    if (LevelChecked(大丰收PlentifulHarvest)
                        && !HasEffect(Buffs.夜游魂Enshrouded)
                        && ((IsEnabled(CustomComboPreset.RPR_ST_SimpleMode) && GetDebuffRemainingTime(Debuffs.死亡烙印DeathsDesign) <= 8)
                            || (IsEnabled(CustomComboPreset.RPR_ST_AdvancedMode) && GetDebuffRemainingTime(Debuffs.死亡烙印DeathsDesign) <= Config.RPR_SoDRefreshRange))
                        && ((GetCooldownRemainingTime(神秘环ArcaneCircle) > GCD * 8) || IsOffCooldown(神秘环ArcaneCircle)))
                        return true;

                    //below lvl 88 use
                    if (!LevelChecked(大丰收PlentifulHarvest) && ((IsEnabled(CustomComboPreset.RPR_ST_SimpleMode) && GetDebuffRemainingTime(Debuffs.死亡烙印DeathsDesign) <= 8) || (IsEnabled(CustomComboPreset.RPR_ST_AdvancedMode) && GetDebuffRemainingTime(Debuffs.死亡烙印DeathsDesign) <= Config.RPR_SoDRefreshRange)))
                        return true;

                }
                return false;
            }
        }
    }
}