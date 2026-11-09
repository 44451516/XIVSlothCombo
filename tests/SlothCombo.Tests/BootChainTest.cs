using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SlothCombo.Tests;

/// <summary>
/// The test that loads the managed runtime the way the game does.
/// </summary>
/// <remarks>
/// <para>
/// This is the part of the program that cannot be checked by reading the
/// code: the boot library has to find the runtime, hand the managed assembly
/// over to it and call into it.
/// </para>
/// <para>
/// The test runs the library the way the loader of the system runs it,
/// because a process that holds a runtime of its own cannot start a second
/// one. The game is a plain program, so the loader of the system stands in
/// for it.
/// </para>
/// </remarks>
internal static class BootChainTest
{
    /// <summary>The name of the export the loader of the system calls.</summary>
    private const string ExportName = "SlothComboBootRundll32";

    /// <summary>How long the boot is given to report its state.</summary>
    private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(60);

    /// <summary>
    /// Loads the runtime through the boot library and checks that it answers.
    /// </summary>
    public static void Run()
    {
        var distribution = Distribution.Find();
        if (distribution is null)
        {
            throw new SkipException("The installation was not found. Run build/publish.ps1 first.");
        }

        var hostFxr = Distribution.FindHostFxr(distribution);
        Assert.True(hostFxr is not null, "The library that hosts the runtime is missing from the installation.");

        var runtimeFolder = Path.Combine(distribution, "game");
        var runtimeConfig = Path.Combine(runtimeFolder, "SlothCombo.Runtime.runtimeconfig.json");
        var assembly = Path.Combine(runtimeFolder, "SlothCombo.Runtime.dll");
        Assert.True(File.Exists(runtimeConfig), "The configuration of the runtime is missing from the installation.");
        Assert.True(File.Exists(assembly), "The managed part of the runtime is missing from the installation.");

        var dataFolder = Path.Combine(Path.GetTempPath(), "slothcombo-tests", "boot");
        Directory.CreateDirectory(dataFolder);

        var statusPath = Path.Combine(dataFolder, "boot-status.json");
        TryDelete(statusPath);

        var settings = new BootSettings
        {
            HostFxrPath = hostFxr!,
            RuntimeConfigPath = runtimeConfig,
            AssemblyPath = assembly,
            TypeName = "SlothCombo.Runtime.Bootstrap.SlothComboRuntime, SlothCombo.Runtime",
            MethodName = "Boot",
            DataFolder = dataFolder,
            GameInstallPath = string.Empty,
            StatusPath = statusPath,
        };

        var configFolder = Path.Combine(Path.GetTempPath(), "XIVSlothCombo");
        Directory.CreateDirectory(configFolder);
        File.WriteAllText(
            Path.Combine(configFolder, "boot.json"),
            JsonSerializer.Serialize(settings, BootJsonContext.Default.BootSettings));

        var loader = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "rundll32.exe");
        var library = Path.Combine(distribution, Distribution.BootLibraryName);

        var process = Process.Start(new ProcessStartInfo(loader)
        {
            Arguments = $"\"{library}\",{ExportName}",
            UseShellExecute = false,
            CreateNoWindow = true,
        });

        Assert.True(process is not null, "The loader of the system could not be started.");
        process!.WaitForExit((int)Timeout.TotalMilliseconds);
        Assert.True(process.HasExited, "The loader of the system did not finish in time.");

        var status = ReadStatus(statusPath);
        Assert.True(status is not null, "The boot library did not report its state in time.");

        // The managed part of the runtime is started inside a program that is
        // not the game, so it cannot find the game and reports that it
        // refused to start. Anything else means that the chain broke before
        // the managed part was reached.
        Assert.True(
            !status!.Message.Contains("could not be loaded", StringComparison.OrdinalIgnoreCase),
            $"The managed runtime could not be loaded: {status.Message}");

        Assert.True(
            status.Message.Contains("refused to start", StringComparison.OrdinalIgnoreCase),
            $"The managed runtime did not report a refusal: {status.Message}");
    }

    /// <summary>
    /// Reads the state that the boot library wrote.
    /// </summary>
    private static BootStatus? ReadStatus(string path)
    {
        var deadline = DateTime.UtcNow + Timeout;

        while (DateTime.UtcNow < deadline)
        {
            try
            {
                if (File.Exists(path))
                {
                    var json = File.ReadAllText(path);
                    var status = JsonSerializer.Deserialize(json, BootJsonContext.Default.BootStatus);
                    if (!string.IsNullOrEmpty(status?.State))
                    {
                        return status;
                    }
                }
            }
            catch (IOException)
            {
                // The file is being written right now.
            }

            System.Threading.Thread.Sleep(100);
        }

        return null;
    }

    /// <summary>
    /// Deletes a file, ignoring the failures.
    /// </summary>
    private static void TryDelete(string path)
    {
        try
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        catch (IOException)
        {
            // A file that cannot be deleted is overwritten by the boot anyway.
        }
    }
}

/// <summary>The settings the boot library reads.</summary>
internal sealed class BootSettings
{
    /// <summary>Gets or sets the library that hosts the runtime.</summary>
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
}

/// <summary>The state the boot reports.</summary>
internal sealed class BootStatus
{
    /// <summary>Gets or sets the state of the boot.</summary>
    public string State { get; set; } = string.Empty;

    /// <summary>Gets or sets the details of the state.</summary>
    public string Message { get; set; } = string.Empty;
}

/// <summary>
/// The description of the data the test and the boot library share.
/// </summary>
[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(BootSettings))]
[JsonSerializable(typeof(BootStatus))]
internal partial class BootJsonContext : JsonSerializerContext;
