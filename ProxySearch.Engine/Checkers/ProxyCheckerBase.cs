using System;
using System.Collections.Generic;
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
                            proxy.Details.Type = await GetProxyType(new ProxyInfo(proxy.Address, proxy.Port));
                            feedback.OnAliveProxy(proxy);
                        }
                    }
                });
            }
        }

        private async Task<string> GetProxyType(ProxyInfo proxy)
        {
            string result = await Context.Get<CheckerUtils>().GetContentOrNull(string.Format(Resources.ProxyTypeDetectorUrl, Guid.NewGuid()), proxy);

            if (result == null)
                return Resources.CannotVerify;

            string[] data = result.Split('|');

            if (data.Length != 2 || data[0] != Resources.ProxyTypeDetectorGuid || data[1].Length > 20)
                return Resources.ChangesContent;

            return data[1];
        }

        protected abstract Task<bool> Alive(ProxyInfo info);
    }
}
