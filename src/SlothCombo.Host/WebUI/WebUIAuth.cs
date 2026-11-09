using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace SlothCombo.Host.WebUI;

/// <summary>
/// Guards the local web server. The server only listens on the loopback interface,
/// but every request still has to prove that it knows the session token and originates
/// from the settings page itself.
/// </summary>
internal sealed class WebUIAuth
{
    private readonly byte[] _tokenBytes;
    private readonly string _token;

    /// <summary> Creates a session token bound to the given port. </summary>
    public WebUIAuth(int port)
    {
        Port = port;
        _tokenBytes = RandomNumberGenerator.GetBytes(32);
        _token = Convert.ToHexString(_tokenBytes);
    }

    /// <summary> Port the server listens on. </summary>
    public int Port { get; }

    /// <summary> The session token that has to be present on every request. </summary>
    public string Token => _token;

    /// <summary> The URL that opens the settings page in the browser. </summary>
    public string Url => $"http://127.0.0.1:{Port}/?t={_token}";

    /// <summary> Whether the request carries the session token and a matching origin. </summary>
    public bool Validate(string? token, string? origin, string? referer)
    {
        if (string.IsNullOrEmpty(token)) return false;
        if (!FixedTimeEquals(token, _token)) return false;
        return IsAllowedOrigin(origin, referer);
    }

    private bool IsAllowedOrigin(string? origin, string? referer)
    {
        var expected = $"http://127.0.0.1:{Port}";

        if (!string.IsNullOrEmpty(origin))
        {
            return origin.StartsWith(expected, StringComparison.Ordinal);
        }

        if (!string.IsNullOrEmpty(referer))
        {
            return referer.StartsWith(expected, StringComparison.Ordinal);
        }

        // No origin information at all; the session token alone has to be enough.
        return true;
    }

    private bool FixedTimeEquals(string candidate, string expected)
    {
        var candidateBytes = Encoding.UTF8.GetBytes(candidate);
        var expectedBytes = Encoding.UTF8.GetBytes(expected);
        if (candidateBytes.Length != expectedBytes.Length) return false;
        return CryptographicOperations.FixedTimeEquals(candidateBytes, expectedBytes);
    }
}
