using System;
using System.Collections.Generic;
using System.Linq;
using SlothCombo.Attributes;
using SlothCombo.Combos;
using SlothCombo.Core;
using SlothCombo.Extensions;
using SlothCombo.Data;
using SlothCombo.Runtime.Services;

using SlothCombo.Runtime.WebUI.Contracts;

namespace SlothCombo.Runtime.WebUI;

/// <summary>
/// Server side representation of the combo presets. Mirrors the grouping and the
/// parent/child relations the legacy ImGui window used to build.
/// </summary>
internal static class PresetService
{
    /// <summary> Game version each job's features were last updated for. </summary>
    internal static readonly Dictionary<uint, string> JobVersions = new()
    {
        { 0, "7.0" }, { 1, "7.0" }, { 2, "7.0" }, { 3, "7.0" }, { 4, "7.0" },
        { 5, "7.0" }, { 6, "7.0" }, { 7, "7.0" }, { 8, "7.0" }, { 9, "7.0" },
        { 10, "7.0" }, { 11, "7.0" }, { 12, "7.0" }, { 13, "7.0" }, { 14, "7.0" },
        { 15, "7.0" }, { 16, "7.0" }, { 17, "7.0" }, { 18, "7.0" }, { 19, "7.0" },
        { 20, "7.0" }, { 21, "7.0" }, { 22, "7.0" }, { 23, "7.0" }, { 24, "7.0" },
        { 25, "7.0" }, { 26, "7.0" }, { 27, "7.0" }, { 28, "7.0" }, { 29, "7.0" },
        { 30, "7.0" }, { 31, "7.0" }, { 32, "7.0" }, { 33, "7.0" }, { 34, "7.0" },
        { 35, "7.0" }, { 36, "6.X" },
    };

    // The fields are built in declaration order, so a field that is built from
    // another one has to come after it. allPresets has to be first, because the
    // grouped presets and the infos are built from it.
    private static readonly List<CustomComboPreset> allPresets = BuildAllPresets();
    private static readonly Dictionary<CustomComboPreset, CustomComboInfoAttribute> presetInfos = BuildPresetInfos();
    private static readonly Dictionary<string, List<(CustomComboPreset Preset, CustomComboInfoAttribute Info)>> groupedPresets = BuildGroupedPresets();
    private static readonly Dictionary<CustomComboPreset, (CustomComboPreset Preset, CustomComboInfoAttribute Info)[]> presetChildren = BuildPresetChildren();

    /// <summary> Top level presets grouped by job name. </summary>
    internal static Dictionary<string, List<(CustomComboPreset Preset, CustomComboInfoAttribute Info)>> GroupedPresets => groupedPresets;

    /// <summary> Child presets of every preset. </summary>
    internal static Dictionary<CustomComboPreset, (CustomComboPreset Preset, CustomComboInfoAttribute Info)[]> PresetChildren => presetChildren;

    private static List<CustomComboPreset> BuildAllPresets() =>
        Enum.GetValues<CustomComboPreset>()
            .Where(preset => (int)preset > 100 && preset != CustomComboPreset.Disabled)
            .Where(preset => preset.GetAttribute<CustomComboInfoAttribute>() != null)
            .ToList();

    private static Dictionary<CustomComboPreset, CustomComboInfoAttribute> BuildPresetInfos()
    {
        var map = new Dictionary<CustomComboPreset, CustomComboInfoAttribute>();
        foreach (var preset in allPresets)
        {
            var info = preset.GetAttribute<CustomComboInfoAttribute>();
            if (info != null) map[preset] = info;
        }

        return map;
    }

    private static Dictionary<string, List<(CustomComboPreset Preset, CustomComboInfoAttribute Info)>> BuildGroupedPresets() =>
        allPresets
            .Select(preset => (Preset: preset, Info: preset.GetAttribute<CustomComboInfoAttribute>()!))
            .Where(tpl => PresetStorage.GetParent(tpl.Preset) == null)
            .OrderByDescending(tpl => tpl.Info.JobID == 0)
            .ThenByDescending(tpl => tpl.Info.JobID == 36)
            .ThenByDescending(tpl => tpl.Info.JobID == 35)
            .ThenByDescending(tpl => tpl.Info.Role == 1)
            .ThenByDescending(tpl => tpl.Info.Role == 4)
            .ThenByDescending(tpl => tpl.Info.Role == 2)
            .ThenByDescending(tpl => tpl.Info.Role == 3)
            .ThenBy(tpl => tpl.Info.ClassJobCategory)
            .ThenBy(tpl => tpl.Info.JobName)
            .ThenBy(tpl => tpl.Info.Order)
            .GroupBy(tpl => tpl.Info.JobName)
            .ToDictionary(tpl => tpl.Key, tpl => tpl.ToList())!;

    private static Dictionary<CustomComboPreset, (CustomComboPreset Preset, CustomComboInfoAttribute Info)[]> BuildPresetChildren()
    {
        var childCombos = Enum.GetValues<CustomComboPreset>()
            .ToDictionary(tpl => tpl, tpl => new List<CustomComboPreset>());

        foreach (var preset in Enum.GetValues<CustomComboPreset>())
        {
            var parent = preset.GetAttribute<ParentComboAttribute>()?.ParentPreset;
            if (parent != null) childCombos[parent.Value].Add(preset);
        }

        return childCombos.ToDictionary(
            kvp => kvp.Key,
            kvp => kvp.Value
                .Select(preset => (Preset: preset, Info: preset.GetAttribute<CustomComboInfoAttribute>()!))
                .OrderBy(tpl => tpl.Info?.Order ?? 0)
                .ToArray())!;
    }

    /// <summary> Resolves a preset identifier sent by the frontend. </summary>
    internal static bool TryResolve(string id, out CustomComboPreset preset)
    {
        preset = default;
        if (string.IsNullOrWhiteSpace(id)) return false;
        return Enum.TryParse(id, out preset) && allPresets.Contains(preset);
    }

    /// <summary> Builds the whole preset tree sent to the frontend. </summary>
    internal static List<PresetNode> BuildPresetNodes()
    {
        var nodes = new List<PresetNode>(allPresets.Count);
        foreach (var preset in allPresets)
        {
            var info = presetInfos[preset];
            var parent = PresetStorage.GetParent(preset);

            nodes.Add(new PresetNode
            {
                Id = preset.ToString(),
                FancyName = info.FancyName,
                Description = info.Description,
                JobId = info.JobID,
                JobName = info.JobName,
                JobShorthand = info.JobShorthand,
                Order = info.Order,
                ParentId = parent?.ToString(),
                ChildIds = presetChildren.TryGetValue(preset, out var children)
                    ? children.Where(c => c.Info != null).Select(c => c.Preset.ToString()).ToList()
                    : new List<string>(),
                ConflictIds = PresetStorage.GetConflicts(preset).Select(c => c.ToString()).ToList(),
                IsVariant = PresetStorage.IsVariant(preset),
                IsBozja = PresetStorage.IsBozja(preset),
                IsEureka = PresetStorage.IsEureka(preset),
                IsPvP = PresetStorage.IsPvP(preset),
                Enabled = PresetStorage.IsEnabled(preset),
                Hidden = IsHidden(preset),
            });
        }

        return nodes;
    }

    /// <summary> Builds the job list shown in the sidebar. </summary>
    internal static List<JobInfo> BuildJobs()
    {
        var jobs = new List<JobInfo>();
        foreach (var (jobName, presets) in groupedPresets)
        {
            var first = presets.First().Info;
            var jobId = first.JobID;
            jobs.Add(new JobInfo
            {
                JobId = jobId,
                Name = jobName,
                Shorthand = first.JobShorthand,
                Version = JobVersions.TryGetValue(jobId, out var version) ? version : null,
                Disabled = DisabledJobs.DisabledJobsPVE.Contains(jobId) || DisabledJobs.DisabledJobsPVP.Contains(jobId),
                IconId = JobIconId(jobId),
                Notice = GetBluNotice(jobName),
            });
        }

        return jobs;
    }

    /// <summary>
    /// Works out the number of the picture that stands for the given job.
    ///
    /// The number is the number of the file that holds the picture in the files
    /// of the game, so the interface can ask the host that serves the game data
    /// for the picture itself.
    /// </summary>
    /// <param name="jobId">The number of the job.</param>
    /// <returns>The number of the picture.</returns>
    private static uint JobIconId(uint jobId) => jobId switch
    {
        0 => 62571,
        51 => 62116,
        _ => 62100 + jobId,
    };

    /// <summary> Builds the additional information rendered on a preset card. </summary>
    internal static PresetDetail BuildDetail(CustomComboPreset preset)
    {
        var detail = new PresetDetail { Id = preset.ToString() };

        var parent = PresetStorage.GetParent(preset);
        while (parent != null)
        {
            detail.ParentChain.Insert(0, parent.Value.ToString());
            parent = PresetStorage.GetParent(parent.Value);
        }

        foreach (var conflict in PresetStorage.GetConflicts(preset))
        {
            detail.Conflicts.Add(conflict.ToString());
            detail.ConflictNames.Add(Describe(conflict));
        }

        var replace = preset.GetAttribute<ReplaceSkillAttribute>();
        if (replace != null)
        {
            detail.Replaces.AddRange(replace.ActionNames);
            detail.ReplaceIcons.AddRange(replace.ActionIcons.Select(icon => (uint)icon));
        }

        var blueInactive = preset.GetAttribute<BlueInactiveAttribute>();
        if (blueInactive != null)
        {
            if (blueInactive.Actions.Count > 0)
            {
                detail.BluStatus = "missing";
                detail.BluMissing = string.Join(", ", blueInactive.Actions.Select(x => ActionWatching.GetBLUIndex(x) + ActionWatching.GetActionName(x)));
            }
            else
            {
                detail.BluStatus = "ready";
            }
        }

        detail.VariantParents.AddRange(ParentNames(preset.GetAttribute<VariantParentAttribute>()?.ParentPresets));
        detail.BozjaParents.AddRange(ParentNames(preset.GetAttribute<BozjaParentAttribute>()?.ParentPresets));
        detail.EurekaParents.AddRange(ParentNames(preset.GetAttribute<EurekaParentAttribute>()?.ParentPresets));

        return detail;
    }

    private static IEnumerable<string> ParentNames(CustomComboPreset[]? parents)
    {
        if (parents == null) yield break;
        foreach (var parent in parents) yield return Describe(parent);
    }

    /// <summary> Human readable name of a preset, including its parent chain. </summary>
    internal static string Describe(CustomComboPreset preset)
    {
        var info = preset.GetAttribute<CustomComboInfoAttribute>();
        var name = info?.FancyName ?? preset.ToString();

        var chain = new List<string>();
        var parent = PresetStorage.GetParent(preset);
        while (parent != null)
        {
            var parentInfo = parent.Value.GetAttribute<CustomComboInfoAttribute>();
            chain.Insert(0, parentInfo?.FancyName ?? parent.Value.ToString());
            parent = PresetStorage.GetParent(parent.Value);
        }

        if (chain.Count == 0) return name;
        return string.Join(" -> ", chain) + " -> " + name;
    }

    /// <summary> The Blue Mage notice shown above the Blue Mage feature list. </summary>
    internal static string? GetBluNotice(string jobName)
    {
        if (jobName != CustomComboInfoAttribute.JobIDToName(36)) return null;

        if (Service.Configuration.ActiveBLUSpells.Count == 0)
        {
            return "Please open the Blue Magic Spellbook to populate your active spells and enable features.";
        }

        return "Please note that even if you do not have all the required spells active, you may still use these features.\nAny spells you do not have active will be skipped over so if a feature is not working as intended then\nplease try and enable more required spells.";
    }

    /// <summary>
    /// Enables every parent of a preset, mirroring the behaviour of the legacy UI where
    /// enabling a child feature also enables the features it is nested under.
    /// </summary>
    internal static void EnableParents(CustomComboPreset preset)
    {
        var parentMaybe = PresetStorage.GetParent(preset);
        while (parentMaybe != null)
        {
            var parent = parentMaybe.Value;
            if (!Service.Configuration.EnabledActions.Contains(parent))
            {
                Service.Configuration.EnabledActions.Add(parent);
                foreach (var conflict in PresetStorage.GetConflicts(parent))
                {
                    Service.Configuration.EnabledActions.Remove(conflict);
                }
            }

            parentMaybe = PresetStorage.GetParent(parent);
        }
    }

    /// <summary>
    /// Whether a feature has to be hidden because it conflicts with an enabled one.
    /// Mirrors the "hide conflicting combos" behaviour of the legacy UI.
    /// </summary>
    internal static bool IsHidden(CustomComboPreset preset)
    {
        if (!Service.Configuration.HideConflictedCombos) return false;

        var conflicts = PresetStorage.GetConflicts(preset);
        if (conflicts.Length == 0) return false;

        // Only presets that carry the conflicting attribute can be hidden.
        if (!PresetStorage.GetAllConflicts().Contains(preset)) return false;

        return conflicts.Any(conflict => PresetStorage.IsEnabled(conflict));
    }
}
