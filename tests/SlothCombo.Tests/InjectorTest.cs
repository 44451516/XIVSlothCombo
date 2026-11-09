using System;
using SlothCombo.Host.Injection;

namespace SlothCombo.Tests;

/// <summary>
/// The tests of the parts of the injector that run without a game.
/// </summary>
/// <remarks>
/// The injector reaches the loader of the system and reads the modules of a
/// process through the entry points of kernel32. The names of those entry
/// points and the marshalling of their strings are the part that broke, so
/// they are checked against this process, which stands in for the game.
/// </remarks>
internal static class InjectorTest
{
    /// <summary>
    /// Resolves the address of the loader the injection starts.
    /// </summary>
    public static void ResolvesSystemLoader()
    {
        var loader = DllInjector.ResolveSystemLoader();
        Assert.True(loader != IntPtr.Zero, "The loader of the system could not be resolved.");
    }

    /// <summary>
    /// Reads the modules of this process the way the injector reads the game.
    /// </summary>
    public static void FindsModuleInProcess()
    {
        var module = DllInjector.FindRemoteModule(IntPtr.Zero, Environment.ProcessId, "kernel32.dll");
        Assert.True(module != IntPtr.Zero, "A module of this process was not found.");
    }
}