using ProxySearch.Engine.GeoIP;

namespace ProxySearch.Engine.Checkers
{
    public interface IProxyChecker
    {
        void Check(ProxyInfo info, IProxySearchFeedback feedback, IGeoIP geoIP);
    }
}