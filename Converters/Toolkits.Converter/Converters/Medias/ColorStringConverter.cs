using System.Globalization;
#if ___WPF___
using System.Windows;
using System.Windows.Data;
#endif
#if ___AVALONIA___
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;
#endif

namespace Toolkits;

/// <summary>
///
/// </summary>
public class ColorStringConverter : IValueConverter
{
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
        if (value is not string @string)
        {
            return default!;
        }
#if ___WPF___
        return System.Windows.Media.ColorConverter.ConvertFromString(@string);
#endif
#if ___AVALONIA___
        if (Color.TryParse(@string, out var color))
        {
            return color;
        }
#endif
#if ___MAUI___
        try
        {
            return Color.FromArgb(@string)!;
        }
        catch (Exception)
        {
            if (Color.TryParse(@string, out var ss))
            {
                return ss;
            }

            return default!;
        }

#endif

        return default!;
    }

    object IValueConverter.ConvertBack(
        object? value,
        Type targetType,
        object? parameter,
        CultureInfo culture
    )
    {
        throw new NotImplementedException();
    }
}
