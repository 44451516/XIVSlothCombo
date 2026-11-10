using System;
using System.Collections.Generic;
using SlothCombo.Abstractions;

namespace SlothCombo.Interop;

/// <summary>
/// The functions of the game that the program uses.
/// </summary>
/// <remarks>
/// <para>
/// The addresses of the functions are found when the runtime starts, and are
/// kept here so that the structures of the game can reach them.
/// </para>
/// <para>
/// A function is called through a plain pointer, which is what the game
/// expects. The detour of a hook is the only place where the call is
/// replaced.
/// </para>
/// </remarks>
public static class GameFunctions
{
    private static readonly GameAddressTable Table = Create();

    /// <summary>Gets the table that holds the addresses.</summary>
    public static GameAddressTable Addresses => Table;

    /// <summary>Gets the names of the addresses that could not be resolved.</summary>
    public static IReadOnlyList<string> Unresolved => Table.Unresolved;

    /// <summary>
    /// Finds the address of every function the program uses.
    /// </summary>
    /// <param name="cachePath">The path of the file the addresses are read from and written to.</param>
    /// <param name="scanner">The scanner that looks for the patterns.</param>
    /// <returns><c>true</c> when every address resolved.</returns>
    public static bool Resolve(string cachePath, ISignatureScanner scanner)
        => Table.Resolve(cachePath, scanner);

    /// <summary>
    /// Gets the address of the function with the given name.
    /// </summary>
    /// <param name="name">The name of the address.</param>
    /// <returns>The address, or <c>nint.Zero</c> when it did not resolve.</returns>
    public static nint Get(string name) => Table[name]?.Value ?? nint.Zero;

    /// <summary>
    /// Creates the table and fills it with the addresses of the game.
    /// </summary>
    /// <returns>The table.</returns>
    private static GameAddressTable Create()
    {
        var table = new GameAddressTable();
        GameAddresses.Register(table);
        return table;
    }
}
