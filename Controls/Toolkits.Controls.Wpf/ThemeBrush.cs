using System.Windows;
using System.Windows.Markup;

namespace Toolkits.Wpf;

/// <summary>
/// a class of <see cref="ThemeBrush"/>
/// </summary>
public class ThemeBrush : DependencyObject
{
    /// <summary>
    ///  border brush.
    /// </summary>
    public Brush BorderBrush
    {
        get { return (Brush)GetValue(BorderBrushProperty); }
        set { SetValue(BorderBrushProperty, value); }
    }

    /// <summary>
    /// border brush property
    /// </summary>
    public static readonly DependencyProperty BorderBrushProperty = DependencyProperty.Register(
        "BorderBrush",
        typeof(Brush),
        typeof(ThemeBrush),
        new PropertyMetadata(null)
    );

    /// <summary>
    /// background.
    /// </summary>
    public Brush Background
    {
        get { return (Brush)GetValue(BackgroundProperty); }
        set { SetValue(BackgroundProperty, value); }
    }

    /// <summary>
    ///  background property
    /// </summary>
    public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register(
        "Background",
        typeof(Brush),
        typeof(ThemeBrush),
        new PropertyMetadata(null)
    );

    /// <summary>
    /// foreground.
    /// </summary>
    public Brush Foreground
    {
        get { return (Brush)GetValue(ForegroundProperty); }
        set { SetValue(ForegroundProperty, value); }
    }

    /// <summary>
    /// foreground property
    /// </summary>
    public static readonly DependencyProperty ForegroundProperty = DependencyProperty.Register(
        "Foreground",
        typeof(Brush),
        typeof(ThemeBrush),
        new PropertyMetadata(null)
    );

    /// <summary>
    /// operation area brush.
    /// </summary>
    public Brush OperationAreaBrush
    {
        get { return (Brush)GetValue(OperationAreaBrushProperty); }
        set { SetValue(OperationAreaBrushProperty, value); }
    }

    /// <summary>
    /// operation area brush property
    /// </summary>
    public static readonly DependencyProperty OperationAreaBrushProperty = DependencyProperty.Register(
        "OperationAreaBrush",
        typeof(Brush),
        typeof(ThemeBrush),
        new PropertyMetadata(null)
    );
}
