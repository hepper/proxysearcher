using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ProxySearch.Console.Code.Converters
{
    public class SizeToAngleConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double actualWidth = (double)values[0];
            double actualHeight = (double)values[1];

            return 270.0 + RadianToDegree(Math.Atan(actualWidth / actualHeight));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        private double RadianToDegree(double angle)
        {
            return angle * (180.0 / Math.PI);
        }
    }
}
