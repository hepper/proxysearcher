using System;
using System.Globalization;
using System.Windows.Data;
using ProxySearch.Engine.Ratings;

namespace ProxySearch.Console.Code.Converters
{
    public class RatingValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            RatingData ratingData = value as RatingData;

            if (ratingData == null || ratingData.State != RatingState.Ready && ratingData.State != RatingState.Updated)
                return 0;

            return (int)ratingData.Rating.Value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}