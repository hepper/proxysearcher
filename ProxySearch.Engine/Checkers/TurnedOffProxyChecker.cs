using System;
using System.Threading;
using System.Threading.Tasks;
using ProxySearch.Engine.Proxies;
using ProxySearch.Engine.Proxies.Http;
using ProxySearch.Engine.ProxyDetailsProvider;

namespace ProxySearch.Engine.Checkers
{
    public class TurnedOffProxyChecker : ProxyCheckerBase<HttpProxyDetailsProvider>
    {
        protected override Task<bool> Alive(Proxy proxy, Action begin, Action<int> firstTime, Action<int> end)
        {
            return Task.FromResult(true);
        }

        protected override Task<object> GetProxyDetails(Proxy proxy, CancellationTokenSource cancellationToken)
        {           
            return Task.FromResult<object>(new HttpProxyDetails(HttpProxyTypes.Unchecked));
        }

        protected override Task<object> UpdateProxyDetails(Proxy proxy, CancellationTokenSource cancellationToken)
        {
            return base.GetProxyDetails(proxy, cancellationToken);
        }
    }
}
