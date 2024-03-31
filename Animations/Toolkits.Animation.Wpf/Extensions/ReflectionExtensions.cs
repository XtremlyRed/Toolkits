using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Toolkits.Animation.Wpf.Extensions;

[EditorBrowsable(EditorBrowsableState.Never)]
public class ReflectionExtensions
{
    /// <summary>
    ///  get proprety name from expression
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TPropertyType"></typeparam>
    /// <param name="propertySelector">property Selector</param>
    /// <returns></returns>
    /// <Exception cref="ArgumentNullException"></Exception>
    public static string GetPropertyName<TSource, TPropertyType>(
        Expression<Func<TSource, TPropertyType>> propertySelector
    )
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

        return unaryExpression?.Operand is MemberExpression memberExpression2
            ? memberExpression2.Member.Name
            : string.Empty;
    }
}
