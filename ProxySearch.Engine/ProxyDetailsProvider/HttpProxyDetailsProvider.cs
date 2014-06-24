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
    public class HttpProxyDetailsProvider : ProxyDetailsProviderBase
    {
        public override async Task<ProxyTypeDetails> GetProxyDetails(Proxy proxy, CancellationTokenSource cancellationToken)
        {
            string result = await Context.Get<IHttpDownloaderContainer>().HttpDownloader.GetContentOrNull(GetProxyTypeDetectorUrl(proxy, Resources.HttpProxyType),
                                                                                                          proxy,
                                                                                                          cancellationToken);

            if (result == null)
                return new HttpProxyDetails(HttpProxyTypes.CannotVerify);

            HttpProxyTypes proxyType;

            if (!Enum.TryParse(result, out proxyType))
                return new HttpProxyDetails(HttpProxyTypes.ChangesContent);

            return new HttpProxyDetails(proxyType);
        }

        public override ProxyTypeDetails GetUncheckedProxyDetails()
        {
            return new HttpProxyDetails(HttpProxyTypes.Unchecked);
        }
    }
}
