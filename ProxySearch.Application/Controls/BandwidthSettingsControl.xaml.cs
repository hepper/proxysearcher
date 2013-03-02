using System.Windows.Controls;
using ProxySearch.Common;
using ProxySearch.Console.Code.Settings;

namespace ProxySearch.Console.Controls
{
    /// <summary>
    /// Interaction logic for BandwidthSettingsControl.xaml
    /// </summary>
    public partial class BandwidthSettingsControl : UserControl
    {
        public BandwidthSettingsControl()
        {
            InitializeComponent();
        }

        public double MaxBandwidth
        {
            get
            {
                return Context.Get<AllSettings>().MaxBandwidth;
            }

            set
            {
                Context.Get<AllSettings>().MaxBandwidth = value;
            }
        }
    }
}
