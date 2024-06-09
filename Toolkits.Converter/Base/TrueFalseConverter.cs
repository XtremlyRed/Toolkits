using System.ComponentModel;
using System.Globalization;
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
/// a class of <see cref="TrueFalseConverter{T}"/>
/// </summary>
/// <seealso cref="IValueConverter" />

[EditorBrowsable(EditorBrowsableState.Never)]
public abstract class TrueFalseConverter<T> : ValueConverterBase<T>, IValueConverter
{
    /// <summary>
    /// true value
    /// </summary>
    public object True
    {
        get => GetValue(TrueProperty)!;
        set => SetValue(TrueProperty, value);
    }

    /// <summary>
    ///  false value
    /// </summary>
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
        typeof(TrueFalseConverter<T>),
        new PropertyMetadata(null)
    );

    /// <summary>
    /// The false property
    /// </summary>
    private static readonly DependencyProperty FalseProperty = DependencyProperty.Register(
        "False",
        typeof(object),
        typeof(TrueFalseConverter<T>),
        new PropertyMetadata(null)
    );
#endif
#if ___MAUI___
    /// <summary>
    /// The true property
    /// </summary>
    private static readonly BindableProperty TrueProperty = BindableProperty.Create("True", typeof(object), typeof(TrueFalseConverter<T>), (null));

    /// <summary>
    /// The false property
    /// </summary>
    private static readonly BindableProperty FalseProperty = BindableProperty.Create("False", typeof(object), typeof(TrueFalseConverter<T>), (null));
#endif
#if ___AVALONIA___
    /// <summary>
    /// The true property
    /// </summary>
    private static readonly AvaloniaProperty TrueProperty = AvaloniaProperty.Register<TrueFalseConverter<T>, object>("True", (null!));

    /// <summary>
    /// The false property
    /// </summary>
    private static readonly AvaloniaProperty FalseProperty = AvaloniaProperty.Register<TrueFalseConverter<T>, object>("False", (null!));
#endif
}
