using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using ProxySearch.Common;
using ProxySearch.Console.Code.Detectable;
using ProxySearch.Console.Code.Interfaces;
using ProxySearch.Console.Code.Settings;
using ProxySearch.Console.Code.UI;
using ProxySearch.Engine;

namespace ProxySearch.Console.Controls
{
    /// <summary>
    /// Interaction logic for TabSettingControl.xaml
    /// </summary>
    public partial class TabSettingControl : UserControl
    {
        public TabSettingControl()
        {
            AllTabSettings.CollectionChanged += AllTabSettings_CollectionChanged;
            ExtendedTabSettings = new ObservableCollection<object>(AllTabSettings);
            ExtendedTabSettings.Add(new DummyTabSettings());

            InitializeComponent();
        }

        public ObservableCollection<TabSettings> AllTabSettings
        {
            get
            {
                return Context.Get<AllSettings>().TabSettings;
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

        public int SelectedSearchEngineIndex
        {
            get
            {
                return SearchEngines.FindIndex(item => item.GetType().AssemblyQualifiedName == AllTabSettings[PropertyTabControl.SelectedIndex].SearchEngineDetectableType);
            }
            set
            {
                AllTabSettings[PropertyTabControl.SelectedIndex].SearchEngineDetectableType = SearchEngines[value].GetType().AssemblyQualifiedName;
            }
        }

        public int SelectedProxyCheckerIndex
        {
            get
            {
                return ProxyCheckers.FindIndex(item => item.GetType().AssemblyQualifiedName == AllTabSettings[PropertyTabControl.SelectedIndex].ProxyCheckerDetectableType);
            }
            set
            {
                AllTabSettings[PropertyTabControl.SelectedIndex].ProxyCheckerDetectableType = ProxyCheckers[value].GetType().AssemblyQualifiedName;
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
            AllTabSettings.RemoveAt(PropertyTabControl.SelectedIndex);
        }

        private void PropertyTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 1 && e.AddedItems[0].GetType() == typeof(DummyTabSettings))
            {
                TabSettings settings = new DefaultSettingsFactory().CreateTabSettings();
                AllTabSettings.Add(settings);
                PropertyTabControl.SelectedValue = settings;
            }
        }

        private void TabNameControl_Menu(object sender, RoutedEventArgs e)
        {
            PropertyTabControl.SelectedValue = ((TabItem)((TabNameControl)sender).Tag).DataContext;
        }
    }
}
