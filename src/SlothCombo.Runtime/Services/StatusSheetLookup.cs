using System;

namespace SlothCombo.Runtime.Services;

/// <summary>
/// Resolves the names of the status effects.
/// </summary>
/// <remarks>
/// The status structs of the game only carry the id of the effect, so the name
/// has to come from the static data. The lookup is registered once by the data
/// service, which keeps the object wrappers free of a data dependency.
/// </remarks>
internal static class StatusSheetLookup
{
    private static Func<uint, string>? _resolver;

    /// <summary>Registers the resolver that maps a status id to its name.</summary>
    /// <param name="resolver">The resolver to use, or <c>null</c> to let it go.</param>
    public static void Register(Func<uint, string>? resolver) => _resolver = resolver;

    /// <summary>Releases the resolver, which holds the data service it came from.</summary>
    public static void Unregister() => _resolver = null;

    /// <summary>Gets the name of the status with the given id.</summary>
    /// <param name="statusId">The id of the status.</param>
    /// <returns>The name of the status, or an empty string.</returns>
    public static string GetName(uint statusId)
    {
        try
        {
            return _resolver?.Invoke(statusId) ?? string.Empty;
        }
        catch (Exception)
        {
            return string.Empty;
        }
    }
}
