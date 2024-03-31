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
#endif
#if ___MAUI___
using Microsoft.Maui.Graphics.Converters;
using Microsoft.Maui.Media;
#endif

namespace Toolkits;

public class StringIsNullOrEmptyConverter : IValueConverter
{
    public object Convert(
        object? value,
        Type targetType,
        object? parameter,
        System.Globalization.CultureInfo culture
    )
    {
        return value is not string stringValue
            ? throw new ArgumentException($"current value type is not {typeof(string)}")
            : string.IsNullOrEmpty(stringValue);
    }

    public object ConvertBack(
        object? value,
        Type targetType,
        object? parameter,
        CultureInfo culture
    )
    {
        throw new NotImplementedException();
    }
}
