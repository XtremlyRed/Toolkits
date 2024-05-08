using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
#if ___WPF___
using System.Windows;
using System.Windows.Data;
#endif
#if ___AVALONIA___
using Avalonia;
using global::Avalonia.Data;
using global::Avalonia.Data.Converters;
#endif

#if ___WPF___
namespace Toolkits.Wpf;

#endif
#if ___AVALONIA___
namespace Toolkits.Avalonia;

#endif
#if ___MAUI___
namespace Toolkits.Maui;

#endif

/// <summary>
///
/// </summary>
public abstract class NumberRangeConverterBase :
#if ___WPF___
    DependencyObject,
#endif
#if ___MAUI___
    BindableObject,
#endif
#if ___AVALONIA___
    AvaloniaObject,
#endif
        IValueConverter
{
    /// <summary>
    /// Converts the specified value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="targetType">Type of the target.</param>
    /// <param name="parameter">The parameter.</param>
    /// <param name="culture">The culture.</param>
    /// <returns></returns>
    public abstract object Convert(object? value, Type targetType, object? parameter, CultureInfo culture);

    /// <summary>
    /// Converts the back.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="targetType">Type of the target.</param>
    /// <param name="parameter">The parameter.</param>
    /// <param name="culture">The culture.</param>
    /// <returns></returns>
    public abstract object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture);

    /// <summary>
    /// maximum value.
    /// </summary>
    /// <value>
    /// The maximum value.
    /// </value>
    public double MaxValue
    {
        get => (double)GetValue(MaxValueProperty)!;
        set => SetValue(MaxValueProperty, value);
    }

    /// <summary>
    /// minimum value.
    /// </summary>
    /// <value>
    /// The minimum value.
    /// </value>
    public double MinValue
    {
        get => (double)GetValue(MinValueProperty)!;
        set => SetValue(MinValueProperty, value);
    }

#if ___WPF___

    /// <summary>
    /// The max value property
    /// </summary>
    private static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register(
        "MaxValue",
        typeof(double),
        typeof(NumberInRangeConverter),
        new PropertyMetadata(double.MaxValue)
    );

    /// <summary>
    /// The min value property
    /// </summary>
    private static readonly DependencyProperty MinValueProperty = DependencyProperty.Register(
        "MinValue",
        typeof(double),
        typeof(NumberInRangeConverter),
        new PropertyMetadata(double.MinValue)
    );

#endif

#if ___MAUI___

    /// <summary>
    /// The max value property
    /// </summary>
    private static readonly BindableProperty MaxValueProperty = BindableProperty.Create(
        "MaxValue",
        typeof(object),
        typeof(NumberInRangeConverter),
        (object)double.MaxValue,
        BindingMode.Default,
        null,
        (s, n, o) => { },
        null
    );

    /// <summary>
    /// The min value property
    /// </summary>
    private static readonly BindableProperty MinValueProperty = BindableProperty.Create(
        "MinValue",
        typeof(object),
        typeof(NumberInRangeConverter),
        (object)double.MinValue,
        BindingMode.Default,
        null,
        (s, n, o) => { },
        null
    );
#endif

#if ___AVALONIA___

    /// <summary>
    /// The max value property
    /// </summary>
    private static readonly AvaloniaProperty MaxValueProperty = AvaloniaProperty.Register<NumberInRangeConverter, double>(
        "MaxValue",
        double.MaxValue,
        false,
        BindingMode.OneWay
    );

    /// <summary>
    /// The min value property
    /// </summary>
    private static readonly AvaloniaProperty MinValueProperty = AvaloniaProperty.Register<NumberInRangeConverter, double>(
        "MinValue",
        double.MinValue!,
        false,
        BindingMode.OneWay
    );
#endif
}
