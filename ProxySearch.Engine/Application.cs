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
using ProxySearch.Engine.Proxies;
using ProxySearch.Engine.SearchEngines;

namespace ProxySearch.Engine
{
    public class Application
    {
        ISearchEngine searchEngine;
        IProxyParser proxyParser;
        IProxySearchFeedback feedback;
        IProxyChecker checker;
        IGeoIP geoIP;

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

                    if (uri == null || Context.Get<CancellationTokenSource>().IsCancellationRequested)
                        return;

                    string document = await GetDocumentAsync(uri);

                    if (Context.Get<CancellationTokenSource>().IsCancellationRequested)
                        return;

                    if (document == null)
                        continue;

                    List<ProxyInfo> proxies = await proxyParser.ParseProxiesAsync(document);

                    if (proxies.Any())
                        checker.CheckAsync(proxies, feedback, geoIP);
                }
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
