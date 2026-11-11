using System;
using System.Runtime.InteropServices;

namespace SlothCombo.Interop.Game;

/// <summary>The kind of an action.</summary>
public enum ActionType : uint
{
    /// <summary>No action.</summary>
    None = 0,

    /// <summary>A skill of a job.</summary>
    Action = 1,

    /// <summary>An item.</summary>
    Item = 2,

    /// <summary>An item of an event.</summary>
    EventItem = 3,

    /// <summary>An action of an event.</summary>
    EventAction = 4,

    /// <summary>A general action.</summary>
    GeneralAction = 5,

    /// <summary>An action of a buddy.</summary>
    BuddyAction = 6,

    /// <summary>A command.</summary>
    MainCommand = 7,

    /// <summary>An action of a companion.</summary>
    Companion = 8,

    /// <summary>An action of a craft.</summary>
    CraftAction = 9,

    /// <summary>Nothing.</summary>
    Unk10 = 10,

    /// <summary>An action of a pet.</summary>
    PetAction = 11,

    /// <summary>Nothing.</summary>
    Unk12 = 12,

    /// <summary>An action of a mount.</summary>
    Mount = 13,

    /// <summary>An action of a battle against other players.</summary>
    PvPAction = 14,

    /// <summary>A marker.</summary>
    FieldMarker = 15,

    /// <summary>An action of a race of chocobos.</summary>
    ChocoboRaceAbility = 16,

    /// <summary>An item of a race of chocobos.</summary>
    ChocoboRaceItem = 17,

    /// <summary>Nothing.</summary>
    Unk18 = 18,

    /// <summary>An action of an army.</summary>
    BgcArmyAction = 19,

    /// <summary>An ornament.</summary>
    Ornament = 20,
}

/// <summary>The way an action is used.</summary>
public enum UseActionMode : uint
{
    /// <summary>The action is used right away.</summary>
    None = 0,

    /// <summary>The action was queued before.</summary>
    Queue = 1,

    /// <summary>The action is used by a macro.</summary>
    Macro = 2,

    /// <summary>The action is used by a combo.</summary>
    Combo = 3,
}

/// <summary>The combo that is being carried out.</summary>
[StructLayout(LayoutKind.Explicit, Size = 0x08)]
public struct ComboDetail
{
    /// <summary>The time the combo is left.</summary>
    [FieldOffset(0x00)]
    public float Timer;

    /// <summary>The action of the combo.</summary>
    [FieldOffset(0x04)]
    public uint Action;
}

/// <summary>The actions of the player.</summary>
/// <remarks>
/// The manager holds the state of the actions the player uses: the action
/// that is cast, the action that is queued, and the combo that is carried out.
/// </remarks>
[StructLayout(LayoutKind.Explicit, Size = 0x800)]
public unsafe struct ActionManager
{
    /// <summary>The number that stands for no target at all.</summary>
    public const ulong NoTarget = 0xE000_0000;

    /// <summary>The time until which the player cannot act.</summary>
    [FieldOffset(0x08)]
    public float AnimationLock;

    /// <summary>The action that is cast.</summary>
    [FieldOffset(0x24)]
    public uint CastSpellId;

    /// <summary>The kind of the action that is cast.</summary>
    [FieldOffset(0x28)]
    public ActionType CastActionType;

    /// <summary>The action that is cast.</summary>
    [FieldOffset(0x2C)]
    public uint CastActionId;

    /// <summary>The time the cast has taken so far.</summary>
    [FieldOffset(0x30)]
    public float CastTimeElapsed;

    /// <summary>The time the cast takes.</summary>
    [FieldOffset(0x34)]
    public float CastTimeTotal;

    /// <summary>The object the cast is aimed at.</summary>
    [FieldOffset(0x38)]
    public GameObjectId CastTargetId;

    /// <summary>The combo that is being carried out.</summary>
    [FieldOffset(0x60)]
    public ComboDetail Combo;

    /// <summary>Whether an action is waiting to be used.</summary>
    [FieldOffset(0x68)]
    public bool ActionQueued;

    /// <summary>The kind of the action that is queued.</summary>
    [FieldOffset(0x6C)]
    public ActionType QueuedActionType;

    /// <summary>The action that is queued.</summary>
    [FieldOffset(0x70)]
    public uint QueuedActionId;

    /// <summary>The action that is being aimed at a place.</summary>
    [FieldOffset(0x90)]
    public uint AreaTargetingActionId;

    /// <summary>The number of the action that was used last.</summary>
    [FieldOffset(0x120)]
    public ushort LastUsedActionSequence;

    /// <summary>The number of the action that was handled last.</summary>
    [FieldOffset(0x122)]
    public ushort LastHandledActionSequence;

    /// <summary>Gets the manager of the actions of the player.</summary>
    /// <returns>The manager, or <c>null</c> when the game is not ready.</returns>
    public static ActionManager* Instance()
        => (ActionManager*)GameFunctions.Get(GameAddresses.ActionManager);

    /// <summary>Uses an action.</summary>
    /// <param name="actionType">The kind of the action.</param>
    /// <param name="actionId">The number of the action.</param>
    /// <param name="targetId">The object the action is aimed at.</param>
    /// <param name="extraParam">The extra number of the action.</param>
    /// <param name="mode">The way the action is used.</param>
    /// <param name="comboRouteId">The number of the route of the combo.</param>
    /// <param name="outOptAreaTargeted">Tells whether the action was aimed at an area.</param>
    /// <returns><c>true</c> when the action was used.</returns>
    public bool UseAction(
        ActionType actionType,
        uint actionId,
        ulong targetId = NoTarget,
        uint extraParam = 0,
        UseActionMode mode = UseActionMode.None,
        uint comboRouteId = 0,
        bool* outOptAreaTargeted = null)
    {
        var function = (delegate* unmanaged<ActionManager*, ActionType, uint, ulong, uint, UseActionMode, uint, bool*, bool>)
            GameFunctions.Get(GameAddresses.UseAction);

        return function(StructPtr.Of(ref this), actionType, actionId, targetId, extraParam, mode, comboRouteId, outOptAreaTargeted);
    }

    /// <summary>Uses an action at a place.</summary>
    /// <param name="actionType">The kind of the action.</param>
    /// <param name="actionId">The number of the action.</param>
    /// <param name="targetId">The object the action is aimed at.</param>
    /// <param name="location">The place the action is aimed at.</param>
    /// <param name="extraParam">The extra number of the action.</param>
    /// <param name="a7">An extra number of the action.</param>
    /// <returns><c>true</c> when the action was used.</returns>
    public bool UseActionLocation(
        ActionType actionType,
        uint actionId,
        ulong targetId = NoTarget,
        System.Numerics.Vector3* location = null,
        uint extraParam = 0,
        byte a7 = 0)
    {
        var function = (delegate* unmanaged<ActionManager*, ActionType, uint, ulong, System.Numerics.Vector3*, uint, byte, bool>)
            GameFunctions.Get(GameAddresses.UseActionLocation);

        return function(StructPtr.Of(ref this), actionType, actionId, targetId, location, extraParam, a7);
    }

    /// <summary>Tells the state of an action.</summary>
    /// <param name="actionType">The kind of the action.</param>
    /// <param name="actionId">The number of the action.</param>
    /// <param name="targetId">The object the action is aimed at.</param>
    /// <param name="checkRecastActive">Whether the time the action takes is checked.</param>
    /// <param name="checkCastingActive">Whether the cast is checked.</param>
    /// <param name="outOptExtraInfo">Receives the extra number of the state.</param>
    /// <returns>The state of the action.</returns>
    public uint GetActionStatus(
        ActionType actionType,
        uint actionId,
        ulong targetId = NoTarget,
        bool checkRecastActive = true,
        bool checkCastingActive = true,
        uint* outOptExtraInfo = null)
    {
        var function = (delegate* unmanaged<ActionManager*, ActionType, uint, ulong, bool, bool, uint*, uint>)
            GameFunctions.Get(GameAddresses.GetActionStatus);

        return function(StructPtr.Of(ref this), actionType, actionId, targetId, checkRecastActive, checkCastingActive, outOptExtraInfo);
    }

    /// <summary>Gives the number that stands for an action.</summary>
    /// <param name="actionId">The number of the action.</param>
    /// <returns>The number the game uses for the action.</returns>
    public uint GetAdjustedActionId(uint actionId)
    {
        var function = (delegate* unmanaged<ActionManager*, uint, uint>)
            GameFunctions.Get(GameAddresses.GetAdjustedActionId);

        return function(StructPtr.Of(ref this), actionId);
    }

    /// <summary>Gives the time the cast of an action has taken.</summary>
    /// <param name="actionType">The kind of the action.</param>
    /// <param name="actionId">The number of the action.</param>
    /// <returns>The time in seconds.</returns>
    public float GetRecastTimeElapsed(ActionType actionType, uint actionId)
    {
        var function = (delegate* unmanaged<ActionManager*, ActionType, uint, float>)
            GameFunctions.Get(GameAddresses.GetRecastTimeElapsed);

        return function(StructPtr.Of(ref this), actionType, actionId);
    }

    /// <summary>Gives the number of the charges of an action that are left.</summary>
    /// <param name="actionId">The number of the action.</param>
    /// <returns>The number of the charges.</returns>
    public uint GetCurrentCharges(uint actionId)
    {
        var function = (delegate* unmanaged<ActionManager*, uint, uint>)
            GameFunctions.Get(GameAddresses.GetCurrentCharges);

        return function(StructPtr.Of(ref this), actionId);
    }

    /// <summary>Gives the time an action takes to be used again.</summary>
    /// <param name="actionType">The kind of the action.</param>
    /// <param name="actionId">The number of the action.</param>
    /// <param name="applyClassMechanics">Whether the skills of the class are counted.</param>
    /// <returns>The time in hundredths of a second.</returns>
    public static int GetAdjustedRecastTime(ActionType actionType, uint actionId, bool applyClassMechanics = true)
    {
        var function = (delegate* unmanaged<ActionType, uint, bool, int>)
            GameFunctions.Get(GameAddresses.GetAdjustedRecastTime);

        return function(actionType, actionId, applyClassMechanics);
    }

    /// <summary>Gives the number of the charges an action has.</summary>
    /// <param name="actionId">The number of the action.</param>
    /// <param name="level">The level of the class, or zero for the current one.</param>
    /// <returns>The number of the charges.</returns>
    public static ushort GetMaxCharges(uint actionId, uint level)
    {
        var function = (delegate* unmanaged<uint, uint, ushort>)
            GameFunctions.Get(GameAddresses.GetMaxCharges);

        return function(actionId, level);
    }

    /// <summary>Gives what an action costs.</summary>
    /// <param name="actionType">The kind of the action.</param>
    /// <param name="actionId">The number of the action.</param>
    /// <param name="a3">An extra number of the action.</param>
    /// <param name="a4">An extra number of the action.</param>
    /// <param name="a5">An extra number of the action.</param>
    /// <param name="a6">An extra number of the action.</param>
    /// <returns>What the action costs.</returns>
    public static int GetActionCost(ActionType actionType, uint actionId, byte a3, byte a4, byte a5, byte a6)
    {
        var function = (delegate* unmanaged<ActionType, uint, byte, byte, byte, byte, int>)
            GameFunctions.Get(GameAddresses.GetActionCost);

        return function(actionType, actionId, a3, a4, a5, a6);
    }

    /// <summary>Tells whether an action reaches its target.</summary>
    /// <param name="actionId">The number of the action.</param>
    /// <param name="sourceObject">The object the action comes from.</param>
    /// <param name="targetObject">The object the action is aimed at.</param>
    /// <returns>The state of the reach of the action.</returns>
    public static uint GetActionInRangeOrLoS(uint actionId, GameObject* sourceObject, GameObject* targetObject)
    {
        var function = (delegate* unmanaged<uint, GameObject*, GameObject*, uint>)
            GameFunctions.Get(GameAddresses.GetActionInRangeOrLoS);

        return function(actionId, sourceObject, targetObject);
    }

    /// <summary>Checks whether the resources of an action are there.</summary>
    /// <param name="actionType">The kind of the action.</param>
    /// <param name="actionId">The number of the action.</param>
    /// <param name="actionData">The data of the action.</param>
    /// <returns>The state of the resources.</returns>
    public uint CheckActionResources(ActionType actionType, uint actionId, void* actionData = null)
    {
        var function = (delegate* unmanaged<ActionManager*, ActionType, uint, void*, uint>)
            GameFunctions.Get(GameAddresses.CheckActionResources);

        return function(StructPtr.Of(ref this), actionType, actionId, actionData);
    }

    /// <summary>Gives the time an action takes to be used again.</summary>
    /// <param name="actionType">The kind of the action.</param>
    /// <param name="actionId">The number of the action.</param>
    /// <returns>The time in seconds.</returns>
    public float GetRecastTime(ActionType actionType, uint actionId)
    {
        var function = (delegate* unmanaged<ActionManager*, ActionType, uint, float>)
            GameFunctions.Get(GameAddresses.GetRecastTime);

        return function(StructPtr.Of(ref this), actionType, actionId);
    }

    /// <summary>Gives the time an action takes to be cast.</summary>
    /// <param name="actionType">The kind of the action.</param>
    /// <param name="actionId">The number of the action.</param>
    /// <param name="applyProcs">Whether the effects of the class are counted.</param>
    /// <returns>The time in milliseconds.</returns>
    public static int GetAdjustedCastTime(ActionType actionType, uint actionId, bool applyProcs = true)
    {
        var function = (delegate* unmanaged<ActionType, uint, bool, int>)
            GameFunctions.Get(GameAddresses.GetAdjustedCastTime);

        return function(actionType, actionId, applyProcs);
    }

    /// <summary>Tells whether an action may be used on an object.</summary>
    /// <param name="actionId">The number of the action.</param>
    /// <param name="target">The object the action is aimed at.</param>
    /// <returns><c>true</c> when the action may be used on the object.</returns>
    public static bool CanUseActionOnTarget(uint actionId, GameObject* target)
    {
        var function = (delegate* unmanaged<uint, GameObject*, bool>)
            GameFunctions.Get(GameAddresses.CanUseActionOnTarget);

        return function(actionId, target);
    }
}

/// <summary>The handler of the effects of an action.</summary>
/// <remarks>
/// The game calls the handler when it receives the packet of an action, which
/// holds the head of the action and the effects it had on every target.
/// </remarks>
[StructLayout(LayoutKind.Explicit, Size = 0xF00)]
public unsafe struct ActionEffectHandler
{
    /// <summary>One effect of an action on a target.</summary>
    [StructLayout(LayoutKind.Explicit, Size = 0x08)]
    public struct Effect
    {
        /// <summary>The kind of the effect.</summary>
        [FieldOffset(0)]
        public byte Type;

        /// <summary>The first extra number of the effect.</summary>
        [FieldOffset(1)]
        public byte Param0;

        /// <summary>The second extra number of the effect.</summary>
        [FieldOffset(2)]
        public byte Param1;

        /// <summary>The third extra number of the effect.</summary>
        [FieldOffset(3)]
        public byte Param2;

        /// <summary>The fourth extra number of the effect.</summary>
        [FieldOffset(4)]
        public byte Param3;

        /// <summary>The fifth extra number of the effect.</summary>
        [FieldOffset(5)]
        public byte Param4;

        /// <summary>The number of the effect.</summary>
        [FieldOffset(6)]
        public ushort Value;
    }

    /// <summary>The effects of an action on one target.</summary>
    [StructLayout(LayoutKind.Explicit, Size = 0x40)]
    public unsafe struct TargetEffects
    {
        /// <summary>The effects themselves.</summary>
        [FieldOffset(0x00)]
        public Effect* Effects;
    }

    /// <summary>The head of the effects of an action.</summary>
    [StructLayout(LayoutKind.Explicit, Size = 0x28)]
    public struct Header
    {
        /// <summary>The object the animation of the action is played on.</summary>
        [FieldOffset(0x00)]
        public GameObjectId AnimationTargetId;

        /// <summary>The number of the action.</summary>
        [FieldOffset(0x08)]
        public uint ActionId;

        /// <summary>The number that tells the order of the action.</summary>
        [FieldOffset(0x0C)]
        public uint GlobalSequence;

        /// <summary>The time the caster is held.</summary>
        [FieldOffset(0x10)]
        public float AnimationLock;

        /// <summary>The number of the artillery that is aimed.</summary>
        [FieldOffset(0x14)]
        public uint BallistaEntityId;

        /// <summary>The number that tells the order of the actions of the client.</summary>
        [FieldOffset(0x18)]
        public ushort SourceSequence;

        /// <summary>The turn of the action.</summary>
        [FieldOffset(0x1A)]
        public ushort RotationInt;

        /// <summary>The number of the spell of the action.</summary>
        [FieldOffset(0x1C)]
        public ushort SpellId;

        /// <summary>The change of the animation of the action.</summary>
        [FieldOffset(0x1E)]
        public byte AnimationVariation;

        /// <summary>The kind of the action.</summary>
        [FieldOffset(0x1F)]
        public byte ActionType;

        /// <summary>The flags of the action.</summary>
        [FieldOffset(0x20)]
        public byte Flags;

        /// <summary>The number of the targets of the action.</summary>
        [FieldOffset(0x21)]
        public byte NumTargets;

        /// <summary>Gets a value indicating whether the action is written into the log.</summary>
        public bool ShowInLog => BitField.GetBit(Flags, 0);

        /// <summary>Gets a value indicating whether the caster is held by the action.</summary>
        public bool ForceAnimationLock => BitField.GetBit(Flags, 1);
    }

    /// <summary>
    /// Handles the effects of an action.
    /// </summary>
    /// <param name="casterEntityId">The number of the object that used the action.</param>
    /// <param name="casterPtr">The character that used the action.</param>
    /// <param name="targetPos">The place the action is aimed at.</param>
    /// <param name="header">The head of the effects.</param>
    /// <param name="effects">The effects on every target.</param>
    /// <param name="targetEntityIds">The numbers of the targets.</param>
    public static void Receive(
        uint casterEntityId,
        Character* casterPtr,
        System.Numerics.Vector3* targetPos,
        Header* header,
        TargetEffects* effects,
        GameObjectId* targetEntityIds)
    {
        var function = (delegate* unmanaged<uint, Character*, System.Numerics.Vector3*, Header*, TargetEffects*, GameObjectId*, void>)
            GameFunctions.Get(GameAddresses.ActionEffectHandlerReceive);

        function(casterEntityId, casterPtr, targetPos, header, effects, targetEntityIds);
    }
}
