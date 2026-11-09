using System;
using SlothCombo.Abstractions;
using FFXIVClientStructs.FFXIV.Client.Game;
using SlothCombo.Runtime.Services;

namespace SlothCombo.Core
{
    /// <summary> Plugin address resolver. </summary>
    internal class PluginAddressResolver
    {
        /// <summary> Gets the address of fpIsIconReplacable. </summary>
        public IntPtr IsActionIdReplaceable { get; private set; }

        /// <inheritdoc/>
        public unsafe void Setup(ISignatureScanner scanner)
        {
            
            IsActionIdReplaceable = scanner.ScanText(HookAddress.ActionIdReplaceable);

            Service.PluginLog.Verbose("===== X I V S L O T H C O M B O =====");
            
            Service.PluginLog.Debug($"{nameof(IsActionIdReplaceable)} 0x{IsActionIdReplaceable:X}");
        }
    }
}
