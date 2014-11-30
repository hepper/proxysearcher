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
    public partial class ToolbarControl : UserControl
    {
        public ToolbarControl()
        {
            InitializeComponent();
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
    }
}
