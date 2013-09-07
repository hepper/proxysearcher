using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using ProxySearch.Common;
using ProxySearch.Console.Code.Detectable;
using ProxySearch.Console.Code.GoogleAnalytics;
using ProxySearch.Console.Code.Interfaces;
using ProxySearch.Console.Code.Settings;
using ProxySearch.Console.Code.UI;
using ProxySearch.Engine.Checkers;
using ProxySearch.Engine.GeoIP;
using ProxySearch.Engine.ProxyDetailsProvider;
using ProxySearch.Engine.SearchEngines;

namespace ProxySearch.Console.Controls
{
    /// <summary>
    /// Interaction logic for TabSettingControl.xaml
    /// </summary>
    public partial class TabSettingControl : UserControl, INotifyPropertyChanged, ISettingsTabNavigator
    {
        public TabSettingControl()
        {
            AllTabSettings.CollectionChanged += AllTabSettings_CollectionChanged;
            ExtendedTabSettings = new ObservableCollection<object>(AllTabSettings);
            ExtendedTabSettings.Insert(0, new GeneralTabSettings());
            ExtendedTabSettings.Insert(1, new AdvancedTabSettings());
            ExtendedTabSettings.Add(new DummyTabSettings());

            InitializeComponent();

            Context.Set<ISettingsTabNavigator>(this);
        }

        public AllSettings AllSettings
        {
            get
            {
                return Context.Get<AllSettings>();
            }
            set
            {
                Context.Set(value);
            }
        }

        public ObservableCollection<TabSettings> AllTabSettings
        {
            get
            {
                return AllSettings.TabSettings;
            }
        }

        public TabSettings CurrentTabSettings
        {
            get
            {
                var tabIndex = PropertyTabControl.SelectedIndex > 0 && PropertyTabControl.SelectedIndex - 1 <= AllTabSettings.Count
                                   ? PropertyTabControl.SelectedIndex - 2
                                   : 0;
                return AllTabSettings[tabIndex];
            }
        }

        public ObservableCollection<object> ExtendedTabSettings
        {
            get;
            set;
        }

        public List<IDetectable> SearchEngines
        {
            get
            {
                return Context.Get<IDetectableSearcher>().Get<ISearchEngine>(ProxyTypes[SelectedProxyTypeIndex]);
            }
        }

        public List<IDetectable> ProxyCheckers
        {
            get
            {
                return Context.Get<IDetectableSearcher>().Get<IProxyChecker>(ProxyTypes[SelectedProxyTypeIndex]);
            }
        }

        public List<IDetectable> ProxyTypes
        {
            get
            {
                return Context.Get<IDetectableSearcher>().Get<IProxyType>();
            }
        }

        public List<IDetectable> GeoIPs
        {
            get
            {
                return Context.Get<IDetectableSearcher>().Get<IGeoIP>();
            }
        }

        public List<IDetectable> ProxyDetailsProvider
        {
            get
            {
                return Context.Get<IDetectableSearcher>().Get<IProxyDetailsProvider>();
            }
        }

        public int SelectedSearchEngineIndex
        {
            get
            {
                return SearchEngines.FindIndex(item => item.GetType().AssemblyQualifiedName == CurrentTabSettings.SearchEngineDetectableType);
            }
            set
            {
                CurrentTabSettings.SearchEngineDetectableType = SearchEngines[value].GetType().AssemblyQualifiedName;
                FirePropertyChanged("SelectedSearchEngineIndex");
            }
        }

        public int SelectedProxyCheckerIndex
        {
            get
            {
                return ProxyCheckers.FindIndex(item => item.GetType().AssemblyQualifiedName == CurrentTabSettings.ProxyCheckerDetectableType);
            }
            set
            {
                CurrentTabSettings.ProxyCheckerDetectableType = ProxyCheckers[value].GetType().AssemblyQualifiedName;
                FirePropertyChanged("SelectedProxyCheckerIndex");
            }
        }

        public int SelectedProxyTypeIndex
        {
            get
            {
                return ProxyTypes.FindIndex(item => ((IProxyType)Activator.CreateInstance(item.Implementation)).Type == CurrentTabSettings.ProxyType);
            }
            set
            {
                CurrentTabSettings.ProxyType = ((IProxyType)Activator.CreateInstance(ProxyTypes[value].Implementation)).Type;

                UpdateBindings();

                FirePropertyChanged("SelectedProxyTypeIndex");

                SelectedSearchEngineIndex = 0;
                SelectedProxyCheckerIndex = 0;
            }
        }

        public int SelectedGeoIPIndex
        {
            get
            {
                return GeoIPs.FindIndex(item => item.GetType().AssemblyQualifiedName == Context.Get<AllSettings>().GeoIPDetectableType);
            }
            set
            {
                Context.Get<AllSettings>().GeoIPDetectableType = GeoIPs[value].GetType().AssemblyQualifiedName;
            }
        }

        public void OpenAdvancedTab()
        {
            PropertyTabControl.SelectedIndex = 1;
        }

        private void AllTabSettings_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    ExtendedTabSettings.Insert(ExtendedTabSettings.Count - 1, e.NewItems[0]);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    PropertyTabControl.SelectedIndex = e.OldStartingIndex + 1;
                    ExtendedTabSettings.Remove(e.OldItems[0]);
                    break;
                case NotifyCollectionChangedAction.Move:
                case NotifyCollectionChangedAction.Replace:
                case NotifyCollectionChangedAction.Reset:
                default:
                    throw new NotSupportedException();
            }
        }

        private void TabNameControl_Delete(object sender, RoutedEventArgs e)
        {
            if (AllTabSettings.Count == 1)
                Context.Get<IMessageBox>().Information(Properties.Resources.YouCannotDeleteLastSearchSettings);
            else
                AllTabSettings.RemoveAt(PropertyTabControl.SelectedIndex - 2);
        }

        private void PropertyTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.OriginalSource is TabControl)
            {
                if (e.AddedItems.Count == 1 && e.AddedItems[0].GetType() == typeof(DummyTabSettings))
                {
                    TabSettings settings = new DefaultSettingsFactory().CreateDefaultTabSettings();
                    AllTabSettings.Add(settings);
                    PropertyTabControl.SelectedValue = settings;
                }

                UpdateBindings();
            }
        }

        private void UpdateBindings()
        {
            FirePropertyChanged("SearchEngines");
            FirePropertyChanged("ProxyCheckers");

            FirePropertyChanged("SelectedProxyTypeIndex");
            FirePropertyChanged("SelectedSearchEngineIndex");
            FirePropertyChanged("SelectedProxyCheckerIndex");
        }

        private void TabNameControl_Menu(object sender, RoutedEventArgs e)
        {
            PropertyTabControl.SelectedValue = ((TabItem)((TabNameControl)sender).Tag).DataContext;
        }

        private void RestoreDefaults_Click(object sender, RoutedEventArgs e)
        {
            if (Context.Get<IMessageBox>().YesNoQuestion(Properties.Resources.AllSettingsWillBeRevertedToTheirDefaults) == MessageBoxResult.Yes)
            {
                AllSettings = new DefaultSettingsFactory().Create();
                Context.Get<ISearchControl>().Rebind();
                Context.Get<IControlNavigator>().GoTo(new SettingsControl());
            }
        }

        private void ClearHistory_Click(object sender, RoutedEventArgs e)
        {
            if (Context.Get<IMessageBox>().YesNoQuestion(Properties.Resources.DoYouReallyWantToClearProxyUsageHistory) == MessageBoxResult.Yes)
            {
                Context.Get<IUsedProxies>().Clear();
            }
        }

        private void ClearBlackList_Click(object sender, RoutedEventArgs e)
        {
            if (Context.Get<IMessageBox>().YesNoQuestion(Properties.Resources.DoYouReallyWantToClearBlacklist) == MessageBoxResult.Yes)
            {
                Context.Get<IBlackListManager>().Clear();
            }
        }

        private void FirePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void ShareStatisticCheckBoxChecked(object sender, RoutedEventArgs e)
        {
            bool savedShareUsageStatistic = AllSettings.ShareUsageStatistic;

            try
            {
                AllSettings.ShareUsageStatistic = true;
                Context.Get<IGA>().TrackEventAsync(EventType.CheckBoxChanged, Properties.Resources.ShareCheckBox, savedShareUsageStatistic);
            }
            finally
            {
                AllSettings.ShareUsageStatistic = savedShareUsageStatistic;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
