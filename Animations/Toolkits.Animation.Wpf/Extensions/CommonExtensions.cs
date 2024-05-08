using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Toolkits.Wpf;

internal static class CommonExtensions
{
    /// <summary>
    /// Fors the each.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <param name="source">The source.</param>
    /// <param name="loop">The loop.</param>
    public static void ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource> loop)
    {
        if (source is null || loop is null)
        {
            return;
        }

        foreach (var item in source)
        {
            loop(item);
        }
    }

    /// <summary>
    /// get value from range
    /// </summary>
    /// <param name="value">current value</param>
    /// <param name="minValue">min value</param>
    /// <param name="maxValue">max value</param>
    /// <returns></returns>
    public static double FromRange(this double value, double minValue = double.MinValue, double maxValue = double.MaxValue)
    {
        return value < minValue
            ? minValue
            : value > maxValue
                ? maxValue
                : value;
    }

    /// <summary>
    ///  get proprety name from expression
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TPropertyType"></typeparam>
    /// <param name="propertySelector">property Selector</param>
    /// <returns></returns>
    /// <Exception cref="ArgumentNullException"></Exception>
    public static string GetPropertyName<TSource, TPropertyType>(this Expression<Func<TSource, TPropertyType>> propertySelector)
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
