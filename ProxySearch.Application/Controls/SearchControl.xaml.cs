using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using ProxySearch.Common;
using ProxySearch.Console.Code;
using ProxySearch.Console.Code.GoogleAnalytics;
using ProxySearch.Console.Code.GoogleAnalytics.Timing;
using ProxySearch.Console.Code.Interfaces;
using ProxySearch.Console.Code.Settings;
using ProxySearch.Engine.Error;
using ProxySearch.Engine.Tasks;

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

            Context.Set<ISearchControl>(this);

            Context.Get<ITaskManager>().OnCompleted += () =>
            {
                Dispatcher.Invoke(() => EnableButtons(true));
            };
        }

        private void BeginSearch_Click(object sender, RoutedEventArgs e)
        {
            Context.Get<IGA>().TrackEventAsync(EventType.ButtonClick, Buttons.BeginSearch.ToString());
            Context.Get<IGA>().TrackEventAsync(EventType.General, Properties.Resources.SearchStarted);
            Context.Get<IGA>().StartTrackTiming(TimingCategory.SearchSpeed, TimingVariable.TimeForGetFirstProxy);

            EnableButtons(false);

            Context.Get<ISearchResult>().Clear();
            Context.Get<IActionInvoker>().StartAsync(DoBeginSearch);
        }

        private void EnableButtons(bool isEnabled)
        {
            beginSearchButton.IsEnabled = isEnabled;
            fastSettingsButton.IsEnabled = isEnabled;

            if (!isEnabled)
                fastSettingsButton.IsExpanded = false;
        }

        private async void DoBeginSearch()
        {
            ProxySearchFeedback feedback = new ProxySearchFeedback();

            using (TaskItem task = Context.Get<ITaskManager>().Create(Properties.Resources.SearchInitialization))
            {
                Engine.Application application = new ProxySearchEngineApplicationFactory().Create(task, feedback);
                await application.SearchAsync(Context.Get<CancellationTokenSource>());
            }
        }

        public ObservableCollection<TabSettings> TabSettings
        {
            get
            {
                return Context.Get<AllSettings>().TabSettings;
            }
        }

        public void Rebind()
        {
            CurrentTab.GetBindingExpression(ComboBox.ItemsSourceProperty).UpdateTarget();
            CurrentTab.GetBindingExpression(ComboBox.SelectedValueProperty).UpdateTarget();
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
                searchSpeedControl.UpdateBindings();
            }
        }
    }
}
