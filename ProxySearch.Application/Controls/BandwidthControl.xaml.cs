using System.Net.Http.Handlers;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ProxySearch.Common;
using ProxySearch.Console.Code.Interfaces;
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
        private CancellationTokenSource cancellationToken = null;

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
            cancellationToken = new CancellationTokenSource();
            Context.Get<BandwidthManager>().Measure(ProxyInfo, cancellationToken);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            cancellationToken.Cancel();
        }
    }
}
