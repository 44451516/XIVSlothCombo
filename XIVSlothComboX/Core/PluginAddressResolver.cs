using System;
using Dalamud.Game;
using FFXIVClientStructs.FFXIV.Client.Game;
using XIVSlothComboX.Services;

namespace XIVSlothComboX.Core
{
    /// <summary> Plugin address resolver. </summary>
    internal class PluginAddressResolver
    {
        /// <summary> Gets the address of fpIsIconReplacable. </summary>
        public IntPtr IsActionIdReplaceable { get; private set; }

        /// <inheritdoc/>
        public unsafe void Setup(ISigScanner scanner)
        {
            
            IsActionIdReplaceable = scanner.ScanText(HookAddress.ActionIdReplaceable);

            Service.PluginLog.Verbose("===== X I V S L O T H C O M B O =====");
            
            Service.PluginLog.Error($"{nameof(IsActionIdReplaceable)} 0x{IsActionIdReplaceable:X}");
        }
    }
}
