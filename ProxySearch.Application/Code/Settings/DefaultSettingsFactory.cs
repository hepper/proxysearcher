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
using ProxySearch.Engine;
using ProxySearch.Engine.GeoIP;

namespace ProxySearch.Console.Code.Settings
{
    public class DefaultSettingsFactory
    {
        public AllSettings Create()
        {
            AllSettings settings = new AllSettings()
            {
                CheckUpdates = true,
                GeoIPDetectableType = typeof(GeoIPDetectable).AssemblyQualifiedName,
                GeoIPSettings = GetSettings<IGeoIP>(),
                MaxThreadCount = 500,
                TabSettings = new ObservableCollection<TabSettings>()
                {
                    CreateTabSettings("HTTP", new Guid("0EBFAAA5-C241-4560-822C-0E2429F3F03C"))
                }
            };

            settings.SelectedTabSettingsId = settings.TabSettings[0].Id;
            return settings;
        }

        public TabSettings CreateTabSettings()
        {
            return CreateTabSettings(Resources.DefaultTabName, Guid.NewGuid());
        }

        private TabSettings CreateTabSettings(string name, Guid guid)
        {
            return new TabSettings()
            {
                Id = guid,
                Name = name,
                ProxyCheckerDetectableType = typeof(ProxyCheckerByUrlDetectable).AssemblyQualifiedName,
                SearchEngineDetectableType = typeof(GoogleEngineDetectable).AssemblyQualifiedName,
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
