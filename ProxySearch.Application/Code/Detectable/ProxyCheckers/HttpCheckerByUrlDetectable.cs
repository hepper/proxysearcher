using ProxySearch.Console.Properties;
using ProxySearch.Engine.Checkers;
using ProxySearch.Engine.ProxyDetailsProvider;

namespace ProxySearch.Console.Code.Detectable.ProxyCheckers
{
    public class HttpCheckerByUrlDetectable : CheckerByUrlDetectableBase<ProxyCheckerByUrl<HttpProxyDetailsProvider>>
    {
        public HttpCheckerByUrlDetectable()
            : base(Resources.HttpProxyType, Resources.ProxySearcherStaticHtml)
        {
        }
    }
}