using ProxySearch.Console.Properties;
using ProxySearch.Engine.Checkers;
using ProxySearch.Engine.ProxyDetailsProvider;

namespace ProxySearch.Console.Code.Detectable.ProxyCheckers
{
    public class SocksSimpleProxyCheckerDetectable : SimpleDetectableBase<IProxyChecker, SimpleProxyChecker<SocksProxyDetailsProvider>>
    {
        public SocksSimpleProxyCheckerDetectable()
            : base(Resources.SimpleProxyChecker, Resources.SimpleProxyCheckerDescription, 3, Resources.SocksProxyType)
        {
        }
    }
}
