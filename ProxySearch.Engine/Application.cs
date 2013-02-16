using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Net;
using ProxySearch.Common;
using System.Threading;
using System.Net.Http;
using System.Threading.Tasks;

namespace ProxySearch.Engine
{
    public class Application
    {
        ISearchEngine searchEngine;
        IProxySearcher proxySearcher;

        public Application(ISearchEngine searchEngine, IProxySearcher proxySearcher)
        {
            this.searchEngine = searchEngine;
            this.proxySearcher = proxySearcher;
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

                                proxySearcher.BeginSearch(document);
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
