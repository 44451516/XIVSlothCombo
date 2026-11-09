using System;
using System.IO;

namespace SlothCombo.Tests;

/// <summary>
/// The entry point of the tests.
/// </summary>
internal static class Program
{
    private static int Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("SlothCombo tests");
        Console.WriteLine();

        var runner = new TestRunner();
        runner.Add("The boot library loads the managed runtime", BootChainTest.Run);
        runner.Add("The configuration survives a round trip", ConfigurationTest.RoundTrip);
        runner.Add("The exports of a program file are found", PeExportTest.FindsSystemExport);
        runner.Add("The export of the boot library is found", PeExportTest.FindsBootExport);
        runner.Add("An export that is not there is reported", PeExportTest.RejectsUnknownExport);
        runner.Add("A message of the interface survives a round trip", IpcTest.RoundTrip);
        runner.Add("A picture survives a round trip", IpcTest.CarriesPicture);
        runner.Add("The pipe of the interface carries the name of the user", IpcTest.PipeCarriesUser);
        runner.Add("The program talks to the runtime over the pipe", IpcTest.ConnectsOverPipe);

        return runner.Run();
    }
}

/// <summary>
/// The tests of the configuration store.
/// </summary>
internal static class ConfigurationTest
{
    /// <summary>
    /// Writes a configuration, reads it back and compares it.
    /// </summary>
    public static void RoundTrip()
    {
        var folder = Path.Combine(Path.GetTempPath(), "slothcombo-tests");
        Directory.CreateDirectory(folder);

        var path = Path.Combine(folder, "config.json");
        if (File.Exists(path))
        {
            File.Delete(path);
        }

        var store = new SlothCombo.Runtime.Configuration.ConfigStore(path);
        var written = new SlothCombo.Core.PluginConfiguration
        {
            EnabledOutputLog = true,
            MeleeOffset = 1.5,
        };

        store.Save(written);

        var read = store.Load<SlothCombo.Core.PluginConfiguration>();
        Assert.Equal(true, read.EnabledOutputLog);
        Assert.Equal(1.5, read.MeleeOffset);

        // A configuration that cannot be read falls back to the defaults
        // rather than stopping the runtime from starting.
        File.WriteAllText(path, "{ this is not a configuration");
        var broken = store.Load<SlothCombo.Core.PluginConfiguration>();
        Assert.Equal(false, broken.EnabledOutputLog);
    }
}
