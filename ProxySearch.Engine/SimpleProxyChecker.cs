using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Threading.Tasks;
using ProxySearch.Common;

namespace ProxySearch.Engine
{
    public class SimpleProxyChecker : IProxyChecker
    {
        public async Task<bool> Alive(ProxyInfo info)
        {
            using (TcpClient tcpClient = new TcpClient())
            {
                try
                {
                    if (info.Port == 80)
                        return false;

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
