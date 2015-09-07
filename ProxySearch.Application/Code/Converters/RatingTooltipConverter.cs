using System;
using System.Globalization;
using System.Windows.Data;
using ProxySearch.Console.Properties;
using ProxySearch.Engine.Ratings;

namespace ProxySearch.Console.Code.Converters
{
    public class RatingTooltipConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            RatingData ratingData = value as RatingData;

            if (ratingData == null)
                return string.Empty;

            switch (ratingData.State)
            {
                case RatingState.Ready:
                case RatingState.Updated:
                    return string.Format(Resources.RatingVotesFormat, ratingData.Rating.Value, ratingData.Rating.Amount);
                case RatingState.Updating:
                    return Resources.UpdatingRating;
                case RatingState.Error:
                    return Resources.CannotRateThisProxy;
                default:
                    return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}