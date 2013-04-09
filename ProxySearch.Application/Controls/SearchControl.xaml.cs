using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using ProxySearch.Common;
using ProxySearch.Console.Code;
using ProxySearch.Console.Code.Interfaces;
using ProxySearch.Console.Code.Settings;
using ProxySearch.Engine;

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
        }

        private void BeginSearch_Click(object sender, RoutedEventArgs e)
        {
            BeginSearch.IsEnabled = false;
            Context.Get<ISearchResult>().Clear();
            Context.Get<IActionInvoker>().StartAsync(DoBeginSearch);
        }

        private void DoBeginSearch()
        {
            Context.Set(new TaskCounter());

            ProxySearchFeedback feedback = new ProxySearchFeedback();

            Context.Get<TaskCounter>().OnStarted += Context.Get<IActionInvoker>().Begin;

            Context.Get<TaskCounter>().OnCompleted += () =>
            {
                if (Context.Get<CancellationTokenSource>().IsCancellationRequested)
                {
                    feedback.OnSearchCancelled();
                }
                else
                {
                    feedback.OnSearchFinished();
                }
            };

            Context.Get<TaskCounter>().JobCountChanged += feedback.UpdateJobCount;

            using (Context.Get<TaskCounter>().Listen(TaskType.Init))
            {
                Engine.Application application = new ProxySearchEngineApplicationFactory().Create(feedback);
                application.SearchAsync();
            }
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
            }
        }
    }
}
