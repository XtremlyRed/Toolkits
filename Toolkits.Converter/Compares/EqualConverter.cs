#if ___WPF___
using System.Windows;
#endif
namespace Toolkits.Converter;

/// <summary>
/// a class of <see cref="EqualConverter"/>
/// </summary>
/// <seealso cref="CompareConverter" />
public class EqualConverter : CompareConverter
{
    /// <summary>
    /// create a new instance of <see cref="EqualConverter"/>
    /// </summary>
    public EqualConverter()
        : base(CompareMode.Equal) { }
}

#if ___WPF___ || ___MAUI___

/// <summary>
/// a class of <see cref="EqualToVisitilityConverter"/>
/// </summary>
/// <seealso cref="EqualToVisitilityConverter" />
public class EqualToVisitilityConverter : CompareConverter
{
    /// <summary>
    /// create a new instance of <see cref="EqualConverter"/>
    /// </summary>
    public EqualToVisitilityConverter()
        : base(CompareMode.Equal)
    {
        True = Visibility.Visible;
        False = Visibility.Collapsed;
    }
}

#endif
