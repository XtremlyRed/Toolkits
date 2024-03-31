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
using Toolkits.Popup;

namespace WPF_UI_TEST
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PopupManager popupManager = new PopupManager();

            for (int i = 0; i < 5; i++)
            {
                ThreadPool.QueueUserWorkItem(async o =>
                {
                    var resu = await popupManager.PopupAsync(() => new PopupView());
                });
            }
        }
    }
}
