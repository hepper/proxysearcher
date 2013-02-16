using System;
using System.Globalization;
using System.Windows.Data;
using ProxySearch.Common;
using ProxySearch.Console.Code.Settings;

namespace ProxySearch.Console.Code.Converters
{
    public class PageCountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int? count = (int?)value;

            if (!count.HasValue)
                return 0;

            if (!Context.IsSet<AllSettings>() || count.Value == 0)
            {
                return string.Empty;
            }

            return Math.Ceiling((decimal)count.Value / Context.Get<AllSettings>().PageSize);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
