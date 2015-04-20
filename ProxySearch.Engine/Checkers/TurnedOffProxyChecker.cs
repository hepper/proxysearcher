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
        public TurnedOffProxyChecker():base(int.MaxValue)
        {
        }

        protected override Task<bool> Alive(Proxy proxy, TaskItem task, Action begin, Action<int> firstTime, Action<int> end, CancellationTokenSource cancellationToken)
        {
            return Task.FromResult(true);
        }

        protected override async Task<ProxyTypeDetails> GetProxyDetails(Proxy proxy, TaskItem task, CancellationTokenSource cancellationToken)
        {
            return await Task.FromResult<ProxyTypeDetails>(base.DetailsProvider.GetUncheckedProxyDetails());
        }

        protected override async Task<ProxyTypeDetails> UpdateProxyDetails(Proxy proxy, TaskItem task, CancellationTokenSource cancellationToken)
        {
            return await base.GetProxyDetails(proxy, task, cancellationToken);
        }
    }
}
