namespace Toolkits.Converter;

#if ___WPF___
using System.Windows;
using System.Windows.Media;
using System.Windows.Data;
#endif
#if ___AVALONIA___
using Avalonia;
using global::Avalonia.Media;
#endif

#if ___MAUI___
using Microsoft.Maui.Media;
#endif
/// <summary>
/// a class of <see cref="ColorStringConverter"/>
/// </summary>
public class ColorStringConverter : MediaConverter<string, Color>
{
#if ___WPF___ || ___AVALONIA___
    static BrushConverter brushConverter = new BrushConverter();
#endif

    /// <summary>
    /// convert from
    /// </summary>
    /// <param name="from"></param>
    /// <returns></returns>
    protected override Color ConvertFrom(string from)
    {
#if ___WPF___
        return (Color)ColorConverter.ConvertFromString(from);
#endif
#if ___AVALONIA___
        return Color.Parse(from);
#endif

#if ___MAUI___
        return Color.FromArgb(from);
#endif
    }
}
