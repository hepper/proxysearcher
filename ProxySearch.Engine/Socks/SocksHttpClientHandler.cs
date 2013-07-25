using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ProxySearch.Engine.Socks
{
    public class SocksHttpClientHandler : HttpClientHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (Proxy == null)
                return base.SendAsync(request, cancellationToken);

            return new SocksHttpManager().GetResponse(new SocksHttpManagerParameters
            {
                Request = request,
                CancellationToken = cancellationToken,
                Handler = this
            });
        }
    }
}
