using System;
using System.Windows.Data;

namespace ProxySearch.Console.Code.Converters
{
    public class AccuracyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double doubleValue = (double)value;

            return Math.Round((double)value * 100).ToString() + "%";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
