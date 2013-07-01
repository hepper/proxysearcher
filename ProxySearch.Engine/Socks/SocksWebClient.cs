using System;
using System.Net;
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

        public SocksWebClient(IPAddress ipAddress, ushort port, ProxyTypes proxyType)
        {
            SocksProxy = new SocksWebProxy(ipAddress.ToString(), port, proxyType);
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            SocksHttpWebRequest request = SocksHttpWebRequest.Create(address, SocksProxy);
            request.AllowAutoRedirect = true;

            return request;
        }
    }
}
