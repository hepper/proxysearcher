using System.Threading.Tasks;
using ProxySearch.Engine.Proxies;

namespace ProxySearch.Engine.Ratings
{
    public interface IRatingManager
    {
        Task<RatingData> GetRatingData(Proxy proxy);
        Task<RatingData> UpdateRatingData(Proxy proxy, int? ratingValue);
    }
}