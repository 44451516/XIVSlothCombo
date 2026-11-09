using System;
using System.IO;
using SlothCombo.Host.Injection;

namespace SlothCombo.Tests;

/// <summary>
/// The tests of the copies the game loads the runtime from.
/// </summary>
/// <remarks>
/// The copies decide which build the game runs. They also have to stay out of
/// the installation, because a file the game has loaded cannot be replaced
/// while the game is running.
/// </remarks>
internal static class StagingTest
{
    /// <summary>
    /// Stages a build, stages it again, and stages the next one.
    /// </summary>
    public static void StagesTheBuild()
    {
        var root = Path.Combine(Path.GetTempPath(), "slothcombo-tests", $"staging.{Guid.NewGuid():N}");
        var install = Path.Combine(root, "install");
        var game = Path.Combine(install, "game");
        var copies = Path.Combine(root, "copies");

        Directory.CreateDirectory(game);
        File.WriteAllText(Path.Combine(install, "SlothCombo.Boot.dll"), "boot-one");
        File.WriteAllText(Path.Combine(game, "SlothCombo.Runtime.dll"), "build-one");
        File.WriteAllText(Path.Combine(game, "SlothCombo.Runtime.deps.json"), "{}");

        RuntimeStaging.RootOverride = copies;

        try
        {
            var first = RuntimeStaging.StageRuntime(install);
            Assert.True(File.Exists(Path.Combine(first, "SlothCombo.Runtime.dll")), "The runtime was not copied.");
            Assert.Equal("build-one", File.ReadAllText(Path.Combine(first, "SlothCombo.Runtime.dll")));

            // The same build is the same folder, so a program that is started
            // twice does not leave a second copy behind.
            var again = RuntimeStaging.StageRuntime(install);
            Assert.Equal(first, again);
            Assert.Equal(1, Directory.GetDirectories(Path.Combine(copies, "runtime")).Length);

            // A new build is a new folder, and the copy of the old build goes.
            File.WriteAllText(Path.Combine(game, "SlothCombo.Runtime.dll"), "build-two");
            var second = RuntimeStaging.StageRuntime(install);
            Assert.True(!string.Equals(first, second, StringComparison.OrdinalIgnoreCase), "A new build kept the name of the old one.");
            Assert.Equal("build-two", File.ReadAllText(Path.Combine(second, "SlothCombo.Runtime.dll")));
            Assert.True(!Directory.Exists(first), "The copy of the old build was not removed.");

            // The boot library is copied by content as well, so the file of
            // the installation is never the one the game loads.
            var boot = RuntimeStaging.StageBoot(install);
            Assert.Equal("boot-one", File.ReadAllText(boot));
            Assert.True(!string.Equals(boot, Path.Combine(install, "SlothCombo.Boot.dll"), StringComparison.OrdinalIgnoreCase),
                "The boot library was handed to the game from the installation.");

            File.WriteAllText(Path.Combine(install, "SlothCombo.Boot.dll"), "boot-two");
            var nextBoot = RuntimeStaging.StageBoot(install);
            Assert.True(!string.Equals(boot, nextBoot, StringComparison.OrdinalIgnoreCase), "A new boot library kept the name of the old one.");
            Assert.Equal("boot-two", File.ReadAllText(nextBoot));
        }
        finally
        {
            RuntimeStaging.RootOverride = null;

            try
            {
                Directory.Delete(root, true);
            }
            catch (Exception)
            {
                // The next run of the tests starts with a fresh folder.
            }
        }
    }
}
