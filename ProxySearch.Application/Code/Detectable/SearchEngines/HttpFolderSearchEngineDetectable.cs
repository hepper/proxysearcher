using System.Collections.Generic;
using ProxySearch.Console.Controls;
using ProxySearch.Console.Properties;
using ProxySearch.Engine.SearchEngines;
using ProxySearch.Engine.SearchEngines.FolderSearch;

namespace ProxySearch.Console.Code.Detectable.SearchEngines
{
    public class HttpFolderSearchEngineDetectable : DetectableBase<ISearchEngine, FolderSearchEngine, FolderSearchEngineControl>
    {
        public HttpFolderSearchEngineDetectable()
            : base(Resources.FolderSearchEngine, Resources.FolderSearchEngineDescription,
                   1, Resources.HttpProxyType, new List<object> { Constants.DefaultExportFolder.Http.Location })
        {
        }
    }
}
