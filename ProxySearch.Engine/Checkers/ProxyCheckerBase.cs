using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ProxySearch.Common;
using ProxySearch.Engine.GeoIP;
using ProxySearch.Engine.Properties;
using ProxySearch.Engine.Proxies;

namespace ProxySearch.Engine.Checkers
{
    public abstract class ProxyCheckerBase : IProxyChecker
    {
        public void CheckAsync(List<ProxyInfo> proxies, IProxySearchFeedback feedback, IGeoIP geoIP)
        {
            foreach (ProxyInfo proxy in proxies)
            {
                Task.Run(async () =>
                {
                    using (Context.Get<TaskCounter>().Listen(TaskType.Search))
                    {
                        if (await Alive(proxy))
                        {
                            proxy.CountryInfo = await geoIP.GetLocation(proxy.Address.ToString());
                            proxy.Details = new ProxyDetails(await GetProxyDetails(proxy, Context.Get<CancellationTokenSource>()), GetProxyDetails);
                            feedback.OnAliveProxy(proxy);
                        }
                    }
                });
            }
        }

        protected abstract Task<bool> Alive(ProxyInfo proxy);
        protected abstract Task<object> GetProxyDetails(ProxyInfo proxy, CancellationTokenSource cancellationToken);
    }
}
