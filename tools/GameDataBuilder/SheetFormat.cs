using System.Text;

namespace SlothCombo.GameDataBuilder;

/// <summary>
/// The layout of a sheet in the published data.
/// </summary>
/// <remarks>
/// <para>
/// The data is published as a CSV per sheet and per language. The file has
/// four rows of a header before the data starts:
/// </para>
/// <list type="number">
/// <item>the index of every column,</item>
/// <item>the name of every column,</item>
/// <item>the offset of every column in the record of the game,</item>
/// <item>the type of every column.</item>
/// </list>
/// <para>
/// A column without a name is a column the game does not publish under a
/// name of its own. Such a column holds a value that the runtime has no use
/// for, so it is dropped.
/// </para>
/// </remarks>
internal sealed class SheetFormat
{
    /// <summary>The name of the column that holds the id of the row.</summary>
    public const string KeyColumn = "#";

    private readonly string[] _names;
    private readonly string[] _types;

    /// <summary>Creates a format from the header rows of a sheet.</summary>
    /// <param name="nameRow">The row that holds the names of the columns.</param>
    /// <param name="typeRow">The row that holds the types of the columns.</param>
    public SheetFormat(string[] nameRow, string[] typeRow)
    {
        _names = nameRow;
        _types = typeRow;
    }

    /// <summary>Gets the number of columns of the sheet.</summary>
    public int ColumnCount => _names.Length;

    /// <summary>Gets the name of the column at the given index.</summary>
    public string NameAt(int index) => _names[index];

    /// <summary>Gets the type of the column at the given index.</summary>
    public string TypeAt(int index) => _types[index];

    /// <summary>
    /// Finds the index of the column with the given name.
    /// </summary>
    /// <param name="name">The name of the column.</param>
    /// <returns>The index, or <c>-1</c> when there is no such column.</returns>
    public int IndexOfColumn(string name)
    {
        for (var index = 0; index < _names.Length; index++)
        {
            if (string.Equals(_names[index], name, StringComparison.Ordinal))
            {
                return index;
            }
        }

        return -1;
    }
}

/// <summary>
/// Reads the published data of a sheet.
/// </summary>
/// <remarks>
/// The values are already decoded in the published data, so the reader only
/// has to pick the columns apart. The packed booleans are published as
/// <c>True</c> and <c>False</c> in the column they belong to, which saves the
/// reader from having to know how the game packs them.
/// </remarks>
internal sealed class SheetReader
{
    private readonly SheetFormat _format;
    private readonly List<string[]> _rows;

    /// <summary>Creates a reader over the given rows.</summary>
    /// <param name="format">The layout of the sheet.</param>
    /// <param name="rows">The data rows of the sheet.</param>
    public SheetReader(SheetFormat format, List<string[]> rows)
    {
        _format = format;
        _rows = rows;
    }

    /// <summary>Gets the layout of the sheet.</summary>
    public SheetFormat Format => _format;

    /// <summary>Gets the number of rows of the sheet.</summary>
    public int RowCount => _rows.Count;

    /// <summary>Gets the row at the given index.</summary>
    /// <param name="index">The index of the row.</param>
    /// <returns>The row.</returns>
    public string[] RowAt(int index) => _rows[index];

    /// <summary>
    /// Reads the value of a column of a row.
    /// </summary>
    /// <param name="row">The row.</param>
    /// <param name="column">The name of the column.</param>
    /// <returns>The value, or an empty string when the column is not there.</returns>
    public string Value(string[] row, string column)
    {
        var index = _format.IndexOfColumn(column);
        if (index < 0 || index >= row.Length)
        {
            return string.Empty;
        }

        return row[index];
    }
}

/// <summary>
/// Reads the published data of a sheet from a CSV file.
/// </summary>
internal static class SheetParser
{
    /// <summary>
    /// Reads the sheet from the given lines.
    /// </summary>
    /// <param name="lines">The lines of the file.</param>
    /// <returns>The reader over the sheet.</returns>
    /// <exception cref="FormatException">The file is not a sheet of the expected shape.</exception>
    public static SheetReader Parse(IReadOnlyList<string> lines)
    {
        if (lines.Count < 5)
        {
            throw new FormatException("The sheet is too short to hold a header and data.");
        }

        var names = SplitLine(lines[1]);
        var types = SplitLine(lines[3]);

        if (names.Length != types.Length)
        {
            throw new FormatException("The names and the types of the columns do not match.");
        }

        var rows = new List<string[]>(lines.Count - 4);
        for (var index = 4; index < lines.Count; index++)
        {
            var line = lines[index];
            if (line.Length == 0)
            {
                continue;
            }

            rows.Add(SplitLine(line));
        }

        return new SheetReader(new SheetFormat(names, types), rows);
    }

    /// <summary>
    /// Splits a line of the file into its values.
    /// </summary>
    /// <param name="line">The line.</param>
    /// <returns>The values of the line.</returns>
    /// <remarks>
    /// The values may hold a comma inside a pair of quotes, so the line is
    /// walked by hand instead of being handed to a parser of the framework,
    /// which would have to be told about the shape of the file.
    /// </remarks>
    private static string[] SplitLine(string line)
    {
        var values = new List<string>();
        var current = new StringBuilder();
        var quoted = false;

        for (var index = 0; index < line.Length; index++)
        {
            var character = line[index];

            if (quoted)
            {
                if (character == '"')
                {
                    // A pair of quotes inside a quoted value stands for one
                    // quote of the value itself.
                    if (index + 1 < line.Length && line[index + 1] == '"')
                    {
                        current.Append('"');
                        index++;
                        continue;
                    }

                    quoted = false;
                    continue;
                }

                current.Append(character);
                continue;
            }

            switch (character)
            {
                case '"':
                    quoted = true;
                    continue;
                case ',':
                    values.Add(current.ToString());
                    current.Clear();
                    continue;
                default:
                    current.Append(character);
                    continue;
            }
        }

        values.Add(current.ToString());
        return values.ToArray();
    }
}
