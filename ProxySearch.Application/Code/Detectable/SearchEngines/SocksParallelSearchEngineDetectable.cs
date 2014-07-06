using ProxySearch.Console.Properties;

namespace ProxySearch.Console.Code.Detectable.SearchEngines
{
    public class SocksParallelSearchEngineDetectable : ParallelSearchEngineDetectableBase
    {
        public SocksParallelSearchEngineDetectable()
            : base(Resources.SocksProxyType, typeof(SocksUrlListSearchEngineDetectable), typeof(SocksGoogleEngineDetectable))
        {
        }
    }
}
