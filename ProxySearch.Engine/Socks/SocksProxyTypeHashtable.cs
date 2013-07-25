using System.Collections;
using ProxySearch.Engine.Proxies.Socks;

namespace ProxySearch.Engine.Socks
{
    public class SocksProxyTypeHashtable : ISocksProxyTypeHashtable
    {
        Hashtable hashtable = new Hashtable();

        public void Add(string proxyUrl, SocksProxyTypes type)
        {
            if (!Exists(proxyUrl))
            {
                hashtable.Add(proxyUrl, type);
            }
        }

        public bool Exists(string proxyUrl)
        {
            return hashtable.ContainsKey(proxyUrl);
        }

        public SocksProxyTypes this[string proxyUrl]
        {
            get
            {
                return (SocksProxyTypes)hashtable[proxyUrl];
            }
        }
    }
}
