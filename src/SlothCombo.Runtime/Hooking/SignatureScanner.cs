using System;
using System.Collections.Generic;
using System.Diagnostics;
using SlothCombo.Abstractions;

namespace SlothCombo.Runtime.Hooking;

/// <summary>
/// Scans the game module for byte patterns.
/// </summary>
/// <remarks>
/// <para>
/// This is the replacement for the Dalamud signature scanner service. The
/// runtime lives inside the game process, so the module that is searched is
/// the main module of the current process.
/// </para>
/// <para>
/// A pattern that starts with a call or a jump instruction resolves to the
/// target of that instruction rather than to the instruction itself, which is
/// the behaviour the legacy plugin was written against.
/// </para>
/// </remarks>
internal sealed unsafe class SignatureScanner : ISignatureScanner
{
    /// <summary>
    /// The scanner used by the hook factory.
    /// </summary>
    /// <remarks>
    /// The hook factory needs a scanner to resolve signatures, but it is
    /// created before the runtime has a chance to set one up. The current
    /// instance is published here so the two stay in sync.
    /// </remarks>
    public static ISignatureScanner? Current { get; private set; }

    /// <summary>
    /// Lets go of the scanner of the runtime that is being stopped.
    /// </summary>
    internal static void Reset() => Current = null;

    private readonly IntPtr _moduleBase;
    private readonly IntPtr _textBase;
    private readonly int _textSize;
    private readonly Dictionary<string, IntPtr> _resolved = new(StringComparer.Ordinal);

    /// <summary>Creates a scanner for the module of the current process.</summary>
    public SignatureScanner()
    {
        var module = Process.GetCurrentProcess().MainModule;
        if (module is null)
        {
            throw new InvalidOperationException("The main module of the game could not be read.");
        }

        _moduleBase = module.BaseAddress;
        (_textBase, _textSize) = FindTextSection(_moduleBase);

        Current = this;
    }

    /// <inheritdoc/>
    public IntPtr ScanText(string signature)
    {
        ArgumentException.ThrowIfNullOrEmpty(signature);

        if (_resolved.TryGetValue(signature, out var cached))
        {
            return cached;
        }

        var address = Scan(signature);
        _resolved[signature] = address;
        return address;
    }

    /// <inheritdoc/>
    public IntPtr ScanText(string signature, int offset, bool isPointer = false)
    {
        var address = ScanText(signature);
        if (address == IntPtr.Zero)
        {
            return IntPtr.Zero;
        }

        var target = address + offset;

        // When the matched location holds a pointer to the static rather than
        // the static itself, the pointer has to be dereferenced.
        return isPointer ? ReadPointer(target) : target;
    }

    /// <summary>
    /// Searches the text section for the given pattern.
    /// </summary>
    /// <param name="signature">The pattern to search for.</param>
    /// <returns>The address of the match, or <c>IntPtr.Zero</c>.</returns>
    private IntPtr Scan(string signature)
    {
        var (needle, wildcard) = Parse(signature);

        // The first byte that is not a wildcard is used to narrow the search
        // down, the rest of the pattern is only checked at the candidates.
        var anchor = -1;
        for (var i = 0; i < needle.Length; i++)
        {
            if (!wildcard[i])
            {
                anchor = i;
                break;
            }
        }

        if (anchor < 0)
        {
            // A pattern made of wildcards only would match everywhere.
            return IntPtr.Zero;
        }

        var text = new ReadOnlySpan<byte>((byte*)_textBase, _textSize);
        var anchorByte = needle[anchor];
        var search = anchor;

        while (search < text.Length)
        {
            var relative = text.Slice(search).IndexOf(anchorByte);
            if (relative < 0)
            {
                break;
            }

            var candidate = search + relative;
            var start = candidate - anchor;
            if (start < 0)
            {
                search = candidate + 1;
                continue;
            }

            if (start + needle.Length > text.Length)
            {
                break;
            }

            if (Matches(text.Slice(start), needle, wildcard))
            {
                return FollowRelativeCall(_textBase + start);
            }

            search = candidate + 1;
        }

        return IntPtr.Zero;
    }

    /// <summary>
    /// Checks whether the text at the given place matches the pattern.
    /// </summary>
    private static bool Matches(ReadOnlySpan<byte> text, byte[] needle, bool[] wildcard)
    {
        for (var i = 0; i < needle.Length; i++)
        {
            if (wildcard[i])
            {
                continue;
            }

            if (text[i] != needle[i])
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Resolves a call or a jump to the function it points at.
    /// </summary>
    /// <remarks>
    /// The legacy plugin used the Dalamud scanner, which follows the relative
    /// offset of a call or a jump when the pattern starts with such an
    /// instruction. The patterns of the plugin are written that way, so the
    /// same rule is applied here.
    /// </remarks>
    private static IntPtr FollowRelativeCall(IntPtr match)
    {
        var opcode = *(byte*)match;
        if (opcode is not (0xE8 or 0xE9))
        {
            return match;
        }

        var relative = *(int*)((byte*)match + 1);
        return match + 5 + relative;
    }

    /// <summary>
    /// Splits a pattern into the bytes to match and the bytes to ignore.
    /// </summary>
    private static (byte[] Needle, bool[] Wildcard) Parse(string signature)
    {
        var tokens = signature.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var needle = new byte[tokens.Length];
        var wildcard = new bool[tokens.Length];

        for (var i = 0; i < tokens.Length; i++)
        {
            var token = tokens[i];
            if (token is "??" or "**")
            {
                wildcard[i] = true;
                continue;
            }

            if (token.Length != 2 || !byte.TryParse(token, System.Globalization.NumberStyles.HexNumber, null, out var value))
            {
                throw new FormatException($"'{token}' is not a byte of a signature.");
            }

            needle[i] = value;
        }

        return (needle, wildcard);
    }

    /// <summary>
    /// Reads the pointer stored at the given address.
    /// </summary>
    private static unsafe IntPtr ReadPointer(IntPtr address)
    {
        if (address == IntPtr.Zero)
        {
            return IntPtr.Zero;
        }

        return *(IntPtr*)address;
    }

    /// <summary>
    /// Finds the text section of the module at the given address.
    /// </summary>
    /// <remarks>
    /// The section headers of the module are walked by hand, which keeps the
    /// runtime free of another native dependency just to read a few offsets.
    /// </remarks>
    private static (IntPtr Text, int Size) FindTextSection(IntPtr moduleBase)
    {
        var module = (byte*)moduleBase;

        // The offset of the header of the module is stored in the first
        // header of the file.
        var headerOffset = *(int*)(module + 0x3C);
        var header = module + headerOffset;

        // The number of sections is part of the file header, which follows
        // the signature of the header of the module.
        var numberOfSections = *(short*)(header + 6);

        // The section headers follow the optional header, which is 240 bytes
        // long for a 64 bit module.
        var section = header + 4 + 20 + 240;

        for (var i = 0; i < numberOfSections; i++)
        {
            var current = section + i * 40;

            // The name of the section is stored as eight bytes, ".text" ends
            // with two zero bytes.
            if (*(long*)current == 0x747865742E)
            {
                var virtualSize = *(int*)(current + 8);
                var virtualAddress = *(int*)(current + 12);
                var rawSize = *(int*)(current + 16);

                return (moduleBase + virtualAddress, Math.Max(virtualSize, rawSize));
            }
        }

        throw new InvalidOperationException("The text section of the game module could not be found.");
    }
}
