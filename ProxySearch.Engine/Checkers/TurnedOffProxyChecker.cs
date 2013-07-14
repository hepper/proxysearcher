using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using ProxySearch.Engine.Proxies;
using ProxySearch.Engine.Proxies.Http;
using ProxySearch.Engine.Utils;

namespace ProxySearch.Engine.Checkers
{
    public class TurnedOffProxyChecker : ProxyCheckerBase
    {
        Hashtable hashtable = new Hashtable();
        protected override Task<bool> Alive(Proxy proxy, Action begin, Action<int> firstTime, Action<int> end)
        {
            return Task.FromResult(true);
        }

        protected override Task<object> GetProxyDetails(Proxy proxy, CancellationTokenSource cancellationToken)
        {
            if (hashtable.ContainsKey(proxy.AddressPort))
            {
                return new HttpUtils().GetProxyDetails(proxy, cancellationToken);
            }

            hashtable.Add(proxy.AddressPort, null);
            return Task.FromResult((object)new HttpProxyDetails(HttpProxyTypes.Unchecked));
        }
    }
}
