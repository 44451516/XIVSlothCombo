using System.Collections.Generic;
using System.Numerics;
using Dalamud.Interface;
using Dalamud.Interface.Internal;
using Dalamud.Interface.Utility;
using Dalamud.Utility;
using ECommons.ImGuiMethods;
using ECommons.LanguageHelpers;
using ImGuiNET;
using Lumina.Excel;
using Newtonsoft.Json;
using XIVSlothComboX.Combos.PvE;
using XIVSlothComboX.Core;
using XIVSlothComboX.CustomComboNS.Functions;
using XIVSlothComboX.Data;
using XIVSlothComboX.Services;
using Action = Lumina.Excel.GeneratedSheets.Action;

namespace XIVSlothComboX.Window.Tabs
{
    internal class TimelineMainWindows : ConfigWindow
    {
        static int removeIndex = -1;
        private const string 编辑窗口 = "edit";

        private static string saveAs = "";

        internal new static void Draw()
        {
            List<CustomTimeline> customTimelineList = PluginConfiguration.CustomTimelineList;
            if (CustomComboFunctions._CustomTimeline != null)
            {
                var customTimeline = CustomComboFunctions._CustomTimeline;
                ImGui.Text($"当前使用的时间轴是:{customTimeline.Index}-{customTimeline.JobId}-{customTimeline.Name}");
            }

            ImGui.Separator();

            if (ImGui.Checkbox("是否使用爆发药", ref CustomComboFunctions.IsUseItem))
            {
                PluginConfiguration.SetCustomBoolValue(CustomComboFunctions.UseCustomTimeKeyUseItem, CustomComboFunctions.IsUseItem);
                Service.Configuration.Save();
            }
            ImGui.SameLine();
            
            //本人时间和精力有限，如果你能加入开发那更好了。。
            if (ImGui.Button("帮助"))
            {
                Util.OpenLink("https://docs.qq.com/doc/DT2hlRGl5WWhVTGNM");                
            }
            ImGui.Separator();

            if (ImGui.Button("停用"))
            {
                foreach (var customTimeline in customTimelineList)
                {
                    customTimeline.Enable = false;
                }

                CustomComboFunctions.ResetCustomTime();
                Service.Configuration.Save();
            }


            ImGui.SameLine();
            if (ImGui.Button("导入"))
            {
                var json = ImGui.GetClipboardText();
                CustomTimeline? customTimeline = JsonConvert.DeserializeObject<CustomTimeline>(json);
                if (customTimeline != null)
                {
                    int count = customTimelineList.Count;
                    customTimeline.Index = ++count;
                    customTimelineList.Add(customTimeline);
                    Service.Configuration.Save();
                }
            }

            ImGuiEx.Tooltip("从剪贴版导入");

            ImGui.SameLine();
            ImGui.SetCursorPosX(400f);

            if (ImGui.Button("清空") && ImGui.GetIO().KeyCtrl)
            {
                customTimelineList.Clear();
                Service.Configuration.Save();
            }
            ImGuiEx.Tooltip("Delete Hold CTRL+click.");
            
          
            
            ImGui.Separator();
            
          

            for (var i = 0; i < customTimelineList.Count; i++)
            {
                ImGui.PushID("List" + i);
                var customTimeline = customTimelineList[i];

                if (ImGui.Button("加载"))
                {
                    foreach (var tCustomTimeline in customTimelineList)
                    {
                        {
                            tCustomTimeline.Enable = false;
                        }
                    }

                    customTimeline.Enable = true;

                    CustomComboFunctions.LoadCustomTime(customTimeline);
                    Service.Configuration.Save();
                }

                ImGui.SameLine();
                // ImGui.SetCursorPosX(40f);
                ImGui.Text($"{customTimeline.Index}\t {customTimeline.Name}");


                #region 编辑

                ImGui.SameLine();
                ImGui.SetCursorPosX(350f);

                if (ImGuiEx.Button("编辑"))
                {
                    saveAs = customTimeline.Name;
                    ImGui.OpenPopup(编辑窗口);
                }

                if (ImGui.BeginPopup(编辑窗口))
                {
                    ImGui.InputTextWithHint("", "请输入名字", ref saveAs, 30);

                    if (ImGui.Button("保存".Loc()))
                    {
                        customTimeline.Name = saveAs;
                        Service.Configuration.Save();
                        ImGui.CloseCurrentPopup();
                    }


                    ImGui.EndPopup();
                }

                #endregion

                #region 导出

                ImGui.SameLine();

                if (ImGuiEx.Button("导出"))
                {
                    string json = JsonConvert.SerializeObject(customTimeline);
                    ImGui.SetClipboardText(json);
                }

                #endregion

                #region 删除

                ImGui.SameLine();
                // ImGui.SetCursorPosX(400f);

                if (ImGuiEx.IconButton(FontAwesomeIcon.Trash) && ImGui.GetIO().KeyCtrl)
                {
                    removeIndex = i;
                }

                ImGuiEx.Tooltip("Delete list. Hold CTRL+click.");

                #endregion

                ImGui.PopID();
            }

            if (removeIndex > -1)
            {
                customTimelineList.RemoveAt(removeIndex);
                Service.Configuration.Save();
                removeIndex = -1;
            }
        }
    }
}