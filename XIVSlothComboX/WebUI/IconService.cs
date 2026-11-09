using System;
using System.Collections.Generic;
using Dalamud.Interface.Textures;
using Dalamud.Utility;
using ECommons.DalamudServices;
using Lumina.Data.Files;
using XIVSlothComboX.Services;

namespace XIVSlothComboX.WebUI;

/// <summary>
/// Turns game icons into PNG images that the web frontend can display.
/// Reads the raw texture data through Lumina, because Dalamud's texture wrappers
/// do not expose their pixels.
/// </summary>
internal static class IconService
{
    private static readonly Dictionary<uint, byte[]> Cache = new();
    private static readonly object CacheLock = new();

    /// <summary> Game icon id used for the icon of the given job. </summary>
    internal static uint GetJobIconId(uint jobId) => jobId switch
    {
        0 => 62571,
        51 => 62116,
        _ => 62100 + jobId,
    };

    /// <summary>
    /// Returns the PNG bytes of an icon, or <c>null</c> when the icon cannot be read.
    /// Must be called on the main thread.
    /// </summary>
    internal static byte[]? GetPng(uint iconId, bool hd = true)
    {
        var key = (iconId << 1) | (hd ? 1u : 0u);

        lock (CacheLock)
        {
            if (Cache.TryGetValue(key, out var cached)) return cached;
        }

        var png = Encode(iconId, hd);
        if (png == null) return null;

        lock (CacheLock)
        {
            Cache[key] = png;
        }

        return png;
    }

    private static byte[]? Encode(uint iconId, bool hd)
    {
        try
        {
            var lookup = new GameIconLookup(iconId, false, hd);
            var path = Svc.Texture.GetIconPath(lookup);
            if (string.IsNullOrEmpty(path)) return null;

            var tex = Svc.Data.GameData.GetFileFromDisk<TexFile>(path);
            var rgba = tex.GetRgbaImageData();
            var width = tex.Header.Width;
            var height = tex.Header.Height;
            if (width <= 0 || height <= 0) return null;
            if (rgba.Length < width * height * 4) return null;

            return PngEncoder.EncodeRgba(rgba, width, height);
        }
        catch (Exception ex)
        {
            Service.PluginLog.Debug($"Failed to encode icon {iconId}: {ex.Message}");
            return null;
        }
    }

    /// <summary> Drops every cached icon. </summary>
    internal static void ClearCache()
    {
        lock (CacheLock) Cache.Clear();
    }
}
