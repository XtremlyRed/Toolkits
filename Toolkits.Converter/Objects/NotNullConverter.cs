using System.Globalization;

namespace Toolkits.Converter;

/// <summary>
/// a class of <see cref="NotNullConverter"/>
/// </summary>
/// <seealso cref="TrueFalseConverter{Object}" />
public class NotNullConverter : TrueFalseConverter<object>
{
    /// <summary>
    /// object not null convert
    /// </summary>
    /// <param name="value"></param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    protected override object? Convert(object value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is not null ? True : False;
    }

    /// <summary>
    /// input convert
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    protected override object InputConvert(object? value)
    {
        return value!;
    }
}
