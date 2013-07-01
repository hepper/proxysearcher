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
        protected override Task<string> Download(string url, Proxy proxy, Action begin, Action firstTime, Action<int> end)
        {
            begin();
            firstTime();

            return Task.Run(() =>
            {
                using (SocksWebClient client = new SocksWebClient(proxy.Address, proxy.Port, ProxyTypes.Socks5))
                {
                    string result = client.DownloadString(url);

                    end(result.Length);
                    return result;
                }
            }, Context.Get<CancellationTokenSource>().Token);
        }

        protected override Task<object> GetProxyDetails(Proxy proxy, CancellationTokenSource cancellationToken)
        {
            return Task.FromResult<object>(null);
        }
    }
}
