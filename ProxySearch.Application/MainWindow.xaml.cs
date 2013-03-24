using System;
using System.Windows;
using System.Windows.Controls;
using ProxySearch.Common;
using ProxySearch.Console.Code;
using ProxySearch.Console.Code.Interfaces;
using ProxySearch.Console.Controls;
using ProxySearch.Engine.SearchEngines.Google;

namespace ProxySearch.Console
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ICaptchaWindow
    {
        private SearchControl searchControl = new SearchControl();

        public MainWindow()
        {
            InitializeComponent();

            Context.Set<IControlNavigator>(new ControlNavigator(Placeholder));
            Context.Set<IActionInvoker>(ActionInvoker);
            Context.Set<ICaptchaWindow>(this);
        }

        public void ShowControl(UserControl control)
        {
            Placeholder.Content = control;
        }

        public void GoToSearch()
        {
            Placeholder.Content = searchControl;
        }

        public void Show(string url)
        {            
        }
    }
}
