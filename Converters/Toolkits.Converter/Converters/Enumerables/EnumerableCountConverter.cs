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

namespace Toolkits;

public class EnumerableCountConverter : IValueConverter
{
    static string errorMessage = $"current value type is not " + typeof(IEnumerable).FullName;

    public object Convert(
        object? value,
        Type targetType,
        object? parameter,
        System.Globalization.CultureInfo culture
    )
    {
        if (value is not IEnumerable enumerable)
        {
            throw new ArgumentException(errorMessage);
        }

        if (enumerable is Array array)
        {
            return array.Length;
        }

        if (enumerable is ICollection collection)
        {
            return collection.Count;
        }

        var count = 0;
        foreach (var _ in enumerable)
        {
            count++;
        }
        return count;
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
