﻿using System;
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

        protected override async Task<string> Download(string url, Proxy proxy, Action begin, Action<int> firstTime, Action<int> end)
        {
            return await Context.Get<HttpDownloader>().GetContentOrNull(url, proxy, Context.Get<CancellationTokenSource>(), begin, firstTime, end);
        }

        protected override Task<object> GetProxyDetails(Proxy proxy, CancellationTokenSource cancellationToken)
        {
            return new HttpUtils().GetProxyDetails(proxy, cancellationToken);
        }
    }
}
