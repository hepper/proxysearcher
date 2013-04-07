using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ProxySearch.Common;
using ProxySearch.Engine.Proxies;

namespace ProxySearch.Engine.Checkers
{
    public class ProxyCheckerByUrlAndKeywords : HttpProxyCheckerBase
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

        protected override async Task<bool> Alive(Proxy info, Action begin, Action firstTime, Action<int> end)
        {
            string content = await Context.Get<Downloader>().GetContentOrNull(Url, info, Context.Get<CancellationTokenSource>(), begin, firstTime, end);

            return !Keywords.Any(item => !content.Contains(item));
        }
    }
}
