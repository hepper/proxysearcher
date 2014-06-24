using System;
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

            if (hashtable.Exists(proxyUriString))
            {
                SocksProxyTypes socksProxyType = hashtable[proxyUriString];
                return new SocksProxyDetails(socksProxyType);
            }

            var httpDownloaderContainer = new HttpDownloaderContainer<SocksHttpClientHandler, SocksProgressMessageHandler>();

            string content = await httpDownloaderContainer.HttpDownloader.GetContentOrNull(GetProxyTypeDetectorUrl(proxy, 
                                                                                                                   Resources.SocksProxyType), 
                                                                                           proxy, 
                                                                                           cancellationToken);

            if (content != null)
                return new SocksProxyDetails(hashtable[proxyUriString]);

            return new SocksProxyDetails(SocksProxyTypes.CannotVerify);
        }

        public override ProxyTypeDetails GetUncheckedProxyDetails()
        {
            return new SocksProxyDetails(SocksProxyTypes.Unchecked);
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
