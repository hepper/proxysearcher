using System.Collections.Generic;
using System.Threading.Tasks;
using ProxySearch.Common;
using ProxySearch.Engine.GeoIP;

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
                            feedback.OnAliveProxy(proxy);
                        }
                    }
                });
            }
        }

        protected abstract Task<bool> Alive(ProxyInfo info);
    }
}
