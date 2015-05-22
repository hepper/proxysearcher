using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ProxySearch.Console.Code.Converters
{
    public class PageCountMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)(DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue))
            {
                return string.Empty;
            }

            int? count = (int?)values[0];
            int? pageSize = (int?)values[1];

            if (!count.HasValue || !pageSize.HasValue || count == 0 || pageSize == 0)
                return string.Empty;

            return Math.Ceiling((decimal)count.Value / pageSize.Value).ToString();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
