using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using ProxySearch.Common;
using ProxySearch.Console.Code.Detectable;
using ProxySearch.Console.Code.Interfaces;
using ProxySearch.Console.Code.Settings;
using ProxySearch.Engine.GeoIP;

namespace ProxySearch.Console.Controls
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class SettingsControl : UserControl
    {
        public SettingsControl()
        {
            InitializeComponent();
        }

        public AllSettings AllSettings
        {
            get
            {
                return Context.Get<AllSettings>();
            }
        }

        public List<IDetectable> GeoIPs
        {
            get
            {
                return Context.Get<IDetectableSearcher>().Get<IGeoIP>();
            }
        }

        public int SelectedGeoIPIndex
        {
            get
            {
                return GeoIPs.FindIndex(item => item.GetType().AssemblyQualifiedName == Context.Get<AllSettings>().GeoIPDetectableType);
            }
            set
            {
                Context.Get<AllSettings>().GeoIPDetectableType = GeoIPs[value].GetType().AssemblyQualifiedName;
            }
        }

        private void RestoreDefaults_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(Properties.Resources.AllSettingsWillBeRevertedToTheirDefaults, Properties.Resources.Question, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Context.Set<AllSettings>(new DefaultSettingsFactory().Create());
                Context.Get<IControlNavigator>().GoTo(new SettingsControl());
            }
        }

        private void ClearHistory_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(Properties.Resources.DoYouReallyWantToClearProxyUsageHistory, Properties.Resources.Question, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Context.Get<UsedProxies>().Proxies.Clear();
            }
        }
    }
}
