using System;
using System.IO;

namespace SlothCombo.Runtime.Bootstrap;

/// <summary>
/// The settings the runtime is started with.
/// </summary>
/// <remarks>
/// The runtime no longer has a plugin host that hands it a folder, so the
/// folder is passed in by whoever starts the runtime. The defaults match the
/// place the legacy plugin kept its configuration.
/// </remarks>
public sealed class RuntimeOptions
{
    /// <summary>
    /// The folder the configuration, the logs and the caches are kept in.
    /// </summary>
    public string DataFolder { get; init; } = GetDefaultDataFolder();

    /// <summary>
    /// The installation directory of the game.
    /// </summary>
    /// <remarks>
    /// The static game data is read from the installation. When it is not
    /// given, the directory of the running client is used.
    /// </remarks>
    public string? GameInstallPath { get; init; }

    /// <summary>
    /// The name of the configuration file inside the data folder.
    /// </summary>
    public string ConfigurationFileName { get; init; } = "config.json";

    /// <summary>
    /// Gets the full path of the configuration file.
    /// </summary>
    public string ConfigurationPath => Path.Combine(DataFolder, ConfigurationFileName);

    /// <summary>
    /// Gets the folder the log files are written to.
    /// </summary>
    public string LogFolder => Path.Combine(DataFolder, "logs");

    /// <summary>
    /// Gets the folder the runtime is installed in.
    /// </summary>
    /// <remarks>
    /// The front end of the settings page is served from this folder.
    /// </remarks>
    public string WebUiFolder { get; init; } = AppContext.BaseDirectory;

    /// <summary>
    /// Gets the default data folder of the runtime.
    /// </summary>
    private static string GetDefaultDataFolder()
    {
        var root = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        return Path.Combine(root, "XIVSlothComboX");
    }
}
