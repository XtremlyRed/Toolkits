using System.Collections.Concurrent;
using System.Diagnostics;
using System.Globalization;
#if ___WPF___
using System.Windows;
using System.Windows.Data;
#endif
#if ___AVALONIA___
using Avalonia;
using Avalonia.Data.Converters;
#pragma warning disable AVP1002  
#endif
namespace Toolkits.Converter;

/// <summary>
/// a class of <see cref="MediaConverter{From,To}"/>
/// </summary> 
public abstract class MediaConverter<From, To> :
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
    where From: notnull
{

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static readonly ConcurrentDictionary<From, To> storages = new();

    /// <summary>
    /// null value
    /// </summary>
    public object? Null
    {
        get => GetValue(NullProperty);
        set => SetValue(NullProperty, value);
    }
    
    /// <summary>
    /// The null property
    /// </summary>
#if ___WPF___
    public static readonly DependencyProperty NullProperty = DependencyProperty.Register(
        "Null",
        typeof(object),
        typeof(MediaConverter<From,To>),
        new PropertyMetadata(null)
    );
#endif
#if ___MAUI___
    public static readonly BindableProperty NullProperty = BindableProperty.Create(
        "Null",
        typeof(object),
        typeof(MediaConverter<From, To>),
        null
    );
#endif
#if ___AVALONIA___
    /// <summary>
    /// The null property
    /// </summary>
    public static readonly AvaloniaProperty NullProperty = AvaloniaProperty.Register<MediaConverter<From,To>, object>("Null", (null!));
#endif

    /// <summary>
    /// media convert
    /// </summary>
    /// <param name="value"></param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    /// <exception cref="InvalidCastException"></exception>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not From fromValue)
        {
            return Null!;
        }

        if (storages.TryGetValue(fromValue, out To? targetValue) == false)
        {
            storages[fromValue] = targetValue = ConvertFrom(fromValue);
        }

        return targetValue!;
    }

    object IValueConverter.ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }

    /// <summary>
    /// convert from 
    /// </summary>
    /// <param name="from"></param>
    /// <returns></returns>
    protected abstract To ConvertFrom(From from);

}
