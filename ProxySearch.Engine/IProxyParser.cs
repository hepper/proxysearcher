using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProxySearch.Engine
{
    public interface IProxyParser
    {
        Task<List<ProxyInfo>> ParseProxiesAsync(string document);
    }
}
