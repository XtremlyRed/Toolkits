using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
#if ___WPF___
using System.Windows.Data;
#endif

#if ___AVALONIA___
using Avalonia;
using Avalonia.Data.Converters;
#pragma warning disable AVP1002
#endif
namespace Toolkits.Converter;

/// <summary>
/// a class of <see cref="TrueFalseOrNullConverter{T}"/>
/// </summary>
/// <seealso cref="IValueConverter" />

[EditorBrowsable(EditorBrowsableState.Never)]
public abstract class TrueFalseOrNullConverter<T> : TrueFalseConverter<T>, IValueConverter
{
    /// <summary>
    ///  null value
    /// </summary>
    public object? Null
    {
        get { return GetValue(NullProperty); }
        set { SetValue(NullProperty, value); }
    }

#if ___WPF___
    /// <summary>
    /// The null property
    /// </summary>
    public static readonly DependencyProperty NullProperty = DependencyProperty.Register(
        "Null",
        typeof(object),
        typeof(TrueFalseOrNullConverter<T>),
        new PropertyMetadata(null)
    );
#endif
#if ___MAUI___
    /// <summary>
    /// The null property
    /// </summary>
    public static readonly BindableProperty NullProperty = BindableProperty.Create(
        "Null",
        typeof(object),
        typeof(TrueFalseOrNullConverter<T>),
        (null)
    );
#endif
#if ___AVALONIA___
    /// <summary>
    /// The null property
    /// </summary>
    public static readonly AvaloniaProperty NullProperty = AvaloniaProperty.Register<TrueFalseOrNullConverter<T>, object>("Null", (null!));
#endif
}
