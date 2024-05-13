using System.Collections;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace Toolkits.Wpf.Internal;

/// <summary>
/// reflection extensions
/// </summary>
/// 2024/1/26 13:24
internal static class ReflectionExtensions
{
    /// <summary>
    ///  get proprety name from expression
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TPropertyType"></typeparam>
    /// <param name="propertySelector">property Selector</param>
    /// <returns></returns>
    /// <Exception cref="ArgumentNullException"></Exception>
    public static string GetPropertyName<TSource, TPropertyType>(Expression<Func<TSource, TPropertyType>> propertySelector)
    {
        if (propertySelector is null)
        {
            throw new ArgumentNullException(nameof(propertySelector));
        }

        if (propertySelector.Body is MemberExpression memberExpression)
        {
            return memberExpression.Member.Name;
        }

        UnaryExpression? unaryExpression = propertySelector.Body as UnaryExpression;

        return unaryExpression?.Operand is MemberExpression memberExpression2 ? memberExpression2.Member.Name : string.Empty;
    }
}
