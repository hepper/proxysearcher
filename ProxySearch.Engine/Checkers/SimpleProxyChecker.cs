using System.Net.Sockets;
using System.Threading.Tasks;
using ProxySearch.Engine.Proxies;

namespace ProxySearch.Engine.Checkers
{
    public class SimpleProxyChecker : HttpProxyCheckerBase
    {
        protected override async Task<bool> Alive(ProxyInfo info)
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
    }
}
