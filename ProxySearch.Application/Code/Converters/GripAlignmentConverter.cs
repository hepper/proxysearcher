using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using ProxySearch.Console.Code.UI;

namespace ProxySearch.Console.Code.Converters
{
    public class GripAlignmentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Orientation orientation = (Orientation)parameter;
            ResizeDirection resizeDirection = (ResizeDirection)value;

            switch (orientation)
            {
                case Orientation.Horizontal:
                    if (resizeDirection == ResizeDirection.TopRight || resizeDirection == ResizeDirection.BottomRight)
                        return HorizontalAlignment.Right;

                    return HorizontalAlignment.Left;
                case Orientation.Vertical:
                    if (resizeDirection == ResizeDirection.TopRight || resizeDirection == ResizeDirection.TopLeft)
                        return VerticalAlignment.Top;

                    return VerticalAlignment.Bottom;
                default:
                    return DependencyProperty.UnsetValue;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
