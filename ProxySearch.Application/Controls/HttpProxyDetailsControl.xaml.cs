using System.Windows;
using System.Windows.Controls;
using ProxySearch.Engine.Proxies;

namespace ProxySearch.Console.Controls
{
    /// <summary>
    /// Interaction logic for HttpProxyDetailsControl.xaml
    /// </summary>
    public partial class HttpProxyDetailsControl : UserControl
    {
        public static readonly DependencyProperty ProxyProperty = DependencyProperty.Register("Proxy", typeof(ProxyInfo), typeof(HttpProxyDetailsControl));

        public HttpProxyDetailsControl()
        {
            InitializeComponent();
        }

        public ProxyInfo Proxy
        {
            get
            {
                return (ProxyInfo)this.GetValue(ProxyProperty);
            }
            set
            {
                this.SetValue(ProxyProperty, value);
            }
        }
    }
}
