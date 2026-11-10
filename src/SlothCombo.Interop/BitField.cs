using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace SlothCombo.Interop;

/// <summary>
/// Reads a value out of a bit field.
/// </summary>
/// <remarks>
/// The game packs several small values into one number. A bit field is read
/// by shifting the number to the right and keeping only the bits of the
/// field. The bits are counted from the lowest one, which is the order the
/// game uses.
/// </remarks>
public static class BitField
{
    /// <summary>Reads a single bit of a number.</summary>
    /// <param name="value">The number that holds the field.</param>
    /// <param name="index">The number of the bit, counted from the lowest one.</param>
    /// <returns><c>true</c> when the bit is set.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool GetBit(ulong value, int index) => ((value >> index) & 1UL) != 0;

    /// <summary>Reads a field of a number.</summary>
    /// <param name="value">The number that holds the field.</param>
    /// <param name="index">The number of the first bit of the field.</param>
    /// <param name="length">The number of bits of the field.</param>
    /// <returns>The value of the field.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint GetBits(ulong value, int index, int length)
    {
        if (length >= 64)
        {
            return (uint)value;
        }

        var mask = (1UL << length) - 1UL;
        return (uint)((value >> index) & mask);
    }

    /// <summary>Reads a field of a number as a small number.</summary>
    /// <param name="value">The number that holds the field.</param>
    /// <param name="index">The number of the first bit of the field.</param>
    /// <param name="length">The number of bits of the field.</param>
    /// <returns>The value of the field.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte GetByteBits(ulong value, int index, int length) => (byte)GetBits(value, index, length);

    /// <summary>Reads a field of a number as a signed number.</summary>
    /// <param name="value">The number that holds the field.</param>
    /// <param name="index">The number of the first bit of the field.</param>
    /// <param name="length">The number of bits of the field.</param>
    /// <returns>The value of the field.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetIntBits(ulong value, int index, int length) => (int)GetBits(value, index, length);
}
