using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProxySearch.Engine;

namespace ProxySearch.Console.Code.Interfaces
{
    public interface ISearchResult
    {
        void Clear();
        void Add(ProxyInfo proxy);
    }
}
