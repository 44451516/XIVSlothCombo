using System.Collections.Generic;
using Newtonsoft.Json;

namespace XIVSlothComboX.WebUI.Contracts;

/// <summary> Payload of <c>/api/meta</c>; describes the plugin and the whole preset tree. </summary>
public sealed class MetaDto
{
    /// <summary> Plugin version. </summary>
    [JsonProperty("version")]
    public string Version { get; set; } = string.Empty;

    /// <summary> Port the local web server listens on. </summary>
    [JsonProperty("port")]
    public int Port { get; set; }

    /// <summary> Jobs that own at least one preset. </summary>
    [JsonProperty("jobs")]
    public List<JobInfo> Jobs { get; set; } = new();

    /// <summary> Every preset known to the plugin. </summary>
    [JsonProperty("presets")]
    public List<PresetNode> Presets { get; set; } = new();

    /// <summary> Whether the plugin was built with the debug endpoints enabled. </summary>
    [JsonProperty("debug")]
    public bool Debug { get; set; }
}

/// <summary> A job grouping presets in the frontend. </summary>
public sealed class JobInfo
{
    /// <summary> Job id. </summary>
    [JsonProperty("jobId")]
    public uint JobId { get; set; }

    /// <summary> Job display name. </summary>
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary> Job abbreviation. </summary>
    [JsonProperty("shorthand")]
    public string Shorthand { get; set; } = string.Empty;

    /// <summary> Game version the job's features were last updated for. </summary>
    [JsonProperty("version")]
    public string? Version { get; set; }

    /// <summary> Whether the job is currently disabled by the plugin. </summary>
    [JsonProperty("disabled")]
    public bool Disabled { get; set; }

    /// <summary> Game icon id of the job. </summary>
    [JsonProperty("iconId")]
    public uint IconId { get; set; }

    /// <summary> Notice shown above the feature list of the job, if any. </summary>
    [JsonProperty("notice")]
    public string? Notice { get; set; }
}
