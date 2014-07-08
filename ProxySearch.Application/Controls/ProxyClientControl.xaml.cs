using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using ProxySearch.Common;
using ProxySearch.Console.Code.GoogleAnalytics;
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
        public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ProxyClientControl));

        public event RoutedEventHandler Click
        {
            add { AddHandler(ClickEvent, value); }
            remove { RemoveHandler(ClickEvent, value); }
        }

        private static event Action notifyAllInstances;

        public ProxyClientControl()
        {
            notifyAllInstances += () =>
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
                Context.Get<IGA>().TrackEventAsync(EventType.ButtonClick, 
                                                   string.Format("{0}_{1}", Buttons.ProxyClient, ProxyClient.GetType().Name), value != null); 
                ProxyClient.Proxy = value ? ProxyInfo : null;

                if (ProxyClient.Proxy != ProxyInfo)
                {
                    Context.Get<IGA>().TrackException(new InvalidOperationException(string.Format("Proxy was not set: {0}!={1}", ProxyClient.Proxy, ProxyInfo)));
                }

                RaiseEvent(new RoutedEventArgs(ProxyClientControl.ClickEvent));
                notifyAllInstances();
            }
        }

        private void IsCheckedChanged(object sender, EventArgs e)
        {
            notifyAllInstances();
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
