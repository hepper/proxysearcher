using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using ProxySearch.Common;
using ProxySearch.Console.Code.Interfaces;
using ProxySearch.Console.Code.Settings;
using ProxySearch.Engine.Parser;

namespace ProxySearch.Console.Controls
{
    /// <summary>
    /// Interaction logic for AdvancedTabControl.xaml
    /// </summary>
    public partial class AdvancedTabControl : UserControl
    {
        public AdvancedTabControl()
        {
            InitializeComponent();
        }

        public List<ParseDetails> ParseDetails
        {
            get
            {
                return Context.Get<AllSettings>().ParseDetails;
            }
        }

        private void DeleteButtonClick(object sender, RoutedEventArgs e)
        {
            if (Context.Get<IMessageBox>().OkCancelQuestion(Properties.Resources.DoYouReallyWantToDeleteThisParseMethod) == MessageBoxResult.OK)
            {
                ParseDetails.Remove((ParseDetails)((Button)sender).Tag);
                parseMethodsGrid.Items.Refresh();
            }
        }

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            Context.Get<IControlNavigator>().GoTo(new ParseMethodEditor() { ParseDetails = new ParseDetails() 
                                                                                           { Url = "Enter domain name here", 
                                                                                             RawRegularExpression = ".*",
                                                                                             Code="return null;" }, 
                                                                            IsNew = true });
        }

        private void EditButtonClick(object sender, RoutedEventArgs e)
        {
            Context.Get<IControlNavigator>().GoTo(new ParseMethodEditor() { ParseDetails = (ParseDetails)((Button)sender).Tag, IsNew = false });
        }
    }
}
