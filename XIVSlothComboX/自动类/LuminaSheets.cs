using System;
using System.Collections.Generic;
using System.Linq;
using Dalamud.Game;
using Dalamud.Game.Text.SeStringHandling;
using ECommons;
using ECommons.DalamudServices;
using FFXIVClientStructs.FFXIV.Client.Game;
using Lumina.Data;
using Lumina.Excel.Sheets;
using Action = Lumina.Excel.Sheets.Action;
using Status = Lumina.Excel.Sheets.Status;

namespace XIVSlothComboX.自动类
{
    public class LuminaSheets
    {

        public static Dictionary<uint, Recipe>? RecipeSheet;

        public static Dictionary<uint, GatheringItem>? GatheringItemSheet;

        public static Dictionary<uint, SpearfishingItem>? SpearfishingItemSheet;

        public static Dictionary<uint, GatheringPointBase>? GatheringPointBaseSheet;

        public static Dictionary<uint, FishParameter>? FishParameterSheet;

        public static Dictionary<uint, ClassJob>? ClassJobSheet;

        public static Dictionary<uint, Item>? ItemSheet;

        public static Dictionary<uint, Action>? ActionSheet;

        public static Dictionary<uint, Status>? StatusSheet;

        public static Dictionary<uint, CraftAction>? CraftActions;

        // public static Dictionary<uint, CraftLevelDifference>? CraftLevelDifference;

        public static Dictionary<uint, RecipeLevelTable>? RecipeLevelTableSheet;

        public static Dictionary<uint, Addon>? AddonSheet;

        public static Dictionary<uint, SpecialShop>? SpecialShopSheet;

        public static Dictionary<uint, LogMessage>? LogMessageSheet;

        public static Dictionary<uint, ItemFood>? ItemFoodSheet;

        public static Dictionary<uint, ENpcResident>? ENPCResidentSheet;

        public static Dictionary<uint, Quest>? QuestSheet;

        public static Dictionary<uint, CompanyCraftPart>? WorkshopPartSheet;

        public static Dictionary<uint, CompanyCraftProcess>? WorkshopProcessSheet;

        public static Dictionary<uint, CompanyCraftSequence>? WorkshopSequenceSheet;

        public static Dictionary<uint, CompanyCraftSupplyItem>? WorkshopSupplyItemSheet;

        public static void Init()
        {
            RecipeSheet = Svc.Data?.GetExcelSheet<Recipe>()?
           .Where(x => x.ItemResult.RowId > 0)
                .DistinctBy(x => x.RowId)
                .OrderBy(x => x.RecipeLevelTable.Value.ClassJobLevel)
                .ThenBy(x => x.ItemResult.Value.Name.ToString())
                .ToDictionary(x => x.RowId, x => x);

            GatheringItemSheet = Svc.Data?.GetExcelSheet<GatheringItem>()?
                .Where(x => x.GatheringItemLevel.Value.GatheringItemLevel > 0)
                .ToDictionary(i => i.RowId, i => i);

            SpearfishingItemSheet = Svc.Data?.GetExcelSheet<SpearfishingItem>()?
                .Where(x => x.GatheringItemLevel.Value.GatheringItemLevel > 0)
                .ToDictionary(i => i.RowId, i => i);

            GatheringPointBaseSheet = Svc.Data?.GetExcelSheet<GatheringPointBase>()?
               .Where(x => x.GatheringLevel > 0)
               .ToDictionary(i => i.RowId, i => i);

            FishParameterSheet = Svc.Data?.GetExcelSheet<FishParameter>()?
                 .Where(x => x.GatheringItemLevel.Value.GatheringItemLevel > 0)
                 .ToDictionary(i => i.RowId, i => i);

            ClassJobSheet = Svc.Data?.GetExcelSheet<ClassJob>()?
                       .ToDictionary(i => i.RowId, i => i);

            ItemSheet = Svc.Data?.GetExcelSheet<Item>()?
                       .ToDictionary(i => i.RowId, i => i);

            ActionSheet = Svc.Data?.GetExcelSheet<Action>()?
                        .ToDictionary(i => i.RowId, i => i);

            StatusSheet = Svc.Data?.GetExcelSheet<Status>()?
                       .ToDictionary(i => i.RowId, i => i);

            CraftActions = Svc.Data?.GetExcelSheet<CraftAction>()?
                       .ToDictionary(i => i.RowId, i => i);

            // CraftLevelDifference = Svc.Data?.GetExcelSheet<CraftLevelDifference>()?.ToDictionary(i => i.RowId, i => i);

            RecipeLevelTableSheet = Svc.Data?.GetExcelSheet<RecipeLevelTable>()?
                       .ToDictionary(i => i.RowId, i => i);

            AddonSheet = Svc.Data?.GetExcelSheet<Addon>()?
                       .ToDictionary(i => i.RowId, i => i);

            SpecialShopSheet = Svc.Data?.GetExcelSheet<SpecialShop>()?
                       .ToDictionary(i => i.RowId, i => i);

            LogMessageSheet = Svc.Data?.GetExcelSheet<LogMessage>()?
                       .ToDictionary(i => i.RowId, i => i);

            ItemFoodSheet = Svc.Data?.GetExcelSheet<ItemFood>()?
                       .ToDictionary(i => i.RowId, i => i);

            ENPCResidentSheet = Svc.Data?.GetExcelSheet<ENpcResident>()?
                       .Where(x => x.Singular.ExtractText().Length > 0)
                       .ToDictionary(i => i.RowId, i => i);

            QuestSheet = Svc.Data?.GetExcelSheet<Quest>()?
                        .Where(x => x.Id.ExtractText().Length > 0)
                        .ToDictionary(i => i.RowId, i => i);

            WorkshopPartSheet = Svc.Data?.GetExcelSheet<CompanyCraftPart>()?
                       .ToDictionary(i => i.RowId, i => i);

            WorkshopProcessSheet = Svc.Data?.GetExcelSheet<CompanyCraftProcess>()?
                       .ToDictionary(i => i.RowId, i => i);

            WorkshopSequenceSheet = Svc.Data?.GetExcelSheet<CompanyCraftSequence>()?
                       .ToDictionary(i => i.RowId, i => i);

            WorkshopSupplyItemSheet = Svc.Data?.GetExcelSheet<CompanyCraftSupplyItem>()?
                       .ToDictionary(i => i.RowId, i => i);

            Svc.Log.Debug("Lumina sheets initialized");
        }

        public static void Dispose()
        {
            var type = typeof(LuminaSheets);
            foreach (var prop in type.GetFields(System.Reflection.BindingFlags.Static))
            {
                prop.SetValue(null, null);
            }
        }
    }

  
}
