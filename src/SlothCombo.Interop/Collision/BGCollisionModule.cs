using System;
using System.Numerics;
using System.Runtime.InteropServices;

namespace SlothCombo.Interop.Collision;

/// <summary>The place a ray of the game hit.</summary>
/// <remarks>
/// The place holds where the ray hit, the normal of the surface it hit and
/// the distance the ray had travelled when it did.
/// </remarks>
[StructLayout(LayoutKind.Explicit, Size = 0x58)]
public struct RaycastHit
{
    /// <summary>The place the ray hit.</summary>
    [FieldOffset(0x00)]
    public Vector3 Point;

    /// <summary>The normal of the surface the ray hit.</summary>
    [FieldOffset(0x30)]
    public Vector3 Normal;

    /// <summary>The material of the surface the ray hit.</summary>
    [FieldOffset(0x40)]
    public ulong Material;

    /// <summary>The distance the ray had travelled when it hit.</summary>
    [FieldOffset(0x48)]
    public float Distance;
}

/// <summary>The collisions of the world of the game.</summary>
/// <remarks>
/// The module holds the scene of the world and shoots the rays that are used
/// to find out whether a place can be reached.
/// </remarks>
[StructLayout(LayoutKind.Explicit, Size = 0xA0)]
public unsafe struct BGCollisionModule
{
    /// <summary>Whether the module is shutting down.</summary>
    [FieldOffset(0x01)]
    public bool ShuttingDown;

    /// <summary>The scene of the world.</summary>
    [FieldOffset(0x10)]
    public void* SceneManager;

    /// <summary>
    /// Shoots a ray into the world and tells where it hit.
    /// </summary>
    /// <param name="hit">Receives the place the ray hit.</param>
    /// <param name="origin">The place the ray starts at.</param>
    /// <param name="direction">The way the ray goes.</param>
    /// <param name="maxDistance">How far the ray may travel.</param>
    /// <param name="layerMask">The layers the ray goes through.</param>
    /// <param name="flags">The settings of the way the ray is shot.</param>
    /// <returns><c>true</c> when the ray hit something.</returns>
    public bool RaycastMaterialFilter(RaycastHit* hit, Vector3* origin, Vector3* direction, float maxDistance, int layerMask, int* flags)
    {
        var function = (delegate* unmanaged<BGCollisionModule*, RaycastHit*, Vector3*, Vector3*, float, int, int*, bool>)
            GameFunctions.Get(GameAddresses.RaycastMaterialFilter);

        return function(StructPtr.Of(ref this), hit, origin, direction, maxDistance, layerMask, flags);
    }

    /// <summary>
    /// Shoots a ray into the world and tells where it hit.
    /// </summary>
    /// <param name="origin">The place the ray starts at.</param>
    /// <param name="direction">The way the ray goes.</param>
    /// <param name="hit">Receives the place the ray hit.</param>
    /// <param name="maxDistance">How far the ray may travel.</param>
    /// <returns><c>true</c> when the ray hit something.</returns>
    /// <remarks>
    /// The settings of the ray are the ones the game itself uses, so that a
    /// ray shot here hits the same things a ray of the game would hit.
    /// </remarks>
    public static bool RaycastMaterialFilter(Vector3 origin, Vector3 direction, out RaycastHit hit, float maxDistance = 1000000f)
    {
        var result = default(RaycastHit);

        var framework = UI.Framework.Instance();
        if (framework is null)
        {
            hit = result;
            return false;
        }

        var module = framework->BGCollisionModule;
        if (module is null)
        {
            hit = result;
            return false;
        }

        var flags = new int[] { 0x4000, 0, 0x4000, 0 };
        var points = new Vector3[] { origin, direction };

        fixed (int* flagPointer = flags)
        fixed (Vector3* pointPointer = points)
        {
            var hitPointer = &result;
            var found = module->RaycastMaterialFilter(
                hitPointer, pointPointer, pointPointer + 1, maxDistance, 1, flagPointer);
            hit = result;
            return found;
        }
    }
}
