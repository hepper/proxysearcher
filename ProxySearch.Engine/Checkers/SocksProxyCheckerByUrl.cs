using System.Threading;
using System.Threading.Tasks;
using ProxySearch.Engine.Proxies;

namespace ProxySearch.Engine.Checkers
{
    public class SocksProxyCheckerByUrl : ProxyCheckerByUrlBase
    {
        public SocksProxyCheckerByUrl(string url, double accuracy)
            : base(url, accuracy)
        {
        }

        protected override Task<object> GetProxyDetails(Proxy proxy, CancellationTokenSource cancellationToken)
        {
            return Task.FromResult<object>(null);
        }
    }
}
