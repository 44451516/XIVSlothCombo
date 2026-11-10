using System.Globalization;
using System.IO.Compression;
using System.Text;

namespace SlothCombo.GameDataBuilder;

/// <summary>
/// Turns one sheet into the JSON of the table of the program.
/// </summary>
internal static class SheetWriter
{
    /// <summary>
    /// Writes the table of the given sheet.
    /// </summary>
    /// <param name="sheet">The sheet to read.</param>
    /// <param name="plan">The columns that are carried over.</param>
    /// <param name="outputPath">The path of the file to write.</param>
    public static void Write(SheetReader sheet, SheetPlan plan, string outputPath)
    {
        var json = Build(sheet, plan);

        // The tables are carried along with the program, so they are stored
        // compressed. A table of plain text is a good deal bigger than the
        // same table compressed, and the program only ever reads one language
        // at a time.
        using var file = File.Create(outputPath);
        using var compressed = new GZipStream(file, CompressionLevel.SmallestSize);
        using var writer = new StreamWriter(compressed, new UTF8Encoding(false));
        writer.Write(json);
    }

    /// <summary>
    /// Turns the sheet into the JSON of the table of the program.
    /// </summary>
    /// <param name="sheet">The sheet to read.</param>
    /// <param name="plan">The columns that are carried over.</param>
    /// <returns>The JSON of the table.</returns>
    private static string Build(SheetReader sheet, SheetPlan plan)
    {
        var columns = plan.Columns
            .Select(column => (Name: column, Index: sheet.Format.IndexOfColumn(column)))
            .Where(column => column.Index >= 0)
            .ToArray();

        var writer = new StringBuilder();
        writer.Append('[');

        var first = true;

        for (var rowIndex = 0; rowIndex < sheet.RowCount; rowIndex++)
        {
            var row = sheet.RowAt(rowIndex);
            var id = ReadUInt(row, 0);
            if (id is null)
            {
                continue;
            }

            // A row that holds nothing but its number is of no use, so it is
            // left out. That keeps the tables of the big sheets small.
            if (IsEmpty(row, columns))
            {
                continue;
            }

            if (!first)
            {
                writer.Append(',');
            }

            first = false;

            writer.Append("{\"id\":");
            writer.Append(id.Value.ToString(CultureInfo.InvariantCulture));

            foreach (var column in columns)
            {
                // The value is written on its own first, so that a value that
                // is the same as the one a missing field would leave behind can
                // be left out. The tables of the big sheets are mostly made of
                // such values, and leaving them out makes them much smaller.
                var value = new StringBuilder();
                WriteValue(value, sheet, row, column.Index, column.Name);

                if (IsDefault(value))
                {
                    continue;
                }

                writer.Append(",\"");
                writer.Append(ToCamelCase(column.Name));
                writer.Append("\":");
                writer.Append(value);
            }

            writer.Append('}');
        }

        writer.Append(']');

        return writer.ToString();
    }

    /// <summary>
    /// Checks whether a value is the same as the one a missing field leaves.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns><c>true</c> when the value is the default of its kind.</returns>
    private static bool IsDefault(StringBuilder value)
    {
        var text = value.ToString();
        return text is "0" or "false" or "null" or "[]" or "\"\"";
    }

    /// <summary>
    /// Checks whether a row holds nothing but its number.
    /// </summary>
    /// <param name="row">The row.</param>
    /// <param name="columns">The columns that are carried over.</param>
    /// <returns><c>true</c> when the row holds nothing of use.</returns>
    private static bool IsEmpty(string[] row, (string Name, int Index)[] columns)
    {
        foreach (var column in columns)
        {
            if (column.Index >= row.Length)
            {
                continue;
            }

            var value = row[column.Index];
            if (value.Length == 0 || value == "0" || value == "False")
            {
                continue;
            }

            return false;
        }

        return true;
    }

    /// <summary>
    /// Writes the value of a column of a row.
    /// </summary>
    /// <param name="writer">The writer to write to.</param>
    /// <param name="sheet">The sheet the row belongs to.</param>
    /// <param name="row">The row.</param>
    /// <param name="index">The index of the column.</param>
    /// <param name="name">The name of the column.</param>
    private static void WriteValue(StringBuilder writer, SheetReader sheet, string[] row, int index, string name)
    {
        if (index >= row.Length)
        {
            writer.Append("null");
            return;
        }

        // The columns whose name ends with a number hold several values,
        // which are carried over as a list.
        if (name.EndsWith("]", StringComparison.Ordinal))
        {
            WriteArray(writer, sheet, row, name);
            return;
        }

        var value = row[index];
        var type = sheet.Format.TypeAt(index);

        if (type == "String")
        {
            WriteString(writer, value);
            return;
        }

        if (type.StartsWith("PackedBool", StringComparison.Ordinal))
        {
            writer.Append(string.Equals(value, "True", StringComparison.OrdinalIgnoreCase) ? "true" : "false");
            return;
        }

        if (type is "Float" or "Single")
        {
            writer.Append(float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var single)
                ? single.ToString(CultureInfo.InvariantCulture)
                : "0");
            return;
        }

        if (type is "Int64" or "UInt64")
        {
            writer.Append(ulong.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var big)
                ? big.ToString(CultureInfo.InvariantCulture)
                : "0");
            return;
        }

        if (type.StartsWith("Int", StringComparison.Ordinal))
        {
            // The game writes some numbers as negative ones to say that there
            // is none. The rows of the program have no place for that, so the
            // number becomes the smallest one there is.
            writer.Append(long.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var integer)
                ? Math.Max(integer, 0).ToString(CultureInfo.InvariantCulture)
                : "0");
            return;
        }

        if (uint.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var number))
        {
            writer.Append(number.ToString(CultureInfo.InvariantCulture));
            return;
        }

        WriteString(writer, value);
    }

    /// <summary>
    /// Writes the values of the columns that hold several values.
    /// </summary>
    /// <param name="writer">The writer to write to.</param>
    /// <param name="sheet">The sheet the row belongs to.</param>
    /// <param name="row">The row.</param>
    /// <param name="name">The name of the columns.</param>
    private static void WriteArray(StringBuilder writer, SheetReader sheet, string[] row, string name)
    {
        // The name of the columns ends with the number of the last one, so
        // every column of the run is looked up by that name.
        var bracket = name.IndexOf('[', StringComparison.Ordinal);
        if (bracket <= 0)
        {
            writer.Append("[]");
            return;
        }

        var first = sheet.Format.IndexOfColumn(name);
        if (first < 0)
        {
            writer.Append("[]");
            return;
        }

        // The name of the run is the name without the number, so the run of
        // the data of an item is not mistaken for the run of the data of the
        // better item. The columns of the run do not have to follow each
        // other: the food carries a handful of values for every one of its
        // entries, and those are spread over the columns of the sheet.
        var baseName = name[..bracket];
        var indexes = new List<int>();
        for (var index = 0; index < sheet.Format.ColumnCount; index++)
        {
            var columnName = sheet.Format.NameAt(index);
            if (columnName.StartsWith(baseName + "[", StringComparison.Ordinal))
            {
                indexes.Add(index);
            }
        }

        writer.Append('[');

        var written = false;
        foreach (var index in indexes)
        {
            if (index >= row.Length)
            {
                continue;
            }

            if (written)
            {
                writer.Append(',');
            }

            written = true;

            writer.Append(int.TryParse(row[index], NumberStyles.Integer, CultureInfo.InvariantCulture, out var value)
                ? value.ToString(CultureInfo.InvariantCulture)
                : "0");
        }

        writer.Append(']');
    }

    /// <summary>
    /// Writes a text as a piece of JSON.
    /// </summary>
    /// <param name="writer">The writer to write to.</param>
    /// <param name="value">The text.</param>
    private static void WriteString(StringBuilder writer, string value)
    {
        writer.Append('"');

        foreach (var character in value)
        {
            switch (character)
            {
                case '"':
                    writer.Append("\\\"");
                    break;
                case '\\':
                    writer.Append("\\\\");
                    break;
                case '\n':
                    writer.Append("\\n");
                    break;
                case '\r':
                    writer.Append("\\r");
                    break;
                case '\t':
                    writer.Append("\\t");
                    break;
                default:
                    if (character < 0x20)
                    {
                        writer.Append("\\u");
                        writer.Append(((int)character).ToString("x4", CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        writer.Append(character);
                    }

                    break;
            }
        }

        writer.Append('"');
    }

    /// <summary>
    /// Turns the name of a column into the name of the field of the row.
    /// </summary>
    /// <param name="name">The name of the column.</param>
    /// <returns>The name of the field.</returns>
    private static string ToCamelCase(string name)
    {
        if (name.Length == 0)
        {
            return name;
        }

        // The name of the columns that hold several values ends with the
        // number of the last one, which is not part of the name.
        var bracket = name.IndexOf('[', StringComparison.Ordinal);
        if (bracket > 0)
        {
            name = name[..bracket];
        }

        // A name that starts with several capital letters is the start of a
        // word of its own, so the capitals are lowered. The last capital of
        // the run starts the next word, unless the run is the whole name.
        var run = 1;
        while (run < name.Length && char.IsUpper(name[run]))
        {
            run++;
        }

        if (run > 1 && run < name.Length)
        {
            run--;
        }

        var builder = new StringBuilder(name.Length);
        for (var index = 0; index < run; index++)
        {
            builder.Append(char.ToLowerInvariant(name[index]));
        }

        builder.Append(name[run..]);
        return builder.ToString();
    }

    /// <summary>
    /// Reads the number of a row.
    /// </summary>
    /// <param name="row">The row.</param>
    /// <param name="index">The index of the column.</param>
    /// <returns>The number of the row, or <c>null</c> when it has none.</returns>
    private static uint? ReadUInt(string[] row, int index)
    {
        if (index >= row.Length)
        {
            return null;
        }

        return uint.TryParse(row[index], NumberStyles.Integer, CultureInfo.InvariantCulture, out var value)
            ? value
            : null;
    }
}
