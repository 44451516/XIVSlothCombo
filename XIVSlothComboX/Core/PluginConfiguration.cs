using System;
using System.Collections.Generic;
using System.Linq;
using Dalamud.Configuration;
using Dalamud.Utility;
using Newtonsoft.Json;
using XIVSlothComboX.Attributes;
using XIVSlothComboX.Combos;
using XIVSlothComboX.Combos.PvE;
using XIVSlothComboX.Services;
using XIVSlothComboX.Extensions;
using System.Numerics;
using XIVSlothComboX.Data;

namespace XIVSlothComboX.Core
{
    /// <summary> Plugin configuration. </summary>
    [Serializable]
    public class PluginConfiguration : IPluginConfiguration
    {
        #region Version

        /// <summary> Gets or sets the configuration version. </summary>
        public int Version { get; set; } = 5;

        #endregion

        #region EnabledActions

        /// <summary> Gets or sets the collection of enabled combos. </summary>
        [JsonProperty("EnabledActionsV6")]
        public HashSet<CustomComboPreset> EnabledActions { get; set; } = [];

        #endregion

        #region Settings Options

        /// <summary> Gets or sets a value indicating whether to output combat log to the chatbox. </summary>
        public bool EnabledOutputLog { get; set; } = false;
        public bool SetOutChat { get; set; } = false;

        /// <summary> Gets or sets a value indicating whether to hide combos which conflict with enabled presets. </summary>
        public bool HideConflictedCombos { get; set; } = false;

        /// <summary> Gets or sets a value indicating whether to hide the children of a feature if it is disabled. </summary>
        public bool HideChildren { get; set; } = false;

        /// <summary> Gets or sets the offset of the melee range check. Default is 0. </summary>
        public double MeleeOffset { get; set; } = 0;

        public Vector4 TargetHighlightColor { get; set; } = new() { W = 1, X = 0.5f, Y = 0.5f, Z = 0.5f };

        public bool 自动精炼 { get; set; } = false;
        public bool 只精炼亚力山大 { get; set; } = false;
        
        public bool 自动食物 { get; set; } = false;
        public uint 自动食物Id { get; set; } = 4752;
        public bool RequiredFoodHQ { get; set; } = false;
        public bool 自动精炼药 { get; set; } = false;
        #endregion

        #region Custom Float Values

        [JsonProperty("CustomFloatValuesV6")]
        internal static Dictionary<string, float> CustomFloatValues { get; set; } = [];

        /// <summary> Gets a custom float value. </summary>
        public static float GetCustomFloatValue(string config, float defaultMinValue = 0)
        {
            if (!CustomFloatValues.TryGetValue(config, out float configValue))
            {
                SetCustomFloatValue(config, defaultMinValue);
                return defaultMinValue;
            }

            return configValue;
        }

        /// <summary> Sets a custom float value. </summary>
        public static void SetCustomFloatValue(string config, float value) => CustomFloatValues[config] = value;

        #endregion

        #region Custom Int Values

        [JsonProperty("CustomIntValuesV6")]
        internal static Dictionary<string, int> CustomIntValues { get; set; } = [];

        /// <summary> Gets a custom integer value. </summary>
        public static int GetCustomIntValue(string config, int defaultMinVal = 0)
        {
            if (!CustomIntValues.TryGetValue(config, out int configValue))
            {
                SetCustomIntValue(config, defaultMinVal);
                return defaultMinVal;
            }

            return configValue;
        }

        /// <summary> Sets a custom integer value. </summary>
        public static void SetCustomIntValue(string config, int value) => CustomIntValues[config] = value;

        #endregion

        #region Custom Int Array Values
        [JsonProperty("CustomIntArrayValuesV6")]
        internal static Dictionary<string, int[]> CustomIntArrayValues { get; set; } = [];

        /// <summary> Gets a custom integer array value. </summary>
        public static int[] GetCustomIntArrayValue(string config)
        {
            if (!CustomIntArrayValues.TryGetValue(config, out int[]? configValue))
            {
                SetCustomIntArrayValue(config, []);
                return [];
            }

            return configValue;
        }

        /// <summary> Sets a custom integer array value. </summary>
        public static void SetCustomIntArrayValue(string config, int[] value) => CustomIntArrayValues[config] = value;

        #endregion

        #region Custom Bool Values

        [JsonProperty("CustomBoolValuesV6")]
        internal static Dictionary<string, bool> CustomBoolValues { get; set; } = [];

        /// <summary> Gets a custom boolean value. </summary>
        public static bool GetCustomBoolValue(string config)
        {
            if (!CustomBoolValues.TryGetValue(config, out bool configValue))
            {
                SetCustomBoolValue(config, false);
                return false;
            }

            return configValue;
        }

        /// <summary> Sets a custom boolean value. </summary>
        public static void SetCustomBoolValue(string config, bool value) => CustomBoolValues[config] = value;

        #endregion

        #region Custom Bool Array Values

        [JsonProperty("CustomBoolArrayValuesV6")]
        internal static Dictionary<string, bool[]> CustomBoolArrayValues { get; set; } = [];

        /// <summary> Gets a custom boolean array value. </summary>
        public static bool[] GetCustomBoolArrayValue(string config)
        {
            if (!CustomBoolArrayValues.TryGetValue(config, out bool[]? configValue))
            {
                SetCustomBoolArrayValue(config, Array.Empty<bool>());
                return Array.Empty<bool>();
            }

            return configValue;
        }

        /// <summary> Sets a custom boolean array value. </summary>
        public static void SetCustomBoolArrayValue(string config, bool[] value) => CustomBoolArrayValues[config] = value;

        #endregion

        #region Job-specific

        /// <summary> Gets active Blue Mage (BLU) spells. </summary>
        public List<uint> ActiveBLUSpells { get; set; } = [];

        
        #endregion

        
        
        [JsonProperty]
        public static List<CustomTimeline> CustomTimelineList { get; set; } = new();
        
        #region Preset Resetting

        [JsonProperty]
        private static Dictionary<string, bool> ResetFeatureCatalog { get; set; } = [];

        private static bool GetResetValues(string config)
        {
            if (ResetFeatureCatalog.TryGetValue(config, out var value)) return value;

            return false;
        }

        private static void SetResetValues(string config, bool value)
        {
            ResetFeatureCatalog[config] = value;
        }

        public void ResetFeatures(string config, int[] values)
        {
            Service.PluginLog.Debug($"{config} {GetResetValues(config)}");
            if (!GetResetValues(config))
            {
                bool needToResetMessagePrinted = false;

                var presets = Enum.GetValues<CustomComboPreset>().Cast<int>();

                foreach (int value in values)
                {
                    Service.PluginLog.Debug(value.ToString());
                    if (presets.Contains(value))
                    {
                        var preset = Enum.GetValues<CustomComboPreset>()
                            .Where(preset => (int)preset == value)
                            .First();

                        if (!PresetStorage.IsEnabled(preset)) continue;

                        if (!needToResetMessagePrinted)
                        {
                            Service.ChatGui.PrintError($"[XIVSlothCombo] Some features have been disabled due to an internal configuration update:");
                            needToResetMessagePrinted = !needToResetMessagePrinted;
                        }

                        var info = preset.GetComboAttribute();
                        Service.ChatGui.PrintError($"[XIVSlothCombo] - {info.JobName}: {info.FancyName}");
                        EnabledActions.Remove(preset);
                    }
                }
                
                if (needToResetMessagePrinted)
                Service.ChatGui.PrintError($"[XIVSlothCombo] Please re-enable these features to use them again. We apologise for the inconvenience");
            }
            SetResetValues(config, true);
            Save();
        }

        #endregion

        #region Other (SpecialEvent, MotD, Save)

        /// <summary> Handles 'special event' feature naming. </summary>
        public bool SpecialEvent { get; set; } = false;

        /// <summary> Hides the message of the day. </summary>
        public bool HideMessageOfTheDay { get; set; } = false;

        public bool RecommendedSettingsViewed { get; set; } = false;

        /// <summary> Save the configuration to disk. </summary>
        public void Save() => Service.Interface.SavePluginConfig(this);

        #endregion
        
        #region test

        public string FakeName { get; set; } = string.Empty;
        public bool isFakeName { get; set; } = false;

        #endregion
    }
}
