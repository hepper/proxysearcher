using System.Threading.Tasks;
using ProxySearch.Engine.Proxies;

namespace ProxySearch.Engine.Ratings
{
    public class DisabledRatingManager : IRatingManager
    {
        public Task<RatingData> GetRatingDataAsync(Proxy proxy)
        {
            return Task.FromResult<RatingData>(new RatingData());
        }

        public Task<RatingData> UpdateRatingDataAsync(Proxy proxy, int? ratingValue)
        {
            return Task.FromResult<RatingData>(new RatingData(RatingState.Disabled, null));
        }
    }
}