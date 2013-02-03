using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Data;

namespace ProxySearch.Console.Code.Converters
{
    public class ValueToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int colorValue = (int)Math.Round((int)value * 2.55);
            return string.Format("#{0:x2}{1:x2}{2:x2}", colorValue, 255 - colorValue, 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
