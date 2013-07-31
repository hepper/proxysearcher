using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace ProxySearch.Engine.Extended
{
    public class TcpClientExtended : TcpClient
    {
        private bool IsDisposed
        {
            get;
            set;
        }

        public bool IsCancelled
        {
            get;
            private set;
        }

        public Task ConnectAsync(string host, int port, CancellationToken cancellationToken)
        {
            return ConnectAsync(() => base.ConnectAsync(host, port), cancellationToken);
        }

        public Task ConnectAsync(IPAddress address, int port, CancellationToken cancellationToken)
        {
            return ConnectAsync(() => base.ConnectAsync(address, port), cancellationToken);
        }

        protected override void Dispose(bool disposing)
        {
            IsDisposed = true;
            base.Dispose(disposing);

            ThrowIfCancelled();
        }

        private async Task ConnectAsync(Func<Task> connectAction, CancellationToken cancellationToken)
        {
            await Task.WhenAny(connectAction(), Task.Run(() =>
            {
                cancellationToken.WaitHandle.WaitOne();

                if (!IsDisposed)
                {
                    IsCancelled = true;
                    Client.Dispose();
                }
            }));

            ThrowIfCancelled();
        }

        private void ThrowIfCancelled()
        {
            if (IsCancelled)
            {
                throw new TaskCanceledException();
            }
        }
    }
}
