using System;
using System.Globalization;
using System.Windows.Data;
using ProxySearch.Engine.Tasks;

namespace ProxySearch.Console.Code.Converters
{
    public class FilteringPopupIsCheckedMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            IComparable value = values[0] as IComparable;
            ObservableList<IComparable> checkedList = values[1] as ObservableList<IComparable>;
 
            if (value == null || checkedList == null)
                return false;

            return checkedList.Contains(value);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
