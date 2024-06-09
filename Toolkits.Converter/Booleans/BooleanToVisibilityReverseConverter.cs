using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
#if ___WPF___


using System.Windows.Data;

namespace Toolkits.Converter;

/// <summary>
/// a class of <see cref="BooleanToVisibilityReverseConverter"/>
/// </summary>
/// <seealso cref="IValueConverter" />
public class BooleanToVisibilityReverseConverter : ValueConverterBase<bool>
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="value"></param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>

    protected override object? Convert(bool value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value ? Visibility.Collapsed : Visibility.Visible;
    }
}

#endif
