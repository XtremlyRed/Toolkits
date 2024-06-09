using System.ComponentModel;
using System.Globalization;

#if ___WPF___
using System.Windows;
using System.Windows.Data;
#endif
#if ___AVALONIA___
using Avalonia;
using Avalonia.Data.Converters;
#endif
namespace Toolkits.Converter;

/// <summary>
/// a class of <see cref="ValueConverterBase{T}"/>
/// </summary>
/// <seealso cref="IValueConverter" />

[EditorBrowsable(EditorBrowsableState.Never)]
public abstract class ValueConverterBase<T> :
#if ___WPF___
    DependencyObject
#endif
#if ___MAUI___
    BindableObject
#endif
#if ___AVALONIA___
    AvaloniaObject
#endif
     
    , IValueConverter
{
    /// <summary>
    /// Converts the specified value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="targetType">Type of the target.</param>
    /// <param name="parameter">The parameter.</param>
    /// <param name="culture">The culture.</param>
    /// <returns></returns>
    protected abstract object? Convert(T value, Type targetType, object? parameter, CultureInfo culture);

    object? IValueConverter.Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var targetValue = InputConvert(value);

        return this.Convert(targetValue, targetType, parameter, culture);
    }

    /// <summary>
    ///  type verify
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    protected virtual T InputConvert(object? value)
    {
        if (value is not T targetValue)
        {
            throw new ArgumentException($"current value type is not {typeof(T).FullName}");
        }

        return targetValue;
    }

    /// <summary>
    /// Converts the back.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="targetType">Type of the target.</param>
    /// <param name="parameter">The parameter.</param>
    /// <param name="culture">The culture.</param>
    /// <returns></returns>
    protected virtual object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }

    object? IValueConverter.ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return ConvertBack(value, targetType, parameter, culture);
    }
}
