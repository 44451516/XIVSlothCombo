using System;
using System.Reflection;
using SlothCombo.Abstractions;

namespace SlothCombo.Runtime.Services;

/// <summary>
/// The interface between the runtime and the outside world.
/// </summary>
/// <remarks>
/// This is the replacement for the Dalamud plugin interface. The runtime lives
/// in the game process, so the location of the assembly is the location of the
/// runtime itself and the configuration is written next to it.
/// </remarks>
internal sealed class PluginInterface : IPluginInterface
{
    private readonly Configuration.ConfigStore _configStore;

    /// <summary>Creates the interface over the given configuration store.</summary>
    /// <param name="configStore">The store the configuration is written to.</param>
    public PluginInterface(Configuration.ConfigStore configStore) => _configStore = configStore;

    /// <inheritdoc/>
    public string AssemblyLocation
    {
        get
        {
            var location = Assembly.GetExecutingAssembly().Location;
            return string.IsNullOrEmpty(location)
                ? AppContext.BaseDirectory
                : location;
        }
    }

    /// <inheritdoc/>
    public void SavePluginConfig(object configuration) => _configStore.Save(configuration);
}
