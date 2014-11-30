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
            ISearchEngine searchEngine = new ParallelSearchEngine(new UrlListSearchEngine("http://proxysearcher.sourceforge.net/ProxyList.php?type=http&filtered=true&limit=1000"),
                                                                  new GoogleSearchEngine(40, "http proxy list", null));

            IProxyChecker checker = new ProxyCheckerByUrl<HttpProxyDetailsProvider>("http://google.com", 0.9);
            IHttpDownloaderContainer httpDownloaderContainer = new HttpDownloaderContainer<HttpClientHandler, ProgressMessageHandler>();

            Application application = new Application(searchEngine, checker, httpDownloaderContainer);

            application.ProxyAlive += application_ProxyAlive;

            application.SearchAsync().GetAwaiter().GetResult();
        }

        static void application_ProxyAlive(ProxyInfo proxyInfo)
        {
            switch ((HttpProxyTypes)Enum.Parse(typeof(HttpProxyTypes), proxyInfo.Details.Details.Type))
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
