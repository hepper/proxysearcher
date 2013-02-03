using System.Windows;
using System.Windows.Controls;
using ProxySearch.Common;
using ProxySearch.Console.Code.Interfaces;

namespace ProxySearch.Console.Controls
{
    /// <summary>
    /// Interaction logic for BackPlaceholder.xaml
    /// </summary>
    public partial class BackControl : UserControl
    {
        public BackControl()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Context.Get<IControlNavigator>().GoToSearch();
        }
    }
}
