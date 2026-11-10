using System;
using System.Text;
using SlothCombo.Interop.UI;
using SlothCombo.Abstractions;

namespace SlothCombo.Runtime.Services;

/// <summary>
/// The chat output backed by the struct library.
/// </summary>
/// <remarks>
/// The chat log is written through the log module, which appends the message
/// to the local chat history. Nothing is sent to other players, which matches
/// what the Dalamud chat service does for its print helpers.
/// </remarks>
internal sealed unsafe class ChatService : IChatService
{
    /// <inheritdoc/>
    public void Print(string message) => Write(message);

    /// <inheritdoc/>
    public void PrintError(string message) => Write(message);

    /// <summary>
    /// Writes a line into the chat log.
    /// </summary>
    /// <param name="message">The message to write.</param>
    private static void Write(string message)
    {
        try
        {
            var log = RaptureLogModule.Instance();
            if (log is null)
            {
                return;
            }

            log->PrintString(message);
        }
        catch (Exception)
        {
            // Chat output is a convenience, it must never break the caller.
        }
    }
}
