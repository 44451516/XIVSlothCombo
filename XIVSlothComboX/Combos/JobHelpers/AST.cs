using System.Collections.Generic;
using System.Linq;
using Dalamud.Game.ClientState.Conditions;
using Dalamud.Game.ClientState.JobGauge.Enums;
using Dalamud.Game.ClientState.Objects.Types;
using Dalamud.Plugin.Services;
using ECommons.DalamudServices;
using ECommons.GameFunctions;
using XIVSlothComboX.CustomComboNS.Functions;
using XIVSlothComboX.Extensions;
using static XIVSlothComboX.Combos.PvE.AST;

namespace XIVSlothComboX.Combos.JobHelpers
{
    internal static class AST
    {
        internal static void Init()
        {
            Svc.Framework.Update += CheckCards;
        }

        private static void CheckCards(IFramework framework)
        {
            if (Svc.Objects.LocalPlayer is null || Svc.Objects.LocalPlayer.ClassJob.RowId != 33)
                return;

            if (Svc.Condition[ConditionFlag.BetweenAreas] || Svc.Condition[ConditionFlag.Unconscious])
            {
                AST_QuickTargetCards.SelectedRandomMember = null;
                return;
            }

            if (DrawnCard != Gauge.DrawnCards[0])
            {
                DrawnCard = Gauge.DrawnCards[0];
            }

            if (CustomComboFunctions.IsEnabled(CustomComboPreset.AST_Cards_QuickTargetCards) && (AST_QuickTargetCards.SelectedRandomMember is null || BetterTargetAvailable()))
            {
                if (CustomComboFunctions.ActionReady(Play1))
                {
                    AST_QuickTargetCards.Invoke();
                }
            }

            if (DrawnCard == CardType.None)
                AST_QuickTargetCards.SelectedRandomMember = null;

        }

        private static bool BetterTargetAvailable()
        {
            if (AST_QuickTargetCards.SelectedRandomMember is null || AST_QuickTargetCards.SelectedRandomMember.IsDead || CustomComboFunctions.OutOfRange(Balance, AST_QuickTargetCards.SelectedRandomMember))
                return true;

            var m = AST_QuickTargetCards.SelectedRandomMember as IBattleChara;
            if ((DrawnCard is CardType.Balance && CustomComboFunctions.JobIDs.Melee.Any(x => x == m.ClassJob.RowId)) || (DrawnCard is CardType.Spear && CustomComboFunctions.JobIDs.Ranged.Any(x => x == m.ClassJob.RowId)))
                return false;

            var targets = new List<IBattleChara>();
            for (int i = 1; i <= 8; i++) //Checking all 8 available slots and skipping nulls & DCs
            {
                if (CustomComboFunctions.GetPartySlot(i) is not IBattleChara member) continue;
                if (member.GameObjectId == AST_QuickTargetCards.SelectedRandomMember.GameObjectId) continue;
                if (member is null) continue; //Skip nulls/disconnected people
                if (member.IsDead) continue;
                if (CustomComboFunctions.OutOfRange(Balance, member)) continue;

                if (CustomComboFunctions.FindEffectOnMember(Buffs.BalanceBuff, member) is not null) continue;
                if (CustomComboFunctions.FindEffectOnMember(Buffs.SpearBuff, member) is not null) continue;

                if (Config.AST_QuickTarget_SkipDamageDown && CustomComboFunctions.TargetHasDamageDown(member)) continue;
                if (Config.AST_QuickTarget_SkipRezWeakness && CustomComboFunctions.TargetHasRezWeakness(member)) continue;

                if (member.GetRole() is CombatRole.Healer or CombatRole.Tank) continue;

                targets.Add(member);
            }

            if (targets.Count == 0) return false;

            if ((DrawnCard is CardType.Balance && targets.Any(x => CustomComboFunctions.JobIDs.Melee.Any(y => y == x.ClassJob.RowId))) || (DrawnCard is CardType.Spear && targets.Any(x => CustomComboFunctions.JobIDs.Ranged.Any(y => y == x.ClassJob.RowId))))
            {
                AST_QuickTargetCards.SelectedRandomMember = null;
                return true;
            }

            return false;

        }

        internal class AST_QuickTargetCards : CustomComboFunctions
        {
            internal static List<IGameObject> PartyTargets = [];

            internal static IGameObject? SelectedRandomMember;

            public static void Invoke()
            {
                if (DrawnCard is not CardType.None)
                {
                    if (GetPartySlot(2) is not null)
                    {
                        SetTarget();
                        Svc.Log.Debug($"Set card to {SelectedRandomMember?.Name}");
                    }
                    else
                    {
                        Svc.Log.Debug($"Setting card to {LocalPlayer?.Name}");
                        SelectedRandomMember = LocalPlayer;
                    }
                }
                else
                {
                    SelectedRandomMember = null;
                }
            }

            private static bool SetTarget()
            {
                if (Gauge.DrawnCards[0].Equals(CardType.None)) return false;

                CardType cardDrawn = Gauge.DrawnCards[0];
                PartyTargets.Clear();
                for (int i = 1; i <= 8; i++) //Checking all 8 available slots and skipping nulls & DCs
                {
                    if (GetPartySlot(i) is not IBattleChara member) continue;
                    if (member is null) continue; //Skip nulls/disconnected people
                    if (member.IsDead) continue;
                    if (OutOfRange(Balance, member)) continue;

                    if (FindEffectOnMember(Buffs.BalanceBuff, member) is not null) continue;
                    if (FindEffectOnMember(Buffs.SpearBuff, member) is not null) continue;

                    if (Config.AST_QuickTarget_SkipDamageDown && TargetHasDamageDown(member)) continue;
                    if (Config.AST_QuickTarget_SkipRezWeakness && TargetHasRezWeakness(member)) continue;

                    PartyTargets.Add(member);
                }
                //The inevitable "0 targets found" because of debuffs
                if (PartyTargets.Count == 0)
                {
                    for (int i = 1; i <= 8; i++) //Checking all 8 available slots and skipping nulls & DCs
                    {
                        if (GetPartySlot(i) is not IBattleChara member) continue;
                        if (member is null) continue; //Skip nulls/disconnected people
                        if (member.IsDead) continue;
                        if (OutOfRange(Balance, member)) continue;

                        if (FindEffectOnMember(Buffs.BalanceBuff, member) is not null) continue;
                        if (FindEffectOnMember(Buffs.SpearBuff, member) is not null) continue;

                        PartyTargets.Add(member);
                    }
                }

                if (SelectedRandomMember is not null)
                {
                    if (PartyTargets.Any(x => x.GameObjectId == SelectedRandomMember.GameObjectId))
                    {
                        //TargetObject(SelectedRandomMember);
                        return true;
                    }
                }


                if (PartyTargets.Count > 0)
                {
                    PartyTargets.Shuffle();
                    //Give card to DPS first
                    for (int i = 0; i <= PartyTargets.Count - 1; i++)
                    {
                        byte job = PartyTargets[i] is IBattleChara ? (byte)(PartyTargets[i] as IBattleChara).ClassJob.RowId : (byte)0;
                        if (((cardDrawn is CardType.Balance) && JobIDs.Melee.Contains(job)) || ((cardDrawn is CardType.Spear) && JobIDs.Ranged.Contains(job)))
                        {
                            //TargetObject(PartyTargets[i]);
                            SelectedRandomMember = PartyTargets[i];
                            return true;
                        }
                    }
                    //Give card to unsuitable DPS next
                    for (int i = 0; i <= PartyTargets.Count - 1; i++)
                    {
                        byte job = PartyTargets[i] is IBattleChara ? (byte)(PartyTargets[i] as IBattleChara).ClassJob.RowId : (byte)0;
                        if (((cardDrawn is CardType.Balance) && JobIDs.Ranged.Contains(job)) || ((cardDrawn is CardType.Spear) && JobIDs.Melee.Contains(job)))
                        {
                            //TargetObject(PartyTargets[i]);
                            SelectedRandomMember = PartyTargets[i];
                            return true;
                        }
                    }

                    //Give cards to healers/tanks if backup is turned on
                    if (IsEnabled(CustomComboPreset.AST_Cards_QuickTargetCards_TargetExtra1))
                    {
                        for (int i = 0; i <= PartyTargets.Count - 1; i++)
                        {
                            byte job = PartyTargets[i] is IBattleChara ? (byte)(PartyTargets[i] as IBattleChara).ClassJob.RowId : (byte)0;
                            if (cardDrawn is CardType.Balance && JobIDs.Tank.Contains(job))
                            {
                                SelectedRandomMember = PartyTargets[i];
                                return true;
                            }
                        }
                    }

                    if (IsEnabled(CustomComboPreset.AST_Cards_QuickTargetCards_TargetExtra2))
                    {
                        for (int i = 0; i <= PartyTargets.Count - 1; i++)
                        {
                            byte job = PartyTargets[i] is IBattleChara ? (byte)(PartyTargets[i] as IBattleChara).ClassJob.RowId : (byte)0;
                            if (cardDrawn is CardType.Spear && JobIDs.Healer.Contains(job))
                            {
                                SelectedRandomMember = PartyTargets[i];
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
        }

        internal static void Dispose()
        {
            Svc.Framework.Update -= CheckCards;
        }
    }
}