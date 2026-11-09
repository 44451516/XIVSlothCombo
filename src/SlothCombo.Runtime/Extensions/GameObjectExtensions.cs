using SlothCombo.Abstractions;
using SlothCombo.CustomComboNS.Functions;

namespace SlothCombo.Extensions;

/// <summary>
/// Helpers for the objects of the game world.
/// </summary>
internal static class GameObjectExtensions
{
    /// <summary>
    /// Gets the combat role of the object.
    /// </summary>
    /// <param name="obj">The object to check.</param>
    /// <returns>The combat role of the job of the object.</returns>
    /// <remarks>
    /// The role is taken from the job lists the combo logic already keeps,
    /// so the answer stays in line with the rotations of the plugin.
    /// </remarks>
    public static CombatRole GetRole(this IGameObject obj)
    {
        var job = (byte)obj.ClassJob.RowId;

        if (CustomComboFunctions.JobIDs.Tank.Contains(job))
        {
            return CombatRole.Tank;
        }

        if (CustomComboFunctions.JobIDs.Healer.Contains(job))
        {
            return CombatRole.Healer;
        }

        if (CustomComboFunctions.JobIDs.Melee.Contains(job)
            || CustomComboFunctions.JobIDs.Ranged.Contains(job))
        {
            return CombatRole.Dps;
        }

        return CombatRole.None;
    }
}
