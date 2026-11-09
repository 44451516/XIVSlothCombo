namespace SlothCombo.Abstractions;

/// <summary>
/// A status effect on an object.
/// </summary>
public interface IStatus
{
    /// <summary>Gets the id of the status effect.</summary>
    uint StatusId { get; }

    /// <summary>Gets the remaining time of the status effect in seconds.</summary>
    float RemainingTime { get; }

    /// <summary>Gets the parameter of the status effect.</summary>
    ushort Param { get; }

    /// <summary>Gets the entity id of the source of the status effect.</summary>
    ulong SourceId { get; }

    /// <summary>Gets the name of the status effect.</summary>
    string Name { get; }
}
