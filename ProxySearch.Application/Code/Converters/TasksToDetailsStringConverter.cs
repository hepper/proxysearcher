using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using ProxySearch.Engine.Tasks;

namespace ProxySearch.Console.Code.Converters
{
    public class TasksToDetailsStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IEnumerable<TaskData> tasks = value as IEnumerable<TaskData>;

            if (tasks == null)
                return null;

            string result = string.Join(Environment.NewLine, tasks.Where(task => task.Details != null)
                                                   .Select(task => task.Details));

            return string.IsNullOrWhiteSpace(result) ? null : result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
