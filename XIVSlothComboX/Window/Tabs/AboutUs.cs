using System;
using Dalamud.Interface.Colors;
using Dalamud.Utility;
using ImGuiNET;
using System.Numerics;
using System.Reflection;

namespace XIVSlothComboX.Window.Tabs
{
    internal class AboutUs : ConfigWindow
    {
        public static Version version = null!;

        public const string Ban = "[XIVSlothComboX]请不要去卫月频道交流此插件,包括提BUG";
        
        internal static new void Draw()
        {
            
            PvEFeatures.HasToOpenJob = true;
            version ??= Assembly.GetExecutingAssembly().GetName().Version!;
            
            PvEFeatures.HasToOpenJob = true;

            ImGui.BeginChild("About", new Vector2(0, 0), true);

            ImGui.TextColored(ImGuiColors.ParsedGreen, $"XIVSlothComboX - v{version}");
            // ImGui.Spacing();
            // ImGui.Spacing();
            // ImGui.Spacing();
            // ImGui.TextWrapped("Brought to you by: \nAki, k-kz, ele-starshade, damolitionn, Taurenkey, Augporto, grimgal, Genesis-Nova, Tartarga and many other contributors!");
            // ImGui.Spacing();
            // ImGui.Spacing();
            // ImGui.Spacing();
            // ImGui.PushStyleColor(ImGuiCol.Button, ImGuiColors.ParsedPurple);
            // ImGui.PushStyleColor(ImGuiCol.ButtonHovered, ImGuiColors.HealerGreen);

         
            if (ImGui.Button("有问题来这里提"))
            {
                Util.OpenLink("https://docs.qq.com/sheet/DT0JZdlVHaklSSkN6&tab=cr8tma");
                // Util.OpenLink("https://github.com/44451516/XIVSlothCombo/issues");
            }

            // ImGui.PopStyleColor();
            // ImGui.PopStyleColor();

       
            if (ImGui.Button("交流群"))
            {
                // Util.OpenLink("https://qm.qq.com/q/eBaUVYbKpi");
                // 点击链接加入群聊【XIVSlothCombo汉化】：https://qm.qq.com/q/5UdSTydlfi
                Util.OpenLink("https://qm.qq.com/q/5UdSTydlfi");
            }

            ImGui.Spacing();
            ImGui.Spacing();
            ImGui.Spacing();
            ImGui.Spacing();
            // ImGui.TextColored(ImGuiColors.ParsedGreen, $"由于写的汉化脚本不算智能，部分区域未匹配到而没有汉化。");
            // ImGui.TextColored(ImGuiColors.ParsedGreen, $"如果你要是从某个咸鱼小铺买的，恭喜你，上大当了，请立刻退款差评免得被人嘲笑。");
            // ImGui.TextColored(ImGuiColors.ParsedGreen, $"温馨提醒：怂就别开，但开别怂，少开点挂。");
            ImGui.Spacing();
            ImGui.Spacing();
            ImGui.Spacing();
            ImGui.TextColored(ImGuiColors.DPSRed, $"{Ban}");
            ImGui.TextColored(ImGuiColors.DPSRed, $"{Ban}");
            ImGui.TextColored(ImGuiColors.DPSRed, $"{Ban}");
            ImGui.EndChild();
        }
    }
}
