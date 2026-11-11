using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using SlothCombo.Abstractions;

namespace SlothCombo.Interop;

/// <summary>
/// The addresses of the functions of the game that the program uses.
/// </summary>
/// <remarks>
/// <para>
/// The table holds every function the program needs. It is filled in before
/// the first hook is installed, because a hook patches the very bytes a
/// pattern is matched against.
/// </para>
/// <para>
/// The result is written to a file next to the data of the runtime. A game
/// that was patched keeps most of its addresses, so the entries that still
/// resolve are taken from the file and only the ones that broke are searched
/// for again.
/// </para>
/// </remarks>
public sealed class GameAddressTable
{
    /// <summary>The name of the file the resolved addresses are written to.</summary>
    public const string CacheFileName = "addresses.json";

    /// <summary>
    /// The version of the file.
    /// </summary>
    /// <remarks>
    /// The whole file is thrown away when the version changes. Bumping the
    /// version is the supported way to drop the addresses that were repaired
    /// by hand for a game that has been patched since.
    /// </remarks>
    public const string CacheVersion = "2";

    /// <summary>
    /// The fingerprint of the game the addresses belong to.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The addresses of a game only hold for the build they were taken from, so
    /// the file carries the fingerprint of that build. A game that was updated
    /// produces a different fingerprint, which makes the file be ignored
    /// instead of handing the offsets of the old game to the new one.
    /// </para>
    /// <para>
    /// The fingerprint is the hash of the executable of the game, so it also
    /// changes when the game is patched without the addresses being wrong.
    /// </para>
    /// </remarks>
    private static string Fingerprint()
    {
        try
        {
            using var process = System.Diagnostics.Process.GetCurrentProcess();
            using var module = process.MainModule
                ?? throw new InvalidOperationException("The game executable could not be located.");
            using var stream = File.OpenRead(module.FileName);
            return Convert.ToHexString(System.Security.Cryptography.SHA256.HashData(stream));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                "The fingerprint of the game executable could not be read; refusing to reuse signature addresses.", ex);
        }
    }

    private readonly Dictionary<string, GameAddress> _addresses = new(StringComparer.Ordinal);
    private readonly Dictionary<string, nint> _cached = new(StringComparer.Ordinal);

    /// <summary>Gets the address with the given name.</summary>
    /// <param name="name">The name of the address.</param>
    /// <returns>The address, or <c>null</c> when there is none.</returns>
    public GameAddress? this[string name] => _addresses.GetValueOrDefault(name);

    /// <summary>Gets the names of the addresses that could not be resolved.</summary>
    public IReadOnlyList<string> Unresolved { get; private set; } = [];

    /// <summary>
    /// Adds an address to the table.
    /// </summary>
    /// <param name="address">The address.</param>
    public void Add(GameAddress address)
    {
        ArgumentNullException.ThrowIfNull(address);
        _addresses[address.Name] = address;
    }

    /// <summary>
    /// Resolves every address of the table.
    /// </summary>
    /// <param name="cachePath">The path of the file the addresses are read from and written to.</param>
    /// <param name="scanner">The scanner that looks for the patterns.</param>
    /// <returns><c>true</c> when every address resolved.</returns>
    public bool Resolve(string cachePath, ISignatureScanner scanner)
    {
        ArgumentException.ThrowIfNullOrEmpty(cachePath);
        ArgumentNullException.ThrowIfNull(scanner);

        var directory = Path.GetDirectoryName(cachePath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var fingerprint = Fingerprint();
        LoadCache(cachePath, fingerprint);

        var failed = new List<string>();
        foreach (var address in _addresses.Values)
        {
            if (_cached.TryGetValue(address.Name, out var cached) && cached != nint.Zero)
            {
                address.Value = cached;
                continue;
            }

            var found = scanner.ScanTextRaw(address.Signature);
            if (found == nint.Zero)
            {
                failed.Add(address.Name);
                continue;
            }

            address.Value = Follow(found, address);
        }

        Unresolved = failed;
        SaveCache(cachePath, fingerprint);

        return failed.Count == 0;
    }

    /// <summary>
    /// Walks the distances that lead from the place the pattern was found to
    /// the address itself.
    /// </summary>
    /// <param name="found">The place the pattern was found.</param>
    /// <param name="address">The address that describes the walk.</param>
    /// <returns>The address of the function or of the place.</returns>
    private static unsafe nint Follow(nint found, GameAddress address)
    {
        var location = found;

        foreach (var relativeFollowOffset in address.RelativeFollowOffsets)
        {
            var distance = GameAddress.ReadDistance(location + relativeFollowOffset);
            location += relativeFollowOffset + 4 + distance;
        }

        return address.IsPointer && location != nint.Zero ? *(nint*)location : location;
    }

    /// <summary>
    /// Reads the addresses of a previous run from the given file.
    /// </summary>
    /// <param name="cachePath">The path of the file.</param>
    /// <param name="fingerprint">The fingerprint of the game the addresses must belong to.</param>
    private void LoadCache(string cachePath, string fingerprint)
    {
        if (!File.Exists(cachePath))
        {
            return;
        }

        try
        {
            var json = File.ReadAllText(cachePath);
            var cache = JsonSerializer.Deserialize<CacheFile>(json, CacheJsonOptions);
            if (cache is null || cache.Version != CacheVersion)
            {
                return;
            }

            // The addresses of another build of the game are of no use here,
            // so the file is ignored when it belongs to a different one.
            if (!string.Equals(cache.Fingerprint, fingerprint, StringComparison.Ordinal))
            {
                return;
            }

            foreach (var entry in cache.Addresses)
            {
                _cached[entry.Key] = (nint)entry.Value;
            }
        }
        catch (Exception)
        {
            // A file that cannot be read is of no use. The addresses are
            // searched for again, which is slower but always works.
        }
    }

    /// <summary>
    /// Writes the addresses of this run to the given file.
    /// </summary>
    /// <param name="cachePath">The path of the file.</param>
    /// <param name="fingerprint">The fingerprint of the game the addresses belong to.</param>
    private void SaveCache(string cachePath, string fingerprint)
    {
        try
        {
            var cache = new CacheFile { Version = CacheVersion, Fingerprint = fingerprint, Addresses = [] };

            foreach (var address in _addresses.Values)
            {
                if (address.Value != nint.Zero)
                {
                    cache.Addresses[address.Name] = (long)address.Value;
                }
            }

            File.WriteAllText(cachePath, JsonSerializer.Serialize(cache, CacheJsonOptions));
        }
        catch (Exception)
        {
            // The file is a convenience that saves a search on the next
            // start. Not being able to write it is not a failure.
        }
    }

    /// <summary>The shape of the file the addresses are written to.</summary>
    private sealed class CacheFile
    {
        /// <summary>Gets or sets the version of the file.</summary>
        public string Version { get; set; } = string.Empty;

        /// <summary>Gets or sets the fingerprint of the game the addresses belong to.</summary>
        public string Fingerprint { get; set; } = string.Empty;

        /// <summary>Gets or sets the addresses.</summary>
        public Dictionary<string, long> Addresses { get; set; } = new(StringComparer.Ordinal);
    }

    /// <summary>The way the file is written and read.</summary>
    private static readonly JsonSerializerOptions CacheJsonOptions = new()
    {
        WriteIndented = true,
    };
}
