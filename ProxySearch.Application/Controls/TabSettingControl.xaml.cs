﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using ProxySearch.Common;
using ProxySearch.Console.Code.Detectable;
using ProxySearch.Console.Code.Interfaces;
using ProxySearch.Console.Code.Settings;
using ProxySearch.Console.Code.UI;
using ProxySearch.Engine.Checkers;
using ProxySearch.Engine.GeoIP;
using ProxySearch.Engine.SearchEngines;

namespace ProxySearch.Console.Controls
{
    /// <summary>
    /// Interaction logic for TabSettingControl.xaml
    /// </summary>
    public partial class TabSettingControl : UserControl, INotifyPropertyChanged
    {
        public TabSettingControl()
        {
            AllTabSettings.CollectionChanged += AllTabSettings_CollectionChanged;
            ExtendedTabSettings = new ObservableCollection<object>(AllTabSettings);
            ExtendedTabSettings.Insert(0, new GeneralTabSettings());
            ExtendedTabSettings.Add(new DummyTabSettings());

            InitializeComponent();
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
                var tabIndex = PropertyTabControl.SelectedIndex > 0 && PropertyTabControl.SelectedIndex <= AllTabSettings.Count
                                   ? PropertyTabControl.SelectedIndex - 1
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
                return Context.Get<IDetectableSearcher>().Get<ISearchEngine>();
            }
        }

        public List<IDetectable> ProxyCheckers
        {
            get
            {
                return Context.Get<IDetectableSearcher>().Get<IProxyChecker>();
            }
        }

        public List<IDetectable> GeoIPs
        {
            get
            {
                return Context.Get<IDetectableSearcher>().Get<IGeoIP>();
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

        private void AllTabSettings_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    ExtendedTabSettings.Insert(ExtendedTabSettings.Count - 1, e.NewItems[0]);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    PropertyTabControl.SelectedIndex = e.OldStartingIndex - 1;
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
                MessageBox.Show(Properties.Resources.YouCannotDeleteLastSearchSettings, Properties.Resources.Information, MessageBoxButton.OK, MessageBoxImage.Information);
            else
                AllTabSettings.RemoveAt(PropertyTabControl.SelectedIndex - 1);
        }

        private void PropertyTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 1 && e.AddedItems[0].GetType() == typeof(DummyTabSettings))
            {
                TabSettings settings = new DefaultSettingsFactory().CreateHttpTabSettings();
                AllTabSettings.Add(settings);
                PropertyTabControl.SelectedValue = settings;
            }

            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedProxyCheckerIndex"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedSearchEngineIndex"));
            }
        }

        private void TabNameControl_Menu(object sender, RoutedEventArgs e)
        {
            PropertyTabControl.SelectedValue = ((TabItem)((TabNameControl)sender).Tag).DataContext;
        }

        private void RestoreDefaults_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(Properties.Resources.AllSettingsWillBeRevertedToTheirDefaults, Properties.Resources.Question, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                AllSettings = new DefaultSettingsFactory().Create();
                Context.Get<ISearchControl>().Rebind();
                Context.Get<IControlNavigator>().GoTo(new SettingsControl());
            }
        }

        private void ClearHistory_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(Properties.Resources.DoYouReallyWantToClearProxyUsageHistory, Properties.Resources.Question, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Context.Get<IUsedProxies>().Clear();
            }
        }

        private void ClearBlackList_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(Properties.Resources.DoYouReallyWantToClearBlacklist, Properties.Resources.Question, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Context.Get<IBlackListManager>().Clear();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
