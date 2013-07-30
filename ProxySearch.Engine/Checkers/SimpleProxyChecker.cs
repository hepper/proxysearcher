using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using ProxySearch.Common;
using ProxySearch.Engine.Extensions;
using ProxySearch.Engine.Proxies;
using ProxySearch.Engine.Proxies.Http;
using ProxySearch.Engine.ProxyDetailsProvider;

namespace ProxySearch.Engine.Checkers
{
    public class SimpleProxyChecker<ProxyDetailsProviderType> : ProxyCheckerBase<ProxyDetailsProviderType>
                                                                 where ProxyDetailsProviderType : IProxyDetailsProvider, new()
    {
        protected override async Task<bool> Alive(Proxy info, Action begin, Action<int> firstTime, Action<int> end)
        {
            using (TcpClient tcpClient = new TcpClient())
            {
                try
                {
                    await tcpClient.ConnectAsync(info.Address, info.Port, Context.Get<CancellationTokenSource>().Token);
                }
                catch (SocketException)
                {
                    return false;
                }

                return true;
            }
        }

        protected override Task<object> GetProxyDetails(Proxy proxy, CancellationTokenSource cancellationToken)
        {
            return Task.FromResult(DetailsProvider.GetUncheckedProxyDetails());
        }

        protected override Task<object> UpdateProxyDetails(Proxy proxy, CancellationTokenSource cancellationToken)
        {
            return base.GetProxyDetails(proxy, cancellationToken);
        }
    }
}
