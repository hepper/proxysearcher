using System;
using System.Threading;
using System.Threading.Tasks;
using ProxySearch.Engine.Properties;
using ProxySearch.Engine.Proxies;

namespace ProxySearch.Engine.ProxyDetailsProvider
{
    public abstract class ProxyDetailsProviderBase : IProxyDetailsProvider
    {
        public abstract Task<ProxyTypeDetails> GetProxyDetails(Proxy proxy, CancellationTokenSource cancellationToken);
        public abstract ProxyTypeDetails GetUncheckedProxyDetails();

        protected string GetProxyTypeDetectorUrl(Proxy proxy, string proxyType)
        {
            return string.Format(Resources.ProxyTypeDetectorUrl, proxy.Address, proxy.Port, proxyType, Guid.NewGuid());
        }
    }
}
