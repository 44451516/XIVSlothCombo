using System;
using System.Numerics;
using ImGuiNET;
using XIVSlothComboX.Attributes;
using XIVSlothComboX.Services;

namespace XIVSlothComboX.Window.Tabs
{
    internal class Settings : ConfigWindow
    {
        internal static new void Draw()
        {
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
            
            
            ImGui.EndChild();
        }
    }
}
