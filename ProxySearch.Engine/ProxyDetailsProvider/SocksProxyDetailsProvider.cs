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
        private Dictionary<string, IPAddress> outgoingIPAddressCache = new Dictionary<string, IPAddress>();

        public override async Task<ProxyTypeDetails> GetProxyDetails(Proxy proxy, CancellationTokenSource cancellationToken)
        {
            string proxyUriString = GetProxyUriString(proxy);

            ISocksProxyTypeHashtable hashtable = Context.Get<ISocksProxyTypeHashtable>();

            if (hashtable.Exists(proxyUriString))
            {
                return new SocksProxyDetails( hashtable[proxyUriString], outgoingIPAddressCache[proxyUriString]);
            }

            var httpDownloaderContainer = new HttpDownloaderContainer<SocksHttpClientHandler, SocksProgressMessageHandler>();

            string content = await httpDownloaderContainer.HttpDownloader.GetContentOrNull(GetProxyTypeDetectorUrl(proxy,
                                                                                                                   Resources.SocksProxyType),
                                                                                           proxy,
                                                                                           cancellationToken);
            if (content == null)
                return new SocksProxyDetails(SocksProxyTypes.CannotVerify, null);

            string[] values = content.Split(',');
            IPAddress outgoingIPAdress;

            if (values.Length != 2 || !IPAddress.TryParse(values[1], out outgoingIPAdress))
                return new SocksProxyDetails(SocksProxyTypes.CannotVerify, null);

            if (!outgoingIPAddressCache.ContainsKey(proxyUriString))
                outgoingIPAddressCache.Add(proxyUriString, outgoingIPAdress);

            return new SocksProxyDetails(hashtable[proxyUriString], outgoingIPAddressCache[proxyUriString]);
        }

        public override ProxyTypeDetails GetUncheckedProxyDetails()
        {
            return new SocksProxyDetails(SocksProxyTypes.Unchecked, null);
        }

        private static string GetProxyUriString(Proxy proxy)
        {
            UriBuilder uriBuilder = new UriBuilder();
            uriBuilder.Host = proxy.Address.ToString();
            uriBuilder.Port = proxy.Port;

            return uriBuilder.ToString();
        }
    }
}
