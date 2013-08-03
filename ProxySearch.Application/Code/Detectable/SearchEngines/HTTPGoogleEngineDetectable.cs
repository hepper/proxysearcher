using ProxySearch.Console.Properties;

namespace ProxySearch.Console.Code.Detectable.SearchEngines
{
    public class HttpGoogleEngineDetectable : GoogleEngineDetectableBase
    {
        public HttpGoogleEngineDetectable():base(Resources.HttpProxyType, "http proxy list 3128")
        {

        }
    }
}
