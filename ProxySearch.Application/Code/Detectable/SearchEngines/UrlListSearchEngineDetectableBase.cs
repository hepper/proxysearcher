using System.Collections.Generic;
using ProxySearch.Console.Controls;
using ProxySearch.Console.Properties;
using ProxySearch.Engine.SearchEngines;

namespace ProxySearch.Console.Code.Detectable.SearchEngines
{
    public abstract class UrlListSearchEngineDetectableBase : DetectableBase<ISearchEngine, UrlListSearchEngine, UrlListPropertyControl>
    {
        public UrlListSearchEngineDetectableBase(string proxyType, List<object> defaultSettings)
            : base(Resources.UrlListEngine, Resources.UrlListEngineDescription, 2, proxyType, defaultSettings)
        {
        }
    }
}
