using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Windows.Media;

namespace Toolkits.Wpf;

/// <summary>
/// a class of <see cref="SolidColorBrushExtension"/>
/// </summary>
/// <seealso cref="System.Windows.Markup.MarkupExtension" />
[MarkupExtensionReturnType(typeof(SolidColorBrush))]
public class SolidColorBrushExtension : MarkupExtension
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
    public Color Color { get; set; }

    /// <summary>
    /// opacity.
    /// </summary>
    public double Opacity { get; set; } = 1;

    /// <summary>
    /// provide value
    /// </summary>
    /// <param name="serviceProvider"> </param>
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return new SolidColorBrush(Color) { Opacity = Opacity };
    }
}
