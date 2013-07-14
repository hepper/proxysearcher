using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ProxySearch.Common;
using ProxySearch.Engine.Proxies;
using ProxySearch.Engine.Utils;

namespace ProxySearch.Engine.Checkers
{
    public class ProxyCheckerByUrlAndKeywords : ProxyCheckerBase
    {
        private string Url
        {
            get;
            set;
        }

        private string[] Keywords
        {
            get;
            set;
        }

        public ProxyCheckerByUrlAndKeywords(string url, string keywords)
        {
            Url = url;
            Keywords = keywords.Split(' ');
        }

        protected override async Task<bool> Alive(Proxy info, Action begin, Action<int> firstTime, Action<int> end)
        {
            string content = await Context.Get<HttpDownloader>().GetContentOrNull(Url, info, Context.Get<CancellationTokenSource>(), begin, firstTime, end);

            return !Keywords.Any(item => !content.Contains(item));
        }

        protected override Task<object> GetProxyDetails(Proxy proxy, CancellationTokenSource cancellationToken)
        {
            return new HttpUtils().GetProxyDetails(proxy, cancellationToken);
        }
    }
}
