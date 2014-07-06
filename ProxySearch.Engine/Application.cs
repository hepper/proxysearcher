using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ProxySearch.Common;
using ProxySearch.Engine.Checkers;
using ProxySearch.Engine.GeoIP;
using ProxySearch.Engine.Parser;
using ProxySearch.Engine.Properties;
using ProxySearch.Engine.Proxies;
using ProxySearch.Engine.SearchEngines;
using ProxySearch.Engine.Socks;
using ProxySearch.Engine.Tasks;

namespace ProxySearch.Engine
{
    public class Application
    {
        ISearchEngine searchEngine;
        IProxyProvider proxyProvider;
        IProxySearchFeedback feedback;
        IProxyChecker checker;
        IGeoIP geoIP;

        public Application(ISearchEngine searchEngine, IProxyProvider proxyProvider, IProxySearchFeedback feedback, IProxyChecker checker, IGeoIP geoIP)
        {
            this.searchEngine = searchEngine;
            this.proxyProvider = proxyProvider;

            this.feedback = feedback;
            this.checker = checker;
            this.geoIP = geoIP ?? new TurnOffGeoIP();

            Context.Set<ISocksProxyTypeHashtable>(new SocksProxyTypeHashtable());
        }

        public async void SearchAsync()
        {
            IEnumerable<ISearchEngine> searchEngines = searchEngine as IEnumerable<ISearchEngine>;

            if (searchEngines == null)
            {
                await SearchAsyncInternal(searchEngine);
            }
            else
            {
                foreach (ISearchEngine engine in searchEngines)
                {
                    Task task = SearchAsyncInternal(engine);
                }
            }
        }

        private async Task SearchAsyncInternal(ISearchEngine searchEngine)
        {
            try
            {
                using (TaskItem task = Context.Get<TaskManager>().Create(Resources.ProxySearching))
                {
                    while (true)
                    {
                        task.UpdateDetails(searchEngine.Status);

                        Uri uri = await searchEngine.GetNext();

                        if (uri == null || Context.Get<CancellationTokenSource>().IsCancellationRequested)
                            return;

                        task.UpdateDetails(string.Format(Resources.DownloadingFormat, uri.ToString()));

                        string document = await GetDocumentAsync(uri);

                        if (Context.Get<CancellationTokenSource>().IsCancellationRequested)
                            return;

                        if (document == null)
                            continue;

                        List<Proxy> proxies = await proxyProvider.ParseProxiesAsync(uri, document);

                        if (proxies.Any())
                            checker.CheckAsync(proxies, feedback, geoIP);
                    }
                }
            }
            catch
            {
            }
        }

        private async Task<string> GetDocumentAsync(Uri uri)
        {
            if (uri.IsFile)
            {
                return await GetFileContentAsync(uri);
            }

            return await GetInternetDocumentAsync(uri);
        }

        private static async Task<string> GetFileContentAsync(Uri uri)
        {
            try
            {
                string result = File.ReadAllText(uri.LocalPath);
                return await Task.FromResult<string>(result);
            }
            catch (Exception exception)
            {
                Context.Get<IExceptionLogging>().Write(exception);
                return null;
            }
        }

        private static async Task<string> GetInternetDocumentAsync(Uri uri)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                using (HttpResponseMessage response = await client.GetAsync(uri, Context.Get<CancellationTokenSource>().Token))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsStringAsync();
                    }
                }
            }
            catch
            {
            }

            return null;
        }
    }
}
