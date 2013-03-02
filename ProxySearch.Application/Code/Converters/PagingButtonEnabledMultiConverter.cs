using System;
using System.Globalization;
using System.Windows.Data;
using ProxySearch.Common;
using ProxySearch.Console.Code.Settings;

namespace ProxySearch.Console.Code.Converters
{
    public class PagingButtonEnabledMultiConverter : IMultiValueConverter
    {
        private enum ButtonType
        {
            Top,            
            Left,
            Right,
            Bottom
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            int? count = values[0] as int?;
            int? page = values[1] as int?;

            if (!count.HasValue || !page.HasValue || count == 0)
                return false;

            int pageCount = (int)Math.Ceiling((double)count / Context.Get<AllSettings>().PageSize);
            ButtonType type = (ButtonType) Enum.Parse(typeof(ButtonType), (string)parameter);

            switch (type)
            {
                case ButtonType.Top:
                case ButtonType.Left:
                    return page.Value > 1;
                case ButtonType.Right:
                case ButtonType.Bottom:
                    return page.Value < pageCount;
                default:
                    throw new NotSupportedException();
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
