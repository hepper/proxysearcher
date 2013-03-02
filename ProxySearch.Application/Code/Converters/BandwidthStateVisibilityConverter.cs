using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using ProxySearch.Engine.Bandwidth;

namespace ProxySearch.Console.Code.Converters
{
    public class BandwidthStateVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            BandwidthState state = (BandwidthState)value;
            int type = int.Parse((string)parameter);

            switch (state)
            {
                case BandwidthState.Ready:
                    return ToVisibility(type == 1);
                case BandwidthState.Progress:
                    return ToVisibility(type == 2);
                case BandwidthState.Completed:
                case BandwidthState.Error:
                    return ToVisibility(type == 3);
            }

            throw new NotSupportedException();
        }

        private Visibility ToVisibility(bool isVisible)
        {
            return isVisible ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
