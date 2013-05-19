using ProxySearch.Console.Properties;

namespace ProxySearch.Console.Code.Detectable.SearchEngines
{
    public class HTTPGoogleEngineDetectable : GoogleEngineDetectableBase
    {
        public HTTPGoogleEngineDetectable():base(Resources.HttpProxyType, "http proxy list 3128")
        {

        }
    }
}
