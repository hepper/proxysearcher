﻿using System;
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
using ProxySearch.Engine.Proxies;

namespace ProxySearch.Console.Controls
{
    /// <summary>
    /// Interaction logic for SearchResult.xaml
    /// </summary>
    public partial class SearchResult : UserControl, ISearchResult, INotifyPropertyChanged
    {
        private enum RowStyle
        {
            Unused,
            Used,
            Selected
        }

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
                client.PropertyChanged += (sender, e) => UpdateGridStyle(((IProxyClient)sender).Proxy);
            }

            DataGridControl.ItemContainerGenerator.StatusChanged += ItemContainerGenerator_StatusChanged;
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

                DataGridControl.UpdateLayout();
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

        private void ItemContainerGenerator_StatusChanged(object sender, EventArgs e)
        {
            if (DataGridControl.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
            {
                UpdateGridStyle(null);
            }
        }

        private void UpdateGridStyle(ProxyInfo proxy)
        {
            for (int i = 0; i < PageData.Count; i++)
            {
                if (proxy == PageData[i])
                {
                    Context.Get<UsedProxies>().Add(PageData[i]);
                }

                RowStyle style = GetRowStyle(PageData[i]);
                ApplyStyle(PageData[i], style);
            }
        }

        private RowStyle GetRowStyle(ProxyInfo currentProxyInfo)
        {
            if (Context.Get<IProxyClientSearcher>().Clients.Any(item => item.Proxy == currentProxyInfo))
            {
                return RowStyle.Selected;
            }

            if (Context.Get<UsedProxies>().Contains(currentProxyInfo))
            {
                return RowStyle.Used;
            }

            return RowStyle.Unused;
        }

        private void ApplyStyle(ProxyInfo proxyInfo, RowStyle style)
        {
            Brush brush = GetStyleBrush(style);

            try
            {
                for (int j = 0; j < 4; j++)
                {
                    ContentPresenter contentPresenter = (ContentPresenter)DataGridControl.Columns[j].GetCellContent(proxyInfo);

                    if (contentPresenter != null)
                    {
                        TextBox textBox = (TextBox)contentPresenter.ContentTemplate.FindName("textBox", contentPresenter);
                        textBox.Foreground = brush;
                    }
                }
            }
            catch (InvalidOperationException)
            {
            }
        }

        private static Brush GetStyleBrush(RowStyle style)
        {
            switch (style)
            {
                case RowStyle.Unused:
                    return Brushes.Black;
                case RowStyle.Used:
                    return Brushes.Gray;
                case RowStyle.Selected:
                    return Brushes.Blue;
                default:
                    throw new InvalidOperationException(string.Format(Properties.Resources.RowStyleIsNotSupported, style));
            }
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
