using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SlothCombo.Core;
using SlothCombo.CustomComboNS.Functions;
using SlothCombo.Data;
using SlothCombo.Runtime.Services;
using ItemRow = SlothCombo.Abstractions.GameData.ItemRow;
using ActionRow = SlothCombo.Abstractions.GameData.ActionRow;

using SlothCombo.Runtime.WebUI;

using SlothCombo.Runtime.WebUI.Contracts;

namespace SlothCombo.Runtime.WebUI.Handlers;

/// <summary>
/// Serves the custom timeline list and the working copy the timeline editor edits.
/// </summary>
internal static class TimelineHandler
{
    internal static async Task HandleAsync(ApiRequest apiRequest, ApiResponse response, GameThreadDispatcher dispatcher)
    {
        var method = apiRequest.Method;

        if (method == "GET")
        {
            var payload = await dispatcher.RunAsync(Build).ConfigureAwait(false);
            response.SetJson(200, ApiResult.Success(payload));
            return;
        }

        if (method == "POST")
        {
            var body = apiRequest.Body;
            JObject request;
            try
            {
                request = JObject.Parse(body);
            }
            catch (Exception ex)
            {
                response.SetJson(400, ApiResult.Failure("BAD_REQUEST", ex.Message));
                return;
            }

            var action = request["action"]?.Value<string>();
            if (string.IsNullOrEmpty(action))
            {
                response.SetJson(400, ApiResult.Failure("BAD_REQUEST", "缺少 action"));
                return;
            }

            try
            {
                var result = await dispatcher.RunAsync(() => Execute(action, request)).ConfigureAwait(false);
                response.SetJson(200, ApiResult.Success(result));
            }
            catch (Exception ex)
            {
                Service.PluginLog.Error(ex, $"Timeline action {action} failed");
                response.SetJson(400, ApiResult.Failure("ACTION_FAILED", ex.Message));
            }

            return;
        }

        response.SetJson(405, ApiResult.Failure("METHOD_NOT_ALLOWED", "不支持的方法"));
    }

    private static object Build()
    {
        var list = PluginConfiguration.CustomTimelineList;
        return new
        {
            timelines = list.Select(ToDto).ToList(),
            working = ActionWatching.TimelineList.Select(ToDto).ToList(),
            current = CustomComboFunctions._CustomTimeline == null ? null : ToDto(CustomComboFunctions._CustomTimeline),
            useItem = CustomComboFunctions.IsUseItem,
            customTypes = new List<CustomTypeDto>
            {
                new() { Value = Data.CustomType.序列, Label = "序列" },
                new() { Value = Data.CustomType.时间, Label = "时间" },
                new() { Value = Data.CustomType.药品, Label = "药品" },
                new() { Value = Data.CustomType.地面, Label = "地面" },
                new() { Value = Data.CustomType.强制插入, Label = "强制插入" },
            },
            customTypeSubs = new List<CustomTypeDto>
            {
                new() { Value = Data.CustomTypeSub.默认, Label = "默认" },
                new() { Value = Data.CustomTypeSub.长期替换, Label = "长期替换" },
                new() { Value = Data.CustomTypeSub.能力技窗口, Label = "能力技窗口" },
                new() { Value = Data.CustomTypeSub.能力技窗口后半, Label = "能力技窗口后半" },
                new() { Value = Data.CustomTypeSub.强制, Label = "强制" },
            },
        };
    }

    private static object Execute(string action, JObject request)
    {
        var list = PluginConfiguration.CustomTimelineList;

        switch (action)
        {
            case "load":
            {
                // Load the timeline that is currently running into the working copy.
                ActionWatching.TimelineList.Clear();
                foreach (var customAction in CustomComboFunctions.整个轴)
                {
                    ActionWatching.TimelineList.Add(customAction);
                }

                return new { ok = true };
            }

            case "save":
            {
                var customTimeline = CustomComboFunctions._CustomTimeline;
                if (customTimeline == null) throw new InvalidOperationException("当前没有加载中的时间轴。");

                customTimeline.ActionList = new List<CustomAction>();
                customTimeline.ActionList.Clear();
                foreach (var customAction in ActionWatching.TimelineList)
                {
                    customTimeline.ActionList.Add(customAction);
                }

                CustomComboFunctions.LoadCustomTime(customTimeline, false);
                Service.Configuration.Save();
                return new { ok = true, timeline = ToDto(customTimeline) };
            }

            case "saveAs":
            {
                var name = request["name"]?.Value<string>();
                if (string.IsNullOrWhiteSpace(name)) throw new InvalidOperationException("请输入名字。");

                var customTimeline = new CustomTimeline();
                if (list.Count > 0)
                {
                    customTimeline.Index = list.Last().Index + 1;
                }
                else
                {
                    customTimeline.Index = 1;
                }

                customTimeline.JobId = Service.ObjectTable.LocalPlayer?.ClassJob.RowId ?? 0;
                customTimeline.Name = name;
                customTimeline.Enable = false;
                customTimeline.ActionList = new List<CustomAction>();
                foreach (var customAction in ActionWatching.TimelineList)
                {
                    customTimeline.ActionList.Add(customAction);
                }

                list.Add(customTimeline);
                Service.Configuration.Save();
                return new { ok = true, timeline = ToDto(customTimeline) };
            }

            case "disableAll":
            {
                foreach (var customTimeline in list) customTimeline.Enable = false;
                CustomComboFunctions.ResetCustomTime();
                Service.Configuration.Save();
                return new { ok = true };
            }

            case "clearAll":
            {
                list.Clear();
                Service.Configuration.Save();
                return new { ok = true };
            }

            case "clearWorking":
            {
                ActionWatching.TimelineList.Clear();
                return new { ok = true };
            }

            case "loadTimeline":
            {
                var index = request["index"]?.Value<int>() ?? -1;
                if (index < 0 || index >= list.Count) throw new InvalidOperationException("时间轴不存在。");

                var customTimeline = list[index];
                foreach (var other in list) other.Enable = false;
                customTimeline.Enable = true;
                CustomComboFunctions.LoadCustomTime(customTimeline);
                Service.Configuration.Save();
                return new { ok = true, timeline = ToDto(customTimeline) };
            }

            case "rename":
            {
                var index = request["index"]?.Value<int>() ?? -1;
                var name = request["name"]?.Value<string>();
                if (index < 0 || index >= list.Count) throw new InvalidOperationException("时间轴不存在。");
                if (string.IsNullOrWhiteSpace(name)) throw new InvalidOperationException("请输入名字。");

                list[index].Name = name;
                Service.Configuration.Save();
                return new { ok = true, timeline = ToDto(list[index]) };
            }

            case "delete":
            {
                var index = request["index"]?.Value<int>() ?? -1;
                if (index < 0 || index >= list.Count) throw new InvalidOperationException("时间轴不存在。");

                list.RemoveAt(index);
                Service.Configuration.Save();
                return new { ok = true };
            }

            case "import":
            {
                var json = request["json"]?.Value<string>();
                if (string.IsNullOrWhiteSpace(json)) throw new InvalidOperationException("剪贴板内容为空。");

                var customTimeline = JsonConvert.DeserializeObject<CustomTimeline>(json);
                if (customTimeline == null) throw new InvalidOperationException("无法解析该内容。");

                customTimeline.Index = list.Count + 1;
                list.Add(customTimeline);
                Service.Configuration.Save();
                return new { ok = true, timeline = ToDto(customTimeline) };
            }

            case "export":
            {
                var index = request["index"]?.Value<int>() ?? -1;
                if (index < 0 || index >= list.Count) throw new InvalidOperationException("时间轴不存在。");

                return new { ok = true, json = JsonConvert.SerializeObject(list[index]) };
            }

            case "exportWorking":
            {
                return new { ok = true, json = JsonConvert.SerializeObject(ActionWatching.TimelineList) };
            }

            case "setUseItem":
            {
                var value = request["value"]?.Value<bool>() ?? false;
                CustomComboFunctions.IsUseItem = value;
                PluginConfiguration.SetCustomBoolValue(CustomComboFunctions.UseCustomTimeKeyUseItem, value);
                Service.Configuration.Save();
                return new { ok = true, useItem = value };
            }

            case "setWorking":
            {
                var actions = request["actions"] as JArray;
                if (actions == null) throw new InvalidOperationException("缺少动作列表。");

                ActionWatching.TimelineList.Clear();
                foreach (var entry in actions)
                {
                    var dto = entry.ToObject<TimelineActionDto>();
                    if (dto is null) continue;

                    ActionWatching.TimelineList.Add(FromDto(dto));
                }

                return new { ok = true };
            }

            default:
                throw new InvalidOperationException($"未知的操作 {action}");
        }
    }

    private static TimelineDto ToDto(CustomTimeline timeline) => new()
    {
        Index = timeline.Index,
        Enable = timeline.Enable,
        JobId = timeline.JobId,
        Name = timeline.Name,
        ActionList = timeline.ActionList.Select(ToDto).ToList(),
    };

    private static TimelineActionDto ToDto(CustomAction action)
    {
        var dto = new TimelineActionDto
        {
            ActionId = action.ActionId,
            UseTimeStart = action.UseTimeStart,
            UseTimeEnd = action.UseTimeEnd,
            TargetType = action.TargetType,
            CustomActionType = action.CustomActionType,
            CustomActionTypeSub = action.CustomActionTypeSub,
            X = action.Vector3.X,
            Y = action.Vector3.Y,
            Z = action.Vector3.Z,
        };

        try
        {
            if (action.CustomActionType == Data.CustomType.药品)
            {
                dto.ActionName = ActionWatching.GetItemName(action.ActionId);

                var items = Service.DataManager.GetTable<ItemRow>();
                if (items.TryGetRow(action.ActionId, out var item)) dto.ActionIcon = item.Icon;
            }
            else
            {
                dto.ActionName = ActionWatching.GetActionName(action.ActionId);

                var actions = Service.DataManager.GetTable<ActionRow>();
                if (actions.TryGetRow(action.ActionId, out var row)) dto.ActionIcon = row.Icon;
            }
        }
        catch
        {
            dto.ActionName = null;
        }

        return dto;
    }

    private static CustomAction FromDto(TimelineActionDto dto) => new()
    {
        ActionId = dto.ActionId,
        UseTimeStart = dto.UseTimeStart,
        UseTimeEnd = dto.UseTimeEnd,
        TargetType = dto.TargetType,
        CustomActionType = dto.CustomActionType,
        CustomActionTypeSub = dto.CustomActionTypeSub,
        Vector3 = new System.Numerics.Vector3(dto.X, dto.Y, dto.Z),
    };
}
