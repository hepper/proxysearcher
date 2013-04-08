using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using ProxySearch.Engine.Proxies;
using ProxySearch.Engine.Proxies.Http;

namespace ProxySearch.Engine.Checkers
{
    public class TurnedOffProxyChecker : HttpProxyCheckerBase
    {
        Hashtable hashtable = new Hashtable();
        protected override Task<bool> Alive(Proxy proxy, Action begin, Action firstTime, Action<int> end)
        {
            return Task.FromResult(true);
        }

        protected override Task<object> GetProxyDetails(Proxy proxy, CancellationTokenSource cancellationToken)
        {
            if (hashtable.ContainsKey(proxy.AddressPort))
            {
                return base.GetProxyDetails(proxy, cancellationToken);
            }

            hashtable.Add(proxy.AddressPort, null);
            return Task.FromResult((object)new HttpProxyDetails(HttpProxyTypes.Unchecked));
        }
    }
}
