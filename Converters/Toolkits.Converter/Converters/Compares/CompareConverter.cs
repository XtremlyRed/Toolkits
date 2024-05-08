using System;
using System.Collections.Generic;
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
///
/// </summary>
public class CompareConverter :
#if ___WPF___
    DependencyObject,
#endif
#if ___AVALONIA___
    AvaloniaObject,
#endif
#if ___MAUI___
    BindableObject,
#endif

    IValueConverter
{
    /// <summary>
    /// Gets or sets the matched value .
    /// </summary>
    /// <value>
    /// The matched value.
    /// </value>
    public object Matched
    {
        get => GetValue(MatchedProperty);
        set => SetValue(MatchedProperty, value);
    }

    /// <summary>
    /// Gets or sets the unmatched value.
    /// </summary>
    /// <value>
    /// The unmatched value.
    /// </value>
    public object Unmatched
    {
        get => GetValue(UnmatchedProperty);
        set => SetValue(UnmatchedProperty, value);
    }

    /// <summary>
    /// Gets or sets the compare value.
    /// </summary>
    /// <value>
    /// The compare value.
    /// </value>
    public IComparable? Compare
    {
        get => GetValue(CompareProperty) as IComparable;
        set => SetValue(CompareProperty, value);
    }

    /// <summary>
    /// Gets or sets the compare mode.
    /// </summary>
    /// <value>
    /// The compare mode.
    /// </value>
    public CompareMode Mode
    {
        get => (CompareMode)GetValue(ModeProperty);
        set => SetValue(ModeProperty, value);
    }

#if ___WPF___
    /// <summary>
    /// The matched property
    /// </summary>
    public static readonly DependencyProperty MatchedProperty = DependencyProperty.Register(
        "Matched",
        typeof(object),
        typeof(CompareConverter),
        new PropertyMetadata(true)
    );

    /// <summary>
    /// The unmatched property
    /// </summary>
    public static readonly DependencyProperty UnmatchedProperty = DependencyProperty.Register(
        "Unmatched",
        typeof(object),
        typeof(CompareConverter),
        new PropertyMetadata(false)
    );

    /// <summary>
    /// The compare value property
    /// </summary>
    public static readonly DependencyProperty CompareProperty = DependencyProperty.Register(
        "Compare",
        typeof(IComparable),
        typeof(CompareConverter),
        new PropertyMetadata(default(object))
    );

    /// <summary>
    /// The compare mode property
    /// </summary>
    public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(
        "Mode",
        typeof(CompareMode),
        typeof(CompareConverter),
        new PropertyMetadata(CompareMode.Equal)
    );
#endif

#if ___MAUI___
    /// <summary>
    /// The matched property
    /// </summary>
    public static readonly BindableProperty MatchedProperty = BindableProperty.Create(
        "Matched",
        typeof(object),
        typeof(CompareConverter),
        true,
        BindingMode.Default,
        null,
        (s, n, o) => { },
        null
    );

    /// <summary>
    /// The unmatched property
    /// </summary>
    public static readonly BindableProperty UnmatchedProperty = BindableProperty.Create(
        "Unmatched",
        typeof(object),
        typeof(CompareConverter),
        false,
        BindingMode.Default,
        null,
        (s, n, o) => { },
        null
    );

    /// <summary>
    /// The compare value property
    /// </summary>
    public static readonly BindableProperty CompareProperty = BindableProperty.Create(
        "Compare",
        typeof(IComparable),
        typeof(CompareConverter),
        null,
        BindingMode.Default,
        null,
        (s, n, o) => { },
        null
    );

    /// <summary>
    /// The compare mode property
    /// </summary>
    public static readonly BindableProperty ModeProperty = BindableProperty.Create(
        "Mode",
        typeof(CompareMode),
        typeof(CompareConverter),
        CompareMode.Equal,
        BindingMode.Default,
        null,
        (s, n, o) => { },
        null
    );
#endif

#if ___AVALONIA___
    /// <summary>
    /// The matched property
    /// </summary>
    public static readonly AvaloniaProperty MatchedProperty = AvaloniaProperty.Register<CompareConverter, object>(
        "Matched",
        (object)true,
        false,
        BindingMode.OneWay
    );

    /// <summary>
    /// The unmatched property
    /// </summary>
    public static readonly AvaloniaProperty UnmatchedProperty = AvaloniaProperty.Register<CompareConverter, object>(
        "Unmatched",
        (object)false,
        false,
        BindingMode.OneWay
    );

    /// <summary>
    /// The compare value property
    /// </summary>
    public static readonly AvaloniaProperty CompareProperty = AvaloniaProperty.Register<CompareConverter, IComparable>(
        "Compare",
        null!,
        false,
        BindingMode.OneWay
    );

    /// <summary>
    /// The compare mode property
    /// </summary>
    public static readonly AvaloniaProperty ModeProperty = AvaloniaProperty.Register<CompareConverter, CompareMode>(
        "Mode",
        CompareMode.Equal!,
        false,
        BindingMode.OneWay
    );
#endif

    /// <summary>
    /// </summary>
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is IComparable cValue)
        {
            bool condiction = Match(cValue, Compare, Mode);

            return condiction ? Matched : Unmatched;
        }

#if _WPF_

        return Binding.DoNothing;

#endif

        return default!;
    }

    /// <summary>
    /// </summary>
    object? IValueConverter.ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Matches the specified value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="comparer">The comparer.</param>
    /// <param name="compareMode">The compare mode.</param>
    /// <returns></returns>
    public static bool Match(IComparable? value, IComparable? comparer, CompareMode? compareMode)
    {
        switch (compareMode)
        {
            case CompareMode.Equal:
                return value!.CompareTo(comparer) == 0;
            case CompareMode.NotEqual:
                return value!.CompareTo(comparer) != 0;
            case CompareMode.GreaterThan:
                return value!.CompareTo(comparer) > 0;
            case CompareMode.GreaterThanOrEqual:
                return value!.CompareTo(comparer) >= 0;
            case CompareMode.LessThan:
                return value!.CompareTo(comparer) < 0;
            case CompareMode.LessThanOrEqual:
                return value!.CompareTo(comparer) <= 0;
            default:
                return true;
        }
    }
}

/// <summary>
/// compare mode
/// </summary>
public enum CompareMode
{
    /// <summary>
    /// equal
    /// </summary>
    Equal,

    /// <summary>
    /// not equal
    /// </summary>
    NotEqual,

    /// <summary>
    /// greater than
    /// </summary>
    GreaterThan,

    /// <summary>
    /// greater than or equal
    /// </summary>
    GreaterThanOrEqual,

    /// <summary>
    /// less than
    /// </summary>
    LessThan,

    /// <summary>
    /// less than or equal
    /// </summary>
    LessThanOrEqual,
}
