using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SlothCombo.Host.Injection;

/// <summary>
/// Reads the exports of a program file.
/// </summary>
/// <remarks>
/// <para>
/// The injector has to know where an export of the boot library sits inside
/// the library, because the address of the export in the game is the address
/// of the export in this program plus the place the library was loaded at in
/// the game.
/// </para>
/// <para>
/// The file is read the way the loader of the system reads it: the headers
/// come first, and every address in them is relative to the start of the
/// image until it is turned into a place in the file by the table of the
/// sections.
/// </para>
/// </remarks>
internal static class PeExportReader
{
    /// <summary>The mark at the start of every program file.</summary>
    private const ushort DosSignature = 0x5A4D;

    /// <summary>The mark at the start of the header of a program file.</summary>
    private const uint NtSignature = 0x00004550;

    /// <summary>The mark of a header that uses four byte addresses.</summary>
    private const ushort Pe32Magic = 0x010B;

    /// <summary>The mark of a header that uses eight byte addresses.</summary>
    private const ushort Pe32PlusMagic = 0x020B;

    /// <summary>The place of the table of the exports in the header.</summary>
    private const int ExportDirectoryIndex = 0;

    /// <summary>The size of the header of a program file.</summary>
    private const int SectionHeaderSize = 40;

    /// <summary>
    /// Reads where an export sits inside the given file.
    /// </summary>
    /// <param name="filePath">The path of the file to read.</param>
    /// <param name="exportName">The name of the export to look for.</param>
    /// <returns>The offset of the export, relative to the start of the image.</returns>
    /// <exception cref="InvalidDataException">The file has no such export.</exception>
    public static int GetExportRva(string filePath, string exportName)
    {
        using var stream = File.OpenRead(filePath);
        using var reader = new BinaryReader(stream);

        if (reader.ReadUInt16() != DosSignature)
        {
            throw new InvalidDataException($"'{filePath}' is not a program file.");
        }

        stream.Position = 0x3C;
        var headerOffset = reader.ReadInt32();

        stream.Position = headerOffset;
        if (reader.ReadUInt32() != NtSignature)
        {
            throw new InvalidDataException($"'{filePath}' has no header of a program file.");
        }

        reader.ReadUInt16(); // The machine the file was made for.
        var sectionCount = reader.ReadUInt16();
        reader.ReadUInt32(); // The moment the file was made.
        reader.ReadUInt32(); // The table of the symbols.
        reader.ReadUInt32(); // The number of the symbols.
        var optionalHeaderSize = reader.ReadUInt16();
        reader.ReadUInt16(); // The marks of the file.

        var optionalHeaderOffset = stream.Position;
        var magic = reader.ReadUInt16();

        var directoriesOffset = optionalHeaderOffset + (magic == Pe32PlusMagic ? 112 : magic == Pe32Magic ? 96 : -1);
        if (directoriesOffset < 0)
        {
            throw new InvalidDataException($"'{filePath}' has a header that is not known.");
        }

        stream.Position = directoriesOffset + (ExportDirectoryIndex * 8);
        var exportRva = reader.ReadUInt32();
        if (exportRva == 0)
        {
            throw new InvalidDataException($"'{filePath}' has no exports.");
        }

        var sections = ReadSections(stream, reader, optionalHeaderOffset + optionalHeaderSize, sectionCount);

        var directoryOffset = ToFileOffset(sections, exportRva, filePath);

        stream.Position = directoryOffset + 28;
        var functionsOffset = reader.ReadUInt32();
        stream.Position = directoryOffset + 32;
        var namesOffset = reader.ReadUInt32();
        stream.Position = directoryOffset + 36;
        var ordinalsOffset = reader.ReadUInt32();
        stream.Position = directoryOffset + 24;
        var nameCount = reader.ReadUInt32();

        if (nameCount == 0 || namesOffset == 0)
        {
            throw new InvalidDataException($"'{filePath}' has no named exports.");
        }

        var namesFileOffset = ToFileOffset(sections, namesOffset, filePath);
        var ordinalsFileOffset = ToFileOffset(sections, ordinalsOffset, filePath);
        var functionsFileOffset = ToFileOffset(sections, functionsOffset, filePath);

        for (var index = 0; index < nameCount; index++)
        {
            stream.Position = namesFileOffset + (index * 4);
            var nameRva = reader.ReadUInt32();

            stream.Position = ToFileOffset(sections, nameRva, filePath);
            var name = ReadName(reader);

            if (!string.Equals(name, exportName, StringComparison.Ordinal))
            {
                continue;
            }

            stream.Position = ordinalsFileOffset + (index * 2);
            var ordinal = reader.ReadUInt16();

            stream.Position = functionsFileOffset + (ordinal * 4);
            return unchecked((int)reader.ReadUInt32());
        }

        throw new InvalidDataException($"'{filePath}' has no export named '{exportName}'.");
    }

    /// <summary>
    /// Reads the table of the sections of the file.
    /// </summary>
    private static List<Section> ReadSections(Stream stream, BinaryReader reader, long offset, int count)
    {
        var sections = new List<Section>(count);

        for (var index = 0; index < count; index++)
        {
            stream.Position = offset + (index * SectionHeaderSize);

            reader.ReadBytes(8); // The name of the section.
            var virtualSize = reader.ReadUInt32();
            var virtualAddress = reader.ReadUInt32();
            var rawSize = reader.ReadUInt32();
            var rawPointer = reader.ReadUInt32();

            sections.Add(new Section(virtualAddress, Math.Max(virtualSize, rawSize), rawPointer));
        }

        return sections;
    }

    /// <summary>
    /// Turns an address inside the image into a place inside the file.
    /// </summary>
    private static int ToFileOffset(List<Section> sections, uint address, string filePath)
    {
        foreach (var section in sections)
        {
            if (address >= section.VirtualAddress && address < section.VirtualAddress + section.Size)
            {
                return (int)(section.RawPointer + (address - section.VirtualAddress));
            }
        }

        throw new InvalidDataException($"'{filePath}' has an address outside of its sections.");
    }

    /// <summary>
    /// Reads a name that ends with a zero.
    /// </summary>
    private static string ReadName(BinaryReader reader)
    {
        var bytes = new List<byte>();

        while (true)
        {
            var value = reader.ReadByte();
            if (value == 0)
            {
                break;
            }

            bytes.Add(value);
        }

        return Encoding.ASCII.GetString(bytes.ToArray());
    }

    /// <summary>One section of a program file.</summary>
    private readonly struct Section
    {
        /// <summary>Gets the place of the section inside the image.</summary>
        public uint VirtualAddress { get; }

        /// <summary>Gets the size of the section inside the image.</summary>
        public uint Size { get; }

        /// <summary>Gets the place of the section inside the file.</summary>
        public uint RawPointer { get; }

        /// <summary>Creates the section.</summary>
        public Section(uint virtualAddress, uint size, uint rawPointer)
        {
            VirtualAddress = virtualAddress;
            Size = size;
            RawPointer = rawPointer;
        }
    }
}
