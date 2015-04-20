using ProxySearch.Console.Properties;
using ProxySearch.Engine.ProxyDetailsProvider;

namespace ProxySearch.Console.Code.Detectable.ProxyCheckers
{
    public class SocksSimpleProxyCheckerDetectable : SimpleProxyCheckerDetectableBase<SocksProxyDetailsProvider>
    {
        public SocksSimpleProxyCheckerDetectable()
            : base(Resources.SocksProxyType)
        {
        }
    }
}
