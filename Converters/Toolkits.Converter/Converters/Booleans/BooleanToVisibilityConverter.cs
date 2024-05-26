using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if ___WPF___
using System.Windows;
using System.Windows.Data;

namespace Toolkits.Wpf;

/// <summary>
/// a class of <see cref="BooleanToVisibilityConverter"/>
/// </summary>
/// <seealso cref="IValueConverter" />
public class BooleanToVisibilityConverter : DependencyObject, IValueConverter
{
    /// <summary>
    /// Converts the specified value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="targetType">Type of the target.</param>
    /// <param name="parameter">The parameter.</param>
    /// <param name="culture">The culture.</param>
    /// <returns></returns>
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is not bool boolValue
            ? throw new ArgumentException($"current value type is not {typeof(bool)}")
            : boolValue
                ? Visibility.Visible
                : Visibility.Collapsed;
    }

    /// <summary>
    /// Converts the back.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="targetType">Type of the target.</param>
    /// <param name="parameter">The parameter.</param>
    /// <param name="culture">The culture.</param>
    /// <returns></returns>
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is Visibility.Visible ? true : false;
    }
}
#endif
