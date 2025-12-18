using System.Collections.Generic;
using System.Linq;
using Artisan.GameInterop;
using ECommons.DalamudServices;
using FFXIVClientStructs.FFXIV.Client.Game;
using FFXIVClientStructs.FFXIV.Client.System.Framework;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using Lumina.Excel.Sheets;

namespace XIVSlothComboX.自动类
{
#pragma warning disable CS8604,CS8618,CS0649
    internal unsafe class ConsumableChecker
    {
        internal static (uint Id, string Name)[] Food;
        internal static (uint Id, string Name)[] Pots;
        internal static (uint Id, string Name)[] Manuals;
        internal static (uint Id, string Name)[] SquadronManuals;
        static Dictionary<uint, string> Usables;
        static AgentInterface* itemContextMenuAgent;

        internal static void Init()
        {
            itemContextMenuAgent =
                Framework.Instance()->UIModule->GetAgentModule()->GetAgentByInternalId(AgentId.InventoryContext);
            Usables = Svc.Data.GetExcelSheet<Item>().Where(i => i.ItemAction.RowId > 0)
                .ToDictionary(i => i.RowId, i => i.Name.ToString().ToLower())
                .Concat(Svc.Data.GetExcelSheet<EventItem>().Where(i => i.Action.RowId > 0)
                    .ToDictionary(i => i.RowId, i => i.Name.ToString().ToLower()))
                .ToDictionary(kv => kv.Key, kv => kv.Value);
            Food = Svc.Data.GetExcelSheet<Item>().Where(IsFood).Select(x => (x.RowId, x.Name.ToString())).ToArray();
            Pots = Svc.Data.GetExcelSheet<Item>().Where(IsCraftersPot).Select(x => (x.RowId, x.Name.ToString()))
                .ToArray();
            Manuals = Svc.Data.GetExcelSheet<Item>().Where(IsManual).Select(x => (x.RowId, x.Name.ToString()))
                .ToArray();
            SquadronManuals = Svc.Data.GetExcelSheet<Item>().Where(IsSquadronManual)
                .Select(x => (x.RowId, x.Name.ToString())).ToArray();
        }

        internal static (uint Id, string Name)[] GetFood(bool inventoryOnly = false, bool hq = false)
        {
            if (inventoryOnly)
                return Food.Where(x => InventoryManager.Instance()->GetInventoryItemCount(x.Id, hq) > 0f).ToArray();

            return Food;
        }

        internal static (uint Id, string Name)[] GetPots(bool inventoryOnly = false, bool hq = false)
        {
            if (inventoryOnly)
                return Pots.Where(x => InventoryManager.Instance()->GetInventoryItemCount(x.Id, hq) > 0).ToArray();

            return Pots;
        }

        internal static (uint Id, string Name)[] GetManuals(bool inventoryOnly = false, bool hq = false)
        {
            if (inventoryOnly)
                return Manuals.Where(x => InventoryManager.Instance()->GetInventoryItemCount(x.Id, hq) > 0).ToArray();

            return Manuals;
        }

        internal static (uint Id, string Name)[] GetSquadronManuals(bool inventoryOnly = false, bool hq = false)
        {
            if (inventoryOnly)
                return SquadronManuals.Where(x => InventoryManager.Instance()->GetInventoryItemCount(x.Id, hq) > 0)
                    .ToArray();

            return SquadronManuals;
        }

        internal static ItemFood? GetItemConsumableProperties(Item item, bool hq)
        {
            var action = item.ItemAction.Value;

            // [0] = status, [1] = extra == ItemFood row, [2] = duration
            if (hq)
            {
                var actionParams = action.DataHQ; // Collection<ushort>
                if (actionParams[0] is not 48 and not 49)
                    return null; // not 'well fed' or 'medicated'
                return Svc.Data.GetExcelSheet<ItemFood>()?.GetRow(actionParams[1]);
            }
            else
            {
                var actionParams = action.Data; // Collection<ushort>
                if (actionParams[0] is not 48 and not 49)
                    return null; // not 'well fed' or 'medicated'
                return Svc.Data.GetExcelSheet<ItemFood>()?.GetRow(actionParams[1]);
            }
        }


        internal static bool IsFood(Item item)
        {
            if (item.ItemUICategory.RowId == 46)
            {
                var consumable = GetItemConsumableProperties(item, false);
                return consumable != null; // cp/craftsmanship/control
            }

            return false;
        }

        internal static bool IsCraftersFood(Item item)
        {
            if (item.ItemUICategory.RowId == 45 || item.ItemUICategory.RowId == 46)
            {
                var consumable = GetItemConsumableProperties(item, false);
                return consumable != null; // cp/craftsmanship/control
            }

            return false;
        }

        internal static bool IsCraftersPot(Item item)
        {
            if (item.ItemUICategory.RowId != 44)
                return false; // not a 'medicine'

            var consumable = GetItemConsumableProperties(item, false);

            return consumable != null &&
                   consumable.Value.Params.Any(p =>
                       p.BaseParam.RowId is 11 or 70 or 71 or 69
                           or 68); // cp/craftsmanship/control/increased spiritbond/reduced durability loss
        }

        internal static bool IsManual(Item item)
        {
            if (item.ItemUICategory.RowId != 63)
                return false; // not 'other'

            var action = item.ItemAction.Value;
            return action.RowId == 816 && action.Data[0] is 300 or 301 or 1751 or 5329;
        }

        internal static bool IsSquadronManual(Item item)
        {
            if (item.ItemUICategory.RowId != 63)
                return false; // not 'other'

            var action = item.ItemAction.Value;
            return action.RowId == 816 && action.Data[0] is 2291 or 2292 or 2293 or 2294;
        }


        internal static bool UseItem(uint id, bool hq = false)
        {
            if (Throttler.Throttle(2000))
            {
                if (hq)
                {
                    return UseItem2(id + 1_000_000);
                }
                else
                {
                    return UseItem2(id);
                }
            }

            return false;
        }

        internal static unsafe bool UseItem2(uint ItemId) => ActionManagerEx.UseItem(ItemId);


        internal static bool HasItem(uint requiredItem, bool requiredItemHQ)
        {
            if (requiredItem == 0) return true;

            return InventoryManager.Instance()->GetInventoryItemCount(requiredItem, requiredItemHQ) > 0;
        }
    }
}