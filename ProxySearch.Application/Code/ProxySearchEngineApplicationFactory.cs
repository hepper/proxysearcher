using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using ProxySearch.Common;
using ProxySearch.Console.Code.Detectable;
using ProxySearch.Console.Code.Interfaces;
using ProxySearch.Console.Code.Settings;
using ProxySearch.Engine;
using ProxySearch.Engine.Checkers;
using ProxySearch.Engine.GeoIP;
using ProxySearch.Engine.SearchEngines;
using ProxySearch.Engine.SearchEngines.FolderSearch;

namespace ProxySearch.Console.Code
{
    public class ProxySearchEngineApplicationFactory
    {
        public Application Create(ProxySearchFeedback feedback)
        {
            Context.Set(new CancellationTokenSource());
            Context.Set<Downloader>(new Downloader());

            IDetectable searchEngineDetectable = CreateDetectableInstance<ISearchEngine>(Settings.SelectedTabSettings.SearchEngineDetectableType);
            IDetectable proxyCheckerDetectable = CreateDetectableInstance<IProxyChecker>(Settings.SelectedTabSettings.ProxyCheckerDetectableType);
            IDetectable geoIPDetectable = CreateDetectableInstance<IGeoIP>(Settings.GeoIPDetectableType);

            ISearchEngine searchEngine = CreateImplementationInstance<ISearchEngine>(searchEngineDetectable.Implementation,
                                                                                     Settings.SelectedTabSettings.SearchEngineSettings,
                                                                                     searchEngineDetectable.InterfaceSettings);
            feedback.ExportAllowed = !(searchEngine is FolderSearchEngine);

            return new Application(searchEngine,
                                   new ProxyParser(Context.Get<IBlackList>()),
                                   feedback,
                                   CreateImplementationInstance<IProxyChecker>(proxyCheckerDetectable.Implementation,
                                                                               Settings.SelectedTabSettings.ProxyCheckerSettings,
                                                                               proxyCheckerDetectable.InterfaceSettings),
                                   CreateImplementationInstance<IGeoIP>(geoIPDetectable.Implementation,
                                                                        Settings.GeoIPSettings,
                                                                        geoIPDetectable.InterfaceSettings));
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

        private T CreateImplementationInstance<T>(Type type, List<ParametersPair> parametersList, List<object> interfacesList)
        {
            try
            {
                ParametersPair parameterPair = parametersList.SingleOrDefault(item => item.TypeName == type.AssemblyQualifiedName);

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
