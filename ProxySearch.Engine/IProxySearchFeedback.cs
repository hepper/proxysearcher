using ProxySearch.Engine.Proxies;

namespace ProxySearch.Engine
{
    public interface IProxySearchFeedback
    {
        void OnAliveProxy(ProxyInfo proxyInfo);
        void OnSearchFinished();
        void OnSearchCancelled();
        void UpdateJobCount(TaskType type, int currentCount, int totalCount);
    }
}
