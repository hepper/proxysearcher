using System.Net.Http;
using System.Net.Http.Handlers;
using System.Threading;
using System.Threading.Tasks;

namespace ProxySearch.Engine.Socks
{
    public class SocksProgressMessageHandler : ProgressMessageHandler
    {
        public SocksProgressMessageHandler()
            : base()
        {
        }

        public SocksProgressMessageHandler(HttpClientHandler innerHandler)
            : base(innerHandler)
        {
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpClientHandler handler = InnerHandler as HttpClientHandler;

            if (handler == null || handler.Proxy == null)
                return base.SendAsync(request, cancellationToken);

            return new SocksHttpManager().GetResponse(new SocksHttpManagerParameters
            {
                Request = request,
                CancellationToken = cancellationToken,
                Handler = handler,
                ReportRequestProgress = (transeffed, total) => OnHttpRequestProgress(request, CreateEventArgs(transeffed, total)),
                ReportResponseProgress = (transeffed, total) => OnHttpResponseProgress(request, CreateEventArgs(transeffed, total)),
                ProxyType = Application.SocksProxyHashTable[handler.Proxy.GetProxy(request.RequestUri).ToString()]
            });
        }

        private HttpProgressEventArgs CreateEventArgs(int transeffer, long? total)
        {
            if (!total.HasValue)
            {
                return new HttpProgressEventArgs(0, null, transeffer, total);
            }

            return new HttpProgressEventArgs((int)(transeffer * 100 / total.Value), null, transeffer, total);
        }
    }
}
