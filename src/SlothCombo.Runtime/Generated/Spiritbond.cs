using SlothCombo.Runtime.Services;

using SlothCombo.Interop.Game;
using SlothCombo.Interop.UI;
using System;
using SlothCombo.Abstractions;

using SlothCombo.Generated;

namespace Artisan.RawInformation
{
    public unsafe static class Spiritbond
    {
        public static ushort Weapon { get => InventoryManager.Instance()->GetInventoryContainer(InventoryType.EquippedItems)->Items[0].SpiritbondOrCollectability; }

        public static ushort Offhand { get => InventoryManager.Instance()->GetInventoryContainer(InventoryType.EquippedItems)->Items[1].SpiritbondOrCollectability; }

        public static ushort Helm { get => InventoryManager.Instance()->GetInventoryContainer(InventoryType.EquippedItems)->Items[2].SpiritbondOrCollectability; }

        public static ushort Body { get => InventoryManager.Instance()->GetInventoryContainer(InventoryType.EquippedItems)->Items[3].SpiritbondOrCollectability; }

        public static ushort Hands { get => InventoryManager.Instance()->GetInventoryContainer(InventoryType.EquippedItems)->Items[4].SpiritbondOrCollectability; }

        public static ushort Legs { get => InventoryManager.Instance()->GetInventoryContainer(InventoryType.EquippedItems)->Items[6].SpiritbondOrCollectability; }

        public static ushort Feet { get => InventoryManager.Instance()->GetInventoryContainer(InventoryType.EquippedItems)->Items[7].SpiritbondOrCollectability; }

        public static ushort Earring { get => InventoryManager.Instance()->GetInventoryContainer(InventoryType.EquippedItems)->Items[8].SpiritbondOrCollectability; }

        public static ushort Neck { get => InventoryManager.Instance()->GetInventoryContainer(InventoryType.EquippedItems)->Items[9].SpiritbondOrCollectability; }

        public static ushort Wrist { get => InventoryManager.Instance()->GetInventoryContainer(InventoryType.EquippedItems)->Items[10].SpiritbondOrCollectability; }

        public static ushort Ring1 { get => InventoryManager.Instance()->GetInventoryContainer(InventoryType.EquippedItems)->Items[11].SpiritbondOrCollectability; }

        public static ushort Ring2 { get => InventoryManager.Instance()->GetInventoryContainer(InventoryType.EquippedItems)->Items[12].SpiritbondOrCollectability; }

        public static bool IsSpiritbondReadyAny()
        {
            if (Weapon == 10000) return true;
            if (Offhand == 10000) return true;
            if (Helm == 10000) return true;
            if (Body == 10000) return true;
            if (Hands == 10000) return true;
            if (Legs == 10000) return true;
            if (Feet == 10000) return true;
            if (Earring == 10000) return true;
            if (Neck == 10000) return true;
            if (Wrist == 10000) return true;
            if (Ring1 == 10000) return true;
            if (Ring2 == 10000) return true;

            return false;
        }
        public static bool UseMateriaExtraction() => ActionManager.Instance()->UseAction(ActionType.GeneralAction, 14);
        public static bool IsMateriaMenuOpen() => Svc.GameGui.GetAddonByName("Materialize", 1) != IntPtr.Zero;

        public static bool IsMateriaMenuDialogOpen() => Svc.GameGui.GetAddonByName("MaterializeDialog", 1) != IntPtr.Zero;
        public unsafe static void OpenMateriaMenu()
        {
            if (Svc.GameGui.GetAddonByName("Materialize", 1) == IntPtr.Zero)
            {
                UseMateriaExtraction();
            }
        }

        public unsafe static void CloseMateriaMenu()
        {
            if (Svc.GameGui.GetAddonByName("Materialize", 1) != IntPtr.Zero)
            {
                UseMateriaExtraction();
            }
        }

        public unsafe static void ConfirmMateriaDialog()
        {
            try
            {
                var materializePTR = Svc.GameGui.GetAddonByName("MaterializeDialog", 1);
                if (materializePTR == IntPtr.Zero)
                    return;

                var materalizeWindow = (AtkUnitBase*)materializePTR;
                if (materalizeWindow == null)
                    return;

                new AddonMaster.MaterializeDialog(materializePTR).Materialize();
            }
            catch
            {

            }
        }

        private unsafe static bool QuestUnlocked(int v)
        {
            return QuestManager.IsQuestComplete((uint)v);
        }

        public static bool MateriaExtractionUnlocked() => QuestUnlocked(66174);
        
        private static DateTime _nextRetry;
        
        public static bool Occupied()
        {
            return Svc.Condition[ConditionFlag.Occupied]
                   || Svc.Condition[ConditionFlag.Occupied30]
                   || Svc.Condition[ConditionFlag.Occupied33]
                   || Svc.Condition[ConditionFlag.Occupied38]
                   || Svc.Condition[ConditionFlag.Occupied39]
                   || Svc.Condition[ConditionFlag.OccupiedInCutSceneEvent]
                   || Svc.Condition[ConditionFlag.OccupiedInEvent]
                   || Svc.Condition[ConditionFlag.OccupiedInQuestEvent]
                   || Svc.Condition[ConditionFlag.OccupiedSummoningBell];
        }

        public unsafe static bool ExtractMateriaTask(bool option)
        {
            if (!MateriaExtractionUnlocked()) 
                return true;


            if (option)
            {
                if (IsMateriaMenuOpen() && !IsSpiritbondReadyAny())
                {
                    if (DateTime.Now < _nextRetry) return false;
                    CloseMateriaMenu();
                    _nextRetry = DateTime.Now.Add(TimeSpan.FromMilliseconds(500));
                    return false;
                }

                if (IsSpiritbondReadyAny())
                {
                    if (DateTime.Now < _nextRetry) return false;
                    if (!IsMateriaMenuOpen())
                    {
                        OpenMateriaMenu();
                        _nextRetry = DateTime.Now.Add(TimeSpan.FromMilliseconds(500));
                        return false;
                    }

                    if (IsMateriaMenuOpen() && !Occupied())
                    {
                        ExtractFirstMateria();
                        _nextRetry = DateTime.Now.Add(TimeSpan.FromMilliseconds(500));
                        return false;
                    }

                    _nextRetry = DateTime.Now.Add(TimeSpan.FromMilliseconds(500));
                    return false;
                }
            }

            return true;
        }

        public unsafe static void ExtractFirstMateria()
        {
            try
            {
                if (IsSpiritbondReadyAny())
                {
                    if (IsMateriaMenuDialogOpen())
                    {
                        ConfirmMateriaDialog();
                    }
                    else
                    {
                        var materializePTR = Svc.GameGui.GetAddonByName("Materialize", 1);
                        if (materializePTR == IntPtr.Zero)
                            return;

                        var materalizeWindow = (AtkUnitBase*)materializePTR;
                        if (materalizeWindow == null)
                            return;

                        var list = (AtkComponentList*)materalizeWindow->UldManager.NodeList[5];

                        var values = stackalloc AtkValue[2];
                        values[0] = new()
                        {
                            Type = SlothCombo.Interop.UI.AtkValueType.Int,
                            Int = 2,
                        };
                        values[1] = new()
                        {
                            Type = SlothCombo.Interop.UI.AtkValueType.UInt,
                            UInt = 0,
                        };

                        materalizeWindow->FireCallback(1, values);



                    }
                }


            }
            catch (Exception e)
            {
                Service.PluginLog.Error(e, "Failed to extract materia.");
            }
        }
    }
}
