using ProxySearch.Console.Properties;
using ProxySearch.Engine.ProxyDetailsProvider;

namespace ProxySearch.Console.Code.Detectable.ProxyCheckers
{
    public class HttpSimpleProxyCheckerDetectable : SimpleProxyCheckerDetectableBase<HttpProxyDetailsProvider>
    {
        public HttpSimpleProxyCheckerDetectable()
            : base(Resources.HttpProxyType)
        {
        }
    }
}