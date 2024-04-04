using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if ___WPF___
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
#endif
#if ___AVALONIA___
using global::Avalonia;
using global::Avalonia.Data;
using global::Avalonia.Data.Converters;
#endif

namespace Toolkits;

#if ___WPF___
[ContentProperty(nameof(Converters))]
#endif

[DefaultProperty(nameof(Converters))]
public class CompositeConverter :
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
    /// Gets or sets the converters.
    /// </summary>
    /// <value>
    /// The converters.
    /// </value>
    public ConverterCollection Converters
    {
        get { return (ConverterCollection)GetValue(ConvertersProperty)!; }
        set { SetValue(ConvertersProperty, value); }
    }
#if ___WPF___
    public static readonly DependencyProperty ConvertersProperty = DependencyProperty.Register(
        "Converters",
        typeof(ConverterCollection),
        typeof(CompositeConverter),
        new PropertyMetadata(new ConverterCollection())
    );
#endif
#if ___MAUI___
    /// <summary>
    /// The converters property
    /// </summary>
    public static readonly BindableProperty ConvertersProperty = BindableProperty.Create(
        "Converters",
        typeof(ConverterCollection),
        typeof(CompositeConverter),
        new ConverterCollection(),
        BindingMode.OneWay,
        (s, e) => true,
        (s, n, o) => { },
        null
    );
#endif
#if ___AVALONIA___
    /// <summary>
    /// converter collection property
    /// </summary>
    public static readonly AvaloniaProperty ConvertersProperty = AvaloniaProperty.Register<
        CompositeConverter,
        ConverterCollection
    >("Converters", new ConverterCollection()!, false, BindingMode.OneWay);
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
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var concurrent = value;

        foreach (var item in Converters)
        {
            concurrent = item.Convert(concurrent, targetType, parameter, culture);
        }

        return concurrent!;
    }

    /// <summary>
    /// Converts the back.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="targetType">Type of the target.</param>
    /// <param name="parameter">The parameter.</param>
    /// <param name="culture">The culture.</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public object ConvertBack(
        object? value,
        Type targetType,
        object? parameter,
        CultureInfo culture
    )
    {
        throw new NotImplementedException();
    }
}

/// <summary>
///
/// </summary>
/// <seealso cref="System.Collections.ObjectModel.Collection&lt;System.Windows.Data.IValueConverter&gt;" />
public class ConverterCollection : Collection<IValueConverter> { }
