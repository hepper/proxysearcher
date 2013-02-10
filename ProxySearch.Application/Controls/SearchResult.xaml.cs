using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ProxySearch.Common;
using ProxySearch.Console.Code.Interfaces;
using ProxySearch.Console.Code.ProxyClients;
using ProxySearch.Engine;
using System.Linq;
using System;
using ProxySearch.Console.Code.Settings;
using System.Windows.Controls.Primitives;
using System.Threading;

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

        public ObservableCollection<ProxyInfo> Data
        {
            get;
            set;
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

        public void Clear()
        {
            Dispatcher.Invoke(() => 
            {
                Data.Clear();
            });
        }
        
        public void Add(ProxyInfo proxy)
        {
            Dispatcher.Invoke(() =>
            {
                Data.Add(proxy);
                Context.Get<IActionInvoker>().UpdateStatus(string.Format(Properties.Resources.FoundProxiesFormat, Data.Count));
             });
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
    }
}
