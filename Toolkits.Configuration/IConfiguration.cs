using System.ComponentModel;

namespace Toolkits.Configuration;

public interface IConfiguration
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    void Clear();

    T Get<T>(string key, T defaultValue = default!);
    void Set<T>(string key, T value);
}
