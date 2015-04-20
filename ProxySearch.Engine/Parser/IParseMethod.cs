using System.Collections.Generic;
using ProxySearch.Engine.Proxies;

namespace ProxySearch.Engine.Parser
{
    public interface IParseMethod
    {
        IEnumerable<Proxy> Parse(string document);
    }
}