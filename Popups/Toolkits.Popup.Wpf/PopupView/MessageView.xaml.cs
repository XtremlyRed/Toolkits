using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Toolkits.Popup;

/// <summary>
/// MessageView.xaml
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
internal partial class MessageView : PopupMessageViewBase
{
    private string[]? buttonContents;
    private bool isLoaded = false;

    internal MessageView()
    {
        InitializeComponent();
        DataContext = ThemeDataContext.themeDataContext;
    }

    /// <summary>
    /// </summary>
    /// <param name="message"></param>
    /// <param name="title"></param>
    /// <param name="buttonContents"></param>
    protected override void SetPopupMessageInfo(
        string message,
        string title,
        string[] buttonContents
    )
    {
        TitleBox.Text = title;
        MessageBox.Text = message;

        ButtonBoxs.ItemsSource = this.buttonContents = buttonContents;
        isLoaded = true;
    }

    private void Button_Loaded(object sender, RoutedEventArgs e)
    {
        if (sender is not Button btn || isLoaded == false)
        {
            return;
        }

        isLoaded = false;

        Container.MinWidth = Application.Current?.MainWindow.Width / 3d ?? 400d;

        Container.MaxWidth = Application.Current?.MainWindow.Width / 3d * 2d ?? 600d;

        if ((btn.Content as string)! == buttonContents![0])
        {
            btn.Foreground = System.Windows.Media.Brushes.White;
            btn.Background = SystemParameters.WindowGlassBrush;
            btn.Focus();
        }
    }

    private void Btn_Container_Loaded(object sender, RoutedEventArgs e)
    {
        if (sender is not UniformGrid uniformGrid || buttonContents == null)
        {
            return;
        }

        if (buttonContents.Length > 2)
        {
            uniformGrid.HorizontalAlignment = HorizontalAlignment.Stretch;
            return;
        }

        uniformGrid.HorizontalAlignment = HorizontalAlignment.Right;
    }

    private void Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button brn)
        {
            return;
        }

        base.SetCurrentClickContent((brn.Content as string)!);
    }
}
