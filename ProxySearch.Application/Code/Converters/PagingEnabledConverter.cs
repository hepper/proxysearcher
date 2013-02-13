using System;
using System.Globalization;
using System.Windows.Data;
using ProxySearch.Common;
using ProxySearch.Console.Code.Settings;

namespace ProxySearch.Console.Code.Converters
{
    public class PagingEnabledConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int? count = (int?)value;

            if (!Context.IsSet<AllSettings>() || !count.HasValue)
                return false;

            return count.Value > Context.Get<AllSettings>().PageSize;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
