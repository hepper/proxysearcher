using System;
using System.Threading;
using System.Threading.Tasks;
using ProxySearch.Common;
using ProxySearch.Engine.DownloaderContainers;
using ProxySearch.Engine.Properties;
using ProxySearch.Engine.Proxies;
using ProxySearch.Engine.Proxies.Http;

namespace ProxySearch.Engine.ProxyDetailsProvider
{
    public class HttpProxyDetailsProvider : IProxyDetailsProvider
    {
        public async Task<ProxyTypeDetails> GetProxyDetails(Proxy proxy, CancellationTokenSource cancellationToken)
        {
            string result = await Context.Get<IHttpDownloaderContainer>().HttpDownloader.GetContentOrNull(ProxyTypeDetectorUrl, proxy, cancellationToken);

            if (result == null)
                return new HttpProxyDetails(HttpProxyTypes.CannotVerify);

            HttpProxyTypes proxyType;

            if (!Enum.TryParse(result, out proxyType))
                return new HttpProxyDetails(HttpProxyTypes.ChangesContent);

            return new HttpProxyDetails(proxyType);
        }

        public ProxyTypeDetails GetUncheckedProxyDetails()
        {
            return new HttpProxyDetails(HttpProxyTypes.Unchecked);
        }

        private string ProxyTypeDetectorUrl
        {
            get
            {
                return string.Format(Resources.ProxyTypeDetectorUrl, Guid.NewGuid());
            }
        }
    }
}
