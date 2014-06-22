using ProxySearch.Engine.Proxies;

namespace ProxySearch.Engine
{
    public interface IProxySearchFeedback
    {
        void OnAliveProxy(ProxyInfo proxyInfo);
        void OnSearchFinished();
        void OnSearchCancelled();
    }
}
