using System;
using System.Linq;
using System.Numerics;
using Dalamud.Bindings.ImGui;
using XIVSlothComboX.Attributes;
using XIVSlothComboX.Services;
using XIVSlothComboX.自动类;

namespace XIVSlothComboX.Window.Tabs
{
    internal class Settings : ConfigWindow
    {
        public static uint RequiredFood = 0;
        public static uint RequiredPotion = 0;
        public static bool RequiredFoodHQ = true;

        internal static new void Draw()
        {
            PvEFeatures.HasToOpenJob = true;
            ImGui.BeginChild("main", new Vector2(0, 0), true);
            ImGui.Text("此选项卡允许您在启用功能时自定义选项");

            #region SubCombos

            bool hideChildren = Service.Configuration.HideChildren;

            if (ImGui.Checkbox("隐藏子连击选项[可能有BUG]", ref hideChildren))
            {
                Service.Configuration.HideChildren = hideChildren;
                Service.Configuration.Save();
            }

            if (ImGui.IsItemHovered())
            {
                ImGui.BeginTooltip();
                ImGui.TextUnformatted("隐藏已禁用功能的子选项[可能有BUG]");
                ImGui.EndTooltip();
            }
            ImGui.NextColumn();

            #endregion

            #region Conflicting

            bool hideConflicting = Service.Configuration.HideConflictedCombos;
            if (ImGui.Checkbox("隐藏冲突的连击[可能有BUG]", ref hideConflicting))
            {
                Service.Configuration.HideConflictedCombos = hideConflicting;
                Service.Configuration.Save();
            }

            if (ImGui.IsItemHovered())
            {
                ImGui.BeginTooltip();
                ImGui.TextUnformatted("隐藏与您选择的其他连击冲突的任何连击");
                ImGui.EndTooltip();
            }

            #endregion

            #region Combat Log

            bool showCombatLog = Service.Configuration.EnabledOutputLog;

            if (ImGui.Checkbox("向聊天框输出日志 Output Log to Chat", ref showCombatLog))
            {
                Service.Configuration.EnabledOutputLog = showCombatLog;
                Service.Configuration.Save();
            }

            if (ImGui.IsItemHovered())
            {
                ImGui.BeginTooltip();
                ImGui.TextUnformatted("每次使用一个技能，插件都会将其输出到聊天框中。Every time you use an action, the plugin will print it to the chat.");
                ImGui.EndTooltip();
            }

            #endregion

            #region SpecialEvent

            float offset = (float)Service.Configuration.MeleeOffset;
            ImGui.PushItemWidth(75);

            bool inputChangedeth = false;
            inputChangedeth |= ImGui.InputFloat("近战距离偏移量", ref offset);

            if (inputChangedeth)
            {
                Service.Configuration.MeleeOffset = (double)offset;
                Service.Configuration.Save();
            }

            if (ImGui.IsItemHovered())
            {
                ImGui.BeginTooltip();
                ImGui.TextUnformatted("检查近战距离的偏移量，对于有偏移量的功能。对于那些不希望在Boss稍微超出射程时立即使用远程攻击的人来说，这非常有用。 ");
                ImGui.EndTooltip();
            }

            #endregion

            #region Message of the Day

            bool motd = Service.Configuration.HideMessageOfTheDay;

            if (ImGui.Checkbox("隐藏登录资讯", ref motd))
            {
                Service.Configuration.HideMessageOfTheDay = motd;
                Service.Configuration.Save();
            }

            if (ImGui.IsItemHovered())
            {
                ImGui.BeginTooltip();
                ImGui.TextUnformatted("当您登录时，禁用聊天中的系统提醒。");
                ImGui.EndTooltip();
            }
            ImGui.NextColumn();

            #endregion

            Vector4 colour = Service.Configuration.TargetHighlightColor;
            if (ImGui.ColorEdit4("目标高亮颜色 Target Highlight Colour", ref colour, ImGuiColorEditFlags.NoInputs | ImGuiColorEditFlags.AlphaPreview | ImGuiColorEditFlags.AlphaBar))
            {
                Service.Configuration.TargetHighlightColor = colour;
                Service.Configuration.Save();
            }

            // if (ImGui.IsItemHovered())
            // {
            //     ImGui.BeginTooltip();
            //     ImGui.TextUnformatted($"Used for {CustomComboInfoAttribute.JobIDToName(33)} card targeting features.\r\nSet Alpha to 0 to hide the box.");
            // }
            //

            #region 输出到聊天框

            var setOutChat = Service.Configuration.SetOutChat;

            //if (ImGui.Checkbox("Hide Message of the Day", ref motd))
            if (ImGui.Checkbox("" + "Set输出到聊天" + "", ref setOutChat))
            {
                Service.Configuration.SetOutChat = setOutChat;
                Service.Configuration.Save();
            }

            #endregion

            #region 自动精炼

            var 自动精炼 = Service.Configuration.自动精炼;

            if (ImGui.Checkbox("" + "自动精炼" + "", ref 自动精炼))
            {
                Service.Configuration.自动精炼 = 自动精炼;
                Service.Configuration.Save();
            }

            var 只精炼亚力山大 = Service.Configuration.只精炼亚力山大;

            ImGui.SameLine();
            if (ImGui.Checkbox("" + "只精炼亚力山大" + "", ref 只精炼亚力山大))
            {
                Service.Configuration.只精炼亚力山大 = 只精炼亚力山大;
                Service.Configuration.Save();
            }

            #endregion

            #region 自动食物

            var 自动食物 = Service.Configuration.自动食物;

            if (ImGui.Checkbox("" + "自动食物" + "", ref 自动食物))
            {
                Service.Configuration.自动食物 = 自动食物;
                Service.Configuration.Save();
            }

            ImGui.SameLine();
            if (ImGui.BeginCombo("##foodBuff", RequiredFood == 0 ? "Disabled" : $"{(RequiredFoodHQ ? " " : "")}{ConsumableChecker.Food.FirstOrDefault(x => x.Id == RequiredFood).Name}"))
            {
                if (ImGui.Selectable("Disable"))
                {
                    Service.Configuration.自动食物Id = 0;
                    Service.Configuration.RequiredFoodHQ = false;
                    Service.Configuration.Save();
                }
                foreach (var x in ConsumableChecker.GetFood(true))
                {
                    if (ImGui.Selectable($"{x.Name}"))
                    {
                        Service.Configuration.自动食物Id = x.Id;
                        Service.Configuration.RequiredFoodHQ = false;
                        Service.Configuration.Save();
                    }
                }
                foreach (var x in ConsumableChecker.GetFood(true, true))
                {
                    if (ImGui.Selectable($" {x.Name}"))
                    {
                        Service.Configuration.自动食物Id = x.Id;
                        Service.Configuration.RequiredFoodHQ = true;
                        Service.Configuration.Save();
                    }
                }
                ImGui.EndCombo();
            }

            #endregion

            #region 自动精炼药

            var 自动精炼药 = Service.Configuration.自动精炼药;
            if (ImGui.Checkbox("" + "自动精炼药" + "", ref 自动精炼药))
            {
                Service.Configuration.自动精炼药 = 自动精炼药;
                Service.Configuration.Save();
            }

            #endregion
            
            // ImGui.PushItemWidth(75);
            if (ImGui.InputFloat("移动延时", ref Service.Configuration.MovementLeeway))
                Service.Configuration.Save();

            // if (ImGui.Checkbox("Performance Mode", ref Service.Configuration.PerformanceMode))
            //     Service.Configuration.Save();
            
            ImGui.EndChild();
        }
    }
}