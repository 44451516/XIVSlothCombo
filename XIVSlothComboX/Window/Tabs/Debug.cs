using System;
using System.Linq;
using System.Numerics;
using Dalamud.Game.ClientState.JobGauge.Types;
using Dalamud.Game.ClientState.Objects.SubKinds;
using Dalamud.Game.ClientState.Objects.Types;
using Dalamud.Interface.Colors;
using Dalamud.Interface.Utility.Raii;
using ECommons.DalamudServices;
using ECommons.ImGuiMethods;
using ECommons.Logging;
using FFXIVClientStructs.FFXIV.Client.Game;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using Dalamud.Bindings.ImGui;
using Lumina.Excel.Sheets;
using XIVSlothComboX.Combos;
using XIVSlothComboX.Combos.PvE;
using XIVSlothComboX.CustomComboNS;
using XIVSlothComboX.CustomComboNS.Functions;
using XIVSlothComboX.Data;
using XIVSlothComboX.Extensions;
using XIVSlothComboX.Services;
using Status = Dalamud.Game.ClientState.Statuses.IStatus;
using Action = Lumina.Excel.Sheets.Action;
using static XIVSlothComboX.CustomComboNS.Functions.CustomComboFunctions;

#if DEBUG
namespace XIVSlothComboX.Window.Tabs
{
    internal class Debug : ConfigWindow
    {
        public static int debugNum = 0;

        internal class DebugCombo : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; }
            protected override uint Invoke(uint actionID, uint lastComboActionID, float comboTime, byte level) => actionID;
        }

        internal static Action? debugSpell;

        internal unsafe static new void Draw()
        {
            // DebugCombo? comboClass = new();
            IPlayerCharacter? LocalPlayer = Svc.Objects.LocalPlayer;
            uint[] statusBlacklist = { 360, 361, 362, 363, 364, 365, 366, 367, 368 }; // Duration will not be displayed for these status effects

            // Custom Styling
            static void CustomStyleText(string label, object? value)
            {
                if (!string.IsNullOrEmpty(label))
                {
                    ImGui.TextUnformatted(label);
                    ImGui.SameLine(0, 4f);
                }
                ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.DalamudGrey);
                ImGui.TextUnformatted(value?.ToString() ?? "");
                ImGui.PopStyleColor();
            }

            if (LocalPlayer != null)
            {
                // Player Status Effects
                if (ImGui.CollapsingHeader("Player Status Effects"))
                {
                    foreach (Status? status in LocalPlayer.StatusList)
                    {
                        // Null Check (Source Name)
                        if (status.SourceObject is not null)
                        {
                            ImGui.TextUnformatted($"{status.SourceObject.Name} ->");
                            ImGui.SameLine(0, 4f);
                        }

                        // Null Check (Status Name)
                        if (!string.IsNullOrEmpty(ActionWatching.GetStatusName(status.StatusId)))
                        {
                            CustomStyleText(ActionWatching.GetStatusName(status.StatusId) + ":", status.StatusId);
                        }
                        else CustomStyleText("", status.StatusId);

                        // Duration + Blacklist Check
                        float buffDuration = CustomComboFunctions.GetBuffRemainingTime((ushort)status.StatusId, false);
                        if (buffDuration != 0 && !statusBlacklist.Contains(status.StatusId))
                        {
                            string formattedDuration;
                            if (buffDuration >= 60)
                            {
                                int minutes = (int)(buffDuration / 60);
                                formattedDuration = $"{minutes}m";
                            }
                            else formattedDuration = $"{buffDuration:F1}s";

                            ImGui.SameLine(0, 4f);
                            CustomStyleText("", $"({formattedDuration})");
                        }
                    }
                }

                // Target Status Effects
                if (ImGui.CollapsingHeader("Target Status Effects"))
                {
                    if (LocalPlayer.TargetObject is IBattleChara chara)
                    {
                        foreach (Status? status in chara.StatusList)
                        {
                            // Null Check (Source Name)
                            if (status.SourceObject is not null)
                            {
                                ImGui.TextUnformatted($"{status.SourceObject.Name} ->");
                                ImGui.SameLine(0, 4f);
                            }

                            // Null Check (Status Name)
                            if (!string.IsNullOrEmpty(ActionWatching.GetStatusName(status.StatusId)))
                            {
                                CustomStyleText(ActionWatching.GetStatusName(status.StatusId) + ":", status.StatusId);
                            }
                            else CustomStyleText("", status.StatusId);

                            // Duration + Blacklist Check
                            float debuffDuration = CustomComboFunctions.GetDebuffRemainingTime((ushort)status.StatusId, false);
                            if (debuffDuration != 0 && !statusBlacklist.Contains(status.StatusId))
                            {
                                string formattedDuration;
                                if (debuffDuration >= 60)
                                {
                                    int minutes = (int)(debuffDuration / 60);
                                    formattedDuration = $"{minutes}m";
                                }
                                else formattedDuration = $"{debuffDuration:F1}s";

                                ImGui.SameLine(0, 4f);
                                CustomStyleText("", $"({formattedDuration})");
                            }
                        }

                    }
                }
                if (ImGui.CollapsingHeader("Action Info"))
                {
                    string prev = debugSpell == null ? "Select Action" : $"({debugSpell.Value.RowId}) Lv.{debugSpell.Value.ClassJobLevel}. {debugSpell.Value.Name} - {(debugSpell.Value.IsPvP ? "PvP" : "Normal")}";
                    ImGuiEx.SetNextItemFullWidth();
                    using (var comboBox = ImRaii.Combo("###ActionCombo", prev))
                    {
                        if (comboBox)
                        {
                            if (ImGui.Selectable("", debugSpell == null))
                            {
                                debugSpell = null;
                            }

                            var classId = CustomComboFunctions.JobIDs.JobToClass(JobID.Value);
                            foreach (var act in Svc.Data.GetExcelSheet<Lumina.Excel.Sheets.Action>().Where(x => x.IsPlayerAction && x.ClassJob.RowId == classId || x.ClassJob.RowId == JobID.Value).OrderBy(x => x.ClassJobLevel))
                            {
                                if (ImGui.Selectable($"({act.RowId}) Lv.{act.ClassJobLevel}. {act.Name} - {(act.IsPvP ? "PvP" : "Normal")}", debugSpell?.RowId == act.RowId))
                                {
                                    debugSpell = act;
                                }
                            }
                        }
                    }

                    if (debugSpell != null)
                    {
                        var actionStatus = ActionManager.Instance()->GetActionStatus(ActionType.Action, debugSpell.Value.RowId);
                        var icon = Svc.Texture.GetFromGameIcon(new(debugSpell.Value.Icon)).GetWrapOrEmpty().Handle;
                        ImGui.Image(icon, new System.Numerics.Vector2(60f.Scale(), 60f.Scale()));
                        ImGui.SameLine();
                        ImGui.Image(icon, new System.Numerics.Vector2(30f.Scale(), 30f.Scale()));
                        CustomStyleText($"Action Status:", $"{actionStatus} ({Svc.Data.GetExcelSheet<LogMessage>().GetRow(actionStatus).Text})");
                        CustomStyleText($"Action Type:", debugSpell.Value.ActionCategory.Value.Name);
                        if (debugSpell.Value.UnlockLink.RowId != 0)
                            CustomStyleText($"Quest:", $"{Svc.Data.GetExcelSheet<Quest>().GetRow(debugSpell.Value.UnlockLink.RowId).Name} ({(UIState.Instance()->IsUnlockLinkUnlockedOrQuestCompleted(debugSpell.Value.UnlockLink.RowId) ? "Completed" : "Not Completed")})");
                        CustomStyleText($"Base Recast:", $"{debugSpell.Value.Recast100ms / 10f}s");
                        CustomStyleText($"Max Charges:", $"{debugSpell.Value.MaxCharges}");
                        if (ActionWatching.ActionTimestamps.ContainsKey(debugSpell.Value.RowId))
                            CustomStyleText($"Time Since Last Use:", $"{(Environment.TickCount64 - ActionWatching.ActionTimestamps[debugSpell.Value.RowId])/1000f:F2}");
                    }
                }

                // Player Info
                ImGui.Spacing();
                ImGui.Text("Player Info");
                ImGui.Separator();
                CustomStyleText("Job:", $"{LocalPlayer.ClassJob.Value.NameEnglish} (ID: {LocalPlayer.ClassJob.RowId})");
                CustomStyleText("Zone:", $"{Svc.Data.GetExcelSheet<TerritoryType>()?.FirstOrDefault(x => x.RowId == Svc.ClientState.TerritoryType).PlaceName.Value.Name} (ID: {Svc.ClientState.TerritoryType})");
                CustomStyleText("In PvP:", CustomComboFunctions.InPvP());
                CustomStyleText("In Combat:", CustomComboFunctions.InCombat());
                CustomStyleText("Hitbox Radius:", LocalPlayer.HitboxRadius);
                ImGui.Spacing();

                // Target Info
                ImGui.Spacing();
                ImGui.Text("Target Info");
                ImGui.Separator();
                CustomStyleText("ObjectId:", LocalPlayer.TargetObject?.GameObjectId);
                CustomStyleText("ObjectKind:", LocalPlayer.TargetObject?.ObjectKind);
                CustomStyleText("DataId:", LocalPlayer.TargetObject?.BaseId);
                CustomStyleText("Is BattleChara:", LocalPlayer.TargetObject is IBattleChara);
                CustomStyleText("Is PlayerCharacter:", LocalPlayer.TargetObject is IPlayerCharacter);
                CustomStyleText("Distance:", $"{Math.Round(CustomComboFunctions.GetTargetDistance(), 2)}y");
                CustomStyleText("Hitbox Radius:", LocalPlayer.TargetObject?.HitboxRadius);
                CustomStyleText("In Melee Range:", CustomComboFunctions.InMeleeRange());
                CustomStyleText("Relative Position:", CustomComboFunctions.AngleToTarget() is 2 ? "Rear" : (CustomComboFunctions.AngleToTarget() is 1 or 3) ? "Flank" : CustomComboFunctions.AngleToTarget() is 4 ? "Front" : "");
                CustomStyleText("Health:", $"{CustomComboFunctions.EnemyHealthCurrentHp().ToString("N0")} / {CustomComboFunctions.EnemyHealthMaxHp().ToString("N0")} ({Math.Round(CustomComboFunctions.GetTargetHPPercent(), 2)}%)");
                ImGui.Spacing();

                // Action Info
                ImGui.Spacing();
                ImGui.Text("Action Info");
                ImGui.Separator();
                CustomStyleText("Last Action:", ActionWatching.LastAction == 0 ? string.Empty : $"{(string.IsNullOrEmpty(ActionWatching.GetActionName(ActionWatching.LastAction)) ? "Unknown" : ActionWatching.GetActionName(ActionWatching.LastAction))} (ID: {ActionWatching.LastAction})");
                CustomStyleText("Last Action Cost:", CustomComboFunctions.GetResourceCost(ActionWatching.LastAction));
                CustomStyleText("Last Action Type:", ActionWatching.GetAttackType(ActionWatching.LastAction));
                CustomStyleText("Last Weaponskill:", ActionWatching.GetActionName(ActionWatching.LastWeaponskill));
                CustomStyleText("Last Spell:", ActionWatching.GetActionName(ActionWatching.LastSpell));
                CustomStyleText("Last Ability:", ActionWatching.GetActionName(ActionWatching.LastAbility));
                CustomStyleText("Combo Timer:", $"{CustomComboFunctions.ComboTimer:F1}");
                CustomStyleText("Combo Action:", CustomComboFunctions.ComboAction == 0 ? string.Empty : $"{(string.IsNullOrEmpty(ActionWatching.GetActionName(CustomComboFunctions.ComboAction)) ? "Unknown" : ActionWatching.GetActionName(CustomComboFunctions.ComboAction))} (ID: {CustomComboFunctions.ComboAction})");
                CustomStyleText("Cast Time:", $"{LocalPlayer.CurrentCastTime:F2} / {LocalPlayer.TotalCastTime:F2}");
                CustomStyleText("Cast Action:", LocalPlayer.CastActionId == 0 ? string.Empty : $"{(string.IsNullOrEmpty(ActionWatching.GetActionName(LocalPlayer.CastActionId)) ? "Unknown" : ActionWatching.GetActionName(LocalPlayer.CastActionId))} (ID: {LocalPlayer.CastActionId})");
                ImGui.Spacing();

                // Party Info
                ImGui.Spacing();
                ImGui.Text("Party Info");
                ImGui.Separator();
                CustomStyleText("Party ID:", Svc.Party.PartyId);
                CustomStyleText("Party Size:", Svc.Party.Length);
                if (ImGui.CollapsingHeader("Party Members"))
                {
                    for (int i = 1; i <= 8; i++)
                    {
                        if (CustomComboFunctions.GetPartySlot(i) is not IBattleChara member || member is null) continue;

                        ImGui.TextUnformatted($"Slot {i} ->");
                        ImGui.SameLine(0, 4f);
                        CustomStyleText($"{CustomComboFunctions.GetPartySlot(i).Name}", $"({member.ClassJob.Value.Abbreviation})");
                        
                    }
                }
                ImGui.Spacing();

                // Misc. Info
                ImGui.Spacing();
                ImGui.Text("Miscellaneous Info");
                ImGui.Separator();
                if (ImGui.CollapsingHeader("Active Blue Mage Spells"))
                {
                    ImGui.TextUnformatted($"{string.Join("\n", Service.Configuration.ActiveBLUSpells.Select(x => ActionWatching.GetActionName(x)).OrderBy(x => x))}");
                }

                if (ImGui.CollapsingHeader("自定义"))
                {
                    
                    float GCD = GetCooldown(RPR.切割Slice).CooldownTotal * 3;
                    var JobHelpers = ActionManager.Instance()->Combo.Timer != 0 && ActionManager.Instance()->Combo.Timer < GCD;
                      
                    
                    // ImGui.TextUnformatted($"JobHelpers-{JobHelpers}");
                    // ImGui.TextUnformatted($"Timer-{ActionManager.Instance()->Combo.Timer}");
                    // ImGui.TextUnformatted($"GCD-{GCD}");
                    var healTarget = GetHealTarget(WHM.Config.WHM_AoEHeals_MedicaMO);
                    // ImGui.TextUnformatted($"ShadowTimeRemaining-{GetJobGauge<DRKGauge>().ShadowTimeRemaining}");
                    ImGui.TextUnformatted($"血壤Bloodfest-{GetCooldownRemainingTime(GNB.血壤Bloodfest)}");
                    ImGui.TextUnformatted($"无情NoMercy-{GetCooldownRemainingTime(GNB.无情NoMercy)}");
                  
                    
                    // ImGui.TextUnformatted($"GetActionStatus-{ActionManager.Instance()->GetActionStatus(ActionType.Item, 29492+ 1000000)}");

                    // ImGui.TextUnformatted($"CooldownRemaining-{GetCooldown(DNC.喷泉Fountain).CooldownRemaining }");
                    // ImGui.TextUnformatted($"TotalCastTime-{LocalPlayer.TotalCastTime}");
                    // ImGui.TextUnformatted($"CurrentCastTime-{LocalPlayer.CurrentCastTime}");
                    // ImGui.TextUnformatted($"aGetPartySlot2-{CustomComboFunctions.GetPartySlot(2)?.Name}");
                    // ImGui.TextUnformatted($"GetPartySlot2-{CustomComboFunctions.GetPartySlot(3)?.Name}");

                    // ImGui.TextUnformatted($"厄运流转CircleOfScorn-{ActionReady(PLD.厄运流转CircleOfScorn)}");
                    // ImGui.TextUnformatted($"深奥之灵SpiritsWithin-{ActionReady(PLD.深奥之灵SpiritsWithin.OriginalHook())}");
                    // ImGui.TextUnformatted($"GetPartySlot1-{CustomComboFunctions.GetPartySlot(1)?.Name}");
                    // ImGui.TextUnformatted($"GetPartySlot2-{CustomComboFunctions.GetPartySlot(2)?.Name}");
                    // ImGui.TextUnformatted($"GetPartySlot2-{CustomComboFunctions.GetPartySlot(3)?.Name}");
                    // ImGui.TextUnformatted($"{GetResourceCost(PLD.圣灵HolySpirit)}");
                    // ImGui.TextUnformatted($"{LocalPlayer?.CurrentMount}");
                    // ImGui.TextUnformatted($"{LocalPlayer?.CurrentMount==null}");

                    // ImGui.TextUnformatted($"{DRK.血溅Bloodspiller.OriginalHook()}");
                    // ImGui.TextUnformatted($"{DRK.血乱层数()}");
                    // ImGui.TextUnformatted($"{GetCooldownRemainingTime(DRK.血乱Delirium) + GetCooldownRemainingTime(DRK.血溅Bloodspiller)}");
                    // ImGui.TextUnformatted($"{HasEffect(GNB.Buffs.ReadyToRaze命运之印预备)}");
                    // ImGui.TextUnformatted($"安魂祈祷Requiescat:{PLD.安魂祈祷Requiescat.OriginalHook().ActionReady()}");
                    // VPRGauge? gauge = GetJobGauge<VPRGauge>();
                    // ImGui.TextUnformatted($"VPRGauge:{gauge.DreadCombo}");
                }
            }

            else
            {
                ImGui.TextUnformatted("Please log into the game to use this tab.");
            }
        }
    }
}
#endif