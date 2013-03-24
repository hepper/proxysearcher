using System.Collections.Generic;
using ProxySearch.Console.Controls;
using ProxySearch.Console.Properties;
using ProxySearch.Engine.SearchEngines;
using ProxySearch.Engine.SearchEngines.FolderSearch;

namespace ProxySearch.Console.Code.Detectable.SearchEngines
{
    public class FolderSearchEngineDetectable : DetectableBase<ISearchEngine, FolderSearchEngine, FolderSearchEngineControl>
    {
        public FolderSearchEngineDetectable()
            : base(Resources.FolderSearchEngine, Resources.FolderSearchEngineDescription, 1, new List<object> { Constants.DefaultExportFolder.Location })
        {
        }
    }
}
