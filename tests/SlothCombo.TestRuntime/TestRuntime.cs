using System;
using System.IO;

namespace SlothCombo.Runtime.Bootstrap;

/// <summary>
/// A stand-in for the runtime of the game.
/// </summary>
/// <remarks>
/// The tests of the loader need an assembly that it can load, call and
/// unload, and the real runtime cannot be started outside of the game. This
/// one carries the very type name the loader reaches for, so the test covers
/// the same path a build of the runtime takes. It writes a line per call
/// into the folder it is given, which is what the tests count.
/// </remarks>
public static class SlothComboRuntime
{
    private const string BootMarker = "booted.txt";
    private const string StopMarker = "stopped.txt";

    private static string? _lastFolder;

    /// <summary>Starts the stand-in.</summary>
    /// <param name="dataFolder">The folder the markers are written to.</param>
    /// <param name="gameInstallPath">The installation directory of the game.</param>
    /// <returns>Zero, always.</returns>
    public static int BootManaged(string? dataFolder, string? gameInstallPath)
    {
        Append(dataFolder, BootMarker);
        return 0;
    }

    /// <summary>Stops the stand-in.</summary>
    /// <remarks>
    /// The folder is not passed to the stop, so it is remembered from the
    /// start. The tests give every run a folder of its own.
    /// </remarks>
    public static void StopManaged()
    {
        Append(_lastFolder, StopMarker);
    }

    private static void Append(string? dataFolder, string marker)
    {
        if (string.IsNullOrEmpty(dataFolder))
        {
            return;
        }

        _lastFolder = dataFolder;

        Directory.CreateDirectory(dataFolder);
        File.AppendAllText(Path.Combine(dataFolder, marker), marker + Environment.NewLine);
    }
}
