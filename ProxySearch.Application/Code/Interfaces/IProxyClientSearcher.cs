using System.Collections.Generic;

namespace ProxySearch.Console.Code.Interfaces
{
    public interface IProxyClientSearcher
    {
        List<IProxyClient> SelectedClients
        {
            get;
        }

        IProxyClient SelectedSystemProxy
        {
            get;
        }

        List<IProxyClient> AllClients
        {
            get;
        }
    }
}
