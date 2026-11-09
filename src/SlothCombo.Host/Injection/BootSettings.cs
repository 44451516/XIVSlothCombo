using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SlothCombo.Host.Injection;

/// <summary>
/// The settings the boot library reads when it is loaded into the game.
/// </summary>
/// <remarks>
/// <para>
/// The boot library is loaded by path alone, so it cannot be handed any
/// arguments. The host program writes this file right before it injects the
/// library, and the library reads it from the temporary folder of the user,
/// which is the one place both of them can be sure of.
/// </para>
/// <para>
/// The settings carry the full paths of everything the boot needs, so the
/// library never has to work out where it was loaded from.
/// </para>
/// </remarks>
internal sealed class BootSettings
{
    /// <summary>The name of the settings file.</summary>
    public const string FileName = "boot.json";

    /// <summary>Gets the folder the settings file is written to.</summary>
    public static string Folder => Path.Combine(Path.GetTempPath(), "XIVSlothCombo");

    /// <summary>Gets the full path of the settings file.</summary>
    public static string FilePath => Path.Combine(Folder, FileName);

    /// <summary>Gets or sets the library that hosts the runtime.</summary>
    /// <remarks>
    /// The library works out where the runtime lives from its own place, so
    /// it has to sit in the folder the runtime was installed in.
    /// </remarks>
    public string HostFxrPath { get; set; } = string.Empty;

    /// <summary>Gets or sets the runtime configuration of the managed part.</summary>
    public string RuntimeConfigPath { get; set; } = string.Empty;

    /// <summary>Gets or sets the loader that keeps the runtime replaceable.</summary>
    /// <remarks>
    /// The loader is the only assembly that is handed to the default context
    /// of the runtime, where it stays for the rest of the session. It has to
    /// be loaded from the same path every time, otherwise a second loader
    /// would lose track of the runtime the first one kept.
    /// </remarks>
    public string LoaderPath { get; set; } = string.Empty;

    /// <summary>Gets or sets the assembly that holds the managed runtime.</summary>
    /// <remarks>
    /// This is the copy in the temporary folder, not the file of the
    /// installation. The copy is loaded into a context that can be unloaded,
    /// so the installation stays writable while the game runs.
    /// </remarks>
    public string RuntimePath { get; set; } = string.Empty;

    /// <summary>Gets or sets the folder the runtime keeps its data in.</summary>
    public string DataFolder { get; set; } = string.Empty;

    /// <summary>Gets or sets the installation directory of the game.</summary>
    public string GameInstallPath { get; set; } = string.Empty;

    /// <summary>Gets or sets the file the boot library reports its progress in.</summary>
    public string StatusPath { get; set; } = string.Empty;

    /// <summary>
    /// Writes the settings to the place the boot library reads them from.
    /// </summary>
    /// <param name="settings">The settings to write.</param>
    public static void Write(BootSettings settings)
    {
        Directory.CreateDirectory(Folder);

        var json = JsonSerializer.Serialize(settings, BootJsonContext.Default.BootSettings);
        File.WriteAllText(FilePath, json);
    }
}

/// <summary>The state the boot reports to the host program.</summary>
internal sealed class BootStatus
{
    /// <summary>Gets or sets the state of the boot.</summary>
    public string State { get; set; } = string.Empty;

    /// <summary>Gets or sets the details of the state.</summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>Gets or sets the moment the state was written.</summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Reads the state that was written last.
    /// </summary>
    /// <param name="path">The path of the file the state is written to.</param>
    /// <returns>The state, or <c>null</c> when it cannot be read.</returns>
    public static BootStatus? Read(string path)
    {
        try
        {
            if (!File.Exists(path))
            {
                return null;
            }

            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize(json, BootJsonContext.Default.BootStatus);
        }
        catch (Exception)
        {
            return null;
        }
    }
}

/// <summary>
/// The description of the data the host program and the boot library share.
/// </summary>
[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(BootSettings))]
[JsonSerializable(typeof(BootStatus))]
internal partial class BootJsonContext : JsonSerializerContext;
