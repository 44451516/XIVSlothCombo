using System;
using System.Diagnostics;
using System.IO;
using Lumina;
using Lumina.Excel;
using SlothCombo.Abstractions;
using LuminaLanguage = Lumina.Data.Language;

namespace SlothCombo.Runtime.Services;

/// <summary>
/// The static game data backed by the Excel reader.
/// </summary>
/// <remarks>
/// The Excel reader needs the game installation to read the sheet data from.
/// The path is discovered from the running process, which keeps the runtime
/// independent of any launcher specific configuration.
/// </remarks>
internal sealed class LuminaDataService : IDataManager, IDisposable
{
    private readonly ExcelModule _module;
    private readonly GameData _gameData;
    private ClientLanguage _language = ClientLanguage.Unknown;

    /// <summary>Creates the service for the given game installation.</summary>
    /// <param name="gameInstallPath">The installation directory of the game.</param>
    public LuminaDataService(string gameInstallPath)
    {
        ArgumentException.ThrowIfNullOrEmpty(gameInstallPath);

        _gameData = new GameData(ResolveDataPath(gameInstallPath), new LuminaOptions());
        _module = new ExcelModule(_gameData);

        // The status structs of the game only carry an id, so the names are
        // resolved through the status sheet of the game data.
        StatusSheetLookup.Register(ResolveStatusName);
    }

    /// <summary>
    /// Works out the folder that holds the data of the game.
    /// </summary>
    /// <remarks>
    /// The reader reads the data from the folder with the data of the game,
    /// which sits next to the client in the folder of the game.
    /// </remarks>
    private static string ResolveDataPath(string gameInstallPath)
    {
        var data = Path.Combine(gameInstallPath, "sqpack");
        return Directory.Exists(data) ? data : gameInstallPath;
    }

    /// <summary>
    /// Tries to locate the game installation from the running process.
    /// </summary>
    /// <returns>The installation directory, or <c>null</c> when it cannot be found.</returns>
    public static string? TryFindGameInstallPath()
    {
        try
        {
            var module = Process.GetCurrentProcess().MainModule;
            var path = module?.FileName;
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }

            // The client lives in <install>/game/ffxiv_dx11.exe.
            var gameDirectory = Path.GetDirectoryName(path);
            return string.IsNullOrEmpty(gameDirectory) ? null : gameDirectory;
        }
        catch (Exception)
        {
            return null;
        }
    }

    /// <inheritdoc/>
    public ExcelSheet<T> GetExcelSheet<T>() where T : struct, IExcelRow<T>
        => _module.GetSheet<T>(ToLuminaLanguage(Language));

    /// <summary>
    /// Maps the language of the client to the language the sheets are read in.
    /// </summary>
    private static LuminaLanguage ToLuminaLanguage(ClientLanguage language) => language switch
    {
        ClientLanguage.Ja => LuminaLanguage.Japanese,
        ClientLanguage.En => LuminaLanguage.English,
        ClientLanguage.De => LuminaLanguage.German,
        ClientLanguage.Fr => LuminaLanguage.French,
        // The Chinese client ships the simplified sheets; the traditional
        // sheets of the Taiwanese client are not distinguished here.
        ClientLanguage.Zh => LuminaLanguage.ChineseSimplified,
        ClientLanguage.Ko => LuminaLanguage.Korean,
        // The client language could not be read; English is the last resort.
        _ => LuminaLanguage.English,
    };

    /// <summary>
    /// Gets the data of the game, which holds the files of the game itself.
    /// </summary>
    /// <remarks>
    /// The pictures of the interface are read from the files of the game, so
    /// the raw data is needed next to the sheets.
    /// </remarks>
    public GameData GameData => _gameData;

    /// <inheritdoc/>
    public ClientLanguage Language
    {
        get
        {
            if (_language == ClientLanguage.Unknown)
            {
                _language = ReadLanguage();
            }

            return _language;
        }
        set => _language = value;
    }

    /// <summary>Resolves the name of the status with the given id.</summary>
    /// <param name="statusId">The id of the status.</param>
    /// <returns>The name of the status, or an empty string.</returns>
    private string ResolveStatusName(uint statusId)
    {
        try
        {
            var sheet = _module.GetSheet<Lumina.Excel.Sheets.Status>(ToLuminaLanguage(Language));
            var row = sheet.TryGetRow(statusId, out var found) ? found : default;
            return row.Name.ToString();
        }
        catch (Exception)
        {
            return string.Empty;
        }
    }

    /// <summary>Reads the language of the game data.</summary>
    /// <returns>The language of the game data.</returns>
    private static unsafe ClientLanguage ReadLanguage()
    {
        try
        {
            var framework = FFXIVClientStructs.FFXIV.Client.System.Framework.Framework.Instance();
            if (framework is null)
            {
                return ClientLanguage.Unknown;
            }

            return framework->ClientLanguage switch
            {
                0 => ClientLanguage.Ja,
                1 => ClientLanguage.En,
                2 => ClientLanguage.De,
                3 => ClientLanguage.Fr,
                4 or 5 => ClientLanguage.Zh,
                6 => ClientLanguage.Ko,
                _ => ClientLanguage.Unknown,
            };
        }
        catch (Exception)
        {
            return ClientLanguage.Unknown;
        }
    }

    /// <summary>Closes the files of the game data.</summary>
    /// <remarks>
    /// A runtime that takes the place of this one opens the data again, so
    /// the files are closed before the context of this one goes away.
    /// </remarks>
    public void Dispose() => _gameData.Dispose();
}
