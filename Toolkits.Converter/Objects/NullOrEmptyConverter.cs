using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Toolkits.Converter;

/// <summary>
/// a class of <see cref="NullOrEmptyConverter"/>
/// </summary>
/// <seealso cref="TrueFalseConverter{IEnumerable}" />
public class NullOrEmptyConverter : TrueFalseConverter<IEnumerable>
{
    /// <summary>
    ///  enumerable convert
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
            return True;
        }

        if (value is ICollection collection)
        {
            return collection.Count > 0 ? False : True;
        }

        foreach (var _ in value)
        {
            return False;
        }

        return True;
    }

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
}
