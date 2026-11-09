using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using XIVSlothComboX.Combos;
using XIVSlothComboX.Core;
using XIVSlothComboX.Services;
using XIVSlothComboX.WebUI.Contracts;
using XIVSlothComboX.Window.Functions;

namespace XIVSlothComboX.WebUI.Handlers;

/// <summary> Serves the preset tree, the option schema and the enable/disable action. </summary>
internal static class PresetHandler
{
    internal static async Task HandleAsync(HttpListenerContext context, MainThreadDispatcher dispatcher, string[] segments)
    {
        var method = context.Request.HttpMethod;

        // /api/presets/{id}/options
        if (segments.Length >= 4 && segments[3].Equals("options", StringComparison.OrdinalIgnoreCase))
        {
            if (!PresetService.TryResolve(segments[2], out var preset))
            {
                await WebUIServer.WriteJsonAsync(context, 404, ApiResult.Failure("PRESET_NOT_FOUND", $"找不到功能 {segments[2]}")).ConfigureAwait(false);
                return;
            }

            var options = await dispatcher.RunAsync(() => BuildOptions(preset)).ConfigureAwait(false);
            await WebUIServer.WriteJsonAsync(context, 200, ApiResult.Success(new
            {
                presetId = preset.ToString(),
                options,
            })).ConfigureAwait(false);
            return;
        }

        // /api/presets/{id}/toggle
        if (segments.Length >= 4 && segments[3].Equals("toggle", StringComparison.OrdinalIgnoreCase))
        {
            if (method != "POST")
            {
                await WebUIServer.WriteJsonAsync(context, 405, ApiResult.Failure("METHOD_NOT_ALLOWED", "请使用 POST")).ConfigureAwait(false);
                return;
            }

            if (!PresetService.TryResolve(segments[2], out var preset))
            {
                await WebUIServer.WriteJsonAsync(context, 404, ApiResult.Failure("PRESET_NOT_FOUND", $"找不到功能 {segments[2]}")).ConfigureAwait(false);
                return;
            }

            var body = await WebUIServer.ReadBodyAsync(context).ConfigureAwait(false);
            var request = Newtonsoft.Json.JsonConvert.DeserializeObject<ToggleRequest>(body);
            if (request == null)
            {
                await WebUIServer.WriteJsonAsync(context, 400, ApiResult.Failure("BAD_REQUEST", "请求内容无效")).ConfigureAwait(false);
                return;
            }

            var result = await dispatcher.RunAsync(() => Toggle(preset, request.Enabled)).ConfigureAwait(false);
            await WebUIServer.WriteJsonAsync(context, 200, ApiResult.Success(result)).ConfigureAwait(false);
            return;
        }

        // /api/presets/{id}
        if (segments.Length >= 3)
        {
            if (!PresetService.TryResolve(segments[2], out var preset))
            {
                await WebUIServer.WriteJsonAsync(context, 404, ApiResult.Failure("PRESET_NOT_FOUND", $"找不到功能 {segments[2]}")).ConfigureAwait(false);
                return;
            }

            var payload = await dispatcher.RunAsync(() => new
            {
                detail = PresetService.BuildDetail(preset),
                options = BuildOptions(preset),
            }).ConfigureAwait(false);

            await WebUIServer.WriteJsonAsync(context, 200, ApiResult.Success(payload)).ConfigureAwait(false);
            return;
        }

        await WebUIServer.WriteJsonAsync(context, 404, ApiResult.Failure("NOT_FOUND", "缺少功能标识")).ConfigureAwait(false);
    }

    /// <summary> Builds the option schema of a preset by replaying its legacy option declarations. </summary>
    internal static List<OptionSchema> BuildOptions(CustomComboPreset preset)
    {
        var builder = SchemaCollector.Begin();
        try
        {
            // The legacy renderer only draws the options of an enabled feature. The schema is
            // collected with the feature treated as enabled so that a disabled feature still
            // exposes its options to the frontend.
            UserConfigItems.Draw(preset, true);
        }
        catch (Exception ex)
        {
            Service.PluginLog.Error(ex, $"Failed to build the option schema of {preset}");
        }
        finally
        {
            SchemaCollector.End(builder);
        }

        return builder.Options.ToList();
    }

    /// <summary> Enables or disables a preset, keeping parents and conflicts consistent. </summary>
    internal static ToggleResult Toggle(CustomComboPreset preset, bool enabled)
    {
        var result = new ToggleResult { Enabled = enabled };

        if (enabled)
        {
            PresetService.EnableParents(preset);
            Service.Configuration.EnabledActions.Add(preset);

            foreach (var conflict in PresetStorage.GetConflicts(preset))
            {
                if (Service.Configuration.EnabledActions.Remove(conflict))
                {
                    result.ForcedDisabled.Add(conflict.ToString());
                }
            }
        }
        else
        {
            Service.Configuration.EnabledActions.Remove(preset);
        }

        Service.Configuration.Save();
        return result;
    }

    private sealed class ToggleRequest
    {
        public bool Enabled { get; set; }
    }

    /// <summary> Result of an enable or disable request. </summary>
    internal sealed class ToggleResult
    {
        public bool Enabled { get; set; }

        public List<string> ForcedDisabled { get; } = new();
    }
}
