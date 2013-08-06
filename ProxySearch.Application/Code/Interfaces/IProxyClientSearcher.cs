using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProxySearch.Console.Code.Interfaces
{
    public interface IProxyClientSearcher
    {
        List<IProxyClient> SelectedClients
        {
            get;
        }

        List<IProxyClient> AllClients
        {
            get;
        }
    }
}
