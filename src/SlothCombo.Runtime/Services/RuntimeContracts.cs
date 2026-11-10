namespace SlothCombo.Runtime.Services;

/// <summary>
/// The pets and the companions of the player.
/// </summary>
/// <remarks>
/// The game keeps the summoned pet and the companion of the player in the same
/// slot, but tells them apart by the kind of the companion. The combo logic
/// needs to know which of the two is there, so the two are told apart here.
/// </remarks>
internal interface IBuddyList
{
    /// <summary>Gets a value indicating whether the player has a summoned pet.</summary>
    bool HasPet { get; }

    /// <summary>Gets a value indicating whether the player has a companion.</summary>
    bool HasCompanion { get; }
}

/// <summary>
/// The interface between the runtime and the outside world.
/// </summary>
/// <remarks>
/// This is the replacement for the interface of the plugin host. The runtime
/// lives in the game process, so the location of the assembly is the location
/// of the runtime itself and the configuration is written next to it.
/// </remarks>
internal interface IPluginInterface
{
    /// <summary>Gets the location of the assembly of the runtime.</summary>
    string AssemblyLocation { get; }

    /// <summary>Writes the given configuration to the outside world.</summary>
    /// <param name="configuration">The configuration to write.</param>
    void SavePluginConfig(object configuration);
}
