using System.Net.Http;
using System.Net.Http.Handlers;
using System.Threading;
using ProxySearch.Console.Code.Interfaces;
using ProxySearch.Console.Code.Settings;
using ProxySearch.Console.Properties;
using ProxySearch.Engine;
using ProxySearch.Engine.Checkers;
using ProxySearch.Engine.DownloaderContainers;
using ProxySearch.Engine.Error;
using ProxySearch.Engine.GeoIP;
using ProxySearch.Engine.Parser;
using ProxySearch.Engine.SearchEngines;
using ProxySearch.Engine.Socks;
using ProxySearch.Engine.Tasks;

namespace ProxySearch.Console.Code
{
    public class ProxySearchEngineApplicationFactory
    {
        public Application Create(TaskItem task, ProxySearchFeedback feedback)
        {
            Context.Set(new CancellationTokenSource());
            Context.Set<IHttpDownloaderContainer>(HttpDownloaderContainer);

            task.UpdateDetails(Resources.ReadingConfigurationOfSelectedSearch);

            IDetectable searchEngineDetectable = DetectableManager.CreateDetectableInstance<ISearchEngine>(Settings.SelectedTabSettings.SearchEngineDetectableType);
            IDetectable proxyCheckerDetectable = DetectableManager.CreateDetectableInstance<IProxyChecker>(Settings.SelectedTabSettings.ProxyCheckerDetectableType);
            IDetectable geoIPDetectable = DetectableManager.CreateDetectableInstance<IGeoIP>(Settings.GeoIPDetectableType);
            ISearchEngine searchEngine = DetectableManager.CreateImplementationInstance<ISearchEngine>(searchEngineDetectable,
                                                                                                       Settings.SelectedTabSettings.SearchEngineSettings,
                                                                                                       searchEngineDetectable.InterfaceSettings);

            feedback.ExportAllowed = !(searchEngine is FolderSearchEngine);

            task.UpdateDetails(Resources.PreparingProxyProvider);

            IProxyProvider proxyProvider =  new ProxyProvider(Context.Get<IBlackList>(), new ParseMethodsProvider(Settings.ParseDetails));

            task.UpdateDetails(Resources.PreparingProxyChecker);

            IProxyChecker proxyChecker = DetectableManager.CreateImplementationInstance<IProxyChecker>(proxyCheckerDetectable,
                                                                                                       Settings.SelectedTabSettings.ProxyCheckerSettings,
                                                                                                       proxyCheckerDetectable.InterfaceSettings);
            task.UpdateDetails(Resources.PreparingGeoIpService);

            IGeoIP geoIP = DetectableManager.CreateImplementationInstance<IGeoIP>(geoIPDetectable,
                                                                                  Settings.GeoIPSettings,
                                                                                  geoIPDetectable.InterfaceSettings);

            task.UpdateDetails(Resources.PreparingApplication);

            Application application = new Application(searchEngine, proxyChecker, HttpDownloaderContainer, geoIP, proxyProvider, 
                                                      Context.Get<ITaskManager>(), Context.Get<IErrorFeedback>());

            application.ProxyAlive += feedback.OnAliveProxy;

            return application;
        }

        private IHttpDownloaderContainer HttpDownloaderContainer
        {
            get
            {
                if (Settings.SelectedTabSettings.ProxyType == Resources.SocksProxyType)
                {
                    return new HttpDownloaderContainer<SocksHttpClientHandler, SocksProgressMessageHandler>();
                }

                return new HttpDownloaderContainer<HttpClientHandler, ProgressMessageHandler>();
            }
        }

        private AllSettings Settings
        {
            get
            {
                return Context.Get<AllSettings>();
            }
        }

        private IDetectableManager DetectableManager
        {
            get 
            {
                return Context.Get<IDetectableManager>(); 
            }
        }
    }
}
