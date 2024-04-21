using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Toolkits.Configuration.Internal;

namespace Toolkits.Configuration;

[DebuggerDisplay("{thisToken}")]
public class JsonConfiguration : IConfiguration
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private FileInfo configurationPath;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    JToken? thisToken;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    AsyncLocker asyncLocker = new AsyncLocker(1, 1);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    static JsonSerializerSettings serializerSettings;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    static char[] splitChars = new[] { '.', ' ' };

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    const string intervalChar = ".";

    static JsonConfiguration()
    {
        serializerSettings = new JsonSerializerSettings()
        {
            DateFormatString = "yyyy-MM-dd HH:mm:ss",
            Formatting = Formatting.Indented,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        serializerSettings.Converters.Add(new StringEnumConverter());
        serializerSettings.NullValueHandling = NullValueHandling.Ignore;
    }

    public JsonConfiguration(FileInfo configurationPath)
    {
        this.configurationPath = configurationPath;

        if (configurationPath.Exists == false)
        {
            thisToken = JToken.FromObject(new object());

            return;
        }

        var content = File.ReadAllText(configurationPath.FullName, Encoding.UTF8);

        try
        {
            thisToken = JToken.Parse(content);
        }
        catch
        {
            thisToken = JToken.FromObject(new object());
        }
    }

    public T Get<T>(string key, T defaultValue = default!)
    {
        _ = string.IsNullOrWhiteSpace(key) ? throw new ArgumentNullException(nameof(key)) : 0;

        try
        {
            asyncLocker.Wait();

            var valueToken = InternalGet(key);

            if (valueToken is null)
            {
                if (defaultValue is not null)
                {
                    IntervalSet(key, defaultValue);
                }

                return defaultValue;
            }

            return valueToken!.ToObject<T>()!;
        }
        finally
        {
            asyncLocker.Release();
        }
    }

    public void Set<T>(string key, T value)
    {
        _ = string.IsNullOrWhiteSpace(key) ? throw new ArgumentNullException(nameof(key)) : 0;
        _ = value ?? throw new ArgumentNullException(nameof(value));

        try
        {
            asyncLocker.Wait();

            IntervalSet(key, value);
        }
        finally
        {
            asyncLocker.Release();
        }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void Clear()
    {
        try
        {
            thisToken = JToken.FromObject(new object());

            var jsonString = JsonConvert.SerializeObject(thisToken, serializerSettings);

            File.WriteAllText(configurationPath.FullName, jsonString, Encoding.UTF8);
        }
        finally
        {
            asyncLocker.Release();
        }
    }

    private void IntervalSet<T>(string key, T value)
    {
        if (key.Contains(intervalChar))
        {
            var currentToken = thisToken!;
            var splits = key.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0, length = splits.Length - 1; i < length; i++)
            {
                currentToken[splits[i]] = currentToken = JToken.FromObject(new object());
            }

            currentToken[splits[splits.Length - 1]] = JToken.FromObject(value!);
        }
        else
        {
            thisToken![key] = JToken.FromObject(value!);
        }

        var jsonString = JsonConvert.SerializeObject(thisToken, serializerSettings);

        File.WriteAllText(configurationPath.FullName, jsonString, Encoding.UTF8);
    }

    private JToken InternalGet(string key)
    {
        if (key.Contains(intervalChar))
        {
            var currentToken = thisToken!;

            var splits = key.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0, length = splits.Length; i < length; i++)
            {
                currentToken = currentToken![splits[i]];

                if (currentToken is null)
                {
                    return null!;
                }
            }

            return currentToken;
        }
        else
        {
            return thisToken![key]!;
        }
    }
}
