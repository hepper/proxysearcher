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
using ProxySearch.Engine;

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

        public ObservableList<ProxyInfo> Data
        {
            get;
            set;
        }

        public IEnumerable<ProxyInfo> PageData
        {
            get
            {
                if (Paging == null || !Paging.Page.HasValue)
                {
                    return Data;
                }

                return Data.Skip((Paging.Page.Value - 1) * Context.Get<AllSettings>().PageSize).Take(Context.Get<AllSettings>().PageSize);
            }
        }

        public SearchResult()
        {
            Data = new ObservableList<ProxyInfo>();
            Context.Set<ISearchResult>(this);

            InitializeComponent();

            foreach (IProxyClient client in Context.Get<IProxyClientSearcher>().Clients)
            {
                client.PropertyChanged += (sender, e) => UpdateGridStyle(((IProxyClient)sender).Proxy);
            }

            DataGridControl.ItemContainerGenerator.StatusChanged += ItemContainerGenerator_StatusChanged;
        }

        private void PageChanged(object sender, RoutedEventArgs e)
        {
            using (new PreventChangeSortingDirection(DataGridControl))
            {
                FirePageDataChanged();
            }
        }

        private void FirePageDataChanged()
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("PageData"));
            }
        }

        public void Clear()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                using (new PreventChangeSortingDirection(DataGridControl))
                {
                    Data.Clear();
                }
            }));
        }

        public void Add(ProxyInfo proxy)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                using (PreventChangeSortingDirection preventor = new PreventChangeSortingDirection(DataGridControl))
                {
                    Data.Insert(GetInsertIndex(proxy, preventor), proxy);

                    if (!Paging.Page.HasValue || (Paging.Page == Paging.PageCount && Data.Count % Context.Get<AllSettings>().PageSize != 0))
                    {
                        FirePageDataChanged();
                    }

                    Context.Get<IActionInvoker>().UpdateStatus(string.Format(Properties.Resources.FoundProxiesFormat, Data.Count));
                }
            }));
        }

        private int GetInsertIndex(ProxyInfo proxy, PreventChangeSortingDirection preventor)
        {
            if (preventor.HasSorting)
            {
                int index = Data.BinarySearch(proxy, new ProxyInfoComparer(preventor.SortMemberPath, preventor.SortDirection));

                if (index < 0)
                {
                    index = ~index;
                }

                if (index >= 0)
                {
                    return index;
                }
            }

            return Data.Count;
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
            for (int i = 0; i < Data.Count; i++)
            {
                if (proxy == Data[i])
                {
                    Context.Get<UsedProxies>().Add(Data[i]);
                }

                RowStyle style = GetRowStyle(Data[i]);
                ApplyStyle(Data[i], style);
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
            Brush brush = null;

            switch (style)
            {
                case RowStyle.Unused:
                    brush = Brushes.Black;
                    break;
                case RowStyle.Used:
                    brush = Brushes.Gray;
                    break;
                case RowStyle.Selected:
                    brush = Brushes.Blue;
                    break;
                default:
                    throw new InvalidOperationException(string.Format(Properties.Resources.RowStyleIsNotSupported, style));
            }

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

        public event PropertyChangedEventHandler PropertyChanged;

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

            FirePageDataChanged();
            Paging.Page = currentPage;
            e.Column.SortDirection = sortDirection;
        }
    }
}
