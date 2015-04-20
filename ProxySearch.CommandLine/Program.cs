using System;
using System.Net.Http;
using System.Net.Http.Handlers;
using ProxySearch.Engine;
using ProxySearch.Engine.Checkers;
using ProxySearch.Engine.DownloaderContainers;
using ProxySearch.Engine.Proxies;
using ProxySearch.Engine.Proxies.Http;
using ProxySearch.Engine.ProxyDetailsProvider;
using ProxySearch.Engine.SearchEngines;

namespace ProxySearch.CommandLine
{
    public class Program
    {
        static void Main(string[] args)
        {
            var url = "http://proxysearcher.sourceforge.net/Proxy%20List.php?type=http&filtered=true&limit=1000";
            ISearchEngine searchEngine = new ParallelSearchEngine(new UrlListSearchEngine(url),
                                                                  new GoogleSearchEngine(40, "http proxy list", null));

            IProxyChecker checker =
                new ProxyCheckerByUrl<HttpProxyDetailsProvider>("http://wikipedia.org/", 0.9, 100);
            IHttpDownloaderContainer httpDownloaderContainer =
                new HttpDownloaderContainer<HttpClientHandler, ProgressMessageHandler>();

            Application application = new Application(searchEngine, checker, httpDownloaderContainer);

            application.ProxyAlive += application_ProxyAlive;
            application.OnError += exception => Console.WriteLine(exception.Message);

            application.SearchAsync().GetAwaiter().GetResult();
        }

        static void application_ProxyAlive(ProxyInfo proxyInfo)
        {
            switch (proxyInfo.Details.Details.GetStrongType<HttpProxyTypes>())
            {
                case HttpProxyTypes.Anonymous:
                case HttpProxyTypes.HighAnonymous:
                    Console.WriteLine(proxyInfo.AddressPort);
                    break;
                default:
                    break;
            }
        }
    }
}