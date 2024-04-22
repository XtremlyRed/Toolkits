using System.ComponentModel;

namespace Toolkits.Configuration;

/// <summary>
///
/// </summary>
public interface IConfiguration
{
    /// <summary>
    /// Clears this instance.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    void Clear();

    /// <summary>
    /// Gets the specified key.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">The key.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns></returns>
    T Get<T>(string key, T defaultValue = default!);

    /// <summary>
    /// Sets the specified key.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    void Set<T>(string key, T value);
}
