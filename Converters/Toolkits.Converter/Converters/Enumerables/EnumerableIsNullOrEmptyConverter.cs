using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if ___WPF___
using System.Windows;
using System.Windows.Data;
#endif
#if ___AVALONIA___
using Avalonia;
using Avalonia.Data.Converters;
#endif

namespace Toolkits.Converter;

/// <summary>
///
/// </summary>
public class EnumerableIsNullOrEmptyConverter : IValueConverter
{
    /// <summary>
    /// Converts the specified value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="targetType">Type of the target.</param>
    /// <param name="parameter">The parameter.</param>
    /// <param name="culture">The culture.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">current value type is not {typeof(IEnumerable).FullName}</exception>
    public object Convert(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
    {
        if (value is not IEnumerable enumerable)
        {
            throw new ArgumentException($"current value type is not {typeof(IEnumerable).FullName}");
        }

        foreach (var _ in enumerable)
        {
            return false;
        }

        return true;
    }

    object IValueConverter.ConvertBack(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
