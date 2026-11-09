using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SlothCombo.Host.Injection;

/// <summary>
/// Copies the parts that are loaded into the game out of the installation.
/// </summary>
/// <remarks>
/// <para>
/// A file that the game has loaded stays locked until the runtime that loaded
/// it is gone, which would make the installation impossible to rebuild while
/// the game is running. The runtime and the boot library are therefore loaded
/// from a copy in the temporary folder, and the installation is only ever
/// read.
/// </para>
/// <para>
/// The copy of the runtime is named after the build it holds. A new build
/// gets a new folder, so the files of the build that is still running are
/// never overwritten, no matter whether its context could be unloaded yet.
/// </para>
/// </remarks>
internal static class RuntimeStaging
{
    private const string FolderName = "XIVSlothCombo";

    /// <summary>Gets or sets the folder the copies are kept in.</summary>
    /// <remarks>
    /// The tests point this at a folder of their own, so they never touch the
    /// copies the game is running from.
    /// </remarks>
    internal static string? RootOverride { get; set; }

    /// <summary>Gets the folder the copies are kept in.</summary>
    private static string Root => RootOverride ?? Path.Combine(Path.GetTempPath(), FolderName);

    /// <summary>
    /// Copies the managed part of the installation and returns the copy.
    /// </summary>
    /// <param name="baseFolder">The folder the program is installed in.</param>
    /// <returns>The folder the runtime of this build is loaded from.</returns>
    public static string StageRuntime(string baseFolder)
    {
        var source = Path.Combine(baseFolder, "game");
        if (!Directory.Exists(source))
        {
            throw new DirectoryNotFoundException($"The managed part of the runtime is missing under '{source}'.");
        }

        var hash = ComputeTreeHash(source);

        var root = Path.Combine(Root, "runtime");
        var target = Path.Combine(root, hash);

        if (!Directory.Exists(target))
        {
            Directory.CreateDirectory(root);

            // The copy is written next to its final place and moved there in
            // one step, so a failure never leaves a half build behind under a
            // name that looks complete.
            var temporary = Path.Combine(root, $".tmp-{Guid.NewGuid():N}");
            try
            {
                CopyTree(source, temporary);
                Directory.Move(temporary, target);
            }
            catch (IOException) when (Directory.Exists(target))
            {
                // Another program staged the same build first.
                TryDeleteTree(temporary);
            }
            catch (Exception)
            {
                TryDeleteTree(temporary);
                throw;
            }
        }

        RemoveOlderBuilds(root, hash);

        return target;
    }

    /// <summary>
    /// Copies the boot library and returns the path of the copy.
    /// </summary>
    /// <param name="baseFolder">The folder the program is installed in.</param>
    /// <returns>The path of the boot library this build is loaded from.</returns>
    /// <remarks>
    /// The library is named after its content, so a new build is a new
    /// module and the installation copy is never locked by the game.
    /// </remarks>
    public static string StageBoot(string baseFolder)
    {
        var source = Path.Combine(baseFolder, "SlothCombo.Boot.dll");
        var hash = ComputeFileHash(source);
        var target = Path.Combine(Root, "boot", $"SlothCombo.Boot.{hash[..8]}.dll");

        if (!File.Exists(target))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(target)!);
            File.Copy(source, target, true);
        }

        return target;
    }

    /// <summary>
    /// Removes the copies of the builds that are no longer used.
    /// </summary>
    /// <remarks>
    /// A build that the game still holds cannot be removed, and there is no
    /// reason to report that: the next start tries again.
    /// </remarks>
    private static void RemoveOlderBuilds(string root, string current)
    {
        try
        {
            foreach (var folder in Directory.GetDirectories(root))
            {
                var name = Path.GetFileName(folder);
                if (name.StartsWith(".tmp-", StringComparison.Ordinal) ||
                    string.Equals(name, current, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                TryDeleteTree(folder);
            }
        }
        catch (Exception)
        {
            // The folder of the copies is a convenience; it must never stop
            // the program.
        }
    }

    /// <summary>
    /// Copies a folder and everything in it.
    /// </summary>
    private static void CopyTree(string source, string target)
    {
        Directory.CreateDirectory(target);

        foreach (var file in Directory.GetFiles(source))
        {
            File.Copy(file, Path.Combine(target, Path.GetFileName(file)), true);
        }

        foreach (var folder in Directory.GetDirectories(source))
        {
            CopyTree(folder, Path.Combine(target, Path.GetFileName(folder)));
        }
    }

    /// <summary>
    /// Deletes a folder without reporting a failure.
    /// </summary>
    private static void TryDeleteTree(string path)
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
            // A folder that is still held by the game is removed next time.
        }
    }

    /// <summary>
    /// Works out the name of the build in the given folder.
    /// </summary>
    /// <remarks>
    /// The content decides the name, not the moment it was written: a build
    /// that is published twice without a change is the same build, and the
    /// runtime that is already up to date is not replaced for nothing.
    /// </remarks>
    private static string ComputeTreeHash(string folder)
    {
        var builder = new StringBuilder();

        foreach (var file in Directory
                     .GetFiles(folder, "*", SearchOption.AllDirectories)
                     .OrderBy(path => path, StringComparer.Ordinal))
        {
            using var stream = File.OpenRead(file);
            var digest = Convert.ToHexString(SHA256.HashData(stream));

            builder
                .Append(Path.GetRelativePath(folder, file))
                .Append('|')
                .Append(digest)
                .Append('\n');
        }

        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(builder.ToString()));
        return Convert.ToHexString(bytes)[..16].ToLowerInvariant();
    }

    /// <summary>
    /// Works out the content hash of a file.
    /// </summary>
    private static string ComputeFileHash(string path)
    {
        using var stream = File.OpenRead(path);
        return Convert.ToHexString(SHA256.HashData(stream)).ToLowerInvariant();
    }
}
