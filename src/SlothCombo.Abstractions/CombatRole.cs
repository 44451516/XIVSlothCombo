namespace SlothCombo.Abstractions;

/// <summary>
/// The combat role of a job.
/// </summary>
public enum CombatRole
{
    /// <summary>The job has no combat role.</summary>
    None = 0,

    /// <summary>The job is a tank.</summary>
    Tank = 1,

    /// <summary>The job is a healer.</summary>
    Healer = 2,

    /// <summary>The job deals damage.</summary>
    Dps = 3,
}
