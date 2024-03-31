using System;
using System.Collections.Generic;
using System.Text;

namespace Toolkits;

using System.Globalization;

/// <summary>
///
/// </summary>
public class NumberInRangeConverter : NumberRangeConverterBase
{
    /// <summary>
    /// Converts the specified value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="targetType">Type of the target.</param>
    /// <param name="parameter">The parameter.</param>
    /// <param name="culture">The culture.</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public override object Convert(
        object? value,
        Type targetType,
        object? parameter,
        CultureInfo culture
    )
    {
        if (value is not null)
        {
            return false;
        }

        var doubleValue = (double)System.Convert.ChangeType(value, typeof(double))!;

        return MinValue <= doubleValue && doubleValue <= MaxValue;
    }

    /// <summary>
    /// Converts the back.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="targetType">Type of the target.</param>
    /// <param name="parameter">The parameter.</param>
    /// <param name="culture">The culture.</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public override object ConvertBack(
        object? value,
        Type targetType,
        object? parameter,
        CultureInfo culture
    )
    {
        throw new NotImplementedException();
    }
}
