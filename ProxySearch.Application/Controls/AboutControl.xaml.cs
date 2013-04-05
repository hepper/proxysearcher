using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using ProxySearch.Common;
using ProxySearch.Console.Code.Interfaces;

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

        private void Donate(object sender, RoutedEventArgs e)
        {
            Process.Start(Properties.Resources.DonateLink);
        }

        public string ProgramName
        {
            get
            {
                int version = Context.Get<IVersionProvider>().Version;
                int minor = version % 10;
                int major = (version - version % 10) / 10 + 1;

                return string.Format(Properties.Resources.ProxySearcherVersionFormat, major, minor);
            }
        }
    }
}
