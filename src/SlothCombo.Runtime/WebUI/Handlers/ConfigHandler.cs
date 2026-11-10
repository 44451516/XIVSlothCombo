using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using SlothCombo.Core;
using SlothCombo.Runtime.Services;

using SlothCombo.Runtime.WebUI;

using SlothCombo.Runtime.WebUI.Contracts;

namespace SlothCombo.Runtime.WebUI.Handlers;

/// <summary> Serves the top level plugin configuration. </summary>
internal static class ConfigHandler
{
    internal static async Task HandleAsync(ApiRequest apiRequest, ApiResponse response, GameThreadDispatcher dispatcher)
    {
        var method = apiRequest.Method;

        if (method == "GET")
        {
            var config = await dispatcher.RunAsync(Build).ConfigureAwait(false);
            response.SetJson(200, ApiResult.Success(config));
            return;
        }

        if (method == "PATCH" || method == "POST")
        {
            var body = apiRequest.Body;
            JObject payload;
            try
            {
                payload = JObject.Parse(body);
            }
            catch (Exception ex)
            {
                response.SetJson(400, ApiResult.Failure("BAD_REQUEST", ex.Message));
                return;
            }

            var config = await dispatcher.RunAsync(() => Apply(payload)).ConfigureAwait(false);
            response.SetJson(200, ApiResult.Success(config));
            return;
        }

        response.SetJson(405, ApiResult.Failure("METHOD_NOT_ALLOWED", "不支持的方法"));
    }

    private static ConfigDto Build()
    {
        var configuration = Service.Configuration;
        return new ConfigDto
        {
            EnabledOutputLog = configuration.EnabledOutputLog,
            SetOutChat = configuration.SetOutChat,
            HideConflictedCombos = configuration.HideConflictedCombos,
            HideChildren = configuration.HideChildren,
            MeleeOffset = configuration.MeleeOffset,
            TargetHighlightColor = new ColorDto
            {
                R = configuration.TargetHighlightColor.X,
                G = configuration.TargetHighlightColor.Y,
                B = configuration.TargetHighlightColor.Z,
                A = configuration.TargetHighlightColor.W,
            },
            HideMessageOfTheDay = configuration.HideMessageOfTheDay,
            SpecialEvent = configuration.SpecialEvent,
            MovementLeeway = configuration.MovementLeeway,
            PerformanceMode = configuration.PerformanceMode,
            RecommendedSettingsViewed = configuration.RecommendedSettingsViewed,
            ActiveBluSpells = configuration.ActiveBLUSpells.ToList(),
        };
    }

    private static ConfigDto Apply(JObject payload)
    {
        var configuration = Service.Configuration;

        if (payload.TryGetValue("enabledOutputLog", out var v)) configuration.EnabledOutputLog = v.Value<bool>();
        if (payload.TryGetValue("setOutChat", out v)) configuration.SetOutChat = v.Value<bool>();
        if (payload.TryGetValue("hideConflictedCombos", out v)) configuration.HideConflictedCombos = v.Value<bool>();
        if (payload.TryGetValue("hideChildren", out v)) configuration.HideChildren = v.Value<bool>();
        if (payload.TryGetValue("meleeOffset", out v)) configuration.MeleeOffset = v.Value<double>();
        if (payload.TryGetValue("hideMessageOfTheDay", out v)) configuration.HideMessageOfTheDay = v.Value<bool>();
        if (payload.TryGetValue("specialEvent", out v)) configuration.SpecialEvent = v.Value<bool>();
        if (payload.TryGetValue("movementLeeway", out v)) configuration.MovementLeeway = v.Value<float>();
        if (payload.TryGetValue("performanceMode", out v)) configuration.PerformanceMode = v.Value<bool>();
        if (payload.TryGetValue("recommendedSettingsViewed", out v)) configuration.RecommendedSettingsViewed = v.Value<bool>();

        if (payload.TryGetValue("targetHighlightColor", out var color) && color is JObject colorObject)
        {
            var current = configuration.TargetHighlightColor;
            configuration.TargetHighlightColor = new System.Numerics.Vector4(
                colorObject.TryGetValue("r", out var r) ? r.Value<float>() : current.X,
                colorObject.TryGetValue("g", out var g) ? g.Value<float>() : current.Y,
                colorObject.TryGetValue("b", out var b) ? b.Value<float>() : current.Z,
                colorObject.TryGetValue("a", out var a) ? a.Value<float>() : current.W);
        }

        Service.Configuration.Save();
        return Build();
    }
}
