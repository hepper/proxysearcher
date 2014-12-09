using System.Collections.Generic;

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

        List<IProxyClient> IEClients
        {
            get;
        }

        IProxyClient GetInternetExplorerClientOrNull(string type);
    }
}
