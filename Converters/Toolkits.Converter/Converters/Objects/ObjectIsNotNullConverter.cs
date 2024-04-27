#if ___WPF___
using System.Windows;
using System.Windows.Data;
#endif
#if ___AVALONIA___
using Avalonia;
using Avalonia.Data.Converters;
#endif
#if ___MAUI___
using Microsoft.Maui.Graphics.Converters;
using Microsoft.Maui.Media;
#endif

namespace Toolkits;

/// <summary>
///
/// </summary>
public class ObjectIsNotNullConverter : IValueConverter
{
    /// <summary>
    /// Converts the specified value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="targetType">Type of the target.</param>
    /// <param name="parameter">The parameter.</param>
    /// <param name="culture">The culture.</param>
    /// <returns></returns>
    public object Convert(
        object? value,
        Type targetType,
        object? parameter,
        System.Globalization.CultureInfo culture
    )
    {
        return value is not null;
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
    object IValueConverter.ConvertBack(
        object? value,
        Type targetType,
        object? parameter,
        System.Globalization.CultureInfo culture
    )
    {
        throw new NotImplementedException();
    }
}
