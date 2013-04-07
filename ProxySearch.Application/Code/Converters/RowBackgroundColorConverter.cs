using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using ProxySearch.Common;
using ProxySearch.Console.Code.Interfaces;
using ProxySearch.Console.Code.Settings;
using ProxySearch.Console.Code.UI;
using ProxySearch.Engine.Proxies;

namespace ProxySearch.Console.Code.Converters
{
    public class RowBackgroundColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ProxyInfo proxy = (ProxyInfo)value;

            if (Context.Get<IProxyClientSearcher>().Clients.Any(item => item.Proxy == proxy))
            {
                return RowStyles.Selected;
            }

            if (Context.Get<IUsedProxies>().Contains(proxy))
            {
                return RowStyles.Used;
            }

            return RowStyles.Unused;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
