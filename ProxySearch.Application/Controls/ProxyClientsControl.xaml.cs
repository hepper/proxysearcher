using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using ProxySearch.Console.Code;
using ProxySearch.Console.Code.Interfaces;
using ProxySearch.Engine.Proxies;

namespace ProxySearch.Console.Controls
{
    /// <summary>
    /// Interaction logic for ProxySubscribersControl.xaml
    /// </summary>
    public partial class ProxyClientsControl : UserControl
    {
        public static readonly DependencyProperty ProxyInfoProperty = DependencyProperty.Register("ProxyInfo", typeof(ProxyInfo), typeof(ProxyClientsControl));

        public ProxyClientsControl()
        {
            InitializeComponent();
        }

        public ProxyInfo ProxyInfo
        {
            get
            {
                return (ProxyInfo)this.GetValue(ProxyInfoProperty);
            }
            set
            {
                this.SetValue(ProxyInfoProperty, value);
            }
        }

        public List<IProxyClient> SelectedClients
        {
            get
            {
                return Context.Get<IProxyClientSearcher>().SelectedClients;
            }
        }
    }
}
