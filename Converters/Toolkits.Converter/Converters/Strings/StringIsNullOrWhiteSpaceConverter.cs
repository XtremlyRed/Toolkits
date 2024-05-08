using System.Globalization;
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

#if ___WPF___
namespace Toolkits.Wpf;

#endif
#if ___AVALONIA___
namespace Toolkits.Avalonia;

#endif
#if ___MAUI___
namespace Toolkits.Maui;

#endif

/// <summary>
///
/// </summary>
public class StringIsNullOrWhiteSpaceConverter : IValueConverter
{
    /// <summary>
    /// Converts the specified value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="targetType">Type of the target.</param>
    /// <param name="parameter">The parameter.</param>
    /// <param name="culture">The culture.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">current value type is not {typeof(string)}</exception>
    public object Convert(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
    {
        return value is not string stringValue
            ? throw new ArgumentException($"current value type is not {typeof(string)}")
            : string.IsNullOrWhiteSpace(stringValue);
    }

    object IValueConverter.ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
