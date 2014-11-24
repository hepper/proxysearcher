using System.Threading;
using System.Threading.Tasks;
using ProxySearch.Engine.Proxies;
using ProxySearch.Engine.Tasks;

namespace ProxySearch.Engine.ProxyDetailsProvider
{
    public interface IProxyDetailsProvider
    {
        Task<ProxyTypeDetails> GetProxyDetails(Proxy proxy, TaskItem task, CancellationTokenSource cancellationToken);
        ProxyTypeDetails GetUncheckedProxyDetails();
    }
}
