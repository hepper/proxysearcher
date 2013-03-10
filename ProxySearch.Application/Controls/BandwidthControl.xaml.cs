using System.Windows;
using System.Windows.Controls;
using ProxySearch.Common;
using ProxySearch.Engine;
using ProxySearch.Engine.Bandwidth;

namespace ProxySearch.Console.Controls
{
    /// <summary>
    /// Interaction logic for BandwidthControl.xaml
    /// </summary>
    public partial class BandwidthControl : UserControl
    {
        public static readonly DependencyProperty ProxyInfoProperty = DependencyProperty.Register("ProxyInfo", typeof(ProxyInfo), typeof(BandwidthControl));

        public BandwidthControl()
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

        private void MeasureButton_Click(object sender, RoutedEventArgs e)
        {
            Context.Get<BandwidthManager>().MeasureAsync(ProxyInfo);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Context.Get<BandwidthManager>().Cancel(ProxyInfo);
        }
    }
}
