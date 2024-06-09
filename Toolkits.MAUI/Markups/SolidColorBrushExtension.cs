using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Toolkits.Maui;

/// <summary>
/// a class of <see cref="SolidColorBrushExtension"/>
/// </summary>
/// <seealso cref="IMarkupExtension" />
public class SolidColorBrushExtension : IMarkupExtension, IMarkupExtension<SolidColorBrush>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SolidColorBrushExtension"/> class.
    /// </summary>
    public SolidColorBrushExtension() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="SolidColorBrushExtension"/> class.
    /// </summary>
    /// <param name="color">The color.</param>
    public SolidColorBrushExtension(Color color)
    {
        Color = color;
    }

    /// <summary>
    /// color.
    /// </summary>
    public Color Color { get; set; } = Colors.Transparent;

    /// <summary>
    /// opacity.
    /// </summary>
    public float Opacity { get; set; } = 1;

    /// <summary>
    /// provide value
    /// </summary>
    /// <param name="serviceProvider"> </param>
    public object ProvideValue(IServiceProvider serviceProvider)
    {
        if (Color is null)
        {
            Color = Colors.Transparent;
        }

        var color = new Color(Color.Red, Color.Green, Color.Blue, Opacity);

        return new SolidColorBrush(color);
    }

    SolidColorBrush IMarkupExtension<SolidColorBrush>.ProvideValue(IServiceProvider serviceProvider)
    {
        return (SolidColorBrush)ProvideValue(serviceProvider);
    }
}
