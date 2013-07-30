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
    public class SocksProxyDetailsProvider : IProxyDetailsProvider
    {
        public async Task<object> GetProxyDetails(Proxy proxy, CancellationTokenSource cancellationToken)
        {
            UriBuilder uriBuilder = new UriBuilder();
            uriBuilder.Host = proxy.Address.ToString();
            uriBuilder.Port = proxy.Port;

            ISocksProxyTypeHashtable hashtable = Context.Get<ISocksProxyTypeHashtable>();

            if (hashtable.Exists(uriBuilder.ToString()))
            {
                SocksProxyTypes socksProxyType = hashtable[uriBuilder.ToString()];
                return new SocksProxyDetails(socksProxyType);
            }

            string content = await new HttpDownloaderContainer<SocksHttpClientHandler, SocksProgressMessageHandler>().HttpDownloader.GetContentOrNull(Resources.SpeedTestUrl, proxy, cancellationToken);

            if (content != null)
                return Task.FromResult<object>(new SocksProxyDetails(hashtable[uriBuilder.ToString()]));

            return Task.FromResult<object>(new SocksProxyDetails(SocksProxyTypes.CannotVerify));
        }

        public object GetUncheckedProxyDetails()
        {
            return new SocksProxyDetails(SocksProxyTypes.Unchecked);
        }
    }
}
