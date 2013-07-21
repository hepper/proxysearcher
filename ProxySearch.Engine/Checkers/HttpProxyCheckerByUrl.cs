using System;
using System.Threading;
using System.Threading.Tasks;
using ProxySearch.Common;
using ProxySearch.Engine.Proxies;
using ProxySearch.Engine.Utils;

namespace ProxySearch.Engine.Checkers
{
    public class HttpProxyCheckerByUrl : ProxyCheckerByUrlBase
    {
        public HttpProxyCheckerByUrl(string url, double accuracy)
            : base(url, accuracy)
        {
        }

        protected override Task<object> GetProxyDetails(Proxy proxy, CancellationTokenSource cancellationToken)
        {
            return new HttpUtils().GetProxyDetails(proxy, cancellationToken);
        }
    }
}
