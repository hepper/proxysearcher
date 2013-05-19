using ProxySearch.Console.Properties;

namespace ProxySearch.Console.Code.Detectable.SearchEngines
{
    public class SOCKSGoogleEngineDetectable : GoogleEngineDetectableBase
    {
        public SOCKSGoogleEngineDetectable()
            : base(Resources.SocksProxyType, "socks proxy list 1080")
        {
        }
    }
}
