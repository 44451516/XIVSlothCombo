using System;

namespace SlothCombo.Interop;

/// <summary>
/// The address of a function of the game, or of a place the game keeps.
/// </summary>
/// <remarks>
/// <para>
/// An address is written down as the pattern of the bytes that lead to it.
/// The pattern is searched for in the code of the game when the runtime
/// starts, and the result is written to a file so that the next start does
/// not have to search again.
/// </para>
/// <para>
/// A pattern that starts with a call or a jump instruction stands for the
/// function that instruction calls, because the game reaches most of its
/// functions that way. The address of that function is found by reading the
/// distance the instruction jumps over.
/// </para>
/// </remarks>
public sealed class GameAddress
{
    /// <summary>The name of the address, which is also the name of its entry.</summary>
    public string Name { get; }

    /// <summary>The pattern of the bytes that lead to the address.</summary>
    public string Signature { get; }

    /// <summary>
    /// The places that hold a distance that leads to the address.
    /// </summary>
    /// <remarks>
    /// Every entry moves the address on by the number of bytes of the
    /// distance and by the distance itself. A pattern that starts with a call
    /// has one such place, the distance of the call. A pattern that leads to
    /// a place that holds a pointer has one as well.
    /// </remarks>
    public ushort[] RelativeFollowOffsets { get; }

    /// <summary>
    /// Whether the place the pattern leads to holds the address instead of
    /// being the address.
    /// </summary>
    public bool IsPointer { get; }

    /// <summary>The address the pattern was resolved to.</summary>
    public nint Value { get; internal set; }

    /// <summary>Creates an address.</summary>
    /// <param name="name">The name of the address.</param>
    /// <param name="signature">The pattern of the bytes that lead to it.</param>
    /// <param name="relativeFollowOffsets">The places that hold a distance.</param>
    /// <param name="isPointer">Whether the place holds a pointer.</param>
    public GameAddress(string name, string signature, ushort[] relativeFollowOffsets, bool isPointer = false)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        ArgumentException.ThrowIfNullOrEmpty(signature);
        ArgumentNullException.ThrowIfNull(relativeFollowOffsets);

        Name = name;
        Signature = signature;
        RelativeFollowOffsets = relativeFollowOffsets;
        IsPointer = isPointer;
    }

    /// <summary>
    /// Creates the address of a function of the game.
    /// </summary>
    /// <param name="name">The name of the address.</param>
    /// <param name="signature">The pattern of the bytes that lead to it.</param>
    /// <returns>The address.</returns>
    /// <remarks>
    /// A pattern that starts with a call or a jump stands for the function
    /// that instruction reaches, which is found by reading the distance of
    /// the instruction. Every other pattern stands for the function itself.
    /// </remarks>
    public static GameAddress Member(string name, string signature)
    {
        ArgumentException.ThrowIfNullOrEmpty(signature);

        var follows = signature.StartsWith("E8", StringComparison.Ordinal)
                      || signature.StartsWith("E9", StringComparison.Ordinal)
            ? new ushort[] { 1 }
            : [];

        return new GameAddress(name, signature, follows);
    }

    /// <summary>
    /// Creates the address of a place the game keeps.
    /// </summary>
    /// <param name="name">The name of the address.</param>
    /// <param name="signature">The pattern of the bytes that lead to it.</param>
    /// <param name="relativeFollowOffset">The place that holds the distance.</param>
    /// <param name="isPointer">Whether the place holds a pointer.</param>
    /// <returns>The address.</returns>
    public static GameAddress Static(string name, string signature, ushort relativeFollowOffset, bool isPointer = false)
        => new(name, signature, [relativeFollowOffset], isPointer);

    /// <summary>
    /// Reads the distance that is stored at the given place.
    /// </summary>
    /// <param name="address">The place that holds the distance.</param>
    /// <returns>The distance.</returns>
    internal static unsafe int ReadDistance(nint address) => *(int*)address;
}
