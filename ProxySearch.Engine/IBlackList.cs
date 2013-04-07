using ProxySearch.Engine.Proxies;

namespace ProxySearch.Engine
{
    public interface IBlackList
    {
        bool Contains(Proxy proxy);
    }
}
