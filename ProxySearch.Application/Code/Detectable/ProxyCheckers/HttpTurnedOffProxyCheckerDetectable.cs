using ProxySearch.Console.Properties;
using ProxySearch.Engine.Checkers;
using ProxySearch.Engine.ProxyDetailsProvider;

namespace ProxySearch.Console.Code.Detectable.ProxyCheckers
{
    public class HttpTurnedOffProxyCheckerDetectable : SimpleDetectableBase<IProxyChecker, TurnedOffProxyChecker<HttpProxyDetailsProvider>>
    {
        public HttpTurnedOffProxyCheckerDetectable()
            : base(Resources.TurnedOffProxyChecker, Resources.TurnedOffProxyCheckerDetails, 4, Resources.HttpProxyType)
        {
        }
    }
}