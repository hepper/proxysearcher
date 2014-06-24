using System.Collections.Generic;
using ProxySearch.Console.Controls;
using ProxySearch.Console.Properties;
using ProxySearch.Engine.SearchEngines;
using ProxySearch.Engine.SearchEngines.UrlList;

namespace ProxySearch.Console.Code.Detectable.SearchEngines
{
    public class HttpUrlListSearchEngineDetectable : DetectableBase<ISearchEngine, UrlListSearchEngine, UrlListPropertyControl>
    {
        public HttpUrlListSearchEngineDetectable()
            : base(Resources.UrlListEngine, Resources.UrlListEngineDescription, 2, Resources.HttpProxyType, 
            new List<object>
            {
                "http://proxysearcher.sourceforge.net/ProxyList.php?type=http",
                "http://www.aliveproxy.com/proxy-list-port-3128/\n" +
                "http://www.proxynova.com/proxy-server-list/port-3128/\n" +
                "http://www.checker.freeproxy.ru/checker/last_checked_proxies.php\n" +
                "http://vzlom.makewap.ru/Free_proxy_list.html?sid=9aa1f48d87\n" +
                "http://soft.bz/razdacha-proxy/proxy-list-http-socks4-socks5-27-10-12/\n" +
                "http://www.prime-speed.ru/proxy/free-proxy-list/all-working-proxies.php"
            })
        {
        }
    }
}
