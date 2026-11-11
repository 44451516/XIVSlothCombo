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
    public const int MaxMessageLength = 32 * 1024 * 1024;

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
        if (size > MaxMessageLength)
        {
            throw new IpcMessageTooLargeException(size);
        }

        if (size <= 0)
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

    /// <summary>Checks the complete UTF-8 JSON message, including string escaping and its envelope.</summary>
    /// <typeparam name="T">The kind of the message.</typeparam>
    /// <param name="message">The message that would be sent.</param>
    /// <exception cref="IpcMessageTooLargeException">The serialized message exceeds the transport capacity.</exception>
    public static void ValidateMessageSize<T>(T message)
    {
        _ = SerializeMessage(message);
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
        var payload = SerializeMessage(message);

        var length = new byte[4];
        BinaryPrimitives.WriteInt32LittleEndian(length, payload.Length);

        await stream.WriteAsync(length, token).ConfigureAwait(false);
        await stream.WriteAsync(payload, token).ConfigureAwait(false);
        await stream.FlushAsync(token).ConfigureAwait(false);
    }

    private static byte[] SerializeMessage<T>(T message)
    {
        var payload = JsonSerializer.SerializeToUtf8Bytes(message);
        if (payload.Length > MaxMessageLength)
        {
            throw new IpcMessageTooLargeException(payload.Length);
        }

        return payload;
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

/// <summary>A request body or serialized pipe message exceeds the transport capacity.</summary>
public sealed class IpcMessageTooLargeException : IOException
{
    /// <summary>Creates an error carrying the observed size and the applicable capacity.</summary>
    /// <param name="messageSize">The number of bytes observed.</param>
    /// <param name="maximumSize">The maximum number of bytes accepted.</param>
    public IpcMessageTooLargeException(long messageSize, long maximumSize = IpcFraming.MaxMessageLength)
        : base($"The payload of {messageSize} bytes exceeds the transport capacity of {maximumSize} bytes.")
    {
        MessageSize = messageSize;
        MaximumSize = maximumSize;
    }

    /// <summary>The number of bytes observed.</summary>
    public long MessageSize { get; }

    /// <summary>The maximum number of bytes accepted.</summary>
    public long MaximumSize { get; }
}
