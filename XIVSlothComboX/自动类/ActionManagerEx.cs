using ECommons.DalamudServices;
using FFXIVClientStructs.FFXIV.Client.Game;
using FFXIVClientStructs.FFXIV.Client.UI;

namespace Artisan.GameInterop;

// wrapper around in-game action manager that provides nicer api + use-action hooking
public static unsafe class ActionManagerEx
{
    public static float AnimationLock => ((float*)ActionManager.Instance())[2];

    public static bool CanUseAction(ActionType actionType, uint actionID) => ActionManager.Instance()->GetActionStatus(actionType, actionID) == 0;

   

    
    
    public static bool UseItem(uint ItemId) => ActionManager.Instance()->UseAction(ActionType.Item, ItemId, extraParam: 65535);
    public static bool UseRepair() => ActionManager.Instance()->UseAction(ActionType.GeneralAction, 6);
    public static bool UseMateriaExtraction() => ActionManager.Instance()->UseAction(ActionType.GeneralAction, 14);
}
