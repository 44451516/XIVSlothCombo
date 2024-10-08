﻿using Dalamud.Game.ClientState.Conditions;
using Dalamud.Game.ClientState.Objects.SubKinds;
using ECommons.DalamudServices;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using Lumina.Excel.GeneratedSheets;
using XIVSlothComboX.Services;
using GameMain = FFXIVClientStructs.FFXIV.Client.Game.GameMain;

namespace XIVSlothComboX.CustomComboNS.Functions
{
    internal abstract partial class CustomComboFunctions
    {
        /// <summary> Gets the player or null. </summary>
        public static IPlayerCharacter? LocalPlayer => Service.ClientState.LocalPlayer;

        /// <summary> Find if the player has a certain condition. </summary>
        /// <param name="flag"> Condition flag. </param>
        /// <returns> A value indicating whether the player is in the condition. </returns>
        public static bool HasCondition(ConditionFlag flag) => Service.Condition[flag];

        /// <summary> Find if the player is in combat. </summary>
        /// <returns> A value indicating whether the player is in combat. </returns>
        public static bool InCombat() => Service.Condition[ConditionFlag.InCombat];

        /// <summary> Find if the player has a pet present. </summary>
        /// <returns> A value indicating whether the player has a pet (fairy/carbuncle) present. </returns>
        public static bool HasPetPresent() => Service.BuddyList.PetBuddy!=null;

        /// <summary> Find if the player has a companion (chocobo) present. </summary>
        /// <returns> A value indicating whether the player has a companion (chocobo). </returns>
        public static bool HasCompanionPresent() => Service.BuddyList.CompanionBuddy !=null ;

        /// <summary> Checks if the player is in a PVP enabled zone. </summary>
        /// <returns> A value indicating whether the player is in a PVP enabled zone. </returns>
        public static bool InPvP() => GameMain.IsInPvPArea() || GameMain.IsInPvPInstance();
        
        
        /// <summary> Checks if the player has completed the required job quest for the ability. </summary>
        /// <returns> A value indicating a quest has been completed for a job action.</returns>
        public static unsafe bool IsActionUnlocked(uint id)
        {
            var unlockLink = Service.DataManager.GetExcelSheet<Action>().GetRow(id).UnlockLink;
            if (unlockLink == 0) return true;
            return UIState.Instance()->IsUnlockLinkUnlockedOrQuestCompleted(unlockLink);
        }
    }
}
