using SlothCombo.CustomComboNS.Functions;
using SlothCombo.Data;

namespace SlothCombo.Extensions
{
    internal static class UIntExtensions
    {
        internal static bool LevelChecked(this uint value) => CustomComboFunctions.LevelChecked(value);

        internal static bool TraitLevelChecked(this uint value) => CustomComboFunctions.TraitLevelChecked(value);

        internal static bool ActionReady(this  uint value) => CustomComboFunctions.ActionReady(value);
        internal static uint OriginalHook(this uint value) => CustomComboFunctions.OriginalHook(value);

        internal static string ActionName(this uint value) => ActionWatching.GetActionName(value);
    }

    internal static class UShortExtensions
    {
        internal static string StatusName(this ushort value) => ActionWatching.GetStatusName(value);
    }
}
