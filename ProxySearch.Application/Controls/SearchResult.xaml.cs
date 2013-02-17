using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using ProxySearch.Common;
using ProxySearch.Console.Code.Interfaces;
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

        public ObservableCollection<ProxyInfo> Data
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
            Data = new ObservableCollection<ProxyInfo>();
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
            FirePageDataChanged();
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
                Data.Clear();
            }));
        }

        public void Add(ProxyInfo proxy)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                Data.Add(proxy);
                Context.Get<IActionInvoker>().UpdateStatus(string.Format(Properties.Resources.FoundProxiesFormat, Data.Count));

                if (!Paging.Page.HasValue || (Paging.Page == Paging.PageCount && Data.Count % Context.Get<AllSettings>().PageSize != 0))
                {
                    FirePageDataChanged();
                }
            }));
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

            ListSortDirection sortDirection = (e.Column.SortDirection == ListSortDirection.Ascending) ?
                          ListSortDirection.Descending :
                          ListSortDirection.Ascending;

            List<ProxyInfo> data = Data.ToList();
            data.Sort(GetComparer(e.Column.SortMemberPath, sortDirection));
            Data.Clear();
            data.ForEach(item => Data.Add(item));
            FirePageDataChanged();

            e.Column.SortDirection = sortDirection;
        }

        private Comparison<ProxyInfo> GetComparer(string sortMemberPath, ListSortDirection sortDirection)
        {
            return (proxyInfo1, proxyInfo2) =>
            {
                IComparable object1 = (IComparable)GetPropertyValue(proxyInfo1, sortMemberPath);
                IComparable object2 = (IComparable)GetPropertyValue(proxyInfo2, sortMemberPath);

                if (sortDirection == ListSortDirection.Descending)
                {
                    return object2.CompareTo(object1);
                }

                return object1.CompareTo(object2);
            };
        }

        private object GetPropertyValue(object source, string path)
        {
            object propValue = source;
            foreach (string propName in path.Split('.'))
            {
                PropertyInfo propInfo = propValue.GetType().GetProperty(propName);
                propValue = propInfo.GetValue(propValue, null);
            }

            return propValue;
        }
    }
}
