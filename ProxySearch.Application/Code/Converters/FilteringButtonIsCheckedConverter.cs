using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using ProxySearch.Console.Code.Collections;

namespace ProxySearch.Console.Code.Converters
{
    public class FilteringButtonIsCheckedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ObservableList<IComparable> checkedList = value as ObservableList<IComparable>;

            if (checkedList == null)
                return false;

            return checkedList.Any();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
