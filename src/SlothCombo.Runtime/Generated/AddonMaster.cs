using System;
using FFXIVClientStructs.FFXIV.Client.UI;
using FFXIVClientStructs.FFXIV.Component.GUI;

namespace SlothCombo.Generated;

/// <summary>
/// Helpers for the in-game windows the plugin drives.
/// </summary>
/// <remarks>
/// This is the replacement for the addon helpers of ECommons. Only the
/// windows the plugin actually drives are covered.
/// </remarks>
internal static unsafe class AddonMaster
{
    /// <summary>The dialog that asks the player to confirm a materia extraction.</summary>
    internal sealed class MaterializeDialog
    {
        private readonly IntPtr _addon;

        /// <summary>Wraps the addon with the given address.</summary>
        /// <param name="addon">The address of the addon.</param>
        public MaterializeDialog(IntPtr addon) => _addon = addon;

        /// <summary>Confirms the extraction.</summary>
        public void Materialize()
        {
            var addon = (AddonMaterializeDialog*)_addon;
            ClickButtonIfEnabled(addon->YesButton, &addon->AtkUnitBase);
        }
    }

    /// <summary>Clicks the given button when it can be clicked.</summary>
    /// <param name="button">The button to click.</param>
    /// <param name="addon">The addon the button belongs to.</param>
    private static void ClickButtonIfEnabled(AtkComponentButton* button, AtkUnitBase* addon)
    {
        if (button is null || addon is null)
        {
            return;
        }

        if (!button->IsEnabled)
        {
            return;
        }

        var node = button->AtkComponentBase.OwnerNode->AtkResNode;
        if ((node.NodeFlags & NodeFlags.Visible) == 0)
        {
            return;
        }

        var evt = (AtkEvent*)node.AtkEventManager.Event;
        if (evt is null)
        {
            return;
        }

        addon->ReceiveEvent(evt->State.EventType, (int)evt->Param, node.AtkEventManager.Event);
    }
}
