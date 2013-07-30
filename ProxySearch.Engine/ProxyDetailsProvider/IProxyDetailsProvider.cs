using System.Threading;
using System.Threading.Tasks;
using ProxySearch.Engine.Proxies;

namespace ProxySearch.Engine.ProxyDetailsProvider
{
    public interface IProxyDetailsProvider
    {
        Task<ProxyTypeDetails> GetProxyDetails(Proxy proxy, CancellationTokenSource cancellationToken);
        ProxyTypeDetails GetUncheckedProxyDetails();
    }
}
