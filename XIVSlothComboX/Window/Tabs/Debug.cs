using System;
using System.Linq;
using System.Numerics;
using Dalamud.Game.ClientState.JobGauge.Types;
using Dalamud.Game.ClientState.Objects.SubKinds;
using Dalamud.Game.ClientState.Objects.Types;
using FFXIVClientStructs.FFXIV.Client.Game;
using ImGuiNET;
using XIVSlothComboX.Combos;
using XIVSlothComboX.Combos.JobHelpers;
using XIVSlothComboX.Combos.PvE;
using XIVSlothComboX.Core;
using XIVSlothComboX.CustomComboNS;
using XIVSlothComboX.CustomComboNS.Functions;
using XIVSlothComboX.Data;
using XIVSlothComboX.Extensions;
using XIVSlothComboX.Services;
using Status = Dalamud.Game.ClientState.Statuses.Status;

#if DEBUG
namespace XIVSlothComboX.Window.Tabs
{

    internal class Debug : ConfigWindow
    {

        internal class DebugCombo : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; }

            protected override uint Invoke(uint actionID, uint lastComboActionID, float comboTime, byte level) => actionID;
        }

        internal static new  void Draw()
        {
            IPlayerCharacter? LocalPlayer = Service.ClientState.LocalPlayer;
            // DebugCombo? comboClass = new();

            if (LocalPlayer != null)
            {
                unsafe
                {
                    if (Service.ClientState.LocalPlayer.TargetObject is IBattleChara chara)
                    {
                        foreach (Status? status in chara.StatusList)
                        {
                            ImGui.TextUnformatted($"TARGET STATUS CHECK: {chara.Name} -> {ActionWatching.GetStatusName(status.StatusId)}: {status.StatusId}");
                        }
                    }

                    foreach (Status? status in Service.ClientState.LocalPlayer.StatusList)
                    {
                        ImGui.TextUnformatted($"SELF STATUS CHECK: {Service.ClientState.LocalPlayer.Name} -> {ActionWatching.GetStatusName(status.StatusId)}: {status.StatusId}");
                    }

                    ImGui.TextUnformatted($"TARGET OBJECT KIND: {Service.ClientState.LocalPlayer.TargetObject?.ObjectKind}");
                    ImGui.TextUnformatted($"TARGET IS BATTLE CHARA: {Service.ClientState.LocalPlayer.TargetObject is IBattleChara}");
                    ImGui.TextUnformatted($"Level: {Service.ClientState.LocalPlayer.Level}");
                    ImGui.TextUnformatted($"IN COMBAT: {CustomComboFunctions.InCombat()}");
                    ImGui.TextUnformatted($"ActionWatching.CombatActions.Count: {ActionWatching.CombatActions.Count}");
                    ImGui.TextUnformatted($"IN MELEE RANGE: {CustomComboFunctions.InMeleeRange()}");
                    ImGui.TextUnformatted($"DISTANCE FROM TARGET: {CustomComboFunctions.GetTargetDistance()}");
                    ImGui.TextUnformatted($"TARGET HP VALUE: {CustomComboFunctions.EnemyHealthCurrentHp()}");
                    ImGui.TextUnformatted($"LAST ACTION: {ActionWatching.GetActionName(ActionWatching.LastAction)} (ID:{ActionWatching.LastAction})");
                    ImGui.TextUnformatted($"LAST ACTION COST: {CustomComboFunctions.GetResourceCost(ActionWatching.LastAction)}");
                    ImGui.TextUnformatted($"LAST ACTION TYPE: {ActionWatching.GetAttackType(ActionWatching.LastAction)}");
                    ImGui.TextUnformatted($"LAST WEAPONSKILL: {ActionWatching.GetActionName(ActionWatching.LastWeaponskill)}");
                    ImGui.TextUnformatted($"LAST SPELL: {ActionWatching.GetActionName(ActionWatching.LastSpell)}");
                    ImGui.TextUnformatted($"LAST ABILITY: {ActionWatching.GetActionName(ActionWatching.LastAbility)}");
                    ImGui.TextUnformatted($"ZONE: {Service.ClientState.TerritoryType}");
                    ImGui.TextUnformatted($"战斗时间 : {CustomComboFunctions.CombatEngageDuration().TotalSeconds}");

                    ImGui.TextUnformatted($"倒计时 : {Countdown.TimeRemaining()} ");
                    {
                        uint itemId = 4551;
                        ImGui.TextUnformatted($"恢复药数量 : {InventoryManager.Instance()->GetInventoryItemCount(itemId, true)}");
                        ImGui.TextUnformatted($"恢复药状态 : {ActionManager.Instance()->GetActionStatus(ActionType.Item,itemId+ 1000000)}");
                        
                        
                        ImGui.TextUnformatted($"TimelineList.Count : {ActionWatching.TimelineList.Count}");
                        
                        
                        // ActionManager.Instance()->UseActionLocation()
                        // ImGui.TextUnformatted($"恢复药状态 : {}");
                        
                        // ImGui.TextUnformatted($"恢复药状态 : {ActionManager.Addresses.GetAdjustedActionIdwas}");
                    }


                    // ImGui.TextUnformatted($"buff : {CustomComboFunctions.GetBuffRemainingTime(PLD.Buffs.DivineMight)}");
                    // ImGui.TextUnformatted($"王权层数 : {CustomComboFunctions.GetBuffStacks(PLD.Buffs.忠义之剑SwordOath)}");

                  
                    // ImGui.TextUnformatted($"{CustomComboFunctions.HasEffect(DNC.Buffs.标准舞步预备StandardStep)} ");
                    // ImGui.TextUnformatted($"四色技巧舞步结束TechnicalFinish4 : {CustomComboFunctions.WasLastAction(DNC.四色技巧舞步结束TechnicalFinish4)} 进攻之探戈Devilment {CustomComboFunctions.WasLastAction(DNC.进攻之探戈Devilment)}}} 综合{!CustomComboFunctions.WasLastAction(DNC.四色技巧舞步结束TechnicalFinish4) && !CustomComboFunctions.WasLastAbility(DNC.进攻之探戈Devilment) } ");

                    // if (ActionWatching.特殊起手Actions.Count >0 )
                    // {
                    //     ImGui.TextUnformatted($"{MCHOpenerLogic.currentState }  ActionWatching.特殊起手Actions.Count: {ActionWatching.特殊起手Actions.Count} {ActionWatching.GetActionName(ActionWatching.特殊起手Actions[0])}->{ActionWatching.特殊起手Actions[0]} ");
                    // }
                    //
                    // foreach (var combatAction in ActionWatching.CombatActions)
                    // {
                    //     ImGui.TextUnformatted($"{ActionWatching.GetActionName(combatAction)} {ActionWatching.GetAttackType(combatAction)}");
                    // }
                    //


                    // ImGui.TextUnformatted($"战逃倒计时 : {CustomComboFunctions.GetCooldownRemainingTime(PLD.战逃反应FightOrFlight)}");
                    // ImGui.TextUnformatted($"战逃个数 : {ActionWatching.CombatActions.FindAll(actionId => actionId ==PLD. 战逃反应FightOrFlight).Count}");

                    // ImGui.TextUnformatted($"DK血乱 : {CustomComboFunctions.GetBuffStacks(DRK.Buffs.BloodWeapon)}");
                    // var gauge = GetJobGauge<DRKGauge>();
                    // ImGui.TextUnformatted($"DK2 : {CustomComboFunctions.GetJobGauge<DRKGauge>().Blood}");
                    // ImGui.TextUnformatted($"DK3 : 血乱CD{CustomComboFunctions.GetCooldownRemainingTime(DRK.血乱)},弗雷CD{CustomComboFunctions.GetCooldownRemainingTime(DRK.LivingShadow)}");
                    // ImGui.TextUnformatted($"DK4 : {CustomComboFunctions.FindTargetEffect(RaidBuff.连环计) is not null}");
                    // ImGui.TextUnformatted($"DK5 : {CustomComboFunctions.FindTargetEffectAny(RaidBuff.连环计) is not null}");    


                    // ImGui.TextUnformatted($"gun1 : {CustomComboFunctions.ActionReady(GNB.倍攻DoubleDown)}");
                    // ImGui.TextUnformatted($"gun2 : {CustomComboFunctions.GetCooldownRemainingTime(GNB.倍攻DoubleDown)}");
                    // ImGui.TextUnformatted($"烈牙GnashingFang : {CustomComboFunctions.GetCooldownRemainingTime(GNB.烈牙GnashingFang)}");
                    // ImGui.TextUnformatted($"利刃斩KeenEdge : {CustomComboFunctions.GetCooldownRemainingTime(GNB.利刃斩KeenEdge)}");
                    // ImGui.TextUnformatted($"倍攻DoubleDown : {CustomComboFunctions.GetCooldownRemainingTime(GNB.倍攻DoubleDown)}");


                    // 机工 start
                    // ImGui.TextUnformatted($"倒计时 : {Countdown.TimeRemaining()} HasPrePullCooldowns:{MCHOpenerLogic.HasPrePullCooldowns()} HasCooldowns:{MCHOpenerLogic.HasCooldowns()}");
                    // ImGui.TextUnformatted($"HasPrePullCooldowns:{MCHOpenerLogic.HasPrePullCooldowns()} HasCooldowns:{MCHOpenerLogic.HasCooldowns()}");
                    // ImGui.TextUnformatted($"倒计时 : {Countdown.TimeRemaining()} ");
                    // ImGui.TextUnformatted($"整备ReassembleV2 : {MCH.整备ReassembleV2.LevelChecked()} ");
                    // ImGui.TextUnformatted($"整备ReassembleV21 : {CustomComboFunctions.GetLevel(MCH.整备ReassembleV2)} ");
                    ;
                    ;
                    ;
                    // ImGui.TextUnformatted($"回转飞锯ChainSaw:{CustomComboFunctions.GetCooldownRemainingTime(MCH.回转飞锯ChainSaw)}，野火:{CustomComboFunctions.GetCooldownRemainingTime(MCH.野火Wildfire)}，整备Reassemble:{CustomComboFunctions.GetCooldownRemainingTime(MCH.整备Reassemble)} ");
                    // ImGui.TextUnformatted($"{CustomComboFunctions.FindEffect(MCH.Buffs.野火Wildfire)?.RemainingTime}");
                    // ImGui.TextUnformatted($"虹吸弹GaussRound:{CustomComboFunctions.GetRemainingCharges(MCH.虹吸弹GaussRound)}，弹射Ricochet:{CustomComboFunctions.GetCooldownRemainingTime(MCH.弹射Ricochet)} ");
                    
                    // ImGui.TextUnformatted($"GetRemainingCharges{CustomComboFunctions. GetRemainingCharges(MCH.虹吸弹GaussRound)} IsOffCooldown {CustomComboFunctions.IsOffCooldown(MCH.虹吸弹GaussRound)}");
                    
                    
                    
                    // ImGui.TextUnformatted($"MCHOpenerLogic {MCHOpenerLogic.CanOpener} {MCHOpenerLogic.currentState} {CustomComboFunctions. GetRemainingCharges(MCH.热冲击HeatBlast)}");
                    // ImGui.TextUnformatted($"GetCooldownElapsed {CustomComboFunctions.GetCooldownElapsed(MCH.虹吸弹GaussRound)} {ActionWatching.GetActionCastTime(MCH.虹吸弹GaussRound)}");
                    
                    
                    
                    
                    
                    // ImGui.TextUnformatted($"CanOpener : {MCHOpenerLogic.CanOpener}  currentState{MCHOpenerLogic.currentState.ToString()}");
                    // ImGui.TextUnformatted($"整备Reassemble倒计时1 : {CustomComboFunctions.GetRemainingCharges(MCH.整备Reassemble) }");
                    // ImGui.TextUnformatted($"整备Reassemble倒计时2 : {CustomComboFunctions.GetRemainingCharges(MCH.整备Reassemble) == 0 }");
                    // ImGui.TextUnformatted($"整备Reassemble倒计时3 : {CustomComboFunctions.GetCooldownRemainingTime(MCH.整备Reassemble) == 0}");
                    // ImGui.TextUnformatted($"整备Reassemble倒计时 : {CustomComboFunctions.GetCooldownRemainingTime(MCH.整备Reassemble) }");

                    //机工 end
                    
                    
                
                
                
                    // ImGui.TextUnformatted($"摆烂循环 : {PLD.ActionLoop}");
                    // ImGui.TextUnformatted($"摆烂循环 : {CustomComboFunctions.GetOptionValue(PLD.Config.PLD_FOF_GCD)}");
                
                    // ImGui.TextUnformatted($"WasLast2ActionsAbilities : {ActionWatching.WasLast2ActionsAbilities()}");
                
                
                    // ImGui.TextUnformatted($"Service.ComboCache.GetMaxCharges : {Service.ComboCache.GetMaxCharges(DRK.BloodWeapon)}");
                
                    // ImGui.TextUnformatted($"LocalPlayer.TotalCastTime : {LocalPlayer.TotalCastTime}");
                    //倒计时
                    // ImGui.TextUnformatted($"CooldownRemaining1 : {Service.ComboCache.GetCooldown(DRK.血溅).CooldownRemaining}");
                    // ImGui.TextUnformatted($"CooldownRemaining2 : {CustomComboFunctions.GetCooldownRemainingTime(DRK.血溅)}");
                    // ImGui.TextUnformatted($"CooldownElapsed : {Service.ComboCache.GetCooldown(DRK.血溅).CooldownElapsed}");
                    // ImGui.TextUnformatted($"CooldownElapsed : {CustomComboFunctions.GetBuffRemainingTime(DRK.Buffs.Delirium)}");
                
                
                    // if (ActionWatching.CombatActions.Count > 2)
                    // {
                    //     ImGui.TextUnformatted($"摆烂循环1 : {ActionWatching.CombatActions.Last()}");
                    //     ImGui.TextUnformatted($"摆烂循环2 : {ActionWatching.CombatActions[ActionWatching.CombatActions.Count - 2]}");
                    //     ImGui.TextUnformatted($"摆烂循环3 : {ActionWatching.GetAttackType(ActionWatching.CombatActions.Last())}");
                    // }


                    // ImGui.TextUnformatted($"战逃个数 : {ActionWatching.CombatActions.Count}");
                
                    // ImGui.BeginChild("BLUSPELLS", new Vector2(250, 100), false);
                    // ImGui.TextUnformatted($"SELECTED BLU SPELLS:\n{string.Join("\n", Service.Configuration.ActiveBLUSpells.Select(x => ActionWatching.GetActionName(x)).OrderBy(x => x))}");
                    
                    //舞者 start
                    // ImGui.TextUnformatted($"{DNC.剑舞SaberDance.GCDActionPreReady(DNC.百花争艳Flourish)},百花:{CustomComboFunctions.GetCooldownRemainingTime(DNC.百花争艳Flourish)}，剑舞:{CustomComboFunctions.GetCooldownRemainingTime(DNC.剑舞SaberDance)}");
                    // ImGui.TextUnformatted($"{CustomComboFunctions.FindEffect(MCH.Buffs.野火Wildfire)?.RemainingTime}");
                    //舞者 end
                    
                             
                    //召唤 start
                    // ImGui.TextUnformatted($"{SMN.SMN_Advanced_Combo.DemiAttackCount},{SMN.SMN_Advanced_Combo.UsedDemiAttack}");
                    // ImGui.TextUnformatted($"{CustomComboFunctions.FindEffect(MCH.Buffs.野火Wildfire)?.RemainingTime}");
                    //召唤 end
                    
                    
                    
                    //骑士 start
                    // ImGui.TextUnformatted($"{SMN.SMN_Advanced_Combo.DemiAttackCount},{SMN.SMN_Advanced_Combo.UsedDemiAttack}");
                    ImGui.TextUnformatted($"1-{CustomComboFunctions.GetJobGauge<ASTGauge>().DrawnCard}");
                    ImGui.TextUnformatted($"2-{CustomComboFunctions.GetJobGauge<ASTGauge>().DrawnCrownCard}");
                    //骑士 end
                }
            }

            else
            {
                ImGui.TextUnformatted("Please log in to use this tab.");
            }
        }
    }
}
#endif