using System.Windows.Markup;
using System.Windows.Media;

namespace Toolkits.Wpf;

/// <summary>
/// a class of <see cref="ColorExtension"/>
/// </summary>
/// <seealso cref="System.Windows.Markup.MarkupExtension" />
[MarkupExtensionReturnType(typeof(SolidColorBrush))]
public class ColorExtension : MarkupExtension
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
    ///
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <returns></returns>
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return new Color()
        {
            A = this.A,
            R = R,
            G = G,
            B = B
        };
    }
}
