using System;
using System.IO;
using Newtonsoft.Json;

namespace SlothCombo.Runtime.Configuration;

/// <summary>
/// Reads and writes the plugin configuration.
/// </summary>
/// <remarks>
/// This is the replacement for the Dalamud plugin configuration service. The
/// configuration is stored as a JSON file in the data folder of the runtime.
/// </remarks>
public sealed class ConfigStore
{
    private readonly string _path;
    private readonly object _gate = new();

    /// <summary>Creates a store backed by the given file.</summary>
    /// <param name="path">The path of the configuration file.</param>
    public ConfigStore(string path)
    {
        ArgumentException.ThrowIfNullOrEmpty(path);
        _path = path;
    }

    /// <summary>Gets the path of the configuration file.</summary>
    public string Path => _path;

    /// <summary>
    /// Loads the configuration.
    /// </summary>
    /// <typeparam name="T">The configuration type.</typeparam>
    /// <returns>
    /// The stored configuration, or a new instance when nothing has been stored
    /// yet or the file cannot be read.
    /// </returns>
    public T Load<T>() where T : new()
    {
        lock (_gate)
        {
            try
            {
                if (!File.Exists(_path))
                {
                    return new T();
                }

                var json = File.ReadAllText(_path);
                var config = JsonConvert.DeserializeObject<T>(json);
                return config ?? new T();
            }
            catch (Exception)
            {
                // A damaged configuration must never stop the runtime from
                // starting: the defaults are always usable.
                return new T();
            }
        }
    }

    /// <summary>
    /// Stores the configuration.
    /// </summary>
    /// <typeparam name="T">The configuration type.</typeparam>
    /// <param name="config">The configuration to store.</param>
    public void Save<T>(T config)
    {
        lock (_gate)
        {
            var directory = System.IO.Path.GetDirectoryName(_path);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var json = JsonConvert.SerializeObject(config, Formatting.Indented);

            // The file is written to a temporary location first and then moved
            // into place, so a crash during the write cannot leave a truncated
            // configuration behind.
            var temporary = _path + ".tmp";
            File.WriteAllText(temporary, json);

            if (File.Exists(_path))
            {
                File.Replace(temporary, _path, null);
            }
            else
            {
                File.Move(temporary, _path);
            }
        }
    }
}
