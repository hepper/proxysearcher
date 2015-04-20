using System.Collections.Generic;
using ProxySearch.Console.Properties;

namespace ProxySearch.Console.Code.Detectable.SearchEngines
{
    public class SocksUrlListSearchEngineDetectable : UrlListSearchEngineDetectableBase
    {
        public SocksUrlListSearchEngineDetectable()
            : base(Resources.SocksProxyType, new List<object>
            {
                "http://proxysearcher.sourceforge.net/Proxy%20List.php?type=socks&filtered=true&limit=100\n" +
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