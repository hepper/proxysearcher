using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ProxySearch.Common;
using ProxySearch.Console.Code;
using ProxySearch.Console.Code.Interfaces;
using ProxySearch.Console.Code.Settings;

namespace ProxySearch.Console.Controls
{
    /// <summary>
    /// Interaction logic for MainFrame.xaml
    /// </summary>
    public partial class SearchControl : UserControl, ISearchControl
    {
        public SearchControl()
        {
            InitializeComponent();
        }

        private void BeginSearch_Click(object sender, RoutedEventArgs e)
        {
            Context.Set<ISearchControl>(this);

            BeginSearch.IsEnabled = false;
            Context.Get<ISearchResult>().Clear();

            Context.Get<IActionInvoker>().Begin(async () =>
            {
                Engine.Application application = await new ProxySearchEngineApplicationFactory().Create(new ProxySearchFeedback());
                await application.SearchAsync();
            });
        }

        public void Completed()
        {
            BeginSearch.IsEnabled = true;
        }

        public ObservableCollection<TabSettings> TabSettings
        {
            get
            {
                return Context.Get<AllSettings>().TabSettings;
            }
        }

        public Guid CurrentTabId
        {
            get
            {
                return Context.Get<AllSettings>().SelectedTabSettingsId;
            }
            set
            {
                Context.Get<AllSettings>().SelectedTabSettingsId = value;
            }
        }
    }
}
