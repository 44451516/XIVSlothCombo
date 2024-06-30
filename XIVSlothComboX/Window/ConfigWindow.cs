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
        public ConfigWindow() : base($"{P.Name} {P.GetType().Assembly.GetName().Version}###Artisan")
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
            DrawTargetHelper();

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
                                // P.AboutUs.Draw();
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
                            default:
                                break;
                        }

                        ;
                    }
                }
            }
        }


        public void DrawConfig()
        {
            DrawTargetHelper();
        }

        private unsafe void DrawTargetHelper()
        {
            if (Combos.JobHelpers.AST.AST_QuickTargetCards.SelectedRandomMember is not null)
            {
                for (int i = 1; i <= 8; i++)
                {
                    if (CustomComboFunctions.GetPartySlot(i) == Combos.JobHelpers.AST.AST_QuickTargetCards.SelectedRandomMember)
                    {
                        IntPtr partyPTR = Service.GameGui.GetAddonByName("_PartyList", 1);
                        if (partyPTR == IntPtr.Zero)
                            return;

                        AddonPartyList plist = Marshal.PtrToStructure<AddonPartyList>(partyPTR);


                        // plist.PartyMember.PartyMember0;

                        // var member = i switch
                        // {
                        //     1 => plist.PartyMember.PartyMember0.TargetGlow,
                        //     2 => plist.PartyMember.PartyMember1.TargetGlow,
                        //     3 => plist.PartyMember.PartyMember2.TargetGlow,
                        //     4 => plist.PartyMember.PartyMember3.TargetGlow,
                        //     5 => plist.PartyMember.PartyMember4.TargetGlow,
                        //     6 => plist.PartyMember.PartyMember5.TargetGlow,
                        //     7 => plist.PartyMember.PartyMember6.TargetGlow,
                        //     8 => plist.PartyMember.PartyMember7.TargetGlow,
                        //     _ => plist.PartyMember.PartyMember0.TargetGlow,
                        // };   

                        var member = i switch
                        {
                            1 => plist.PartyMembers[0].TargetGlow,
                            2 => plist.PartyMembers[1].TargetGlow,
                            3 => plist.PartyMembers[2].TargetGlow,
                            4 => plist.PartyMembers[3].TargetGlow,
                            5 => plist.PartyMembers[4].TargetGlow,
                            6 => plist.PartyMembers[5].TargetGlow,
                            7 => plist.PartyMembers[6].TargetGlow,
                            8 => plist.PartyMembers[7].TargetGlow,
                            _ => plist.PartyMembers[0].TargetGlow,
                        };

                        DrawOutline(member->AtkResNode.PrevSiblingNode);
                    }
                }
            }
        }

        private unsafe void DrawOutline(AtkResNode* node)
        {
            var position = GetNodePosition(node);
            var scale = GetNodeScale(node);
            var size = new Vector2(node->Width, node->Height) * scale;

            position += ImGuiHelpers.MainViewport.Pos;

            var colour = Service.Configuration.TargetHighlightColor;
            ImGui.GetForegroundDrawList(ImGuiHelpers.MainViewport)
                .AddRect(position, position + size, ImGui.GetColorU32(colour), 0, ImDrawFlags.RoundCornersAll, 2);
        }

        public unsafe Vector2 GetNodePosition(AtkResNode* node)
        {
            var pos = new Vector2(node->X, node->Y);
            var par = node->ParentNode;
            while (par != null)
            {
                pos *= new Vector2(par->ScaleX, par->ScaleY);
                pos += new Vector2(par->X, par->Y);
                par = par->ParentNode;
            }

            return pos;
        }

        public unsafe Vector2 GetNodeScale(AtkResNode* node)
        {
            if (node == null) return new Vector2(1, 1);
            var scale = new Vector2(node->ScaleX, node->ScaleY);
            while (node->ParentNode != null)
            {
                node = node->ParentNode;
                scale *= new Vector2(node->ScaleX, node->ScaleY);
            }

            return scale;
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
        }
    }
}