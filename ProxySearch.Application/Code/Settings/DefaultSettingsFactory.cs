using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ProxySearch.Common;
using ProxySearch.Console.Code.Detectable;
using ProxySearch.Console.Code.Detectable.GeoIPs;
using ProxySearch.Console.Code.Detectable.ProxyCheckers;
using ProxySearch.Console.Code.Detectable.SearchEngines;
using ProxySearch.Console.Code.Interfaces;
using ProxySearch.Console.Code.Language;
using ProxySearch.Console.Properties;
using ProxySearch.Engine.Checkers;
using ProxySearch.Engine.GeoIP;
using ProxySearch.Engine.Parser;
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
                    CreateTabSettings<HttpGoogleEngineDetectable, HttpCheckerByUrlDetectable>("0EBFAAA5-C241-4560-822C-0E2429F3F03C", Resources.Google, Resources.HttpProxyType),
                    CreateTabSettings<HttpUrlListSearchEngineDetectable, HttpCheckerByUrlDetectable>("7793FED8-36EC-4545-9D9F-8D70A12D311C", Resources.HttpPredefinedUrlList, Resources.HttpProxyType),
                    CreateTabSettings<HttpFolderSearchEngineDetectable, HttpTurnedOffProxyCheckerDetectable>("D187270B-A4B2-4B47-A7A7-26DF26FD2EF1", Resources.HttpOpen, Resources.HttpProxyType),
                    CreateTabSettings<SocksGoogleEngineDetectable, SocksCheckerByUrlDetectable>("29D8044B-FFC1-4FF1-AC8A-150FFEC365CE", Resources.SocksGoogleSearchType, Resources.SocksProxyType),
                    CreateTabSettings<SocksUrlListSearchEngineDetectable, SocksCheckerByUrlDetectable>("2EC5BA2B-3C11-488D-91E4-9D25D7240406", Resources.SocksPredefinedUrlList, Resources.SocksProxyType),
                    CreateTabSettings<SocksFolderSearchEngineDetectable, SocksTurnedOffProxyCheckerDetectable>("E15FA042-122A-4A88-8745-B146F4389FEF", Resources.SocksOpen, Resources.SocksProxyType),
                }
            };

            settings.ExportSettings.ExportSearchResult = true;
            settings.SelectedTabSettingsId = settings.TabSettings[0].Id;
            settings.ParseDetails = new DefaultParseDetails().ParseDetailsList;
 
            return settings;
        }

        public TabSettings CreateDefaultTabSettings()
        {
            return CreateTabSettings<HttpGoogleEngineDetectable, HttpCheckerByUrlDetectable>(Guid.NewGuid(), Resources.DefaultTabName, Resources.HttpProxyType);
        }

        private TabSettings CreateTabSettings<SearchEngineType, ProxyCheckerType>(string id, string name, string proxyType)
            where SearchEngineType : IDetectable
            where ProxyCheckerType : IDetectable
        {
            return CreateTabSettings<SearchEngineType, ProxyCheckerType>(new Guid(id), name, proxyType);
        }

        private TabSettings CreateTabSettings<SearchEngineType, ProxyCheckerType>(Guid id, string name, string proxyType)
            where SearchEngineType : IDetectable
            where ProxyCheckerType : IDetectable
        {
            return new TabSettings()
            {
                Id = id,
                Name = name,
                ProxyType = proxyType,
                SearchEngineDetectableType = typeof(SearchEngineType).AssemblyQualifiedName,
                ProxyCheckerDetectableType = typeof(ProxyCheckerType).AssemblyQualifiedName,
                SearchEngineSettings = GetSettings<ISearchEngine>(),
                ProxyCheckerSettings = GetSettings<IProxyChecker>()
            };
        }

        private List<ParametersPair> GetSettings<T>()
        {
            return Context.Get<IDetectableSearcher>().Get<T>().Select(item => new ParametersPair
            {
                TypeName = item.GetType().AssemblyQualifiedName,
                Parameters = item.DefaultSettings
            }).ToList();
        }
    }
}
