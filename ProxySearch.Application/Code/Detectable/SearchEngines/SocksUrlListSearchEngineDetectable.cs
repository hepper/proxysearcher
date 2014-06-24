using System.Collections.Generic;
using ProxySearch.Console.Controls;
using ProxySearch.Console.Properties;
using ProxySearch.Engine.SearchEngines;
using ProxySearch.Engine.SearchEngines.UrlList;

namespace ProxySearch.Console.Code.Detectable.SearchEngines
{
    public class SocksUrlListSearchEngineDetectable : DetectableBase<ISearchEngine, UrlListSearchEngine, UrlListPropertyControl>
    {
        public SocksUrlListSearchEngineDetectable()
            : base(Resources.UrlListEngine, Resources.UrlListEngineDescription, 2, Resources.SocksProxyType,
            new List<object>
            {
                "http://proxysearcher.sourceforge.net/ProxyList.php?type=socks\n" +
                "http://socksproxy-list.blogspot.com/\n" +
                "http://www.vipsocks24.com/\n" +
                "http://proxy.ucoz.com/\n" +
                "http://www.myiptest.com/staticpages/index.php/Free-SOCKS5-SOCKS4-Proxy-lists.html\n" +
                "http://www.atomintersoft.com/products/alive-proxy/socks5-list/\n" +
                "http://proxy-heaven.blogspot.com/"
            })
        {
        }
    }
}
