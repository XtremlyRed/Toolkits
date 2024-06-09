using System.Collections;
using System.Globalization;

namespace Toolkits.Converter;

/// <summary>
/// a class of <see cref="NotNullOrEmptyConverter"/>
/// </summary>
/// <seealso cref="TrueFalseConverter{IEnumerable}" />
public class NotNullOrEmptyConverter : TrueFalseConverter<IEnumerable>
{
    /// <summary>
    /// input convert
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    protected override IEnumerable InputConvert(object? value)
    {
        if (value is null)
        {
            return default!;
        }

        return base.InputConvert(value);
    }

    /// <summary>
    /// convert to
    /// </summary>
    /// <param name="value"></param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    protected override object? Convert(IEnumerable value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null)
        {
            return False;
        }

        if (value is ICollection collection)
        {
            return collection.Count > 0 ? True : False;
        }

        foreach (var _ in value)
        {
            return True;
        }

        return False;
    }
}
