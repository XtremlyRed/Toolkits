using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Toolkits;
using Toolkits.Controls;

namespace WPF_UI_TEST;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
[PropertyChanged.AddINotifyPropertyChangedInterface]
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;
    }

    public bool? Play { get; set; }

    public double Angle { get; set; }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        // Angle = 90;

        Play = true;

        //PopupManager popupManager = new PopupManager();

        //for (int i = 0; i < 5; i++)
        //{
        //    var resu = await popupManager.PopupAsync(() => new PopupView());
        //}
    }
}
