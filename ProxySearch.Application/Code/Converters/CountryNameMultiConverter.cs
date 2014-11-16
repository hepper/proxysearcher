using System;
using System.Globalization;
using System.Net;
using System.Windows.Data;

namespace ProxySearch.Console.Code.Converters
{
    public class CountryNameMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string countryName = values[0] as string;
            IPAddress outgoingIPAddress = values[1] as IPAddress;
            IPAddress ipAddress = values[2] as IPAddress;

            if (countryName == null || ipAddress == null)
                return string.Empty;

            if (outgoingIPAddress != null && outgoingIPAddress.ToString() != ipAddress.ToString())
            {
                return string.Format("{0} ({1})", countryName, outgoingIPAddress);
            }

            return countryName;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
