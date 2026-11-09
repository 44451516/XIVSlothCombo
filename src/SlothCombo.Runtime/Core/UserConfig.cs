using System;
using System.Collections.Generic;
using System.Numerics;
using SlothCombo.CustomComboNS.Functions;
using SlothCombo.Data;
using SlothCombo.Runtime.WebUI;

namespace SlothCombo.Core;

/// <summary>
/// The configurable options of the features, without any drawing of its own.
/// </summary>
/// <remarks>
/// <para>
/// The options of a feature are declared in one place. The web frontend
/// renders them, so the standalone program only has to describe them.
/// </para>
/// <para>
/// The description is collected while the options of a feature are replayed,
/// which is what the collector of the schema does. Every method here does
/// nothing outside of that replay.
/// </para>
/// </remarks>
public static class UserConfig
{
    /// <summary>The colour the descriptions are shown in.</summary>
    public static readonly Vector4 DescriptionColor = new(1f, 0.85f, 0f, 1f);

    /// <summary>Moves the following options inwards.</summary>
    internal static void Indent()
    {
        // Nothing is drawn, so there is nothing to indent.
    }

    /// <summary>Moves the following options outwards.</summary>
    internal static void Unindent()
    {
        // Nothing is drawn, so there is nothing to unindent.
    }

    /// <summary>Leaves a gap between the options.</summary>
    internal static void Spacing()
    {
        // Nothing is drawn, so there is nothing to space.
    }

    /// <summary>Declares a slider of whole numbers.</summary>
    public static void DrawSliderInt(
        int minValue,
        int maxValue,
        string config,
        string sliderDescription,
        float itemWidth = 150,
        uint sliderIncrement = SliderIncrements.Ones,
        bool hasAdditionalChoice = false,
        string additonalChoiceCondition = "")
    {
        if (SchemaCollector.Current is { } schema)
        {
            schema.AddSliderInt(minValue, maxValue, config, sliderDescription, sliderIncrement);
        }
    }

    /// <summary>Declares a slider of fractions.</summary>
    public static void DrawSliderFloat(
        float minValue,
        float maxValue,
        string config,
        string sliderDescription,
        float itemWidth = 150,
        float defaultMinValue = 0f,
        bool hasAdditionalChoice = false,
        string additonalChoiceCondition = "")
    {
        if (SchemaCollector.Current is { } schema)
        {
            schema.AddSliderFloat(minValue, maxValue, config, sliderDescription, defaultMinValue);
        }
    }

    /// <summary>Declares a drag input of fractions.</summary>
    public static void DrawDragFloat(
        float speed,
        float minValue,
        float maxValue,
        string config,
        string sliderDescription,
        float itemWidth = 150,
        float defaultMinValue = 0f,
        bool hasAdditionalChoice = false,
        string additonalChoiceCondition = "")
    {
        if (SchemaCollector.Current is { } schema)
        {
            schema.AddDragFloat(speed, minValue, maxValue, config, sliderDescription, defaultMinValue);
        }
    }

    /// <summary>Declares a drag input of whole numbers.</summary>
    public static void DrawDragInt(
        int minValue,
        int maxValue,
        string config,
        string sliderDescription,
        float itemWidth = 150,
        bool hasAdditionalChoice = false,
        string additonalChoiceCondition = "")
    {
        if (SchemaCollector.Current is { } schema)
        {
            schema.AddDragInt(minValue, maxValue, config, sliderDescription);
        }
    }

    /// <summary>Declares a slider of fractions with a rounded step.</summary>
    public static void DrawRoundedSliderFloat(
        float minValue,
        float maxValue,
        string config,
        string sliderDescription,
        float itemWidth = 150,
        float defaultMinValue = 0f,
        bool hasAdditionalChoice = false,
        string additonalChoiceCondition = "",
        int digits = 1)
    {
        if (SchemaCollector.Current is { } schema)
        {
            schema.AddRoundedSliderFloat(minValue, maxValue, config, sliderDescription, digits);
        }
    }

    /// <summary>Declares one choice of a group of choices.</summary>
    public static void DrawRadioButton(
        string config,
        string checkBoxName,
        string checkboxDescription,
        int outputValue,
        float itemWidth = 150,
        Vector4 descriptionColor = default)
    {
        if (SchemaCollector.Current is { } schema)
        {
            schema.AddRadioButton(config, checkBoxName, checkboxDescription, outputValue);
        }
    }

    /// <summary>Declares one choice of a group of choices that are shown in a row.</summary>
    public static void DrawHorizontalRadioButton(
        string config,
        string checkBoxName,
        string checkboxDescription,
        int outputValue,
        float itemWidth = 150,
        Vector4 descriptionColor = default)
    {
        if (SchemaCollector.Current is { } schema)
        {
            schema.AddRadioButton(config, checkBoxName, checkboxDescription, outputValue, "horizontal");
        }
    }

    /// <summary>Declares a choice that shows more options when it is turned on.</summary>
    public static void DrawAdditionalBoolChoice(
        string config,
        string checkBoxName,
        string checkboxDescription,
        float itemWidth = 150,
        bool isConditionalChoice = false)
    {
        if (SchemaCollector.Current is { } schema)
        {
            schema.AddAdditionalBoolChoice(config, checkBoxName, checkboxDescription, isConditionalChoice);
        }
    }

    /// <summary>Declares a group of choices that are shown in a row.</summary>
    public static void DrawHorizontalMultiChoice(
        string config,
        string checkBoxName,
        string checkboxDescription,
        int totalChoices,
        int choice,
        float itemWidth = 150,
        Vector4 descriptionColor = default)
    {
        if (SchemaCollector.Current is { } schema)
        {
            schema.AddHorizontalMultiChoice(config, checkBoxName, checkboxDescription, totalChoices, choice);
        }
    }

    /// <summary>Declares the statuses that are blocked in a fight against other players.</summary>
    public static void DrawPvPStatusMultiChoice(string config)
    {
        if (SchemaCollector.Current is { } schema)
        {
            schema.AddPvPStatusMultiChoice(config);
        }
    }

    /// <summary>Declares the order in which the abilities of a feature are used.</summary>
    internal static void DrawPriorityInput(UserIntArray config, int maxValues, int currentItem, string customLabel = "")
    {
        if (SchemaCollector.Current is { } schema)
        {
            schema.AddPriorityInput(config.Name, maxValues, currentItem, customLabel);
        }
    }

    /// <summary>Declares a timeline of the user.</summary>
    /// <remarks>
    /// The timelines of the user are edited through their own part of the
    /// interface, so they are not part of the description of the options.
    /// </remarks>
    public static void DrawCustom(CustomTimeline customTimeline, List<CustomTimeline> customTimelineList)
    {
        // The timelines of the user have an editor of their own.
    }
}
