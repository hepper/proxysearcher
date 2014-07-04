using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace ProxySearch.Console.Code.Converters
{
    public class AndMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Any(value => !(value is bool)))
                return false;

            return values.Cast<bool>().All(value => value);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
