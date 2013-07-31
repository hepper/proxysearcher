using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace ProxySearch.Engine.Extensions
{
    public static class TcpClientExtension
    {
        private static Dictionary<CancellationToken, List<TcpClient>> data = new Dictionary<CancellationToken, List<TcpClient>>();

        public static Task ConnectAsync(this TcpClient tcpClient, string host, int port, CancellationToken cancellationToken)
        {
            return ConnectAsync(tcpClient, () => tcpClient.ConnectAsync(host, port), cancellationToken);
        }

        public static Task ConnectAsync(this TcpClient tcpClient, IPAddress address, int port, CancellationToken cancellationToken)
        {
            return ConnectAsync(tcpClient, () => tcpClient.ConnectAsync(address, port), cancellationToken);
        }

        private static async Task ConnectAsync(TcpClient tcpClient, Func<Task> connectAction, CancellationToken cancellationToken)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            if (!data.ContainsKey(cancellationToken))
            {
                data.Add(cancellationToken, new List<TcpClient>());
                Task task = Task.Run(() =>
                {
                    cancellationToken.WaitHandle.WaitOne();
                    data[cancellationToken].ForEach(client => ((IDisposable)client).Dispose());
                }, cancellationTokenSource.Token);
            }

            data[cancellationToken].Add(tcpClient);

            try
            {
                await connectAction();
            }
            finally
            {
                data[cancellationToken].Remove(tcpClient);

                if (!data[cancellationToken].Any())
                {
                    data.Remove(cancellationToken);
                    cancellationTokenSource.Cancel();
                }
            }
        }
    }
}
