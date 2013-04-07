using ProxySearch.Engine.Proxies;

namespace ProxySearch.Engine
{
    public class EmptyBlackList : IBlackList
    {
        public bool Contains(Proxy proxy)
        {
            return false;
        }
    }
}
