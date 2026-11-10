using System;
using System.Collections.Generic;
using SlothCombo.Abstractions;
using SlothCombo.Interop;

namespace SlothCombo.Runtime.Bootstrap;

/// <summary>
/// Resolves the addresses of the native functions the runtime needs.
/// </summary>
/// <remarks>
/// <para>
/// The addresses are written down as patterns of the bytes that lead to them,
/// and the patterns are searched for in the code of the game when the runtime
/// starts. The layer that holds the patterns is the one that talks to the game,
/// so this class only drives it.
/// </para>
/// <para>
/// The resolver accepts a JSON cache, which is what makes the runtime able to
/// survive a game patch: the addresses that still resolve are taken from the
/// file and only the ones that broke are searched for again.
/// </para>
/// </remarks>
public static class SignatureResolver
{
    /// <summary>The name of the cache file inside the runtime data folder.</summary>
    public const string CacheFileName = GameAddressTable.CacheFileName;

    /// <summary>
    /// Resolves all the addresses of the game.
    /// </summary>
    /// <param name="cachePath">
    /// The path of the JSON cache to read and write. The resolver creates the
    /// file itself when the addresses resolve for the first time.
    /// </param>
    /// <param name="scanner">The scanner that looks for the patterns.</param>
    /// <param name="unresolved">
    /// Receives the names of the addresses that could not be resolved.
    /// </param>
    /// <returns><c>true</c> when every address resolved.</returns>
    public static bool Resolve(string cachePath, ISignatureScanner scanner, out IReadOnlyList<string> unresolved)
    {
        ArgumentException.ThrowIfNullOrEmpty(cachePath);
        ArgumentNullException.ThrowIfNull(scanner);

        // The cache lets the runtime reuse previously resolved addresses, which
        // keeps the start fast and lets a patched cache survive a game update
        // that broke some of the patterns.
        var resolved = GameFunctions.Resolve(cachePath, scanner);

        unresolved = GameFunctions.Unresolved;
        return resolved;
    }
}
