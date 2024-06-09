using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
#if ___WPF___
using System.Windows.Data;
using System.Windows.Markup;
#endif

#if ___AVALONIA___
using Avalonia;
using Avalonia.Data.Converters;
#endif

namespace Toolkits.Converter;

/// <summary>
///  a class of <see cref="CompositeConverter"/>
/// </summary>

#if ___WPF___
[ContentProperty(nameof(Converters))]
#endif
[DefaultProperty(nameof(Converters))]
public class CompositeConverter : ValueConverterBase<object>
{
    /// <summary>
    /// Gets or sets the converters.
    /// </summary>
    /// <value>
    /// The converters.
    /// </value>
    public ConverterCollection Converters
    {
        get => (ConverterCollection)GetValue(ConvertersProperty)!;
        set => SetValue(ConvertersProperty, value);
    }

    /// <summary>
    /// The converters property
    /// </summary>
#if ___WPF___
    public static readonly DependencyProperty ConvertersProperty = DependencyProperty.Register(
        "Converters",
        typeof(ConverterCollection),
        typeof(CompositeConverter),
        new PropertyMetadata(new ConverterCollection())
    );

#endif
#if ___MAUI___
    public static readonly BindableProperty ConvertersProperty = BindableProperty.Create(
        "Converters",
        typeof(ConverterCollection),
        typeof(CompositeConverter),
        null,
        BindingMode.OneWay,
        null,
        null
    );

#endif
#if ___AVALONIA___
    public static readonly AvaloniaProperty ConvertersProperty = AvaloniaProperty.Register<CompositeConverter, ConverterCollection>(
        "Converters",
        new ConverterCollection()!
    );
#endif

    /// <summary>
    /// Initializes a new instance of the <see cref="CompositeConverter"/> class.
    /// </summary>
    public CompositeConverter()
    {
        Converters = new ConverterCollection();
    }

    /// <summary>
    /// Converts the specified value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="targetType">Type of the target.</param>
    /// <param name="parameter">The parameter.</param>
    /// <param name="culture">The culture.</param>
    /// <returns></returns>
    protected override object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        object? concurrent = value;

        foreach (IValueConverter item in Converters)
        {
            concurrent = item.Convert(concurrent, targetType, parameter, culture);
        }

        return concurrent!;
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

/// <summary>
/// a class of <see cref="ConverterCollection"/>
/// </summary>
public class ConverterCollection : Collection<IValueConverter> { }
