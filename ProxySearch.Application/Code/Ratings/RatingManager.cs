using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using ProxySearch.Console.Properties;
using ProxySearch.Engine.DownloaderContainers;
using ProxySearch.Engine.Proxies;
using ProxySearch.Engine.Ratings;

namespace ProxySearch.Console.Code.Ratings
{
    public class RatingManager : IRatingManager
    {
        public async Task<RatingData> GetRatingData(Proxy proxy)
        {
            string content = await HttpDownloader.GetContentOrNull(string.Format(Resources.GetProxyRatingUrlFormat, proxy.Address, proxy.Port), null);

            if (content == null)
                return RatingData.Error;

            return new RatingData(RatingState.Ready, GetRatingOrDefault(content));
        }

        public async Task<RatingData> UpdateRatingData(Proxy proxy, int? ratingValue)
        {
            string content = await HttpDownloader.GetContentOrNull(string.Format(Resources.UpdateProxyRatingUrlFormat, proxy.Address, proxy.Port, ratingValue ?? 0), null);

            if (content == null)
                return RatingData.Error;

            return new RatingData(RatingState.Updated, GetRatingOrDefault(content));
        }

        private Rating GetRatingOrDefault(string content)
        {
            string[] pairs = content.Trim().Split('&');

            double value = 0;
            int amount = 0;

            foreach (string pair in pairs)
            {
                string[] nameValue = pair.Split('=');

                if (nameValue.Length == 2)
                {
                    switch (nameValue[0])
                    {
                        case "UserRating":
                            double.TryParse(nameValue[1], NumberStyles.Any, CultureInfo.InvariantCulture, out value);
                            break;
                        case "UserCount":
                            int.TryParse(nameValue[1], out amount);
                            break;
                    }
                }
            }

            return new Rating { Value = value, Amount = amount };
        }

        private IHttpDownloader HttpDownloader
        {
            get
            {
                return new HttpDownloader<HttpClientHandler>();
            }
        }
    }
}