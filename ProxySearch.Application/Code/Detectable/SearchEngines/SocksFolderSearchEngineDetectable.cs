using System.Collections.Generic;
using ProxySearch.Console.Controls;
using ProxySearch.Console.Properties;
using ProxySearch.Engine.SearchEngines;
using ProxySearch.Engine.SearchEngines.FolderSearch;

namespace ProxySearch.Console.Code.Detectable.SearchEngines
{
    public class SocksFolderSearchEngineDetectable : DetectableBase<ISearchEngine, FolderSearchEngine, FolderSearchEngineControl>
    {
        public SocksFolderSearchEngineDetectable()
            : base(Resources.FolderSearchEngine, Resources.FolderSearchEngineDescription,
                   1, Resources.SocksProxyType, new List<object> { Constants.DefaultExportFolder.Socks.Location })
        {
        }
    }
}
