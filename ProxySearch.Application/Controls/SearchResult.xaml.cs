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
using ProxySearch.Engine;

namespace ProxySearch.Console.Controls
{
    /// <summary>
    /// Interaction logic for SearchResult.xaml
    /// </summary>
    public partial class SearchResult : UserControl, ISearchResult
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
                            PageData.Insert(0, Data[(Paging.Page.Value -1)* Context.Get<AllSettings>().PageSize]);
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

        private int GetInsertIndex(ObservableList<ProxyInfo> data, ProxyInfo proxy, PreventChangeSortingDirection preventor)
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
    }
}
