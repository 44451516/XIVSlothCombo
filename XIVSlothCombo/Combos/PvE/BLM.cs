using System;
using System.Collections.Generic;
using Dalamud.Game.ClientState.JobGauge.Types;
using Dalamud.Game.ClientState.Statuses;
using XIVSlothCombo.Combos.PvE.Content;
using XIVSlothCombo.Core;
using XIVSlothCombo.CustomComboNS;
using XIVSlothCombo.CustomComboNS.Functions;
using static FFXIVClientStructs.FFXIV.Client.UI.Misc.RaptureMacroModule.Macro;

namespace XIVSlothCombo.Combos.PvE
{
    internal static class BLM
    {
        public const byte ClassID = 7;
        public const byte JobID = 25;

        internal const uint
            Fire = 141,
            Blizzard = 142,
            Thunder = 144,
            Blizzard2 = 25793,
            Transpose = 149,
            Fire2 = 147,
            Fire3 = 152,
            Thunder3 = 153,
            Thunder2 = 7447,
            Thunder4 = 7420,
            Blizzard3 = 154,
            Scathe = 156,
            Freeze = 159,
            Flare = 162,
            AetherialManipulation = 155,
            LeyLines = 3573,
            Blizzard4 = 3576,
            Fire4 = 3577,
            BetweenTheLines = 7419,
            Despair = 16505,
            UmbralSoul = 16506,
            Paradox = 25797,
            Amplifier = 25796,
            HighFire2 = 25794,
            HighBlizzard2 = 25795,
            Xenoglossy = 16507,
            Foul = 7422,
            Sharpcast = 3574,
            Manafont = 158,
            Triplecast = 7421;

        internal static class Buffs
        {
            internal const ushort
                Thundercloud = 164,
                LeyLines = 737,
                CircleOfPower = 738,
                Firestarter = 165,
                Sharpcast = 867,
                Triplecast = 1211;
        }

        internal static class Debuffs
        {
            internal const ushort
                Thunder = 161,
                Thunder2 = 162,
                Thunder3 = 163,
                Thunder4 = 1210;
        }

        internal static class Traits
        {
            internal const uint
                AspectMasteryIII = 459,
                EnhancedManafont = 463,
                EnhancedFreeze = 295;
        }
        internal static class MP
        {
            internal const int MaxMP = 10000;
            internal const int Despair = 800; //"ALL MP" spell. Only caring about the absolute minimum.
            internal static int Thunder => CustomComboFunctions.GetResourceCost(CustomComboFunctions.OriginalHook(BLM.Thunder));
            internal static int ThunderAoE => CustomComboFunctions.GetResourceCost(CustomComboFunctions.OriginalHook(BLM.Thunder2));
            internal static int Fire => CustomComboFunctions.GetResourceCost(CustomComboFunctions.OriginalHook(BLM.Fire));
            internal static int FireAoE => CustomComboFunctions.GetResourceCost(CustomComboFunctions.OriginalHook(BLM.Fire2));
            internal static int Fire3 => CustomComboFunctions.GetResourceCost(CustomComboFunctions.OriginalHook(BLM.Fire3));
            //internal static int Blizzard3 => CustomComboFunctions.GetResourceCost(CustomComboFunctions.OriginalHook(BLM.Blizzard3));
        }

        // Debuff Pairs of Actions and Debuff
        internal static readonly Dictionary<uint, ushort>
            ThunderList = new()
            {
                { Thunder,  Debuffs.Thunder  },
                { Thunder2, Debuffs.Thunder2 },
                { Thunder3, Debuffs.Thunder3 },
                { Thunder4, Debuffs.Thunder4 }
            };
        private static BLMGauge Gauge => CustomComboFunctions.GetJobGauge<BLMGauge>();
        private static bool HasPolyglotStacks(this BLMGauge gauge) => gauge.PolyglotStacks > 0;

        internal static class Config
        {
            internal const string BLM_PolyglotsStored = "BlmPolyglotsStored   ";
            internal const string BLM_AstralFireRefresh = "BlmAstralFireRefresh   ";
            internal const string BLM_MovementTime = "BlmMovementTime";
            internal const string BLM_VariantCure = "BlmVariantCure";
        }


        internal class BLM_Blizzard : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BLM_Blizzard;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID is Blizzard && LevelChecked(Freeze) && !Gauge.InUmbralIce)
                    return Blizzard3;
                if (actionID is Freeze && !LevelChecked(Freeze))
                    return Blizzard2;
                return actionID;
            }
        }

        internal class BLM_Fire_1to3 : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BLM_Fire_1to3;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID is Fire && ((LevelChecked(Fire3) && !Gauge.InAstralFire) || HasEffect(Buffs.Firestarter)))
                    return Fire3;

                return actionID;
            }
        }

        internal class BLM_LeyLines : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BLM_LeyLines;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level) =>
                actionID is LeyLines && HasEffect(Buffs.LeyLines) && LevelChecked(BetweenTheLines) ? BetweenTheLines : actionID;
        }

        internal class BLM_AetherialManipulation : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BLM_AetherialManipulation;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level) =>
                actionID is AetherialManipulation &&
                ActionReady(BetweenTheLines) &&
                HasEffect(Buffs.LeyLines) &&
                !HasEffect(Buffs.CircleOfPower) &&
                !IsMoving
                ? BetweenTheLines : actionID;
        }

        internal class BLM_Mana : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BLM_Mana;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level) =>
                actionID is Transpose && Gauge.InUmbralIce && LevelChecked(UmbralSoul) ? UmbralSoul : actionID;
        }

        internal class BLM_AoE_SimpleMode : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BLM_AoE_SimpleMode;

            internal static DateTime previousTime;
            internal static double movementTime = 0.0f;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID is Flare)
                {
                    // Handle movement
                    if (IsEnabled(CustomComboPreset.BLM_Simple_CastMovement) && InCombat())
                    {
                        var movementTimeThreshold = PluginConfiguration.GetCustomFloatValue(Config.BLM_MovementTime);
                        double deltaTime = (DateTime.Now - previousTime).TotalSeconds;
                        previousTime = DateTime.Now;
                        if (IsMoving)
                        {
                            movementTime = movementTime + deltaTime > movementTimeThreshold + 0.02 ? movementTimeThreshold + 0.02 : movementTime + deltaTime;
                        }
                        else
                        {
                            movementTime = movementTime - deltaTime < 0 ? 0 : movementTime - (deltaTime * 2);
                        }

                        if (movementTime > movementTimeThreshold && !HasEffect(Buffs.Triplecast) && !HasEffect(All.Buffs.Swiftcast))
                        {
                            if (InCombat() && LocalPlayer.CurrentCastTime == 0.0f)
                            {
                                if (HasEffect(Buffs.Thundercloud))
                                {
                                    if (!ThunderList.ContainsKey(lastComboMove))
                                    {
                                        uint dot = OriginalHook(Thunder2); //Grab the appropriate DoT Action
                                        return dot;
                                    }
                                }
                                if (IsOffCooldown(All.Swiftcast))
                                {
                                    return All.Swiftcast;
                                }
                                if (ActionReady(Triplecast))
                                {
                                    return Triplecast;
                                }
                            }
                        }
                    }


                    var currentMP = LocalPlayer.CurrentMp;
                    var polyToStore = PluginConfiguration.GetCustomIntValue(Config.BLM_PolyglotsStored);

                    if (IsEnabled(CustomComboPreset.BLM_Variant_Cure) && IsEnabled(Variant.VariantCure) && PlayerHealthPercentageHp() <= GetOptionValue(Config.BLM_VariantCure))
                        return Variant.VariantCure;

                    if (IsEnabled(CustomComboPreset.BLM_Variant_Rampart) &&
                        IsEnabled(Variant.VariantRampart) &&
                        IsOffCooldown(Variant.VariantRampart) &&
                        CanSpellWeave(actionID))
                        return Variant.VariantRampart;

                    // Polyglot usage
                    if (IsEnabled(CustomComboPreset.BLM_AoE_Simple_Foul) && LevelChecked(Manafont) && LevelChecked(Foul))
                    {
                        if (Gauge.InAstralFire && currentMP <= MP.FireAoE && IsOffCooldown(Manafont) && CanSpellWeave(actionID) && lastComboMove == Foul)
                        {
                            return Manafont;
                        }

                        if ((Gauge.InAstralFire && currentMP <= MP.FireAoE && IsOffCooldown(Manafont) && Gauge.HasPolyglotStacks()) || (IsOnCooldown(Manafont) && (GetCooldownRemainingTime(Manafont) >= 30 && Gauge.PolyglotStacks > polyToStore)))
                        {
                            return Foul;
                        }
                    }

                    // Thunder uptime
                    if (currentMP >= MP.ThunderAoE && lastComboMove != Manafont)
                    {
                        uint thunderST = OriginalHook(Thunder);   //Grab whichever Thunder player can use
                        uint thunderAOE = OriginalHook(Thunder2); //Grab whichever Thunder AoE player can use
                        if (ThunderList.TryGetValue(thunderAOE, out ushort dotDebuffID))
                        {
                            var thunderAOEDebuff = TargetHasEffect(dotDebuffID);
                            var thunderAOETimer = FindTargetEffect(dotDebuffID);

                            if (LevelChecked(thunderAOE))
                            {
                                if (lastComboMove != thunderST && lastComboMove != thunderAOE && (!thunderAOEDebuff || thunderAOETimer.RemainingTime <= 4) &&
                                   ((Gauge.InUmbralIce && Gauge.UmbralHearts == 3) ||
                                    (Gauge.InAstralFire && !HasEffect(Buffs.Triplecast) && !HasEffect(All.Buffs.Swiftcast))))
                                {
                                    return thunderAOE;
                                }
                            }
                        }
                    }

                    // Fire 2 / Flare
                    if (Gauge.InAstralFire)
                    {
                        //Grab Fire 2 / High Fire 2 action ID
                        uint fireAoEID = OriginalHook(Fire2);
                        if (currentMP >= 7000)
                        {
                            if (Gauge.UmbralHearts == 1)
                            {
                                return Flare;
                            }
                            return fireAoEID;
                        }
                        else if (currentMP >= MP.Despair)
                        {
                            if (LevelChecked(Flare))
                            {
                                return Flare;
                            }
                            else if (currentMP >= MP.FireAoE)
                            {
                                return fireAoEID;
                            }
                        }
                        else if (!TraitLevelChecked(Traits.AspectMasteryIII))
                        {
                            return Transpose;
                        }
                    }

                    // Umbral Hearts
                    if (Gauge.InUmbralIce)
                    {
                        if (TraitLevelChecked(Traits.EnhancedFreeze) && Gauge.UmbralHearts <= 2)
                        {
                            return Freeze;
                        }
                        else if (LevelChecked(Freeze) && currentMP < (MP.MaxMP - MP.ThunderAoE))
                        {
                            return Freeze;
                        }
                        if (!TraitLevelChecked(Traits.AspectMasteryIII))
                        {
                            return (currentMP >= (MP.MaxMP - MP.ThunderAoE)) ? Transpose : Blizzard2;
                        }
                        if (LevelChecked(Fire2)) return OriginalHook(Fire2);
                    }

                    if (LevelChecked(Blizzard2)) return OriginalHook(Blizzard2);
                }

                return actionID;
            }
        }

        internal class BLM_SimpleMode : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BLM_SimpleMode;

            internal static bool inOpener = false;
            internal static bool openerFinished = false;
            internal static double movementTime = 0.0f;
            internal static DateTime previousTime;

            internal delegate bool DotRecast(int value);

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID is Scathe)
                {
                    var canWeave = CanSpellWeave(actionID);
                    var currentMP = LocalPlayer.CurrentMp;
                    var astralFireRefresh = PluginConfiguration.GetCustomFloatValue(Config.BLM_AstralFireRefresh) * 1000;

                    //var thunder = TargetHasEffect(Debuffs.Thunder);
                    var thunder3 = TargetHasEffect(Debuffs.Thunder3);
                    //var thunderDuration = FindTargetEffect(Debuffs.Thunder);
                    var thunder3Duration = FindTargetEffect(Debuffs.Thunder3);

                    //DotRecast thunderRecast = delegate (int duration)
                    //{
                    //    return !thunder || (thunder && thunderDuration.RemainingTime < duration);
                    //};
                    DotRecast thunder3Recast = delegate (int duration)
                    {
                        return !thunder3 || (thunder3 && thunder3Duration.RemainingTime < duration);
                    };


                    if (IsEnabled(CustomComboPreset.BLM_Variant_Cure) && IsEnabled(Variant.VariantCure) && PlayerHealthPercentageHp() <= GetOptionValue(Config.BLM_VariantCure))
                        return Variant.VariantCure;

                    if (IsEnabled(CustomComboPreset.BLM_Variant_Rampart) &&
                        IsEnabled(Variant.VariantRampart) &&
                        IsOffCooldown(Variant.VariantRampart) &&
                        CanSpellWeave(actionID))
                        return Variant.VariantRampart;

                    // Opener for BLM
                    // Credit to damolitionn for providing code to be used as a base for this opener
                    if (IsEnabled(CustomComboPreset.BLM_Simple_Opener) && LevelChecked(Foul))
                    {
                        // Only enable sharpcast if it's available
                        if (!inOpener && !HasEffect(Buffs.Sharpcast) && HasCharges(Sharpcast) && lastComboMove != Thunder3)
                        {
                            return Sharpcast;
                        }

                        if (!InCombat() && (inOpener || openerFinished))
                        {
                            inOpener = false;
                            openerFinished = false;
                        }

                        if (InCombat() && !inOpener)
                        {
                            inOpener = true;
                        }

                        if (InCombat() && inOpener && !openerFinished)
                        {
                            // Exit out of opener if Enochian is lost
                            if (!Gauge.IsEnochianActive)
                            {
                                openerFinished = true;
                                return Blizzard3;
                            }

                            if (Gauge.InAstralFire)
                            {
                                // First Triplecast
                                if (lastComboMove != Triplecast && !HasEffect(Buffs.Triplecast) && HasCharges(Triplecast))
                                {
                                    var triplecastMP = 7600;
                                    if (IsEnabled(CustomComboPreset.BLM_Simple_OpenerAlternate))
                                    {
                                        triplecastMP = 6000;
                                    }
                                    if (currentMP <= triplecastMP && GetRemainingCharges(Triplecast) > 1)
                                    {
                                        return Triplecast;
                                    }
                                }

                                // Weave other oGCDs
                                if (canWeave)
                                {
                                    // Weave Amplifier and Ley Lines
                                    if (currentMP <= 4400)
                                    {
                                        if (ActionReady(Amplifier))
                                        {
                                            return Amplifier;
                                        }
                                        if (ActionReady(LeyLines))
                                        {
                                            return LeyLines;
                                        }
                                    }

                                    // Swiftcast
                                    if (IsOffCooldown(All.Swiftcast) && IsOnCooldown(LeyLines))
                                    {
                                        return All.Swiftcast;
                                    }

                                    // Manafont
                                    if (IsOffCooldown(Manafont) && (lastComboMove == Despair || lastComboMove == Fire))
                                    {
                                        if (LevelChecked(Despair))
                                        {
                                            if (currentMP < MP.Despair)
                                            {
                                                return Manafont;
                                            }
                                        }
                                        else if (currentMP < MP.Fire)
                                        {
                                            return Manafont;
                                        }
                                    }

                                    // Second Triplecast / Sharpcast
                                    if (!IsEnabled(CustomComboPreset.BLM_Simple_OpenerAlternate))
                                    {
                                        if (!HasEffect(Buffs.Triplecast) && !HasEffect(All.Buffs.Swiftcast) && IsOnCooldown(All.Swiftcast) &&
                                            lastComboMove != All.Swiftcast && HasCharges(Triplecast) && currentMP < MP.Fire)
                                        {
                                            return Triplecast;
                                        }

                                        if (!HasEffect(Buffs.Sharpcast) && HasCharges(Sharpcast) && IsOnCooldown(Manafont) &&
                                            lastComboMove == Fire4)
                                        {
                                            return Sharpcast;
                                        }
                                    }
                                }

                                // Cast Despair
                                if (LevelChecked(Despair) && (currentMP < MP.Fire || Gauge.ElementTimeRemaining <= 4000) && currentMP >= MP.Despair)
                                {
                                    return Despair;
                                }

                                // Cast Fire
                                if (!LevelChecked(Despair) && Gauge.ElementTimeRemaining <= 6000 && currentMP >= MP.Fire)
                                {
                                    return Fire;
                                }

                                // Cast Fire 4 after Manafont
                                if (IsOnCooldown(Manafont))
                                {
                                    if ((!TraitLevelChecked(Traits.EnhancedManafont) && GetCooldownRemainingTime(Manafont) >= 179) ||
                                        (TraitLevelChecked(Traits.EnhancedManafont) && GetCooldownRemainingTime(Manafont) >= 119))
                                    {
                                        return Fire4;
                                    }
                                }

                                // Fire4 / Umbral Ice
                                return currentMP >= MP.Fire ? Fire4 : Blizzard3;
                            }

                            if (Gauge.InUmbralIce)
                            {
                                // Dump Polyglot Stacks
                                if (Gauge.HasPolyglotStacks() && Gauge.ElementTimeRemaining >= 6000)
                                {
                                    return LevelChecked(Xenoglossy) ? Xenoglossy : Foul;
                                }
                                if (Gauge.IsParadoxActive && LevelChecked(Paradox))
                                {
                                    return Paradox;
                                }
                                if (Gauge.UmbralHearts < 3 && lastComboMove != Blizzard4)
                                {
                                    return Blizzard4;
                                }

                                // Refresh Thunder3
                                if (HasEffect(Buffs.Thundercloud) && lastComboMove != Thunder3)
                                {
                                    return Thunder3;
                                }

                                openerFinished = true;
                            }
                        }
                    }

                    // Handle movement
                    if (IsEnabled(CustomComboPreset.BLM_Simple_CastMovement) && InCombat())
                    {
                        var movementTimeThreshold = PluginConfiguration.GetCustomFloatValue(Config.BLM_MovementTime);
                        double deltaTime = (DateTime.Now - previousTime).TotalSeconds;
                        previousTime = DateTime.Now;
                        if (IsMoving)
                        {
                            movementTime = movementTime + deltaTime > movementTimeThreshold + 0.02 ? movementTimeThreshold + 0.02 : movementTime + deltaTime;
                        }
                        else
                        {
                            movementTime = movementTime - deltaTime < 0 ? 0 : movementTime - (deltaTime * 2);
                        }

                        if (movementTime > movementTimeThreshold && !HasEffect(Buffs.Triplecast) && !HasEffect(All.Buffs.Swiftcast))
                        {
                            if (InCombat() && LocalPlayer.CurrentCastTime == 0.0f)
                            {
                                if (LevelChecked(Paradox) && Gauge.IsParadoxActive && Gauge.InUmbralIce)
                                {
                                    return Paradox;
                                }
                                if (IsEnabled(CustomComboPreset.BLM_Simple_CastMovement_Xeno) && LevelChecked(Xenoglossy) && Gauge.HasPolyglotStacks())
                                {
                                    return Xenoglossy;
                                }
                                if (HasEffect(Buffs.Thundercloud))
                                {
                                    if (!ThunderList.ContainsKey(lastComboMove) && //Is not 1 2 3 or 4
                                        !TargetHasEffect(Debuffs.Thunder2) && !TargetHasEffect(Debuffs.Thunder4))
                                    {
                                        uint dot = OriginalHook(Thunder); //Grab the appropriate DoT Action
                                        Status? dotDebuff = FindTargetEffect(ThunderList[dot]); //Match it with it's Debuff ID, and check for the Debuff

                                        if (dotDebuff is null || dotDebuff?.RemainingTime <= 4)
                                            return dot; //Use appropriate DoT Action
                                    }
                                }
                                if (IsOffCooldown(All.Swiftcast))
                                {
                                    return All.Swiftcast;
                                }
                                if (ActionReady(Triplecast))
                                {
                                    return Triplecast;
                                }
                                if (HasEffect(Buffs.Firestarter) && Gauge.InAstralFire)
                                {
                                    return Fire3;
                                }
                                if (IsEnabled(CustomComboPreset.BLM_Simple_CastMovement_Scathe))
                                {
                                    return Scathe;
                                }
                            }
                        }
                    }

                    // Handle thunder uptime and buffs
                    if (Gauge.ElementTimeRemaining > 0)
                    {
                        // Thunder uptime
                        if (IsEnabled(CustomComboPreset.BLM_Thunder) && Gauge.ElementTimeRemaining >= astralFireRefresh)
                        {
                            if (!ThunderList.ContainsKey(lastComboMove) &&
                                !TargetHasEffect(Debuffs.Thunder2) && !TargetHasEffect(Debuffs.Thunder4))
                            {
                                if (HasEffect(Buffs.Thundercloud) || (IsEnabled(CustomComboPreset.BLM_ThunderUptime) && currentMP >= MP.Thunder))
                                {
                                    uint dot = OriginalHook(Thunder); //Grab the appropriate DoT Action
                                    Status? dotDebuff = FindTargetEffect(ThunderList[dot]); //Match it with it's Debuff ID, and check for the Debuff

                                    if (dotDebuff is null || dotDebuff?.RemainingTime <= 3)
                                        return dot; //Use appropriate DoT Action
                                }
                            }
                        }

                        // Buffs
                        if (canWeave)
                        {
                            if (IsEnabled(CustomComboPreset.BLM_Simple_Casts))
                            {
                                // Use Triplecast only with Astral Fire/Umbral Hearts, and we have enough MP to cast Fire IV twice
                                if (ActionReady(Triplecast) && !HasEffect(Buffs.Triplecast) &&
                                    (Gauge.InAstralFire || Gauge.UmbralHearts == 3) && currentMP >= MP.Fire * 2)
                                {
                                    if (!IsEnabled(CustomComboPreset.BLM_Simple_Casts_Pooling) || GetRemainingCharges(Triplecast) > 1)
                                    {
                                        return Triplecast;
                                    }
                                }

                                // Use Swiftcast in Astral Fire
                                if (!IsEnabled(CustomComboPreset.BLM_Simple_Casts_Pooling) && ActionReady(All.Swiftcast) &&
                                     Gauge.InAstralFire && currentMP >= MP.Fire * (HasEffect(Buffs.Triplecast) ? 3 : 1))
                                {
                                    if (LevelChecked(Despair) && currentMP >= MP.Despair)
                                    {
                                        return All.Swiftcast;
                                    }
                                    else if (currentMP >= MP.Fire)
                                    {
                                        return All.Swiftcast;
                                    }
                                }
                            }

                            if (IsEnabled(CustomComboPreset.BLM_Simple_Buffs))
                            {
                                if (ActionReady(Amplifier) && Gauge.PolyglotStacks < 2)
                                {
                                    return Amplifier;
                                }
                            }

                            if (IsEnabled(CustomComboPreset.BLM_Simple_Buffs_LeyLines))
                            {
                                if (ActionReady(LeyLines))
                                {
                                    return LeyLines;
                                }
                            }

                            if (IsEnabled(CustomComboPreset.BLM_Simple_Buffs))
                            {
                                if (IsOffCooldown(Manafont) && Gauge.InAstralFire)
                                {
                                    if (LevelChecked(Despair))
                                    {
                                        if (currentMP < MP.Despair)
                                        {
                                            return Manafont;
                                        }
                                    }
                                    else if (currentMP < MP.Fire)
                                    {
                                        return Manafont;
                                    }
                                }
                                if (ActionReady(Sharpcast) && lastComboMove != Thunder3 && !HasEffect(Buffs.Sharpcast))
                                {
                                    // Try to only sharpcast Thunder 3
                                    if (thunder3Recast(7) || GetRemainingCharges(Sharpcast) == 2 ||
                                       (thunder3Recast(15) && (Gauge.InUmbralIce || (Gauge.InAstralFire && !Gauge.IsParadoxActive))))
                                    {
                                        return Sharpcast;
                                    }
                                }
                            }
                        }
                    }

                    // 20220906 Cleanup Note, could use OriginalHook

                    // Handle initial cast
                    if ((LevelChecked(Blizzard4) && !Gauge.IsEnochianActive) || Gauge.ElementTimeRemaining <= 0)
                    {
                        if (LevelChecked(Fire3))
                        {
                            return (currentMP >= MP.Fire3) ? Fire3 : Blizzard3;
                        }
                        return (currentMP >= MP.Fire) ? Fire : Blizzard;
                    }

                    // Before Blizzard 3; Fire until 0 MP, then Blizzard until max MP.
                    if (!LevelChecked(Blizzard3))
                    {
                        if (Gauge.InAstralFire)
                        {
                            return (currentMP < MP.Fire) ? Transpose : Fire;
                        }
                        if (Gauge.InUmbralIce)
                        {
                            return (currentMP >= MP.MaxMP - MP.Thunder) ? Transpose : Blizzard;
                        }
                    }

                    // Before Fire4; Fire until 0 MP (w/ Firestarter), then Blizzard 3 and Blizzard/Blizzard4 until max MP.
                    if (!LevelChecked(Fire4))
                    {
                        if (Gauge.InAstralFire)
                        {
                            if (HasEffect(Buffs.Firestarter))
                            {
                                return Fire3;
                            }
                            return (currentMP < MP.Fire) ? Blizzard3 : Fire;
                        }
                        if (Gauge.InUmbralIce)
                        {
                            if (LevelChecked(Blizzard4) && Gauge.UmbralHearts < 3)
                            {
                                return Blizzard4;
                            }
                            return (currentMP >= MP.MaxMP || Gauge.UmbralHearts == 3) ? Fire3 : Blizzard;
                        }
                    }

                    // Use polyglot stacks if we don't need it for a future weave
                    if (Gauge.HasPolyglotStacks() && Gauge.ElementTimeRemaining >= astralFireRefresh && (Gauge.InUmbralIce || (Gauge.InAstralFire && Gauge.UmbralHearts == 0)))
                    {
                        if (LevelChecked(Xenoglossy))
                        {
                            // Check leylines and triplecast cooldown
                            if (Gauge.PolyglotStacks == 2 && GetCooldown(LeyLines).CooldownRemaining >= 20 && GetCooldown(Triplecast).ChargeCooldownRemaining >= 20 && !thunder3Recast(15))
                            {
                                if (!IsEnabled(CustomComboPreset.BLM_Simple_Casts_Pooling))
                                {
                                    return Xenoglossy;
                                }
                                if (IsEnabled(CustomComboPreset.BLM_Simple_Casts_Pooling) && !HasCharges(Triplecast))
                                {
                                    return Xenoglossy;
                                }
                            }
                        }
                        else if (LevelChecked(Foul))
                        {
                            return Foul;
                        }
                    }

                    if (Gauge.InAstralFire)
                    {
                        // Refresh AF
                        if (Gauge.ElementTimeRemaining <= 3000 && HasEffect(Buffs.Firestarter))
                        {
                            return Fire3;
                        }
                        if (Gauge.ElementTimeRemaining <= astralFireRefresh && !HasEffect(Buffs.Firestarter) && currentMP >= MP.Fire)
                        {
                            if (LevelChecked(Paradox))
                            {
                                return Gauge.IsParadoxActive ? Paradox : Despair;
                            }
                            return Fire;
                        }

                        // Use Xenoglossy if Amplifier/Triplecast/Leylines/Manafont is available to weave
                        if (lastComboMove != Xenoglossy && Gauge.HasPolyglotStacks() && LevelChecked(Xenoglossy) && Gauge.ElementTimeRemaining >= astralFireRefresh)
                        {
                            var pooledPolyglotStacks = IsEnabled(CustomComboPreset.BLM_Simple_XenoPooling) ? 1 : 0;
                            if (IsEnabled(CustomComboPreset.BLM_Simple_Buffs) && ActionReady(Amplifier))
                            {
                                return Xenoglossy;
                            }
                            if (Gauge.PolyglotStacks > pooledPolyglotStacks)
                            {
                                if (IsEnabled(CustomComboPreset.BLM_Simple_Buffs_LeyLines))
                                {
                                    if (ActionReady(LeyLines))
                                    {
                                        return Xenoglossy;
                                    }
                                }
                                if (IsEnabled(CustomComboPreset.BLM_Simple_Buffs))
                                {
                                    if (ActionReady(Triplecast) && !HasEffect(Buffs.Triplecast) &&
                                        (!IsEnabled(CustomComboPreset.BLM_Simple_Casts_Pooling) || GetRemainingCharges(Triplecast) > 1))
                                    {
                                        return Xenoglossy;
                                    }
                                    if (ActionReady(Manafont) && currentMP < MP.Despair)
                                    {
                                        return Xenoglossy;
                                    }
                                    if (ActionReady(Sharpcast) && !HasEffect(Buffs.Sharpcast) &&
                                        thunder3Recast(15) && lastComboMove != Thunder3 && Gauge.InAstralFire && !Gauge.IsParadoxActive)
                                    {
                                        return Xenoglossy;
                                    }
                                }
                            }
                        }

                        // Cast Fire 4 after Manafont
                        if (IsOnCooldown(Manafont))
                        {
                            if ((!TraitLevelChecked(Traits.EnhancedManafont) && GetCooldownRemainingTime(Manafont) >= 179) ||
                                (TraitLevelChecked(Traits.EnhancedManafont) && GetCooldownRemainingTime(Manafont) >= 119))
                            {
                                return Fire4;
                            }
                        }

                        // Blizzard3/Despair when below Fire 4 + Despair MP
                        if (currentMP < (MP.Fire + MP.Despair))
                        {
                            return (LevelChecked(Despair) && currentMP >= MP.Despair) ? Despair : Blizzard3;
                        }

                        return Fire4;
                    }

                    if (Gauge.InUmbralIce)
                    {
                        // Use Paradox when available
                        if (LevelChecked(Paradox) && Gauge.IsParadoxActive)
                        {
                            return Paradox;
                        }

                        // Fire3 when at max umbral hearts
                        return (Gauge.UmbralHearts == 3 && currentMP >= MP.MaxMP - MP.Thunder) ? Fire3 : Blizzard4;
                    }
                }

                return actionID;
            }
        }

        internal class BLM_Simple_Transpose : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BLM_Simple_Transpose;

            internal static bool inOpener = false;
            internal static bool openerFinished = false;

            internal delegate bool DotRecast(int value);

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID is Scathe)
                {
                    var canWeave = CanSpellWeave(actionID);
                    var canDelayedWeave = CanWeave(actionID, 0.0) && GetCooldown(actionID).CooldownRemaining < 0.7;
                    var currentMP = LocalPlayer.CurrentMp;
                    var astralFireRefresh = PluginConfiguration.GetCustomFloatValue(Config.BLM_AstralFireRefresh) * 1000;
                    var thunder3 = TargetHasEffect(Debuffs.Thunder3);
                    var thunder3Duration = FindTargetEffect(Debuffs.Thunder3);

                    DotRecast thunder3Recast = delegate (int duration)
                    {
                        return !thunder3 || (thunder3 && thunder3Duration.RemainingTime < duration);
                    };

                    // Only enable sharpcast if it's available
                    if (!inOpener && !HasEffect(Buffs.Sharpcast) && HasCharges(Sharpcast) && lastComboMove != Thunder3)
                    {
                        return Sharpcast;
                    }

                    if (!InCombat() && (inOpener || openerFinished))
                    {
                        inOpener = false;
                        openerFinished = false;
                    }

                    if (InCombat() && !inOpener)
                    {
                        inOpener = true;
                    }

                    if (InCombat() && inOpener && !openerFinished)
                    {
                        // Exit out of opener if Enochian is lost
                        if (!Gauge.IsEnochianActive)
                        {
                            openerFinished = true;
                            return Blizzard3;
                        }

                        if (Gauge.InAstralFire)
                        {
                            // First Triplecast
                            if (lastComboMove != Triplecast && !HasEffect(Buffs.Triplecast) && HasCharges(Triplecast))
                            {
                                if (currentMP <= 6000)
                                {
                                    return Triplecast;
                                }
                            }

                            // Weave other oGCDs
                            if (canWeave)
                            {
                                // Manafont
                                if (IsOffCooldown(Manafont) && lastComboMove == Despair)
                                {
                                    if (currentMP < MP.Despair)
                                    {
                                        return Manafont;
                                    }
                                }

                                // Weave Amplifier and Ley Lines
                                if (currentMP <= 2800)
                                {
                                    if (IsOffCooldown(Amplifier))
                                    {
                                        return Amplifier;
                                    }
                                    if (IsOffCooldown(LeyLines))
                                    {
                                        return LeyLines;
                                    }
                                }

                                if (IsOnCooldown(LeyLines))
                                {
                                    // Swiftcast
                                    if (IsOffCooldown(All.Swiftcast))
                                    {
                                        return All.Swiftcast;
                                    }

                                    // Sharpcast
                                    if (!HasEffect(Buffs.Sharpcast) && HasCharges(Sharpcast) && IsOnCooldown(LeyLines))
                                    {
                                        return Sharpcast;
                                    }
                                }

                                // Second Triplecast
                                if (!HasEffect(Buffs.Triplecast) && !HasEffect(All.Buffs.Swiftcast) && IsOnCooldown(All.Swiftcast) &&
                                    lastComboMove != All.Swiftcast && HasCharges(Triplecast) && currentMP < 6000)
                                {
                                    return Triplecast;
                                }

                                // Lucid Dreaming
                                if (!HasCharges(Triplecast) && IsOffCooldown(All.LucidDreaming))
                                {
                                    return All.LucidDreaming;
                                }
                            }

                            // Cast Despair
                            if (currentMP < MP.Fire && currentMP >= MP.Despair)
                            {
                                return Despair;
                            }

                            // Cast Fire 4 after Manafont
                            if (IsOnCooldown(Manafont) && GetCooldownRemainingTime(Manafont) >= 119)
                            {
                                return Fire4;
                            }

                            return currentMP >= MP.Fire ? Fire4 : Transpose;
                        }

                        if (Gauge.InUmbralIce)
                        {
                            if (Gauge.IsParadoxActive)
                            {
                                return Paradox;
                            }
                            if (Gauge.HasPolyglotStacks() && lastComboMove != Xenoglossy)
                            {
                                return Xenoglossy;
                            }
                            if (HasEffect(Buffs.Thundercloud) && lastComboMove != Thunder3)
                            {
                                return Thunder3;
                            }
                            openerFinished = true;
                        }
                    }

                    if (Gauge.ElementTimeRemaining == 0 || !Gauge.IsEnochianActive)
                    {
                        if (currentMP >= MP.Fire3)
                        {
                            return Fire3;
                        }
                        return Blizzard3;
                    }

                    if (Gauge.ElementTimeRemaining > 0)
                    {
                        // Thunder
                        if (IsEnabled(CustomComboPreset.BLM_Thunder) && Gauge.ElementTimeRemaining >= astralFireRefresh)
                        {
                            if (!ThunderList.ContainsKey(lastComboMove) &&
                                !TargetHasEffect(Debuffs.Thunder2) && !TargetHasEffect(Debuffs.Thunder4) && thunder3Recast(4))
                            {
                                if (HasEffect(Buffs.Thundercloud) || (IsEnabled(CustomComboPreset.BLM_ThunderUptime) && currentMP >= MP.Thunder))
                                {
                                    return Thunder3;
                                }
                            }
                        }

                        // Buffs
                        if (canWeave)
                        {
                            // Use Triplecast only with Astral Fire/Umbral Hearts, and we have enough MP to cast Fire IV twice
                            if (!HasEffect(Buffs.Triplecast) && HasCharges(Triplecast) &&
                                (Gauge.InAstralFire || Gauge.UmbralHearts >= 1) && currentMP >= MP.Fire * 2)
                            {
                                if (!IsEnabled(CustomComboPreset.BLM_Simple_Transpose_Pooling) || GetRemainingCharges(Triplecast) > 1)
                                {
                                    return Triplecast;
                                }
                            }

                            if (IsOffCooldown(Amplifier) && Gauge.PolyglotStacks < 2)
                            {
                                return Amplifier;
                            }

                            if (IsEnabled(CustomComboPreset.BLM_Simple_Transpose_LeyLines) && IsOffCooldown(LeyLines))
                            {
                                return LeyLines;
                            }

                            if (IsOffCooldown(Manafont) && Gauge.InAstralFire && currentMP < MP.Despair)
                            {
                                return Manafont;
                            }

                            if (HasCharges(Sharpcast) && !HasEffect(Buffs.Sharpcast))
                            {
                                return Sharpcast;
                            }
                        }
                    }

                    if (Gauge.InUmbralIce)
                    {
                        // Standard
                        if (Gauge.UmbralIceStacks == 3)
                        {
                            if (Gauge.PolyglotStacks == 2)
                            {
                                return Xenoglossy;
                            }
                            if (Gauge.IsParadoxActive)
                            {
                                return Paradox;
                            }
                            if (Gauge.UmbralHearts < 3)
                            {
                                return Blizzard4;
                            }
                            return Fire3;
                        }

                        // Transpose Instant F3
                        if (canWeave)
                        {
                            if (!HasEffect(Buffs.Firestarter) && !HasEffect(All.Buffs.Swiftcast) && !HasEffect(Buffs.Triplecast))
                            {
                                if (IsOffCooldown(All.Swiftcast))
                                {
                                    return All.Swiftcast;
                                }
                            }
                            if (IsOffCooldown(All.LucidDreaming))
                            {
                                return All.LucidDreaming;
                            }
                        }

                        // Paradox for Transpose Lines
                        if (Gauge.IsParadoxActive)
                        {
                            return Paradox;
                        }

                        // Filler GCDs
                        if (currentMP <= MP.MaxMP - MP.Fire)
                        {
                            if (lastComboMove != Xenoglossy && Gauge.HasPolyglotStacks())
                            {
                                return Xenoglossy;
                            }
                            if (lastComboMove != Thunder3 && thunder3Recast(7))
                            {
                                return Thunder3;
                            }
                            if (Gauge.HasPolyglotStacks())
                            {
                                return Xenoglossy;
                            }
                        }

                        if (IsOffCooldown(Transpose) && (canDelayedWeave || currentMP >= MP.MaxMP - MP.Fire))
                        {
                            return Transpose;
                        }
                        if (HasEffect(All.Buffs.Swiftcast))
                        {
                            return Fire3;
                        }
                        if (Gauge.HasPolyglotStacks())
                        {
                            return Xenoglossy;
                        }
                        return Blizzard4;
                    }

                    if (Gauge.InAstralFire)
                    {
                        // F3
                        if (Gauge.AstralFireStacks < 3)
                        {
                            return Fire3;
                        }

                        // Xenoglossy for Manafont weave
                        if (Gauge.HasPolyglotStacks() && IsOffCooldown(Manafont) && currentMP < MP.Despair)
                        {
                            return Xenoglossy;
                        }

                        // Early Despair
                        if (currentMP < (MP.Fire + MP.Despair) && currentMP >= MP.Despair)
                        {
                            return Despair;
                        }

                        // Cast Fire 4 after Manafont
                        if (IsOnCooldown(Manafont) && GetCooldownRemainingTime(Manafont) >= 119)
                        {
                            return Fire4;
                        }

                        // Transpose if F3 is available, or Thundercloud + Xenoglossy is available
                        if (currentMP < MP.Fire && lastComboMove != Manafont && IsOnCooldown(Manafont) && GetCooldownRemainingTime(Manafont) <= 118)
                        {
                            if ((HasEffect(Buffs.LeyLines) && GetBuffRemainingTime(Buffs.LeyLines) >= 15) || HasEffect(Buffs.Firestarter) ||
                                 lastComboMove == Xenoglossy || lastComboMove == Thunder3 || (IsOffCooldown(All.Swiftcast) && (Gauge.PolyglotStacks == 2)))
                            {
                                if (lastComboMove != Despair && lastComboMove != Fire4)
                                {
                                    return Transpose;
                                }
                                if (lastComboMove == Despair)
                                {
                                    if (Gauge.HasPolyglotStacks())
                                    {
                                        return Xenoglossy;
                                    }
                                    if (HasEffect(Buffs.Thundercloud))
                                    {
                                        return Thunder3;
                                    }
                                }
                            }
                        }

                        // Regular Despair / Paradox
                        if (Gauge.ElementTimeRemaining <= astralFireRefresh)
                        {
                            return !Gauge.IsParadoxActive ? Despair : Paradox;
                        }
                        if (currentMP >= MP.Fire)
                        {
                            return Fire4;
                        }
                        return Blizzard3;
                    }
                }

                return actionID;
            }
        }

        internal class BLM_Variant_Raise : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BLM_Variant_Raise;

            protected override uint Invoke(uint actionID, uint lastComboActionID, float comboTime, byte level)
            {
                if (actionID is All.Swiftcast)
                {
                    if (HasEffect(All.Buffs.Swiftcast) && IsEnabled(Variant.VariantRaise))
                        return Variant.VariantRaise;
                }

                return actionID;
            }
        }

        internal class BLM_Paradox : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BLM_Paradox;

            internal static bool inOpener = false;
            internal static bool openerFinished = false;

            internal delegate bool DotRecast(int value);

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID is Scathe)
                {
                    var canWeave = CanSpellWeave(actionID);
                    var currentMP = LocalPlayer.CurrentMp;
                    var thunder3 = TargetHasEffect(Debuffs.Thunder3);
                    var thunder3Duration = FindTargetEffect(Debuffs.Thunder3);

                    DotRecast thunder3Recast = delegate (int duration)
                    {
                        return !thunder3 || (thunder3 && thunder3Duration.RemainingTime < duration);
                    };

                    // Only enable sharpcast if it's available
                    if (!inOpener && !HasEffect(Buffs.Sharpcast) && HasCharges(Sharpcast) && lastComboMove != Thunder3)
                    {
                        return Sharpcast;
                    }

                    if (!InCombat() && (inOpener || openerFinished))
                    {
                        inOpener = false;
                        openerFinished = false;
                    }

                    if (InCombat() && !inOpener)
                    {
                        inOpener = true;
                    }

                    if (InCombat() && inOpener && !openerFinished)
                    {
                        if (InCombat() && inOpener && !openerFinished)
                        {
                            // Exit out of opener if Enochian is lost
                            if (!Gauge.IsEnochianActive)
                            {
                                openerFinished = true;
                                return Blizzard3;
                            }

                            if (Gauge.InAstralFire)
                            {
                                // First Triplecast
                                if (lastComboMove != Triplecast && !HasEffect(Buffs.Triplecast) && HasCharges(Triplecast))
                                {
                                    var triplecastMP = 7600;
                                    if (currentMP <= triplecastMP)
                                    {
                                        return Triplecast;
                                    }
                                }

                                // Weave other oGCDs
                                if (canWeave)
                                {
                                    // Weave Amplifier and Ley Lines
                                    if (currentMP <= 4400)
                                    {
                                        if (IsOffCooldown(Amplifier))
                                        {
                                            return Amplifier;
                                        }
                                        if (IsOffCooldown(LeyLines))
                                        {
                                            return LeyLines;
                                        }
                                    }

                                    // Swiftcast
                                    if (IsOffCooldown(All.Swiftcast) && IsOnCooldown(LeyLines))
                                    {
                                        return All.Swiftcast;
                                    }

                                    // Manafont
                                    if (IsOffCooldown(Manafont) && lastComboMove == Despair)
                                    {
                                        if (currentMP < MP.Despair)
                                        {
                                            return Manafont;
                                        }
                                    }

                                    // Second Triplecast / Sharpcast
                                    if (!IsEnabled(CustomComboPreset.BLM_Simple_OpenerAlternate))
                                    {
                                        if (!HasEffect(Buffs.Triplecast) && !HasEffect(All.Buffs.Swiftcast) && IsOnCooldown(All.Swiftcast) &&
                                            lastComboMove != All.Swiftcast && HasCharges(Triplecast) && currentMP < MP.Fire)
                                        {
                                            return Triplecast;
                                        }

                                        if (!HasEffect(Buffs.Sharpcast) && HasCharges(Sharpcast) && IsOnCooldown(Manafont) &&
                                            lastComboMove == Fire4)
                                        {
                                            return Sharpcast;
                                        }
                                    }
                                }

                                // Cast Despair
                                if ((currentMP < MP.Fire || Gauge.ElementTimeRemaining <= 4000) && currentMP >= MP.Despair)
                                {
                                    return Despair;
                                }

                                // Cast Fire 4 after Manafont
                                if (IsOnCooldown(Manafont) && GetCooldownRemainingTime(Manafont) >= 119)
                                {
                                    return Fire4;
                                }

                                // Fire4 / Umbral Ice
                                return currentMP >= MP.Fire ? Fire4 : Blizzard3;
                            }

                            if (Gauge.InUmbralIce)
                            {
                                // Dump Polyglot Stacks
                                if (Gauge.HasPolyglotStacks() && Gauge.ElementTimeRemaining >= 6000)
                                {
                                    return Xenoglossy;
                                }
                                if (Gauge.IsParadoxActive && LevelChecked(Paradox))
                                {
                                    return Paradox;
                                }
                                if (Gauge.UmbralHearts < 3 && lastComboMove != Blizzard4)
                                {
                                    return Blizzard4;
                                }

                                // Refresh Thunder3
                                if (HasEffect(Buffs.Thundercloud) && lastComboMove != Thunder3)
                                {
                                    return Thunder3;
                                }

                                openerFinished = true;
                            }
                        }
                    }

                    if (Gauge.ElementTimeRemaining == 0 || !Gauge.IsEnochianActive)
                    {
                        if (currentMP >= MP.Fire3)
                        {
                            return Fire3;
                        }
                        return Blizzard3;
                    }

                    if (Gauge.ElementTimeRemaining > 0)
                    {
                        // Thunder
                        if (lastComboMove != Thunder3 && currentMP >= MP.Thunder &&
                            thunder3Recast(4) && !TargetHasEffect(Debuffs.Thunder2) && !TargetHasEffect(Debuffs.Thunder4))
                        {
                            return Thunder3;
                        }

                        // Buffs
                        if (canWeave)
                        {
                            if (!HasEffect(Buffs.Triplecast) && HasCharges(Triplecast))
                            {
                                return Triplecast;
                            }

                            if (IsOffCooldown(Amplifier) && Gauge.PolyglotStacks < 2)
                            {
                                return Amplifier;
                            }

                            if (IsEnabled(CustomComboPreset.BLM_Paradox_LeyLines) && IsOffCooldown(LeyLines))
                            {
                                return LeyLines;
                            }

                            if (IsOffCooldown(Manafont) && Gauge.InAstralFire && currentMP < MP.Despair)
                            {
                                return Manafont;
                            }

                            if (IsOffCooldown(All.Swiftcast))
                            {
                                return All.Swiftcast;
                            }

                            if (HasCharges(Sharpcast) && !HasEffect(Buffs.Sharpcast))
                            {
                                return Sharpcast;
                            }
                        }
                    }

                    // Play standard while inside of leylines
                    if (HasEffect(Buffs.LeyLines))
                    {
                        if (Gauge.InAstralFire)
                        {
                            if (Gauge.ElementTimeRemaining <= 3000 && HasEffect(Buffs.Firestarter))
                            {
                                return Fire3;
                            }
                            if (Gauge.ElementTimeRemaining <= 6000 && !HasEffect(Buffs.Firestarter) && currentMP >= MP.Fire)
                            {
                                return Gauge.IsParadoxActive ? Paradox : Despair;
                            }
                            return (currentMP >= MP.Fire + MP.Despair) ? Fire4 : (currentMP >= MP.Despair ? Despair : Blizzard3);
                        }

                        if (Gauge.InUmbralIce)
                        {
                            if (Gauge.PolyglotStacks == 2)
                            {
                                return Xenoglossy;
                            }
                            return Gauge.IsParadoxActive ? Paradox : (Gauge.UmbralHearts == 3 ? Fire3 : Blizzard4);
                        }
                    }

                    if (Gauge.InUmbralIce)
                    {
                        if (Gauge.IsParadoxActive)
                        {
                            return Paradox;
                        }
                        if (currentMP >= MP.Despair && (HasEffect(Buffs.Firestarter) || HasEffect(Buffs.Triplecast) || HasEffect(All.Buffs.Swiftcast)))
                        {
                            return Fire3;
                        }
                        if (Gauge.UmbralIceStacks < 3)
                        {
                            return UmbralSoul;
                        }
                        if (IsOffCooldown(Transpose))
                        {
                            return Transpose;
                        }
                    }

                    if (Gauge.InAstralFire)
                    {
                        if (Gauge.AstralFireStacks < 3 && HasEffect(Buffs.Firestarter) && !HasEffect(Buffs.Triplecast) && !HasEffect(All.Buffs.Swiftcast))
                        {
                            return Fire3;
                        }

                        // Cast Despair after Manafont
                        if (IsOnCooldown(Manafont) && GetCooldownRemainingTime(Manafont) >= 119)
                        {
                            return Despair;
                        }

                        if (HasEffect(Buffs.Triplecast) || HasEffect(All.Buffs.Swiftcast) || HasEffect(Buffs.Sharpcast))
                        {
                            if (!HasEffect(Buffs.Firestarter) && currentMP >= MP.Fire)
                            {
                                if (Gauge.IsParadoxActive)
                                {
                                    return Paradox;
                                }
                                if (!HasEffect(Buffs.Triplecast) && !HasEffect(All.Buffs.Swiftcast))
                                {
                                    return Fire;
                                }
                            }
                            if (currentMP >= MP.Despair)
                            {
                                return Despair;
                            }
                        }
                        if (IsOffCooldown(Transpose) && openerFinished)
                        {
                            return Transpose;
                        }
                    }

                    if (Gauge.ElementTimeRemaining > 0)
                    {
                        if (Gauge.HasPolyglotStacks())
                        {
                            return Xenoglossy;
                        }
                        if (HasEffect(Buffs.Thundercloud) && lastComboMove != Thunder3)
                        {
                            return Thunder3;
                        }
                        return currentMP <= MP.Despair ? (Gauge.InAstralFire ? Transpose : UmbralSoul) : Scathe;
                    }
                }

                return actionID;
            }
        }
        internal class BLM_ScatheXeno : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BLM_ScatheXeno;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID is Scathe)
                {
                    if (LevelChecked(Xenoglossy) && Gauge.PolyglotStacks > 0)
                        return Xenoglossy;
                }
                return actionID;
            }
        }

        internal class BLM_Zimo : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BLM_Zimo;

            internal static bool fireChance = true;
            internal static bool thunderChance = true;
            internal static int fire4Count = 0;
            internal static uint lastGCD = 0;
            internal static uint lastOGCD = 0;
            internal static bool chant = false;
            internal static string pecialCombo = "";

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID is Scathe)
                {
                    //var currentMP = LocalPlayer.CurrentMp;
                    //Dalamud.Logging.PluginLog.Warning($"{LevelChecked(Triplecast) && Gauge.InAstralFire && GetRemainingCharges(Triplecast) > 1 &&
                    //        FindEffect(Buffs.Triplecast) == null && lastOGCD != Triplecast &&
                    //        (currentMP >= 4000 && LevelChecked(Despair) && Gauge.ElementTimeRemaining > 6000 ||
                    //        currentMP >= 2400 && Gauge.UmbralHearts == 3 && Gauge.ElementTimeRemaining > 10000)}");
                    //return actionID;

                    // ��׼��һЩ����
                    var canWeave = CanSpellWeave(actionID);
                    var currentMP = LocalPlayer.CurrentMp;

                    // ��Ҫ�����ļ���ʱ��
                    // ��ʼ����ʱ��
                    if (comboTime == 0 && !chant)
                    {
                        chant = true;
                        if (lastGCD == Fire4)
                        {
                            fire4Count++;
                        }
                        //if (lastGCD == Fire || lastGCD == Fire3 || lastGCD == Paradox || lastGCD == Despair)
                        //{
                        //    fire4Count = 0;
                        //    var firestarterBuff = FindEffect(Buffs.Firestarter);
                        //    if (firestarterBuff != null && firestarterBuff.RemainingTime > 15000)
                        //    {
                        //        fireChance = true;
                        //    }
                        //}
                        if (lastGCD == Thunder || lastGCD == Thunder3 || lastGCD == Foul)
                        {
                            fireChance = false;
                        }
                        if (lastGCD == Thunder || lastGCD == Thunder3)
                        {
                            thunderChance = false;
                        }
                    }
                    if (comboTime != 0 && chant)
                    {
                        chant = false;
                        thunderChance = true;
                    }
                    // �����ͷ�ʱ��

                    // ����Ҫ�����ļ���ʱ��
                    if (lastComboMove == Thunder || lastComboMove == Thunder3 || lastComboMove == Xenoglossy ||
                    lastComboMove == Foul)
                    {
                        fireChance = false;
                    }
                    if (lastComboMove == Fire || lastComboMove == Fire3 || lastComboMove == Paradox || lastComboMove == Despair)
                    {
                        fire4Count = 0;
                        var firestarterBuff = FindEffect(Buffs.Firestarter);
                        if (firestarterBuff != null && firestarterBuff.RemainingTime > 15000)
                        {
                            fireChance = true;
                        }
                    }

                    // �жϲ�ʹ����������
                    if (pecialCombo == "")
                    {
                        // ����
                        if (LevelChecked(Paradox) && GetRemainingCharges(Triplecast) > 0 && !Gauge.InUmbralIce && !Gauge.InAstralFire &&
                            currentMP == 10000 && GetRemainingCharges(All.Swiftcast) > 0)
                        {
                            pecialCombo = "AtFirst";
                        }
                        // ħȪ��β
                        if (LevelChecked(Xenoglossy) && GetRemainingCharges(Manafont) > 0 && GetRemainingCharges(Triplecast) > 0 && FindEffect(Buffs.Triplecast) == null &&
                            Gauge.InAstralFire && currentMP < MP.Fire && currentMP >= MP.Despair && Gauge.PolyglotStacks > 0 && Gauge.ElementTimeRemaining >= 3000)
                        {
                            pecialCombo = "ManafontWindUp";
                        }
                    }
                    var result = actionID;
                    switch (pecialCombo)
                    {
                        case "":
                            break;
                        case "AtFirst":
                            // ����
                            result = AtFirst(actionID, lastComboMove, comboTime, level, canWeave, currentMP);
                            if (result != actionID)
                            {
                                return result;
                            }
                            break;
                        case "ManafontWindUp":
                            // ħȪ��β
                            result = ManafontWindUp(actionID, lastComboMove, comboTime, level, canWeave, currentMP);
                            if (result != actionID)
                            {
                                return result;
                            }
                            break;
                    }

                    #region ��״̬�ж�
                    // �������׶�
                    if (canWeave)
                    {
                        // ��ħ��
                        if (LevelChecked(LeyLines) && GetRemainingCharges(LeyLines) > 0)
                        {
                            lastOGCD = LeyLines;
                            return LeyLines;
                        }
                        // ����
                        if (LevelChecked(Amplifier) && GetRemainingCharges(Amplifier) > 0 && Gauge.PolyglotStacks < 2 &&
                            !(Gauge.PolyglotStacks == 1 && Gauge.EnochianTimer < 10000 && Gauge.EnochianTimer != 0))
                        {
                            lastOGCD = Amplifier;
                            return Amplifier;
                        }
                        // ����ӽ��
                        if (LevelChecked(Triplecast) && Gauge.InAstralFire && GetRemainingCharges(Triplecast) > 1 &&
                            !HasEffect(Buffs.Triplecast) && lastOGCD != Triplecast &&
                            currentMP >= MP.Fire * 2 + MP.Despair)
                            //(currentMP >= 4000 && LevelChecked(Despair) && Gauge.ElementTimeRemaining > 600cu0 ||
                            //currentMP >= 2400 && Gauge.UmbralHearts == 3 && Gauge.ElementTimeRemaining > 10000))
                        {
                            lastOGCD = Triplecast;
                            return Triplecast;
                        }
                        // ����ӽ��
                        var thunder = OriginalHook(Thunder);
                        var buff = FindTargetEffect(ThunderList[thunder]);
                        if (LevelChecked(Sharpcast) && GetRemainingCharges(Sharpcast) > 0 && !HasEffect(Buffs.Sharpcast) && (buff is null || buff.RemainingTime <= 8))
                        {
                            lastOGCD = Sharpcast;
                            return Sharpcast;
                        }
                        // ����ӽ��2
                        if (LevelChecked(Sharpcast) && GetRemainingCharges(Sharpcast) > 0 && !HasEffect(Buffs.Sharpcast) && GetRemainingCharges(Sharpcast) > 1)
                        {
                            lastOGCD = Sharpcast;
                            return Sharpcast;
                        }
                    }
                    // �ָ��׶�
                    if (!Gauge.InUmbralIce && !Gauge.InAstralFire)
                    {
                        // ����ӽ��
                        if (LevelChecked(Sharpcast) && GetRemainingCharges(Sharpcast) > 0 && !HasEffect(Buffs.Sharpcast))
                        {
                            lastOGCD = Sharpcast;
                            return Sharpcast;
                        }
                        // ��3
                        if (LevelChecked(Fire3) && currentMP >= 6400)
                        {
                            lastGCD = Fire3;
                            return Fire3;
                        }
                        // ��1
                        if (LevelChecked(Fire) && currentMP >= 4000)
                        {
                            lastGCD = Fire;
                            return Fire;
                        }
                        // ��3
                        if (LevelChecked(Blizzard3) && currentMP < 6400)
                        {
                            lastGCD = Blizzard3;
                            return Blizzard3;
                        }
                        // ��1
                        if (LevelChecked(Blizzard) && currentMP < 4000)
                        {
                            lastGCD = Blizzard;
                            return Blizzard;
                        }
                    }
                    // ���׶�
                    if (Gauge.InUmbralIce)
                    {
                        // �ͷſ�Ҫ���ڵ���Դ
                        // ��
                        var thunder = OriginalHook(Thunder);
                        var thundercloudBuff = FindEffect(Buffs.Thundercloud);
                        if (LevelChecked(thunder) && thundercloudBuff != null && thundercloudBuff.RemainingTime <= 4 && thunderChance && lastComboMove != thunder)
                        {
                            lastGCD = thunder;
                            return thunder;
                        }
                        // ����
                        if (LevelChecked(Xenoglossy) && (Gauge.PolyglotStacks == 2 || Gauge.EnochianTimer < 5000 && Gauge.PolyglotStacks == 1))
                        {
                            lastGCD = Xenoglossy;
                            return Xenoglossy;
                        }
                        // ����
                        if (LevelChecked(Foul) && (Gauge.PolyglotStacks == 2 || Gauge.EnochianTimer < 5000 && Gauge.PolyglotStacks == 1))
                        {
                            lastGCD = Foul;
                            return Foul;
                        }

                        // ��buff
                        // ��2
                        var buff = FindTargetEffect(ThunderList[thunder]);
                        if (LevelChecked(thunder) && (buff is null || buff.RemainingTime <= 4) && currentMP >= MP.Thunder && thunderChance && lastComboMove != thunder)
                        {
                            lastGCD = thunder;
                            return thunder;
                        }

                        // ���漼��
                        // ����
                        if (LevelChecked(Blizzard4) && Gauge.UmbralHearts < 3)
                        {
                            lastGCD = Blizzard4;
                            return Blizzard4;
                        }
                        // ���
                        if (LevelChecked(Paradox) && Gauge.IsParadoxActive)
                        {
                            lastGCD = Paradox;
                            return Paradox;
                        }
                        // ����
                        if (LevelChecked(Fire3) && currentMP > 9500)
                        {
                            lastGCD = Fire3;
                            return Fire3;
                        }
                        // ����2
                        if (LevelChecked(Blizzard4))
                        {
                            lastGCD = Blizzard4;
                            return Blizzard4;
                        }

                        // �͵ȼ����漼��
                        // ����
                        if (LevelChecked(Blizzard) && currentMP <= 9500)
                        {
                            lastGCD = Blizzard;
                            return Blizzard;
                        }
                        // ������λ
                        if (LevelChecked(Transpose) && currentMP > 9500)
                        {
                            lastGCD = Transpose;
                            return Transpose;
                        }

                        // �����ϲ��ᵽ��
                        return actionID;
                    }
                    // ��׶�
                    if (Gauge.InAstralFire)
                    {

                        // �ͷſ�Ҫ���ڵ���Դ
                        // ����2
                        var firestarterBuff = FindEffect(Buffs.Firestarter);
                        if (LevelChecked(Fire3) && firestarterBuff != null && firestarterBuff.RemainingTime <= 4)
                        {
                            lastGCD = Fire3;
                            return Fire3;
                        }
                        // ��
                        var thunder = OriginalHook(Thunder);
                        var thundercloudBuff = FindEffect(Buffs.Thundercloud);
                        if (LevelChecked(thunder) && thundercloudBuff != null && thundercloudBuff.RemainingTime <= 4 && thunderChance && fireChance && lastComboMove != thunder)
                        {
                            lastGCD = thunder;
                            return thunder;
                        }
                        // ����
                        if (LevelChecked(Xenoglossy) && Gauge.PolyglotStacks == 2 && fireChance)
                        {
                            lastGCD = Xenoglossy;
                            return Xenoglossy;
                        }
                        // ����
                        if (LevelChecked(Foul) && Gauge.PolyglotStacks == 2 && fireChance)
                        {
                            lastGCD = Xenoglossy;
                            return Foul;
                        }

                        // ��buff
                        // ��2
                        var thunderBuff = FindTargetEffect(ThunderList[thunder]);
                        if (LevelChecked(thunder) && (thunderBuff is null || thunderBuff.RemainingTime <= 4) && currentMP >= MP.Thunder && thunderChance && fireChance && lastComboMove != thunder)
                        {
                            lastGCD = thunder;
                            return thunder;
                        }

                        // �������ﲻ��
                        // ����
                        if (LevelChecked(Fire3) && HasEffect(Buffs.Firestarter) && (Gauge.ElementTimeRemaining < 4000 || fire4Count >= 3))
                        {
                            lastGCD = Fire3;
                            return Fire3;
                        }
                        // ���
                        if (LevelChecked(Paradox) && Gauge.IsParadoxActive && (Gauge.ElementTimeRemaining < 4000 || fire4Count >= 3))
                        {
                            lastGCD = Paradox;
                            return Paradox;
                        }
                        // ����
                        if (LevelChecked(Fire) && currentMP >= MP.Fire && (Gauge.ElementTimeRemaining < 4000 || fire4Count >= 3))
                        {
                            lastGCD = Fire;
                            return Fire;
                        }

                        // ���漼��
                        // ����
                        if (LevelChecked(Fire4) && currentMP >= MP.Fire)
                        {
                            lastGCD = Fire4;
                            return Fire4;
                        }
                        // ����
                        if (LevelChecked(Despair) && currentMP >= MP.Despair)
                        {
                            lastGCD = Despair;
                            return Despair;
                        }
                        // ����
                        if (LevelChecked(Blizzard3) && currentMP < Fire)
                        {
                            lastGCD = Blizzard3;
                            return Blizzard3;
                        }

                        // �͵ȼ����漼��
                        // ����2
                        if (LevelChecked(Fire) && currentMP >= MP.Fire)
                        {
                            lastGCD = Fire;
                            return Fire;
                        }

                        // �����ϲ��ᵽ��
                        return actionID;
                    }
                    #endregion
                }
                return actionID;
            }

            internal static bool ManafontWindUp_Xenoglossy = false;

            protected uint ManafontWindUp(uint actionID, uint lastComboMove, float comboTime, byte level, bool canWeave, float currentMP)
            {
                if (lastComboMove == Xenoglossy)
                {
                    ManafontWindUp_Xenoglossy = true;
                }
                // ����
                if (Gauge.PolyglotStacks > 0 && FindEffect(Buffs.Triplecast) == null && !ManafontWindUp_Xenoglossy)
                {
                    lastGCD = Xenoglossy;
                    return Xenoglossy;
                }
                // ����ӽ��
                if (canWeave && GetRemainingCharges(Triplecast) > 0 && FindEffect(Buffs.Triplecast) == null)
                {
                    lastOGCD = Triplecast;
                    return Triplecast;
                }
                // ����
                if (currentMP >= MP.Fire && FindEffect(Buffs.Triplecast) != null)
                {
                    lastGCD = Fire4;
                    return Fire4;
                }
                // ����
                if (currentMP >= MP.Despair && FindEffect(Buffs.Triplecast) != null)
                {
                    if (GetRemainingCharges(Manafont) == 0)
                    {
                        ManafontWindUp_Xenoglossy = false;
                        pecialCombo = "";
                    }
                    lastGCD = Despair;
                    return Despair;
                }
                // ����2
                if (GetRemainingCharges(Manafont) == 0 && FindEffect(Buffs.Triplecast) != null)
                {
                    lastGCD = Fire4;
                    return Fire4;
                }
                // ħȪ
                if (currentMP == 0 && GetRemainingCharges(Manafont) > 0)
                {
                    lastOGCD = Manafont;
                    return Manafont;
                }
                // ����������
                ManafontWindUp_Xenoglossy = false;
                pecialCombo = "";
                return actionID;
            }

            protected uint AtFirst(uint actionID, uint lastComboMove, float comboTime, byte level, bool canWeave, float currentMP)
            {
                // ����ӽ��
                if (GetRemainingCharges(Sharpcast) > 0 && !HasEffect(Buffs.Sharpcast) && !Gauge.InUmbralIce && !Gauge.InAstralFire)
                {
                    lastOGCD = Sharpcast;
                    return Sharpcast;
                }
                // ����
                if (!Gauge.InUmbralIce && !Gauge.InAstralFire && currentMP > MP.Fire3)
                {
                    lastGCD = Fire3;
                    return Fire3;
                }
                // ����
                if (lastComboMove == Fire3 && currentMP > MP.Thunder)
                {
                    lastGCD = Thunder3;
                    return Thunder3;
                }
                // ����ӽ��
                if (lastComboMove == Fire4 && GetRemainingCharges(Triplecast) > 0 && (currentMP == 7600 || currentMP == 6000) && FindEffect(Buffs.Triplecast) == null)
                {
                    lastOGCD = Triplecast;
                    return Triplecast;
                }
                if (canWeave)
                {
                    // ����
                    if (GetRemainingCharges(Amplifier) > 0 && Gauge.PolyglotStacks < 2 &&
                        !(Gauge.PolyglotStacks == 1 && Gauge.EnochianTimer < 10000 && Gauge.EnochianTimer != 0))
                    {
                        lastOGCD = Amplifier;
                        return Amplifier;
                    }
                    // ��ħ��
                    if (GetRemainingCharges(LeyLines) > 0)
                    {
                        lastOGCD = LeyLines;
                        return LeyLines;
                    }
                    // ����ӽ��
                    if (FindEffect(Buffs.Triplecast) == null && GetRemainingCharges(All.Swiftcast) > 0)
                    {
                        lastOGCD = All.Swiftcast;
                        return All.Swiftcast;
                    }
                }
                // ����
                if (currentMP >= MP.Fire)
                {
                    lastGCD = Fire4;
                    return Fire4;
                }
                // ����
                if (currentMP >= MP.Despair)
                {
                    if (GetRemainingCharges(Manafont) == 0)
                    {
                        pecialCombo = "";
                    }
                    lastGCD = Despair;
                    return Despair;
                }
                // ����2
                if (GetRemainingCharges(Manafont) == 0)
                {
                    lastGCD = Fire4;
                    return Fire4;
                }
                // ħȪ
                if (GetRemainingCharges(Manafont) > 0)
                {
                    lastOGCD = Manafont;
                    return Manafont;
                }
                // ����������
                pecialCombo = "";
                return actionID;
            }
        }
    }
}
