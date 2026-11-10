using SlothCombo.Abstractions.GameData;

namespace SlothCombo.Abstractions;

/// <summary>
/// The static game data of the client.
/// </summary>
/// <remarks>
/// <para>
/// This is the replacement for the Dalamud data manager. The tables of the
/// game are read from the files that are carried along with the program, so
/// the combo logic keeps reading the data of the game the way it did before.
/// </para>
/// <para>
/// The files of the game are not needed, so the program also works on a
/// machine that does not have the game installed.
/// </para>
/// </remarks>
public interface IDataManager
{
    /// <summary>Gets the table of the given kind of row.</summary>
    /// <typeparam name="T">The kind of the rows of the table.</typeparam>
    /// <returns>The table.</returns>
    IGameTable<T> GetTable<T>()
        where T : GameRow, new();

    /// <summary>Gets the language of the game data.</summary>
    ClientLanguage Language { get; }
}
