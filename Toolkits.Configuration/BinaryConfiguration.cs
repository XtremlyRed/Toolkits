using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Toolkits.Configuration;

/// <summary>
/// a class of <see cref="BinaryConfiguration"/>
/// </summary>
/// <seealso cref="Toolkits.Configuration.IConfiguration" />
[DebuggerDisplay("{thisMaps}")]
public class BinaryConfiguration : IConfiguration
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private FileInfo configurationPath;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    Dictionary<string, object> thisMaps = new();

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    SemaphoreSlim asyncLocker = new SemaphoreSlim(1, 1);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    static char[] splitChars = new[] { '.', ' ' };

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    const string intervalChar = ".";

    /// <summary>
    /// Initializes a new instance of the <see cref="BinaryConfiguration"/> class.
    /// </summary>
    /// <param name="configurationPath">The configuration path.</param>
    public BinaryConfiguration(FileInfo configurationPath)
    {
        this.configurationPath = configurationPath;

        if (configurationPath.Exists == false)
        {
            return;
        }

        var buffers = File.ReadAllBytes(configurationPath.FullName);

        thisMaps = Deserialize<Dictionary<string, object>>(buffers);
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

            var valueToken = InternalGet(key);

            if (valueToken is null)
            {
                if (defaultValue is not null)
                {
                    IntervalSet(key, defaultValue);
                }

                return defaultValue;
            }

            if (valueToken is T targetValue)
            {
                return targetValue;
            }

            IntervalSet(key, defaultValue);

            return defaultValue;
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
            thisMaps.Clear();

            Serialize(thisMaps, configurationPath.FullName);
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
            var currentToken = thisMaps!;
            var splits = key.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0, length = splits.Length - 1; i < length; i++)
            {
                var @new = new Dictionary<string, object>();
                currentToken[splits[i]] = @new;
                currentToken = @new;
            }

            currentToken[splits[splits.Length - 1]] = value!;
        }
        else
        {
            thisMaps![key] = value!;
        }

        Serialize(thisMaps, configurationPath.FullName);
    }

    private object InternalGet(string key)
    {
        if (key.Contains(intervalChar))
        {
            var currentToken = thisMaps!;

            var splits = key.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0, length = splits.Length - 1; i < length; i++)
            {
                if (currentToken.Count == 0)
                {
                    return null!;
                }

                if (
                    currentToken.TryGetValue(splits[i], out var value)
                    && value is Dictionary<string, object> map
                )
                {
                    currentToken = map;
                }
            }

            if (currentToken.TryGetValue(splits[splits.Length - 1], out var value2))
            {
                return value2;
            }
        }
        else
        {
            if (thisMaps.TryGetValue(key, out var value3))
            {
                return value3;
            }
        }

        return null!;
    }

    static void Serialize(object obj, string fileName)
    {
        using FileStream fileStream = new FileStream(fileName, FileMode.Create);

        using MemoryStream memoryStream = new MemoryStream();

        BinaryFormatter binaryFormatter = new BinaryFormatter();
        binaryFormatter.Serialize(memoryStream, obj);

        if (memoryStream.CanSeek)
        {
            memoryStream.Seek(0, SeekOrigin.Begin);
        }
        memoryStream.CopyTo(fileStream);

        fileStream.Flush();
    }

    static T Deserialize<T>(byte[] data)
    {
        using MemoryStream memoryStream = new MemoryStream(data);

        BinaryFormatter binaryFormatter = new BinaryFormatter();
        return (T)binaryFormatter.Deserialize(memoryStream);
    }
}
