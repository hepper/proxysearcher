using System;
using System.Globalization;
using System.Net;
using System.Windows.Data;
using ProxySearch.Console.Properties;

namespace ProxySearch.Console.Code.Converters
{
    public class CountryDescriptionMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string countryName = values[0] as string;
            IPAddress outgoingIPAddress = values[1] as IPAddress;
            IPAddress ipAddress = values[2] as IPAddress;

            if (countryName == null || ipAddress == null)
                return null;

            if (outgoingIPAddress != null && outgoingIPAddress.ToString() != ipAddress.ToString())
            {
                return string.Format(Resources.NameOfCountryWasDeterminedBasedOnOutgoingAddress, outgoingIPAddress, ipAddress);
            }

            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
