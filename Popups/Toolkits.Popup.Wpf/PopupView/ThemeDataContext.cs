using System.Windows.Media;
using PropertyChanged;

namespace Toolkits.Popup;

[AddINotifyPropertyChangedInterface]
internal class ThemeDataContext
{
    public static ThemeDataContext themeDataContext = new();

    public Brush? BorderBrush { get; set; }
    public Brush? Background { get; set; }
    public Brush? Foreground { get; set; }
    public Brush? OperationAreaBrush { get; set; }

    private ThemeDataContext()
    {
        ChangedTheme(false);
    }

    public void ChangedTheme(bool isDarkTheme)
    {
        Foreground = isDarkTheme
            ? new SolidColorBrush(Colors.White)
            : new SolidColorBrush(Colors.Black);
        Background = isDarkTheme
            ? new SolidColorBrush(
                (Color)System.Windows.Media.ColorConverter.ConvertFromString("#1e1e1e")
            )
            : new SolidColorBrush(
                (Color)System.Windows.Media.ColorConverter.ConvertFromString("#fafafa")
            );

        BorderBrush = isDarkTheme
            ? new SolidColorBrush(
                (Color)System.Windows.Media.ColorConverter.ConvertFromString("#6a6a6a")
            )
            : new SolidColorBrush(
                (Color)System.Windows.Media.ColorConverter.ConvertFromString("#8f8f8f")
            );

        OperationAreaBrush = isDarkTheme
            ? new SolidColorBrush(
                (Color)System.Windows.Media.ColorConverter.ConvertFromString("#444444")
            )
            : new SolidColorBrush(
                (Color)System.Windows.Media.ColorConverter.ConvertFromString("#eaeaea")
            );
    }
}
