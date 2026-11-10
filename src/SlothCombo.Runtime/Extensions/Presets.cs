
using System.Collections.Generic;
using SlothCombo.Attributes;
using SlothCombo.Combos;

namespace SlothCombo.Extensions
{
    internal static class PresetExtensions
    {
        internal static Dictionary<CustomComboPreset, CustomComboInfoAttribute> comboInfoCache = new();

        ///<summary> Retrieves the <see cref="CustomComboInfoAttribute"/> for the preset if it exists.</summary>
        internal static CustomComboInfoAttribute? GetComboAttribute(this CustomComboPreset preset)
        {
            if (comboInfoCache.TryGetValue(preset, out var customComboInfoAttribute))
            {
                return customComboInfoAttribute;
            }

            CustomComboInfoAttribute? att = preset.GetAttribute<CustomComboInfoAttribute>();
            return att != null && comboInfoCache.TryAdd(preset, att) ? comboInfoCache[preset] : null;

        }
    }
}
