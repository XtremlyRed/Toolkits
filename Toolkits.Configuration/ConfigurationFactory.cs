using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toolkits.Configuration;

/// <summary>
///
/// </summary>
public static class ConfigurationFactory
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static ConcurrentDictionary<string, IConfiguration> configurationMaps = new();

    /// <summary>
    /// Gets the configuration.
    /// </summary>
    /// <param name="configurationPath">The configuration path.</param>
    /// <param name="configurationType">Type of the configuration.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">configurationPath</exception>
    public static IConfiguration GetConfiguration(
        string configurationPath,
        ConfigurationType configurationType = ConfigurationType.Json
    )
    {
        _ = string.IsNullOrWhiteSpace(configurationPath)
            ? throw new ArgumentNullException(nameof(configurationPath))
            : 0;

        var fileInfo = new FileInfo(configurationPath);

        if (configurationMaps.TryGetValue(fileInfo.FullName, out var config) == false)
        {
            lock (configurationMaps)
            {
                if (fileInfo.Directory!.Exists == false)
                {
                    fileInfo.Directory.Create();
                }
            }

            configurationMaps[fileInfo.FullName] = config = configurationType switch
            {
                ConfigurationType.Json => new JsonConfiguration(fileInfo),
                ConfigurationType.Xml => new XMLConfiguration(fileInfo),
                ConfigurationType.Binary => new BinaryConfiguration(fileInfo),

                _ => throw new NotSupportedException()
            };
        }

        return config!;
    }
}
