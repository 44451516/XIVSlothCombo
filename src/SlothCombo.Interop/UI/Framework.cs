using System;
using System.Runtime.InteropServices;

namespace SlothCombo.Interop.UI;

/// <summary>The virtual functions of the framework of the game.</summary>
[StructLayout(LayoutKind.Explicit)]
public unsafe struct FrameworkVirtualTable
{
    /// <summary>The function that sets the framework up.</summary>
    [FieldOffset(0x08)]
    public delegate* unmanaged<Framework*, bool> Setup;

    /// <summary>The function that destroys the framework.</summary>
    [FieldOffset(0x10)]
    public delegate* unmanaged<Framework*, bool> Destroy;

    /// <summary>The function that frees the framework.</summary>
    [FieldOffset(0x18)]
    public delegate* unmanaged<Framework*, void> Free;

    /// <summary>The function that runs one turn of the game.</summary>
    [FieldOffset(0x20)]
    public delegate* unmanaged<Framework*, bool> Tick;
}

/// <summary>The framework of the game.</summary>
/// <remarks>
/// The framework is the state the game runs on. It holds the language of the
/// client and the module of the interface.
/// </remarks>
[StructLayout(LayoutKind.Explicit, Size = 0x35D8)]
public unsafe struct Framework
{
    /// <summary>The table of the virtual functions of the framework.</summary>
    [FieldOffset(0x00)]
    public FrameworkVirtualTable* VirtualTable;

    /// <summary>The language of the client.</summary>
    [FieldOffset(0x0580)]
    public byte ClientLanguage;

    /// <summary>The module of the interface.</summary>
    [FieldOffset(0x2B68)]
    public UIModule* UIModule;

    /// <summary>Gets the framework of the game.</summary>
    /// <returns>The framework, or <c>null</c> when the game is not ready.</returns>
    public static Framework* Instance() => (Framework*)GameFunctions.Get(GameAddresses.Framework);

    /// <summary>Gets the table of the virtual functions of the framework.</summary>
    /// <returns>The table, or <c>null</c> when the game is not ready.</returns>
    public static FrameworkVirtualTable* StaticVirtualTablePointer
        => (FrameworkVirtualTable*)GameFunctions.Get(GameAddresses.FrameworkVirtualTable);

    /// <summary>Gets the module of the interface.</summary>
    /// <returns>The module, or <c>null</c> when the game is not ready.</returns>
    public UIModule* GetUIModule() => UIModule;
}

/// <summary>The virtual functions of the module of the interface.</summary>
[StructLayout(LayoutKind.Explicit)]
public unsafe struct UIModuleVirtualTable
{
    /// <summary>The function that gives the module of the windows.</summary>
    [FieldOffset(0x38)]
    public delegate* unmanaged<UIModule*, RaptureAtkModule*> GetRaptureAtkModule;

    /// <summary>The function that gives the module of the pronouns.</summary>
    [FieldOffset(0x50)]
    public delegate* unmanaged<UIModule*, PronounModule*> GetPronounModule;

    /// <summary>The function that gives the module of the log.</summary>
    [FieldOffset(0x58)]
    public delegate* unmanaged<UIModule*, RaptureLogModule*> GetRaptureLogModule;

    /// <summary>The function that gives the module of the agents.</summary>
    [FieldOffset(0x128)]
    public delegate* unmanaged<UIModule*, AgentModule*> GetAgentModule;
}

/// <summary>The module of the interface of the game.</summary>
[StructLayout(LayoutKind.Explicit, Size = 0xFFBA0)]
public unsafe struct UIModule
{
    /// <summary>The table of the virtual functions of the module.</summary>
    [FieldOffset(0x00)]
    public UIModuleVirtualTable* VirtualTable;

    /// <summary>The place of the module of the log.</summary>
    private const int RaptureLogModuleOffset = 0x1AC0;

    /// <summary>The place of the module of the pronouns.</summary>
    private const int PronounModuleOffset = 0xBADA0;

    /// <summary>The place of the module of the windows.</summary>
    private const int RaptureAtkModuleOffset = 0xD2690;

    /// <summary>The module of the log.</summary>
    [FieldOffset(RaptureLogModuleOffset)]
    public RaptureLogModule RaptureLogModule;

    /// <summary>The module of the pronouns.</summary>
    [FieldOffset(PronounModuleOffset)]
    public PronounModule PronounModule;

    /// <summary>The module of the windows.</summary>
    [FieldOffset(RaptureAtkModuleOffset)]
    public RaptureAtkModule RaptureAtkModule;

    /// <summary>Gets the module of the windows.</summary>
    /// <returns>The module, or <c>null</c> when the game is not ready.</returns>
    public RaptureAtkModule* GetRaptureAtkModule()
    {
        if (VirtualTable is null)
        {
            return (RaptureAtkModule*)((byte*)StructPtr.Of(ref this) + RaptureAtkModuleOffset);
        }

        return VirtualTable->GetRaptureAtkModule(StructPtr.Of(ref this));
    }

    /// <summary>Gets the module of the pronouns.</summary>
    /// <returns>The module, or <c>null</c> when the game is not ready.</returns>
    public PronounModule* GetPronounModule()
    {
        if (VirtualTable is null)
        {
            return (PronounModule*)((byte*)StructPtr.Of(ref this) + PronounModuleOffset);
        }

        return VirtualTable->GetPronounModule(StructPtr.Of(ref this));
    }

    /// <summary>Gets the module of the log.</summary>
    /// <returns>The module, or <c>null</c> when the game is not ready.</returns>
    public RaptureLogModule* GetRaptureLogModule()
    {
        if (VirtualTable is null)
        {
            return (RaptureLogModule*)((byte*)StructPtr.Of(ref this) + RaptureLogModuleOffset);
        }

        return VirtualTable->GetRaptureLogModule(StructPtr.Of(ref this));
    }

    /// <summary>Gets the module of the agents.</summary>
    /// <returns>The module, or <c>null</c> when the game is not ready.</returns>
    public AgentModule* GetAgentModule()
    {
        if (VirtualTable is null)
        {
            return null;
        }

        return VirtualTable->GetAgentModule(StructPtr.Of(ref this));
    }
}

/// <summary>The agents of the interface of the game.</summary>
/// <remarks>
/// Every agent of the interface is reached through its number, which is the
/// place of the agent in the table of the module.
/// </remarks>
[StructLayout(LayoutKind.Explicit, Size = 0x1018)]
public unsafe struct AgentModule
{
    /// <summary>The agents themselves.</summary>
    [FieldOffset(0x20)]
    public AgentInterface** Agents;

    /// <summary>Gets the agent with the given number.</summary>
    /// <param name="agentId">The number of the agent.</param>
    /// <returns>The agent, or <c>null</c> when there is none.</returns>
    public AgentInterface* GetAgentByInternalId(AgentId agentId)
    {
        var function = (delegate* unmanaged<AgentModule*, AgentId, AgentInterface*>)
            GameFunctions.Get(GameAddresses.AgentModuleGetAgentByInternalId);

        return function(StructPtr.Of(ref this), agentId);
    }
}

/// <summary>An agent of the interface of the game.</summary>
[StructLayout(LayoutKind.Explicit, Size = 0x28)]
public unsafe struct AgentInterface
{
    /// <summary>The number of the addon of the agent.</summary>
    [FieldOffset(0x20)]
    public uint AddonId;
}

/// <summary>The numbers of the agents of the interface of the game.</summary>
/// <remarks>
/// Only the agents the program uses are written down here. The number of an
/// agent is the place of the agent in the table of the module.
/// </remarks>
public enum AgentId : uint
{
    /// <summary>The agent of the context menu of an item.</summary>
    InventoryContext = 10,

    /// <summary>The agent of the map.</summary>
    Map = 40,

    /// <summary>The agent of the dialog that sets the count down.</summary>
    CountDownSettingDialog = 251,
}

/// <summary>The agent of the map of the game.</summary>
/// <remarks>
/// The agent holds the state of the map of the game, among it whether the
/// player is on its way somewhere.
/// </remarks>
[StructLayout(LayoutKind.Explicit, Size = 0x144E8)]
public unsafe struct AgentMap
{
    /// <summary>Whether the player is moving.</summary>
    [FieldOffset(0x5B08)]
    public bool IsPlayerMoving;

    /// <summary>Gets the agent of the map.</summary>
    /// <returns>The agent, or <c>null</c> when the game is not ready.</returns>
    public static AgentMap* Instance()
    {
        var framework = Framework.Instance();
        if (framework is null)
        {
            return null;
        }

        var uiModule = framework->GetUIModule();
        if (uiModule is null)
        {
            return null;
        }

        var agentModule = uiModule->GetAgentModule();
        if (agentModule is null)
        {
            return null;
        }

        return (AgentMap*)agentModule->GetAgentByInternalId(AgentId.Map);
    }
}

/// <summary>The module of the windows of the interface.</summary>
[StructLayout(LayoutKind.Explicit, Size = 0x2A980)]
public unsafe struct RaptureAtkModule
{
    /// <summary>The manager of the windows.</summary>
    [FieldOffset(0x02C0)]
    public AtkUnitManager* AtkUnitManager;

    /// <summary>The place of the module of the agents.</summary>
    private const int AgentModuleOffset = 0x12438;

    /// <summary>The module of the agents.</summary>
    [FieldOffset(AgentModuleOffset)]
    public AgentModule AgentModule;

    /// <summary>Gets the module of the agents.</summary>
    /// <returns>The module, or <c>null</c> when the game is not ready.</returns>
    public AgentModule* GetAgentModule()
        => (AgentModule*)((byte*)StructPtr.Of(ref this) + AgentModuleOffset);

    /// <summary>Gets the module of the windows.</summary>
    /// <returns>The module, or <c>null</c> when the game is not ready.</returns>
    public static RaptureAtkModule* Instance()
    {
        var framework = Framework.Instance();
        if (framework is null)
        {
            return null;
        }

        var uiModule = framework->GetUIModule();
        return uiModule is null ? null : uiModule->GetRaptureAtkModule();
    }
}

/// <summary>The module of the log of the interface.</summary>
[StructLayout(LayoutKind.Explicit, Size = 0x46D0)]
public unsafe struct RaptureLogModule
{
    /// <summary>Writes a line into the log.</summary>
    /// <param name="message">The line to write.</param>
    public void PrintString(byte* message)
    {
        var function = (delegate* unmanaged<RaptureLogModule*, byte*, void>)
            GameFunctions.Get(GameAddresses.RaptureLogModulePrintString);

        function(StructPtr.Of(ref this), message);
    }

    /// <summary>Writes a line into the log.</summary>
    /// <param name="message">The line to write.</param>
    public void PrintString(string message)
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes(message + "\0");
        fixed (byte* pointer = bytes)
        {
            PrintString(pointer);
        }
    }

    /// <summary>Gets the module of the log.</summary>
    /// <returns>The module, or <c>null</c> when the game is not ready.</returns>
    public static RaptureLogModule* Instance()
    {
        var framework = Framework.Instance();
        if (framework is null)
        {
            return null;
        }

        var uiModule = framework->GetUIModule();
        return uiModule is null ? null : uiModule->GetRaptureLogModule();
    }
}

/// <summary>The module of the pronouns of the interface.</summary>
[StructLayout(LayoutKind.Explicit, Size = 0x3B0)]
public unsafe struct PronounModule
{
    /// <summary>The object the list of the party hovers over.</summary>
    [FieldOffset(0x0290)]
    public Game.GameObject* UiMouseOverTarget;
}

/// <summary>The manager of the windows of the interface.</summary>
[StructLayout(LayoutKind.Explicit, Size = 0x9CA0)]
public unsafe struct AtkUnitManager
{
    /// <summary>Gets the window with the given name.</summary>
    /// <param name="name">The name of the window.</param>
    /// <param name="index">The number of the window among the ones with that name.</param>
    /// <returns>The window, or <c>null</c> when there is none.</returns>
    public AtkUnitBase* GetAddonByName(byte* name, int index = 1)
    {
        var function = (delegate* unmanaged<AtkUnitManager*, byte*, int, AtkUnitBase*>)
            GameFunctions.Get(GameAddresses.AtkUnitManagerGetAddonByName);

        return function(StructPtr.Of(ref this), name, index);
    }

    /// <summary>Gets the window with the given name.</summary>
    /// <param name="name">The name of the window.</param>
    /// <param name="index">The number of the window among the ones with that name.</param>
    /// <returns>The window, or <c>null</c> when there is none.</returns>
    public AtkUnitBase* GetAddonByName(string name, int index = 1)
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes(name + "\0");
        fixed (byte* pointer = bytes)
        {
            return GetAddonByName(pointer, index);
        }
    }
}

/// <summary>The data of the window that is loaded from a file.</summary>
[StructLayout(LayoutKind.Explicit, Size = 0x90)]
public unsafe struct AtkUldManager
{
    /// <summary>The number of the nodes of the window.</summary>
    [FieldOffset(0x42)]
    public ushort NodeListCount;

    /// <summary>The nodes of the window.</summary>
    [FieldOffset(0x50)]
    public AtkResNode** NodeList;
}

/// <summary>The flags of a node of the interface.</summary>
[Flags]
public enum NodeFlags : ushort
{
    /// <summary>The node is anchored to the top.</summary>
    AnchorTop = 0x01,

    /// <summary>The node is anchored to the left.</summary>
    AnchorLeft = 0x02,

    /// <summary>The node is anchored to the bottom.</summary>
    AnchorBottom = 0x04,

    /// <summary>The node is anchored to the right.</summary>
    AnchorRight = 0x08,

    /// <summary>The node is shown.</summary>
    Visible = 0x10,

    /// <summary>The node can be used.</summary>
    Enabled = 0x20,

    /// <summary>The node is clipped.</summary>
    Clip = 0x40,

    /// <summary>The node is filled.</summary>
    Fill = 0x80,

    /// <summary>The node has a collision.</summary>
    HasCollision = 0x100,

    /// <summary>The node answers the mouse.</summary>
    RespondToMouse = 0x200,

    /// <summary>The node can be focused.</summary>
    Focusable = 0x400,

    /// <summary>The node can be dropped on.</summary>
    Droppable = 0x800,

    /// <summary>The node is the top node.</summary>
    IsTopNode = 0x1000,

    /// <summary>The node sends events.</summary>
    EmitsEvents = 0x2000,

    /// <summary>The node uses the order of the depth.</summary>
    UseDepthBasedPriority = 0x4000,

    /// <summary>Nothing.</summary>
    UnkFlag2 = 0x8000,
}

/// <summary>The kinds of event of the interface.</summary>
public enum AtkEventType : byte
{
    /// <summary>The mouse was pressed down.</summary>
    MouseDown = 3,

    /// <summary>The mouse was released.</summary>
    MouseUp = 4,

    /// <summary>The mouse was moved.</summary>
    MouseMove = 5,

    /// <summary>The mouse came over the node.</summary>
    MouseOver = 6,

    /// <summary>The mouse left the node.</summary>
    MouseOut = 7,

    /// <summary>The wheel of the mouse was turned.</summary>
    MouseWheel = 8,

    /// <summary>The mouse was clicked.</summary>
    MouseClick = 9,

    /// <summary>The mouse was clicked twice.</summary>
    MouseDoubleClick = 10,

    /// <summary>The node got the focus.</summary>
    FocusStart = 18,

    /// <summary>The node lost the focus.</summary>
    FocusStop = 19,

    /// <summary>The button was pressed down.</summary>
    ButtonPress = 23,

    /// <summary>The button was released.</summary>
    ButtonRelease = 24,

    /// <summary>The button was clicked.</summary>
    ButtonClick = 25,

    /// <summary>The value of the node changed.</summary>
    ValueUpdate = 27,
}

/// <summary>The data of the event of the interface.</summary>
[StructLayout(LayoutKind.Explicit, Size = 0x28)]
public struct AtkEventData
{
}

/// <summary>The state of an event of the interface.</summary>
[StructLayout(LayoutKind.Explicit, Size = 0x04)]
public struct AtkEventState
{
    /// <summary>The kind of the event.</summary>
    [FieldOffset(0x00)]
    public AtkEventType EventType;

    /// <summary>The flags of what the event did.</summary>
    [FieldOffset(0x01)]
    public byte ReturnFlags;

    /// <summary>The flags of the state of the event.</summary>
    [FieldOffset(0x02)]
    public byte StateFlags;
}

/// <summary>The kinds of value the interface passes around.</summary>
public enum AtkValueType
{
    /// <summary>No value at all.</summary>
    Undefined = 0,

    /// <summary>Nothing.</summary>
    Null = 0x1,

    /// <summary>A truth value.</summary>
    Bool = 0x2,

    /// <summary>A number.</summary>
    Int = 0x3,

    /// <summary>A wide number.</summary>
    Int64 = 0x4,

    /// <summary>A number that is not negative.</summary>
    UInt = 0x5,

    /// <summary>A wide number that is not negative.</summary>
    UInt64 = 0x6,

    /// <summary>A number with a fraction.</summary>
    Float = 0x7,

    /// <summary>A text.</summary>
    String = 0x8,

    /// <summary>A text of wide characters.</summary>
    WideString = 0x9,

    /// <summary>A text that is not changed.</summary>
    ConstString = 0xA,

    /// <summary>A list of values.</summary>
    Vector = 0xB,

    /// <summary>A pointer.</summary>
    Pointer = 0xC,

    /// <summary>A list of values.</summary>
    AtkValues = 0xD,
}

/// <summary>A value the interface passes around.</summary>
/// <remarks>
/// The value holds one number of one kind. The kind is told by the field
/// <see cref="Type"/>, the number itself by the field that matches it.
/// </remarks>
[StructLayout(LayoutKind.Explicit, Size = 0x10)]
public unsafe struct AtkValue
{
    /// <summary>The kind of the value.</summary>
    [FieldOffset(0x00)]
    public AtkValueType Type;

    /// <summary>The value, when it is a truth value.</summary>
    [FieldOffset(0x08)]
    public bool Bool;

    /// <summary>The value, when it is a number.</summary>
    [FieldOffset(0x08)]
    public int Int;

    /// <summary>The value, when it is a wide number.</summary>
    [FieldOffset(0x08)]
    public long Int64;

    /// <summary>The value, when it is a number that is not negative.</summary>
    [FieldOffset(0x08)]
    public uint UInt;

    /// <summary>The value, when it is a wide number that is not negative.</summary>
    [FieldOffset(0x08)]
    public ulong UInt64;

    /// <summary>The value, when it is a number with a fraction.</summary>
    [FieldOffset(0x08)]
    public float Float;

    /// <summary>The value, when it is a pointer.</summary>
    [FieldOffset(0x08)]
    public void* Pointer;
}

/// <summary>An event of the interface.</summary>
[StructLayout(LayoutKind.Explicit, Size = 0x30)]
public unsafe struct AtkEvent
{
    /// <summary>The node the event belongs to.</summary>
    [FieldOffset(0x00)]
    public AtkResNode* Node;

    /// <summary>The node the event is aimed at.</summary>
    [FieldOffset(0x08)]
    public void* Target;

    /// <summary>The node that listens to the event.</summary>
    [FieldOffset(0x10)]
    public void* Listener;

    /// <summary>The extra number of the event.</summary>
    [FieldOffset(0x18)]
    public uint Param;

    /// <summary>The next event of the node.</summary>
    [FieldOffset(0x20)]
    public AtkEvent* NextEvent;

    /// <summary>The state of the event.</summary>
    [FieldOffset(0x28)]
    public AtkEventState State;
}

/// <summary>The events of a node of the interface.</summary>
[StructLayout(LayoutKind.Explicit, Size = 0x08)]
public unsafe struct AtkEventManager
{
    /// <summary>The first event of the node.</summary>
    [FieldOffset(0x00)]
    public AtkEvent* Event;
}

/// <summary>A node of the interface.</summary>
[StructLayout(LayoutKind.Explicit, Size = 0xC0)]
public unsafe struct AtkResNode
{
    /// <summary>The table of the virtual functions of the node.</summary>
    [FieldOffset(0x00)]
    public void* VirtualTable;

    /// <summary>The number of the node.</summary>
    [FieldOffset(0x08)]
    public uint NodeId;

    /// <summary>The events of the node.</summary>
    [FieldOffset(0x18)]
    public AtkEventManager AtkEventManager;

    /// <summary>The flags of the node.</summary>
    [FieldOffset(0xAE)]
    public NodeFlags NodeFlags;
}

/// <summary>The node that holds a component of the interface.</summary>
[StructLayout(LayoutKind.Explicit, Size = 0xC8)]
public unsafe struct AtkComponentNode
{
    /// <summary>The data of the node itself.</summary>
    [FieldOffset(0x00)]
    public AtkResNode AtkResNode;

    /// <summary>The component itself.</summary>
    [FieldOffset(0xC0)]
    public AtkComponentBase* Component;
}

/// <summary>The virtual functions of the thing that listens to the events.</summary>
[StructLayout(LayoutKind.Explicit)]
public unsafe struct AtkEventListenerVirtualTable
{
    /// <summary>The function that frees the thing.</summary>
    [FieldOffset(0x00)]
    public delegate* unmanaged<AtkUnitBase*, AtkUnitBase*> Dtor;

    /// <summary>The function that receives the events of the whole interface.</summary>
    [FieldOffset(0x08)]
    public delegate* unmanaged<AtkUnitBase*, AtkEventType, int, AtkEvent*, AtkEventData*, void> ReceiveGlobalEvent;

    /// <summary>The function that receives the events of the thing itself.</summary>
    [FieldOffset(0x10)]
    public delegate* unmanaged<AtkUnitBase*, AtkEventType, int, AtkEvent*, AtkEventData*, void> ReceiveEvent;
}

/// <summary>The data every component of the interface has.</summary>
[StructLayout(LayoutKind.Explicit, Size = 0xC0)]
public unsafe struct AtkComponentBase
{
    /// <summary>The table of the virtual functions of the component.</summary>
    [FieldOffset(0x00)]
    public void* VirtualTable;

    /// <summary>The data of the window the component belongs to.</summary>
    [FieldOffset(0x08)]
    public AtkUldManager UldManager;

    /// <summary>The flags of the component.</summary>
    [FieldOffset(0x98)]
    public uint ComponentFlags;

    /// <summary>The node of the component.</summary>
    [FieldOffset(0xA0)]
    public AtkResNode* AtkResNode;

    /// <summary>The node that holds the component.</summary>
    [FieldOffset(0xA8)]
    public AtkComponentNode* OwnerNode;
}

/// <summary>A button of the interface.</summary>
[StructLayout(LayoutKind.Explicit, Size = 0xF0)]
public unsafe struct AtkComponentButton
{
    /// <summary>The data every component of the interface has.</summary>
    [FieldOffset(0x00)]
    public AtkComponentBase AtkComponentBase;

    /// <summary>The left edge of the button.</summary>
    [FieldOffset(0xC0)]
    public short Left;

    /// <summary>The top edge of the button.</summary>
    [FieldOffset(0xC2)]
    public short Top;

    /// <summary>The right edge of the button.</summary>
    [FieldOffset(0xC4)]
    public short Right;

    /// <summary>The bottom edge of the button.</summary>
    [FieldOffset(0xC6)]
    public short Bottom;

    /// <summary>Gets a value indicating whether the button can be clicked.</summary>
    public bool IsEnabled
    {
        get
        {
            var owner = AtkComponentBase.OwnerNode;
            if (owner is null)
            {
                return false;
            }

            return (owner->AtkResNode.NodeFlags & NodeFlags.Enabled) != 0;
        }
    }
}

/// <summary>A list of the interface.</summary>
[StructLayout(LayoutKind.Explicit, Size = 0x178)]
public unsafe struct AtkComponentList
{
    /// <summary>The data every component of the interface has.</summary>
    [FieldOffset(0x00)]
    public AtkComponentBase AtkComponentBase;
}

/// <summary>A window of the interface.</summary>
[StructLayout(LayoutKind.Explicit, Size = 0x238)]
public unsafe struct AtkUnitBase
{
    /// <summary>The table of the virtual functions of the window.</summary>
    [FieldOffset(0x00)]
    public AtkEventListenerVirtualTable* VirtualTable;

    /// <summary>The number of bytes the name of the window takes.</summary>
    private const int NameByteCount = 32;

    /// <summary>The name of the window, as a run of bytes that end with a zero.</summary>
    [FieldOffset(0x08)]
    public fixed byte NameBytes[NameByteCount];

    /// <summary>The data of the window that is loaded from a file.</summary>
    [FieldOffset(0x28)]
    public AtkUldManager UldManager;

    /// <summary>The number of the window.</summary>
    [FieldOffset(0x1E4)]
    public ushort Id;

    /// <summary>Receives an event of the interface.</summary>
    /// <param name="eventType">The kind of the event.</param>
    /// <param name="eventParam">The extra number of the event.</param>
    /// <param name="atkEvent">The event itself.</param>
    /// <param name="atkEventData">The data of the event.</param>
    public void ReceiveEvent(AtkEventType eventType, int eventParam, AtkEvent* atkEvent, AtkEventData* atkEventData = null)
    {
        if (VirtualTable is null)
        {
            return;
        }

        VirtualTable->ReceiveEvent(StructPtr.Of(ref this), eventType, eventParam, atkEvent, atkEventData);
    }

    /// <summary>
    /// Tells the window that something happened, the way a button would.
    /// </summary>
    /// <param name="valueCount">The number of the values.</param>
    /// <param name="values">The values themselves.</param>
    /// <param name="close">Whether the window closes afterwards.</param>
    /// <returns><c>true</c> when the window took the values.</returns>
    public bool FireCallback(uint valueCount, AtkValue* values, bool close = false)
    {
        var function = (delegate* unmanaged<AtkUnitBase*, uint, AtkValue*, bool, bool>)
            GameFunctions.Get(GameAddresses.AtkUnitBaseFireCallback);

        return function(StructPtr.Of(ref this), valueCount, values, close);
    }
}

/// <summary>The window that asks the player to confirm a refinement.</summary>
[StructLayout(LayoutKind.Explicit, Size = 0x260)]
public unsafe struct AddonMaterializeDialog
{
    /// <summary>The window itself.</summary>
    [FieldOffset(0x00)]
    public AtkUnitBase AtkUnitBase;

    /// <summary>The button that confirms.</summary>
    [FieldOffset(0x250)]
    public AtkComponentButton* YesButton;

    /// <summary>The button that gives up.</summary>
    [FieldOffset(0x258)]
    public AtkComponentButton* NoButton;
}
