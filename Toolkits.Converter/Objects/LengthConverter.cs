using System.Collections;
using System.Globalization;

namespace Toolkits.Converter;

/// <summary>
/// a class of <see cref="LengthConverter"/>
/// </summary>
/// <seealso cref="ValueConverterBase{IEnumerable}" />
public class LengthConverter : ValueConverterBase<IEnumerable>
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="items"></param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>

    protected override object? Convert(IEnumerable items, Type targetType, object? parameter, CultureInfo culture)
    {
        if (items is ICollection list)
        {
            return list.Count;
        }

        var count = 0;

        foreach (var item in items!)
        {
            count++;
        }

        return count;
    }
}
