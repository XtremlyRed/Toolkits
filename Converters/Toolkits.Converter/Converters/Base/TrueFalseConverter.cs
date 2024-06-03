﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if ___WPF___
using System.Windows;
using System.Windows.Data;
#endif
#if ___AVALONIA___
using global::Avalonia;
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
/// a class of <see cref="TrueFalseConverter"/>
/// </summary>
/// <seealso cref="IValueConverter" />
public abstract class TrueFalseConverter :
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
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static readonly object TrueObject = true;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static readonly object FalseObject = false;

    /// <summary>
    /// Gets or sets the true.
    /// </summary>
    /// <value>
    /// The true.
    /// </value>
    public object True
    {
        get => GetValue(TrueProperty)!;
        set => SetValue(TrueProperty, value);
    }

    /// <summary>
    /// Gets or sets the false.
    /// </summary>
    /// <value>
    /// The false.
    /// </value>
    public object False
    {
        get => GetValue(FalseProperty)!;
        set => SetValue(FalseProperty, value);
    }

#if ___WPF___

    /// <summary>
    /// The true property
    /// </summary>
    private static readonly DependencyProperty TrueProperty = DependencyProperty.Register(
        "True",
        typeof(object),
        typeof(TrueFalseConverter),
        new PropertyMetadata(true)
    );

    /// <summary>
    /// The false property
    /// </summary>
    private static readonly DependencyProperty FalseProperty = DependencyProperty.Register(
        "False",
        typeof(object),
        typeof(TrueFalseConverter),
        new PropertyMetadata(false)
    );
#endif

#if ___MAUI___

    /// <summary>
    /// The true property
    /// </summary>
    private static readonly BindableProperty TrueProperty = BindableProperty.Create(
        "True",
        typeof(object),
        typeof(TrueFalseConverter),
        (object)true,
        BindingMode.Default,
        null,
        (s, n, o) => { },
        null
    );

    /// <summary>
    /// The false property
    /// </summary>
    private static readonly BindableProperty FalseProperty = BindableProperty.Create(
        "False",
        typeof(object),
        typeof(TrueFalseConverter),
        (object)false,
        BindingMode.Default,
        null,
        (s, n, o) => { },
        null
    );
#endif

#if ___AVALONIA___

    /// <summary>
    /// The true property
    /// </summary>
    private static readonly AvaloniaProperty TrueProperty = AvaloniaProperty.Register<TrueFalseConverter, object>(
        "True",
        true!,
        false,
        BindingMode.OneWay
    );

    /// <summary>
    /// The false property
    /// </summary>
    private static readonly AvaloniaProperty FalseProperty = AvaloniaProperty.Register<TrueFalseConverter, object>(
        "False",
        false!,
        false,
        BindingMode.OneWay
    );
#endif

    /// <summary>
    /// Converts the specified value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="targetType">Type of the target.</param>
    /// <param name="parameter">The parameter.</param>
    /// <param name="culture">The culture.</param>
    /// <returns></returns>
    protected abstract object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture);

    object? IValueConverter.Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return Convert(value, targetType, parameter, culture);
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