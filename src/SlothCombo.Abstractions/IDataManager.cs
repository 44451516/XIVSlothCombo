using System;
using Lumina.Excel;

namespace SlothCombo.Abstractions;

/// <summary>
/// The static game data of the client.
/// </summary>
/// <remarks>
/// This is the replacement for the Dalamud data manager. It hands out the
/// Excel sheets of the game directly, so the combo logic keeps reading the
/// sheets the way it did before.
/// </remarks>
public interface IDataManager
{
    /// <summary>Gets the Excel sheet of the given type.</summary>
    /// <typeparam name="T">The sheet type.</typeparam>
    /// <returns>The sheet.</returns>
    ExcelSheet<T> GetExcelSheet<T>() where T : struct, IExcelRow<T>;

    /// <summary>Gets the language of the game data.</summary>
    ClientLanguage Language { get; }
}

/// <summary>
/// The pets of the player.
/// </summary>
public interface IBuddyList
{
    /// <summary>Gets a value indicating whether the player has a pet summoned.</summary>
    bool HasPet { get; }

    /// <summary>Gets a value indicating whether the player has a companion out.</summary>
    bool HasCompanion { get; }
}

/// <summary>
/// The interface between the runtime and the outside world.
/// </summary>
/// <remarks>
/// This is the replacement for the Dalamud plugin interface. It carries the
/// few things the plugin needs from its host: where the assembly lives and how
/// the configuration is persisted.
/// </remarks>
public interface IPluginInterface
{
    /// <summary>Gets the location of the assembly of the runtime.</summary>
    string AssemblyLocation { get; }

    /// <summary>Saves the configuration of the plugin.</summary>
    /// <param name="configuration">The configuration to save.</param>
    void SavePluginConfig(object configuration);
}
