using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using ProxySearch.Console.Code.Interfaces;
using ProxySearch.Engine.Proxies;

namespace ProxySearch.Console.Controls
{
    /// <summary>
    /// Interaction logic for ProxyClientControl.xaml
    /// </summary>
    public partial class ProxyClientControl : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty ProxyInfoProperty = DependencyProperty.Register("ProxyInfo", typeof(ProxyInfo), typeof(ProxyClientControl));
        public static readonly DependencyProperty ProxyClientProperty = DependencyProperty.Register("ProxyClient", typeof(IProxyClient), typeof(ProxyClientControl));

        private static event Action isCheckedChanged;

        public ProxyClientControl()
        {
            isCheckedChanged += () =>
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("IsChecked"));
                }
            };

            InitializeComponent();

            DependencyPropertyDescriptor.FromProperty(ProxyClientProperty, typeof(ProxyClientControl)).AddValueChanged(this, IsCheckedChanged);
            DependencyPropertyDescriptor.FromProperty(ProxyInfoProperty, typeof(ProxyClientControl)).AddValueChanged(this, IsCheckedChanged);
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

        public IProxyClient ProxyClient
        {
            get
            {
                return (IProxyClient)this.GetValue(ProxyClientProperty);
            }
            set
            {
                this.SetValue(ProxyClientProperty, value);
            }
        }

        public bool IsChecked
        {
            get
            {
                if (ProxyClient == null)
                {
                    return false;
                }

                return ProxyClient.Proxy == ProxyInfo;
            }
            set
            {
                IProxyClientRestartable restartableProxyClient = ProxyClient as IProxyClientRestartable;
                bool restartRequested = false;

                if (restartableProxyClient != null && restartableProxyClient.IsRunning)
                {
                    MessageBoxResult result = MessageBox.Show(
                                                string.Format(Properties.Resources.DoYouWantToRestartBrowser, ProxyClient.Name),
                                                Properties.Resources.Question,
                                                MessageBoxButton.OKCancel,
                                                MessageBoxImage.Question);

                    if (result == MessageBoxResult.Cancel)
                    {
                        return;
                    }

                    restartRequested = true;
                }

                if (restartRequested)
                {
                    restartableProxyClient.Close();
                }

                ProxyClient.Proxy = value ? ProxyInfo : null;

                if (restartRequested)
                {
                    restartableProxyClient.Open();
                }

                isCheckedChanged();
            }
        }

        private void IsCheckedChanged(object sender, EventArgs e)
        {
            isCheckedChanged();
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
