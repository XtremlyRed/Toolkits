using System.Windows.Media;
using PropertyChanged;
using ColorConverter = System.Windows.Media.ColorConverter;

namespace Toolkits.Controls.PopupView;

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
            ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1e1e1e"))
            : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#fafafa"));

        BorderBrush = isDarkTheme
            ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6a6a6a"))
            : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8f8f8f"));

        OperationAreaBrush = isDarkTheme
            ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#444444"))
            : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#eaeaea"));
    }
}
