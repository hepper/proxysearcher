using ProxySearch.Engine.GeoIP;

namespace ProxySearch.Engine.Checkers
{
    public interface IProxyChecker
    {
        void Alive(ProxyInfo info, IProxySearchFeedback feedback, IGeoIP geoIP);
    }
}
