using Dalamud.Game.ClientState.Objects.SubKinds;
using Dalamud.Interface;
using Dalamud.Interface.Colors;
using Dalamud.Utility;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using ECommons;
using XIVSlothComboX.Combos;
using XIVSlothComboX.Combos.PvE;
using XIVSlothComboX.Combos.PvP;
using XIVSlothComboX.Core;
using XIVSlothComboX.CustomComboNS.Functions;
using XIVSlothComboX.Services;
using XIVSlothComboX.Data;

namespace XIVSlothComboX.Window.Functions
{
    public static class UserConfig
    {
        /// <summary> Draws a slider that lets the user set a given value for their feature. </summary>
        /// <param name="minValue"> The absolute minimum value you'll let the user pick. </param>
        /// <param name="maxValue"> The absolute maximum value you'll let the user pick. </param>
        /// <param name="config"> The config ID. </param>
        /// <param name="sliderDescription"> Description of the slider. Appends to the right of the slider. </param>
        /// <param name="itemWidth"> How long the slider should be. </param>
        /// <param name="sliderIncrement"> How much you want the user to increment the slider by. Uses SliderIncrements as a preset. </param>
        /// <param name="hasAdditionalChoice">True if this config can trigger additional configs depending on value.</param>
        /// <param name="additonalChoiceCondition">What the condition is to convey to the user what triggers it.</param>
        public static void DrawSliderInt(int minValue, int maxValue, string config, string sliderDescription, float itemWidth = 150,
            uint sliderIncrement = SliderIncrements.Ones, bool hasAdditionalChoice = false, string additonalChoiceCondition = "")
        {
            ImGui.Indent();
            int output = PluginConfiguration.GetCustomIntValue(config, minValue);
            if (output < minValue)
            {
                output = minValue;
                PluginConfiguration.SetCustomIntValue(config, output);
                Service.Configuration.Save();
            }

            sliderDescription = sliderDescription.Replace("%", "%%");
            float contentRegionMin = ImGui.GetItemRectMax().Y - ImGui.GetItemRectMin().Y;
            float wrapPos = ImGui.GetContentRegionMax().X - 35f;

            InfoBox box = new()
            {
                Color = Colors.White,
                BorderThickness = 1f,
                CurveRadius = 3f,
                AutoResize = true,
                HasMaxWidth = true,
                IsSubBox = true,
                ContentsAction = () =>
                {
                    bool inputChanged = false;
                    Vector2 currentPos = ImGui.GetCursorPos();
                    ImGui.SetCursorPosX(currentPos.X + itemWidth);
                    ImGui.PushTextWrapPos(wrapPos);
                    ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.DalamudWhite);
                    ImGui.Text($"{sliderDescription}");
                    Vector2 height = ImGui.GetItemRectSize();
                    float lines = height.Y / ImGui.GetFontSize();
                    Vector2 textLength = ImGui.CalcTextSize(sliderDescription);
                    string newLines = "";
                    for (int i = 1; i < lines; i++)
                    {
                        if (i % 2 == 0)
                        {
                            newLines += "\n";
                        }
                        else
                        {
                            newLines += "\n\n";
                        }
                    }

                    if (hasAdditionalChoice)
                    {
                        ImGui.SameLine();
                        ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.HealerGreen);
                        ImGui.PushFont(UiBuilder.IconFont);
                        ImGui.Dummy(new Vector2(5, 0));
                        ImGui.SameLine();
                        ImGui.TextWrapped($"{FontAwesomeIcon.Search.ToIconString()}");
                        ImGui.PopFont();
                        ImGui.PopStyleColor();

                        if (ImGui.IsItemHovered())
                        {
                            ImGui.BeginTooltip();
                            ImGui.TextUnformatted(
                                $"This setting has additional options depending on its value.{(string.IsNullOrEmpty(additonalChoiceCondition) ? "" : $"\nCondition: {additonalChoiceCondition}")}");
                            ImGui.EndTooltip();
                        }
                    }

                    ImGui.PopStyleColor();
                    ImGui.PopTextWrapPos();
                    ImGui.SameLine();
                    ImGui.SetCursorPosX(currentPos.X);
                    ImGui.PushItemWidth(itemWidth);
                    inputChanged |= ImGui.SliderInt($"{newLines}###{config}", ref output, minValue, maxValue);

                    if (inputChanged)
                    {
                        if (output % sliderIncrement != 0)
                        {
                            output = output.RoundOff(sliderIncrement);
                            if (output < minValue) output = minValue;
                            if (output > maxValue) output = maxValue;
                        }

                        PluginConfiguration.SetCustomIntValue(config, output);
                        Service.Configuration.Save();
                    }
                }
            };

            box.Draw();
            ImGui.Spacing();
            ImGui.Unindent();
        }

        /// <summary> Draws a slider that lets the user set a given value for their feature. </summary>
        /// <param name="minValue"> The absolute minimum value you'll let the user pick. </param>
        /// <param name="maxValue"> The absolute maximum value you'll let the user pick. </param>
        /// <param name="config"> The config ID. </param>
        /// <param name="sliderDescription"> Description of the slider. Appends to the right of the slider. </param>
        /// <param name="itemWidth"> How long the slider should be. </param>
        /// <param name="hasAdditionalChoice"></param>
        /// <param name="additonalChoiceCondition"></param>
        public static void DrawSliderFloat(float minValue, float maxValue, string config, string sliderDescription, float itemWidth = 150,
            bool hasAdditionalChoice = false, string additonalChoiceCondition = "")
        {
            float output = PluginConfiguration.GetCustomFloatValue(config, minValue);
            if (output < minValue)
            {
                output = minValue;
                PluginConfiguration.SetCustomFloatValue(config, output);
                Service.Configuration.Save();
            }

            sliderDescription = sliderDescription.Replace("%", "%%");
            float contentRegionMin = ImGui.GetItemRectMax().Y - ImGui.GetItemRectMin().Y;
            float wrapPos = ImGui.GetContentRegionMax().X - 35f;


            InfoBox box = new()
            {
                Color = Colors.White,
                BorderThickness = 1f,
                CurveRadius = 3f,
                AutoResize = true,
                HasMaxWidth = true,
                IsSubBox = true,
                ContentsAction = () =>
                {
                    bool inputChanged = false;
                    Vector2 currentPos = ImGui.GetCursorPos();
                    ImGui.SetCursorPosX(currentPos.X + itemWidth);
                    ImGui.PushTextWrapPos(wrapPos);
                    ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.DalamudWhite);
                    ImGui.Text($"{sliderDescription}");
                    Vector2 height = ImGui.GetItemRectSize();
                    float lines = (height.Y / ImGui.GetFontSize());
                    Vector2 textLength = ImGui.CalcTextSize(sliderDescription);
                    string newLines = "";
                    for (int i = 1; i < lines; i++)
                    {
                        if (i % 2 == 0)
                        {
                            newLines += "\n";
                        }
                        else
                        {
                            newLines += "\n\n";
                        }
                    }

                    if (hasAdditionalChoice)
                    {
                        ImGui.SameLine();
                        ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.HealerGreen);
                        ImGui.PushFont(UiBuilder.IconFont);
                        ImGui.Dummy(new Vector2(5, 0));
                        ImGui.SameLine();
                        ImGui.TextWrapped($"{FontAwesomeIcon.Search.ToIconString()}");
                        ImGui.PopFont();
                        ImGui.PopStyleColor();

                        if (ImGui.IsItemHovered())
                        {
                            ImGui.BeginTooltip();
                            ImGui.TextUnformatted(
                                $"This setting has additional options depending on its value.{(string.IsNullOrEmpty(additonalChoiceCondition) ? "" : $"\nCondition: {additonalChoiceCondition}")}");
                            ImGui.EndTooltip();
                        }
                    }

                    ImGui.PopStyleColor();
                    ImGui.PopTextWrapPos();
                    ImGui.SameLine();
                    ImGui.SetCursorPosX(currentPos.X);
                    ImGui.PushItemWidth(itemWidth);
                    inputChanged |= ImGui.SliderFloat($"{newLines}###{config}", ref output, minValue, maxValue);

                    if (inputChanged)
                    {
                        PluginConfiguration.SetCustomFloatValue(config, output);
                        Service.Configuration.Save();
                    }
                }
            };

            box.Draw();
            ImGui.Spacing();
        }

        /// <summary> Draws a slider that lets the user set a given value for their feature. </summary>
        /// <param name="minValue"> The absolute minimum value you'll let the user pick. </param>
        /// <param name="maxValue"> The absolute maximum value you'll let the user pick. </param>
        /// <param name="config"> The config ID. </param>
        /// <param name="sliderDescription"> Description of the slider. Appends to the right of the slider. </param>
        /// <param name="itemWidth"> How long the slider should be. </param>
        /// <param name="hasAdditionalChoice"></param>
        /// <param name="additonalChoiceCondition"></param>
        /// <param name="digits"></param>
        public static void DrawRoundedSliderFloat(float minValue, float maxValue, string config, string sliderDescription, float itemWidth = 150,
            bool hasAdditionalChoice = false, string additonalChoiceCondition = "", int digits = 1)
        {
            float output = PluginConfiguration.GetCustomFloatValue(config, minValue);
            if (output < minValue)
            {
                output = minValue;
                PluginConfiguration.SetCustomFloatValue(config, output);
                Service.Configuration.Save();
            }

            sliderDescription = sliderDescription.Replace("%", "%%");
            float contentRegionMin = ImGui.GetItemRectMax().Y - ImGui.GetItemRectMin().Y;
            float wrapPos = ImGui.GetContentRegionMax().X - 35f;


            InfoBox box = new()
            {
                Color = Colors.White,
                BorderThickness = 1f,
                CurveRadius = 3f,
                AutoResize = true,
                HasMaxWidth = true,
                IsSubBox = true,
                ContentsAction = () =>
                {
                    bool inputChanged = false;
                    Vector2 currentPos = ImGui.GetCursorPos();
                    ImGui.SetCursorPosX(currentPos.X + itemWidth);
                    ImGui.PushTextWrapPos(wrapPos);
                    ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.DalamudWhite);
                    ImGui.Text($"{sliderDescription}");
                    Vector2 height = ImGui.GetItemRectSize();
                    float lines = (height.Y / ImGui.GetFontSize());
                    Vector2 textLength = ImGui.CalcTextSize(sliderDescription);
                    string newLines = "";
                    for (int i = 1; i < lines; i++)
                    {
                        if (i % 2 == 0)
                        {
                            newLines += "\n";
                        }
                        else
                        {
                            newLines += "\n\n";
                        }
                    }

                    if (hasAdditionalChoice)
                    {
                        ImGui.SameLine();
                        ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.HealerGreen);
                        ImGui.PushFont(UiBuilder.IconFont);
                        ImGui.Dummy(new Vector2(5, 0));
                        ImGui.SameLine();
                        ImGui.TextWrapped($"{FontAwesomeIcon.Search.ToIconString()}");
                        ImGui.PopFont();
                        ImGui.PopStyleColor();

                        if (ImGui.IsItemHovered())
                        {
                            ImGui.BeginTooltip();
                            ImGui.TextUnformatted(
                                $"This setting has additional options depending on its value.{(string.IsNullOrEmpty(additonalChoiceCondition) ? "" : $"\nCondition: {additonalChoiceCondition}")}");
                            ImGui.EndTooltip();
                        }
                    }

                    ImGui.PopStyleColor();
                    ImGui.PopTextWrapPos();
                    ImGui.SameLine();
                    ImGui.SetCursorPosX(currentPos.X);
                    ImGui.PushItemWidth(itemWidth);
                    inputChanged |= ImGui.SliderFloat($"{newLines}###{config}", ref output, minValue, maxValue, $"%.{digits}f");

                    if (inputChanged)
                    {
                        PluginConfiguration.SetCustomFloatValue(config, output);
                        Service.Configuration.Save();
                    }
                }
            };

            box.Draw();
            ImGui.Spacing();
        }

        /// <summary> Draws a checkbox intended to be linked to other checkboxes sharing the same config value. </summary>
        /// <param name="config"> The config ID. </param>
        /// <param name="checkBoxName"> The name of the feature. </param>
        /// <param name="checkboxDescription"> The description of the feature. </param>
        /// <param name="outputValue"> If the user ticks this box, this is the value the config will be set to. </param>
        /// <param name="itemWidth"></param>
        /// <param name="descriptionColor"></param>
        public static void DrawRadioButton(string config, string checkBoxName, string checkboxDescription, int outputValue, float itemWidth = 150,
            Vector4 descriptionColor = new Vector4())
        {
            ImGui.Indent();
            if (descriptionColor == new Vector4())
                descriptionColor = ImGuiColors.DalamudYellow;
            int output = PluginConfiguration.GetCustomIntValue(config, outputValue);
            ImGui.PushItemWidth(itemWidth);
            ImGui.SameLine();
            ImGui.Dummy(new Vector2(21, 0));
            ImGui.SameLine();
            bool enabled = output == outputValue;

            if (ImGui.RadioButton($"{checkBoxName}###{config}{outputValue}", enabled))
            {
                PluginConfiguration.SetCustomIntValue(config, outputValue);
                Service.Configuration.Save();
            }

            if (!StringExtensions.IsNullOrEmpty(checkboxDescription))
            {
                ImGui.PushStyleColor(ImGuiCol.Text, descriptionColor);
                ImGui.TextWrapped(checkboxDescription);
                ImGui.PopStyleColor();
            }

            ImGui.Unindent();
            ImGui.Spacing();
        }


        public static void DrawCustom(CustomTimeline customTimeline, List<CustomTimeline> customTimelineList)
        {
            ImGui.PushID(customTimeline.Name);
            ImGui.Indent();
            ImGui.SameLine();


            if (ImGui.Button("加载"))
            {
                foreach (var tCustomTimeline in customTimelineList)
                {
                    // if (tCustomTimeline != customTimeline)
                    {
                        tCustomTimeline.Enable = false;
                    }
                }

                customTimeline.Enable = true;

                Service.Configuration.Save();

                CustomComboFunctions.LoadCustomTime(customTimeline);
            }


            ImGui.SameLine();
            ImGui.SetCursorPosX(60);
            if (ImGui.Button("停用"))
            {
                customTimeline.Enable = false;
                Service.Configuration.Save();

                CustomComboFunctions.ResetCustomTime();
            }


            {
                ImGui.SameLine();
                ImGui.SetCursorPosX(110);
                ImGui.PushItemWidth(300);


                Vector4 descriptionColor = ImGuiColors.DalamudRed;

                if (customTimeline.Enable)
                {
                    ImGui.TextColored(descriptionColor, customTimeline.Name);
                }
                else
                {
                    ImGui.Text(customTimeline.Name);
                }
            }


            ImGui.Unindent();
            ImGui.Spacing();
            ImGui.PopID();
        }


        /// <summary> Draws a checkbox in a horizontal configuration intended to be linked to other checkboxes sharing the same config value. </summary>
        /// <param name="config"> The config ID. </param>
        /// <param name="checkBoxName"> The name of the feature. </param>
        /// <param name="checkboxDescription"> The description of the feature. </param>
        /// <param name="outputValue"> If the user ticks this box, this is the value the config will be set to. </param>
        /// <param name="itemWidth"></param>
        /// <param name="descriptionColor"></param>
        public static void DrawHorizontalRadioButton(string config, string checkBoxName, string checkboxDescription, int outputValue,
            float itemWidth = 150, Vector4 descriptionColor = new Vector4())
        {
            ImGui.Indent();
            if (descriptionColor == new Vector4()) descriptionColor = ImGuiColors.DalamudYellow;
            int output = PluginConfiguration.GetCustomIntValue(config);
            ImGui.PushItemWidth(itemWidth);
            ImGui.SameLine();
            ImGui.Dummy(new Vector2(21, 0));
            ImGui.SameLine();
            bool enabled = output == outputValue;

            ImGui.PushStyleColor(ImGuiCol.Text, descriptionColor);
            if (ImGui.RadioButton($"{checkBoxName}###{config}{outputValue}", enabled))
            {
                PluginConfiguration.SetCustomIntValue(config, outputValue);
                Service.Configuration.Save();
            }

            if (!StringExtensions.IsNullOrEmpty(checkboxDescription) && ImGui.IsItemHovered())
            {
                ImGui.BeginTooltip();
                ImGui.TextUnformatted(checkboxDescription);
                ImGui.EndTooltip();
            }

            ImGui.PopStyleColor();

            ImGui.Unindent();
        }

        /// <summary>A true or false configuration. Similar to presets except can be used as part of a condition on another config.</summary>
        /// <param name="config">The config ID.</param>
        /// <param name="checkBoxName">The name of the feature.</param>
        /// <param name="checkboxDescription">The description of the feature</param>
        /// <param name="itemWidth"></param>
        /// <param name="isConditionalChoice"></param>
        public static void DrawAdditionalBoolChoice(string config, string checkBoxName, string checkboxDescription, float itemWidth = 150,
            bool isConditionalChoice = false)
        {
            bool output = PluginConfiguration.GetCustomBoolValue(config);
            ImGui.PushItemWidth(itemWidth);
            if (!isConditionalChoice)
                ImGui.Indent();
            else
            {
                ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.HealerGreen);
                ImGui.PushFont(UiBuilder.IconFont);
                ImGui.AlignTextToFramePadding();
                ImGui.TextWrapped($"{FontAwesomeIcon.Plus.ToIconString()}");
                ImGui.PopFont();
                ImGui.PopStyleColor();
                ImGui.SameLine();
                ImGui.Dummy(new Vector2(3));
                ImGui.SameLine();
                if (isConditionalChoice) ImGui.Indent(); //Align checkbox after the + symbol
            }

            if (ImGui.Checkbox($"{checkBoxName}###{config}", ref output))
            {
                PluginConfiguration.SetCustomBoolValue(config, output);
                Service.Configuration.Save();
            }

            if (!StringExtensions.IsNullOrEmpty(checkboxDescription))
            {
                ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.DalamudGrey);
                ImGui.TextWrapped(checkboxDescription);
                ImGui.PopStyleColor();
            }

            //if (!isConditionalChoice)
            ImGui.Unindent();
            ImGui.Spacing();
        }

        /// <summary> Draws multi choice checkboxes in a horizontal configuration. </summary>
        /// <param name="config"> The config ID. </param>
        /// <param name="checkBoxName"> The name of the feature. </param>
        /// <param name="checkboxDescription"> The description of the feature. </param>
        /// <param name="totalChoices"> The total number of options for the feature </param>
        /// /// <param name="choice"> If the user ticks this box, this is the value the config will be set to. </param>
        /// <param name="itemWidth"></param>
        /// <param name="descriptionColor"></param>
        public static void DrawHorizontalMultiChoice(string config, string checkBoxName, string checkboxDescription, int totalChoices, int choice,
            float itemWidth = 150, Vector4 descriptionColor = new Vector4())
        {
            ImGui.Indent();
            if (descriptionColor == new Vector4())
                descriptionColor = ImGuiColors.DalamudWhite;
            ImGui.PushItemWidth(itemWidth);
            ImGui.SameLine();
            ImGui.Dummy(new Vector2(21, 0));
            ImGui.SameLine();
            bool[]? values = PluginConfiguration.GetCustomBoolArrayValue(config);

            if (ImGui.GetColumnsCount() == totalChoices)
            {
                ImGui.NextColumn();
            }
            else
            {
                ImGui.Columns(totalChoices, null, false);
            }

            //If new saved options or amount of choices changed, resize and save
            if (values.Length == 0 || values.Length != totalChoices)
            {
                Array.Resize(ref values, totalChoices);
                PluginConfiguration.SetCustomBoolArrayValue(config, values);
                Service.Configuration.Save();
            }

            ImGui.PushStyleColor(ImGuiCol.Text, descriptionColor);
            if (ImGui.Checkbox($"{checkBoxName}###{config}{choice}", ref values[choice]))
            {
                PluginConfiguration.SetCustomBoolArrayValue(config, values);
                Service.Configuration.Save();
            }

            if (!StringExtensions.IsNullOrEmpty(checkboxDescription) && ImGui.IsItemHovered())
            {
                ImGui.BeginTooltip();
                ImGui.TextUnformatted(checkboxDescription);
                ImGui.EndTooltip();
            }

            if (ImGui.GetColumnIndex() == totalChoices - 1)
                ImGui.Columns(1);

            ImGui.PopStyleColor();
            ImGui.Unindent();
        }

        public static void DrawGridMultiChoice(string config, byte columns, string[,] nameAndDesc, float itemWidth = 150,
            Vector4 descriptionColor = new Vector4())
        {
            int totalChoices = nameAndDesc.GetLength(0);
            if (totalChoices > 0)
            {
                ImGui.Indent();
                if (descriptionColor == new Vector4()) descriptionColor = ImGuiColors.DalamudWhite;
                //ImGui.PushItemWidth(itemWidth);
                //ImGui.SameLine();
                //ImGui.Dummy(new Vector2(21, 0));
                //ImGui.SameLine();
                bool[]? values = PluginConfiguration.GetCustomBoolArrayValue(config);

                //If new saved options or amount of choices changed, resize and save
                if (values.Length == 0 || values.Length != totalChoices)
                {
                    Array.Resize(ref values, totalChoices);
                    PluginConfiguration.SetCustomBoolArrayValue(config, values);
                    Service.Configuration.Save();
                }

                ImGui.BeginTable($"Grid###{config}", columns);
                ImGui.TableNextRow();
                //Convert the 2D array of names and descriptions into radio buttons
                for (int idx = 0; idx < totalChoices; idx++)
                {
                    ImGui.TableNextColumn();
                    string checkBoxName = nameAndDesc[idx, 0];
                    string checkboxDescription = nameAndDesc[idx, 1];

                    ImGui.PushStyleColor(ImGuiCol.Text, descriptionColor);
                    if (ImGui.Checkbox($"{checkBoxName}###{config}{idx}", ref values[idx]))
                    {
                        PluginConfiguration.SetCustomBoolArrayValue(config, values);
                        Service.Configuration.Save();
                    }

                    if (!StringExtensions.IsNullOrEmpty(checkboxDescription) && ImGui.IsItemHovered())
                    {
                        ImGui.BeginTooltip();
                        ImGui.TextUnformatted(checkboxDescription);
                        ImGui.EndTooltip();
                    }

                    ImGui.PopStyleColor();

                    if (((idx + 1) % columns) == 0)
                        ImGui.TableNextRow();
                }

                ImGui.EndTable();
                ImGui.Unindent();
            }
        }


        public static void DrawPvPStatusMultiChoice(string config)
        {
            bool[]? values = PluginConfiguration.GetCustomBoolArrayValue(config);

            ImGui.Columns(7, $"{config}", false);

            if (values.Length == 0) Array.Resize(ref values, 7);

            ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.ParsedPink);

            if (ImGui.Checkbox($"[眩晕]###{config}0", ref values[0]))
            {
                PluginConfiguration.SetCustomBoolArrayValue(config, values);
                Service.Configuration.Save();
            }

            ImGui.NextColumn();

            if (ImGui.Checkbox($"[冻结]###{config}1", ref values[1]))
            {
                PluginConfiguration.SetCustomBoolArrayValue(config, values);
                Service.Configuration.Save();
            }

            ImGui.NextColumn();

            if (ImGui.Checkbox($"[渐渐睡眠]###{config}2", ref values[2]))
            {
                PluginConfiguration.SetCustomBoolArrayValue(config, values);
                Service.Configuration.Save();
            }

            ImGui.NextColumn();

            if (ImGui.Checkbox($"[睡眠]###{config}3", ref values[3]))
            {
                PluginConfiguration.SetCustomBoolArrayValue(config, values);
                Service.Configuration.Save();
            }

            ImGui.NextColumn();

            if (ImGui.Checkbox($"[止步]###{config}4", ref values[4]))
            {
                PluginConfiguration.SetCustomBoolArrayValue(config, values);
                Service.Configuration.Save();
            }

            ImGui.NextColumn();

            if (ImGui.Checkbox($"[加重]###{config}5", ref values[5]))
            {
                PluginConfiguration.SetCustomBoolArrayValue(config, values);
                Service.Configuration.Save();
            }

            ImGui.NextColumn();

            if (ImGui.Checkbox($"[沉默]###{config}6", ref values[6]))
            {
                PluginConfiguration.SetCustomBoolArrayValue(config, values);
                Service.Configuration.Save();
            }

            ImGui.Columns(1);
            ImGui.PopStyleColor();
            ImGui.Spacing();
        }

        public static void DrawRoleGridMultiChoice(string config)
        {
            bool[]? values = PluginConfiguration.GetCustomBoolArrayValue(config);

            ImGui.Columns(5, $"{config}", false);

            if (values.Length == 0) Array.Resize(ref values, 5);

            ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.TankBlue);

            if (ImGui.Checkbox($"Tanks###{config}0", ref values[0]))
            {
                PluginConfiguration.SetCustomBoolArrayValue(config, values);
                Service.Configuration.Save();
            }

            ImGui.NextColumn();
            ImGui.PopStyleColor();
            ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.HealerGreen);

            if (ImGui.Checkbox($"Healers###{config}1", ref values[1]))
            {
                PluginConfiguration.SetCustomBoolArrayValue(config, values);
                Service.Configuration.Save();
            }

            ImGui.NextColumn();
            ImGui.PopStyleColor();
            ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.DPSRed);

            if (ImGui.Checkbox($"Melee###{config}2", ref values[2]))
            {
                PluginConfiguration.SetCustomBoolArrayValue(config, values);
                Service.Configuration.Save();
            }

            ImGui.NextColumn();
            ImGui.PopStyleColor();
            ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.DalamudYellow);

            if (ImGui.Checkbox($"Ranged###{config}3", ref values[3]))
            {
                PluginConfiguration.SetCustomBoolArrayValue(config, values);
                Service.Configuration.Save();
            }

            ImGui.NextColumn();
            ImGui.PopStyleColor();
            ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.ParsedPurple);

            if (ImGui.Checkbox($"Casters###{config}4", ref values[4]))
            {
                PluginConfiguration.SetCustomBoolArrayValue(config, values);
                Service.Configuration.Save();
            }

            ImGui.Columns(1);
            ImGui.PopStyleColor();
            ImGui.Spacing();
        }

        public static void DrawRoleGridSingleChoice(string config)
        {
            int value = PluginConfiguration.GetCustomIntValue(config);
            bool[] values = new bool[20];

            for (int i = 0; i <= 4; i++)
            {
                if (value == i) values[i] = true;
                else
                    values[i] = false;
            }

            ImGui.Columns(5, $"{config}", false);

            if (values.Length == 0) Array.Resize(ref values, 5);

            ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.TankBlue);

            if (ImGui.Checkbox($"Tanks###{config}0", ref values[0]))
            {
                PluginConfiguration.SetCustomIntValue(config, 0);
                Service.Configuration.Save();
            }

            ImGui.NextColumn();
            ImGui.PopStyleColor();
            ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.HealerGreen);

            if (ImGui.Checkbox($"Healers###{config}1", ref values[1]))
            {
                PluginConfiguration.SetCustomIntValue(config, 1);
                Service.Configuration.Save();
            }

            ImGui.NextColumn();
            ImGui.PopStyleColor();
            ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.DPSRed);

            if (ImGui.Checkbox($"Melee###{config}2", ref values[2]))
            {
                PluginConfiguration.SetCustomIntValue(config, 2);
                Service.Configuration.Save();
            }

            ImGui.NextColumn();
            ImGui.PopStyleColor();
            ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.DalamudYellow);

            if (ImGui.Checkbox($"Ranged###{config}3", ref values[3]))
            {
                PluginConfiguration.SetCustomIntValue(config, 3);
                Service.Configuration.Save();
            }

            ImGui.NextColumn();
            ImGui.PopStyleColor();
            ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.ParsedPurple);

            if (ImGui.Checkbox($"Casters###{config}4", ref values[4]))
            {
                PluginConfiguration.SetCustomIntValue(config, 4);
                Service.Configuration.Save();
            }

            ImGui.Columns(1);
            ImGui.PopStyleColor();
            ImGui.Spacing();
        }

        public static void DrawJobGridMultiChoice(string config)
        {
            bool[]? values = PluginConfiguration.GetCustomBoolArrayValue(config);

            ImGui.Columns(5, $"{config}", false);

            if (values.Length == 0) Array.Resize(ref values, 20);

            ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.TankBlue);

            if (ImGui.Checkbox($"Paladin###{config}0", ref values[0]))
            {
                PluginConfiguration.SetCustomBoolArrayValue(config, values);
                Service.Configuration.Save();
            }

            ImGui.NextColumn();

            if (ImGui.Checkbox($"Warrior###{config}1", ref values[1]))
            {
                PluginConfiguration.SetCustomBoolArrayValue(config, values);
                Service.Configuration.Save();
            }

            ImGui.NextColumn();

            if (ImGui.Checkbox($"Dark Knight###{config}2", ref values[2]))
            {
                PluginConfiguration.SetCustomBoolArrayValue(config, values);
                Service.Configuration.Save();
            }

            ImGui.NextColumn();

            if (ImGui.Checkbox($"Gunbreaker###{config}3", ref values[3]))
            {
                PluginConfiguration.SetCustomBoolArrayValue(config, values);
                Service.Configuration.Save();
            }

            ImGui.NextColumn();
            ImGui.NextColumn();

            ImGui.PopStyleColor();
            ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.HealerGreen);

            if (ImGui.Checkbox($"White Mage###{config}", ref values[4]))
            {
                PluginConfiguration.SetCustomBoolArrayValue(config, values);
                Service.Configuration.Save();
            }

            ImGui.NextColumn();

            if (ImGui.Checkbox($"Scholar###{config}5", ref values[5]))
            {
                PluginConfiguration.SetCustomBoolArrayValue(config, values);
                Service.Configuration.Save();
            }

            ImGui.NextColumn();

            if (ImGui.Checkbox($"Astrologian###{config}6", ref values[6]))
            {
                PluginConfiguration.SetCustomBoolArrayValue(config, values);
                Service.Configuration.Save();
            }

            ImGui.NextColumn();

            if (ImGui.Checkbox($"Sage###{config}7", ref values[7]))
            {
                PluginConfiguration.SetCustomBoolArrayValue(config, values);
                Service.Configuration.Save();
            }

            ImGui.NextColumn();
            ImGui.NextColumn();

            ImGui.PopStyleColor();
            ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.DPSRed);

            if (ImGui.Checkbox($"Monk###{config}8", ref values[8]))
            {
                PluginConfiguration.SetCustomBoolArrayValue(config, values);
                Service.Configuration.Save();
            }

            ImGui.NextColumn();

            if (ImGui.Checkbox($"Dragoon###{config}9", ref values[9]))
            {
                PluginConfiguration.SetCustomBoolArrayValue(config, values);
                Service.Configuration.Save();
            }

            ImGui.NextColumn();

            if (ImGui.Checkbox($"Ninja###{config}10", ref values[10]))
            {
                PluginConfiguration.SetCustomBoolArrayValue(config, values);
                Service.Configuration.Save();
            }

            ImGui.NextColumn();

            if (ImGui.Checkbox($"Samurai###{config}11", ref values[11]))
            {
                PluginConfiguration.SetCustomBoolArrayValue(config, values);
                Service.Configuration.Save();
            }

            ImGui.NextColumn();

            if (ImGui.Checkbox($"Reaper###{config}12", ref values[12]))
            {
                PluginConfiguration.SetCustomBoolArrayValue(config, values);
                Service.Configuration.Save();
            }

            ImGui.PopStyleColor();
            ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.DalamudYellow);
            ImGui.NextColumn();

            if (ImGui.Checkbox($"Bard###{config}13", ref values[13]))
            {
                PluginConfiguration.SetCustomBoolArrayValue(config, values);
                Service.Configuration.Save();
            }

            ImGui.NextColumn();

            if (ImGui.Checkbox($"Machinist###{config}14", ref values[14]))
            {
                PluginConfiguration.SetCustomBoolArrayValue(config, values);
                Service.Configuration.Save();
            }

            ImGui.NextColumn();

            if (ImGui.Checkbox($"Dancer###{config}15", ref values[15]))
            {
                PluginConfiguration.SetCustomBoolArrayValue(config, values);
                Service.Configuration.Save();
            }

            ImGui.NextColumn();
            ImGui.NextColumn();
            ImGui.NextColumn();

            ImGui.PopStyleColor();
            ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.ParsedPurple);

            if (ImGui.Checkbox($"Black Mage###{config}16", ref values[16]))
            {
                PluginConfiguration.SetCustomBoolArrayValue(config, values);
                Service.Configuration.Save();
            }

            ImGui.NextColumn();

            if (ImGui.Checkbox($"Summoner###{config}17", ref values[17]))
            {
                PluginConfiguration.SetCustomBoolArrayValue(config, values);
                Service.Configuration.Save();
            }

            ImGui.NextColumn();

            if (ImGui.Checkbox($"Red Mage###{config}18", ref values[18]))
            {
                PluginConfiguration.SetCustomBoolArrayValue(config, values);
                Service.Configuration.Save();
            }

            ImGui.NextColumn();

            if (ImGui.Checkbox($"Blue Mage###{config}19", ref values[19]))
            {
                PluginConfiguration.SetCustomBoolArrayValue(config, values);
                Service.Configuration.Save();
            }

            ImGui.PopStyleColor();
            ImGui.NextColumn();
            ImGui.Columns(1);
            ImGui.Spacing();
        }

        public static void DrawJobGridSingleChoice(string config)
        {
            int value = PluginConfiguration.GetCustomIntValue(config);
            bool[] values = new bool[20];

            for (int i = 0; i <= 19; i++)
            {
                if (value == i) values[i] = true;
                else
                    values[i] = false;
            }

            ImGui.Columns(5, null, false);
            ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.TankBlue);

            if (ImGui.Checkbox($"Paladin###{config}0", ref values[0]))
            {
                PluginConfiguration.SetCustomIntValue(config, 0);
                Service.Configuration.Save();
            }

            ImGui.NextColumn();

            if (ImGui.Checkbox($"Warrior###{config}1", ref values[1]))
            {
                PluginConfiguration.SetCustomIntValue(config, 1);
                Service.Configuration.Save();
            }

            ImGui.NextColumn();

            if (ImGui.Checkbox($"Dark Knight###{config}2", ref values[2]))
            {
                PluginConfiguration.SetCustomIntValue(config, 2);
                Service.Configuration.Save();
            }

            ImGui.NextColumn();

            if (ImGui.Checkbox($"Gunbreaker###{config}3", ref values[3]))
            {
                PluginConfiguration.SetCustomIntValue(config, 3);
                Service.Configuration.Save();
            }

            ImGui.NextColumn();
            ImGui.NextColumn();

            ImGui.PopStyleColor();
            ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.HealerGreen);

            if (ImGui.Checkbox($"White Mage###{config}4", ref values[4]))
            {
                PluginConfiguration.SetCustomIntValue(config, 4);
                Service.Configuration.Save();
            }

            ImGui.NextColumn();

            if (ImGui.Checkbox($"Scholar###{config}5", ref values[5]))
            {
                PluginConfiguration.SetCustomIntValue(config, 5);
                Service.Configuration.Save();
            }

            ImGui.NextColumn();

            if (ImGui.Checkbox($"Astrologian###{config}6", ref values[6]))
            {
                PluginConfiguration.SetCustomIntValue(config, 6);
                Service.Configuration.Save();
            }

            ImGui.NextColumn();

            if (ImGui.Checkbox($"Sage###{config}7", ref values[7]))
            {
                PluginConfiguration.SetCustomIntValue(config, 7);
                Service.Configuration.Save();
            }

            ImGui.NextColumn();
            ImGui.NextColumn();

            ImGui.PopStyleColor();
            ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.DPSRed);

            if (ImGui.Checkbox($"Monk###{config}8", ref values[8]))
            {
                PluginConfiguration.SetCustomIntValue(config, 8);
                Service.Configuration.Save();
            }

            ImGui.NextColumn();

            if (ImGui.Checkbox($"Dragoon###{config}9", ref values[9]))
            {
                PluginConfiguration.SetCustomIntValue(config, 9);
                Service.Configuration.Save();
            }

            ImGui.NextColumn();

            if (ImGui.Checkbox($"Ninja###{config}10", ref values[10]))
            {
                PluginConfiguration.SetCustomIntValue(config, 10);
                Service.Configuration.Save();
            }

            ImGui.NextColumn();

            if (ImGui.Checkbox($"Samurai###{config}11", ref values[11]))
            {
                PluginConfiguration.SetCustomIntValue(config, 11);
                Service.Configuration.Save();
            }

            ImGui.NextColumn();

            if (ImGui.Checkbox($"Reaper###{config}12", ref values[12]))
            {
                PluginConfiguration.SetCustomIntValue(config, 12);
                Service.Configuration.Save();
            }

            ImGui.PopStyleColor();
            ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.DalamudYellow);
            ImGui.NextColumn();

            if (ImGui.Checkbox($"Bard###{config}13", ref values[13]))
            {
                PluginConfiguration.SetCustomIntValue(config, 13);
                Service.Configuration.Save();
            }

            ImGui.NextColumn();

            if (ImGui.Checkbox($"Machinist###{config}14", ref values[14]))
            {
                PluginConfiguration.SetCustomIntValue(config, 14);
                Service.Configuration.Save();
            }

            ImGui.NextColumn();

            if (ImGui.Checkbox($"Dancer###{config}15", ref values[15]))
            {
                PluginConfiguration.SetCustomIntValue(config, 15);
                Service.Configuration.Save();
            }

            ImGui.NextColumn();
            ImGui.NextColumn();
            ImGui.NextColumn();

            ImGui.PopStyleColor();
            ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.ParsedPurple);

            if (ImGui.Checkbox($"Black Mage###{config}16", ref values[16]))
            {
                PluginConfiguration.SetCustomIntValue(config, 16);
                Service.Configuration.Save();
            }

            ImGui.NextColumn();

            if (ImGui.Checkbox($"Summoner###{config}17", ref values[17]))
            {
                PluginConfiguration.SetCustomIntValue(config, 17);
                Service.Configuration.Save();
            }

            ImGui.NextColumn();

            if (ImGui.Checkbox($"Red Mage###{config}18", ref values[18]))
            {
                PluginConfiguration.SetCustomIntValue(config, 18);
                Service.Configuration.Save();
            }

            ImGui.NextColumn();

            if (ImGui.Checkbox($"Blue Mage###{config}19", ref values[19]))
            {
                PluginConfiguration.SetCustomIntValue(config, 19);
                Service.Configuration.Save();
            }

            ImGui.PopStyleColor();
            ImGui.NextColumn();
            ImGui.Columns(1);
            ImGui.Spacing();
        }

        public static int RoundOff(this int i, uint sliderIncrement)
        {
            double sliderAsDouble = Convert.ToDouble(sliderIncrement);
            return ((int)Math.Round(i / sliderAsDouble)) * (int)sliderIncrement;
        }
    }

    public static class UserConfigItems
    {
        /// <summary> Draws the User Configurable settings. </summary>
        /// <param name="preset"> The preset it's attached to. </param>
        /// <param name="enabled"> If it's enabled or not. </param>
        internal static void Draw(CustomComboPreset preset, bool enabled)
        {
            if (!enabled) return;

            // ====================================================================================

            #region Misc

            #endregion

            // ====================================================================================

            #region ADV

            #endregion

            // ====================================================================================

            #region ASTROLOGIAN
            
            if (preset is CustomComboPreset.AST_Advanced_CustomMode)
            {
                List<CustomTimeline> customTimelineList =
                    PluginConfiguration.CustomTimelineList.FindAll(CustomTimeline => CustomTimeline.JobId == AST.JobID);
                
                
                for (var i = 0; i < customTimelineList.Count; i++)
                {
                    CustomTimeline customTimeline = customTimelineList[i];
                    UserConfig.DrawCustom(customTimeline,customTimelineList);
                }
            }	
            
            if (preset is CustomComboPreset.AST_ST_DPS)
            {
                UserConfig.DrawRadioButton(AST.Config.AST_DPS_AltMode, "凶星", "", 0);
                UserConfig.DrawRadioButton(AST.Config.AST_DPS_AltMode, "烧灼", "可选dps模式. 留下凶星按键用于打dps， 其他特性冷却时也变为凶星", 1);
            }

            if (preset is CustomComboPreset.AST_DPS_Lucid)
                UserConfig.DrawSliderInt(4000, 9500, AST.Config.AST_LucidDreaming, "当你的蓝量低于此值时会触发本特性", 150, SliderIncrements.Hundreds);

            if (preset is CustomComboPreset.AST_ST_DPS_CombustUptime)
            {
                UserConfig.DrawSliderInt(0, 100, AST.Config.AST_DPS_CombustOption, "当敌人HP百分比低于此设置值时停止使用. 如果想要忽略这个检测，设置为0.");

                UserConfig.DrawAdditionalBoolChoice(nameof(AST.Config.AST_ST_DPS_CombustUptime_Adv), "高级选项", "", isConditionalChoice: true);
                if (PluginConfiguration.GetCustomBoolValue(nameof(AST.Config.AST_ST_DPS_CombustUptime_Adv)))
                {
                    ImGui.Indent();
                    UserConfig.DrawRoundedSliderFloat(0, 4, nameof(AST.Config.AST_ST_DPS_CombustUptime_Threshold),
                        "DOT刷新检测秒数（低于此时间就尝试刷新）. 如果想要忽略这个检测，设置为0.", digits: 1);
                    ImGui.Unindent();
                }
            }

            if (preset is CustomComboPreset.AST_DPS_Divination)
                UserConfig.DrawSliderInt(0, 100, AST.Config.AST_DPS_DivinationOption, "当敌人HP百分比低于此设置值时停止使用. 如果想要忽略这个检测，设置为0.");

            if (preset is CustomComboPreset.AST_DPS_LightSpeed)
                UserConfig.DrawSliderInt(0, 100, AST.Config.AST_DPS_LightSpeedOption, "当敌人HP百分比低于此设置值时停止使用. 如果想要忽略这个检测，设置为0.");


            if (preset is CustomComboPreset.AST_ST_SimpleHeals)
            {
                UserConfig.DrawAdditionalBoolChoice(AST.Config.AST_ST_SimpleHeals_Adv, "高级选项", "", isConditionalChoice: true);
                if (AST.Config.AST_ST_SimpleHeals_Adv)
                {
                    ImGui.Indent();
                    ImGui.Spacing();
                    UserConfig.DrawAdditionalBoolChoice(AST.Config.AST_ST_SimpleHeals_UIMouseOver, "队伍UI鼠标悬停检测",
                        "检测团队成员生命值和Buff，通过将鼠标悬停于小队列表.\n" + "这个功能是用来和Redirect/Reaction/etc结合使用的.（译者注：这三个好像是鼠标悬停施法插件。）");
                    ImGui.Unindent();
                }
            }

            if (preset is CustomComboPreset.AST_ST_SimpleHeals_Esuna)
            {
                UserConfig.DrawSliderInt(0, 100, AST.Config.AST_ST_SimpleHeals_Esuna, "当生命值低于％时停止使用。将其设置为零以禁用此检查");
            }


            if (preset is CustomComboPreset.AST_ST_SimpleHeals_EssentialDignity)
                UserConfig.DrawSliderInt(0, 100, AST.Config.AST_EssentialDignity, "设置百分比数值");

            if (preset is CustomComboPreset.AST_Cards_QuickTargetCards)
            {
                UserConfig.DrawRadioButton(AST.Config.AST_QuickTarget_Override, "无覆盖", "", 0);
                UserConfig.DrawRadioButton(AST.Config.AST_QuickTarget_Override, "硬目标覆盖", "Overrides selection with hard target if you have one", 1);
                UserConfig.DrawRadioButton(AST.Config.AST_QuickTarget_Override, "UI悬停覆盖",
                    "Overrides selection with UI mouseover target if you have one", 2);

                ImGui.Spacing();
                UserConfig.DrawAdditionalBoolChoice(AST.Config.AST_QuickTarget_SkipDamageDown, $"跳过有{ActionWatching.GetStatusName(62)} debuff", "");
                UserConfig.DrawAdditionalBoolChoice(AST.Config.AST_QuickTarget_SkipRezWeakness,
                    $"跳过有 {ActionWatching.GetStatusName(43)} or {ActionWatching.GetStatusName(44)} debuff", "");
            }

            if (preset is CustomComboPreset.AST_DPS_AutoPlay)
            {
                UserConfig.DrawRadioButton(AST.Config.AST_ST_DPS_Play_SpeedSetting, "快速 (1 DPS GCD minimum delay)", "", 1);
                UserConfig.DrawRadioButton(AST.Config.AST_ST_DPS_Play_SpeedSetting, "平衡 (2 DPS GCD minimum delay)", "", 2);
                UserConfig.DrawRadioButton(AST.Config.AST_ST_DPS_Play_SpeedSetting, "缓慢 (3 DPS GCD minimum delay)", "", 3);
            }

            #endregion

            // ====================================================================================

            #region BLACK MAGE

            if (preset is CustomComboPreset.BLM_Advanced_CustomMode)
            {
                List<CustomTimeline> customTimelineList =
                    PluginConfiguration.CustomTimelineList.FindAll(CustomTimeline => CustomTimeline.JobId == BLM.JobID);


                for (var i = 0; i < customTimelineList.Count; i++)
                {
                    CustomTimeline customTimeline = customTimelineList[i];
                    UserConfig.DrawCustom(customTimeline, customTimelineList);
                }
            }

            if (preset is CustomComboPreset.BLM_ST_AdvancedMode)
            {
                UserConfig.DrawHorizontalRadioButton(BLM.Config.BLM_Adv_InitialCast, "火 3 Initial Cast", "", 0);
                UserConfig.DrawHorizontalRadioButton(BLM.Config.BLM_Adv_InitialCast, "冰结 3 Initial Cast", "", 1);
                ImGui.Indent();
                UserConfig.DrawRoundedSliderFloat(3.0f, 8.0f, BLM.Config.BLM_AstralFire_Refresh, "Seconds before refreshing 星极火");
                ImGui.Unindent();
            }

            if (preset is CustomComboPreset.BLM_Variant_Cure)
                UserConfig.DrawSliderInt(1, 100, BLM.Config.BLM_VariantCure, "HP% to be at or under", 200);

            if (preset is CustomComboPreset.BLM_Adv_Opener)
            {
                UserConfig.DrawHorizontalRadioButton(BLM.Config.BLM_Advanced_OpenerSelection, "Standard Opener", "Uses Standard Opener.", 0);
                UserConfig.DrawHorizontalRadioButton(BLM.Config.BLM_Advanced_OpenerSelection, "Double 星灵移位 Opener",
                    "Uses Fire III opener - Double Transpose variation.", 1);
            }

            if (preset is CustomComboPreset.BLM_Adv_Rotation)
            {
                UserConfig.DrawHorizontalRadioButton(BLM.Config.BLM_Adv_Rotation_Options, "Standard Rotation", "Uses Standard Rotation.", 0);
                UserConfig.DrawHorizontalRadioButton(BLM.Config.BLM_Adv_Rotation_Options, "Double 星灵移位 Rotation",
                    "Uses Double Transpose rotation.\nOnly works at Lv.90.", 1);

                if (BLM.Config.BLM_Adv_Rotation_Options == 0)
                {
                    ImGui.Indent();
                    UserConfig.DrawAdditionalBoolChoice(BLM.Config.BLM_Adv_Xeno_Burst, "Use 异言 for burst",
                        "Will save Xenoglossy for every minute burst window.");
                    ImGui.Unindent();
                    ImGui.Spacing();
                }
            }

            if (preset is CustomComboPreset.BLM_Adv_Cooldowns)
            {
                UserConfig.DrawGridMultiChoice(BLM.Config.BLM_Adv_Cooldowns_Choice,
                    4,
                    new string[,]
                    {
                        { "魔泉", "Add Manafont to the rotation." },
                        { "激情咏唱", "Add Sharpcast to the rotation." },
                        { "详述", "Add Amplifier to the rotation." },
                        { "黑魔纹", "Add Ley Lines to the rotation." },
                    });
            }

            if (preset is CustomComboPreset.BLM_AoE_Adv_Cooldowns)
            {
                UserConfig.DrawGridMultiChoice(BLM.Config.BLM_AoE_Adv_Cooldowns_Choice,
                    5,
                    new string[,]
                    {
                        { "魔泉", "Add Manafont to the rotation." },
                        { "激情咏唱", "Add Sharpcast to the rotation." },
                        { "详述", "Add Amplifier to the rotation." },
                        { "黑魔纹", "Add Ley Lines to the rotation." },
                        { "三连咏唱", "Add Triplecast to the rotation" }
                    });
            }

            if (preset is CustomComboPreset.BLM_Adv_Movement)
            {
                UserConfig.DrawGridMultiChoice(BLM.Config.BLM_Adv_Movement_Choice,
                    4,
                    new string[,]
                    {
                        { "激情咏唱", "Add Sharpcast." },
                        { "雷1/雷3", "Add Thunder I/Thunder III." },
                        { "星极火", "Add 星极火 when in Astral 火炎." },
                        { "悖论", "Add Paradox when in Umbral Ice." },
                        { "异言", "Add Xenoglossy.\nOne charge will be held for rotation." },
                        { "即刻咏唱", "Add Swiftcast." },
                        { "三连咏唱", "Add (pooled) Triplecast." },
                        { "崩溃", "Add Scathe." }
                    });
            }

            if (preset is CustomComboPreset.BLM_ST_Adv_Thunder)
                UserConfig.DrawSliderInt(0, 5, BLM.Config.BLM_Adv_Thunder, "刷新雷电前剩余的秒数");

            if (preset is CustomComboPreset.BLM_AoE_Adv_ThunderUptime)
                UserConfig.DrawSliderInt(0, 5, BLM.Config.BLM_AoE_Adv_ThunderUptime, "刷新雷电前剩余的秒数");

            if (preset is CustomComboPreset.BLM_ST_Adv_Thunder)
                UserConfig.DrawSliderInt(0, 5, BLM.Config.BLM_ST_Adv_ThunderHP, "停止使用雷云的目标HP％");

            if (preset is CustomComboPreset.BLM_AoE_Adv_ThunderUptime)
                UserConfig.DrawSliderInt(0, 5, BLM.Config.BLM_AoE_Adv_ThunderHP, "停止使用雷云的目标HP％");

            if (preset is CustomComboPreset.BLM_ST_Adv_Thunder_ThunderCloud)
            {
                UserConfig.DrawHorizontalRadioButton(BLM.Config.BLM_Adv_ThunderCloud, "只在更快的施法后（编织窗口）", "", 0);
                UserConfig.DrawHorizontalRadioButton(BLM.Config.BLM_Adv_ThunderCloud, "尽快使用", "", 1);
            }

            #endregion

            // ====================================================================================

            #region BLUE MAGE

            #endregion

            // ====================================================================================

            #region BARD

            if (preset == CustomComboPreset.BRD_Simple_RagingJaws)
                UserConfig.DrawSliderInt(3, 5, BRD.Config.BRD_RagingJawsRenewTime, "持续时间 (单位：秒)");

            if (preset == CustomComboPreset.BRD_Simple_NoWaste)
                UserConfig.DrawSliderInt(1, 10, BRD.Config.BRD_NoWasteHPPercentage, "目标血量百分比");

            if (preset == CustomComboPreset.BRD_ST_SecondWind)
                UserConfig.DrawSliderInt(0, 100, BRD.Config.BRD_STSecondWindThreshold, "HP percent threshold to use Second Wind below.", 150,
                    SliderIncrements.Ones);

            if (preset == CustomComboPreset.BRD_AoE_SecondWind)
                UserConfig.DrawSliderInt(0, 100, BRD.Config.BRD_AoESecondWindThreshold, "HP percent threshold to use Second Wind below.", 150,
                    SliderIncrements.Ones);

            if (preset == CustomComboPreset.BRD_Variant_Cure)
                UserConfig.DrawSliderInt(1, 100, BRD.Config.BRD_VariantCure, "HP% to be at or under", 200);

            #endregion

            // ====================================================================================

            #region DANCER

            #region Simple AoE Sliders

            if (preset == CustomComboPreset.DNC_AoE_Simple_SS)
                UserConfig.DrawSliderInt(0, 10, DNC.Config.DNCSimpleSSAoEBurstPercent, "目标生命值百分比低于此值不再使用标准舞步", 75, SliderIncrements.Ones);

            if (preset == CustomComboPreset.DNC_AoE_Simple_TS)
                UserConfig.DrawSliderInt(0, 10, DNC.Config.DNCSimpleTSAoEBurstPercent, "目标生命值百分比低于此值不再使用技巧舞步", 75, SliderIncrements.Ones);

            // if (preset == CustomComboPreset.DNC_AoE_Simple_SaberDance)
            //     UserConfig.DrawSliderInt(50, 100, DNC.Config.DNCSimpleAoESaberThreshold, "Esprit", 150, SliderIncrements.Fives);

            if (preset == CustomComboPreset.DNC_AoE_Simple_PanicHeals)
                UserConfig.DrawSliderInt(0, 100, DNC.Config.DNCSimpleAoEPanicHealWaltzPercent, "使用治疗华尔兹的生命值百分比临界点", 200, SliderIncrements.Ones);

            if (preset == CustomComboPreset.DNC_AoE_Simple_PanicHeals)
                UserConfig.DrawSliderInt(0, 100, DNC.Config.DNCSimpleAoEPanicHealWindPercent, "使用内丹的生命值百分比临界点", 200, SliderIncrements.Ones);

            #endregion

            #region PvP Sliders

            if (preset == CustomComboPreset.DNCPvP_BurstMode_CuringWaltz)
                UserConfig.DrawSliderInt(0, 90, DNCPvP.Config.DNCPvP_WaltzThreshold, "治疗之华尔兹 HP% - caps at 90 to prevent waste.", 150,
                    SliderIncrements.Ones);


            if (preset == CustomComboPreset.DNC_Variant_Cure)
                UserConfig.DrawSliderInt(1, 100, DNC.Config.DNC_VariantCure, "存几层充能？（0 = 用光，一层不留）", 200);

            #endregion

            #endregion

            // ====================================================================================

            #region DARK KNIGHT

            if (preset is CustomComboPreset.DRK_Advanced_CustomMode)
            {
                List<CustomTimeline> customTimelineList =
                    PluginConfiguration.CustomTimelineList.FindAll(CustomTimeline => CustomTimeline.JobId == DRK.JobID);
                
                
                for (var i = 0; i < customTimelineList.Count; i++)
                {
                    CustomTimeline customTimeline = customTimelineList[i];
                    UserConfig.DrawCustom(customTimeline,customTimelineList);
                }
            }
            
            if (preset == CustomComboPreset.DRK_EoSPooling && enabled)
                UserConfig.DrawSliderInt(0, 3000, DRK.Config.DRK_MPManagement, "保留多少MP (0 = 全部使用)", 150, SliderIncrements.Thousands);


            if (preset == CustomComboPreset.DRKPvP_Burst)
                UserConfig.DrawSliderInt(1, 100, DRKPvP.Config.ShadowbringerThreshold, "HP% to be at or above to use Shadowbringer");

            if (preset == CustomComboPreset.DRK_Variant_Cure)
                UserConfig.DrawSliderInt(1, 100, DRK.Config.DRK_VariantCure, "HP% to be at or under", 200);

            #endregion

            // ====================================================================================

            #region DRAGOON

            if (preset is CustomComboPreset.DRG_Advanced_CustomMode)
            {
                List<CustomTimeline> customTimelineList =
                    PluginConfiguration.CustomTimelineList.FindAll(CustomTimeline => CustomTimeline.JobId == DRG.JobID);
                
                
                for (var i = 0; i < customTimelineList.Count; i++)
                {
                    CustomTimeline customTimeline = customTimelineList[i];
                    UserConfig.DrawCustom(customTimeline,customTimelineList);
                }
            }
            
            if (preset == CustomComboPreset.DRG_ST_Dives && enabled)
            {
                UserConfig.DrawHorizontalRadioButton(DRG.Config.DRG_ST_DiveOptions, "冷却好了用", "单插友好. 冷却好了使用.", 1);
                UserConfig.DrawHorizontalRadioButton(DRG.Config.DRG_ST_DiveOptions, "战斗连倒和红莲龙血下用", "需要双插. 破碎冲龙炎冲在战斗连倒和红莲龙血下用, 坠星冲在红莲龙血下用.", 2);
                UserConfig.DrawHorizontalRadioButton(DRG.Config.DRG_ST_DiveOptions, "配合猛枪使用", "单插友好. 配合猛枪使用破碎冲龙炎冲, 红莲龙血下的坠星冲.", 3);
            }

            if (preset == CustomComboPreset.DRG_AoE_Dives && enabled)
            {
                UserConfig.DrawHorizontalRadioButton(DRG.Config.DRG_AOE_DiveOptions, "冷却好了用", "单插友好. 冷却好了使用.", 1);
                UserConfig.DrawHorizontalRadioButton(DRG.Config.DRG_AOE_DiveOptions, "战斗连倒和红莲龙血下用", "需要双插. 破碎冲龙炎冲在战斗连倒和红莲龙血下用, 坠星冲在红莲龙血下用.", 2);
                UserConfig.DrawHorizontalRadioButton(DRG.Config.DRG_AOE_DiveOptions, "配合猛枪使用", "单插友好. 配合猛枪使用破碎冲龙炎冲, 红莲龙血下的坠星冲.", 3);
            }

            if (preset == CustomComboPreset.DRG_ST_Opener && enabled)
            {
                UserConfig.DrawHorizontalRadioButton(DRG.Config.DRG_OpenerOptions, "标准开场起手", "使用标准起手（自己吃爆发药）", 1);
                UserConfig.DrawHorizontalRadioButton(DRG.Config.DRG_OpenerOptions, "低Ping开场起手", "低延迟下的起手. 第一个直刺连中开猛枪的不吃药开场起手.", 2);
            }

            if (preset == CustomComboPreset.DRG_ST_ComboHeals)
            {
                UserConfig.DrawSliderInt(0, 100, DRG.Config.DRG_STSecondWindThreshold, "使用内丹的生命值百分比临界点 (0 = 禁用)", 150, SliderIncrements.Ones);
                UserConfig.DrawSliderInt(0, 100, DRG.Config.DRG_STBloodbathThreshold, "使用浴血的生命值百分比临界点 (0 = 禁用)", 150, SliderIncrements.Ones);
            }

            if (preset == CustomComboPreset.DRG_AoE_ComboHeals)
            {
                UserConfig.DrawSliderInt(0, 100, DRG.Config.DRG_AoESecondWindThreshold, "使用内丹的生命值百分比临界点 (0 = 禁用)", 150, SliderIncrements.Ones);
                UserConfig.DrawSliderInt(0, 100, DRG.Config.DRG_AoEBloodbathThreshold, "使用浴血的生命值百分比临界点 (0 = 禁用)", 150, SliderIncrements.Ones);
            }

            if (preset == CustomComboPreset.DRG_Variant_Cure)
                UserConfig.DrawSliderInt(1, 100, DRG.Config.DRG_VariantCure, "HP% to be at or under", 200);

            #endregion

            // ====================================================================================

            #region GUNBREAKER

            if (preset is CustomComboPreset.GNB_Advanced_CustomMode)
            {
                List<CustomTimeline> customTimelineList =
                    PluginConfiguration.CustomTimelineList.FindAll(CustomTimeline => CustomTimeline.JobId == GNB.JobID);
                
                
                for (var i = 0; i < customTimelineList.Count; i++)
                {
                    CustomTimeline customTimeline = customTimelineList[i];
                    UserConfig.DrawCustom(customTimeline,customTimelineList);
                }
            }
            
            if (preset == CustomComboPreset.GNB_START_GCD)
            {
                UserConfig.DrawHorizontalRadioButton(GNB.Config.GNB_START_GCD, "1GCD", "", 1);
                UserConfig.DrawHorizontalRadioButton(GNB.Config.GNB_START_GCD, "2GCD", "", 2);
                UserConfig.DrawHorizontalRadioButton(GNB.Config.GNB_START_GCD, "3GCD", "", 3);
            }

            if (preset == CustomComboPreset.GNB_ST_SkSSupport && enabled)
            {
                UserConfig.DrawRadioButton(GNB.Config.GNB_SkS, "子弹连-倍攻-音速破", "", 1);
                UserConfig.DrawRadioButton(GNB.Config.GNB_SkS, "子弹连-音速破-倍攻", "", 2);

                UserConfig.DrawRadioButton(GNB.Config.GNB_SkS, "音速破-子弹连-倍攻", "", 3);
                UserConfig.DrawRadioButton(GNB.Config.GNB_SkS, "音速破-倍攻-子弹连", "", 4);

                UserConfig.DrawRadioButton(GNB.Config.GNB_SkS, "倍攻-音速破-子弹连", "", 5);
                UserConfig.DrawRadioButton(GNB.Config.GNB_SkS, "倍攻-子弹连-音速破", "", 6);
            }

            if (preset == CustomComboPreset.GNB_Variant_Cure)
                UserConfig.DrawSliderInt(1, 100, GNB.Config.GNB_VariantCure, "HP% to be at or under", 200);

            #endregion

            // ====================================================================================

            #region MACHINIST

            if (preset is CustomComboPreset.MCH_ST_CustomMode)
            {
                List<CustomTimeline> customTimelineList =
                    PluginConfiguration.CustomTimelineList.FindAll(CustomTimeline => CustomTimeline.JobId == MCH.JobID);


                for (var i = 0; i < customTimelineList.Count; i++)
                {
                    CustomTimeline customTimeline = customTimelineList[i];
                    UserConfig.DrawCustom(customTimeline, customTimelineList);
                }
            }


            if (preset is CustomComboPreset.MCH_ST_AdvancedMode)
            {
                //哥们一套循环都写不好 还要写3套？？
                UserConfig.DrawHorizontalRadioButton(MCH.Config.MCH_ST_RotationSelection, "标准起手", "", 0);

                UserConfig.DrawHorizontalRadioButton(MCH.Config.MCH_ST_RotationSelection, "90电起手", "", 1);
                // UserConfig.DrawHorizontalRadioButton(MCH.Config.MCH_ST_RotationSelection, "Early Tools", "", 2);
            }

            if (preset is CustomComboPreset.MCH_Adv_TurretQueen)
            {
                UserConfig.DrawHorizontalRadioButton(MCH.Config.MCH_ST_TurretUsage, "能用就用", "使用 50 或以上电池。", 0);
                UserConfig.DrawHorizontalRadioButton(MCH.Config.MCH_ST_TurretUsage, "快溢出用", "尽可能多的电量。", 1);
            }

            if (preset is CustomComboPreset.MCH_ST_Adv_Reassembled)
            {
                UserConfig.DrawHorizontalMultiChoice(MCH.Config.MCH_ST_Reassembled,
                    $"{ActionWatching.GetActionName(MCH.热弹HotShot)}/{ActionWatching.GetActionName(MCH.空气锚AirAnchor)}", "", 3, 0);
                UserConfig.DrawHorizontalMultiChoice(MCH.Config.MCH_ST_Reassembled, $"{ActionWatching.GetActionName(MCH.钻头Drill)}", "", 3, 1);
                UserConfig.DrawHorizontalMultiChoice(MCH.Config.MCH_ST_Reassembled, $"{ActionWatching.GetActionName(MCH.回转飞锯ChainSaw)}", "", 3, 2);
            }

            if (preset is CustomComboPreset.MCH_AoE_Adv_Reassemble)
            {
                UserConfig.DrawHorizontalMultiChoice(MCH.Config.MCH_AoE_Reassembled,
                    $"{ActionWatching.GetActionName(MCH.散射SpreadShot)}/{ActionWatching.GetActionName(MCH.霰弹枪Scattergun)}", "", 3, 0);
                UserConfig.DrawHorizontalMultiChoice(MCH.Config.MCH_AoE_Reassembled, $"{ActionWatching.GetActionName(MCH.自动弩AutoCrossbow)}", "", 3,
                    1);
                UserConfig.DrawHorizontalMultiChoice(MCH.Config.MCH_AoE_Reassembled, $"{ActionWatching.GetActionName(MCH.回转飞锯ChainSaw)}", "", 3, 2);
            }

            if (preset == CustomComboPreset.MCH_ST_Adv_SecondWind)
                UserConfig.DrawSliderInt(0, 100, MCH.Config.MCH_ST_SecondWindThreshold, $"{ActionWatching.GetActionName(All.内丹SecondWind)} HP %", 150,
                    SliderIncrements.Ones);

            if (preset == CustomComboPreset.MCH_AoE_Adv_SecondWind)
                UserConfig.DrawSliderInt(0, 100, MCH.Config.MCH_AoE_SecondWindThreshold, $"{ActionWatching.GetActionName(All.内丹SecondWind)} HP %",
                    150, SliderIncrements.Ones);

            if (preset == CustomComboPreset.MCH_AoE_Adv_Queen)
                UserConfig.DrawSliderInt(50, 100, MCH.Config.MCH_AoE_TurretUsage, "电池阈值", sliderIncrement: 5);

            if (preset == CustomComboPreset.MCH_AoE_Adv_GaussRicochet)
                UserConfig.DrawAdditionalBoolChoice(MCH.Config.MCH_AoE_Hypercharge, $"Use Outwith {ActionWatching.GetActionName(MCH.超荷Hypercharge)}",
                    "");

            if (preset == CustomComboPreset.MCH_Variant_Cure)
                UserConfig.DrawSliderInt(1, 100, MCH.Config.MCH_VariantCure, "存几层充能？（0 = 用光，一层不留）", 200);

            #endregion

            // ====================================================================================

            #region MONK

            if (preset == CustomComboPreset.MNK_ST_SimpleMode)
                UserConfig.DrawRoundedSliderFloat(5.0f, 10.0f, MNK.Config.MNK_Demolish_Apply, "持续时间低于该秒数，刷新破碎拳");

            if (preset == CustomComboPreset.MNK_ST_SimpleMode)
                UserConfig.DrawRoundedSliderFloat(5.0f, 10.0f, MNK.Config.MNK_DisciplinedFist_Apply, "持续时间低于该秒数，刷新功力.");

            if (preset == CustomComboPreset.MNK_ST_ComboHeals)
            {
                UserConfig.DrawSliderInt(0, 100, MNK.Config.MNK_STSecondWindThreshold, "使用内丹的生命值百分比临界点 (0 = 禁用) (0 = Disabled)", 150,
                    SliderIncrements.Ones);
                UserConfig.DrawSliderInt(0, 100, MNK.Config.MNK_STBloodbathThreshold, "使用浴血的生命值百分比临界点 (0 = 禁用)", 150, SliderIncrements.Ones);
            }

            if (preset == CustomComboPreset.MNK_AoE_ComboHeals)
            {
                UserConfig.DrawSliderInt(0, 100, MNK.Config.MNK_AoESecondWindThreshold, "使用内丹的生命值百分比临界点 (0 = 禁用) (0 = Disabled)", 150,
                    SliderIncrements.Ones);
                UserConfig.DrawSliderInt(0, 100, MNK.Config.MNK_AoEBloodbathThreshold, "使用浴血的生命值百分比临界点 (0 = 禁用)", 150, SliderIncrements.Ones);
            }

            if (preset == CustomComboPreset.MNK_Variant_Cure)
                UserConfig.DrawSliderInt(1, 100, MNK.Config.MNK_VariantCure, "HP% to be at or under", 200);

            #endregion

            // ====================================================================================

            #region NINJA

            if (preset == CustomComboPreset.NIN_Simple_Mudras)
            {
                UserConfig.DrawRadioButton(NIN.Config.NIN_SimpleMudra_Choice, "Mudra Path Set 1",
                    $"1. Ten Mudras -> Fuma Shuriken, Raiton/Hyosho Ranryu, Suiton (Doton under Kassatsu).\nChi Mudras -> Fuma Shuriken, Hyoton, Huton.\nJin Mudras -> Fuma Shuriken, Katon/Goka Mekkyaku, Doton",
                    1);
                UserConfig.DrawRadioButton(NIN.Config.NIN_SimpleMudra_Choice, "Mudra Path Set 2",
                    $"2. Ten Mudras -> Fuma Shuriken, Hyoton/Hyosho Ranryu, Doton.\nChi Mudras -> Fuma Shuriken, Katon, Suiton.\nJin Mudras -> Fuma Shuriken, Raiton/Goka Mekkyaku, Huton (Doton under Kassatsu).",
                    2);
            }

            if (preset == CustomComboPreset.NIN_ST_AdvancedMode_Huraijin)
                UserConfig.DrawSliderInt(0, 60, NIN.Config.Huton_RemainingHuraijinST,
                    "Set the amount of time remaining on Huton the feature should wait before using Huraijin");

            if (preset == CustomComboPreset.NIN_ST_AdvancedMode_ArmorCrush)
            {
                UserConfig.DrawSliderInt(0, 30, NIN.Config.Huton_RemainingArmorCrush,
                    "Set the amount of time remaining on Huton the feature should wait before using Armor Crush", hasAdditionalChoice: true,
                    additonalChoiceCondition: "Value set to 12 or less.");

                if (PluginConfiguration.GetCustomIntValue(NIN.Config.Huton_RemainingArmorCrush) <= 12)
                    UserConfig.DrawAdditionalBoolChoice(NIN.Config.Advanced_DoubleArmorCrush, "Double Armor Crush Feature",
                        "Uses the Armor Crush ender twice before switching back to Aeolian Edge.", isConditionalChoice: true);
            }

            if (preset == CustomComboPreset.NIN_ST_AdvancedMode_Bhavacakra)
                UserConfig.DrawSliderInt(50, 100, NIN.Config.Ninki_BhavaPooling,
                    "Set the minimal amount of Ninki required to have before spending on Bhavacakra.");

            if (preset == CustomComboPreset.NIN_ST_AdvancedMode_TrickAttack)
                UserConfig.DrawSliderInt(0, 21, NIN.Config.Trick_CooldownRemaining,
                    "Set the amount of time remaining on Trick Attack cooldown before trying to set up with Suiton.");

            if (preset == CustomComboPreset.NIN_ST_AdvancedMode_Bunshin)
                UserConfig.DrawSliderInt(50, 100, NIN.Config.Ninki_BunshinPoolingST,
                    "Set the amount of Ninki required to have before spending on Bunshin.");

            if (preset == CustomComboPreset.NIN_AoE_AdvancedMode_Bunshin)
                UserConfig.DrawSliderInt(50, 100, NIN.Config.Ninki_BunshinPoolingAoE,
                    "Set the amount of Ninki required to have before spending on Bunshin.");

            if (preset == CustomComboPreset.NIN_ST_AdvancedMode_TrickAttack_Cooldowns)
                UserConfig.DrawSliderInt(0, 21, NIN.Config.Advanced_Trick_Cooldown,
                    "Set the amount of time remaining on Trick Attack cooldown to start saving cooldowns.");

            if (preset == CustomComboPreset.NIN_ST_AdvancedMode_SecondWind)
                UserConfig.DrawSliderInt(0, 100, NIN.Config.SecondWindThresholdST, "设置使用内丹时的剩余HP百分比");

            if (preset == CustomComboPreset.NIN_ST_AdvancedMode_ShadeShift)
                UserConfig.DrawSliderInt(0, 100, NIN.Config.ShadeShiftThresholdST, "设置使用残影时的剩余HP百分比");

            if (preset == CustomComboPreset.NIN_ST_AdvancedMode_Bloodbath)
                UserConfig.DrawSliderInt(0, 100, NIN.Config.BloodbathThresholdST, "设置使用浴血时的剩余HP百分比");

            if (preset == CustomComboPreset.NIN_AoE_AdvancedMode_SecondWind)
                UserConfig.DrawSliderInt(0, 100, NIN.Config.SecondWindThresholdAoE, "设置使用内丹时的剩余HP百分比");

            if (preset == CustomComboPreset.NIN_AoE_AdvancedMode_ShadeShift)
                UserConfig.DrawSliderInt(0, 100, NIN.Config.ShadeShiftThresholdAoE, "设置使用残影时的剩余HP百分比");

            if (preset == CustomComboPreset.NIN_AoE_AdvancedMode_Bloodbath)
                UserConfig.DrawSliderInt(0, 100, NIN.Config.BloodbathThresholdAoE, "设置使用浴血时的剩余HP百分比");

            if (preset == CustomComboPreset.NIN_AoE_AdvancedMode_HellfrogMedium)
                UserConfig.DrawSliderInt(50, 100, NIN.Config.Ninki_HellfrogPooling, "设置积累多少忍气后使用大虾蟆.");

            if (preset == CustomComboPreset.NIN_AoE_AdvancedMode_Ninjitsus_Doton)
            {
                UserConfig.DrawSliderInt(0, 18, NIN.Config.Advanced_DotonTimer, "设置土遁剩余多长时间时再次使用土遁之术.");
                UserConfig.DrawSliderInt(0, 100, NIN.Config.Advanced_DotonHP, "设置当前目标剩余HP百分比大于多少时使用土遁之术.");
            }

            if (preset == CustomComboPreset.NIN_AoE_AdvancedMode_TCJ)
            {
                UserConfig.DrawRadioButton(NIN.Config.Advanced_TCJEnderAoE, "天地人结束1", "天地人以水遁结束.", 0);
                UserConfig.DrawRadioButton(NIN.Config.Advanced_TCJEnderAoE, $"天地人结束2",
                    "天地人以土遁结束.\nIf you have Doton enabled, Ten Chi Jin will be delayed according to the settings in that feature.", 1);
            }

            if (preset == CustomComboPreset.NIN_ST_AdvancedMode_Ninjitsus_Raiton)
            {
                UserConfig.DrawAdditionalBoolChoice(NIN.Config.Advanced_ChargePool, "Pool Charges",
                    "Waits until at least 2 seconds before your 2nd charge or if Trick Attack debuff is on your target before spending.");
            }

            if (preset == CustomComboPreset.NIN_Variant_Cure)
                UserConfig.DrawSliderInt(1, 100, NIN.Config.NIN_VariantCure, "HP% to be at or under", 200);

            #endregion

            // ====================================================================================

            #region PALADIN
            if (preset is CustomComboPreset.PLD_Advanced_CustomMode)
            {
                List<CustomTimeline> customTimelineList =
                    PluginConfiguration.CustomTimelineList.FindAll(CustomTimeline => CustomTimeline.JobId == PLD.JobID);
                
                
                for (var i = 0; i < customTimelineList.Count; i++)
                {
                    CustomTimeline customTimeline = customTimelineList[i];
                    UserConfig.DrawCustom(customTimeline,customTimelineList);
                }
            }
            

            if (preset == CustomComboPreset.PLD_Requiescat_Options)
            {
                UserConfig.DrawRadioButton(PLD.Config.PLD_RequiescatOption, "悔罪", "", 1);
                UserConfig.DrawRadioButton(PLD.Config.PLD_RequiescatOption, "信仰/真理/英勇", "", 2);
                UserConfig.DrawRadioButton(PLD.Config.PLD_RequiescatOption, "悔罪 & 信仰/真理/英勇", "", 3);
                UserConfig.DrawRadioButton(PLD.Config.PLD_RequiescatOption, "圣灵", "", 4);
                UserConfig.DrawRadioButton(PLD.Config.PLD_RequiescatOption, "圣环", "", 5);
            }


            if (preset == CustomComboPreset.PLD_ST_AdvancedMode_Open)
            {
                UserConfig.DrawRadioButton(PLD.Config.PLD_FOF_GCD, "2GCD", "", 1);
                UserConfig.DrawRadioButton(PLD.Config.PLD_FOF_GCD, "3GCD", "", 2);
            }


            if (preset == CustomComboPreset.PLD_SpiritsWithin)
            {
                UserConfig.DrawRadioButton(PLD.Config.PLD_SpiritsWithinOption, "优先 厄运流转", "", 1);
                UserConfig.DrawRadioButton(PLD.Config.PLD_SpiritsWithinOption, "优先 深奥之灵/偿赎剑", "", 2);
            }

            if (preset == CustomComboPreset.PLD_ST_AdvancedMode_Sheltron || preset == CustomComboPreset.PLD_AoE_AdvancedMode_Sheltron)
            {
                UserConfig.DrawSliderInt(50, 100, PLD.Config.PLD_SheltronOption, "保留多少忠义值", sliderIncrement: 5);
            }

            if (preset == CustomComboPreset.PLD_ST_AdvancedMode_Intervene && enabled)
                UserConfig.DrawSliderInt(0, 1, PLD.Config.PLD_Intervene_HoldCharges, "保留多少MP (0 = 全部使用)");

            if (preset == CustomComboPreset.PLD_ST_AdvancedMode_Intervene)
                UserConfig.DrawAdditionalBoolChoice(PLD.Config.PLD_Intervene_MeleeOnly, "近距离限定", "仅在近战范围内使用调停");

            if (preset == CustomComboPreset.PLD_Variant_Cure)
                UserConfig.DrawSliderInt(1, 100, PLD.Config.PLD_VariantCure, "HP% to be at or under", 200);

            #endregion

            // ====================================================================================

            #region REAPER

            if (preset == CustomComboPreset.RPRPvP_Burst_ImmortalPooling && enabled)
                UserConfig.DrawSliderInt(0, 8, RPRPvP.Config.RPRPvP_ImmortalStackThreshold, "设置保留几层死亡祭品层数后进行爆发输出.###RPR", 150, SliderIncrements.Ones);

            if (preset == CustomComboPreset.RPRPvP_Burst_ArcaneCircle && enabled)
                UserConfig.DrawSliderInt(5, 90, RPRPvP.Config.RPRPvP_ArcaneCircleThreshold, "设定hp百分比. 上限 90 以防止浪费.###RPR", 150,
                    SliderIncrements.Ones);

            if (preset == CustomComboPreset.ReaperPositionalConfig && enabled)
            {
                UserConfig.DrawHorizontalRadioButton(RPR.Config.RPR_PositionalChoice, "优先背部", "优先身位: 缢杀 (背部), 虚无收割.", 1);
                UserConfig.DrawHorizontalRadioButton(RPR.Config.RPR_PositionalChoice, "优先侧面", "优先身位: 绞决 (Flank), 交错收割.", 2);
                UserConfig.DrawHorizontalRadioButton(RPR.Config.RPR_PositionalChoice, "后: 切割, 侧: 死亡之影", "背后使用切割，侧面使用死亡之影.", 3);
                UserConfig.DrawHorizontalRadioButton(RPR.Config.RPR_PositionalChoice, "后: 死亡之影, 侧: 切割", "背后使用死亡之影，侧面使用切割.", 4);
            }

            if (preset == CustomComboPreset.RPR_ST_SliceCombo_SoD && enabled)
            {
                UserConfig.DrawSliderInt(0, 6, RPR.Config.RPR_SoDRefreshRange, "在死亡烙印还剩多少秒时刷新.", 150, SliderIncrements.Ones);
                UserConfig.DrawSliderInt(0, 5, RPR.Config.RPR_SoDThreshold, "设置在多少hp百分比下，不需要刷新死亡烙印buff.", 150, SliderIncrements.Ones);
            }

            if (preset == CustomComboPreset.RPR_Soulsow && enabled)
            {
                UserConfig.DrawHorizontalMultiChoice(RPR.Config.RPR_SoulsowOptions, "勾刃", "添加魂播种至勾刃.", 5, 0);
                UserConfig.DrawHorizontalMultiChoice(RPR.Config.RPR_SoulsowOptions, "切割", "添加魂播种至切割.", 5, 1);
                UserConfig.DrawHorizontalMultiChoice(RPR.Config.RPR_SoulsowOptions, "旋转钐割", "添加魂播种至旋转钐割", 5, 2);
                UserConfig.DrawHorizontalMultiChoice(RPR.Config.RPR_SoulsowOptions, "死亡之影", "添加魂播种至死亡之影.", 5, 3);
                UserConfig.DrawHorizontalMultiChoice(RPR.Config.RPR_SoulsowOptions, "隐匿挥割", "添加魂播种至隐匿挥割.", 5, 4);
            }

            if (preset == CustomComboPreset.RPR_ST_SliceCombo_Opener && enabled)
            {
                UserConfig.DrawHorizontalRadioButton(RPR.Config.RPR_OpenerChoice, "Early Gluttony Opener ", "Uses Early Gluttony Opener.", 1);
                UserConfig.DrawHorizontalRadioButton(RPR.Config.RPR_OpenerChoice, "Early Enshroud Opener",
                    "Uses Early Enshroud Opener. Will Clip CD if not at 2.48-2.49.", 2);
            }


            if (preset == CustomComboPreset.RPR_Variant_Cure)
                UserConfig.DrawSliderInt(1, 100, RPR.Config.RPR_VariantCure, "HP% to be at or under", 200);

            #endregion

            // ====================================================================================

            #region RED MAGE

            if (preset is CustomComboPreset.RDM_Advanced_CustomMode)
            {
                List<CustomTimeline> customTimelineList =
                    PluginConfiguration.CustomTimelineList.FindAll(CustomTimeline => CustomTimeline.JobId == RDM.JobID);


                for (var i = 0; i < customTimelineList.Count; i++)
                {
                    CustomTimeline customTimeline = customTimelineList[i];
                    UserConfig.DrawCustom(customTimeline, customTimelineList);
                }
            }

            if (preset is CustomComboPreset.RDM_ST_oGCD)
            {
                UserConfig.DrawAdditionalBoolChoice(RDM.Config.RDM_ST_oGCD_OnAction_Adv, "Advanced Action Options.",
                    "Changes which action this option will replace.", isConditionalChoice: true);
                if (RDM.Config.RDM_ST_oGCD_OnAction_Adv)
                {
                    ImGui.Indent();
                    ImGui.Spacing();
                    UserConfig.DrawHorizontalMultiChoice(RDM.Config.RDM_ST_oGCD_OnAction, "Jolts", "", 3, 0,
                        descriptionColor: ImGuiColors.DalamudYellow);
                    UserConfig.DrawHorizontalMultiChoice(RDM.Config.RDM_ST_oGCD_OnAction, "Fleche", "", 3, 1,
                        descriptionColor: ImGuiColors.DalamudYellow);
                    UserConfig.DrawHorizontalMultiChoice(RDM.Config.RDM_ST_oGCD_OnAction, "Riposte", "", 3, 2,
                        descriptionColor: ImGuiColors.DalamudYellow);
                    ImGui.Unindent();
                }

                UserConfig.DrawAdditionalBoolChoice(RDM.Config.RDM_ST_oGCD_Fleche, "Fleche", "");
                UserConfig.DrawAdditionalBoolChoice(RDM.Config.RDM_ST_oGCD_ContraSixte, "Contra Sixte", "");
                UserConfig.DrawAdditionalBoolChoice(RDM.Config.RDM_ST_oGCD_Engagement, "Engagement", "", isConditionalChoice: true);
                if (RDM.Config.RDM_ST_oGCD_Engagement)
                {
                    ImGui.Indent();
                    ImGui.Spacing();
                    UserConfig.DrawAdditionalBoolChoice(RDM.Config.RDM_ST_oGCD_Engagement_Pooling, "Pool one charge for manual use.", "");
                    ImGui.Unindent();
                }

                UserConfig.DrawAdditionalBoolChoice(RDM.Config.RDM_ST_oGCD_CorpACorps, "Corp-a-Corps", "", isConditionalChoice: true);
                if (RDM.Config.RDM_ST_oGCD_CorpACorps)
                {
                    ImGui.Indent();
                    ImGui.Spacing();
                    UserConfig.DrawAdditionalBoolChoice(RDM.Config.RDM_ST_oGCD_CorpACorps_Melee, "Use only in melee range.", "");
                    UserConfig.DrawAdditionalBoolChoice(RDM.Config.RDM_ST_oGCD_CorpACorps_Pooling, "Pool one charge for manual use.", "");
                    ImGui.Unindent();
                }
            }

            if (preset is CustomComboPreset.RDM_ST_MeleeCombo)
            {
                UserConfig.DrawAdditionalBoolChoice(RDM.Config.RDM_ST_MeleeCombo_Adv, "Advanced Action Options",
                    "Changes which action this option will replace.", isConditionalChoice: true);
                if (RDM.Config.RDM_ST_MeleeCombo_Adv)
                {
                    ImGui.Indent();
                    ImGui.Spacing();
                    UserConfig.DrawHorizontalMultiChoice(RDM.Config.RDM_ST_MeleeCombo_OnAction, "Jolts", "", 2, 0,
                        descriptionColor: ImGuiColors.DalamudYellow);
                    UserConfig.DrawHorizontalMultiChoice(RDM.Config.RDM_ST_MeleeCombo_OnAction, "Riposte", "", 2, 1,
                        descriptionColor: ImGuiColors.DalamudYellow);
                    ImGui.Unindent();
                }
            }

            if (preset is CustomComboPreset.RDM_ST_MeleeFinisher)
            {
                UserConfig.DrawAdditionalBoolChoice(RDM.Config.RDM_ST_MeleeFinisher_Adv, "Advanced Action Options",
                    "Changes which action this option will replace.", isConditionalChoice: true);
                if (RDM.Config.RDM_ST_MeleeFinisher_Adv)
                {
                    ImGui.Indent();
                    ImGui.Spacing();
                    UserConfig.DrawHorizontalMultiChoice(RDM.Config.RDM_ST_MeleeFinisher_OnAction, "Jolts", "", 3, 0,
                        descriptionColor: ImGuiColors.DalamudYellow);
                    UserConfig.DrawHorizontalMultiChoice(RDM.Config.RDM_ST_MeleeFinisher_OnAction, "Riposte", "", 3, 1,
                        descriptionColor: ImGuiColors.DalamudYellow);
                    UserConfig.DrawHorizontalMultiChoice(RDM.Config.RDM_ST_MeleeFinisher_OnAction, "VerAero & VerThunder", "", 3, 2,
                        descriptionColor: ImGuiColors.DalamudYellow);
                    ImGui.Unindent();
                }
            }

            if (preset is CustomComboPreset.RDM_ST_Lucid)
                UserConfig.DrawSliderInt(0, 10000, RDM.Config.RDM_ST_Lucid_Threshold, "Add Lucid Dreaming when below this MP",
                    sliderIncrement: SliderIncrements.Hundreds);

            // AoE
            if (preset is CustomComboPreset.RDM_AoE_oGCD)
            {
                UserConfig.DrawAdditionalBoolChoice(RDM.Config.RDM_AoE_oGCD_Fleche, "Fleche", "");
                UserConfig.DrawAdditionalBoolChoice(RDM.Config.RDM_AoE_oGCD_ContraSixte, "Contra Sixte", "");
                UserConfig.DrawAdditionalBoolChoice(RDM.Config.RDM_AoE_oGCD_Engagement, "Engagement", "", isConditionalChoice: true);
                if (RDM.Config.RDM_AoE_oGCD_Engagement)
                {
                    ImGui.Indent();
                    ImGui.Spacing();
                    UserConfig.DrawAdditionalBoolChoice(RDM.Config.RDM_AoE_oGCD_Engagement_Pooling, "Pool one charge for manual use.", "");
                    ImGui.Unindent();
                }

                UserConfig.DrawAdditionalBoolChoice(RDM.Config.RDM_AoE_oGCD_CorpACorps, "Corp-a-Corps", "", isConditionalChoice: true);
                if (RDM.Config.RDM_AoE_oGCD_CorpACorps)
                {
                    ImGui.Indent();
                    ImGui.Spacing();
                    UserConfig.DrawAdditionalBoolChoice(RDM.Config.RDM_AoE_oGCD_CorpACorps_Melee, "Use only in melee range.", "");
                    UserConfig.DrawAdditionalBoolChoice(RDM.Config.RDM_AoE_oGCD_CorpACorps_Pooling, "Pool one charge for manual use.", "");
                    ImGui.Unindent();
                }
            }

            if (preset is CustomComboPreset.RDM_AoE_MeleeCombo)
            {
                UserConfig.DrawSliderInt(3, 8, RDM.Config.RDM_AoE_MoulinetRange,
                    "Range to use first Moulinet; no range restrictions after first Moulinet", sliderIncrement: SliderIncrements.Ones);
                UserConfig.DrawAdditionalBoolChoice(RDM.Config.RDM_AoE_MeleeCombo_Adv, "Advanced Action Options",
                    "Changes which action this option will replace.", isConditionalChoice: true);
                if (RDM.Config.RDM_AoE_MeleeCombo_Adv)
                {
                    ImGui.Indent();
                    ImGui.Spacing();
                    UserConfig.DrawHorizontalMultiChoice(RDM.Config.RDM_AoE_MeleeCombo_OnAction, "Scatter/Impact", "", 2, 0,
                        descriptionColor: ImGuiColors.DalamudYellow);
                    UserConfig.DrawHorizontalMultiChoice(RDM.Config.RDM_AoE_MeleeCombo_OnAction, "Moulinet", "", 2, 1,
                        descriptionColor: ImGuiColors.DalamudYellow);
                    ImGui.Unindent();
                }
            }

            if (preset is CustomComboPreset.RDM_AoE_MeleeFinisher)
            {
                UserConfig.DrawAdditionalBoolChoice(RDM.Config.RDM_AoE_MeleeFinisher_Adv, "Advanced Action Options",
                    "Changes which action this option will replace.", isConditionalChoice: true);
                if (RDM.Config.RDM_AoE_MeleeFinisher_Adv)
                {
                    ImGui.Indent();
                    ImGui.Spacing();
                    UserConfig.DrawHorizontalMultiChoice(RDM.Config.RDM_AoE_MeleeFinisher_OnAction, "Scatter/Impact", "", 3, 0,
                        descriptionColor: ImGuiColors.DalamudYellow);
                    UserConfig.DrawHorizontalMultiChoice(RDM.Config.RDM_AoE_MeleeFinisher_OnAction, "Moulinet", "", 3, 1,
                        descriptionColor: ImGuiColors.DalamudYellow);
                    UserConfig.DrawHorizontalMultiChoice(RDM.Config.RDM_AoE_MeleeFinisher_OnAction, "VerAero II & VerThunder II", "", 3, 2,
                        descriptionColor: ImGuiColors.DalamudYellow);
                    ImGui.Unindent();
                }
            }

            if (preset is CustomComboPreset.RDM_AoE_Lucid)
                UserConfig.DrawSliderInt(0, 10000, RDM.Config.RDM_AoE_Lucid_Threshold, "Add Lucid Dreaming when below this MP",
                    sliderIncrement: SliderIncrements.Hundreds);

            if (preset is CustomComboPreset.RDM_Variant_Cure)
                UserConfig.DrawSliderInt(1, 100, RDM.Config.RDM_VariantCure, "HP% to be at or under", 200);

            #endregion

            // ====================================================================================

            #region SAGE

            if (preset is CustomComboPreset.SGE_Advanced_CustomMode)
            {
                List<CustomTimeline> customTimelineList =
                    PluginConfiguration.CustomTimelineList.FindAll(CustomTimeline => CustomTimeline.JobId == SGE.JobID);
                
                
                for (var i = 0; i < customTimelineList.Count; i++)
                {
                    CustomTimeline customTimeline = customTimelineList[i];
                    UserConfig.DrawCustom(customTimeline,customTimelineList);
                }
            }	
            
            if (preset is CustomComboPreset.SGE_ST_DPS)
            {
                UserConfig.DrawAdditionalBoolChoice(SGE.Config.SGE_ST_DPS_Adv, "Advanced Action Options", "Change how Dosis actions are handled",
                    isConditionalChoice: true);

                if (SGE.Config.SGE_ST_DPS_Adv)
                {
                    ImGui.Indent();
                    UserConfig.DrawAdditionalBoolChoice(SGE.Config.SGE_ST_DPS_Adv_D2, "Apply all selected options to Dosis II",
                        "Dosis I & III will behave normally.");
                    UserConfig.DrawAdditionalBoolChoice(SGE.Config.SGE_ST_DPS_Adv_GroupInstants, "Instant Actions on Toxikon",
                        "Adds instant GCDs and oGCDs to Toxikon.\nDefaults to Eukrasia.", isConditionalChoice: true);

                    if (SGE.Config.SGE_ST_DPS_Adv_GroupInstants)
                    {
                        ImGui.Indent();
                        ImGui.Spacing(); //Not sure why I need this, indenting did not work without it
                        UserConfig.DrawHorizontalMultiChoice(SGE.Config.SGE_ST_DPS_Adv_GroupInstants_Addl, "Add Toxikon",
                            "Use Toxikon when Addersting is available.", 2, 0);
                        UserConfig.DrawHorizontalMultiChoice(SGE.Config.SGE_ST_DPS_Adv_GroupInstants_Addl, "Add Dyskrasia",
                            "Use Dyskrasia when in range of a selected enemy target.", 2, 1);
                        ImGui.Unindent();
                    }

                    ImGui.Unindent();
                }
            }

            if (preset is CustomComboPreset.SGE_ST_DPS_EDosis)
            {
                UserConfig.DrawSliderInt(0, 100, SGE.Config.SGE_ST_DPS_EDosisHPPer, "Stop using at Enemy HP %. Set to Zero to disable this check");

                UserConfig.DrawAdditionalBoolChoice(SGE.Config.SGE_ST_DPS_EDosis_Adv, "Advanced Options", "", isConditionalChoice: true);
                if (SGE.Config.SGE_ST_DPS_EDosis_Adv)
                {
                    ImGui.Indent();
                    UserConfig.DrawRoundedSliderFloat(0, 4, SGE.Config.SGE_ST_DPS_EDosisThreshold,
                        "Seconds remaining before reapplying the DoT. Set to Zero to disable this check.", digits: 1);
                    ImGui.Unindent();
                }
            }

            if (preset is CustomComboPreset.SGE_ST_DPS_Lucid)
                UserConfig.DrawSliderInt(4000, 9500, SGE.Config.SGE_ST_DPS_Lucid, "MP Threshold", 150, SliderIncrements.Hundreds);


            if (preset is CustomComboPreset.SGE_ST_DPS_Rhizo)
                UserConfig.DrawSliderInt(0, 1, SGE.Config.SGE_ST_DPS_Rhizo, "Addersgall Threshold", 150, SliderIncrements.Ones);

            if (preset is CustomComboPreset.SGE_ST_DPS_Movement)
            {
                UserConfig.DrawHorizontalMultiChoice(SGE.Config.SGE_ST_DPS_Movement, "Toxikon", "Use Toxikon when Addersting is available.", 3, 0);
                UserConfig.DrawHorizontalMultiChoice(SGE.Config.SGE_ST_DPS_Movement, "Dyskrasia",
                    "Use Dyskrasia when in range of a selected enemy target.", 3, 1);
                UserConfig.DrawHorizontalMultiChoice(SGE.Config.SGE_ST_DPS_Movement, "Eukrasia", "Use Eukrasia.", 3, 2);
            }

            if (preset is CustomComboPreset.SGE_AoE_DPS_Lucid)
                UserConfig.DrawSliderInt(4000, 9500, SGE.Config.SGE_AoE_DPS_Lucid, "MP Threshold", 150, SliderIncrements.Hundreds);

            if (preset is CustomComboPreset.SGE_AoE_DPS_Rhizo)
                UserConfig.DrawSliderInt(0, 1, SGE.Config.SGE_AoE_DPS_Rhizo, "Addersgall Threshold", 150, SliderIncrements.Ones);

            if (preset is CustomComboPreset.SGE_ST_Heal)
            {
                UserConfig.DrawAdditionalBoolChoice(SGE.Config.SGE_ST_Heal_Adv, "Advanced Options", "", isConditionalChoice: true);
                if (SGE.Config.SGE_ST_Heal_Adv)
                {
                    ImGui.Indent();
                    UserConfig.DrawAdditionalBoolChoice(SGE.Config.SGE_ST_Heal_UIMouseOver, "队伍UI鼠标悬停检测",
                        "检测团队成员生命值和Buff，通过将鼠标悬停于小队列表.\n" + "这个功能是用来和Redirect/Reaction/etc结合使用的.（译者注：这三个好像是鼠标悬停施法插件。）");
                    ImGui.Unindent();
                }
            }

            if (preset is CustomComboPreset.SGE_ST_Heal_Esuna)
                UserConfig.DrawSliderInt(0, 100, SGE.Config.SGE_ST_Heal_Esuna, "当生命值低于％时停止使用。将其设置为零以禁用此检查");

            if (preset is CustomComboPreset.SGE_ST_Heal_Soteria)
                UserConfig.DrawSliderInt(0, 100, SGE.Config.SGE_ST_Heal_Soteria, "Use Soteria when Target HP is at or below set percentage");

            if (preset is CustomComboPreset.SGE_ST_Heal_Zoe)
                UserConfig.DrawSliderInt(0, 100, SGE.Config.SGE_ST_Heal_Zoe, "Use Zoe when Target HP is at or below set percentage");

            if (preset is CustomComboPreset.SGE_ST_Heal_Pepsis)
                UserConfig.DrawSliderInt(0, 100, SGE.Config.SGE_ST_Heal_Pepsis, "在目标血量处于或低于设定的百分比时使用消化");

            if (preset is CustomComboPreset.SGE_ST_Heal_Taurochole)
                UserConfig.DrawSliderInt(0, 100, SGE.Config.SGE_ST_Heal_Taurochole, "在目标血量处于或低于设定的百分比时使用白牛清汁");

            if (preset is CustomComboPreset.SGE_ST_Heal_Haima)
                UserConfig.DrawSliderInt(0, 100, SGE.Config.SGE_ST_Heal_Haima, "在目标血量处于或低于设定的百分比时使用输血");

            if (preset is CustomComboPreset.SGE_ST_Heal_Krasis)
                UserConfig.DrawSliderInt(0, 100, SGE.Config.SGE_ST_Heal_Krasis, "在目标血量处于或低于设定的百分比时使用混合");

            if (preset is CustomComboPreset.SGE_ST_Heal_Druochole)
                UserConfig.DrawSliderInt(0, 100, SGE.Config.SGE_ST_Heal_Druochole, "在目标血量处于或低于设定的百分比时使用灵橡清汁");

            if (preset is CustomComboPreset.SGE_ST_Heal_EDiagnosis)
            {
                UserConfig.DrawSliderInt(0, 100, SGE.Config.SGE_ST_Heal_EDiagnosisHP,
                    "Use Eukrasian Diagnosis when Target HP is at or below set percentage");
                UserConfig.DrawHorizontalMultiChoice(SGE.Config.SGE_ST_Heal_EDiagnosisOpts, "Ignore Shield Check",
                    "Warning, will force the use of Eukrasia Diagnosis, and normal Diagnosis will be unavailable.", 2, 0);
                UserConfig.DrawHorizontalMultiChoice(SGE.Config.SGE_ST_Heal_EDiagnosisOpts, "Check for Scholar Galvenize",
                    "Enable to not override an existing Scholar's shield.", 2, 1);
            }

            if (preset is CustomComboPreset.SGE_AoE_Heal_Kerachole)
                UserConfig.DrawAdditionalBoolChoice(SGE.Config.SGE_AoE_Heal_KeracholeTrait, "Check for Enhanced Kerachole Trait (Heal over Time)",
                    "Enabling this will prevent Kerachole from being used when the Heal over Time trait is unavailable.");

            if (preset is CustomComboPreset.SGE_Eukrasia)
            {
                UserConfig.DrawRadioButton(SGE.Config.SGE_Eukrasia_Mode, "Eukrasian Dosis", "", 0);
                UserConfig.DrawRadioButton(SGE.Config.SGE_Eukrasia_Mode, "Eukrasian Diagnosis", "", 1);
                UserConfig.DrawRadioButton(SGE.Config.SGE_Eukrasia_Mode, "Eukrasian Prognosis", "", 2);
            }

            #endregion

            // ====================================================================================

            #region SAMURAI

            if (preset == CustomComboPreset.SAM_ST_Overcap && enabled)
                UserConfig.DrawSliderInt(0, 85, SAM.Config.SAM_ST_KenkiOvercapAmount, "设置单体连击剑气积累数量.");
            if (preset == CustomComboPreset.SAM_AoE_Overcap && enabled)
                UserConfig.DrawSliderInt(0, 85, SAM.Config.SAM_AoE_KenkiOvercapAmount, "设置AOE连击剑气积累数量.");

            if (preset == CustomComboPreset.SAM_ST_GekkoCombo_CDs_MeikyoShisui && enabled)
            {
                UserConfig.DrawHorizontalRadioButton(SAM.Config.SAM_MeikyoChoice, "Use after Hakaze/Sen Applier",
                    "Uses Meikyo Shisui after Hakaze, Gekko, Yukikaze, or Kasha.", 1);
                UserConfig.DrawHorizontalRadioButton(SAM.Config.SAM_MeikyoChoice, "Use outside of combo chain",
                    "Uses Meikyo Shisui outside of a combo chain.", 2);
            }

            //PvP
            if (preset == CustomComboPreset.SAMPvP_BurstMode && enabled)
                UserConfig.DrawSliderInt(0, 2, SAMPvP.Config.SAMPvP_SotenCharges, "存几层必杀剑·早天? (0 = 全用，一层不留).");

            if (preset == CustomComboPreset.SAMPvP_KashaFeatures_GapCloser && enabled)
                UserConfig.DrawSliderInt(0, 100, SAMPvP.Config.SAMPvP_SotenHP, "使用必杀剑·早天当敌人低于设置的血量.");

            //Fillers
            if (preset == CustomComboPreset.SAM_ST_GekkoCombo_FillerCombos)
            {
                UserConfig.DrawHorizontalRadioButton(SAM.Config.SAM_FillerCombo, "2.14+", "2 Filler GCDs", 1);
                UserConfig.DrawHorizontalRadioButton(SAM.Config.SAM_FillerCombo, "2.06 - 2.08",
                    "3 Filler GCDs. \nWill use Yaten into Enpi as part of filler and Gyoten back into Range.\nHakaze will be delayed by half a GCD after Enpi.",
                    2);
                UserConfig.DrawHorizontalRadioButton(SAM.Config.SAM_FillerCombo, "1.99 - 2.01", "4 Filler GCDs. \nUses double Yukikaze loop.", 3);
            }

            if (preset == CustomComboPreset.SAM_ST_GekkoCombo_CDs_Iaijutsu)
            {
                UserConfig.DrawSliderInt(0, 100, SAM.Config.SAM_ST_Higanbana_Threshold,
                    "Stop using Higanbana on targets below this HP % (0% = always use).", 150, SliderIncrements.Ones);
            }

            if (preset == CustomComboPreset.SAM_ST_ComboHeals)
            {
                UserConfig.DrawSliderInt(0, 100, SAM.Config.SAM_STSecondWindThreshold, "HP percent threshold to use Second Wind below (0 = Disabled)",
                    150, SliderIncrements.Ones);
                UserConfig.DrawSliderInt(0, 100, SAM.Config.SAM_STBloodbathThreshold, "HP percent threshold to use Bloodbath (0 = Disabled)", 150,
                    SliderIncrements.Ones);
            }

            if (preset == CustomComboPreset.SAM_AoE_ComboHeals)
            {
                UserConfig.DrawSliderInt(0, 100, SAM.Config.SAM_AoESecondWindThreshold,
                    "HP percent threshold to use Second Wind below (0 = Disabled)", 150, SliderIncrements.Ones);
                UserConfig.DrawSliderInt(0, 100, SAM.Config.SAM_AoEBloodbathThreshold, "HP percent threshold to use Bloodbath below (0 = Disabled)",
                    150, SliderIncrements.Ones);
            }

            if (preset == CustomComboPreset.SAM_ST_Execute)
            {
                UserConfig.DrawSliderInt(0, 100, SAM.Config.SAM_ST_ExecuteThreshold, "HP percent threshold to use Shinten below", 150,
                    SliderIncrements.Ones);
            }

            if (preset == CustomComboPreset.SAM_Variant_Cure)
                UserConfig.DrawSliderInt(1, 100, SAM.Config.SAM_VariantCure, "HP% to be at or under", 200);

            #endregion

            // ====================================================================================

            #region SCHOLAR

            if (preset is CustomComboPreset.SCH_Advanced_CustomMode)
            {
                List<CustomTimeline> customTimelineList =
                    PluginConfiguration.CustomTimelineList.FindAll(CustomTimeline => CustomTimeline.JobId == SCH.JobID);
                
                
                for (var i = 0; i < customTimelineList.Count; i++)
                {
                    CustomTimeline customTimeline = customTimelineList[i];
                    UserConfig.DrawCustom(customTimeline,customTimelineList);
                }
            }	
            
            if (preset is CustomComboPreset.SCH_DPS)
            {
                UserConfig.DrawAdditionalBoolChoice(SCH.Config.SCH_ST_DPS_Adv, "Advanced Action Options", "Change how actions are handled",
                    isConditionalChoice: true);

                if (SCH.Config.SCH_ST_DPS_Adv)
                {
                    ImGui.Indent();
                    ImGui.Spacing();
                    UserConfig.DrawHorizontalMultiChoice(SCH.Config.SCH_ST_DPS_Adv_Actions, "On Ruin/Broils", "Apply options to Ruin and all Broils.",
                        3, 0);
                    UserConfig.DrawHorizontalMultiChoice(SCH.Config.SCH_ST_DPS_Adv_Actions, "On Bio/Bio II/Biolysis",
                        "Apply options to Bio and Biolysis.", 3, 1);
                    UserConfig.DrawHorizontalMultiChoice(SCH.Config.SCH_ST_DPS_Adv_Actions, "On Ruin II", "Apply options to Ruin II.", 3, 2);
                    ImGui.Unindent();
                }
            }

            if (preset is CustomComboPreset.SCH_DPS_Lucid)
                UserConfig.DrawSliderInt(4000, 9500, SCH.Config.SCH_ST_DPS_LucidOption, "MP Threshold", 150, SliderIncrements.Hundreds);

            if (preset is CustomComboPreset.SCH_DPS_Bio)
            {
                UserConfig.DrawSliderInt(0, 100, SCH.Config.SCH_ST_DPS_BioOption, "在敌人HP为百分之多少时时停止使用，设置为零可禁用");

                UserConfig.DrawAdditionalBoolChoice(SCH.Config.SCH_ST_DPS_Bio_Adv, "高级选项", "", isConditionalChoice: true);
                if (PluginConfiguration.GetCustomBoolValue(SCH.Config.SCH_ST_DPS_Bio_Adv))
                {
                    ImGui.Indent();
                    UserConfig.DrawRoundedSliderFloat(0, 4, SCH.Config.SCH_ST_DPS_Bio_Threshold, "续 DoT 前的剩余秒数，设置为零可禁用", digits: 1);
                    ImGui.Unindent();
                }
            }

            if (preset is CustomComboPreset.SCH_DPS_ChainStrat)
                UserConfig.DrawSliderInt(0, 100, SCH.Config.SCH_ST_DPS_ChainStratagemOption, "在敌人HP为百分之多少时时停止使用，设置为零可禁用");

            if (preset is CustomComboPreset.SCH_DPS_EnergyDrain)
            {
                UserConfig.DrawAdditionalBoolChoice(SCH.Config.SCH_ST_DPS_EnergyDrain_Adv, "高级选项", "", isConditionalChoice: true);
                if (SCH.Config.SCH_ST_DPS_EnergyDrain_Adv)
                {
                    ImGui.Indent();
                    UserConfig.DrawRoundedSliderFloat(0, 60, SCH.Config.SCH_ST_DPS_EnergyDrain, "以太超流剩余冷却时间", digits: 1);
                    ImGui.Unindent();
                }
            }

            if (preset is CustomComboPreset.SCH_AoE_Lucid)
                UserConfig.DrawSliderInt(4000, 9500, SCH.Config.SCH_AoE_LucidOption, "MP Threshold", 150, SliderIncrements.Hundreds);

            if (preset is CustomComboPreset.SCH_ST_Heal)
            {
                UserConfig.DrawAdditionalBoolChoice(SCH.Config.SCH_ST_Heal_Adv, "高级选项", "", isConditionalChoice: true);
                if (SCH.Config.SCH_ST_Heal_Adv)
                {
                    ImGui.Indent();
                    UserConfig.DrawAdditionalBoolChoice(SCH.Config.SCH_ST_Heal_UIMouseOver, "Party UI Mouseover Checking",
                        "Check party member's HP & Debuffs by using mouseover on the party list.\n" +
                        "To be used in conjunction with Redirect/Reaction/etc");
                    ImGui.Unindent();
                }
            }

            if (preset is CustomComboPreset.SCH_ST_Heal_Lucid)
                UserConfig.DrawSliderInt(4000, 9500, SCH.Config.SCH_ST_Heal_LucidOption, "MP Threshold", 150, SliderIncrements.Hundreds);

            if (preset is CustomComboPreset.SCH_ST_Heal_Adloquium)
                UserConfig.DrawSliderInt(0, 100, SCH.Config.SCH_ST_Heal_AdloquiumOption,
                    "Use Adloquium on targets at or below HP % even if they have Galvanize\n0 = Only ever use Adloquium on targets without Galvanize\n100 = Always use Adloquium");

            if (preset is CustomComboPreset.SCH_ST_Heal_Lustrate)
                UserConfig.DrawSliderInt(0, 100, SCH.Config.SCH_ST_Heal_LustrateOption,
                    "Start using when below HP %. Set to 100 to disable this check");

            if (preset is CustomComboPreset.SCH_DeploymentTactics)
            {
                UserConfig.DrawAdditionalBoolChoice(SCH.Config.SCH_DeploymentTactics_Adv, "高级选项", "", isConditionalChoice: true);
                if (SCH.Config.SCH_DeploymentTactics_Adv)
                {
                    ImGui.Indent();
                    UserConfig.DrawAdditionalBoolChoice(SCH.Config.SCH_DeploymentTactics_UIMouseOver, "Party UI Mouseover Checking",
                        "Check party member's HP & Debuffs by using mouseover on the party list.\n" +
                        "To be used in conjunction with Redirect/Reaction/etc");
                    ImGui.Unindent();
                }
            }

            if (preset is CustomComboPreset.SCH_FairyReminder)
            {
                UserConfig.DrawRadioButton(SCH.Config.SCH_FairyFeature, "Eos", "", 0);
                UserConfig.DrawRadioButton(SCH.Config.SCH_FairyFeature, "Selene", "", 1);
            }

            if (preset is CustomComboPreset.SCH_Aetherflow)
            {
                UserConfig.DrawRadioButton(SCH.Config.SCH_Aetherflow_Display, "仅应用于能量吸收", "", 0);
                UserConfig.DrawRadioButton(SCH.Config.SCH_Aetherflow_Display, "应用于所有使用以太的技能", "", 1);
            }

            if (preset is CustomComboPreset.SCH_Aetherflow_Recite)
            {
                UserConfig.DrawAdditionalBoolChoice(SCH.Config.SCH_Aetherflow_Recite_Excog, "On Excogitation", "", isConditionalChoice: true);
                if (SCH.Config.SCH_Aetherflow_Recite_Excog)
                {
                    ImGui.Indent();
                    ImGui.Spacing();
                    UserConfig.DrawRadioButton(SCH.Config.SCH_Aetherflow_Recite_ExcogMode, "仅当以太溢出时", "", 0);
                    UserConfig.DrawRadioButton(SCH.Config.SCH_Aetherflow_Recite_ExcogMode, "总是应用", "", 1);
                    ImGui.Unindent();
                }

                UserConfig.DrawAdditionalBoolChoice(SCH.Config.SCH_Aetherflow_Recite_Indom, "On Indominability", "", isConditionalChoice: true);
                if (SCH.Config.SCH_Aetherflow_Recite_Indom)
                {
                    ImGui.Indent();
                    ImGui.Spacing();
                    UserConfig.DrawRadioButton(SCH.Config.SCH_Aetherflow_Recite_IndomMode, "仅当以太溢出时", "", 0);
                    UserConfig.DrawRadioButton(SCH.Config.SCH_Aetherflow_Recite_IndomMode, "总是应用", "", 1);
                    ImGui.Unindent();
                }
            }

            if (preset is CustomComboPreset.SCH_Recitation)
            {
                UserConfig.DrawRadioButton(SCH.Config.SCH_Recitation_Mode, "Adloquium", "", 0);
                UserConfig.DrawRadioButton(SCH.Config.SCH_Recitation_Mode, "Succor", "", 1);
                UserConfig.DrawRadioButton(SCH.Config.SCH_Recitation_Mode, "Indomitability", "", 2);
                UserConfig.DrawRadioButton(SCH.Config.SCH_Recitation_Mode, "Excogitation", "", 3);
            }

            #endregion

            // ====================================================================================

            #region SUMMONER

            #region PvE

            if (preset is CustomComboPreset.SMN_Advanced_CustomMode)
            {
                List<CustomTimeline> customTimelineList =
                    PluginConfiguration.CustomTimelineList.FindAll(CustomTimeline => CustomTimeline.JobId == SMN.JobID);


                for (var i = 0; i < customTimelineList.Count; i++)
                {
                    CustomTimeline customTimeline = customTimelineList[i];
                    UserConfig.DrawCustom(customTimeline, customTimelineList);
                }
            }

            if (preset == CustomComboPreset.SMN_DemiEgiMenu_EgiOrder)
            {
                UserConfig.DrawHorizontalRadioButton(SMN.Config.召唤顺序, "土风火", "按泰坦，迦楼罗，伊芙利特的顺序召唤", 1);
                UserConfig.DrawHorizontalRadioButton(SMN.Config.召唤顺序, "土火风", "按迦楼罗，泰坦，伊芙利特的顺序召唤.", 2);

                UserConfig.DrawHorizontalRadioButton(SMN.Config.召唤顺序, "风土火", "按迦楼罗，泰坦，伊芙利特的顺序召唤.", 3);
                UserConfig.DrawHorizontalRadioButton(SMN.Config.召唤顺序, "风火土", "按迦楼罗，泰坦，伊芙利特的顺序召唤.", 4);

                UserConfig.DrawHorizontalRadioButton(SMN.Config.召唤顺序, "火风土", "按迦楼罗，泰坦，伊芙利特的顺序召唤.", 5);
                UserConfig.DrawHorizontalRadioButton(SMN.Config.召唤顺序, "火土风", "按迦楼罗，泰坦，伊芙利特的顺序召唤.", 6);
            }

            if (preset == CustomComboPreset.SMN_DemiEgiMenu_oGCDPooling)
                UserConfig.DrawSliderInt(0, 3, SMN.Config.SMN_Burst_Delay, "延迟几个GCD打爆发", 150, SliderIncrements.Ones);

            if (preset == CustomComboPreset.SMN_DemiEgiMenu_oGCDPooling)
            {
                UserConfig.DrawHorizontalRadioButton(SMN.Config.SMN_BurstPhase, "巴哈姆特", "巴哈阶段爆发.", 1);
                UserConfig.DrawHorizontalRadioButton(SMN.Config.SMN_BurstPhase, "菲尼克斯", "凤凰阶段爆发.", 2);
                UserConfig.DrawHorizontalRadioButton(SMN.Config.SMN_BurstPhase, "巴哈姆特或菲尼克斯", "巴哈或凤凰阶段爆发 (取决于那个先到).", 3);
                UserConfig.DrawHorizontalRadioButton(SMN.Config.SMN_BurstPhase, "好了就用 ", "当灼热之光时爆发, 而不管处在哪个阶段.", 4);
            }

            if (preset == CustomComboPreset.SMN_DemiEgiMenu_SwiftcastEgi)
            {
                UserConfig.DrawHorizontalRadioButton(SMN.Config.SMN_SwiftcastPhase, "迦楼罗", "即刻用来使用螺旋气流", 1);
                UserConfig.DrawHorizontalRadioButton(SMN.Config.SMN_SwiftcastPhase, "伊芙利特", "即刻用来使用红宝石之仪", 2);
                UserConfig.DrawHorizontalRadioButton(SMN.Config.SMN_SwiftcastPhase, "好了就用", "当即刻可用，用来使用第一个能用的灵攻技能.", 3);
            }


            if (preset == CustomComboPreset.SMN_Lucid)
                UserConfig.DrawSliderInt(4000, 9500, SMN.Config.SMN_Lucid, "MP小于等于此值时使用.", 150, SliderIncrements.Hundreds);

            if (preset == CustomComboPreset.SMN_Variant_Cure)
                UserConfig.DrawSliderInt(1, 100, SMN.Config.SMN_VariantCure, "HP% to be at or under", 200);

            #endregion

            #region PvP

            if (preset == CustomComboPreset.SMNPvP_BurstMode)
                UserConfig.DrawSliderInt(50, 100, SMNPvP.Config.SMNPvP_FesterThreshold,
                    "Target HP% to cast Fester below.\nSet to 100 use Fester as soon as it's available.###SMNPvP", 150, SliderIncrements.Ones);

            if (preset == CustomComboPreset.SMNPvP_BurstMode_RadiantAegis)
                UserConfig.DrawSliderInt(0, 90, SMNPvP.Config.SMNPvP_RadiantAegisThreshold, "Caps at 90 to prevent waste.###SMNPvP", 150,
                    SliderIncrements.Ones);

            #endregion

            #endregion

            // ====================================================================================

            #region WARRIOR

            if (preset is CustomComboPreset.WAR_Advanced_CustomMode)
            {
                List<CustomTimeline> customTimelineList =
                    PluginConfiguration.CustomTimelineList.FindAll(CustomTimeline => CustomTimeline.JobId == WAR.JobID);
                
                
                for (var i = 0; i < customTimelineList.Count; i++)
                {
                    CustomTimeline customTimeline = customTimelineList[i];
                    UserConfig.DrawCustom(customTimeline,customTimelineList);
                }
            }
            
            if (preset == CustomComboPreset.WAR_InfuriateFellCleave && enabled)
                UserConfig.DrawSliderInt(0, 50, WAR.Config.WAR_InfuriateRange, "设置怒气值不超过多少时使用此功能。");

            if (preset == CustomComboPreset.WAR_ST_StormsPath && enabled)
                UserConfig.DrawSliderInt(0, 30, WAR.Config.WAR_SurgingRefreshRange, "刷新风暴之径前剩余秒数。");

            if (preset == CustomComboPreset.WAR_ST_StormsPath_Onslaught && enabled)
                UserConfig.DrawSliderInt(0, 2, WAR.Config.WAR_KeepOnslaughtCharges, "存几层充能？（0 = 用光，一层不留）");

            if (preset == CustomComboPreset.WAR_Variant_Cure)
                UserConfig.DrawSliderInt(1, 100, WAR.Config.WAR_VariantCure, "存几层充能？（0 = 用光，一层不留）", 200);

            if (preset == CustomComboPreset.WAR_ST_StormsPath_FellCleave)
                UserConfig.DrawSliderInt(50, 100, WAR.Config.WAR_FellCleaveGauge, "最小消耗仪表盘值");

            if (preset == CustomComboPreset.WAR_AoE_Overpower_Decimate)
                UserConfig.DrawSliderInt(50, 100, WAR.Config.WAR_DecimateGauge, "最小消耗仪表盘值");

            if (preset == CustomComboPreset.WAR_ST_StormsPath_Infuriate)
                UserConfig.DrawSliderInt(0, 50, WAR.Config.WAR_InfuriateSTGauge, "当仪表盘低于或等于该值时使用");

            if (preset == CustomComboPreset.WAR_AoE_Overpower_Infuriate)
                UserConfig.DrawSliderInt(0, 50, WAR.Config.WAR_InfuriateAoEGauge, "当仪表盘低于或等于该值时使用");

            if (preset == CustomComboPreset.WARPvP_BurstMode_Blota)
            {
                UserConfig.DrawHorizontalRadioButton(WARPvP.Config.WARPVP_BlotaTiming, "Before Primal Rend", "", 0);
                UserConfig.DrawHorizontalRadioButton(WARPvP.Config.WARPVP_BlotaTiming, "After Primal Rend", "", 1);
            }

            #endregion

            // ====================================================================================

            #region WHITE MAGE

            if (preset is CustomComboPreset.WHM_Advanced_CustomMode)
            {
                List<CustomTimeline> customTimelineList =
                    PluginConfiguration.CustomTimelineList.FindAll(CustomTimeline => CustomTimeline.JobId == WHM.JobID);
                
                
                for (var i = 0; i < customTimelineList.Count; i++)
                {
                    CustomTimeline customTimeline = customTimelineList[i];
                    UserConfig.DrawCustom(customTimeline,customTimelineList);
                }
            }	
            
            if (preset is CustomComboPreset.WHM_ST_MainCombo)
            {
                UserConfig.DrawAdditionalBoolChoice(WHM.Config.WHM_ST_MainCombo_Adv, "高级选项", "Change how actions are handled",
                    isConditionalChoice: true);

                if (WHM.Config.WHM_ST_MainCombo_Adv)
                {
                    ImGui.Indent();
                    ImGui.Spacing();
                    UserConfig.DrawHorizontalMultiChoice(WHM.Config.WHM_ST_MainCombo_Adv_Actions, "On Stone/Glare",
                        "Apply options to all Stones and Glares.", 3, 0);
                    UserConfig.DrawHorizontalMultiChoice(WHM.Config.WHM_ST_MainCombo_Adv_Actions, "On Aero/Dia", "Apply options to Aeros and Dia.", 3,
                        1);
                    UserConfig.DrawHorizontalMultiChoice(WHM.Config.WHM_ST_MainCombo_Adv_Actions, "On Stone II", "Apply options to Stone II.", 3, 2);
                    ImGui.Unindent();
                }
            }

            if (preset == CustomComboPreset.WHM_ST_MainCombo_Lucid)
                UserConfig.DrawSliderInt(4000, 9500, WHM.Config.WHM_STDPS_Lucid, "设置 MP 值以使此功能正常工作的阈值为或低于该值", 150, SliderIncrements.Hundreds);

            if (preset is CustomComboPreset.WHM_ST_MainCombo_DoT)
            {
                UserConfig.DrawSliderInt(0, 100, WHM.Config.WHM_STDPS_MainCombo_DoT, "Stop using at Enemy HP %. Set to Zero to disable this check.");

                UserConfig.DrawAdditionalBoolChoice(WHM.Config.WHM_ST_MainCombo_DoT_Adv, "高级选项", "", isConditionalChoice: true);
                if (PluginConfiguration.GetCustomBoolValue(WHM.Config.WHM_ST_MainCombo_DoT_Adv))
                {
                    ImGui.Indent();
                    UserConfig.DrawRoundedSliderFloat(0, 4, WHM.Config.WHM_ST_MainCombo_DoT_Threshold, "续 DoT 前的剩余秒数，设置为零可禁用", digits: 1);
                    ImGui.Unindent();
                }
            }

            if (preset == CustomComboPreset.WHM_AoE_DPS_Lucid)
                UserConfig.DrawSliderInt(4000, 9500, WHM.Config.WHM_AoEDPS_Lucid, "设置 MP 值以使此功能正常工作的阈值为或低于该值", 150, SliderIncrements.Hundreds);

            if (preset == CustomComboPreset.WHM_AoEHeals_Lucid)
                UserConfig.DrawSliderInt(4000, 9500, WHM.Config.WHM_AoEHeals_Lucid, "设置 MP 值以使此功能正常工作的阈值为或低于该值", 150, SliderIncrements.Hundreds);

            if (preset == CustomComboPreset.WHM_STHeals_Lucid)
                UserConfig.DrawSliderInt(4000, 9500, WHM.Config.WHM_STHeals_Lucid, "设置 MP 值以使此功能正常工作的阈值为或低于该值", 150, SliderIncrements.Hundreds);

            if (preset is CustomComboPreset.WHM_STHeals_Esuna)
                UserConfig.DrawSliderInt(0, 100, WHM.Config.WHM_STHeals_Esuna, "当生命值低于％时停止使用。将其设置为零以禁用此检查");

            if (preset == CustomComboPreset.WHM_AoeHeals_ThinAir)
                UserConfig.DrawSliderInt(0, 1, WHM.Config.WHM_AoEHeals_ThinAir, "存几层充能？（0 = 用光，一层不留）");

            if (preset == CustomComboPreset.WHM_AoEHeals_Cure3)
                UserConfig.DrawSliderInt(0, 10000, WHM.Config.WHM_AoEHeals_Cure3MP, "当HP低于多少", sliderIncrement: 100);

            if (preset == CustomComboPreset.WHM_STHeals)
                UserConfig.DrawAdditionalBoolChoice(WHM.Config.WHM_STHeals_UIMouseOver, "队伍UI鼠标悬停检测",
                    "检测团队成员生命值和Buff，通过将鼠标悬停于小队列表.\n" + "这个功能是用来和Redirect/Reaction/etc结合使用的.（译者注：这三个好像是鼠标悬停施法插件。）");

            if (preset == CustomComboPreset.WHM_STHeals_ThinAir)
                UserConfig.DrawSliderInt(0, 1, WHM.Config.WHM_STHeals_ThinAir, "存几层充能？（0 = 用光，一层不留）");

            if (preset == CustomComboPreset.WHM_STHeals_Regen)
                UserConfig.DrawRoundedSliderFloat(0f, 6f, WHM.Config.WHM_STHeals_RegenTimer, "Time Remaining Before Refreshing");

            if (preset == CustomComboPreset.WHM_ST_MainCombo_Opener)
                UserConfig.DrawAdditionalBoolChoice(WHM.Config.WHM_ST_MainCombo_Opener_Swiftcast, "Swiftcast Option",
                    "Adds Swiftcast to the opener.");

            if (preset == CustomComboPreset.WHM_STHeals_Benediction)
            {
                UserConfig.DrawAdditionalBoolChoice(WHM.Config.WHM_STHeals_BenedictionWeave, "只在能力技窗口期插入", "");
                UserConfig.DrawSliderInt(1, 100, WHM.Config.WHM_STHeals_BenedictionHP, "在目标生命值百分比等于或低于时使用。");
            }

            if (preset == CustomComboPreset.WHM_STHeals_Tetragrammaton)
            {
                UserConfig.DrawAdditionalBoolChoice(WHM.Config.WHM_STHeals_TetraWeave, "只在能力技窗口期插入", "");
                UserConfig.DrawSliderInt(1, 100, WHM.Config.WHM_STHeals_TetraHP, "在目标生命值百分比等于或低于时使用。");
            }

            if (preset == CustomComboPreset.WHM_STHeals_Benison)
            {
                UserConfig.DrawAdditionalBoolChoice(WHM.Config.WHM_STHeals_BenisonWeave, "只在能力技窗口期插入", "");
                UserConfig.DrawSliderInt(1, 100, WHM.Config.WHM_STHeals_BenisonHP, "在目标生命值百分比等于或低于时使用。");
            }

            if (preset == CustomComboPreset.WHM_STHeals_Aquaveil)
            {
                UserConfig.DrawAdditionalBoolChoice(WHM.Config.WHM_STHeals_AquaveilWeave, "只在能力技窗口期插入", "");
                UserConfig.DrawSliderInt(1, 100, WHM.Config.WHM_STHeals_AquaveilHP, "在目标生命值百分比等于或低于时使用。");
            }

            if (preset == CustomComboPreset.WHM_AoEHeals_Assize)
            {
                UserConfig.DrawAdditionalBoolChoice(WHM.Config.WHM_AoEHeals_AssizeWeave, "只在能力技窗口期插入", "");
            }

            if (preset == CustomComboPreset.WHM_AoEHeals_Plenary)
            {
                UserConfig.DrawAdditionalBoolChoice(WHM.Config.WHM_AoEHeals_PlenaryWeave, "只在能力技窗口期插入", "");
            }

            #endregion

            // ====================================================================================

            #region DOH

            #endregion

            // ====================================================================================

            #region DOL

            #endregion

            // ====================================================================================

            #region PvP VALUES

            PlayerCharacter? pc = Service.ClientState.LocalPlayer;

            if (preset == CustomComboPreset.PvP_EmergencyHeals)
            {
                if (pc != null)
                {
                    uint maxHP = Service.ClientState.LocalPlayer?.MaxHp <= 15000 ? 0 : Service.ClientState.LocalPlayer.MaxHp - 15000;

                    if (maxHP > 0)
                    {
                        int setting = PluginConfiguration.GetCustomIntValue(PvPCommon.Config.EmergencyHealThreshold);
                        float hpThreshold = (float)maxHP / 100 * setting;

                        UserConfig.DrawSliderInt(1, 100, PvPCommon.Config.EmergencyHealThreshold,
                            $"设置百分比数值，低于等于时发挥本功能效果.\n为了防止满血使用浪费。100%指的是你的血量上限-15000.\n生命值低于或等于: {hpThreshold}");
                    }

                    else
                    {
                        UserConfig.DrawSliderInt(1, 100, PvPCommon.Config.EmergencyHealThreshold,
                            "设置百分比数值，低于等于时发挥本功能效果.\n为了防止满血使用浪费。100%指的是你的血量上限-15000.");
                    }
                }

                else
                {
                    UserConfig.DrawSliderInt(1, 100, PvPCommon.Config.EmergencyHealThreshold,
                        "设置百分比数值，低于等于时发挥本功能效果.\n为了防止满血使用浪费。100%指的是你的血量上限-15000.");
                }
            }

            if (preset == CustomComboPreset.PvP_EmergencyGuard)
                UserConfig.DrawSliderInt(1, 100, PvPCommon.Config.EmergencyGuardThreshold, "设置百分比数值，低于等于时发挥本功能效果.");

            if (preset == CustomComboPreset.PvP_QuickPurify)
                UserConfig.DrawPvPStatusMultiChoice(PvPCommon.Config.QuickPurifyStatuses);

            if (preset == CustomComboPreset.NINPvP_ST_Meisui)
            {
                string description = "设置百分比数值，低于等于时发挥本功能效果.\n为了防止满血使用浪费。100%指的是你的血量上限-8000.";

                if (pc != null)
                {
                    uint maxHP = pc.MaxHp <= 8000 ? 0 : pc.MaxHp - 8000;
                    if (maxHP > 0)
                    {
                        int setting = PluginConfiguration.GetCustomIntValue(NINPvP.Config.NINPvP_Meisui_ST);
                        float hpThreshold = (float)maxHP / 100 * setting;

                        description += $"\n生命值低于或等于: {hpThreshold}";
                    }
                }

                UserConfig.DrawSliderInt(1, 100, NINPvP.Config.NINPvP_Meisui_ST, description);
            }

            if (preset == CustomComboPreset.NINPvP_AoE_Meisui)
            {
                string description = "设置百分比数值，低于等于时发挥本功能效果.\n为了防止满血使用浪费。100%指的是你的血量上限-8000.";

                if (pc != null)
                {
                    uint maxHP = pc.MaxHp <= 8000 ? 0 : pc.MaxHp - 8000;
                    if (maxHP > 0)
                    {
                        int setting = PluginConfiguration.GetCustomIntValue(NINPvP.Config.NINPvP_Meisui_AoE);
                        float hpThreshold = (float)maxHP / 100 * setting;

                        description += $"\n生命值低于或等于: {hpThreshold}";
                    }
                }

                UserConfig.DrawSliderInt(1, 100, NINPvP.Config.NINPvP_Meisui_AoE, description);
            }

            #endregion
        }
    }

    public static class SliderIncrements
    {
        public const uint Ones = 1,
            Fives = 5,
            Tens = 10,
            Hundreds = 100,
            Thousands = 1000;
    }
}