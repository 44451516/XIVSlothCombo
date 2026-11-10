using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Text.Json;
using SlothCombo.Abstractions;
using SlothCombo.Abstractions.GameData;

namespace SlothCombo.GameData;

/// <summary>
/// The tables of the game, read from the files that are carried along with
/// the program.
/// </summary>
/// <remarks>
/// <para>
/// The tables are written once, when the program is built, and are read when
/// it starts. The files of the game are not needed, so the program also works
/// on a machine that does not have the game installed.
/// </para>
/// <para>
/// The files live in a folder per language. The language of the client is
/// known when the program starts, so the folder of that language is read.
/// </para>
/// </remarks>
public sealed class GameDataStore : IDataManager
{
    /// <summary>The name of the folder that holds the tables.</summary>
    public const string FolderName = "GameData";

    /// <summary>The ending of the files that hold the tables.</summary>
    public const string TableExtension = ".json.gz";

    /// <summary>The language that is read when the language of the client is not known.</summary>
    private const string FallbackLanguage = "en";

    private static readonly JsonSerializerOptions ReadOptions = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    private readonly Dictionary<string, object> _tables = new(StringComparer.Ordinal);
    private readonly string _folder;

    /// <summary>
    /// Reads the tables of the given language.
    /// </summary>
    /// <param name="rootFolder">The folder that holds the tables.</param>
    /// <param name="language">The language of the tables.</param>
    /// <exception cref="DirectoryNotFoundException">The tables are not there.</exception>
    public GameDataStore(string rootFolder, ClientLanguage language)
    {
        ArgumentException.ThrowIfNullOrEmpty(rootFolder);

        _folder = ResolveFolder(rootFolder, language);
        Language = language;
    }
    /// <inheritdoc/>
    public ClientLanguage Language { get; }

    /// <summary>
    /// Gets the table of the given kind of row.
    /// </summary>
    /// <typeparam name="T">The kind of the rows of the table.</typeparam>
    /// <returns>The table.</returns>
    public IGameTable<T> GetTable<T>()
        where T : GameRow, new()
    {
        var name = TableName<T>();

        if (!_tables.TryGetValue(name, out var table))
        {
            table = Read<T>(name);
            _tables[name] = table;
        }

        return (IGameTable<T>)table;
    }

    /// <summary>
    /// Works out the folder that holds the tables of the given language.
    /// </summary>
    /// <param name="rootFolder">The folder that holds the tables.</param>
    /// <param name="language">The language of the client.</param>
    /// <returns>The folder of the tables.</returns>
    private static string ResolveFolder(string rootFolder, ClientLanguage language)
    {
        var baseFolder = Path.Combine(rootFolder, FolderName);

        var folder = Path.Combine(baseFolder, ToFolderName(language));
        if (Directory.Exists(folder))
        {
            return folder;
        }

        // The tables of the language of the client are not always carried
        // along, so the English ones stand in for them.
        var fallback = Path.Combine(baseFolder, FallbackLanguage);
        if (Directory.Exists(fallback))
        {
            return fallback;
        }

        throw new DirectoryNotFoundException($"The tables of the game are not in '{baseFolder}'.");
    }

    /// <summary>
    /// Turns the language of the client into the name of the folder of the
    /// tables of that language.
    /// </summary>
    /// <param name="language">The language of the client.</param>
    /// <returns>The name of the folder.</returns>
    private static string ToFolderName(ClientLanguage language) => language switch
    {
        ClientLanguage.Ja => "ja",
        ClientLanguage.En => "en",
        ClientLanguage.De => "de",
        ClientLanguage.Fr => "fr",
        ClientLanguage.Zh => "chs",
        ClientLanguage.Ko => "ko",
        _ => FallbackLanguage,
    };

    /// <summary>
    /// Works out the name of the table of the given kind of row.
    /// </summary>
    /// <typeparam name="T">The kind of the rows of the table.</typeparam>
    /// <returns>The name of the table.</returns>
    private static string TableName<T>()
        where T : GameRow, new()
    {
        var name = typeof(T).Name;

        // The rows carry the name of their table with the ending of the kind
        // of a row added to it.
        return name.EndsWith("Row", StringComparison.Ordinal)
            ? name[..^"Row".Length]
            : name;
    }

    /// <summary>
    /// Reads a table from the file of the table.
    /// </summary>
    /// <typeparam name="T">The kind of the rows of the table.</typeparam>
    /// <param name="name">The name of the table.</param>
    /// <returns>The table.</returns>
    private GameTable<T> Read<T>(string name)
        where T : GameRow, new()
    {
        var path = Path.Combine(_folder, name + TableExtension);
        if (!File.Exists(path))
        {
            return new GameTable<T>(Array.Empty<T>());
        }

        // The tables are carried along compressed, so that the installation
        // does not have to hold tens of megabytes of plain text.
        using var file = File.OpenRead(path);
        using var compressed = new GZipStream(file, CompressionMode.Decompress);
        using var reader = new StreamReader(compressed, Encoding.UTF8);

        var rows = JsonSerializer.Deserialize<List<T>>(reader.ReadToEnd(), ReadOptions);
        return new GameTable<T>(rows ?? []);
    }
}

/// <summary>
/// A table of the game, held in memory.
/// </summary>
/// <typeparam name="T">The kind of the rows of the table.</typeparam>
public sealed class GameTable<T> : IGameTable<T>
    where T : GameRow
{
    private readonly Dictionary<uint, T> _rows;

    /// <summary>Creates a table over the given rows.</summary>
    /// <param name="rows">The rows of the table.</param>
    public GameTable(IEnumerable<T> rows)
    {
        ArgumentNullException.ThrowIfNull(rows);

        _rows = new Dictionary<uint, T>();
        foreach (var row in rows)
        {
            _rows[row.RowId] = row;
        }
    }

    /// <inheritdoc/>
    public int Count => _rows.Count;

    /// <inheritdoc/>
    public IEnumerable<T> Rows => _rows.Values;

    /// <inheritdoc/>
    public T? GetRow(uint rowId) => _rows.GetValueOrDefault(rowId);

    /// <inheritdoc/>
    public bool TryGetRow(uint rowId, out T row)
    {
        if (_rows.TryGetValue(rowId, out var found))
        {
            row = found;
            return true;
        }

        row = null!;
        return false;
    }

    /// <inheritdoc/>
    public bool HasRow(uint rowId) => _rows.ContainsKey(rowId);
}
