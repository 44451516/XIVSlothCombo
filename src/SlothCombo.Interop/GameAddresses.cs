using System;

namespace SlothCombo.Interop;

/// <summary>
/// The addresses of the game that the program uses.
/// </summary>
/// <remarks>
/// <para>
/// The patterns are the ones the game was built with. A pattern that starts
/// with a call stands for the function the call goes to, which is how the
/// game reaches most of its functions.
/// </para>
/// <para>
/// A pattern that leads to a place that holds a pointer needs the pointer to
/// be read, which the table takes care of.
/// </para>
/// </remarks>
public static class GameAddresses
{
    /// <summary>The framework of the game.</summary>
    public const string Framework = "Framework";

    /// <summary>The table of the virtual functions of the framework.</summary>
    public const string FrameworkVirtualTable = "FrameworkVirtualTable";

    /// <summary>The action manager of the player.</summary>
    public const string ActionManager = "ActionManager";

    /// <summary>The characters of the world.</summary>
    public const string CharacterManager = "CharacterManager";

    /// <summary>The objects the client makes on its own.</summary>
    public const string ClientObjectManager = "ClientObjectManager";

    /// <summary>The items the player carries.</summary>
    public const string InventoryManager = "InventoryManager";

    /// <summary>The objects the player has its eyes on.</summary>
    public const string TargetSystem = "TargetSystem";

    /// <summary>The group of the player.</summary>
    public const string GroupManager = "GroupManager";

    /// <summary>The main state of the game.</summary>
    public const string GameMain = "GameMain";

    /// <summary>The conditions of the world.</summary>
    public const string Conditions = "Conditions";

    /// <summary>The state of the interface of the game.</summary>
    public const string UIState = "UIState";

    /// <summary>The gauges of the jobs.</summary>
    public const string JobGaugeManager = "JobGaugeManager";

    /// <summary>The function that handles the effects of an action.</summary>
    public const string ActionEffectHandlerReceive = "ActionEffectHandlerReceive";

    /// <summary>The function that uses an action at a place.</summary>
    public const string UseActionLocation = "UseActionLocation";

    /// <summary>The function that uses an action.</summary>
    public const string UseAction = "UseAction";

    /// <summary>The function that tells the state of an action.</summary>
    public const string GetActionStatus = "GetActionStatus";

    /// <summary>The function that tells the number that stands for an action.</summary>
    public const string GetAdjustedActionId = "GetAdjustedActionId";

    /// <summary>The function that tells the time the cast has taken.</summary>
    public const string GetRecastTimeElapsed = "GetRecastTimeElapsed";

    /// <summary>The function that tells the number of the charges of an action.</summary>
    public const string GetCurrentCharges = "GetCurrentCharges";

    /// <summary>The function that tells the time an action takes to be used again.</summary>
    public const string GetAdjustedRecastTime = "GetAdjustedRecastTime";

    /// <summary>The function that tells the number of the charges an action has.</summary>
    public const string GetMaxCharges = "GetMaxCharges";

    /// <summary>The function that tells what an action costs.</summary>
    public const string GetActionCost = "GetActionCost";

    /// <summary>The function that tells whether an action reaches its target.</summary>
    public const string GetActionInRangeOrLoS = "GetActionInRangeOrLoS";

    /// <summary>The function that checks whether the resources of an action are there.</summary>
    public const string CheckActionResources = "CheckActionResources";

    /// <summary>The function that gives the container of the items of a kind.</summary>
    public const string GetInventoryContainer = "GetInventoryContainer";

    /// <summary>The function that gives the number of the items of a kind.</summary>
    public const string GetInventoryItemCount = "GetInventoryItemCount";

    /// <summary>The function that tells whether the player is in a battle of players.</summary>
    public const string IsInPvPArea = "IsInPvPArea";

    /// <summary>The function that tells whether the player is in a battle of players inside an instance.</summary>
    public const string IsInPvPInstance = "IsInPvPInstance";

    /// <summary>The function that tells whether a link of a task is unlocked.</summary>
    public const string IsUnlockLinkUnlockedOrQuestCompleted = "IsUnlockLinkUnlockedOrQuestCompleted";

    /// <summary>The function that writes a line into the chat.</summary>
    public const string RaptureLogModulePrintString = "RaptureLogModulePrintString";

    /// <summary>The function that gives an agent of the interface.</summary>
    public const string AgentModuleGetAgentByInternalId = "AgentModuleGetAgentByInternalId";

    /// <summary>The function that gives the object the player has its eyes on.</summary>
    public const string TargetSystemGetTargetObject = "TargetSystemGetTargetObject";

    /// <summary>The function that gives the object the player has its eyes on for the moment.</summary>
    public const string TargetSystemGetSoftTarget = "TargetSystemGetSoftTarget";

    /// <summary>The function that gives the object the player keeps its eyes on.</summary>
    public const string TargetSystemGetHardTarget = "TargetSystemGetHardTarget";

    /// <summary>The function that points the player at an object.</summary>
    public const string TargetSystemSetHardTarget = "TargetSystemSetHardTarget";

    /// <summary>The function that tells whether an action may be used on an object.</summary>
    public const string CanUseActionOnTarget = "CanUseActionOnTarget";

    /// <summary>The function that tells the time an action takes to be cast.</summary>
    public const string GetAdjustedCastTime = "GetAdjustedCastTime";

    /// <summary>The function that tells the time an action takes to be used again.</summary>
    public const string GetRecastTime = "GetRecastTime";

    /// <summary>The function that shoots a ray into the world of the game.</summary>
    public const string RaycastMaterialFilter = "RaycastMaterialFilter";

    /// <summary>The function that turns a placeholder into the object it stands for.</summary>
    public const string ResolvePlaceholder = "ResolvePlaceholder";

    /// <summary>
    /// Fills the given table with the addresses of the game.
    /// </summary>
    /// <param name="table">The table to fill.</param>
    public static void Register(GameAddressTable table)
    {
        ArgumentNullException.ThrowIfNull(table);

        // The framework is the state the game runs on. It is reached through
        // a place that holds the pointer to it.
        table.Add(GameAddress.Static(Framework, "48 8B 1D ?? ?? ?? ?? 8B 7C 24", 3, true));
        table.Add(GameAddress.Static(FrameworkVirtualTable, "48 8D 05 ?? ?? ?? ?? ?? ?? ?? 48 8B F1 49 89 6B", 3));

        // The managers of the game are all reached the same way: a place in
        // the code that holds the address of the manager.
        table.Add(GameAddress.Static(ActionManager, "48 8D 0D ?? ?? ?? ?? F3 0F 10 13", 3));
        table.Add(GameAddress.Static(CharacterManager, "8B D0 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 85 C0 0F 84 ?? ?? ?? ?? 48 8B D0", 5));
        table.Add(GameAddress.Static(ClientObjectManager, "48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? C7 43 60 FF FF FF FF", 3));
        table.Add(GameAddress.Static(InventoryManager, "48 8D 0D ?? ?? ?? ?? 81 C2", 3));
        table.Add(GameAddress.Static(TargetSystem, "48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 3B C6 0F 95 C0", 3));
        table.Add(GameAddress.Static(GroupManager, "33 D2 48 8D 0D ?? ?? ?? ?? 33 DB", 5));
        table.Add(GameAddress.Static(GameMain, "48 8D 0D ?? ?? ?? ?? 0F 28 F2 48 89 44 24 ??", 3));
        table.Add(GameAddress.Static(Conditions, "48 8D 0D ?? ?? ?? ?? 66 2B D8", 3));
        table.Add(GameAddress.Static(UIState, "48 8D 0D ?? ?? ?? ?? 45 33 C0 BA ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8B 0D ?? ?? ?? ?? BA", 3));
        table.Add(GameAddress.Static(JobGaugeManager, "48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? B8 ?? ?? ?? ?? 66 3B F0", 3));

        // The functions of the game are reached through the call that leads
        // to them, so the pattern starts with the call itself.
        table.Add(GameAddress.Member(ActionEffectHandlerReceive, "E8 ?? ?? ?? ?? 48 8B 8D ?? ?? ?? ?? 48 33 CC E8 ?? ?? ?? ?? 48 81 C4 00 05 00 00"));
        table.Add(GameAddress.Member(UseActionLocation, "E8 ?? ?? ?? ?? 48 8B BC 24 ?? ?? ?? ?? 44 0F B6 F8 B0"));
        table.Add(GameAddress.Member(UseAction, "E8 ?? ?? ?? ?? B0 01 EB B6"));
        table.Add(GameAddress.Member(GetActionStatus, "E8 ?? ?? ?? ?? 4C 8B 6C 24 ?? 85 C0 74"));
        table.Add(GameAddress.Member(GetAdjustedActionId, "E8 ?? ?? ?? ?? 89 03 8B 03"));
        table.Add(GameAddress.Member(GetRecastTimeElapsed, "E8 ?? ?? ?? ?? F3 0F 5C F0 33 F6"));
        table.Add(GameAddress.Member(GetCurrentCharges, "48 89 6C 24 ?? 48 89 74 24 ?? 41 56 48 83 EC ?? 8B EA 48 89 5D 24"));
        table.Add(GameAddress.Member(GetAdjustedRecastTime, "E8 ?? ?? ?? ?? 85 C0 7E 1C"));
        table.Add(GameAddress.Member(GetMaxCharges, "E9 ?? ?? ?? ?? 41 B0 ?? 8B D3"));
        table.Add(GameAddress.Member(GetActionCost, "E8 ?? ?? ?? ?? 40 0F B6 CE E9"));
        table.Add(GameAddress.Member(GetActionInRangeOrLoS, "E8 ?? ?? ?? ?? 85 C0 75 02 33 C0"));
        table.Add(GameAddress.Member(CheckActionResources, "E8 ?? ?? ?? ?? 85 C0 75 15 4C 8B C6"));
        table.Add(GameAddress.Member(GetInventoryContainer, "E8 ?? ?? ?? ?? 88 58 18"));
        table.Add(GameAddress.Member(GetInventoryItemCount, "E8 ?? ?? ?? ?? 8B 53 F1"));
        table.Add(GameAddress.Member(IsInPvPArea, "E8 ?? ?? ?? ?? 88 45 F1"));
        table.Add(GameAddress.Member(IsInPvPInstance, "40 53 48 83 EC 20 48 8B 1D ?? ?? ?? ?? 48 85 DB 74 1E 48 8D 0D"));
        table.Add(GameAddress.Member(IsUnlockLinkUnlockedOrQuestCompleted, "E8 ?? ?? ?? ?? 44 22 F0"));
        table.Add(GameAddress.Member(RaptureLogModulePrintString, "E8 ?? ?? ?? ?? 40 80 C6 41"));
        table.Add(GameAddress.Member(AgentModuleGetAgentByInternalId, "E8 ?? ?? ?? ?? 83 7B 48 00"));
        table.Add(GameAddress.Member(TargetSystemGetTargetObject, "E8 ?? ?? ?? ?? 45 33 F6 48 8B F8 45 0F 57 C0"));
        table.Add(GameAddress.Member(TargetSystemGetSoftTarget, "E8 ?? ?? ?? ?? 48 3B C3 74 ?? B0 01"));
        table.Add(GameAddress.Member(TargetSystemGetHardTarget, "E8 ?? ?? ?? ?? 48 3B C7 75 ?? 32 C0"));
        table.Add(GameAddress.Member(TargetSystemSetHardTarget, "E8 ?? ?? ?? ?? 48 8B 5C 24 ?? 48 83 C4 30 5F C3 8B 83"));
        table.Add(GameAddress.Member(CanUseActionOnTarget, "48 89 5C 24 ?? 57 48 81 EC 20 48 8B DA 48 8B F9 E8 ?? ?? ?? ?? 4C 8B C3"));
        table.Add(GameAddress.Member(GetAdjustedCastTime, "E8 ?? ?? ?? ?? 8B D0 48 8B CF E8 ?? ?? ?? ?? 0F B7 55"));
        table.Add(GameAddress.Member(GetRecastTime, "E8 ?? ?? ?? ?? 83 7F 58 01"));
        table.Add(GameAddress.Member(RaycastMaterialFilter, "E8 ?? ?? ?? ?? 84 C0 41 0F B6 D6"));
        table.Add(GameAddress.Member(ResolvePlaceholder, "E8 ?? ?? ?? ?? 48 8B 5C 24 ?? EB 0C"));
    }
}
