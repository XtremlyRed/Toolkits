using System.Windows.Markup;
using Microsoft.Maui.Graphics;

namespace Toolkits.MAUI;

/// <summary>
/// a class of <see cref="ColorExtension"/>
/// </summary>
public class ColorExtension : IMarkupExtension, IMarkupExtension<Color>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ColorExtension"/> class.
    /// </summary>
    public ColorExtension() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ColorExtension"/> class.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="r"></param>
    /// <param name="g"></param>
    /// <param name="b"></param>
    public ColorExtension(byte a, byte r, byte g, byte b)
    {
        this.A = a;
        this.R = r;
        this.G = g;
        this.B = b;
    }

    /// <summary>
    /// r channel
    /// </summary>
    public byte R { get; set; }

    /// <summary>
    /// g channel
    /// </summary>
    public byte G { get; set; }

    /// <summary>
    /// b channel
    /// </summary>
    public byte B { get; set; }

    /// <summary>
    /// a channel
    /// </summary>
    public byte A { get; set; } = 0xff;

    /// <summary>
    /// convert value
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <returns></returns>
    public object ProvideValue(IServiceProvider serviceProvider)
    {
        return new Color(R, G, B, A);
    }

    /// <summary>
    /// convert value
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <returns></returns>
    Color IMarkupExtension<Color>.ProvideValue(IServiceProvider serviceProvider)
    {
        return new Color(R, G, B, A);
    }
}
