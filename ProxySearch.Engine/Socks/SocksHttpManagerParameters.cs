using System;
using System.Net.Http;
using System.Threading;
using ProxySearch.Engine.Proxies.Socks;

namespace ProxySearch.Engine.Socks
{
    public class SocksHttpManagerParameters
    {
        public HttpRequestMessage Request { get; set; }
        public CancellationToken CancellationToken { get; set; }
        public HttpClientHandler Handler { get; set; }
        public SocksProxyTypes ProxyType { get; set; }
        public Action<int, long?> ReportRequestProgress { get; set; }
        public Action<int, long?> ReportResponseProgress { get; set; }
    }
}
