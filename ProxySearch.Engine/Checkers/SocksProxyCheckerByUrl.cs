using System;
using System.Threading;
using System.Threading.Tasks;
using ProxySearch.Common;
using ProxySearch.Engine.Proxies;
using ProxySearch.Engine.Proxies.Socks;
using ProxySearch.Engine.Socks;
using ProxySearch.Engine.Utils;

namespace ProxySearch.Engine.Checkers
{
    public class SocksProxyCheckerByUrl : ProxyCheckerByUrlBase
    {
        public SocksProxyCheckerByUrl(string url, double accuracy)
            : base(url, accuracy)
        {
        }

        protected override Task<object> GetProxyDetails(Proxy proxy, CancellationTokenSource cancellationToken)
        {
            UriBuilder uriBuilder = new UriBuilder();
            uriBuilder.Host = proxy.Address.ToString();
            uriBuilder.Port = proxy.Port;

            SocksProxyTypes socksProxyType = Context.Get<ISocksProxyTypeHashtable>()[uriBuilder.ToString()];

            return Task.FromResult<object>(new SocksProxyDetails(socksProxyType));
        }
    }
}
