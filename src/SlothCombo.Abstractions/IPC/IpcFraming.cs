using System;
using System.Buffers.Binary;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SlothCombo.Abstractions;

/// <summary>
/// Reads and writes the messages of the pipe.
/// </summary>
/// <remarks>
/// Every message is a number that tells how long the text of the message is,
/// followed by the text itself. The number is written in the order of the
/// machine, so that both sides read it the same way.
/// </remarks>
public static class IpcFraming
{
    /// <summary>The most a single message may be long.</summary>
    private const int MaxMessageLength = 32 * 1024 * 1024;

    /// <summary>
    /// Reads a message.
    /// </summary>
    /// <typeparam name="T">The kind of the message.</typeparam>
    /// <param name="stream">The stream to read from.</param>
    /// <param name="token">The token that stops the wait.</param>
    /// <returns>The message.</returns>
    public static async Task<T> ReadAsync<T>(Stream stream, CancellationToken token)
    {
        var length = new byte[4];
        await ReadExactlyAsync(stream, length, token).ConfigureAwait(false);

        var size = BinaryPrimitives.ReadInt32LittleEndian(length);
        if (size <= 0 || size > MaxMessageLength)
        {
            throw new InvalidDataException($"The message of {size} bytes is not a message that can be read.");
        }

        var payload = new byte[size];
        await ReadExactlyAsync(stream, payload, token).ConfigureAwait(false);

        var message = JsonSerializer.Deserialize<T>(payload);
        if (message is null)
        {
            throw new InvalidDataException("The message could not be read.");
        }

        return message;
    }

    /// <summary>
    /// Writes a message.
    /// </summary>
    /// <typeparam name="T">The kind of the message.</typeparam>
    /// <param name="stream">The stream to write to.</param>
    /// <param name="message">The message.</param>
    /// <param name="token">The token that stops the wait.</param>
    public static async Task WriteAsync<T>(Stream stream, T message, CancellationToken token)
    {
        var payload = JsonSerializer.SerializeToUtf8Bytes(message);

        var length = new byte[4];
        BinaryPrimitives.WriteInt32LittleEndian(length, payload.Length);

        await stream.WriteAsync(length, token).ConfigureAwait(false);
        await stream.WriteAsync(payload, token).ConfigureAwait(false);
        await stream.FlushAsync(token).ConfigureAwait(false);
    }

    /// <summary>
    /// Reads a number of bytes, no matter how many of them arrive at once.
    /// </summary>
    private static async Task ReadExactlyAsync(Stream stream, byte[] buffer, CancellationToken token)
    {
        var read = 0;
        while (read < buffer.Length)
        {
            var count = await stream.ReadAsync(buffer.AsMemory(read), token).ConfigureAwait(false);
            if (count == 0)
            {
                throw new EndOfStreamException("The connection was closed before the message was complete.");
            }

            read += count;
        }
    }
}
