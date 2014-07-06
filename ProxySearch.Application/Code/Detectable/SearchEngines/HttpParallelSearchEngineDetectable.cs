using ProxySearch.Console.Properties;

namespace ProxySearch.Console.Code.Detectable.SearchEngines
{
    public class HttpParallelSearchEngineDetectable : ParallelSearchEngineDetectableBase
    {
        public HttpParallelSearchEngineDetectable()
            : base(Resources.HttpProxyType, typeof(HttpUrlListSearchEngineDetectable), typeof(HttpGoogleEngineDetectable))
        {
        }
    }
}
