using ProxySearch.Console.Properties;
using ProxySearch.Engine.Checkers;
using ProxySearch.Engine.ProxyDetailsProvider;

namespace ProxySearch.Console.Code.Detectable.ProxyCheckers
{
    public class SocksCheckerByUrlDetectable : CheckerByUrlDetectableBase<ProxyCheckerByUrl<SocksProxyDetailsProvider>>
    {
        public SocksCheckerByUrlDetectable():base(Resources.SocksProxyType, Resources.ProxySearcherStaticHtml)
        {
        }
    }
}