using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ProxySearch.Engine.Proxies;

namespace ProxySearch.Engine.Socks
{
    public class SocksHttpClientHandler : HttpClientHandler
    {
        private static readonly HttpStatusCode[] redirectCodes = new HttpStatusCode[]
        {
            HttpStatusCode.Ambiguous,
            HttpStatusCode.Moved,
            HttpStatusCode.Redirect,
            HttpStatusCode.RedirectMethod,
            HttpStatusCode.RedirectKeepVerb
        };

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (Proxy == null)
                return base.SendAsync(request, cancellationToken);
            
            return new SocksHttpManager().GetResponse(request, cancellationToken, this);
        }
    }
}
