using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
#if ___WPF___
using System.Windows.Data;
using System.Windows;
#endif
#if ___AVALONIA___
using Avalonia;
using Avalonia.Data.Converters;
#pragma warning disable AVP1002
#endif
namespace Toolkits.Converter;

/// <summary>
/// a class of <see cref="RangeConverter{T}"/>
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class RangeConverter<T> : TrueFalseConverter<T>
    where T : IComparable
{
    /// <summary>
    /// min value
    /// </summary>
    public T MinValue
    {
        get { return (T)GetValue(MinValueProperty)!; }
        set { SetValue(MinValueProperty, value); }
    }

    /// <summary>
    /// min value
    /// </summary>
#if ___WPF___
    public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register(
        "MinValue",
        typeof(T),
        typeof(RangeConverter<T>),
        new PropertyMetadata(default(T)!)
    );
#endif
#if ___AVALONIA___
    public static readonly AvaloniaProperty MinValueProperty = AvaloniaProperty.Register<RangeConverter<T>, T>("MinValue", (default(T))!);
#endif
#if ___MAUI___
    public static readonly BindableProperty MinValueProperty = BindableProperty.Create(
        "MinValue",
        typeof(T),
        typeof(RangeConverter<T>),
        (default(T)!)
    );
#endif

    /// <summary>
    /// max value
    /// </summary>
    public T MaxValue
    {
        get { return (T)GetValue(MaxValueProperty)!; }
        set { SetValue(MaxValueProperty, value); }
    }

    /// <summary>
    /// max value
    /// </summary>
#if ___WPF___
    public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register(
        "MaxValue",
        typeof(T),
        typeof(RangeConverter<T>),
        new PropertyMetadata(default(T)!)
    );
#endif
#if ___AVALONIA___
    public static readonly AvaloniaProperty MaxValueProperty = AvaloniaProperty.Register<RangeConverter<T>, T>("MaxValue", (default(T))!);
#endif
#if ___MAUI___
    public static readonly BindableProperty MaxValueProperty = BindableProperty.Create(
        "MaxValue",
        typeof(T),
        typeof(RangeConverter<T>),
        (default(T))!
    );
#endif

    /// <summary>
    /// value convert
    /// </summary>
    /// <param name="value"></param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    protected override object? Convert(T value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value.CompareTo(MinValue) >= 0 && MaxValue.CompareTo(value) >= 0)
        {
            return True;
        }

        return False;
    }
}
