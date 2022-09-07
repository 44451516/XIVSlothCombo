using ImGuiNET;
using System;
using System.Numerics;
using XIVSlothCombo.Services;

namespace XIVSlothCombo.Window.Tabs
{
    internal class Settings : ConfigWindow
    {
        internal static new void Draw()
        {
            ImGui.BeginChild("main", new Vector2(0, 0), true);
            //ImGui.Text("This tab allows you to customise your options when enabling features.");
            ImGui.Text("这个选项卡可以在启用功能时定制你的选项。");

            #region SubCombos

            var hideChildren = Service.Configuration.HideChildren;

            //if (ImGui.Checkbox("Hide SubCombo Options", ref hideChildren))
            if (ImGui.Checkbox("隐藏子选项", ref hideChildren))
            {
                Service.Configuration.HideChildren = hideChildren;
                Service.Configuration.Save();
            }

            if (ImGui.IsItemHovered())
            {
                ImGui.BeginTooltip();
                //ImGui.TextUnformatted("Hides the sub-options of disabled features.");
                ImGui.TextUnformatted("隐藏所有已禁用功能的子选项。");
                ImGui.EndTooltip();
            }
            ImGui.NextColumn();

            #endregion

            #region Conflicting

            var hideConflicting = Service.Configuration.HideConflictedCombos;
            //if (ImGui.Checkbox("Hide Conflicted Combos", ref hideConflicting))
            if (ImGui.Checkbox("隐藏冲突功能", ref hideConflicting))
            {
                Service.Configuration.HideConflictedCombos = hideConflicting;
                Service.Configuration.Save();
            }

            if (ImGui.IsItemHovered())
            {
                ImGui.BeginTooltip();
                //ImGui.TextUnformatted("Hides any combos that conflict with others you have selected.");
                ImGui.TextUnformatted("隐藏所有与已启用功能冲突的功能。");
                ImGui.EndTooltip();
            }

            #endregion

            #region Combat Log

            var showCombatLog = Service.Configuration.EnabledOutputLog;

            //if (ImGui.Checkbox("Output Log to Chat", ref showCombatLog))
            if (ImGui.Checkbox("日志输出到聊天框", ref showCombatLog))
            {
                Service.Configuration.EnabledOutputLog = showCombatLog;
                Service.Configuration.Save();
            }

            if (ImGui.IsItemHovered())
            {
                ImGui.BeginTooltip();
                //ImGui.TextUnformatted("Every time you use an action, the plugin will print it to the chat.");
                ImGui.TextUnformatted("把你对插件的操作实时输出到聊天框中。");
                ImGui.EndTooltip();
            }
            #endregion

            #region SpecialEvent

            var isSpecialEvent = DateTime.Now.Day == 1 && DateTime.Now.Month == 4;
            var slothIrl = isSpecialEvent && Service.Configuration.SpecialEvent;

            if (isSpecialEvent)

            {

                if (ImGui.Checkbox("Sloth Mode!?", ref slothIrl))
                {
                    Service.Configuration.SpecialEvent = slothIrl;
                    Service.Configuration.Save();
                }
            }

            else
            {
                Service.Configuration.SpecialEvent = false;
                Service.Configuration.Save();
            }

            float offset = (float)Service.Configuration.MeleeOffset;
            ImGui.PushItemWidth(75);

            var inputChangedeth = false;
            //inputChangedeth |= ImGui.InputFloat("Melee Distance Offset", ref offset);
            inputChangedeth |= ImGui.InputFloat("近战距离偏移量", ref offset);

            if (inputChangedeth)
            {
                Service.Configuration.MeleeOffset = (double)offset;
                Service.Configuration.Save();
            }

            if (ImGui.IsItemHovered())
            {
                ImGui.BeginTooltip();
                //ImGui.TextUnformatted("Offset of melee check distance for features that use it. For those who don't want to immediately use their ranged attack if the boss walks slightly out of range.");
                ImGui.TextUnformatted("如果不想 BOSS 稍微走出近战范围就立即使用远程攻击，那么可以对有近战距离检查的功能设置一个偏移量。");
                ImGui.EndTooltip();
            }

            #endregion

            #region Message of the Day

            var motd = Service.Configuration.HideMessageOfTheDay;

            //if (ImGui.Checkbox("Hide Message of the Day", ref motd))
            if (ImGui.Checkbox("关闭\"今日消息\"", ref motd))
            {
                Service.Configuration.HideMessageOfTheDay = motd;
                Service.Configuration.Save();
            }

            if (ImGui.IsItemHovered())
            {
                ImGui.BeginTooltip();
                //ImGui.TextUnformatted("Disables the Message of the Day message in your chat when you login.");
                ImGui.TextUnformatted("禁用登录时聊天框中的\"今日消息\"。");
                ImGui.EndTooltip();
            }
            ImGui.NextColumn();

            #endregion

            #region 输出到聊天框

            
            var setOutChat = Service.Configuration.SetOutChat;

            //if (ImGui.Checkbox("Hide Message of the Day", ref motd))
            if (ImGui.Checkbox("" + "Set输出到聊天" + "", ref setOutChat))
            {
                Service.Configuration.SetOutChat = setOutChat;
                Service.Configuration.Save();
            }

            #endregion

            #region 语言

            var language = Service.Configuration.Language;

            {
                // ImGui.SetNextItemWidth(50);
                if (ImGui.BeginCombo($"语言", language, ImGuiComboFlags.NoArrowButton))
                {

                    if (ImGui.Selectable("简体中文", language == "zh-CN"))
                    {
                        Service.Configuration.Language = "简体中文";
                        // PluginLog.Information("选择了中文");
                        Service.Configuration.Save();
                    }

                    if (ImGui.Selectable("English", language == "en"))
                    {
                        Service.Configuration.Language = "English";
                        // PluginLog.Information("选择了en");
                        Service.Configuration.Save();
                    }


                    ImGui.EndCombo();
                }
            }

            #endregion

            ImGui.EndChild();
        }
    }
}
