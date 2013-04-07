using ProxySearch.Console.Code.Settings;
using ProxySearch.Engine.Proxies;

namespace ProxySearch.Console.Code.Interfaces
{
    public interface IBlackListManager
    {
        void Add(Proxy proxyInfo);
        void Clear();
        ProxyList ProxyList
        {
            get;
        }
    }
}
