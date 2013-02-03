using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProxySearch.Engine
{
    public interface IProxySearcher
    {
        void BeginSearch(string document);
    }
}
