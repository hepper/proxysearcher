using System;
using System.Threading;
using System.Threading.Tasks;
using ProxySearch.Engine.Proxies;
using ProxySearch.Engine.ProxyDetailsProvider;
using ProxySearch.Engine.Tasks;

namespace ProxySearch.Engine.Checkers
{
    public class TurnedOffProxyChecker<ProxyDetailsProviderType> : ProxyCheckerBase<ProxyDetailsProviderType>
        where ProxyDetailsProviderType : IProxyDetailsProvider, new()
    {
        protected override Task<bool> Alive(Proxy proxy, TaskItem task, Action begin, Action<int> firstTime, Action<int> end)
        {
            return Task.FromResult(true);
        }

        protected override Task<ProxyTypeDetails> GetProxyDetails(Proxy proxy, CancellationTokenSource cancellationToken)
        {
            return Task.FromResult<ProxyTypeDetails>(base.DetailsProvider.GetUncheckedProxyDetails());
        }

        protected override Task<ProxyTypeDetails> UpdateProxyDetails(Proxy proxy, CancellationTokenSource cancellationToken)
        {
            return base.GetProxyDetails(proxy, cancellationToken);
        }
    }
}
