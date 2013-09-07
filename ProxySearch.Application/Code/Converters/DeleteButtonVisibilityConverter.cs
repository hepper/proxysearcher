using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using ProxySearch.Common;
using ProxySearch.Console.Code.Settings;
using ProxySearch.Engine.Parser;

namespace ProxySearch.Console.Code.Converters
{
    public class DeleteButtonVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ParseDetails details = value as ParseDetails;

            return value == null || string.IsNullOrEmpty(details.Url)? Visibility.Hidden : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
