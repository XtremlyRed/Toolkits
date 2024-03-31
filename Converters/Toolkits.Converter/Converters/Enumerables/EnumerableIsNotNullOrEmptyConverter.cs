using System.Collections;
#if ___WPF___
using System.Windows;
using System.Windows.Data;
#endif
#if ___AVALONIA___
using Avalonia;
using Avalonia.Data.Converters;
#endif

namespace Toolkits;

public class EnumerableIsNotNullOrEmptyConverter : IValueConverter
{
    public object Convert(
        object? value,
        Type targetType,
        object? parameter,
        System.Globalization.CultureInfo culture
    )
    {
        if (value is not IEnumerable enumerable)
        {
            throw new ArgumentException(
                $"current value type is not {typeof(IEnumerable).FullName}"
            );
        }

        foreach (var _ in enumerable)
        {
            return true;
        }

        return false;
    }

    public object ConvertBack(
        object? value,
        Type targetType,
        object? parameter,
        System.Globalization.CultureInfo culture
    )
    {
        throw new NotImplementedException();
    }
}
