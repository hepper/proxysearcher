using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using ProxySearch.Engine.Proxies;

namespace ProxySearch.Engine.Checkers
{
    public class SimpleProxyChecker : ProxyCheckerBase
    {
        protected override async Task<bool> Alive(Proxy info, Action begin, Action firstTime, Action<int> end)
        {
            using (TcpClient tcpClient = new TcpClient())
            {
                try
                {
                    await tcpClient.ConnectAsync(info.Address, info.Port);
                }
                catch (SocketException)
                {
                    return false;
                }

                return true;
            }
        }

        protected override Task<object> GetProxyDetails(Proxy proxy, System.Threading.CancellationTokenSource cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
