using System;
using System.Collections.Generic;
using System.IO;
using SlothCombo.Runtime.Services;

namespace SlothCombo.Runtime.Hooking;

/// <summary>
/// Reports an exception that escaped the managed part of a detour.
/// </summary>
/// <remarks>
/// An exception that leaves a detour enters native code of the game, and the
/// game treats it as fatal. Every detour therefore keeps its own work behind
/// this report, so a broken piece of bookkeeping costs a log line instead of
/// the game session.
/// </remarks>
internal static class DetourGuard
{
    private static readonly object Lock = new();
    private static readonly Dictionary<string, long> LastReport = new(StringComparer.Ordinal);

    /// <summary>Reports a failure of the detour without throwing.</summary>
    /// <param name="source">The detour the failure came from.</param>
    /// <param name="exception">The failure.</param>
    public static void Report(string source, Exception exception)
    {
        try
        {
            Service.PluginLog?.Error(exception, $"The hook {source} failed.");
        }
        catch (Exception)
        {
            // The log itself is what failed; the file below still has a chance.
        }

        try
        {
            // A detour that fails on every call would otherwise fill the file
            // at the rate of the game loop.
            lock (Lock)
            {
                var now = Environment.TickCount64;
                if (LastReport.TryGetValue(source, out var last) && now - last < 1000)
                {
                    return;
                }

                LastReport[source] = now;
            }

            var folder = Path.Combine(Path.GetTempPath(), "XIVSlothCombo");
            Directory.CreateDirectory(folder);
            File.AppendAllText(
                Path.Combine(folder, "detour-errors.log"),
                $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} {source}: {exception}{Environment.NewLine}");
        }
        catch (Exception)
        {
            // A detour may not fail because its report failed.
        }
    }
}
