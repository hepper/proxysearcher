using System;
using System.Globalization;
using System.Windows.Data;
using ProxySearch.Console.Code.UI;

namespace ProxySearch.Console.Code.Converters
{
    public class GripRotationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ResizeDirection resizeDirection = (ResizeDirection)value;

            switch (resizeDirection)
            {
                case ResizeDirection.BottomLeft:
                    return 90;
                case ResizeDirection.TopLeft:
                    return 180;
                case ResizeDirection.TopRight:
                    return 270;
                default: 
                    return 0;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
