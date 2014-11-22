using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using ProxySearch.Common;
using ProxySearch.Engine.DownloaderContainers;
using ProxySearch.Engine.Properties;
using ProxySearch.Engine.Proxies;
using ProxySearch.Engine.Proxies.Socks;
using ProxySearch.Engine.Socks;

namespace ProxySearch.Engine.ProxyDetailsProvider
{
    public class SocksProxyDetailsProvider : ProxyDetailsProviderBase
    {
        public override async Task<ProxyTypeDetails> GetProxyDetails(Proxy proxy, CancellationTokenSource cancellationToken)
        {
            string proxyUriString = GetProxyUriString(proxy);

            ISocksProxyTypeHashtable hashtable = Context.Get<ISocksProxyTypeHashtable>();

            var httpDownloaderContainer = new HttpDownloaderContainer<SocksHttpClientHandler, SocksProgressMessageHandler>();

            string content = await httpDownloaderContainer.HttpDownloader.GetContentOrNull(GetProxyTypeDetectorUrl(proxy,
                                                                                                                   Resources.SocksProxyType),
                                                                                           proxy,
                                                                                           cancellationToken);
            if (content == null)
                return new SocksProxyDetails(hashtable[proxyUriString], null);

            string[] values = content.Split(',');
            IPAddress outgoingIPAddress;

            if (values.Length != 2 || !IPAddress.TryParse(values[1], out outgoingIPAddress))
                return new SocksProxyDetails(SocksProxyTypes.ChangesContent, null);

            return new SocksProxyDetails(hashtable[proxyUriString], outgoingIPAddress);
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