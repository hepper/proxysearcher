using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace ProxySearch.Engine.Extensions
{
    public static class TcpClientExtension
    {
        public static Task ConnectAsync(this TcpClient tcpClient, string host, int port, CancellationToken cancellationToken)
        {
            return ConnectAsync(tcpClient, () => tcpClient.ConnectAsync(host, port), cancellationToken);
        }

        public static Task ConnectAsync(this TcpClient tcpClient, IPAddress address, int port, CancellationToken cancellationToken)
        {
            return ConnectAsync(tcpClient, () => tcpClient.ConnectAsync(address, port), cancellationToken);
        }

        private static Task ConnectAsync(TcpClient tcpClient, Func<Task> connectAction, CancellationToken cancellationToken)
        {
            return connectAction();
        }
    }
}
