using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Toolkits.Core;

public abstract partial class BindableBase
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private Dictionary<string, object> propertyValueMapper = new();

    /// <summary>
    /// set value with Ref  by propertyName
    /// </summary>
    /// <typeparam name="TType"></typeparam>
    /// <param name="field">old value</param>
    /// <param name="newValue">new value</param>
    /// <param name="propertyName">propertyName</param>
    /// <returns></returns>
    /// <Exception cref="ArgumentNullException"></Exception>
    protected bool SetValue<TType>(
        ref TType field,
        TType newValue,
        [CallerMemberName] string propertyName = null!
    )
    {
        if (propertyName is null)
        {
            throw new ArgumentNullException(nameof(propertyName));
        }

        if (EqualityComparer<TType>.Default.Equals(field, newValue))
        {
            return false;
        }
        field = newValue;
        RaisePropertyChanged(propertyName);
        return true;
    }

    /// <summary>
    /// set value by propertyName
    /// </summary>
    /// <typeparam name="TType"></typeparam>
    /// <param name="newValue">new value</param>
    /// <param name="propertyName">propertyName</param>
    /// <returns></returns>
    /// <Exception cref="ArgumentNullException"></Exception>
    protected bool SetValue<TType>(TType newValue, [CallerMemberName] string propertyName = null!)
    {
        if (propertyName is null)
        {
            throw new ArgumentNullException(nameof(propertyName));
        }

        if (
            propertyValueMapper!.TryGetValue(propertyName, out object? oldValue)
            && oldValue is TType old
        )
        {
            if (EqualityComparer<TType>.Default.Equals(old, newValue))
            {
                return false;
            }
        }
        propertyValueMapper[propertyName] = newValue!;
        RaisePropertyChanged(propertyName);
        return true;
    }

    /// <summary>
    /// set value by propertyName
    /// </summary>
    /// <typeparam name="TType"></typeparam>
    /// <param name="newValue">new value</param>
    /// <param name="comparer">property value comparer</param>
    /// <param name="propertyName">propertyName</param>
    /// <returns></returns>
    /// <Exception cref="ArgumentNullException"></Exception>
    protected bool SetValue<TType>(
        TType newValue,
        IEqualityComparer<TType> comparer,
        [CallerMemberName] string propertyName = null!
    )
    {
        if (propertyName is null)
        {
            throw new ArgumentNullException(nameof(propertyName));
        }

        if (comparer is null)
        {
            throw new ArgumentNullException(nameof(comparer));
        }

        if (
            propertyValueMapper!.TryGetValue(propertyName, out object? oldValue)
            && oldValue is TType old
        )
        {
            if (comparer.Equals(old, newValue))
            {
                return false;
            }
        }
        propertyValueMapper[propertyName] = newValue!;
        RaisePropertyChanged(propertyName);
        return true;
    }

    /// <summary>
    /// get value by propertyName
    /// </summary>
    /// <typeparam name="TType"></typeparam>
    /// <param name="defaultValue">default Array</param>
    /// <param name="propertyName">propertyName</param>
    /// <returns></returns>
    /// <Exception cref="ArgumentNullException"></Exception>
    protected TType GetValue<TType>(
        TType defaultValue = default!,
        [CallerMemberName] string propertyName = null!
    )
    {
        if (propertyName is null)
        {
            throw new ArgumentNullException(nameof(propertyName));
        }

        if (propertyValueMapper!.TryGetValue(propertyName, out object? value) == false)
        {
            propertyValueMapper[propertyName] = defaultValue!;
            return defaultValue;
        }

        return value is TType tValue ? tValue : defaultValue;
    }
}
