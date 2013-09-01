using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProxySearch.Engine.Proxies;

namespace ProxySearch.Engine.Parser
{
    public interface IProxyProvider
    {
        Task<List<Proxy>> ParseProxiesAsync(Uri uri, string document);
    }
}
