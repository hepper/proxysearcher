using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ProxySearch.Common;
using ProxySearch.Engine.Bandwidth;
using ProxySearch.Engine.DownloaderContainers;
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
                        }, lenght =>
                        {
                            bandwidth.FirstTime = DateTime.Now;
                            bandwidth.FirstCount = lenght * 2;
                        }, lenght =>
                        {
                            bandwidth.EndTime = DateTime.Now;
                            bandwidth.EndCount = lenght * 2;
                        }))
                        {
                            ProxyInfo proxyInfo = new ProxyInfo(proxyCopy)
                            {
                                CountryInfo = await geoIP.GetLocation(proxyCopy.Address.ToString()),
                                Details = new ProxyDetails(await GetProxyDetails(proxy, Context.Get<CancellationTokenSource>()), GetProxyDetails)
                            };

                            if (bandwidth != null)
                                Context.Get<IHttpDownloaderContainer>().BandwidthManager.UpdateBandwidthData(proxyInfo, bandwidth);

                            feedback.OnAliveProxy(proxyInfo);
                        }
                    }
                });
            }
        }

        protected abstract Task<bool> Alive(Proxy proxy, Action begin, Action<int> firstTime, Action<int> end);
        protected abstract Task<object> GetProxyDetails(Proxy proxy, CancellationTokenSource cancellationToken);
    }
}
