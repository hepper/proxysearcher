using System;
using System.Threading;
using System.Threading.Tasks;
using ProxySearch.Common;
using ProxySearch.Engine.Proxies;
using ProxySearch.Engine.Socks;
using ProxySearch.Engine.Socks.Mentalis;

namespace ProxySearch.Engine.Checkers
{
    public class SocksProxyCheckerByUrl : ProxyCheckerByUrlBase
    {
        public SocksProxyCheckerByUrl(string url, double accuracy)
            : base(url, accuracy)
        {

        }
        protected override async Task<string> Download(string url, Proxy proxy, Action begin, Action<int> firstTime, Action<int> end)
        {
            begin();

            using (SocksWebClient client = new SocksWebClient(proxy.Address, proxy.Port, ProxyTypes.Socks5, Context.Get<CancellationTokenSource>()))
            {
                string result = await client.DownloadStringTaskAsync(new Uri(url));

                firstTime(result.Length);
                end(result.Length);
                return result;
            }
        }

        protected override Task<object> GetProxyDetails(Proxy proxy, CancellationTokenSource cancellationToken)
        {
            return Task.FromResult<object>(null);
        }
    }
}
