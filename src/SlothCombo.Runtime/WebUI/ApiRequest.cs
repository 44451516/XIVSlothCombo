using System;
using System.Collections.Generic;

using SlothCombo.Runtime.WebUI.Contracts;

namespace SlothCombo.Runtime.WebUI;

/// <summary>
/// A request for the interface of the program.
/// </summary>
/// <remarks>
/// The program is started outside of the game and asks the runtime inside the
/// game to do the work. The request carries everything the runtime needs, so
/// that the runtime does not have to know anything about the connection.
/// </remarks>
internal sealed class ApiRequest
{
    private Dictionary<string, string>? _query;

    /// <summary>Gets the method of the request.</summary>
    public string Method { get; init; } = "GET";

    /// <summary>Gets the path of the request.</summary>
    public string Path { get; init; } = "/";

    /// <summary>Gets the query of the request.</summary>
    public string Query { get; init; } = string.Empty;

    /// <summary>Gets the body of the request.</summary>
    public string Body { get; init; } = string.Empty;

    /// <summary>Gets the port the interface of the program listens on.</summary>
    public int Port { get; init; }

    /// <summary>Gets the parts of the path.</summary>
    public string[] Segments => Path.Trim('/').Split('/');

    /// <summary>
    /// Reads a value of the query.
    /// </summary>
    /// <param name="key">The name of the value.</param>
    /// <returns>The value, or <c>null</c> when it is not there.</returns>
    public string? QueryValue(string key)
    {
        _query ??= ParseQuery(Query);

        return _query.TryGetValue(key, out var value) ? value : null;
    }

    /// <summary>
    /// Reads the parts of a query into a table.
    /// </summary>
    private static Dictionary<string, string> ParseQuery(string query)
    {
        var values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        if (string.IsNullOrEmpty(query))
        {
            return values;
        }

        foreach (var part in query.TrimStart('?').Split('&', StringSplitOptions.RemoveEmptyEntries))
        {
            var separator = part.IndexOf('=');
            if (separator < 0)
            {
                values[Uri.UnescapeDataString(part)] = string.Empty;
                continue;
            }

            var name = Uri.UnescapeDataString(part[..separator]);
            var value = Uri.UnescapeDataString(part[(separator + 1)..]);
            values[name] = value;
        }

        return values;
    }
}
