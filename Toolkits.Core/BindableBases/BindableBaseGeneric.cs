using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

namespace Toolkits.Core;

/// <summary>
/// create a new instance of the  <typeparamref name="TViewModel"/> class
/// </summary>
/// <typeparam name="TViewModel"></typeparam>
public abstract class BindableBase<TViewModel> : BindableBase
    where TViewModel : BindableBase<TViewModel>, new()
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    private static readonly Lazy<TViewModel> Lazy =
        new(LazyThreadSafetyMode.ExecutionAndPublication);

    /// <summary>
    /// create static  instance  of the <typeparamref name="TViewModel"/>
    /// </summary>
    public static TViewModel Instance => Lazy.Value;

    /// <summary>
    ///  notify property value changed
    /// </summary>
    /// <typeparam name="TPropertyType">property type</typeparam>
    /// <param name="propertyExpression">the Expression of the property changed</param>
    /// <Exception cref="ArgumentException"></Exception>
    public virtual void RaisePropertyChanged<TPropertyType>(
        Expression<Func<TViewModel, TPropertyType>> propertyExpression
    )
    {
        if (propertyExpression is null)
        {
            throw new ArgumentException($"{nameof(propertyExpression)} is Null");
        }

        string propertyName = ReflectionExtensions.GetPropertyName(propertyExpression);

        RaisePropertyChanged(propertyName);
    }

    /// <summary>
    /// notify property collection changed
    /// </summary>
    /// <param name="propertyExpressions"></param>
    protected virtual void RaisePropertiesChanged(
        params Expression<Func<TViewModel, object>>[] propertyExpressions
    )
    {
        if (propertyExpressions == null || propertyExpressions.Length == 0)
        {
            return;
        }

        string[] propertyNames = propertyExpressions
            .Where(i => i != null)
            .Select(ReflectionExtensions.GetPropertyName)
            .ToArray();

        RaisePropertyChanged(propertyNames);
    }
}
