using System;
using System.IO;
using System.Linq;

namespace SlothCombo.Tests;

/// <summary>
/// The installation that the publish script writes.
/// </summary>
/// <remarks>
/// The tests read the installation the way the game does, so they need the
/// folder it lives in. The folder is given by the environment variable
/// <c>SLOTHCOMBO_DIST</c>. When that is not set, the repository is looked
/// through for it.
/// </remarks>
internal static class Distribution
{
    /// <summary>The name of the library that is loaded into the game.</summary>
    public const string BootLibraryName = "SlothCombo.Boot.dll";

    /// <summary>
    /// Finds the folder of the installation.
    /// </summary>
    /// <returns>The folder, or <c>null</c> when it cannot be found.</returns>
    public static string? Find()
    {
        var given = Environment.GetEnvironmentVariable("SLOTHCOMBO_DIST");
        if (!string.IsNullOrEmpty(given) && File.Exists(Path.Combine(given, BootLibraryName)))
        {
            return given;
        }

        var folder = new DirectoryInfo(AppContext.BaseDirectory);
        while (folder is not null)
        {
            var candidate = Path.Combine(folder.FullName, "dist");
            if (File.Exists(Path.Combine(candidate, BootLibraryName)))
            {
                return candidate;
            }

            folder = folder.Parent;
        }

        return null;
    }

    /// <summary>
    /// Finds the library that hosts the runtime.
    /// </summary>
    /// <param name="distribution">The folder of the installation.</param>
    /// <returns>The full path of the library, or <c>null</c> when it is missing.</returns>
    public static string? FindHostFxr(string distribution)
    {
        var folder = Path.Combine(distribution, "host", "fxr");
        if (!Directory.Exists(folder))
        {
            return null;
        }

        var newest = Directory
            .GetDirectories(folder)
            .Select(path => (Path: path, Version: Version.TryParse(Path.GetFileName(path), out var version) ? version : null))
            .Where(entry => entry.Version is not null)
            .OrderByDescending(entry => entry.Version)
            .FirstOrDefault();

        if (newest.Path is null)
        {
            return null;
        }

        var library = Path.Combine(newest.Path, "hostfxr.dll");
        return File.Exists(library) ? library : null;
    }
}
