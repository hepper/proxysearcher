﻿using System.Threading;
using System.Threading.Tasks;
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

        private async void Update_Click(object sender, RoutedEventArgs e)
        {
            Proxy.Details.IsUpdating = true;

            try
            {
                Proxy.Details.Details = await Proxy.Details.UpdateMethod(Proxy, Proxy.Details.CancellationToken);
            }
            catch (TaskCanceledException)
            {
            }
            finally
            {
                Proxy.Details.IsUpdating = false;
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Proxy.Details.CancellationToken.Cancel();
        }
    }
}
