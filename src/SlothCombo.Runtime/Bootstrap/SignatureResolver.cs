using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;
using FFXIVClientStructs.Interop.Generated;
using InteropGenerator.Runtime;
using SlothCombo.Runtime.Services;
using GameFramework = FFXIVClientStructs.FFXIV.Client.System.Framework.Framework;

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
/// The resolver accepts a JSON cache whose offsets are only valid for the
/// executable and struct library that produced them. A game or library update
/// invalidates those offsets before any game state is read.
/// </para>
/// </remarks>
public static class SignatureResolver
{
    /// <summary>The name of the cache file inside the runtime data folder.</summary>
    public const string CacheFileName = "sigcache.json";

    /// <summary>
    /// The cache format version.
    /// </summary>
    /// <remarks>
    /// The executable fingerprint and struct library MVID are appended to this
    /// version. Bumping it also invalidates the cache for an unchanged build.
    /// </remarks>
    public const string CacheVersion = "2";

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

        var version = GetCacheVersion();
        LogCacheState(cache, version);

        var resolver = Resolver.GetInstance;
        resolver.Setup(version: version, cacheFile: cache);

        Addresses.Register();
        resolver.Resolve();

        var failed = resolver.Addresses
            .Where(address => address.Value == 0)
            .Select(address => address.Name)
            .ToList();

        unresolved = failed;

        return failed.Count == 0;
    }

    private static string GetCacheVersion()
    {
        try
        {
            using var process = Process.GetCurrentProcess();
            using var module = process.MainModule
                ?? throw new InvalidOperationException("The game executable could not be located.");
            using var stream = File.OpenRead(module.FileName);
            var executableHash = Convert.ToHexString(SHA256.HashData(stream));
            var structsMvid = typeof(GameFramework).Assembly.ManifestModule.ModuleVersionId;
            return $"{CacheVersion}:{executableHash}:{structsMvid:N}";
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                "The game executable fingerprint could not be read; refusing to reuse signature addresses.", ex);
        }
    }

    private static void LogCacheState(FileInfo cache, string version)
    {
        Service.PluginLog?.Information($"Signature cache fingerprint: {version}");

        if (!cache.Exists)
        {
            Service.PluginLog?.Information("No signature cache was found; scanning the game executable.");
            return;
        }

        try
        {
            using var stream = cache.OpenRead();
            using var json = JsonDocument.Parse(stream);
            if (json.RootElement.TryGetProperty("Version", out var cachedVersion) &&
                cachedVersion.ValueKind == JsonValueKind.String &&
                cachedVersion.GetString() == version)
            {
                Service.PluginLog?.Information("The signature cache fingerprint matches the current build.");
                return;
            }

            Service.PluginLog?.Information(
                "The signature cache fingerprint changed; discarding cached addresses and scanning the game executable.");
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or JsonException or InvalidOperationException)
        {
            Service.PluginLog?.Warning(
                $"The signature cache could not be read; scanning the game executable instead: {ex.Message}");
        }
    }
}
