using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using ProxySearch.Common;
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
        protected override async Task<bool> Alive(Proxy proxy, TaskItem task, Action begin, Action<int> firstTime, Action<int> end)
        {
            task.UpdateDetails(string.Format(Resources.OpeningConnectionFormat, proxy));

            using (TcpClientExtended tcpClient = new TcpClientExtended())
            {
                try
                {
                    await tcpClient.ConnectAsync(proxy.Address, proxy.Port, Context.Get<CancellationTokenSource>().Token);
                }
                catch (SocketException)
                {
                    return false;
                }

                return true;
            }
        }

        protected override Task<ProxyTypeDetails> GetProxyDetails(Proxy proxy, CancellationTokenSource cancellationToken)
        {
            return Task.FromResult(DetailsProvider.GetUncheckedProxyDetails());
        }

        protected override Task<ProxyTypeDetails> UpdateProxyDetails(Proxy proxy, CancellationTokenSource cancellationToken)
        {
            return base.GetProxyDetails(proxy, cancellationToken);
        }
    }
}
