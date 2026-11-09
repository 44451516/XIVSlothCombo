using System;
using System.IO;

using SlothCombo.Runtime.WebUI.Contracts;

namespace SlothCombo.Runtime.WebUI;

/// <summary>
/// Minimal PNG writer for RGBA pixel data. Uses uncompressed deflate blocks so no
/// compression library is needed; the payload is served from localhost anyway.
/// </summary>
internal static class PngEncoder
{
    private static readonly byte[] Signature = { 137, 80, 78, 71, 13, 10, 26, 10 };
    private static readonly uint[] CrcTable = BuildCrcTable();

    /// <summary> Encodes RGBA pixels as a PNG image. </summary>
    public static byte[] EncodeRgba(byte[] rgba, int width, int height)
    {
        if (rgba.Length < width * height * 4)
            throw new ArgumentException("像素数据长度不足。", nameof(rgba));

        using var output = new MemoryStream();
        output.Write(Signature, 0, Signature.Length);

        var header = new byte[13];
        WriteBigEndian(header, 0, width);
        WriteBigEndian(header, 4, height);
        header[8] = 8;  // bit depth
        header[9] = 6;  // colour type: truecolour with alpha
        header[10] = 0; // deflate
        header[11] = 0; // adaptive filtering
        header[12] = 0; // no interlacing
        WriteChunk(output, "IHDR", header);

        WriteChunk(output, "IDAT", ZlibStore(AddFilterBytes(rgba, width, height)));
        WriteChunk(output, "IEND", Array.Empty<byte>());

        return output.ToArray();
    }

    private static byte[] AddFilterBytes(byte[] rgba, int width, int height)
    {
        var stride = width * 4;
        var result = new byte[(stride + 1) * height];
        for (var y = 0; y < height; y++)
        {
            var target = y * (stride + 1);
            result[target] = 0; // filter type "none"
            Buffer.BlockCopy(rgba, y * stride, result, target + 1, stride);
        }

        return result;
    }

    /// <summary> Wraps the data in a zlib stream built from stored deflate blocks. </summary>
    private static byte[] ZlibStore(byte[] data)
    {
        using var output = new MemoryStream();

        // zlib header: deflate with a 32k window, no preset dictionary.
        output.WriteByte(0x78);
        output.WriteByte(0x01);

        const int maxBlock = 65535;
        var offset = 0;
        while (offset < data.Length || data.Length == 0)
        {
            var length = Math.Min(maxBlock, data.Length - offset);
            var isFinal = offset + length >= data.Length;

            output.WriteByte((byte)(isFinal ? 1 : 0));
            output.WriteByte((byte)(length & 0xFF));
            output.WriteByte((byte)((length >> 8) & 0xFF));
            output.WriteByte((byte)(~length & 0xFF));
            output.WriteByte((byte)((~length >> 8) & 0xFF));
            output.Write(data, offset, length);

            offset += length;
            if (isFinal) break;
        }

        var adler = Adler32(data);
        output.WriteByte((byte)((adler >> 24) & 0xFF));
        output.WriteByte((byte)((adler >> 16) & 0xFF));
        output.WriteByte((byte)((adler >> 8) & 0xFF));
        output.WriteByte((byte)(adler & 0xFF));

        return output.ToArray();
    }

    private static void WriteChunk(Stream output, string type, byte[] data)
    {
        var length = new byte[4];
        WriteBigEndian(length, 0, data.Length);
        output.Write(length, 0, 4);

        var typeBytes = new byte[4];
        for (var i = 0; i < 4; i++) typeBytes[i] = (byte)type[i];
        output.Write(typeBytes, 0, 4);
        output.Write(data, 0, data.Length);

        var crc = Crc32(typeBytes, data);
        var crcBytes = new byte[4];
        WriteBigEndian(crcBytes, 0, unchecked((int)crc));
        output.Write(crcBytes, 0, 4);
    }

    private static void WriteBigEndian(byte[] target, int offset, int value)
    {
        target[offset] = (byte)((value >> 24) & 0xFF);
        target[offset + 1] = (byte)((value >> 16) & 0xFF);
        target[offset + 2] = (byte)((value >> 8) & 0xFF);
        target[offset + 3] = (byte)(value & 0xFF);
    }

    private static uint[] BuildCrcTable()
    {
        var table = new uint[256];
        for (uint n = 0; n < 256; n++)
        {
            var c = n;
            for (var k = 0; k < 8; k++) c = (c & 1) != 0 ? 0xEDB88320u ^ (c >> 1) : c >> 1;
            table[n] = c;
        }

        return table;
    }

    private static uint Crc32(byte[] type, byte[] data)
    {
        var crc = 0xFFFFFFFFu;
        foreach (var b in type) crc = CrcTable[(crc ^ b) & 0xFF] ^ (crc >> 8);
        foreach (var b in data) crc = CrcTable[(crc ^ b) & 0xFF] ^ (crc >> 8);
        return crc ^ 0xFFFFFFFFu;
    }

    private static uint Adler32(byte[] data)
    {
        uint a = 1, b = 0;
        foreach (var value in data)
        {
            a = (a + value) % 65521;
            b = (b + a) % 65521;
        }

        return (b << 16) | a;
    }
}
