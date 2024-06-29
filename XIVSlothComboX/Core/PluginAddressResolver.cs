using System;
using Dalamud.Game;
using FFXIVClientStructs.FFXIV.Client.Game;
using XIVSlothComboX.Services;

namespace XIVSlothComboX.Core
{
    /// <summary> Plugin address resolver. </summary>
    internal class PluginAddressResolver
    {
        /// <summary> Gets the address of the member ComboTimer. </summary>
        public IntPtr ComboTimer { get; private set; }

        /// <summary> Gets the address of the member LastComboMove. </summary>
        public IntPtr LastComboMove => ComboTimer + 0x4;

        /// <summary> Gets the address of fpIsIconReplacable. </summary>
        public IntPtr IsActionIdReplaceable { get; private set; }

        /// <inheritdoc/>
        public unsafe void Setup(ISigScanner scanner)
        {
            ComboTimer = new IntPtr(&ActionManager.Instance()->Combo.Timer);

            IsActionIdReplaceable = scanner.ScanText(HookAddress.ActionIdReplaceable);

            Service.PluginLog.Verbose("===== X I V S L O T H C O M B O =====");
            
            Service.PluginLog.Error($"{nameof(IsActionIdReplaceable)} 0x{IsActionIdReplaceable:X}");
            Service.PluginLog.Error($"{nameof(ComboTimer)}            0x{ComboTimer:X}");
            Service.PluginLog.Error($"{nameof(LastComboMove)}         0x{LastComboMove:X}");
        }
    }
}
