using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace Toolkits.Popup;

/// <summary>
/// ToastView.xaml
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
internal partial class ToastView : PopupToastViewBase
{
    /// <summary>
    ///
    /// </summary>
    public ToastView()
    {
        InitializeComponent();
        DataContext = ThemeDataContext.themeDataContext;
    }

    /// <summary>
    /// set toast message info
    /// </summary>
    /// <param name="title"></param>
    /// <param name="message"></param>
    /// <param name="objects"></param>
    protected override void SetToastInfo(string title, string message, params object[] @objects)
    {
        TitleBox.Text = title;
        MessageBox.Text = message;
    }

    void ClosePopupClick(object sender, MouseButtonEventArgs e)
    {
        base.CloseToast();
    }
}
