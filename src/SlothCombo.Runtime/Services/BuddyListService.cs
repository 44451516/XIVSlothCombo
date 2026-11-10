using System;
using SlothCombo.Interop.Game;
using SlothCombo.Abstractions;

namespace SlothCombo.Runtime.Services;

/// <summary>
/// The pets of the player, backed by the struct library.
/// </summary>
/// <remarks>
/// The game keeps the summoned pet and the companion of the player in the
/// same slot, but tells them apart by the kind of the companion.
/// </remarks>
internal sealed unsafe class BuddyListService : IBuddyList
{
    /// <inheritdoc/>
    public bool HasPet
    {
        get
        {
            var companion = GetCompanion();
            return companion is not null && companion->CompanionType == CompanionType.Companion;
        }
    }

    /// <inheritdoc/>
    public bool HasCompanion
    {
        get
        {
            var companion = GetCompanion();
            return companion is not null && companion->CompanionType == CompanionType.FollowMount;
        }
    }

    /// <summary>Gets the companion object of the player, or <c>null</c>.</summary>
    private static Companion* GetCompanion()
    {
        var manager = ClientObjectManager.Instance();
        if (manager is null)
        {
            return null;
        }

        var battleCharas = manager->BattleCharas;
        if (battleCharas is null)
        {
            return null;
        }

        var player = battleCharas[0].BattleChara;
        if (player is null)
        {
            return null;
        }

        return player->Character.CompanionData.CompanionObject;
    }
}
