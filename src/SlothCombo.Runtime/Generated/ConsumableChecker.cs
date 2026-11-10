using System.Collections.Generic;
using System.Linq;
using Artisan.GameInterop;
using SlothCombo.Runtime.Services;
using SlothCombo.Interop.Game;
using SlothCombo.Interop.UI;
using ItemRow = SlothCombo.Abstractions.GameData.ItemRow;
using EventItemRow = SlothCombo.Abstractions.GameData.EventItemRow;
using ItemActionRow = SlothCombo.Abstractions.GameData.ItemActionRow;
using ItemFoodRow = SlothCombo.Abstractions.GameData.ItemFoodRow;

namespace SlothCombo.Generated
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
            Usables = Svc.Data.GetTable<ItemRow>().Rows.Where(i => i.ItemAction > 0)
                .ToDictionary(i => i.RowId, i => i.Name.ToLower())
                .Concat(Svc.Data.GetTable<EventItemRow>().Rows.Where(i => i.Action > 0)
                    .ToDictionary(i => i.RowId, i => i.Name.ToLower()))
                .ToDictionary(kv => kv.Key, kv => kv.Value);
            Food = Svc.Data.GetTable<ItemRow>().Rows.Where(IsFood).Select(x => (x.RowId, x.Name)).ToArray();
            Pots = Svc.Data.GetTable<ItemRow>().Rows.Where(IsCraftersPot).Select(x => (x.RowId, x.Name))
                .ToArray();
            Manuals = Svc.Data.GetTable<ItemRow>().Rows.Where(IsManual).Select(x => (x.RowId, x.Name))
                .ToArray();
            SquadronManuals = Svc.Data.GetTable<ItemRow>().Rows.Where(IsSquadronManual)
                .Select(x => (x.RowId, x.Name)).ToArray();
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

        internal static ItemFoodRow? GetItemConsumableProperties(ItemRow item, bool hq)
        {
            var action = Svc.Data.GetTable<ItemActionRow>().GetRow(item.ItemAction);
            if (action is null)
            {
                return null;
            }

            // [0] = status, [1] = extra == ItemFood row, [2] = duration
            if (hq)
            {
                var actionParams = action.DataHq;
                if (actionParams.Length < 2 || actionParams[0] is not 48 and not 49)
                    return null; // not 'well fed' or 'medicated'
                return Svc.Data.GetTable<ItemFoodRow>().GetRow(actionParams[1]);
            }
            else
            {
                var actionParams = action.Data;
                if (actionParams.Length < 2 || actionParams[0] is not 48 and not 49)
                    return null; // not 'well fed' or 'medicated'
                return Svc.Data.GetTable<ItemFoodRow>().GetRow(actionParams[1]);
            }
        }


        internal static bool IsFood(ItemRow item)
        {
            if (item.ItemUICategory == 46)
            {
                var consumable = GetItemConsumableProperties(item, false);
                return consumable != null; // cp/craftsmanship/control
            }

            return false;
        }

        internal static bool IsCraftersFood(ItemRow item)
        {
            if (item.ItemUICategory is 45 or 46)
            {
                var consumable = GetItemConsumableProperties(item, false);
                return consumable != null; // cp/craftsmanship/control
            }

            return false;
        }

        internal static bool IsCraftersPot(ItemRow item)
        {
            if (item.ItemUICategory != 44)
                return false; // not a 'medicine'

            var consumable = GetItemConsumableProperties(item, false);

            return consumable != null &&
                   consumable.BaseParam.Any(p =>
                       p is 11 or 70 or 71 or 69
                           or 68); // cp/craftsmanship/control/increased spiritbond/reduced durability loss
        }

        internal static bool IsManual(ItemRow item)
        {
            if (item.ItemUICategory != 63)
                return false; // not 'other'

            var action = Svc.Data.GetTable<ItemActionRow>().GetRow(item.ItemAction);
            return action is not null && action.Data.Length > 0 && action.RowId == 816 && action.Data[0] is 300 or 301 or 1751 or 5329;
        }

        internal static bool IsSquadronManual(ItemRow item)
        {
            if (item.ItemUICategory != 63)
                return false; // not 'other'

            var action = Svc.Data.GetTable<ItemActionRow>().GetRow(item.ItemAction);
            return action is not null && action.Data.Length > 0 && action.RowId == 816 && action.Data[0] is 2291 or 2292 or 2293 or 2294;
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