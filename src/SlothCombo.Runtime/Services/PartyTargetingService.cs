using System;
using FFXIVClientStructs.FFXIV.Client.Game.Object;
using FFXIVClientStructs.FFXIV.Client.System.Framework;
using SlothCombo.Core;

namespace SlothCombo.Runtime.Services;

/// <summary>
/// The party targets of the game client.
/// </summary>
/// <remarks>
/// The game keeps the party members the player can target in the pronoun
/// module, which is not part of the object table.
/// </remarks>
internal static unsafe class PartyTargetingService
{
    private static readonly IntPtr pronounModule =
        (IntPtr)Framework.Instance()->GetUIModule()->GetPronounModule();

    /// <summary>Gets the object the party list hovers over.</summary>
    public static GameObject* UITarget => (GameObject*)*(IntPtr*)(pronounModule + 0x290);

    /// <summary>Gets the entity id of the given object.</summary>
    /// <param name="o">The object to read.</param>
    /// <returns>The entity id of the object.</returns>
    public static ulong GetObjectID(GameObject* o)
    {
        var id = o->GetGameObjectId();
        return id.ObjectId;
    }

    private static readonly delegate* unmanaged<IntPtr, uint, GameObject*> getGameObjectFromPronounID =
        (delegate* unmanaged<IntPtr, uint, GameObject*>)
        Svc.SigScanner.ScanText(HookAddress.GetGameObjectFromPronounID);

    /// <summary>Gets the object of the given pronoun id.</summary>
    /// <param name="id">The pronoun id.</param>
    /// <returns>The object, or <c>null</c>.</returns>
    public static GameObject* GetGameObjectFromPronounID(uint id)
        => getGameObjectFromPronounID(pronounModule, id);
}
