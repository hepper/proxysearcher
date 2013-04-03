using System.Collections.Generic;
using ProxySearch.Engine.GeoIP;
using ProxySearch.Engine.Proxies;

namespace ProxySearch.Engine.Checkers
{
    public interface IProxyChecker
    {
        void CheckAsync(List<ProxyInfo> proxies, IProxySearchFeedback feedback, IGeoIP geoIP);
    }
}