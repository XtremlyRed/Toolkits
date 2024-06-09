using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toolkits.Converter;

/// <summary>
/// a class of <see cref="Converters"/>
/// </summary>
public static class Converters
{
    /// <summary>
    ///  <see cref="Boolean"/> reverse converter
    /// </summary>
    public static BooleanReverseConverter BooleanReverse = new() { True = true, False = false };

#if ___WPF___
    /// <summary>
    /// <see cref="Boolean"/> to <see cref="System.Windows.Visibility"/> converter
    /// </summary>
    public static BooleanToVisibilityConverter BooleanToVisibility = new();

    /// <summary>
    /// <see cref="Boolean"/> to <see cref="System.Windows.Visibility"/> reverse converter
    /// </summary>
    public static BooleanToVisibilityReverseConverter BooleanToVisibilityReverse = new();
#endif

    /// <summary>
    /// <see cref="string"/> to Color converter
    /// </summary>

    public static ColorStringConverter StringToColor = new();

    /// <summary>
    /// <see cref="string"/> to Brush converter
    /// </summary>

    public static BrushStringConverter StringToBrush = new();

    /// <summary>
    /// get <see cref="IEnumerable"/> count
    /// </summary>
    public static LengthConverter Length = new();

    /// <summary>
    /// <see cref="IEnumerable"/> <see langword="null"/> or empty to <see cref="bool"/> converter
    /// </summary>
    public static NullOrEmptyConverter NullOrEmpty = new() { True = true, False = false };

    /// <summary>
    /// <see cref="IEnumerable"/> not <see langword="null"/> or empty to <see cref="bool"/> converter
    /// </summary>
    public static NotNullOrEmptyConverter NotNullOrEmpty = new() { True = true, False = false };

    /// <summary>
    /// <see cref="string"/> <see langword="null"/> or white space to <see cref="bool"/> converter
    /// </summary>
    public static NullOrWhiteSpaceConverter NullOrWhiteSpace = new() { True = true, False = false };

    /// <summary>
    /// <see cref="string"/> not <see langword="null"/> or white space to <see cref="bool"/> converter
    /// </summary>
    public static NotNullOrWhiteSpaceConverter NotNullOrWhiteSpace = new() { True = true, False = false };

    /// <summary>
    /// <see cref="object"/> <see langword="null"/> to <see cref="bool"/> converter
    /// </summary>
    public static NullConverter Null = new NullConverter() { True = true, False = false };

    /// <summary>
    /// <see cref="object"/> not <see langword="null"/> to <see cref="bool"/> converter
    /// </summary>
    public static NotNullConverter NotNull = new NotNullConverter() { True = true, False = false };
}
