using System.Threading.Tasks;
using ProxySearch.Engine.Proxies;

namespace ProxySearch.Engine.Ratings
{
    public interface IRatingManager
    {
        Task<RatingData> GetRatingDataAsync(Proxy proxy);
        Task<RatingData> UpdateRatingDataAsync(Proxy proxy, int? ratingValue);
    }
}