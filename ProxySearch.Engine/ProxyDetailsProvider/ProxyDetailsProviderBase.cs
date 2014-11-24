using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using ProxySearch.Engine.Properties;
using ProxySearch.Engine.Proxies;
using ProxySearch.Engine.Tasks;

namespace ProxySearch.Engine.ProxyDetailsProvider
{
    public abstract class ProxyDetailsProviderBase : IProxyDetailsProvider
    {
        public abstract Task<ProxyTypeDetails> GetProxyDetails(Proxy proxy, TaskItem task, CancellationTokenSource cancellationToken);
        public abstract ProxyTypeDetails GetUncheckedProxyDetails();

        protected string GetProxyTypeDetectorUrl(Proxy proxy, string proxyType)
        {
            return string.Format(Resources.ProxyTypeDetectorUrlFormat, proxy.Address, proxy.Port, proxyType, Guid.NewGuid());
        }

        protected string GetProxyTypeDetectorUrl(Proxy proxy, IPAddress myIP, string proxyType)
        {
            if (myIP == null)
                return GetProxyTypeDetectorUrl(proxy, proxyType);

            return string.Format(Resources.ProxyTypeDetectorUrlFormat2, proxy.Address, proxy.Port, proxyType, myIP, Guid.NewGuid());
        }

        protected string MyIPUrl
        {
            get
            {
                return Resources.DetectMyIPUrl;
            }
        }
    }
}
