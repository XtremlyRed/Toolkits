using System.Globalization;

namespace Toolkits.Converter;

/// <summary>
/// a class of <see cref="NullOrWhiteSpaceConverter"/>
/// </summary>
/// <seealso cref="TrueFalseConverter{String}" />
public class NullOrWhiteSpaceConverter : TrueFalseConverter<string>
{
    /// <summary>
    /// input convert
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    protected override string InputConvert(object? value)
    {
        if (value is null)
        {
            return default!;
        }

        return base.InputConvert(value);
    }

    /// <summary>
    /// string convert
    /// </summary>
    /// <param name="value"></param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    protected override object? Convert(string value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (string.IsNullOrWhiteSpace(value) == false)
        {
            return False;
        }

        return True;
    }
}
