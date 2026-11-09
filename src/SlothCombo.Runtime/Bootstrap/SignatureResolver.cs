using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FFXIVClientStructs.Interop.Generated;
using InteropGenerator.Runtime;

namespace SlothCombo.Runtime.Bootstrap;

/// <summary>
/// Resolves the addresses of the native functions the runtime needs.
/// </summary>
/// <remarks>
/// <para>
/// FFXIVClientStructs resolves every address it uses from signatures that live in
/// the library itself. Dalamud normally performs that resolution when it loads a
/// plugin, which is exactly the dependency this project removes: here the
/// resolution is driven explicitly by <see cref="Resolver"/>.
/// </para>
/// <para>
/// The resolver accepts a JSON cache, which is what makes the runtime able to
/// survive a game patch that the upstream struct library has not caught up with
/// yet: the cached module relative offsets are reused, and only the entries that
/// no longer resolve have to be repaired by hand.
/// </para>
/// </remarks>
public static class SignatureResolver
{
    /// <summary>The name of the cache file inside the runtime data folder.</summary>
    public const string CacheFileName = "sigcache.json";

    /// <summary>
    /// The cache version.
    /// </summary>
    /// <remarks>
    /// The resolver drops the whole cache when the version changes. Bumping it
    /// is the supported way to invalidate addresses that were repaired by hand
    /// for a specific game build.
    /// </remarks>
    public const string CacheVersion = "1";

    /// <summary>
    /// Resolves all struct library addresses.
    /// </summary>
    /// <param name="cachePath">
    /// The path of the JSON cache to read and write. The resolver creates the
    /// file itself when the addresses resolve for the first time.
    /// </param>
    /// <param name="unresolved">
    /// Receives the names of the signatures that could not be resolved.
    /// </param>
    /// <returns><c>true</c> when every required address resolved.</returns>
    public static bool Resolve(string cachePath, out IReadOnlyList<string> unresolved)
    {
        ArgumentException.ThrowIfNullOrEmpty(cachePath);

        var cache = new FileInfo(cachePath);
        var directory = cache.Directory;
        if (directory is not null && !directory.Exists)
        {
            directory.Create();
        }

        var resolver = Resolver.GetInstance;

        // Passing the cache lets the resolver reuse previously resolved
        // addresses, which keeps startup fast and lets a patched cache survive
        // a game update that broke some of the upstream signatures.
        resolver.Setup(version: CacheVersion, cacheFile: cache);

        Addresses.Register();
        resolver.Resolve();

        var failed = resolver.Addresses
            .Where(address => address.Value == 0)
            .Select(address => address.Name)
            .ToList();

        unresolved = failed;

        return failed.Count == 0;
    }
}
