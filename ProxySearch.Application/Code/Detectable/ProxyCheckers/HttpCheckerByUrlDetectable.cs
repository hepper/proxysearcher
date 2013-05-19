using ProxySearch.Console.Properties;
using ProxySearch.Engine.Checkers;

namespace ProxySearch.Console.Code.Detectable.ProxyCheckers
{
    public class HttpCheckerByUrlDetectable : CheckerByUrlDetectableBase<HttpProxyCheckerByUrl>
    {
        public HttpCheckerByUrlDetectable():base(Resources.HttpProxyType)
        {
        }
    }
}
