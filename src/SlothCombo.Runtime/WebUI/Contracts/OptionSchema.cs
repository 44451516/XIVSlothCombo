using System.Collections.Generic;
using Newtonsoft.Json;

namespace SlothCombo.Runtime.WebUI.Contracts;

/// <summary>
/// Describes a single configurable option belonging to a preset.
/// The frontend renders one control per entry; the shape of <see cref="Value"/>
/// depends on <see cref="ValueType"/>.
/// </summary>
public sealed class OptionSchema
{
    /// <summary> Control kind: slider, drag, radio, switch, multi, single, priority. </summary>
    [JsonProperty("kind")]
    public string Kind { get; set; } = string.Empty;

    /// <summary> Configuration key backing this option. </summary>
    [JsonProperty("key")]
    public string Key { get; set; } = string.Empty;

    /// <summary> Display label. </summary>
    [JsonProperty("label")]
    public string Label { get; set; } = string.Empty;

    /// <summary> Optional hover description. </summary>
    [JsonProperty("tooltip")]
    public string? Tooltip { get; set; }

    /// <summary> Storage kind: int, float, bool, boolArray or intArray. </summary>
    [JsonProperty("valueType")]
    public string? ValueType { get; set; }

    /// <summary> Lower bound for numeric options. </summary>
    [JsonProperty("min")]
    public double? Min { get; set; }

    /// <summary> Upper bound for numeric options. </summary>
    [JsonProperty("max")]
    public double? Max { get; set; }

    /// <summary> Step size for numeric options. </summary>
    [JsonProperty("step")]
    public double? Step { get; set; }

    /// <summary> Number of decimals for numeric options. </summary>
    [JsonProperty("digits")]
    public int? Digits { get; set; }

    /// <summary> Drag speed for drag options. </summary>
    [JsonProperty("speed")]
    public double? Speed { get; set; }

    /// <summary> Current value; its shape depends on <see cref="ValueType"/>. </summary>
    [JsonProperty("value")]
    public object? Value { get; set; }

    /// <summary> Selectable choices for multi and single options. </summary>
    [JsonProperty("options")]
    public List<OptionChoice>? Options { get; set; }

    /// <summary> Layout hint for multi and single options. </summary>
    [JsonProperty("layout")]
    public string? Layout { get; set; }

    /// <summary> Radio group identifier; radios sharing a group are mutually exclusive. </summary>
    [JsonProperty("group")]
    public string? Group { get; set; }

    /// <summary> The numeric value a radio option selects. </summary>
    [JsonProperty("optionValue")]
    public int? OptionValue { get; set; }

    /// <summary> True when the option is only shown while its parent switch is enabled. </summary>
    [JsonProperty("conditional")]
    public bool Conditional { get; set; }

    /// <summary> Maximum number of entries for priority options. </summary>
    [JsonProperty("maxItems")]
    public int? MaxItems { get; set; }

    /// <summary> Original UI ordering hint. </summary>
    [JsonProperty("order")]
    public int Order { get; set; }
}

/// <summary> A single selectable choice of a multi or single option. </summary>
public sealed class OptionChoice
{
    /// <summary> Stored value. </summary>
    [JsonProperty("value")]
    public string Value { get; set; } = string.Empty;

    /// <summary> Display label. </summary>
    [JsonProperty("label")]
    public string Label { get; set; } = string.Empty;

    /// <summary> Optional hover description. </summary>
    [JsonProperty("tooltip")]
    public string? Tooltip { get; set; }

    /// <summary> Optional game icon id rendered next to the label. </summary>
    [JsonProperty("iconId")]
    public uint? IconId { get; set; }
}
