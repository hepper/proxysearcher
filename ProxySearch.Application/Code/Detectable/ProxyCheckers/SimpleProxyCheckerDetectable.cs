using ProxySearch.Console.Properties;
using ProxySearch.Engine.Checkers;

namespace ProxySearch.Console.Code.Detectable.ProxyCheckers
{
    public class SimpleProxyCheckerDetectable : SimpleDetectableBase<IProxyChecker, SimpleProxyChecker>
    {
        public SimpleProxyCheckerDetectable()
            : base(Resources.SimpleProxyChecker, Resources.SimpleProxyCheckerDescription, 3, new string[] { Resources.HttpProxyType, Resources.SocksProxyType })
        {
        }
    }
}
