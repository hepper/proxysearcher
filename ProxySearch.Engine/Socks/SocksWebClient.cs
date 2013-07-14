using System;
using System.Net;
using System.Threading;
using ProxySearch.Engine.Socks.Ditrans;
using ProxySearch.Engine.Socks.Mentalis;

namespace ProxySearch.Engine.Socks
{
    public class SocksWebClient : WebClient
    {
        public SocksWebProxy SocksProxy
        {
            get;
            private set;
        }

        public CancellationTokenSource CancellationToken
        {
            get;
            private set;
        }

        public SocksWebClient(IPAddress ipAddress, ushort port, ProxyTypes proxyType, CancellationTokenSource cancellationToken)
        {
            SocksProxy = new SocksWebProxy(ipAddress.ToString(), port, proxyType);
            CancellationToken = cancellationToken;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            return new SocksHttpWebRequest(address, SocksProxy)
            {
                AllowAutoRedirect = true,
                CancellationToken = this.CancellationToken
            };
        }
    }
}
