using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using ProxySearch.Console.Code;
using ProxySearch.Console.Code.Interfaces;

namespace ProxySearch.Console.Controls
{
    /// <summary>
    /// Interaction logic for Toolbar.xaml
    /// </summary>
    public partial class ToolbarControl : UserControl, INotifyPropertyChanged
    {
        public List<IProxyClient> UsedProxyClients
        {
            get
            {
                return Context.Get<IProxyClientSearcher>()
                              .AllClients
                              .Where(client => client.Proxy != null)
                              .OrderBy(client => client.Order)
                              .ToList();

            }
        }

        public ToolbarControl()
        {
            InitializeComponent();

            Context.Get<IProxyClientSearcher>().AllClients.ForEach(client => client.ProxyChanged += OnRefresh);
        }

        private void Tools_Click(object sender, RoutedEventArgs e)
        {
            Tools.IsExpanded = false;
            Tools.ContextMenu.IsEnabled = true;
            Tools.ContextMenu.PlacementTarget = Tools;
            Tools.ContextMenu.Placement = PlacementMode.Bottom;
            Tools.ContextMenu.IsOpen = true;
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            Context.Get<IControlNavigator>().GoTo(new SettingsControl());
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            Context.Get<IControlNavigator>().GoTo(new AboutControl());
        }

        private void ContextMenu_Closed(object sender, RoutedEventArgs e)
        {
            Tools.IsExpanded = false;
        }

        private void OnRefresh(object sender, RoutedEventArgs e)
        {
            OnRefresh();
        }

        private void OnRefresh()
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("UsedProxyClients"));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
