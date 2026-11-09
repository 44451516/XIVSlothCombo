using System.Collections.Generic;

namespace SlothCombo.Core;

/// <summary>
/// The jobs whose features are turned off.
/// </summary>
/// <remarks>
/// The lists are empty at the moment. They are kept because the runtime
/// checks them before it replaces an icon, and because the interface shows
/// which jobs are not supported.
/// </remarks>
public static class DisabledJobs
{
    /// <summary>Gets the jobs whose features are turned off in the fights against the enemies.</summary>
    public static IReadOnlyList<uint> DisabledJobsPVE { get; } = new List<uint>();

    /// <summary>Gets the jobs whose features are turned off in the fights against the other players.</summary>
    public static IReadOnlyList<uint> DisabledJobsPVP { get; } = new List<uint>();
}
