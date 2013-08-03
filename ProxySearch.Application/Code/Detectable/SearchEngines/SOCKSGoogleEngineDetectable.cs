using ProxySearch.Console.Properties;

namespace ProxySearch.Console.Code.Detectable.SearchEngines
{
    public class SocksGoogleEngineDetectable : GoogleEngineDetectableBase
    {
        public SocksGoogleEngineDetectable()
            : base(Resources.SocksProxyType, "socks proxy list 1080")
        {
        }
    }
}
