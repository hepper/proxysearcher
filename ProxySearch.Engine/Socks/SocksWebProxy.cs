using System.Net;
using ProxySearch.Engine.Socks.Mentalis;

namespace ProxySearch.Engine.Socks
{
    public class SocksWebProxy : WebProxy
    {
        public ProxyTypes ProxyType
        {
            get;
            private set;
        }

        public SocksWebProxy(string host, int port, ProxyTypes proxyType)
            : base(host, port)
        {
            ProxyType = proxyType;
        }
    }
}
