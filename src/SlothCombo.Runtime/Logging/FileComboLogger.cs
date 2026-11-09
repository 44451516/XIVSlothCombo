using System;
using Serilog;
using SlothCombo.Abstractions;

namespace SlothCombo.Runtime.Logging;

/// <summary>
/// The logger used by the combo logic.
/// </summary>
/// <remarks>
/// This is the replacement for the Dalamud plugin log service. Messages are
/// written to a rolling file so the log survives a game crash, which matters
/// for the hook sites: a crash inside a detour is the most likely failure
/// mode of the standalone runtime.
/// </remarks>
internal sealed class FileComboLogger : IComboLogger
{
    private readonly ILogger _logger;

    /// <summary>Creates a logger writing to the given directory.</summary>
    /// <param name="logDirectory">The directory the log files are written to.</param>
    public FileComboLogger(string logDirectory)
    {
        ArgumentException.ThrowIfNullOrEmpty(logDirectory);

        System.IO.Directory.CreateDirectory(logDirectory);

        _logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File(
                Path.Combine(logDirectory, "sloth-.log"),
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 7,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();
    }

    /// <inheritdoc/>
    public void Verbose(string message) => _logger.Verbose(message);

    /// <inheritdoc/>
    public void Debug(string message) => _logger.Debug(message);

    /// <inheritdoc/>
    public void Information(string message) => _logger.Information(message);

    /// <inheritdoc/>
    public void Warning(string message) => _logger.Warning(message);

    /// <inheritdoc/>
    public void Error(string message) => _logger.Error(message);

    /// <inheritdoc/>
    public void Error(Exception exception, string message) => _logger.Error(exception, message);
}
