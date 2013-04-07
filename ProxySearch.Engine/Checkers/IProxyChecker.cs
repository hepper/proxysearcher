using System.Collections.Generic;
using ProxySearch.Engine.GeoIP;
using ProxySearch.Engine.Proxies;

namespace ProxySearch.Engine.Checkers
{
    public interface IProxyChecker
    {
        void CheckAsync(List<Proxy> proxies, IProxySearchFeedback feedback, IGeoIP geoIP);
    }
}