using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using ProxySearch.Common;
using ProxySearch.Console.Code.Detectable;
using ProxySearch.Console.Code.Interfaces;
using ProxySearch.Console.Code.Settings;
using ProxySearch.Engine;
using ProxySearch.Engine.GeoIP;

namespace ProxySearch.Console.Code
{
    public class ProxySearchEngineApplicationFactory
    {
        public async Task<Application> Create(IProxySearchFeedback feedback)
        {
            return await Task.Run(() =>
            {
                Context.Set(new CancellationTokenSource());
                Context.Set(new TaskCounter());

                Context.Get<TaskCounter>().AllCompleted += () =>
                {
                    if (Context.Get<CancellationTokenSource>().IsCancellationRequested)
                    {
                        feedback.OnSearchCancelled();
                    }
                    else
                    {
                        feedback.OnSearchFinished();
                    }
                };

                Context.Get<TaskCounter>().JobCountChanged += feedback.UpdateJobCount;
                
                IDetectable searchEngineDetectable = CreateDetectableInstance<ISearchEngine>(Settings.SelectedTabSettings.SearchEngineDetectableType);
                IDetectable proxyCheckerDetectable = CreateDetectableInstance<IProxyChecker>(Settings.SelectedTabSettings.ProxyCheckerDetectableType);
                IDetectable geoIPDetectable = CreateDetectableInstance<IGeoIP>(Settings.GeoIPDetectableType);

                ISearchEngine searchEngine = CreateImplementationInstance<ISearchEngine>(searchEngineDetectable.Implementation, Settings.SelectedTabSettings.SearchEngineSettings);

                IProxySearcher proxySearcher = new ProxySearcher(feedback,
                                                                    CreateImplementationInstance<IProxyChecker>(proxyCheckerDetectable.Implementation, Settings.SelectedTabSettings.ProxyCheckerSettings),
                                                                    CreateImplementationInstance<IGeoIP>(geoIPDetectable.Implementation, Settings.GeoIPSettings));

                return new Application(searchEngine, proxySearcher);
            });
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

        private T CreateImplementationInstance<T>(Type type, List<ParametersPair> parametersList)
        {
            try
            {
                return (T)Activator.CreateInstance(type, parametersList.Single(item => item.TypeName == type.AssemblyQualifiedName).Parameters.ToArray());
            }
            catch (TargetInvocationException exception)
            {
                throw exception.InnerException;
            }
        }
    }
}
