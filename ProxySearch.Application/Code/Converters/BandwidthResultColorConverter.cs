using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using ProxySearch.Common;
using ProxySearch.Console.Code.Settings;
using ProxySearch.Engine.Bandwidth;

namespace ProxySearch.Console.Code.Converters
{
    public class BandwidthResultColorConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (!Context.IsSet<AllSettings>())
                return string.Empty;

            double maxBandwidth = Context.Get<AllSettings>().MaxBandwidth;
            BandwidthState? state = values[0] as BandwidthState?;
            double? bandwidth = values[1] as double?;

            if (!state.HasValue || !bandwidth.HasValue)
                return string.Empty;

            if (state == BandwidthState.Error)
                return new SolidColorBrush(Color.FromArgb(128, 0xFF, 0, 0));

            if (bandwidth > maxBandwidth)
                bandwidth = maxBandwidth;

            byte color = (byte)((255 * bandwidth.Value) / maxBandwidth);

            return new SolidColorBrush(Color.FromArgb(128, (byte)(255 - color), color, 0));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
