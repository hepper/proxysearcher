using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Handlers;
using System.Reflection;
using System.Threading;
using ProxySearch.Common;
using ProxySearch.Console.Code.Detectable;
using ProxySearch.Console.Code.Interfaces;
using ProxySearch.Console.Code.Settings;
using ProxySearch.Console.Properties;
using ProxySearch.Engine;
using ProxySearch.Engine.Checkers;
using ProxySearch.Engine.DownloaderContainers;
using ProxySearch.Engine.GeoIP;
using ProxySearch.Engine.SearchEngines;
using ProxySearch.Engine.SearchEngines.FolderSearch;
using ProxySearch.Engine.Socks;

namespace ProxySearch.Console.Code
{
    public class ProxySearchEngineApplicationFactory
    {
        public Application Create(ProxySearchFeedback feedback)
        {
            Context.Set(new CancellationTokenSource());
            Context.Set<IHttpDownloaderContainer>(HttpDownloaderContainer);

            IDetectable searchEngineDetectable = CreateDetectableInstance<ISearchEngine>(Settings.SelectedTabSettings.SearchEngineDetectableType);
            IDetectable proxyCheckerDetectable = CreateDetectableInstance<IProxyChecker>(Settings.SelectedTabSettings.ProxyCheckerDetectableType);
            IDetectable geoIPDetectable = CreateDetectableInstance<IGeoIP>(Settings.GeoIPDetectableType);
            ISearchEngine searchEngine = CreateImplementationInstance<ISearchEngine>(searchEngineDetectable,
                                                                                     Settings.SelectedTabSettings.SearchEngineSettings,
                                                                                     searchEngineDetectable.InterfaceSettings);
            feedback.ExportAllowed = !(searchEngine is FolderSearchEngine);

            return new Application(searchEngine,
                                   new ProxyParser(Context.Get<IBlackList>()),
                                   feedback,
                                   CreateImplementationInstance<IProxyChecker>(proxyCheckerDetectable,
                                                                               Settings.SelectedTabSettings.ProxyCheckerSettings,
                                                                               proxyCheckerDetectable.InterfaceSettings),
                                   CreateImplementationInstance<IGeoIP>(geoIPDetectable,
                                                                        Settings.GeoIPSettings,
                                                                        geoIPDetectable.InterfaceSettings));
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

        private IDetectable CreateDetectableInstance<T>(string typeName)
        {
            try
            {
                return (IDetectable)Activator.CreateInstance(Type.GetType(typeName, true));
            }
            catch
            {
                return CreateDetectableInstance<T>(Context.Get<IDetectableSearcher>().Get<T>().First().GetType().AssemblyQualifiedName);
            }
        }

        private T CreateImplementationInstance<T>(IDetectable detectable, List<ParametersPair> parametersList, List<object> interfacesList)
        {
            try
            {
                Type type = detectable.Implementation;
                ParametersPair parameterPair = parametersList.SingleOrDefault(item => item.TypeName == detectable.GetType().AssemblyQualifiedName);

                if (parameterPair == null && !interfacesList.Any())
                    return (T)Activator.CreateInstance(type);
                else
                {
                    List<object> parameters = new List<object>();
                    if (parameterPair != null)
                        parameters.AddRange(parameterPair.Parameters);

                    parameters.AddRange(interfacesList);

                    return (T)Activator.CreateInstance(type, parameters.ToArray());
                }
            }
            catch (TargetInvocationException exception)
            {
                throw exception.InnerException;
            }
        }
    }
}
