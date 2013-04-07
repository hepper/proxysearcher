using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using ProxySearch.Common;
using ProxySearch.Console.Code.Collections;
using ProxySearch.Console.Code.Interfaces;
using ProxySearch.Console.Code.SearchResult;
using ProxySearch.Console.Code.Settings;
using ProxySearch.Console.Code.UI;
using ProxySearch.Engine.Proxies;

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

        public ObservableList<ProxyInfo> PageData
        {
            get;
            set;
        }

        public SearchResult()
        {
            Data = new ObservableList<ProxyInfo>();
            PageData = new ObservableList<ProxyInfo>();
            Context.Set<ISearchResult>(this);

            InitializeComponent();

            foreach (IProxyClient client in Context.Get<IProxyClientSearcher>().Clients)
            {
                IProxyClient clientCopy = client;
                clientCopy.PropertyChanged += (sender, e) =>
                {
                    if (clientCopy.Proxy != null)
                    {
                        Context.Get<IUsedProxies>().Add(clientCopy.Proxy);
                    }

                    PageData.Reset();
                };
            }

            SearchState = SearchProgress.NotStartedOrCancelled;
        }

        private void PageChanged(object sender, RoutedEventArgs e)
        {
            using (new PreventChangeSortingDirection(DataGridControl))
            {
                UpdatePageData();
            }
        }

        public void Clear()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                using (new PreventChangeSortingDirection(DataGridControl))
                {
                    Data.Clear();
                    PageData.Clear();
                }
            }));
        }

        public void Add(ProxyInfo proxy)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                using (PreventChangeSortingDirection preventor = new PreventChangeSortingDirection(DataGridControl))
                {
                    int index = GetInsertIndex(Data, proxy, preventor);
                    Data.Insert(index, proxy);

                    if (Data.Count > 1)
                    {
                        int page = (int)Math.Ceiling((double)(index + 1) / Context.Get<AllSettings>().PageSize);

                        if (page <= Paging.Page && PageData.Count == Context.Get<AllSettings>().PageSize)
                        {
                            PageData.Remove(PageData.Last());
                        }

                        if (page < Paging.Page)
                        {
                            PageData.Insert(0, Data[(Paging.Page.Value - 1) * Context.Get<AllSettings>().PageSize]);
                        }
                        else if (page == Paging.Page)
                        {
                            PageData.Insert(index - (page - 1) * Context.Get<AllSettings>().PageSize, proxy);
                        }
                    }

                    Context.Get<IActionInvoker>().UpdateStatus(string.Format(Properties.Resources.FoundProxiesFormat, Data.Count));
                }
            }));
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

            Data.Sort((proxyInfo1, proxyInfo2) =>
            {
                return new ProxyInfoComparer(e.Column.SortMemberPath, sortDirection).Compare(proxyInfo1, proxyInfo2);
            });

            UpdatePageData();

            Paging.Page = currentPage;
            e.Column.SortDirection = sortDirection;
        }

        private void UpdatePageData()
        {
            PageData.Clear();

            if (Paging.Page.HasValue)
            {
                PageData.AddRange(Data.Skip((Paging.Page.Value - 1) * Context.Get<AllSettings>().PageSize).Take(Context.Get<AllSettings>().PageSize));
            }
        }

        private void AddToBlackList_Click(object sender, RoutedEventArgs e)
        {
            ProxyInfo proxy = (ProxyInfo)((Button)sender).Tag;

            foreach (IProxyClient client in Context.Get<IProxyClientSearcher>().Clients.Where(item => item.Proxy == proxy))
            {
                client.Proxy = null;
            }
            
            Context.Get<IBlackListManager>().Add(proxy);
            PageData.Remove(proxy);

            if (Paging.Page < Paging.PageCount)
            {
                int index = Paging.Page.Value * Context.Get<AllSettings>().PageSize;
                PageData.Add(Data[index]);
            }

            if (PageData.Count == 0 && Paging.Page > 1)
            {
                Paging.Page--;
            }

            Data.Remove(proxy);
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
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SearchState"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
