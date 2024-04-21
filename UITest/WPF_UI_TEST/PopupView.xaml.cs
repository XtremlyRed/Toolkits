using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Toolkits.Controls;

namespace WPF_UI_TEST
{
    /// <summary>
    /// PopupView.xaml 的交互逻辑
    /// </summary>
    public partial class PopupView : UserControl, IPopupAware
    {
        public PopupView()
        {
            InitializeComponent();
        }

        public event RequestCloseEventHandler? RequestCloseEvent;

        public void OnPopupClosed() { }

        public void OnPopupOpened(Parameters? parameters) { }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RequestCloseEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}
