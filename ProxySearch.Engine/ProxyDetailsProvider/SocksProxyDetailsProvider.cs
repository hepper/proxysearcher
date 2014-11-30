using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using ProxySearch.Engine.DownloaderContainers;
using ProxySearch.Engine.Properties;
using ProxySearch.Engine.Proxies;
using ProxySearch.Engine.Proxies.Socks;
using ProxySearch.Engine.Socks;
using ProxySearch.Engine.Tasks;

namespace ProxySearch.Engine.ProxyDetailsProvider
{
    public class SocksProxyDetailsProvider : ProxyDetailsProviderBase
    {
        public override async Task<ProxyTypeDetails> GetProxyDetails(Proxy proxy, TaskItem task, CancellationTokenSource cancellationToken)
        {
            string proxyUriString = GetProxyUriString(proxy);

            var httpDownloaderContainer = new HttpDownloaderContainer<SocksHttpClientHandler, SocksProgressMessageHandler>();

            string content = await httpDownloaderContainer.HttpDownloader.GetContentOrNull(GetProxyTypeDetectorUrl(proxy,
                                                                                                                   Resources.SocksProxyType),
                                                                                           proxy,
                                                                                           cancellationToken);
            if (content == null)
                return new SocksProxyDetails(Application.SocksProxyHashTable[proxyUriString], null);

            string[] values = content.Split(',');
            IPAddress outgoingIPAddress;

            if (values.Length != 2 || !IPAddress.TryParse(values[1], out outgoingIPAddress))
                return new SocksProxyDetails(SocksProxyTypes.ChangesContent, null);

            return new SocksProxyDetails(Application.SocksProxyHashTable[proxyUriString], outgoingIPAddress);
        }

        public override ProxyTypeDetails GetUncheckedProxyDetails()
        {
            return new SocksProxyDetails(SocksProxyTypes.Unchecked, null);
        }

        private static string GetProxyUriString(Proxy proxy)
        {
            UriBuilder uriBuilder = new UriBuilder();
            uriBuilder.Host = proxy.Address.ToString();
            uriBuilder.Port = proxy.Port == 80 ? -1 : proxy.Port;

            return uriBuilder.ToString();
        }
    }
}