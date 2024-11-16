using System.Collections.Generic;
using System.Numerics;
using Dalamud.Interface;
using Dalamud.Interface.Colors;
using Dalamud.Interface.Internal;
using Dalamud.Interface.Textures.TextureWraps;
using Dalamud.Interface.Utility;
using Dalamud.Utility;
using ECommons.ImGuiMethods;
using ECommons.LanguageHelpers;
using ECommons.Logging;
using ImGuiNET;
using Lumina.Excel;
using Newtonsoft.Json;
using XIVSlothComboX.Combos.PvE;
using XIVSlothComboX.Core;
using XIVSlothComboX.CustomComboNS.Functions;
using XIVSlothComboX.Data;
using XIVSlothComboX.Services;
using Action = Lumina.Excel.Sheets.Action;

namespace XIVSlothComboX.Window.Tabs
{
    internal class AiFaDianWindows : ConfigWindow
    {
        internal new static void Draw()
        {
            ImGui.TextColored(ImGuiColors.ParsedGreen, $"如果你认可我的工作, 可以给我买杯蜜雪冰城，每一份支持都是在传递善意\n注: 捐赠均为【无偿】性质, 我无法因为捐赠给出任何承诺或回报, 请务必三思而后行");
            ImGui.TextColored(ImGuiColors.DPSRed, $"如果你认可我的工作, 可以给我买杯蜜雪冰城，每一份支持都是在传递善意\n注: 捐赠均为【无偿】性质, 我无法因为捐赠给出任何承诺或回报, 请务必三思而后行");
            ImGui.TextColored(ImGuiColors.ParsedGreen, $"如果你认可我的工作, 可以给我买杯蜜雪冰城，每一份支持都是在传递善意\n注: 捐赠均为【无偿】性质, 我无法因为捐赠给出任何承诺或回报, 请务必三思而后行");
            
            if (ImGui.Button("爱发电"))
            {
                Util.OpenLink("https://afdian.com/a/a_44451516");
            }  

        }
    }
}