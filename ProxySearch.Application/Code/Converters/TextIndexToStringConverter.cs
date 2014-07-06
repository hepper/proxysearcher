using System;
using System.Globalization;
using System.Windows.Data;

namespace ProxySearch.Console.Code.Converters
{
    public class TextIndexToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Format("{0} ({1})", parameter, value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
