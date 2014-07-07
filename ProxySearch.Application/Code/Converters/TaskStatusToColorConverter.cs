using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using ProxySearch.Engine.Tasks;

namespace ProxySearch.Console.Code.Converters
{
    public class TaskStatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TaskStatus status = (TaskStatus)value;

            switch (status)
            {
                case TaskStatus.Important:
                    return new BrushConverter().ConvertFrom("#115511");
                case TaskStatus.MostImportant:
                    return new BrushConverter().ConvertFrom("#00AA00");
                case TaskStatus.Normal:
                default:
                    return new BrushConverter().ConvertFrom("#555555");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
