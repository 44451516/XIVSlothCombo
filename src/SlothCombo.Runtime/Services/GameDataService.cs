using System;
using System.IO;
using SlothCombo.Abstractions;
using SlothCombo.Abstractions.GameData;
using SlothCombo.GameData;

namespace SlothCombo.Runtime.Services;

/// <summary>
/// The static game data, read from the tables that are carried along with the
/// program.
/// </summary>
/// <remarks>
/// <para>
/// The tables are written when the program is built and are read when it
/// starts. The files of the game are not needed, so the program also works
/// on a machine that does not have the game installed.
/// </para>
/// <para>
/// The status structs of the game only carry the id of the effect, so the
/// names are resolved through the table of the status effects.
/// </para>
/// </remarks>
internal sealed class GameDataService : IDataManager, IDisposable
{
    private readonly GameDataStore _store;

    /// <summary>
    /// Reads the tables of the given language.
    /// </summary>
    /// <param name="rootFolder">The folder that holds the tables.</param>
    /// <param name="language">The language of the tables.</param>
    public GameDataService(string rootFolder, ClientLanguage language)
    {
        ArgumentException.ThrowIfNullOrEmpty(rootFolder);

        _store = new GameDataStore(rootFolder, language);
        StatusSheetLookup.Register(ResolveStatusName);
    }

    /// <inheritdoc/>
    public IGameTable<T> GetTable<T>()
        where T : GameRow, new() => _store.GetTable<T>();

    /// <inheritdoc/>
    public ClientLanguage Language => _store.Language;

    /// <summary>
    /// Works out the folder that holds the tables.
    /// </summary>
    /// <remarks>
    /// The tables are carried along with the assembly of the runtime, so they
    /// sit in the folder that holds that assembly.
    /// </remarks>
    /// <returns>The folder that holds the tables.</returns>
    public static string ResolveRootFolder()
    {
        var location = typeof(GameDataService).Assembly.Location;
        var folder = Path.GetDirectoryName(location);
        if (string.IsNullOrEmpty(folder))
        {
            throw new InvalidOperationException(
                "The folder of the runtime could not be found. " +
                "It is needed to read the static game data.");
        }

        return folder;
    }

    /// <summary>Resolves the name of the status with the given id.</summary>
    /// <param name="statusId">The id of the status.</param>
    /// <returns>The name of the status, or an empty string.</returns>
    private string ResolveStatusName(uint statusId)
    {
        try
        {
            var row = _store.GetTable<StatusRow>().GetRow(statusId);
            return row?.Name ?? string.Empty;
        }
        catch (Exception)
        {
            return string.Empty;
        }
    }

    /// <summary>Releases the tables when the runtime goes away.</summary>
    public void Dispose() => StatusSheetLookup.Unregister();
}
