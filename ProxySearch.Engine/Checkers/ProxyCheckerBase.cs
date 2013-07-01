using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ProxySearch.Common;
using ProxySearch.Engine.Bandwidth;
using ProxySearch.Engine.GeoIP;
using ProxySearch.Engine.Properties;
using ProxySearch.Engine.Proxies;

namespace ProxySearch.Engine.Checkers
{
    public abstract class ProxyCheckerBase : IProxyChecker
    {
        public void CheckAsync(List<Proxy> proxies, IProxySearchFeedback feedback, IGeoIP geoIP)
        {
            foreach (Proxy proxy in proxies)
            {
                Proxy proxyCopy = proxy;

                Task.Run(async () =>
                {
                    using (Context.Get<TaskCounter>().Listen(TaskType.Search))
                    {
                        BanwidthInfo bandwidth = null;

                        if (await Alive(proxyCopy, () => bandwidth = new BanwidthInfo()
                        {
                            BeginTime = DateTime.Now
                        }, () => bandwidth.FirstTime = DateTime.Now, lenght =>
                        {
                           
                            bandwidth.FirstCount = lenght * 2;
                            bandwidth.EndTime = bandwidth.FirstTime;
                            bandwidth.EndCount = bandwidth.FirstCount;
                        }))
                        {
                            ProxyInfo proxyInfo = new ProxyInfo(proxyCopy)
                            {
                                CountryInfo = await geoIP.GetLocation(proxyCopy.Address.ToString()),
                                Details = new ProxyDetails(await GetProxyDetails(proxy, Context.Get<CancellationTokenSource>()), GetProxyDetails)
                            };

                            if (bandwidth != null)
                                Context.Get<IBandwidthManager>().UpdateBandwidthData(proxyInfo, bandwidth);

                            feedback.OnAliveProxy(proxyInfo);
                        }
                    }
                });
            }
        }

        protected abstract Task<bool> Alive(Proxy proxy, Action begin, Action firstTime, Action<int> end);
        protected abstract Task<object> GetProxyDetails(Proxy proxy, CancellationTokenSource cancellationToken);
    }
}
