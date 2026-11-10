using SlothCombo.Abstractions;

using SlothCombo.Runtime.Services;
using SlothCombo.Interop.Game;
using SlothCombo.Interop.UI;
using ActionRow = SlothCombo.Abstractions.GameData.ActionRow;
using GameMain = SlothCombo.Interop.Game.GameMain;

namespace SlothCombo.CustomComboNS.Functions
{
    internal abstract partial class CustomComboFunctions
    {
        /// <summary> Gets the player or null. </summary>
        public static IGameObject? LocalPlayer => Service.ObjectTable.LocalPlayer;

        /// <summary> Find if the player has a certain condition. </summary>
        /// <param name="flag"> Condition flag. </param>
        /// <returns> A value indicating whether the player is in the condition. </returns>
        public static bool HasCondition(ConditionFlag flag) => Service.Condition[flag];

        /// <summary> Find if the player is in combat. </summary>
        /// <returns> A value indicating whether the player is in combat. </returns>
        public static bool InCombat() => Service.Condition[ConditionFlag.InCombat];

        /// <summary> Find if the player has a pet present. </summary>
        /// <returns> A value indicating whether the player has a pet (fairy/carbuncle) present. </returns>
        public static bool HasPetPresent() => Service.BuddyList.HasPet;

        /// <summary> Find if the player has a companion (chocobo) present. </summary>
        /// <returns> A value indicating whether the player has a companion (chocobo). </returns>
        public static bool HasCompanionPresent() => Service.BuddyList.HasCompanion;

        /// <summary> Checks if the player is in a PVP enabled zone. </summary>
        /// <returns> A value indicating whether the player is in a PVP enabled zone. </returns>
        public static bool InPvP() => GameMain.IsInPvPArea() || GameMain.IsInPvPInstance();
        
        
        /// <summary> Checks if the player has completed the required job quest for the ability. </summary>
        /// <returns> A value indicating a quest has been completed for a job action.</returns>
        public static unsafe bool IsActionUnlocked(uint id)
        {
            var unlockLink = Svc.Data.GetTable<ActionRow>().GetRow(id)?.UnlockLink ?? 0;
            if (unlockLink == 0) return true;
            return UIState.Instance()->IsUnlockLinkUnlockedOrQuestCompleted(unlockLink);
        }
    }
}
