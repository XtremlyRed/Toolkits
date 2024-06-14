#if ___WPF___
using System.Windows;
#endif
namespace Toolkits.Converter;

/// <summary>
/// a class of <see cref="NotEqualConverter"/>
/// </summary>
/// <seealso cref="CompareConverter" />
public class NotEqualConverter : CompareConverter
{
    /// <summary>
    /// create a new instance of <see cref="NotEqualConverter"/>
    /// </summary>
    public NotEqualConverter()
        : base(CompareMode.NotEqual) { }
}

#if ___WPF___ || ___MAUI___

/// <summary>
/// a class of <see cref="NotEqualToVisitilityConverter"/>
/// </summary>
/// <seealso cref="NotEqualToVisitilityConverter" />
public class NotEqualToVisitilityConverter : CompareConverter
{
    /// <summary>
    /// create a new instance of <see cref="EqualConverter"/>
    /// </summary>
    public NotEqualToVisitilityConverter()
        : base(CompareMode.Equal)
    {
        True = Visibility.Visible;
        False = Visibility.Collapsed;
    }
}

#endif
