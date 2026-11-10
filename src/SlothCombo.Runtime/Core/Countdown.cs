using System.Runtime.InteropServices;
using SlothCombo.Interop.UI;

namespace SlothCombo.Core
{
    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct Countdown
    {
        [FieldOffset(0x28)] public float Timer;
        [FieldOffset(0x38)] public byte Active;
        [FieldOffset(0x3C)] public uint Initiator;

        public static unsafe Countdown* Instance => (Countdown*)Framework.Instance()->GetUIModule()->GetAgentModule()->GetAgentByInternalId(AgentId.CountDownSettingDialog);

        public static float? TimeRemaining()
        {
            var inst = Instance;
            return inst->Active != 0 ? inst->Timer : null;
        }
    }
}
