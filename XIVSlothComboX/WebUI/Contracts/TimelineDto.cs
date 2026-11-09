using System.Collections.Generic;
using Newtonsoft.Json;

namespace XIVSlothComboX.WebUI.Contracts;

/// <summary> A user defined combat timeline. </summary>
public sealed class TimelineDto
{
    /// <summary> Index inside the configuration list. </summary>
    [JsonProperty("index")]
    public int Index { get; set; }

    /// <summary> Whether the timeline is active. </summary>
    [JsonProperty("enable")]
    public bool Enable { get; set; }

    /// <summary> Job the timeline belongs to. </summary>
    [JsonProperty("jobId")]
    public uint JobId { get; set; }

    /// <summary> Timeline name. </summary>
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary> Actions of the timeline. </summary>
    [JsonProperty("actionList")]
    public List<TimelineActionDto> ActionList { get; set; } = new();
}

/// <summary> A single action inside a <see cref="TimelineDto"/>. </summary>
public sealed class TimelineActionDto
{
    /// <summary> Action id. </summary>
    [JsonProperty("actionId")]
    public uint ActionId { get; set; }

    /// <summary> Action name, filled in by the server for display. </summary>
    [JsonProperty("actionName")]
    public string? ActionName { get; set; }

    /// <summary> Icon id of the action. </summary>
    [JsonProperty("actionIcon")]
    public uint? ActionIcon { get; set; }

    /// <summary> Start of the usage window. </summary>
    [JsonProperty("useTimeStart")]
    public double UseTimeStart { get; set; }

    /// <summary> End of the usage window. </summary>
    [JsonProperty("useTimeEnd")]
    public double UseTimeEnd { get; set; }

    /// <summary> Target selector; 1 is self, 2 to 8 are party members. </summary>
    [JsonProperty("targetType")]
    public int TargetType { get; set; }

    /// <summary> Custom action category. </summary>
    [JsonProperty("customActionType")]
    public byte CustomActionType { get; set; }

    /// <summary> Custom action sub category. </summary>
    [JsonProperty("customActionTypeSub")]
    public byte CustomActionTypeSub { get; set; }

    /// <summary> Positional offset of the action. </summary>
    [JsonProperty("x")]
    public float X { get; set; }

    /// <summary> Positional offset of the action. </summary>
    [JsonProperty("y")]
    public float Y { get; set; }

    /// <summary> Positional offset of the action. </summary>
    [JsonProperty("z")]
    public float Z { get; set; }
}

/// <summary> Available values of the custom action category. </summary>
public sealed class CustomTypeDto
{
    /// <summary> Numeric value. </summary>
    [JsonProperty("value")]
    public byte Value { get; set; }

    /// <summary> Display label. </summary>
    [JsonProperty("label")]
    public string Label { get; set; } = string.Empty;
}
