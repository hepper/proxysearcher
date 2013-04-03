using System.Collections.Generic;
using System.Threading.Tasks;
using ProxySearch.Engine.Proxies;

namespace ProxySearch.Engine
{
    public interface IProxyParser
    {
        Task<List<ProxyInfo>> ParseProxiesAsync(string document);
    }
}
