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
/// a class of <see cref="BrushStringConverter"/>
/// </summary>
public class BrushStringConverter : MediaConverter<string, Brush>
{
#if ___WPF___ || ___AVALONIA___
    static BrushConverter brushConverter = new BrushConverter();
#endif

    /// <summary>
    /// convert from
    /// </summary>
    /// <param name="from"></param>
    /// <returns></returns>
    protected override Brush ConvertFrom(string from)
    {
#if ___WPF___ || ___AVALONIA___
        return (Brush)brushConverter.ConvertFrom(from)!;
#endif

#if ___MAUI___
        return new SolidColorBrush(Color.FromArgb(from));
#endif
    }
}
