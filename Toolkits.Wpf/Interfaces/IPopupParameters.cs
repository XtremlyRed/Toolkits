using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace Toolkits.Wpf;

/// <summary>
/// a <see langword="interface"/> of <see cref="IPopupParameters"/>
/// </summary>
public interface IPopupParameters
{
    /// <summary>
    /// get value from <see cref="IPopupParameters"/>
    /// </summary>
    /// <typeparam name="Target"></typeparam>
    /// <param name="parameterKey"></param>
    /// <returns></returns>
    Target GetValue<Target>(string parameterKey);

    /// <summary>
    ///  set value into <see cref="IPopupParameters"/>
    /// </summary>
    /// <typeparam name="Target"></typeparam>
    /// <param name="parameterKey"></param>
    /// <param name="parameterValue"></param>
    /// <returns></returns>
    IPopupParameters SetValue<Target>(string parameterKey, Target parameterValue);

    /// <summary>
    /// try get value from <see cref="IPopupParameters"/>
    /// </summary>
    /// <typeparam name="Target"></typeparam>
    /// <param name="parameterKey"></param>
    /// <param name="parameterValue"></param>
    /// <returns></returns>
    bool TryGetValue<Target>(string parameterKey, out Target? parameterValue);
}

/// <summary>
/// a class of <see cref="PopupParameters"/>
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
public class PopupParameters : IDisposable, IPopupParameters
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
    /// set value into <see cref="IPopupParameters"/>
    /// </summary>
    /// <param name="parameterKey">parameterKey</param>
    /// <param name="parameterValue">parameterValue</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public IPopupParameters SetValue<Target>(string parameterKey, Target parameterValue)
    {
        if (string.IsNullOrWhiteSpace(parameterKey))
        {
            throw new ArgumentException($"invalid {nameof(parameterKey)}");
        }
        parametersStorage[parameterKey] = parameterValue;
        return this;
    }

    /// <summary>
    /// get value from <see cref="IPopupParameters"/>
    /// </summary>
    /// <typeparam name="Target"></typeparam>
    /// <param name="parameterKey">parameterKey</param>
    /// <param name="parameterValue">parameterValue</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public Target GetValue<Target>(string parameterKey)
    {
        if (string.IsNullOrWhiteSpace(parameterKey))
        {
            throw new ArgumentException($"invalid {nameof(parameterKey)}");
        }

        return (Target)parametersStorage[parameterKey]!;
    }

    /// <summary>
    /// try get value from <see cref="PopupParameters"/>
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
