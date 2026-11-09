using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SlothCombo.Boot;

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
internal sealed class BootConfig
{
    /// <summary>The folder the settings file is written to.</summary>
    public static string Folder => System.IO.Path.Combine(System.IO.Path.GetTempPath(), "XIVSlothCombo");

    /// <summary>The path of the settings file.</summary>
    public static string FilePath => System.IO.Path.Combine(Folder, "boot.json");

    /// <summary>Gets or sets the library that hosts the runtime.</summary>
    /// <remarks>
    /// <para>
    /// The library works out where the runtime lives from its own place, so
    /// it has to sit in the folder the runtime was installed in. The folder
    /// holds the library of the host of the runtime under
    /// <c>host/fxr/&lt;version&gt;</c>, which is the place the host of the
    /// runtime looks for it as well.
    /// </para>
    /// <para>
    /// The host program writes the full path of the library, so the library
    /// never has to guess where it was installed.
    /// </para>
    /// </remarks>
    public string HostFxrPath { get; set; } = string.Empty;

    /// <summary>Gets or sets the runtime configuration of the managed part.</summary>
    public string RuntimeConfigPath { get; set; } = string.Empty;

    /// <summary>Gets or sets the assembly that holds the managed runtime.</summary>
    public string AssemblyPath { get; set; } = string.Empty;

    /// <summary>Gets or sets the type that holds the entry point.</summary>
    public string TypeName { get; set; } = string.Empty;

    /// <summary>Gets or sets the name of the entry point.</summary>
    public string MethodName { get; set; } = string.Empty;

    /// <summary>Gets or sets the folder the runtime keeps its data in.</summary>
    public string DataFolder { get; set; } = string.Empty;

    /// <summary>Gets or sets the installation directory of the game.</summary>
    public string GameInstallPath { get; set; } = string.Empty;

    /// <summary>Gets or sets the file the boot library reports its progress in.</summary>
    public string StatusPath { get; set; } = string.Empty;

    /// <summary>
    /// Reads the settings.
    /// </summary>
    /// <returns>The settings, or <c>null</c> when they cannot be read.</returns>
    public static BootConfig? Load()
    {
        try
        {
            var path = FilePath;
            if (!File.Exists(path))
            {
                return null;
            }

            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize(json, BootJsonContext.Default.BootConfig);
        }
        catch (Exception)
        {
            return null;
        }
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
}

/// <summary>
/// The description of the data the boot library reads and writes.
/// </summary>
/// <remarks>
/// The library is built ahead of time, so the reader and the writer of the
/// files are described at compile time instead of being looked up at run
/// time.
/// </remarks>
[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(BootConfig))]
[JsonSerializable(typeof(BootStatus))]
internal partial class BootJsonContext : JsonSerializerContext;
