using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using ProxySearch.Console.Code.Interfaces;

namespace ProxySearch.Console.Controls
{
    /// <summary>
    /// Interaction logic for ActiveProxyClientControl.xaml
    /// </summary>
    public partial class ActiveProxyClientControl : UserControl
    {
        public static readonly DependencyProperty ActiveProxyClientProperty =
            DependencyProperty.Register("ActiveProxyClient", typeof(IProxyClient), typeof(ActiveProxyClientControl));

        public IProxyClient ActiveProxyClient
        {
            get
            {
                return (IProxyClient)GetValue(ActiveProxyClientProperty);
            }
            set
            {
                SetValue(ActiveProxyClientProperty, value);
            }
        }

        public ActiveProxyClientControl()
        {
            InitializeComponent();
        }

        private void ClearProxy(object sender, RoutedEventArgs e)
        {
            ActiveProxyClient.Proxy = null;
        }
    }
}
