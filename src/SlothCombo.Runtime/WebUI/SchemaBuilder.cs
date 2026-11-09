using System;
using System.Collections.Generic;
using System.Linq;
using SlothCombo.Core;

using SlothCombo.Runtime.WebUI.Contracts;

namespace SlothCombo.Runtime.WebUI;

/// <summary>
/// Collects the configurable options of a preset into a serialisable description
/// that the Vue frontend renders instead of the legacy ImGui controls.
/// </summary>
public sealed class SchemaBuilder
{
    private readonly List<OptionSchema> _options = new();
    private readonly Dictionary<string, int> _radioGroupValues = new();
    private readonly Dictionary<string, OptionSchema> _multiOptions = new();
    private readonly Dictionary<string, OptionSchema> _priorityOptions = new();
    private int _order;

    /// <summary> Roles offered by the role based selectors. </summary>
    internal static readonly string[] RoleLabels = ["Tanks", "Healers", "Melee", "Ranged", "Casters"];

    /// <summary> Jobs offered by the job based selectors. </summary>
    internal static readonly string[] JobLabels =
    [
        "Paladin", "Warrior", "Dark Knight", "Gunbreaker",
        "White Mage", "Scholar", "Astrologian", "Sage",
        "Monk", "Dragoon", "Ninja", "Samurai",
        "Reaper", "Bard", "Machinist", "Dancer",
        "Black Mage", "Summoner", "Red Mage", "Blue Mage",
    ];

    /// <summary> PvP statuses offered by the PvP status selector. </summary>
    internal static readonly string[] PvPStatusLabels = ["眩晕", "冻结", "渐渐睡眠", "睡眠", "止步", "加重", "沉默"];

    /// <summary> Options collected so far. </summary>
    public IReadOnlyList<OptionSchema> Options => _options;

    private void Add(OptionSchema schema)
    {
        schema.Order = _order++;
        _options.Add(schema);
    }

    /// <summary> Adds an integer slider backed by <c>CustomIntValues</c>. </summary>
    public void AddSliderInt(int minValue, int maxValue, string config, string description, uint sliderIncrement)
    {
        var value = PluginConfiguration.PeekCustomIntValue(config, minValue);
        if (value < minValue) value = minValue;

        Add(new OptionSchema
        {
            Kind = "slider",
            Key = config,
            ValueType = "int",
            Min = minValue,
            Max = maxValue,
            Step = sliderIncrement,
            Label = description,
            Value = value,
        });
    }

    /// <summary> Adds a float slider backed by <c>CustomFloatValues</c>. </summary>
    public void AddSliderFloat(float minValue, float maxValue, string config, string description, float defaultMinValue)
    {
        var value = PluginConfiguration.PeekCustomFloatValue(config, defaultMinValue);
        if (value < minValue) value = minValue;

        Add(new OptionSchema
        {
            Kind = "slider",
            Key = config,
            ValueType = "float",
            Min = minValue,
            Max = maxValue,
            Step = 0.01,
            Digits = 2,
            Label = description,
            Value = value,
        });
    }

    /// <summary> Adds a float drag input backed by <c>CustomFloatValues</c>. </summary>
    public void AddDragFloat(float speed, float minValue, float maxValue, string config, string description, float defaultMinValue)
    {
        var value = PluginConfiguration.PeekCustomFloatValue(config, defaultMinValue);
        if (value < minValue) value = minValue;

        Add(new OptionSchema
        {
            Kind = "drag",
            Key = config,
            ValueType = "float",
            Min = minValue,
            Max = maxValue,
            Speed = speed,
            Digits = 2,
            Label = description,
            Value = value,
        });
    }

    /// <summary> Adds an integer drag input backed by <c>CustomIntValues</c>. </summary>
    public void AddDragInt(int minValue, int maxValue, string config, string description)
    {
        var value = PluginConfiguration.PeekCustomIntValue(config);
        if (value < minValue) value = minValue;

        Add(new OptionSchema
        {
            Kind = "drag",
            Key = config,
            ValueType = "int",
            Min = minValue,
            Max = maxValue,
            Step = 1,
            Digits = 0,
            Label = description,
            Value = value,
        });
    }

    /// <summary> Adds a float slider with a fixed number of decimals. </summary>
    public void AddRoundedSliderFloat(float minValue, float maxValue, string config, string description, int digits)
    {
        var value = PluginConfiguration.PeekCustomFloatValue(config, minValue);
        if (value < minValue) value = minValue;

        Add(new OptionSchema
        {
            Kind = "slider",
            Key = config,
            ValueType = "float",
            Min = minValue,
            Max = maxValue,
            Step = Math.Pow(10, -digits),
            Digits = digits,
            Label = description,
            Value = value,
        });
    }

    /// <summary>
    /// Adds one entry of a radio group. All entries sharing a key form one mutually
    /// exclusive group; the first entry defines the group default.
    /// </summary>
    public void AddRadioButton(string config, string checkBoxName, string checkboxDescription, int outputValue, string? layout = null)
    {
        if (!_radioGroupValues.TryGetValue(config, out var groupValue))
        {
            groupValue = PluginConfiguration.PeekCustomIntValue(config, outputValue);
            _radioGroupValues[config] = groupValue;
        }

        Add(new OptionSchema
        {
            Kind = "radio",
            Key = config,
            Group = config,
            ValueType = "int",
            OptionValue = outputValue,
            Layout = layout,
            Label = checkBoxName,
            Tooltip = string.IsNullOrEmpty(checkboxDescription) ? null : checkboxDescription,
            Value = groupValue,
        });
    }

    /// <summary> Adds a boolean switch backed by <c>CustomBoolValues</c>. </summary>
    public void AddAdditionalBoolChoice(string config, string checkBoxName, string checkboxDescription, bool isConditionalChoice)
    {
        Add(new OptionSchema
        {
            Kind = "switch",
            Key = config,
            ValueType = "bool",
            Label = checkBoxName,
            Tooltip = string.IsNullOrEmpty(checkboxDescription) ? null : checkboxDescription,
            Conditional = isConditionalChoice,
            Value = PluginConfiguration.PeekCustomBoolValue(config),
        });
    }

    /// <summary>
    /// Adds one entry of a horizontal multi choice. The helper is invoked once per
    /// choice, so entries are accumulated into a single option.
    /// </summary>
    public void AddHorizontalMultiChoice(string config, string checkBoxName, string checkboxDescription, int totalChoices, int choice)
    {
        if (!_multiOptions.TryGetValue(config, out var option))
        {
            option = new OptionSchema
            {
                Kind = "multi",
                Key = config,
                ValueType = "boolArray",
                Layout = "horizontal",
                Options = new List<OptionChoice>(),
            };
            for (var i = 0; i < totalChoices; i++)
            {
                option.Options.Add(new OptionChoice { Value = i.ToString(), Label = string.Empty });
            }
            _multiOptions[config] = option;
            Add(option);
        }

        if (choice >= 0 && choice < option.Options!.Count)
        {
            option.Options[choice].Label = checkBoxName;
            option.Options[choice].Tooltip = string.IsNullOrEmpty(checkboxDescription) ? null : checkboxDescription;
        }

        option.Value = PluginConfiguration.PeekCustomBoolArrayValue(config);
    }

    /// <summary> Adds a grid of independent checkboxes. </summary>
    public void AddGridMultiChoice(string config, byte columns, string[,] nameAndDesc)
    {
        var totalChoices = nameAndDesc.GetLength(0);
        var options = new List<OptionChoice>(totalChoices);
        for (var idx = 0; idx < totalChoices; idx++)
        {
            options.Add(new OptionChoice
            {
                Value = idx.ToString(),
                Label = nameAndDesc[idx, 0],
                Tooltip = string.IsNullOrEmpty(nameAndDesc[idx, 1]) ? null : nameAndDesc[idx, 1],
            });
        }

        Add(new OptionSchema
        {
            Kind = "multi",
            Key = config,
            ValueType = "boolArray",
            Layout = $"grid-{columns}",
            Options = options,
            Value = PluginConfiguration.PeekCustomBoolArrayValue(config),
        });
    }

    /// <summary> Adds the PvP status selector. </summary>
    public void AddPvPStatusMultiChoice(string config) =>
        AddFixedMultiChoice(config, "pvp-status", PvPStatusLabels);

    /// <summary> Adds the role based multi selector. </summary>
    public void AddRoleGridMultiChoice(string config) =>
        AddFixedMultiChoice(config, "role-grid", RoleLabels);

    /// <summary> Adds the role based single selector. </summary>
    public void AddRoleGridSingleChoice(string config) =>
        AddFixedSingleChoice(config, "role-grid", RoleLabels);

    /// <summary> Adds the job based multi selector. </summary>
    public void AddJobGridMultiChoice(string config) =>
        AddFixedMultiChoice(config, "job-grid", JobLabels);

    /// <summary> Adds the job based single selector. </summary>
    public void AddJobGridSingleChoice(string config) =>
        AddFixedSingleChoice(config, "job-grid", JobLabels);

    /// <summary>
    /// Adds a priority ordering input backed by <c>CustomIntArrayValues</c>.
    /// The helper is invoked once per entry of a shared priority list, so the entries
    /// are accumulated into a single option; only the entries owned by the current
    /// feature carry a label, the remaining slots stay anonymous.
    /// </summary>
    public void AddPriorityInput(string configKey, int maxValues, int currentItem, string customLabel)
    {
        if (!_priorityOptions.TryGetValue(configKey, out var option))
        {
            var effective = NormalizePriority(PluginConfiguration.PeekCustomIntArrayValue(configKey), maxValues);

            option = new OptionSchema
            {
                Kind = "priority",
                Key = configKey,
                ValueType = "intArray",
                MaxItems = maxValues,
                Label = string.IsNullOrEmpty(customLabel) ? "优先级" : customLabel,
                Tooltip = "Smaller Number = Higher Priority",
                Options = new List<OptionChoice>(maxValues),
                Value = effective,
            };

            for (var i = 0; i < maxValues; i++)
            {
                option.Options.Add(new OptionChoice { Value = effective[i].ToString(), Label = string.Empty });
            }

            _priorityOptions[configKey] = option;
            Add(option);
        }

        if (currentItem >= 0 && currentItem < option.Options!.Count)
        {
            option.Options[currentItem].Label = string.IsNullOrEmpty(customLabel)
                ? $"优先级 {currentItem + 1}"
                : customLabel;
        }
    }

    /// <summary>
    /// Mirrors the legacy normalisation of a priority list: a list of the wrong length or
    /// one that still holds zeroes starts out as <c>1..maxValues</c>. The normalised list is
    /// only reported, it is never written back.
    /// </summary>
    private static int[] NormalizePriority(int[] stored, int maxValues)
    {
        var result = new int[maxValues];
        for (var i = 0; i < maxValues; i++)
        {
            result[i] = i < stored.Length && stored[i] != 0 ? stored[i] : i + 1;
        }

        return result;
    }

    private void AddFixedMultiChoice(string config, string layout, string[] labels)
    {
        var options = labels
            .Select((label, index) => new OptionChoice { Value = index.ToString(), Label = label })
            .ToList();

        Add(new OptionSchema
        {
            Kind = "multi",
            Key = config,
            ValueType = "boolArray",
            Layout = layout,
            Options = options,
            Value = PluginConfiguration.PeekCustomBoolArrayValue(config),
        });
    }

    private void AddFixedSingleChoice(string config, string layout, string[] labels)
    {
        var options = labels
            .Select((label, index) => new OptionChoice { Value = index.ToString(), Label = label })
            .ToList();

        Add(new OptionSchema
        {
            Kind = "single",
            Key = config,
            ValueType = "int",
            Layout = layout,
            Options = options,
            Value = PluginConfiguration.PeekCustomIntValue(config),
        });
    }
}

/// <summary> Holds the schema builder that is currently collecting options. </summary>
public static class SchemaCollector
{
    /// <summary> The active builder, or <c>null</c> when the legacy ImGui path is used. </summary>
    public static SchemaBuilder? Current { get; private set; }

    /// <summary> Whether options are currently being collected. </summary>
    public static bool Active => Current != null;

    /// <summary> Starts collecting options. </summary>
    public static SchemaBuilder Begin()
    {
        Current = new SchemaBuilder();
        return Current;
    }

    /// <summary> Stops collecting options. </summary>
    public static void End(SchemaBuilder builder)
    {
        if (ReferenceEquals(Current, builder)) Current = null;
    }
}
