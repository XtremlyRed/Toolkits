using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace Toolkits;

/// <summary>
/// a class of <see cref="Parameters"/>
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
public class Parameters : IDisposable
{
    [EditorBrowsable(EditorBrowsableState.Never), DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private Dictionary<string, object?> parametersStorage = new();

    void IDisposable.Dispose()
    {
        parametersStorage?.Clear();
        parametersStorage = null!;

        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// set value into <see cref="Parameters"/>
    /// </summary>
    /// <param name="parameterKey">parameterKey</param>
    /// <param name="parameterValue">parameterValue</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public Parameters SetValue(string parameterKey, object? parameterValue)
    {
        if (string.IsNullOrWhiteSpace(parameterKey))
        {
            throw new ArgumentException($"invalid {nameof(parameterKey)}");
        }

        parametersStorage[parameterKey] = parameterValue;

        return this;
    }

    /// <summary>
    /// get value from <see cref="Parameters"/>
    /// </summary>
    /// <typeparam name="Target"></typeparam>
    /// <param name="parameterKey">parameterKey</param>
    /// <param name="parameterValue">parameterValue</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public Parameters GetValue<Target>(string parameterKey, out Target? parameterValue)
    {
        if (string.IsNullOrWhiteSpace(parameterKey))
        {
            throw new ArgumentException($"invalid {nameof(parameterKey)}");
        }

        if (parametersStorage.TryGetValue(parameterKey, out object? value))
        {
            if (value is Target target)
            {
                parameterValue = target;
                return this;
            }
        }
        parameterValue = default;
        return this;
    }

    /// <summary>
    /// try get value from <see cref="Parameters"/>
    /// </summary>
    /// <typeparam name="Target"></typeparam>
    /// <param name="parameterKey">parameterKey</param>
    /// <param name="parameterValue">parameterValue</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public bool TryGetValue<Target>(string parameterKey, out Target? parameterValue)
    {
        if (string.IsNullOrWhiteSpace(parameterKey))
        {
            throw new ArgumentException($"invalid {nameof(parameterKey)}");
        }

        if (parametersStorage.TryGetValue(parameterKey, out object? value))
        {
            if (value is Target target)
            {
                parameterValue = target;
                return true;
            }
        }
        parameterValue = default;
        return false;
    }

    #region hide base function

    /// <summary>
    /// Determines whether the specified object is equal toType the current object.
    /// </summary>
    /// <param name="obj"> The object toType compare with the current object.</param>
    /// <returns>true if the specified object is equal toType the current object; otherwise, false.</returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override bool Equals(object? obj)
    {
        return base.Equals(obj);
    }

    /// <summary>
    ///  Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current object.</returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    /// <summary>
    /// Returns a string that represents the current object.
    /// </summary>
    /// <returns>A string that represents the current object.</returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override string? ToString()
    {
        return base.ToString();
    }

    #endregion
}
