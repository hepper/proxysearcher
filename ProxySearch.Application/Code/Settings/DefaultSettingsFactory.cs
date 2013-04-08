using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ProxySearch.Common;
using ProxySearch.Console.Code.Detectable;
using ProxySearch.Console.Code.Detectable.GeoIPs;
using ProxySearch.Console.Code.Detectable.ProxyCheckers;
using ProxySearch.Console.Code.Detectable.SearchEngines;
using ProxySearch.Console.Properties;
using ProxySearch.Engine.Checkers;
using ProxySearch.Engine.GeoIP;
using ProxySearch.Engine.SearchEngines;

namespace ProxySearch.Console.Code.Settings
{
    public class DefaultSettingsFactory
    {
        public AllSettings Create()
        {
            AllSettings settings = new AllSettings()
            {
                CheckUpdates = true,
                PageSize = 20,
                GeoIPDetectableType = typeof(BuildInGeoIPDetectable).AssemblyQualifiedName,
                GeoIPSettings = GetSettings<IGeoIP>(),
                MaxThreadCount = 500,
                TabSettings = new ObservableCollection<TabSettings>()
                {
                    CreateHttpTabSettings(Resources.Google, new Guid("0EBFAAA5-C241-4560-822C-0E2429F3F03C")),
                    CreateOpenTabSettings()
                }
            };

            settings.ExportSettings.ExportSearchResult = true;
            settings.SelectedTabSettingsId = settings.TabSettings[0].Id;
            return settings;
        }

        public TabSettings CreateHttpTabSettings()
        {
            return CreateHttpTabSettings(Resources.DefaultTabName, Guid.NewGuid());
        }

        private TabSettings CreateHttpTabSettings(string name, Guid guid)
        {
            return new TabSettings()
            {
                Id = guid,
                Name = name,
                ProxyCheckerDetectableType = typeof(CheckerByUrlDetectable).AssemblyQualifiedName,
                SearchEngineDetectableType = typeof(GoogleEngineDetectable).AssemblyQualifiedName,
                SearchEngineSettings = GetSettings<ISearchEngine>(),
                ProxyCheckerSettings = GetSettings<IProxyChecker>()
            };
        }

        private TabSettings CreateOpenTabSettings()
        {
            return new TabSettings()
            {
                Id = new Guid("D187270B-A4B2-4B47-A7A7-26DF26FD2EF1"),
                Name = Resources.Open,
                ProxyCheckerDetectableType = typeof(TurnedOffProxyCheckerDetectable).AssemblyQualifiedName,
                SearchEngineDetectableType = typeof(FolderSearchEngineDetectable).AssemblyQualifiedName,
                SearchEngineSettings = GetSettings<ISearchEngine>(),
                ProxyCheckerSettings = GetSettings<IProxyChecker>()
            };
        }

        private List<ParametersPair> GetSettings<T>()
        {
            return Context.Get<IDetectableSearcher>().Get<T>().Select(item => new ParametersPair
            {
                TypeName = item.Implementation.AssemblyQualifiedName,
                Parameters = item.DefaultSettings
            }).ToList();
        }
    }
}
