using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProxySearch.Engine.Proxies;

namespace ProxySearch.Engine.Parser
{
    public interface IParseMethod
    {
        IEnumerable<Proxy> Parse(string document);
    }
}
