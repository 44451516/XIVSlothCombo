using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Dalamud.Interface;
using Dalamud.Interface.Internal;
using Dalamud.Interface.Textures.TextureWraps;
using Dalamud.Interface.Utility;
using Dalamud.Utility;
using ECommons;
using ECommons.ImGuiMethods;
using ECommons.LanguageHelpers;
using Dalamud.Bindings.ImGui;
using Lumina.Excel;
using Lumina.Excel.Sheets;
using Newtonsoft.Json;
using XIVSlothComboX.Combos.PvE;
using XIVSlothComboX.Core;
using XIVSlothComboX.CustomComboNS.Functions;
using XIVSlothComboX.Data;
using XIVSlothComboX.Services;
using Action = Lumina.Excel.Sheets.Action;

namespace XIVSlothComboX.Window.Tabs
{
    internal class TimelineEditWindows : ConfigWindow
    {
        // static Service.DataManager.GetExcelSheet<Lumina.Excel.Sheets.Action>()


        private const string 前插窗口 = "add";
        private const string 后插窗口 = "last";
        private const string 编辑窗口 = "edit";
        private const string 另存为窗口 = "saveAsWindows";


        private static string saveAs = "";
        static int NewActionIdText = 0;
        static float UseTimeStart = 0f;
        static float UseTimeEnd = 0f;
        static int TargetType = 0;
        static int CustomActionType = 1;
        static int CustomActionTypeSub = 0;

        static int removeIndex = -1;

        static int addIndex = -1;

        static int editIndex = -1;

        static float UseActionX = 0f;
        static float UseActionY = 0f;
        static float UseActionZ = 0f;


        // static int adfgdIndex = -1;
        static CustomAction _AddCustomAction = new CustomAction();


        internal new static void Draw()
        {
            List<CustomTimeline> customTimelineList = PluginConfiguration.CustomTimelineList;
            ImGui.Text("显示战斗的循环...");


            if (ImGui.Button("复制"))
            {
                string json = JsonConvert.SerializeObject(ActionWatching.TimelineList);

                ImGui.SetClipboardText(json);
            }


            ImGui.SameLine();
            if (ImGui.Button("读取"))
            {
                ActionWatching.TimelineList.Clear();
                foreach (var customTimeline in CustomComboFunctions.整个轴)
                {
                    ActionWatching.TimelineList.Add(customTimeline);
                }
            }

            ImGui.SameLine();
            if (ImGui.Button("保存"))
            {
                var customTimeline = CustomComboFunctions._CustomTimeline;
                if (customTimelineList.Count > 0)
                {
                    customTimeline.ActionList = new List<CustomAction>();
                    customTimeline.ActionList.Clear();
                    foreach (var customAction in ActionWatching.TimelineList)
                    {
                        customTimeline.ActionList.Add(customAction);
                    }
                    CustomComboFunctions.LoadCustomTime(customTimeline, false);
                    Service.Configuration.Save();
                }
            }

            ImGuiEx.Tooltip("读取当前使用的时间轴");

            ImGui.SameLine();


            if (ImGui.Button("另存为"))
            {
                saveAs = "";
                ImGui.OpenPopup(另存为窗口);
            }

            if (ImGui.BeginPopup(另存为窗口))
            {
                ImGui.InputTextWithHint("", "请输入名字", ref saveAs, 100);

                if (ImGui.Button("保存".Loc()))
                {


                    var customTimeline = new CustomTimeline();
                    if (customTimelineList.Count == 0)
                    {
                        customTimeline.Index = 1;
                    }
                    else
                    {
                        int newIndex = customTimelineList.Last().Index;
                        customTimeline.Index = ++newIndex;
                    }

                    customTimeline.JobId = Service.ObjectTable.LocalPlayer.ClassJob.RowId;
                    customTimeline.Name = saveAs;
                    customTimeline.Enable = false;
                    customTimeline.ActionList = new List<CustomAction>();

                    foreach (var customAction in ActionWatching.TimelineList)
                    {
                        customTimeline.ActionList.Add(customAction);
                    }

                    customTimelineList.Add(customTimeline);

                    Service.Configuration.Save();
                    ImGui.CloseCurrentPopup();
                }

                ImGui.EndPopup();
            }

            ImGui.SameLine();
            ImGui.SetCursorPosX(300);

            if (ImGui.Button("清空") && ImGui.GetIO().KeyCtrl)
            {
                ActionWatching.TimelineList.Clear();
            }

            ImGuiEx.Tooltip("Delete Hold CTRL+click.");


            var ActionSheet = Service.DataManager.GetExcelSheet<Action>();


            for (var i = 0; i < ActionWatching.TimelineList.Count; i++)
            {
                ImGui.PushID("List" + i);

                var customAction = ActionWatching.TimelineList[i];

                switch (customAction.CustomActionType)
                {
                    case CustomType.序列:
                    case CustomType.时间:
                    case CustomType.地面:
                    {
                        Action? action = ActionSheet.GetRow(customAction.ActionId);
                        if (action != null)
                        {
                            // Service.ChatGui.PrintError(
                            //     $"UseActionLocationDetour{customAction.ActionId}");
                        }


                        IDalamudTextureWrap? textureWrap = Service.IconManager.GetActionIcon(action!.Value);


                        switch (action.Value.ActionCategory.Value.RowId)
                        {
                            //Spell GCD
                            case 2:
                                if (textureWrap != null)
                                {
                                    ImGui.Image(textureWrap.Handle, Vector2.One * 24 * ImGuiHelpers.GlobalScale);
                                    ImGui.SameLine();
                                    ImGui.Text
                                    (
                                        $"{ActionWatching.GetActionName(customAction.ActionId)}[{customAction.ActionId}][{customAction.UseTimeStart}]"
                                    );
                                }

                                break;
                            //Weaponskill GCD
                            case 3:
                                if (textureWrap != null)
                                {
                                    ImGui.Image(textureWrap.Handle, Vector2.One * 24 * ImGuiHelpers.GlobalScale);
                                    ImGui.SameLine();
                                    ImGui.Text
                                    (
                                        $"{ActionWatching.GetActionName(customAction.ActionId)}[{customAction.ActionId}][{customAction.UseTimeStart}]"
                                    );
                                }

                                break;
                            //Ability 能力技
                            case 4:
                                if (textureWrap != null)
                                {
                                    ImGui.Text($"     ");
                                    ImGui.SameLine();

                                    ImGui.Image(textureWrap.Handle, Vector2.One * 16 * ImGuiHelpers.GlobalScale);
                                    ImGui.SameLine();
                                    ImGui.Text
                                    (
                                        $"{ActionWatching.GetActionName(customAction.ActionId)}[{customAction.ActionId}][{customAction.UseTimeStart}]"
                                    );
                                }

                                break;
                        }
                        break;
                    }

                    case CustomType.药品:
                    {
                        var itemsSheet = Service.DataManager.GetExcelSheet<Item>();
                        Item? item = itemsSheet.GetRow(customAction.ActionId);
                        IDalamudTextureWrap? textureWrap = Service.IconManager.GetIconTexture(item.Value.Icon);
                        if (textureWrap != null)
                        {


                            ImGui.Text($"     ");
                            ImGui.SameLine();

                            ImGui.Image(textureWrap.Handle, Vector2.One * 16 * ImGuiHelpers.GlobalScale);
                            ImGui.SameLine();
                            ImGui.Text
                            (
                                $"{ActionWatching.GetItemName(customAction.ActionId)}[{customAction.ActionId}][{customAction.UseTimeStart}]"
                            );

                        }
                        break;
                    }
                }


                ImGui.SameLine();

                #region 编辑

                ImGui.SetCursorPosX(300);
                if (ImGuiEx.Button("编辑".Loc()))
                {
                    NewActionIdText = (int)customAction.ActionId;
                    UseTimeStart = (float)customAction.UseTimeStart;
                    UseTimeEnd = (float)customAction.UseTimeEnd;
                    TargetType = customAction.TargetType;
                    CustomActionType = customAction.CustomActionType;
                    UseActionX = customAction.Vector3.X;
                    UseActionY = customAction.Vector3.Y;
                    UseActionZ = customAction.Vector3.Z;
                    CustomActionTypeSub = customAction.CustomActionTypeSub;

                    addIndex = -1;
                    editIndex = i;

                    ImGui.OpenPopup(编辑窗口);
                }


                if (ImGui.BeginPopup(编辑窗口))
                {
                    addIndex = editPopUI(ref customAction, i, 编辑窗口);
                    ImGui.EndPopup();
                }

                #endregion

                ImGui.SameLine();

                #region 前插

                if (ImGuiEx.Button("前插".Loc()))
                {
                    NewActionIdText = 0;
                    UseTimeStart = 0;
                    UseTimeEnd = 0;
                    TargetType = 0;
                    CustomActionType = CustomType.序列;
                    addIndex = -1;
                    editIndex = -1;
                    _AddCustomAction = new CustomAction();
                    ImGui.OpenPopup(前插窗口);
                }

                if (ImGui.BeginPopup(前插窗口))
                {
                    addIndex = editPopUI(ref _AddCustomAction, i, 前插窗口);
                    ImGui.EndPopup();
                }

                #endregion


                ImGui.SameLine();

                #region 删除

                if (ImGuiEx.IconButton(FontAwesomeIcon.Trash) && ImGui.GetIO().KeyCtrl)
                {
                    removeIndex = i;
                    // Service.ChatGui.PrintError($"删除{removeIndex}");
                }

                ImGuiEx.Tooltip("Delete list. Hold CTRL+click.");

                #endregion

                #region 后插

                ImGui.SameLine();
                if (ImGuiEx.Button("后插".Loc()))
                {
                    NewActionIdText = 0;
                    UseTimeStart = 0;
                    UseTimeEnd = 0;
                    TargetType = 0;
                    CustomActionType = CustomType.序列;
                    addIndex = -1;
                    editIndex = -1;
                    _AddCustomAction = new CustomAction();
                    ImGui.OpenPopup(后插窗口);
                }

                if (ImGui.BeginPopup(后插窗口))
                {
                    addIndex = editPopUI(ref _AddCustomAction, i, 后插窗口);

                    ImGui.EndPopup();
                }

                #endregion

                ImGui.PopID();
            }

            if (removeIndex > -1)
            {
                // Service.ChatGui.PrintError($"删除{removeIndex}");
                if (removeIndex < ActionWatching.TimelineList.Count)
                {
                    ActionWatching.TimelineList.RemoveAt(removeIndex);
                    removeIndex = -1;
                }


            }

            if (addIndex > -1)
            {
                if (addIndex >= ActionWatching.TimelineList.Count)
                {
                    ActionWatching.TimelineList.Add(_AddCustomAction);
                }
                else
                {
                    ActionWatching.TimelineList.Insert(addIndex, _AddCustomAction);
                }

                addIndex = -1;
            }
        }

        private static int editPopUI(ref CustomAction customAction, int i, string 窗口类型)
        {
            ExcelSheet<Action>? actionSheet = Service.DataManager.GetExcelSheet<Action>();

            var addIndex = -1;

            ImGui.DragInt("技能Id", ref NewActionIdText, 0, 100000);
            ImGui.DragFloat("释放开始时间", ref UseTimeStart, 0.1f, 0, 30 * 60);
            ImGui.DragFloat("释放最晚时间", ref UseTimeEnd, 0.1f, 0, 30 * 60);
            ImGui.SliderInt("目标类别", ref TargetType, 0, 20);
            ImGui.SliderInt("技能类别", ref CustomActionType, 1, 5);
            ImGui.SliderInt("替换类别", ref CustomActionTypeSub, 0, 3);
            ImGui.DragFloat("X", ref UseActionX, 0.1f, -99999, 9999);
            ImGui.DragFloat("Y", ref UseActionY, 0.1f, -99999, 9999);
            ImGui.DragFloat("Z", ref UseActionZ, 0.1f, -99999, 9999);


            if (ImGui.Button("保存".Loc()))
            {
                // if (actionSheet.GetRow(customAction.ActionId) != null)
                {
                    switch (窗口类型)
                    {
                        case 前插窗口:
                        {
                            addIndex = i;
                            // Service.ChatGui.PrintError($"添加{addIndex}-{NewActionIdText}");
                            break;
                        }

                        case 后插窗口:
                        {
                            addIndex = i + 1;
                            // Service.ChatGui.PrintError($"添加{addIndex}-{NewActionIdText}");
                            break;
                        }
                    }

                    customAction.ActionId = (uint)NewActionIdText;
                    customAction.UseTimeStart = UseTimeStart;
                    customAction.UseTimeEnd = UseTimeEnd;
                    customAction.TargetType = TargetType;
                    customAction.CustomActionType = (byte)CustomActionType;
                    customAction.Vector3 = new Vector3(UseActionX, UseActionY, UseActionZ);

                    ImGui.CloseCurrentPopup();
                }
            }

            ImGui.SameLine();
            if (ImGui.Button("关闭".Loc()))
            {
                ImGui.CloseCurrentPopup();
            }

            return addIndex;
        }
    }
}