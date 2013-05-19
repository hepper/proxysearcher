using ProxySearch.Console.Properties;
using ProxySearch.Engine.Checkers;

namespace ProxySearch.Console.Code.Detectable.ProxyCheckers
{
    public class SocksCheckerByUrlDetectable : CheckerByUrlDetectableBase<SocksProxyCheckerByUrl>
    {
        public SocksCheckerByUrlDetectable():base(Resources.SocksProxyType)
        {
        }
    }
}
