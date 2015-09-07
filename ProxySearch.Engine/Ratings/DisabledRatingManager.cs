using System.Threading.Tasks;
using ProxySearch.Engine.Proxies;

namespace ProxySearch.Engine.Ratings
{
    public class DisabledRatingManager : IRatingManager
    {
        public Task<RatingData> GetRatingData(Proxy proxy)
        {
            return Task.FromResult<RatingData>(new RatingData());
        }

        public Task<RatingData> UpdateRatingData(Proxy proxy, int? ratingValue)
        {
            return Task.FromResult<RatingData>(new RatingData(RatingState.Disabled, null));
        }
    }
}