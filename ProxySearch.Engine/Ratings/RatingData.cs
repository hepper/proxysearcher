using System;
namespace ProxySearch.Engine.Ratings
{
    public class RatingData : IComparable
    {
        public static readonly RatingData Error = new RatingData(RatingState.Error, new Rating { Value = 0.0, Amount = 0 });

        public RatingData()
            : this(RatingState.Ready, null)
        {
        }

        public RatingData(RatingState state, Rating rating)
        {
            State = state;
            Rating = rating;
        }

        public RatingState State { get; set; }

        public Rating Rating { get; set; }

        public int CompareTo(object obj)
        {
            RatingData data = obj as RatingData;

            if (data == null)
                return 1;

            if (State != data.State)
                return (int)State - (int)data.State;

            switch (State)
            {
                case RatingState.Updated:
                case RatingState.Ready:
                    return (int)(100 * (data.Rating.Value - Rating.Value));
                case RatingState.Error:
                case RatingState.Updating:
                case RatingState.Disabled:
                    return 0;
                default:
                    throw new NotSupportedException();
            }
        }
    }
}