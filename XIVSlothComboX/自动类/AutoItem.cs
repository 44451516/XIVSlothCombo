using System.Linq;
using FFXIVClientStructs.FFXIV.Client.Game;
using XIVSlothComboX.Services;

namespace XIVSlothComboX.自动类;

public class AutoItem
{
    public static unsafe void Useitem(uint itemId)
    {
        uint a4 = 65535;

        //检查有没有HQ
        if (InventoryManager.Instance()->GetInventoryItemCount(itemId, true) > 0)
        {
            if (ActionManager.Instance()->GetActionStatus(ActionType.Item, itemId + 1000000) == 0)
            {
                ActionManager.Instance()->UseAction(ActionType.Item, itemId + 1000000, Service.ObjectTable.LocalPlayer.GameObjectId, a4);
                return;
            }
        }

        if (InventoryManager.Instance()->GetInventoryItemCount(itemId) > 0)
        {
            if (ActionManager.Instance()->GetActionStatus(ActionType.Item, itemId) == 0)
            {
                ActionManager.Instance()->UseAction(ActionType.Item, itemId, Service.ObjectTable.LocalPlayer.GameObjectId, a4);
            }
        }
    }

    public static void 自动吃食物(uint requiredItem, bool requiredItemHQ)
    {
        var LocalPlayer = Service.ObjectTable.LocalPlayer;
        if (LocalPlayer == null)
        {
            return;
        }

        var hasBuff = LocalPlayer.StatusList.FirstOrDefault(x => x.StatusId == 48 & x.RemainingTime > 10f);

        if (hasBuff == null)
        {
            if (ConsumableChecker.HasItem(requiredItem, requiredItemHQ))
            {
                ConsumableChecker.UseItem(requiredItem, requiredItemHQ);
            }
        }
    }

    public static unsafe void 自动精炼药()
    {
        var LocalPlayer = Service.ObjectTable.LocalPlayer;
        if (LocalPlayer == null)
        {
            return;
        }

        bool 是否有食物buff = false;

        foreach (var statuse in LocalPlayer.StatusList)
        {
            if (statuse.StatusId == 49)
            {
                是否有食物buff = true;
            }
        }

        if (是否有食物buff == false)
        {

            const uint 极精炼药 = 27960;
            const uint 强精炼药 = 19885;
            const uint 精炼药 = 7059;


            uint itemId = 0;

            if (InventoryManager.Instance()->GetInventoryItemCount(精炼药, true) > 0)
            {
                itemId = 精炼药;
            }

            if (InventoryManager.Instance()->GetInventoryItemCount(精炼药, false) > 0)
            {
                itemId = 精炼药;
            }


            if (InventoryManager.Instance()->GetInventoryItemCount(强精炼药, true) > 0)
            {
                itemId = 强精炼药;
            }

            if (InventoryManager.Instance()->GetInventoryItemCount(强精炼药, false) > 0)
            {
                itemId = 强精炼药;
            }


            //极精炼药HQ
            if (InventoryManager.Instance()->GetInventoryItemCount(极精炼药, true) > 0)
            {
                itemId = 极精炼药;
            }

            ///极精炼药NQ
            if (InventoryManager.Instance()->GetInventoryItemCount(极精炼药, false) > 0)
            {
                itemId = 极精炼药;
            }


            if (itemId > 0)
            {
                Useitem(itemId);
            }


        }
    }
}