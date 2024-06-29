using System;
using Dalamud.Game.ClientState.Objects.Types;
using FFXIVClientStructs.FFXIV.Client.Game.Object;
using FFXIVClientStructs.FFXIV.Client.System.Framework;
using XIVSlothComboX.Core;

namespace XIVSlothComboX.Services
{
    public static unsafe class PartyTargetingService
    {
        private static readonly IntPtr pronounModule = (IntPtr)Framework.Instance()->GetUIModule()->GetPronounModule();
        public static GameObject* UITarget => (GameObject*)*(IntPtr*)(pronounModule + 0x290);

        public static ulong GetObjectID(FFXIVClientStructs.FFXIV.Client.Game.Object.GameObject* o)
        {
            // var id = o->GetObjectID();
            var id = o->GetGameObjectId();
            return (ulong)((id.Type * 0x1_0000_0000) | id.ObjectId);
        }

        private static readonly delegate* unmanaged<long, GameObject*> getGameObjectFromObjectID = (delegate* unmanaged<long, GameObject*>)Service.SigScanner.ScanText(HookAddress.GetGameObjectFromObjectID);
        public static GameObject* GetGameObjectFromObjectID(long id) => getGameObjectFromObjectID(id);

        private static readonly delegate* unmanaged<IntPtr, uint, GameObject*> getGameObjectFromPronounID = (delegate* unmanaged<IntPtr, uint, GameObject*>)Service.SigScanner.ScanText(HookAddress.GetGameObjectFromPronounID);
        public static GameObject* GetGameObjectFromPronounID(uint id) => getGameObjectFromPronounID(pronounModule, id);
    }
}
