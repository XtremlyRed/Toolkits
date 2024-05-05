using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace Toolkits.Configuration;

/// <summary>
/// a  class of <see cref="JsonConfiguration"/>
/// </summary>
/// <seealso cref="Toolkits.Configuration.IConfiguration" />
[DebuggerDisplay("{thisToken}")]
public class JsonConfiguration : IConfiguration
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly FileInfo configurationPath;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private JToken? thisToken;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly SemaphoreSlim asyncLocker = new SemaphoreSlim(1, 1);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static readonly JsonSerializerSettings serializerSettings;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static readonly char[] splitChars = new[] { '.', ' ' };

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private const string intervalChar = ".";

    /// <summary>
    /// Initializes the <see cref="JsonConfiguration"/> class.
    /// </summary>
    static JsonConfiguration()
    {
        serializerSettings = new JsonSerializerSettings()
        {
            DateFormatString = "yyyy-MM-dd HH:mm:ss",
            Formatting = Formatting.Indented,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore
        };
        serializerSettings.Converters.Add(new StringEnumConverter());
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonConfiguration"/> class.
    /// </summary>
    /// <param name="configurationPath">The configuration path.</param>
    public JsonConfiguration(FileInfo configurationPath)
    {
        this.configurationPath = configurationPath;

        if (configurationPath.Exists == false)
        {
            thisToken = JToken.FromObject(new object());

            return;
        }

        string content = File.ReadAllText(configurationPath.FullName, Encoding.UTF8);

        try
        {
            thisToken = JToken.Parse(content);
        }
        catch
        {
            thisToken = JToken.FromObject(new object());
        }
    }

    /// <summary>
    /// Gets the specified key.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">The key.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">key</exception>
    public T Get<T>(string key, T defaultValue = default!)
    {
        _ = string.IsNullOrWhiteSpace(key) ? throw new ArgumentNullException(nameof(key)) : 0;

        try
        {
            asyncLocker.Wait();

            JToken? valueToken = InternalGet(key);

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

    /// <summary>
    /// Sets the specified key.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    /// <exception cref="ArgumentNullException">
    /// key
    /// or
    /// value
    /// </exception>
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

    /// <summary>
    /// Clears this instance.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public void Clear()
    {
        try
        {
            thisToken = JToken.FromObject(new object());

            string jsonString = JsonConvert.SerializeObject(thisToken, serializerSettings);

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
            JToken currentToken = thisToken!;
            string[] splits = key.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0, length = splits.Length - 1; i < length; i++)
            {
                JToken? token = currentToken[splits[i]];
                if (token is null)
                {
                    currentToken[splits[i]] = token = JToken.FromObject(new object());
                }
                currentToken = token;
            }

            currentToken[splits[splits.Length - 1]] = JToken.FromObject(value!);
        }
        else
        {
            thisToken![key] = JToken.FromObject(value!);
        }

        string jsonString = JsonConvert.SerializeObject(thisToken, serializerSettings);

        File.WriteAllText(configurationPath.FullName, jsonString, Encoding.UTF8);
    }

    private JToken InternalGet(string key)
    {
        if (key.Contains(intervalChar))
        {
            JToken? currentToken = thisToken!;

            string[] splits = key.Contains(intervalChar)
                ? key.Split(splitChars, StringSplitOptions.RemoveEmptyEntries)
                : new[] { key };

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

        return thisToken![key]!;
    }
}
