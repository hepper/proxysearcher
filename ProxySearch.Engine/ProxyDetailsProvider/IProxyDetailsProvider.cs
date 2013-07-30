using System.Threading;
using System.Threading.Tasks;
using ProxySearch.Engine.Proxies;

namespace ProxySearch.Engine.ProxyDetailsProvider
{
    public interface IProxyDetailsProvider
    {
        Task<object> GetProxyDetails(Proxy proxy, CancellationTokenSource cancellationToken);
        object GetUncheckedProxyDetails();
    }
}
