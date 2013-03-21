using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using ProxySearch.Console.Code.Interfaces;
using ProxySearch.Engine;

namespace ProxySearch.Console.Controls
{
    /// <summary>
    /// Interaction logic for ProxyClientControl.xaml
    /// </summary>
    public partial class ProxyClientControl : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty ProxyInfoProperty = DependencyProperty.Register("ProxyInfo", typeof(ProxyInfo), typeof(ProxyClientControl));
        public static readonly DependencyProperty ProxyClientProperty = DependencyProperty.Register("ProxyClient", typeof(IProxyClient), typeof(ProxyClientControl));

        public ProxyClientControl()
        {
            InitializeComponent();

            DependencyPropertyDescriptor.FromProperty(ProxyClientProperty, typeof(ProxyClientControl)).AddValueChanged(this, ProxyClientPropertyChanged);
            DependencyPropertyDescriptor.FromProperty(ProxyInfoProperty, typeof(ProxyClientControl)).AddValueChanged(this, ProxyInfoPropertyChanged);
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
            }
        }

        private void ProxyClientPropertyChanged(object sender, EventArgs e)
        {
            FirePropertyChanged("IsChecked");
            ProxyClient.PropertyChanged += ProxyClient_PropertyChanged;
        }

        private void ProxyInfoPropertyChanged(object sender, EventArgs e)
        {
            FirePropertyChanged("IsChecked");
        }

        private void ProxyClient_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Proxy")
            {
                FirePropertyChanged("IsChecked");
            }
        }

        protected void FirePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
