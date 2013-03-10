using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using ProxySearch.Common;
using ProxySearch.Engine.Checkers;
using ProxySearch.Engine.GeoIP;
using System.Linq;

namespace ProxySearch.Engine
{
    public class Application
    {
        ISearchEngine searchEngine;
        IProxyParser proxyParser;
        IProxySearchFeedback feedback;
        IProxyChecker checker;
        IGeoIP geoIP;
        IWebBrowser webBrowser;

        public Application(ISearchEngine searchEngine, IProxyParser proxyParser, IProxySearchFeedback feedback, IProxyChecker checker, IGeoIP geoIP)
        {
            this.searchEngine = searchEngine;
            this.proxyParser = proxyParser;

            this.feedback = feedback;
            this.checker = checker;
            this.geoIP = geoIP ?? new TurnOffGeoIP();
        }

        public async void SearchAsync()
        {
            using (Context.Get<TaskCounter>().Listen(TaskType.Search))
            {
                while (true)
                {
                    Uri uri = await searchEngine.GetNext();

                    if (uri == null)
                        break;

                    if (Context.Get<CancellationTokenSource>().IsCancellationRequested)
                    {
                        return;
                    }

                    try
                    {
                        using (HttpClient client = new HttpClient())
                        using (HttpResponseMessage response = await client.GetAsync(uri, Context.Get<CancellationTokenSource>().Token))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                string document = await response.Content.ReadAsStringAsync();

                                if (Context.Get<CancellationTokenSource>().IsCancellationRequested)
                                {
                                    return;
                                }

                                List<ProxyInfo> proxies = await proxyParser.ParseProxiesAsync(document);

                                if (proxies.Any())
                                    checker.CheckAsync(proxies, feedback, geoIP);
                            }
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }
    }
}
