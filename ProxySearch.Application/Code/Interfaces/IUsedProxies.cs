using System.Collections.Generic;
using ProxySearch.Console.Code.Settings;
using ProxySearch.Engine.Proxies;

namespace ProxySearch.Console.Code.Interfaces
{
    public interface IUsedProxies
    {
        bool Contains(Proxy proxy);
        void Add(Proxy proxy);
        void Clear();
        ProxyList ProxyList
        {
            get;
        }
    }
}
