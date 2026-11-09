using System;
using System.IO;
using System.Threading;
using LoaderType = SlothCombo.Loader.Loader;

namespace SlothCombo.Tests;

/// <summary>
/// The tests of the loader that keeps the runtime replaceable.
/// </summary>
/// <remarks>
/// The real runtime cannot be started outside of the game, so the tests load
/// a stand-in that carries the same type name and writes a line per call.
/// What they check is the part the game depends on: a new build stops the old
/// one, and the file of the old one is free again afterwards.
/// </remarks>
internal static class LoaderTest
{
    private const string RuntimeFileName = "SlothCombo.TestRuntime.dll";

    /// <summary>
    /// Loads a build, loads it again, and replaces it with the next one.
    /// </summary>
    public static void LoadsAndReplaces()
    {
        var source = Path.Combine(AppContext.BaseDirectory, RuntimeFileName);
        Assert.True(File.Exists(source), $"The stand-in of the runtime is missing from '{AppContext.BaseDirectory}'.");

        var root = Path.Combine(Path.GetTempPath(), "slothcombo-tests", $"loader.{Guid.NewGuid():N}");
        var first = Path.Combine(root, "first");
        var second = Path.Combine(root, "second");
        var data = Path.Combine(root, "data");

        Directory.CreateDirectory(first);
        Directory.CreateDirectory(second);
        File.Copy(source, Path.Combine(first, RuntimeFileName), true);
        File.Copy(source, Path.Combine(second, RuntimeFileName), true);

        try
        {
            var firstAssembly = Path.Combine(first, RuntimeFileName);
            var secondAssembly = Path.Combine(second, RuntimeFileName);

            Assert.Equal(0, LoaderType.BootCore(firstAssembly, data, null));
            Assert.Equal(1, CountLines(Path.Combine(data, "booted.txt")));

            // The same build in the same folder is what a program that is
            // started twice finds; it must not start the runtime again.
            Assert.Equal(0, LoaderType.BootCore(firstAssembly, data, null));
            Assert.Equal(1, CountLines(Path.Combine(data, "booted.txt")));

            // Another folder is another build. The old one is stopped and its
            // context is unloaded before the new one starts.
            Assert.Equal(0, LoaderType.BootCore(secondAssembly, data, null));
            Assert.Equal(1, CountLines(Path.Combine(data, "stopped.txt")));
            Assert.Equal(2, CountLines(Path.Combine(data, "booted.txt")));

            // The unload is what lets the installation be rebuilt, so the
            // file of the old build has to be deletable again.
            DeleteEventually(firstAssembly);
            Assert.True(!File.Exists(firstAssembly), "The assembly of the old build is still held after the unload.");
        }
        finally
        {
            TryDelete(root);
        }
    }

    /// <summary>
    /// Checks that a runtime that is not there is reported, not thrown.
    /// </summary>
    public static void ReportsMissingRuntime()
    {
        var root = Path.Combine(Path.GetTempPath(), "slothcombo-tests", $"loader.{Guid.NewGuid():N}");

        try
        {
            var code = LoaderType.BootCore(Path.Combine(root, "missing.dll"), root, null);

            Assert.Equal(1, code);
            Assert.True(File.Exists(Path.Combine(root, "boot-error.txt")), "The failure was not written down.");
        }
        finally
        {
            TryDelete(root);
        }
    }

    /// <summary>
    /// Counts the lines a stand-in wrote.
    /// </summary>
    private static int CountLines(string path)
    {
        if (!File.Exists(path))
        {
            return 0;
        }

        return File.ReadAllLines(path).Length;
    }

    /// <summary>
    /// Deletes a file that may still be held for a moment.
    /// </summary>
    private static void DeleteEventually(string path)
    {
        for (var attempt = 0; attempt < 20; attempt++)
        {
            try
            {
                File.Delete(path);
                return;
            }
            catch (IOException) when (attempt < 19)
            {
                Thread.Sleep(100);
            }
        }
    }

    /// <summary>
    /// Deletes a folder of the test without reporting a failure.
    /// </summary>
    private static void TryDelete(string path)
    {
        try
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }
        catch (Exception)
        {
            // The next run of the tests starts with a fresh folder.
        }
    }
}
