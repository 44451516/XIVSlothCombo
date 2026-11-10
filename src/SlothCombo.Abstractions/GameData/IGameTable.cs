using System.Collections.Generic;

namespace SlothCombo.Abstractions.GameData;

/// <summary>
/// A table of the game, held in memory.
/// </summary>
/// <typeparam name="T">The kind of the rows of the table.</typeparam>
/// <remarks>
/// The rows are looked up by their number. The numbers of the rows of the
/// game are close to one another, so the rows are kept in a map, which makes
/// the lookup a plain read.
/// </remarks>
public interface IGameTable<T>
    where T : GameRow
{
    /// <summary>Gets the number of the rows of the table.</summary>
    int Count { get; }

    /// <summary>Gets the rows of the table.</summary>
    IEnumerable<T> Rows { get; }

    /// <summary>Gets the row with the given number.</summary>
    /// <param name="rowId">The number of the row.</param>
    /// <returns>The row, or <c>null</c> when the table does not hold it.</returns>
    T? GetRow(uint rowId);

    /// <summary>
    /// Gets the row with the given number.
    /// </summary>
    /// <param name="rowId">The number of the row.</param>
    /// <param name="row">The row.</param>
    /// <returns><c>true</c> when the table holds the row.</returns>
    bool TryGetRow(uint rowId, out T row);

    /// <summary>Gets a value indicating whether the table holds the given row.</summary>
    /// <param name="rowId">The number of the row.</param>
    /// <returns><c>true</c> when the table holds the row.</returns>
    bool HasRow(uint rowId);
}
