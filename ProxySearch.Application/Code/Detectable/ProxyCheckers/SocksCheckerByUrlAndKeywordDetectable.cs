using ProxySearch.Console.Properties;
using ProxySearch.Engine.ProxyDetailsProvider;

namespace ProxySearch.Console.Code.Detectable.ProxyCheckers
{
    public class SocksCheckerByUrlAndKeywordsDetectable : CheckerByUrlAndKeywordDetectableBase<SocksProxyDetailsProvider>
    {
        public SocksCheckerByUrlAndKeywordsDetectable()
            : base(Resources.SocksProxyType)
        {
        }
    }
}
