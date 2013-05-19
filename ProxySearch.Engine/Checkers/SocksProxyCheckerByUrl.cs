using System;
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
        protected override Task<string> Download(string url, Proxy proxy, Action begin, Action firstTime, Action<int> end)
        {
            return Task.FromResult(string.Empty);
        }

        protected override Task<object> GetProxyDetails(Proxy proxy, System.Threading.CancellationTokenSource cancellationToken)
        {
            return Task.FromResult<object>(null);
        }
    }
}
