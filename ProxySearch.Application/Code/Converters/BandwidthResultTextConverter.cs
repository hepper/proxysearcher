using System;
using System.Globalization;
using System.Windows.Data;
using ProxySearch.Common;
using ProxySearch.Console.Code.Settings;
using ProxySearch.Console.Properties;
using ProxySearch.Engine.Bandwidth;

namespace ProxySearch.Console.Code.Converters
{
    public class BandwidthResultTextConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (!Context.IsSet<AllSettings>())
                return string.Empty;

            double maxBandwidth = Context.Get<AllSettings>().MaxBandwidth;
            BandwidthState? state = values[0] as BandwidthState?;
            double? bandwidth = values[1] as double?;
            double? responseTime = values[2] as double?;

            if (!state.HasValue || !bandwidth.HasValue || !responseTime.HasValue)
                return string.Empty;

            if (state == BandwidthState.Error)
                return Resources.Error;

            return string.Format(Resources.SpeedRespondTextFormat, bandwidth, responseTime);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
