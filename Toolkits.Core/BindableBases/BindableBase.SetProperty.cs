using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Toolkits.Core;

public abstract partial class BindableBase
{
    /// <summary>
    ///  Set Property by propertyName
    /// </summary>
    /// <typeparam name="TType"></typeparam>
    /// <param name="field">old value</param>
    /// <param name="newValue">new value</param>
    /// <param name="propertyName">propertyName</param>
    /// <returns></returns>
    /// <Exception cref="ArgumentNullException"></Exception>
    protected bool SetProperty<TType>(
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
    ///   Set Property by propertyName
    /// </summary>
    /// <typeparam name="TType"></typeparam>
    /// <param name="field">old value</param>
    /// <param name="newValue">new value</param>
    /// <param name="comparer">propety value comparer</param>
    /// <param name="propertyName">propertyName</param>
    /// <returns></returns>
    /// <Exception cref="ArgumentNullException"></Exception>
    protected bool SetProperty<TType>(
        ref TType field,
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

        if (comparer.Equals(field, newValue))
        {
            return false;
        }
        field = newValue;
        RaisePropertyChanged(propertyName);
        return true;
    }

    /// <summary>
    ///   Set Property by propertyName
    /// </summary>
    /// <typeparam name="TType"></typeparam>
    /// <param name="oldValue">old value</param>
    /// <param name="newValue">new value</param>
    /// <param name="callback">property value changed callback</param>
    /// <param name="propertyName">propertyName</param>
    /// <returns></returns>
    /// <Exception cref="ArgumentNullException"></Exception>
    protected bool SetProperty<TType>(
        TType oldValue,
        TType newValue,
        Action<TType> callback,
        [CallerMemberName] string propertyName = null!
    )
    {
        if (propertyName is null)
        {
            throw new ArgumentNullException(nameof(propertyName));
        }

        if (callback is null)
        {
            throw new ArgumentNullException(nameof(callback));
        }

        if (EqualityComparer<TType>.Default.Equals(oldValue, newValue))
        {
            return false;
        }
        callback(newValue);
        RaisePropertyChanged(propertyName);
        return true;
    }
}
