using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using Toolkits.Wpf;
using ColorConverter = System.Windows.Media.ColorConverter;

namespace Toolkits.Controls.PopupView;

internal class ThemeDataContext : INotifyPropertyChanged
{
    public static ThemeDataContext themeDataContext = new();

    Brush? borderBrush;
    Brush? background;
    Brush? foreground;
    Brush? operationAreaBrush;

    public event PropertyChangedEventHandler? PropertyChanged;

    private void SetProperty<T>(ref T value, T newValue, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(value, newValue) == false)
        {
            value = newValue;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public Brush? BorderBrush
    {
        get => borderBrush;
        set => SetProperty(ref borderBrush, value);
    }
    public Brush? Background
    {
        get => background;
        set => SetProperty(ref background, value);
    }
    public Brush? Foreground
    {
        get => foreground;
        set => SetProperty(ref foreground, value);
    }
    public Brush? OperationAreaBrush
    {
        get => operationAreaBrush;
        set => SetProperty(ref operationAreaBrush, value);
    }

    private ThemeDataContext()
    {
        ChangedTheme(false);
    }

    public void ChangedTheme(PopupTheme? popupTheme)
    {
        if (popupTheme is null)
        {
            return;
        }

        Foreground = popupTheme.Foreground;
        Background = popupTheme.Background;

        BorderBrush = popupTheme.BorderBrush;
        OperationAreaBrush = popupTheme.OperationAreaBrush;
    }

    public void ChangedTheme(bool isDarkTheme)
    {
        Foreground = isDarkTheme ? new SolidColorBrush(Colors.White) : new SolidColorBrush(Colors.Black);
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
