using System.Windows;
using System.Windows.Controls;
using ProxySearch.Common;
using ProxySearch.Console.Code;
using ProxySearch.Console.Code.Interfaces;
using ProxySearch.Console.Controls;

namespace ProxySearch.Console
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SearchControl searchControl = new SearchControl();

        public MainWindow()
        {
            InitializeComponent();

            Context.Set<IControlNavigator>(new ControlNavigator(Placeholder));
            Context.Set<IActionInvoker>(ActionInvoker);
        }

        public void ShowControl(UserControl control)
        {
            Placeholder.Content = control;
        }

        public void GoToSearch()
        {
            Placeholder.Content = searchControl;
        }
    }
}
