using System;
using System.Net;
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
                return new HttpProxyDetails(HttpProxyTypes.CannotVerify, null);

            string[] values = result.Split(',');

            HttpProxyTypes proxyType;
            IPAddress outgoingIPAddress;

            if (values.Length != 2 || !Enum.TryParse(values[0], out proxyType) || !IPAddress.TryParse(values[1], out outgoingIPAddress))
                return new HttpProxyDetails(HttpProxyTypes.ChangesContent, null);

            return new HttpProxyDetails(proxyType, outgoingIPAddress);
        }

        public override ProxyTypeDetails GetUncheckedProxyDetails()
        {
            return new HttpProxyDetails(HttpProxyTypes.Unchecked, null);
        }
    }
}
