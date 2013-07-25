using ProxySearch.Engine.Proxies.Socks;

namespace ProxySearch.Engine.Socks
{
    public interface ISocksProxyTypeHashtable
    {
        void Add(string proxyUrl, SocksProxyTypes type);
        bool Exists(string proxyUrl);
        SocksProxyTypes this[string proxyUrl]
        {
            get;
        }
    }
}
