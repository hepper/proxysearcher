using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProxySearch.Console.Controls
{
    /// <summary>
    /// Interaction logic for AboutControl.xaml
    /// </summary>
    public partial class AboutControl : UserControl
    {
        public AboutControl()
        {
            InitializeComponent();
        }

        private void LeaveYourFeedback(object sender, RoutedEventArgs e)
        {
            Process.Start(Properties.Resources.FeedbackLink);
        }

        private void ProxySearchNews(object sender, RoutedEventArgs e)
        {
            Process.Start(Properties.Resources.NewsLink);
        }

        private void Tickets(object sender, RoutedEventArgs e)
        {
            Process.Start(Properties.Resources.TicketsLink);
        }

        private void HomePage(object sender, RoutedEventArgs e)
        {
            Process.Start(Properties.Resources.HomePageLink);
        }
    }
}
