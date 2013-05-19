using ProxySearch.Console.Properties;
using ProxySearch.Engine.Checkers;

namespace ProxySearch.Console.Code.Detectable.ProxyCheckers
{
    public class TurnedOffProxyCheckerDetectable : SimpleDetectableBase<IProxyChecker, TurnedOffProxyChecker>
    {
        public TurnedOffProxyCheckerDetectable():base(Resources.TurnedOffProxyChecker, Resources.TurnedOffProxyCheckerDetails, 4, new string[]
        {
            Resources.HttpProxyType,
            Resources.SocksProxyType
        })
        {
        }
    }
}
