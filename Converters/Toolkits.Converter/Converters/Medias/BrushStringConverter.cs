using System;
using System.Collections.Generic;
using System.Globalization;
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
using Avalonia.Media;
#endif
#if ___MAUI___
using Microsoft.Maui.Graphics.Converters;
using Microsoft.Maui.Media;
#endif
namespace Toolkits.Converter;

/// <summary>
///
/// </summary>
public class BrushStringConverter : IValueConverter
{
#if ___MAUI___
    static ColorStringConverter colorString = new ColorStringConverter();
#endif

    /// <summary>
    /// Converts the specified value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="targetType">Type of the target.</param>
    /// <param name="parameter">The parameter.</param>
    /// <param name="culture">The culture.</param>
    /// <returns></returns>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null)
        {
            return default!;
        }

#if ___WPF___
        System.Windows.Media.BrushConverter brushConverter = new System.Windows.Media.BrushConverter();
        if (brushConverter.CanConvertFrom(value.GetType()))
        {
            return brushConverter.ConvertFrom(value)!;
        }
#endif
#if ___AVALONIA___
        BrushConverter brushConverter = new BrushConverter();
        return brushConverter.CanConvertFrom(value.GetType()) ? brushConverter.ConvertFrom(value)! : default!;
#endif
#if ___MAUI___
        var colorString = new ColorStringConverter();
        return new SolidColorBrush() { Color = (Color)colorString.Convert(value, targetType, parameter!, culture)! };
#endif
        return default!;
    }

    object IValueConverter.ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
