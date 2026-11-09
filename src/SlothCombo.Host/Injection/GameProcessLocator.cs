using System;
using System.Diagnostics;
using System.Linq;

namespace SlothCombo.Host.Injection;

/// <summary>
/// Finds the game client.
/// </summary>
internal static class GameProcessLocator
{
    /// <summary>The name of the executable of the game client.</summary>
    private const string ProcessName = "ffxiv_dx11";

    /// <summary>
    /// Finds the running game client.
    /// </summary>
    /// <returns>The process, or <c>null</c> when the game is not running.</returns>
    public static Process? Find()
    {
        var processes = Process.GetProcessesByName(ProcessName);

        // The client is started once, but a leftover process of a client
        // that is on its way out would be picked up as well, so the one
        // with the window is the one that is used.
        var chosen = processes.FirstOrDefault(process => !process.HasExited && GetMainWindowHandle(process) != IntPtr.Zero)
                     ?? processes.FirstOrDefault(process => !process.HasExited);

        // The chosen process is handed to the caller, which reads its id and
        // main module and waits on it, so it has to stay alive. Disposing it
        // here would leave the caller with an object whose id and modules can
        // no longer be read, and the injection would target no process at all.
        foreach (var process in processes)
        {
            if (!ReferenceEquals(process, chosen))
            {
                process.Dispose();
            }
        }

        return chosen;
    }

    /// <summary>
    /// Gets the window of the process, if it has one.
    /// </summary>
    private static IntPtr GetMainWindowHandle(Process process)
    {
        try
        {
            return process.MainWindowHandle;
        }
        catch (Exception)
        {
            // A process that is on its way out does not answer any more.
            return IntPtr.Zero;
        }
    }
}
