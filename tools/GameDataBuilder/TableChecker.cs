using System.IO.Compression;
using System.Text;
using System.Text.Json;

namespace SlothCombo.GameDataBuilder;

/// <summary>
/// Reads a table back, the way the runtime reads it.
/// </summary>
/// <remarks>
/// The tool writes the tables by hand, which is fast but easy to get wrong: a
/// number that does not fit the field it is written into makes the runtime
/// fail when it reads the table. The table is therefore read back here, with
/// the very same reader the runtime uses, so that a table that cannot be read
/// never leaves the tool.
/// </remarks>
internal static class TableChecker
{
    /// <summary>
    /// Reads the table at the given path back.
    /// </summary>
    /// <param name="sheetName">The name of the sheet the table holds.</param>
    /// <param name="path">The path of the table.</param>
    /// <returns>The number of the rows of the table.</returns>
    /// <exception cref="InvalidOperationException">The table cannot be read.</exception>
    public static int Check(string sheetName, string path)
    {
        var rowType = typeof(SlothCombo.Abstractions.GameData.GameRow).Assembly
            .GetType($"SlothCombo.Abstractions.GameData.{sheetName}Row");

        if (rowType is null)
        {
            throw new InvalidOperationException($"There is no row for the sheet '{sheetName}'.");
        }

        var listType = typeof(List<>).MakeGenericType(rowType);

        try
        {
            using var file = File.OpenRead(path);
            using var compressed = new GZipStream(file, CompressionMode.Decompress);
            using var reader = new StreamReader(compressed, Encoding.UTF8);

            var rows = JsonSerializer.Deserialize(reader.ReadToEnd(), listType);
            if (rows is null)
            {
                throw new InvalidOperationException($"The table '{path}' holds no rows.");
            }

            return (int)(rows.GetType().GetProperty(nameof(List<int>.Count))?.GetValue(rows) ?? 0);
        }
        catch (JsonException exception)
        {
            throw new InvalidOperationException(
                $"The table '{path}' cannot be read: {exception.Message}", exception);
        }
    }
}
