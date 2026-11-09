using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using XIVSlothComboX.Core;
using XIVSlothComboX.Services;
using XIVSlothComboX.WebUI.Contracts;

namespace XIVSlothComboX.WebUI.Handlers;

/// <summary>
/// Serves the generic key/value configuration store. The frontend never needs to know
/// which dictionary a value lives in, it only knows the type of the value.
/// </summary>
internal static class ValueHandler
{
    internal static async Task HandleAsync(HttpListenerContext context, MainThreadDispatcher dispatcher)
    {
        var method = context.Request.HttpMethod;

        if (method == "GET")
        {
            var key = context.Request.QueryString["key"];
            var type = context.Request.QueryString["type"] ?? "int";
            if (string.IsNullOrEmpty(key))
            {
                await WebUIServer.WriteJsonAsync(context, 400, ApiResult.Failure("BAD_REQUEST", "缺少 key")).ConfigureAwait(false);
                return;
            }

            var value = await dispatcher.RunAsync(() => Read(key, type)).ConfigureAwait(false);
            await WebUIServer.WriteJsonAsync(context, 200, ApiResult.Success(new
            {
                key,
                type,
                value,
            })).ConfigureAwait(false);
            return;
        }

        if (method == "POST" || method == "PUT")
        {
            var body = await WebUIServer.ReadBodyAsync(context).ConfigureAwait(false);
            var request = JsonConvert.DeserializeObject<ValueRequest>(body);
            if (request == null || string.IsNullOrEmpty(request.Key))
            {
                await WebUIServer.WriteJsonAsync(context, 400, ApiResult.Failure("BAD_REQUEST", "请求内容无效")).ConfigureAwait(false);
                return;
            }

            try
            {
                var value = await dispatcher.RunAsync(() => Write(request)).ConfigureAwait(false);
                await WebUIServer.WriteJsonAsync(context, 200, ApiResult.Success(new
                {
                    key = request.Key,
                    type = request.Type,
                    value,
                })).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await WebUIServer.WriteJsonAsync(context, 400, ApiResult.Failure("BAD_VALUE", ex.Message)).ConfigureAwait(false);
            }

            return;
        }

        await WebUIServer.WriteJsonAsync(context, 405, ApiResult.Failure("METHOD_NOT_ALLOWED", "不支持的方法")).ConfigureAwait(false);
    }

    private static object? Read(string key, string type) => type switch
    {
        "int" => PluginConfiguration.PeekCustomIntValue(key),
        "float" => PluginConfiguration.PeekCustomFloatValue(key),
        "bool" => PluginConfiguration.PeekCustomBoolValue(key),
        "boolArray" => PluginConfiguration.PeekCustomBoolArrayValue(key),
        "intArray" => PluginConfiguration.PeekCustomIntArrayValue(key),
        _ => throw new ArgumentException($"未知的配置类型 {type}"),
    };

    private static object? Write(ValueRequest request)
    {
        var type = request.Type ?? "int";
        switch (type)
        {
            case "int":
            {
                var value = ToInt(request.Value);
                PluginConfiguration.SetCustomIntValue(request.Key, value);
                Service.Configuration.Save();
                return value;
            }

            case "float":
            {
                var value = ToFloat(request.Value);
                PluginConfiguration.SetCustomFloatValue(request.Key, value);
                Service.Configuration.Save();
                return value;
            }

            case "bool":
            {
                var value = ToBool(request.Value);
                PluginConfiguration.SetCustomBoolValue(request.Key, value);
                Service.Configuration.Save();
                return value;
            }

            case "boolArray":
            {
                var value = ToBoolArray(request.Value);
                PluginConfiguration.SetCustomBoolArrayValue(request.Key, value);
                Service.Configuration.Save();
                return value;
            }

            case "intArray":
            {
                var value = ToIntArray(request.Value);
                PluginConfiguration.SetCustomIntArrayValue(request.Key, value);
                Service.Configuration.Save();
                return value;
            }

            default:
                throw new ArgumentException($"未知的配置类型 {type}");
        }
    }

    private static int ToInt(object? value) => value switch
    {
        null => 0,
        int i => i,
        long l => (int)l,
        double d => (int)d,
        float f => (int)f,
        bool b => b ? 1 : 0,
        string s when int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsed) => parsed,
        _ => Convert.ToInt32(value, CultureInfo.InvariantCulture),
    };

    private static float ToFloat(object? value) => value switch
    {
        null => 0f,
        float f => f,
        double d => (float)d,
        int i => i,
        long l => l,
        string s when float.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out var parsed) => parsed,
        _ => Convert.ToSingle(value, CultureInfo.InvariantCulture),
    };

    private static bool ToBool(object? value) => value switch
    {
        null => false,
        bool b => b,
        int i => i != 0,
        long l => l != 0,
        double d => Math.Abs(d) > double.Epsilon,
        string s => bool.TryParse(s, out var parsed) && parsed,
        _ => Convert.ToBoolean(value, CultureInfo.InvariantCulture),
    };

    private static bool[] ToBoolArray(object? value)
    {
        switch (value)
        {
            case null:
                return Array.Empty<bool>();
            case bool[] array:
                return array;
            case JArray array:
                return array.Select(x => ToBool(x)).ToArray();
            case IEnumerable<object> list:
                return list.Select(ToBool).ToArray();
            default:
                throw new ArgumentException("期望布尔数组");
        }
    }

    private static int[] ToIntArray(object? value)
    {
        switch (value)
        {
            case null:
                return Array.Empty<int>();
            case int[] array:
                return array;
            case JArray array:
                return array.Select(x => ToInt(x)).ToArray();
            case IEnumerable<object> list:
                return list.Select(ToInt).ToArray();
            default:
                throw new ArgumentException("期望整数数组");
        }
    }

    private sealed class ValueRequest
    {
        public string Key { get; set; } = string.Empty;

        public string? Type { get; set; }

        public object? Value { get; set; }
    }
}
