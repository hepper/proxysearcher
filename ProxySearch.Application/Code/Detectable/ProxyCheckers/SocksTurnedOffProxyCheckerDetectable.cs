using ProxySearch.Console.Properties;
using ProxySearch.Engine.Checkers;
using ProxySearch.Engine.ProxyDetailsProvider;

namespace ProxySearch.Console.Code.Detectable.ProxyCheckers
{
    public class SocksTurnedOffProxyCheckerDetectable : SimpleDetectableBase<IProxyChecker, TurnedOffProxyChecker<SocksProxyDetailsProvider>>
    {
        public SocksTurnedOffProxyCheckerDetectable()
            : base(Resources.TurnedOffProxyChecker, Resources.TurnedOffProxyCheckerDetails, 4, Resources.SocksProxyType)
        {
        }
    }
}
