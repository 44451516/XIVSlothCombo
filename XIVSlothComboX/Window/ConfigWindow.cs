using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using Dalamud.Interface;
using Dalamud.Interface.ManagedFontAtlas;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Utility;
using ECommons.DalamudServices;
using ECommons.ImGuiMethods;
using FFXIVClientStructs.FFXIV.Client.UI;
using FFXIVClientStructs.FFXIV.Component.GUI;
using ImGuiNET;
using XIVSlothComboX.Attributes;
using XIVSlothComboX.Combos;
using XIVSlothComboX.Combos.PvE;
using XIVSlothComboX.Core;
using XIVSlothComboX.CustomComboNS.Functions;
using XIVSlothComboX.Services;
using XIVSlothComboX.Window.Tabs;

namespace XIVSlothComboX.Window
{
    /// <summary> Plugin configuration window. </summary>
    internal class ConfigWindow : Dalamud.Interface.Windowing.Window
    {
        internal static readonly Dictionary<string, List<(CustomComboPreset Preset, CustomComboInfoAttribute Info)>> groupedPresets =
            GetGroupedPresets();

        internal static readonly Dictionary<CustomComboPreset, (CustomComboPreset Preset, CustomComboInfoAttribute Info)[]> presetChildren =
            GetPresetChildren();

        internal static Dictionary<string, List<(CustomComboPreset Preset, CustomComboInfoAttribute Info)>> GetGroupedPresets()
        {
            return Enum
                .GetValues<CustomComboPreset>()
                .Where(preset => (int)preset > 100 && preset != CustomComboPreset.Disabled)
                .Select(preset => (Preset: preset, Info: preset.GetAttribute<CustomComboInfoAttribute>()))
                .Where(tpl => tpl.Info != null && PresetStorage.GetParent(tpl.Preset) == null)
                .OrderByDescending(tpl => tpl.Info.JobID == 0)
                .ThenByDescending(tpl => tpl.Info.JobID == DOL.JobID)
                .ThenByDescending(tpl => tpl.Info.JobID == DOH.JobID)
                .ThenByDescending(tpl => tpl.Info.Role == 1)
                .ThenByDescending(tpl => tpl.Info.Role == 4)
                .ThenByDescending(tpl => tpl.Info.Role == 2)
                .ThenByDescending(tpl => tpl.Info.Role == 3)
                .ThenBy(tpl => tpl.Info.ClassJobCategory)
                .ThenBy(tpl => tpl.Info.JobName)
                .ThenBy(tpl => tpl.Info.Order)
                .GroupBy(tpl => tpl.Info.JobName)
                .ToDictionary(
                    tpl => tpl.Key,
                    tpl => tpl.ToList())!;
        }

        internal static Dictionary<CustomComboPreset, (CustomComboPreset Preset, CustomComboInfoAttribute Info)[]> GetPresetChildren()
        {
            var childCombos = Enum.GetValues<CustomComboPreset>().ToDictionary(
                tpl => tpl,
                tpl => new List<CustomComboPreset>());

            foreach (CustomComboPreset preset in Enum.GetValues<CustomComboPreset>())
            {
                CustomComboPreset? parent = preset.GetAttribute<ParentComboAttribute>()?.ParentPreset;
                if (parent != null)
                    childCombos[parent.Value].Add(preset);
            }

            return childCombos.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value
                    .Select(preset => (Preset: preset, Info: preset.GetAttribute<CustomComboInfoAttribute>()))
                    .OrderBy(tpl => tpl.Info.Order).ToArray())!;
        }

        public OpenWindowEnum OpenWindow { get; set; } = OpenWindowEnum.PvE;

        /// <summary> Initializes a new instance of the <see cref="ConfigWindow"/> class. </summary>
        public ConfigWindow() : base($"{P.Name} {P.GetType().Assembly.GetName().Version}###SlothComboX")
        {
            RespectCloseHotkey = true;

            SizeCondition = ImGuiCond.FirstUseEver;
            Size = new Vector2(800, 650);
            SetMinSize();

            Service.Interface.UiBuilder.DefaultFontHandle.ImFontChanged += SetMinSize;
        }


        private void SetMinSize(IFontHandle? fontHandle = null, ILockedImFont? lockedFont = null)
        {
            SizeConstraints = new()
            {
                MinimumSize = new Vector2(700f.Scale(), 10f.Scale())
            };
        }

        public override void Draw()
        {
            var region = ImGui.GetContentRegionAvail();
            var itemSpacing = ImGui.GetStyle().ItemSpacing;

            var topLeftSideHeight = region.Y;

            using (var style = ImRaii.PushStyle(ImGuiStyleVar.CellPadding, new Vector2(4, 0)))
            {
                using (var table = ImRaii.Table("###MainTable", 2, ImGuiTableFlags.Resizable))
                {
                    if (!table)
                        return;


                    ImGui.TableSetupColumn("##LeftColumn", ImGuiTableColumnFlags.WidthFixed, ImGui.GetWindowWidth() / 3);

                    ImGui.TableNextColumn();

                    var regionSize = ImGui.GetContentRegionAvail();

                    ImGui.PushStyleVar(ImGuiStyleVar.SelectableTextAlign, new Vector2(0.5f, 0.5f));

                    using (var leftChild = ImRaii.Child($"###SlothLeftSide", regionSize with { Y = topLeftSideHeight }, false,
                               ImGuiWindowFlags.NoDecoration))
                    {
                        // if (ThreadLoadImageHandler.TryGetTextureWrap(
                        //         @"https://github.com/Taurenkey/XIVSlothCombo/blob/main/res/plugin/xivslothcombo.png?raw=true", out var logo))
                        // {
                        //     ImGuiEx.LineCentered("###SlothLogo", () => { ImGui.Image(logo.ImGuiHandle, new(125f.Scale(), 125f.Scale())); });
                        // }
                        //
                        // ImGui.Spacing();
                        // ImGui.Separator();

                        if (ImGui.Selectable("PvE 功能", OpenWindow == OpenWindowEnum.PvE))
                        {
                            OpenWindow = OpenWindowEnum.PvE;
                        }

                        if (ImGui.Selectable("PvP 功能", OpenWindow == OpenWindowEnum.PvP))
                        {
                            OpenWindow = OpenWindowEnum.PvP;
                        }

                        ImGui.Spacing();
                        if (ImGui.Selectable("时间轴编辑", OpenWindow == OpenWindowEnum.TimelineEdit))
                        {
                            OpenWindow = OpenWindowEnum.TimelineEdit;
                        }

                        ImGui.Spacing();
                        if (ImGui.Selectable("时间轴", OpenWindow == OpenWindowEnum.TimelineMain))
                        {
                            OpenWindow = OpenWindowEnum.TimelineMain;
                        }


                        ImGui.Spacing();
                        if (ImGui.Selectable("综合设置", OpenWindow == OpenWindowEnum.Settings))
                        {
                            OpenWindow = OpenWindowEnum.Settings;
                        }


                        ImGui.Spacing();
                        if (ImGui.Selectable("提出建议", OpenWindow == OpenWindowEnum.About))
                        {
                            OpenWindow = OpenWindowEnum.About;
                        }
                        
                        ImGui.Spacing();
                        if (ImGui.Selectable("爱发电", OpenWindow == OpenWindowEnum.AiFaDian))
                        {
                            OpenWindow = OpenWindowEnum.AiFaDian;
                        }

#if DEBUG
                        ImGui.Spacing();
                        if (ImGui.Selectable("调试模式", OpenWindow == OpenWindowEnum.Debug))
                        {
                            OpenWindow = OpenWindowEnum.Debug;
                        }

                        ImGui.Spacing();
#endif
                    }

                    ImGui.PopStyleVar();
                    ImGui.TableNextColumn();
                    using (var rightChild = ImRaii.Child($"###SlothRightSide", Vector2.Zero, false))
                    {
                        switch (OpenWindow)
                        {
                            case OpenWindowEnum.PvE:
                                PvEFeatures.Draw();
                                break;
                            case OpenWindowEnum.PvP:
                                PvPFeatures.Draw();
                                break;
                            case OpenWindowEnum.Settings:
                                Settings.Draw();
                                break;
                            case OpenWindowEnum.About:
                                AboutUs.Draw();
                                break;
#if DEBUG
                            case OpenWindowEnum.Debug:
                                Debug.Draw();
                                break;
#endif
                            case OpenWindowEnum.TimelineEdit:
                                TimelineEditWindows.Draw();
                                break;
                            
                            case OpenWindowEnum.TimelineMain:
                                TimelineMainWindows.Draw();
                                break;
                            
                            case OpenWindowEnum.AiFaDian:
                                AiFaDianWindows.Draw();
                                break;
                            default:
                                break;
                        }

                        ;
                    }
                }
            }
        }

        public void Dispose()
        {
            Service.Interface.UiBuilder.DefaultFontHandle.ImFontChanged -= SetMinSize;
        }

        public enum OpenWindowEnum
        {
            None = 0,
            PvE = 1,
            PvP = 2,
            Settings = 3,
            About = 4,
            Debug = 5,
            TimelineEdit = 6,
            TimelineMain = 7,
            AiFaDian = 8,
            // TimelineMain = 9,
        }
    }
}