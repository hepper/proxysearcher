using ProxySearch.Console.Properties;
using ProxySearch.Engine.ProxyDetailsProvider;

namespace ProxySearch.Console.Code.Detectable.ProxyCheckers
{
    public class HttpCheckerByUrlAndKeywordsDetectable : CheckerByUrlAndKeywordDetectableBase<HttpProxyDetailsProvider>
    {
        public HttpCheckerByUrlAndKeywordsDetectable()
            : base(Resources.HttpProxyType, Resources.GoogleDotCom, Resources.GoogleDotComContent)
        {
        }
    }
}