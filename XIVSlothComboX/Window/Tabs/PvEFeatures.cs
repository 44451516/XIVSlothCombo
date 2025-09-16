using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Dalamud.Interface.Textures.TextureWraps;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Utility.Raii;
using ECommons.ImGuiMethods;
using Dalamud.Bindings.ImGui;
using XIVSlothComboX.Combos.PvE;
using XIVSlothComboX.Core;
using XIVSlothComboX.Services;
using XIVSlothComboX.Window.Functions;
using XIVSlothComboX.Window.MessagesNS;

namespace XIVSlothComboX.Window.Tabs
{
    internal class PvEFeatures : ConfigWindow
    {
        //internal static Dictionary<string, bool> showHeader = new Dictionary<string, bool>();

        public static readonly Dictionary<uint,string> JobsPVEInfoDictionary = new ()
        {
            {WAR.JobID,"7.0"},
            {PLD.JobID,"7.0"},
            {DRK.JobID,"7.0"},
            {GNB.JobID,"7.0"},
            
            {WHM.JobID,"7.0"},
            {SCH.JobID,"7.0"},
            {AST.JobID,"7.0"},
            {SGE.JobID,"7.0"},
            
            {BRD.JobID,"7.0"},
            {MCH.JobID,"7.0"},
            {DNC.JobID,"7.0"},
            
            
            {BLM.JobID,"7.0"},
            {SMN.JobID,"7.0"},
            {RDM.JobID,"7.0"},
            {PCT.JobID,"7.0"},
            
            {MNK.JobID,"7.0"},
            {DRG.JobID,"7.0"},
            {NIN.JobID,"7.0"},
            {SAM.JobID,"7.0"},
            {RPR.JobID,"7.0"},
            {VPR.JobID,"7.0"},
            {BLU.JobID,"6.X"},
        };
        
        internal static bool HasToOpenJob = true;
        internal static string OpenJob = string.Empty;

        internal static new void Draw()
        {
            //#if !DEBUG
            // if (IconReplacer.ClassLocked())
            // {
            //     ImGui.TextWrapped("Equip your job stone to re-unlock features.");
            //     return;
            // }
            //#endif

            using (var scrolling = ImRaii.Child("scrolling", new Vector2(ImGui.GetContentRegionAvail().X, ImGui.GetContentRegionAvail().Y), true))
            {
                int i = 1;
                var indentwidth = 12f.Scale();
                var indentwidth2 = indentwidth + 42f.Scale();
                if (OpenJob == string.Empty)
                {
                    ImGui.SameLine(indentwidth);
                    ImGuiEx.LineCentered(() =>
                    {
                        // ImGuiEx.TextUnderlined("Select a job from below to enable and configure features for it.");
                        ImGuiEx.TextUnderlined("从下面选择一项任务，为其启用和配置功能。.");
                    });

                    foreach (string? jobName in groupedPresets.Keys)
                    {
                        string abbreviation = groupedPresets[jobName].First().Info.JobShorthand;
                        var jobId = groupedPresets[jobName].First().Info.JobID;
                        string jobVersion = string.Empty;
                        if (JobsPVEInfoDictionary.TryGetValue(jobId, out var value))
                        {
                            jobVersion = value;
                        }

                        string header = string.IsNullOrEmpty(abbreviation) ? jobName : $"{jobName} - {abbreviation} [{jobVersion}]";
                        IDalamudTextureWrap? icon = Icons.GetJobIcon(jobId);
                        using (var disabled = ImRaii.Disabled(DisabledJobsPVE.Any(x => x == jobId)))
                        { 
                            if (ImGui.Selectable($"###{header}", OpenJob == jobName, ImGuiSelectableFlags.None, icon == null ? new Vector2(0) : new Vector2(0, (icon.Size.Y / 2f).Scale())))
                            {
                                OpenJob = jobName;
                            }

                            ImGui.SameLine(indentwidth);
                            if (icon != null)
                            {
                                ImGui.Image(icon.Handle, new Vector2(icon.Size.X.Scale(), icon.Size.Y.Scale()) / 2f);
                                ImGui.SameLine(indentwidth2);
                            }
                            else
                            {
                                // PluginLog.Error($"确少的-{jobId}");
                            }

                            ImGui.Text($"{header} {(disabled ? "(因更新而禁用)" : "")}");
                        }
                    }
                }
                else
                {
                    var id = groupedPresets[OpenJob].First().Info.JobID;
                    if (id is ADV.JobID or DOL.JobID or DOH.JobID)
                    {
                        id = SCH.JobID;
                    }

                    IDalamudTextureWrap? icon = Icons.GetJobIcon(id);


                    if (icon != null)
                    {
                        
                        using (var headingTab = ImRaii.Child("HeadingTab", new Vector2(ImGui.GetContentRegionAvail().X, icon is null ? 24f.Scale() : (icon.Size.Y / 2f).Scale() + 4f)))
                        {
                            if (ImGui.Button("Back", new Vector2(0, 24f.Scale())))
                            {
                                OpenJob = "";
                                return;
                            }

                            ImGui.SameLine();
                            ImGuiEx.LineCentered(() =>
                            {
                                if (icon != null)
                                {
                                    ImGui.Image(icon.Handle, new Vector2(icon.Size.X.Scale(), icon.Size.Y.Scale()) / 2f);
                                    ImGui.SameLine();
                                }

                                ImGuiEx.Text($"{OpenJob}");
                            });
                        }

                        using (var contents = ImRaii.Child("Contents", new Vector2(0)))
                        {
                            try
                            {
                                if (ImGui.BeginTabBar($"subTab{OpenJob}", ImGuiTabBarFlags.Reorderable | ImGuiTabBarFlags.AutoSelectNewTabs))
                                {
                                    if (ImGui.BeginTabItem("常规"))
                                    {
                                        DrawHeadingContents(OpenJob, i);
                                        ImGui.EndTabItem();
                                    }

                                    if (groupedPresets[OpenJob].Any(x => PresetStorage.IsVariant(x.Preset)))
                                    {
                                        if (ImGui.BeginTabItem("下水道"))
                                        {
                                            DrawVariantContents(OpenJob);
                                            ImGui.EndTabItem();
                                        }
                                    }

                                    if (groupedPresets[OpenJob].Any(x => PresetStorage.IsBozja(x.Preset)))
                                    {
                                        if (ImGui.BeginTabItem("博兹雅"))
                                        {
                                            ImGui.EndTabItem();
                                        }
                                    }

                                    if (groupedPresets[OpenJob].Any(x => PresetStorage.IsEureka(x.Preset)))
                                    {
                                        if (ImGui.BeginTabItem("优雷卡"))
                                        {
                                            ImGui.EndTabItem();
                                        }
                                    }

                                    ImGui.EndTabBar();
                                }
                            }
                            catch
                            {
                            }
                        }
                    }
                }
            }
        }

        private static void DrawVariantContents(string jobName)
        {
            foreach (var (preset, info) in groupedPresets[jobName].Where(x => PresetStorage.IsVariant(x.Preset)))
            {
                int i = -1;
                InfoBox presetBox = new()
                {
                    Color = Colors.Grey, BorderThickness = 1f, CurveRadius = 8f, ContentsAction = () => { Presets.DrawPreset(preset, info, ref i); }
                };
                presetBox.Draw();
                ImGuiHelpers.ScaledDummy(12.0f);
            }
        }

        internal static void DrawHeadingContents(string jobName, int i)
        {
            if (!Messages.PrintBLUMessage(jobName)) return;

            foreach (var (preset, info) in groupedPresets[jobName].Where(x => !PresetStorage.IsPvP(x.Preset) &&
                                                                              !PresetStorage.IsVariant(x.Preset) &&
                                                                              !PresetStorage.IsBozja(x.Preset) &&
                                                                              !PresetStorage.IsEureka(x.Preset)))
            {
                InfoBox presetBox = new()
                {
                    Color = Colors.Grey, BorderThickness = 2f.Scale(), ContentsOffset = 5f.Scale(),
                    ContentsAction = () => { Presets.DrawPreset(preset, info, ref i); }
                };

                if (Service.Configuration.HideConflictedCombos)
                {
                    var conflictOriginals = PresetStorage.GetConflicts(preset); // Presets that are contained within a ConflictedAttribute
                    var conflictsSource = PresetStorage.GetAllConflicts(); // Presets with the ConflictedAttribute

                    if (!conflictsSource.Where(x => x == preset).Any() || conflictOriginals.Length == 0)
                    {
                        presetBox.Draw();
                        ImGuiHelpers.ScaledDummy(12.0f);
                        continue;
                    }

                    if (conflictOriginals.Any(x => PresetStorage.IsEnabled(x)))
                    {
                        Service.Configuration.EnabledActions.Remove(preset);
                        Service.Configuration.Save();
                    }

                    else
                    {
                        presetBox.Draw();

                        continue;
                    }
                }

                else
                {
                    presetBox.Draw();
                    ImGuiHelpers.ScaledDummy(12.0f);
                }
            }
        }

        internal static string? HeaderToOpen;
    }
}