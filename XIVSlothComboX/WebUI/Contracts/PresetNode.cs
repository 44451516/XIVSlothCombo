using System.Collections.Generic;
using Newtonsoft.Json;

namespace XIVSlothComboX.WebUI.Contracts;

/// <summary> Lightweight description of a combo preset used to build the frontend tree. </summary>
public sealed class PresetNode
{
    /// <summary> Enum member name of the preset; used as the identifier everywhere. </summary>
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary> Display name. </summary>
    [JsonProperty("fancyName")]
    public string FancyName { get; set; } = string.Empty;

    /// <summary> Feature description. </summary>
    [JsonProperty("description")]
    public string Description { get; set; } = string.Empty;

    /// <summary> Job the feature belongs to. </summary>
    [JsonProperty("jobId")]
    public uint JobId { get; set; }

    /// <summary> Job display name. </summary>
    [JsonProperty("jobName")]
    public string JobName { get; set; } = string.Empty;

    /// <summary> Job abbreviation. </summary>
    [JsonProperty("jobShorthand")]
    public string JobShorthand { get; set; } = string.Empty;

    /// <summary> Display order within the job. </summary>
    [JsonProperty("order")]
    public int Order { get; set; }

    /// <summary> Identifier of the parent preset, when the feature is a child. </summary>
    [JsonProperty("parentId")]
    public string? ParentId { get; set; }

    /// <summary> Identifiers of the child presets. </summary>
    [JsonProperty("childIds")]
    public List<string> ChildIds { get; set; } = new();

    /// <summary> Identifiers of presets conflicting with this one. </summary>
    [JsonProperty("conflictIds")]
    public List<string> ConflictIds { get; set; } = new();

    /// <summary> Whether the feature is a variant of another combo. </summary>
    [JsonProperty("isVariant")]
    public bool IsVariant { get; set; }

    /// <summary> Whether the feature belongs to the Bozja category. </summary>
    [JsonProperty("isBozja")]
    public bool IsBozja { get; set; }

    /// <summary> Whether the feature belongs to the Eureka category. </summary>
    [JsonProperty("isEureka")]
    public bool IsEureka { get; set; }

    /// <summary> Whether the feature is restricted to PvP. </summary>
    [JsonProperty("isPvP")]
    public bool IsPvP { get; set; }

    /// <summary> Whether the feature is currently enabled. </summary>
    [JsonProperty("enabled")]
    public bool Enabled { get; set; }

    /// <summary> Whether the feature is hidden because it conflicts with an enabled feature. </summary>
    [JsonProperty("hidden")]
    public bool Hidden { get; set; }
}

/// <summary> Extra information rendered on a preset card. </summary>
public sealed class PresetDetail
{
    /// <summary> Identifier of the preset. </summary>
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary> Identifiers of the parent chain, outermost first. </summary>
    [JsonProperty("parentChain")]
    public List<string> ParentChain { get; set; } = new();

    /// <summary> Identifiers of the presets this feature conflicts with. </summary>
    [JsonProperty("conflicts")]
    public List<string> Conflicts { get; set; } = new();

    /// <summary> Display names of the conflicting features. </summary>
    [JsonProperty("conflictNames")]
    public List<string> ConflictNames { get; set; } = new();

    /// <summary> Names of the skills the feature replaces. </summary>
    [JsonProperty("replaces")]
    public List<string> Replaces { get; set; } = new();

    /// <summary> Icon ids of the skills the feature replaces. </summary>
    [JsonProperty("replaceIcons")]
    public List<uint> ReplaceIcons { get; set; } = new();

    /// <summary> Blue mage hint shown above the feature list of the Blue Mage job. </summary>
    [JsonProperty("bluNotice")]
    public string? BluNotice { get; set; }

    /// <summary> Blue mage status of the feature: none, missing or ready. </summary>
    [JsonProperty("bluStatus")]
    public string BluStatus { get; set; } = "none";

    /// <summary> Missing blue mage spells, formatted for display. </summary>
    [JsonProperty("bluMissing")]
    public string? BluMissing { get; set; }

    /// <summary> Names of the parent combos this variant belongs to. </summary>
    [JsonProperty("variantParents")]
    public List<string> VariantParents { get; set; } = new();

    /// <summary> Names of the parent combos this Bozja feature belongs to. </summary>
    [JsonProperty("bozjaParents")]
    public List<string> BozjaParents { get; set; } = new();

    /// <summary> Names of the parent combos this Eureka feature belongs to. </summary>
    [JsonProperty("eurekaParents")]
    public List<string> EurekaParents { get; set; } = new();
}
