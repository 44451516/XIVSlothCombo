using System.Runtime.CompilerServices;

namespace SlothCombo.Interop;

/// <summary>
/// Turns a structure that is reached through a pointer back into that pointer.
/// </summary>
/// <remarks>
/// <para>
/// The functions of the game take the address of the structure they belong to
/// as their first argument. A method written in C# only holds the structure
/// itself, so the address has to be taken again before the call.
/// </para>
/// <para>
/// This only makes sense for a structure that lives in the memory of the game.
/// A copy of a structure that was made in C# would hand the game the address
/// of the copy, which is not what it expects.
/// </para>
/// </remarks>
public static unsafe class StructPtr
{
    /// <summary>Gets the address of the given structure.</summary>
    /// <typeparam name="T">The kind of the structure.</typeparam>
    /// <param name="value">The structure, reached through a pointer.</param>
    /// <returns>The address of the structure.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T* Of<T>(ref T value) where T : unmanaged => (T*)Unsafe.AsPointer(ref value);
}
