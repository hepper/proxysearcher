using System;
using System.Globalization;
using System.Windows.Data;
using ProxySearch.Engine.Proxies.Http;

namespace ProxySearch.Console.Code.Converters
{
    public class HttpProxyTypeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return HttpProxyDetails.GetName((HttpProxyTypes)value);           
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
