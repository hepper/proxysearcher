using ProxySearch.Console.Properties;
using ProxySearch.Engine.Checkers;
using ProxySearch.Engine.ProxyDetailsProvider;

namespace ProxySearch.Console.Code.Detectable.ProxyCheckers
{
    public class HttpSimpleProxyCheckerDetectable : SimpleDetectableBase<IProxyChecker, SimpleProxyChecker<HttpProxyDetailsProvider>>
    {
        public HttpSimpleProxyCheckerDetectable()
            : base(Resources.SimpleProxyChecker, Resources.SimpleProxyCheckerDescription, 3, new string[] { Resources.HttpProxyType })
        {
        }
    }
}
