using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ProxySearch.Console.Code;
using ProxySearch.Console.Code.Filters;
using ProxySearch.Console.Code.Interfaces;
using ProxySearch.Console.Code.SearchResult;
using ProxySearch.Console.Code.Settings;
using ProxySearch.Engine.Proxies;
using ProxySearch.Engine.Tasks;

namespace ProxySearch.Console.Controls
{
    /// <summary>
    /// Interaction logic for SearchResult.xaml
    /// </summary>
    public partial class SearchResult : UserControl, ISearchResult, INotifyPropertyChanged
    {
        public enum SearchProgress
        {
            NotStartedOrCancelled,
            InProgress,
            Completed
        }

        public ObservableList<ProxyInfo> Data
        {
            get;
            set;
        }

        public ObservableList<ProxyInfo> FilteredData
        {
            get;
            set;
        }

        public ObservableList<ProxyInfo> PageData
        {
            get;
            set;
        }

        public ObservableList<FilterData> Countries
        {
            get;
            set;
        }

        public ObservableList<FilterData> Ports
        {
            get;
            set;
        }

        public ObservableList<FilterData> Types
        {
            get;
            set;
        }

        private ExportSettings ExportSettings
        {
            get
            {
                return Context.Get<AllSettings>().ExportSettings;
            }
        }

        public SearchResult()
        {
            Data = new ObservableList<ProxyInfo>();
            FilteredData = new ObservableList<ProxyInfo>();
            PageData = new ObservableList<ProxyInfo>();

            Countries = new ObservableList<FilterData>();
            Ports = new ObservableList<FilterData>();
            Types = new ObservableList<FilterData>();

            Data.CollectionChanged += (sender, e) =>
            {
                UpdateFiltering(Countries, e, proxy => proxy.CountryInfo.Name);
                UpdateFiltering(Ports, e, proxy => proxy.Port);
                UpdateFiltering(Types, e, proxy => proxy.Details.Details.Name);
            };

            Context.Set<ISearchResult>(this);

            InitializeComponent();

            foreach (DataGridHeaderFilteringControl control in FilteringControls)
            {
                control.SelectedData.CollectionChanged += (sender, e) =>
                {
                    UpdateFilteredData();
                    UpdatePageData();
                };
            }

            Context.Get<IProxyClientSearcher>().AllClients.ForEach(client => client.ProxyChanged += () =>
            {
                UpdatePageData();
            });

            SearchState = SearchProgress.NotStartedOrCancelled;
        }

        private DataGridHeaderFilteringControl[] FilteringControls
        {
            get
            {
                return new DataGridHeaderFilteringControl[]
                {
                    filterCountiesHeader,
                    filterPortsHeader,
                    filterTypeHeader
                };
            }
        }

        public void Clear()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                Data.Clear();
                FilteredData.Clear();
                PageData.Clear();
            }));
        }

        public void Add(ProxyInfo proxy)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                using (PreventChangeSortingDirection preventor = new PreventChangeSortingDirection(DataGridControl))
                {
                    Data.Add(proxy);

                    if (!IsProxyFiltered(proxy))
                    {
                        int index = GetInsertIndex(FilteredData, proxy, preventor);

                        FilteredData.Insert(index, proxy);

                        if (FilteredData.Count > 1) //If count is equal to one then PageData was updated already (on page changed event)
                        {
                            int page = (int)Math.Ceiling((double)(index + 1) / Context.Get<AllSettings>().PageSize);

                            if (page <= Paging.Page && PageData.Count == Context.Get<AllSettings>().PageSize)
                            {
                                PageData.Remove(PageData.Last());
                            }

                            if (page < Paging.Page)
                            {
                                PageData.Insert(0, FilteredData[(Paging.Page.Value - 1) * Context.Get<AllSettings>().PageSize]);
                            }
                            else if (page == Paging.Page)
                            {
                                PageData.Insert(index - (page - 1) * Context.Get<AllSettings>().PageSize, proxy);
                            }
                        }
                    }

                    UpdateStatusString();
                }
            }));
        }

        public void UpdatePageData()
        {
            PageData.Clear();

            if (Paging.Page.HasValue)
            {
                PageData.AddRange(FilteredData.Skip((Paging.Page.Value - 1) * Context.Get<AllSettings>().PageSize).
                                               Take(Context.Get<AllSettings>().PageSize));
            }
        }

        private void PageChanged(object sender, RoutedEventArgs e)
        {
            UpdatePageData();
        }

        private bool IsFilteringEnabled
        {
            get
            {
                return FilteringControls.Any(control => control.SelectedData.Any());
            }
        }

        private bool IsProxyFiltered(ProxyInfo proxy)
        {
            return filterCountiesHeader.SelectedData.Any() && !filterCountiesHeader.SelectedData.Contains(proxy.CountryInfo.Name) ||
                   filterPortsHeader.SelectedData.Any() && !filterPortsHeader.SelectedData.Contains(proxy.Port) ||
                   filterTypeHeader.SelectedData.Any() && !filterTypeHeader.SelectedData.Contains(proxy.Details.Details.Name);
        }

        private void UpdateFiltering<TKey>(ObservableList<FilterData> list,
                                           NotifyCollectionChangedEventArgs e,
                                           Func<ProxyInfo, TKey> keySelector) where TKey : IComparable
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (ProxyInfo proxy in e.NewItems)
                    {
                        TKey data = keySelector(proxy);

                        FilterData filterData = new FilterData { Data = data };

                        int index = list.BinarySearch(filterData, new FilterDataComparer());

                        if (index < 0)
                        {
                            index = ~index;
                            filterData.Count = 1;
                            list.Insert(index, filterData);
                        }
                        else
                        {
                            list[index].Count++;
                        }
                    }

                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (ProxyInfo proxy in e.OldItems)
                    {
                        int index = list.BinarySearch(new FilterData { Data = keySelector(proxy) }, new FilterDataComparer());

                        if (index >= 0)
                        {
                            if (list[index].Count == 1)
                                list.Remove(list[index]);
                            else
                                list[index].Count--;
                        }
                    }

                    break;
                case NotifyCollectionChangedAction.Reset:
                    list.Clear();
                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        private int GetInsertIndex(List<ProxyInfo> data, ProxyInfo proxy, PreventChangeSortingDirection preventor)
        {
            if (preventor.HasSorting)
            {
                int index = data.BinarySearch(proxy, new ProxyInfoComparer(preventor.SortMemberPath, preventor.SortDirection));

                if (index < 0)
                {
                    index = ~index;
                }

                if (index >= 0)
                {
                    return index;
                }
            }

            return data.Count;
        }

        private void DataGridControl_Sorting(object sender, DataGridSortingEventArgs e)
        {
            e.Handled = true;

            int? currentPage = Paging.Page;
            ListSortDirection sortDirection = (e.Column.SortDirection == ListSortDirection.Ascending) ?
                          ListSortDirection.Descending :
                          ListSortDirection.Ascending;

            SortFilteredData(e.Column.SortMemberPath, sortDirection);

            UpdatePageData();

            Paging.Page = currentPage;
            e.Column.SortDirection = sortDirection;
        }

        private void SortFilteredData(string sortMemberPath, ListSortDirection sortDirection)
        {
            FilteredData.Sort((proxyInfo1, proxyInfo2) =>
            {
                return new ProxyInfoComparer(sortMemberPath, sortDirection).Compare(proxyInfo1, proxyInfo2);
            });
        }

        private void UpdateFilteredData()
        {
            Dispatcher.Invoke(() =>
            {
                FilteredData.Clear();
                FilteredData.AddRange(Data.Where(proxy => !IsProxyFiltered(proxy)));

                using (PreventChangeSortingDirection preventor = new PreventChangeSortingDirection(DataGridControl))
                {
                    if (preventor.HasSorting)
                    {
                        SortFilteredData(preventor.SortMemberPath, preventor.SortDirection);
                    }
                }

                UpdateStatusString();
            });
        }

        private void UpdateStatusString()
        {
            string formatString = Data.Count == FilteredData.Count ? Properties.Resources.FoundAndShownProxiesFormat : Properties.Resources.FoundProxiesFormat;
            Context.Get<IActionInvoker>().StatusText = string.Format(formatString, Data.Count, FilteredData.Count);
        }

        private void AddToBlackList_Click(object sender, RoutedEventArgs e)
        {
            ProxyInfo proxy = (ProxyInfo)((Button)sender).Tag;

            foreach (IProxyClient client in Context.Get<IProxyClientSearcher>().SelectedClients.Where(item => item.Proxy == proxy))
            {
                client.Proxy = null;
            }

            Context.Get<IBlackListManager>().Add(proxy);
            PageData.Remove(proxy);

            if (Paging.Page < Paging.PageCount)
            {
                int index = Paging.Page.Value * Context.Get<AllSettings>().PageSize;
                PageData.Add(FilteredData[index]);
            }

            if (PageData.Count == 0 && Paging.Page > 1)
            {
                Paging.Page--;
            }

            Data.Remove(proxy);
            FilteredData.Remove(proxy);

            UpdateStatusString();
        }

        private void ProxyUsageChanged(object sender, RoutedEventArgs e)
        {
            ProxyClientControl control = (ProxyClientControl)e.OriginalSource;

            if (control.ProxyInfo != null)
            {
                Context.Get<IUsedProxies>().Add(control.ProxyInfo);
                foreach (ProxyInfo proxy in PageData)
                {
                    proxy.NotifyProxyChanged();
                }
            }
        }

        public void Started()
        {
            SearchState = SearchProgress.InProgress;
        }

        public void Completed()
        {
            SearchState = SearchProgress.Completed;
        }

        public void Cancelled()
        {
            SearchState = SearchProgress.NotStartedOrCancelled;
        }

        private SearchProgress searchState;
        public SearchProgress SearchState
        {
            get
            {
                return searchState;
            }
            set
            {
                searchState = value;
                FirePropertyChanged("SearchState");
            }
        }

        private void FirePropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void CopyDataIntoBuffer(object sender, RoutedEventArgs e)
        {
            string[] values = FilteredData.Select(info=> info.ToString(ExportSettings.ExportCountry, ExportSettings.ExportProxyType))
                                  .ToArray();

            Clipboard.SetDataObject(string.Join(Environment.NewLine, values));
        }
    }
}
