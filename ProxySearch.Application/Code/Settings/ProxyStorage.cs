using ProxySearch.Console.Code.Interfaces;
using ProxySearch.Engine;
using ProxySearch.Engine.Proxies;

namespace ProxySearch.Console.Code.Settings
{
    public class ProxyStorage : IUsedProxies, IBlackList, IBlackListManager
    {
        ProxyList proxyList;

        public ProxyStorage(ProxyList proxyList)
        {
            this.proxyList = proxyList;
        }

        public bool Contains(Proxy proxy)
        {
            return proxyList.Contains(proxy);
        }

        public void Add(Proxy proxy)
        {
            proxyList.Add(proxy);
        }

        public void Clear()
        {
            proxyList.Proxies.Clear();
        }

        public ProxyList ProxyList
        {
            get
            {
                return proxyList;
            }
        }
    }
}
