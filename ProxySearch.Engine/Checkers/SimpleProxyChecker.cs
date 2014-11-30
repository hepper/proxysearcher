using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using ProxySearch.Engine.Extended;
using ProxySearch.Engine.Properties;
using ProxySearch.Engine.Proxies;
using ProxySearch.Engine.ProxyDetailsProvider;
using ProxySearch.Engine.Tasks;

namespace ProxySearch.Engine.Checkers
{
    public class SimpleProxyChecker<ProxyDetailsProviderType> : ProxyCheckerBase<ProxyDetailsProviderType>
                                                                 where ProxyDetailsProviderType : IProxyDetailsProvider, new()
    {
        protected override async Task<bool> Alive(Proxy proxy, TaskItem task, Action begin, Action<int> firstTime, Action<int> end, CancellationTokenSource cancellationToken)
        {
            task.UpdateDetails(string.Format(Resources.OpeningConnectionFormat, proxy));

            using (TcpClientExtended tcpClient = new TcpClientExtended())
            {
                try
                {
                    await tcpClient.ConnectAsync(proxy.Address, proxy.Port, cancellationToken.Token);
                }
                catch (SocketException)
                {
                    return false;
                }

                return true;
            }
        }

        protected override async Task<ProxyTypeDetails> GetProxyDetails(Proxy proxy, TaskItem task, CancellationTokenSource cancellationToken)
        {
            return await Task.FromResult(DetailsProvider.GetUncheckedProxyDetails());
        }

        protected override async Task<ProxyTypeDetails> UpdateProxyDetails(Proxy proxy, TaskItem task, CancellationTokenSource cancellationToken)
        {
            return await base.GetProxyDetails(proxy, task, cancellationToken);
        }
    }
}
