﻿using System;
using System.Collections.Generic;
using System.Linq;
using ProxySearch.Common;
using ProxySearch.Console.Code.Interfaces;
using ProxySearch.Console.Code.SearchEngines;
using ProxySearch.Console.Code.Settings;
using ProxySearch.Console.Controls;
using ProxySearch.Console.Properties;
using ProxySearch.Engine.SearchEngines;

namespace ProxySearch.Console.Code.Detectable.SearchEngines
{
    public abstract class ParallelSearchEngineDetectableBase
        : DetectableBase<ISearchEngine, ParallelSearchEngine, ParallelSearchEnginePropertyControl>
    {
        public ParallelSearchEngineDetectableBase(string proxyType, Type urlListSearchEngineDetectableType, Type googleEngineDetectableBaseType)
            : base(Resources.Parallel, Resources.ParallelDescription, 3, proxyType, new List<object>
            {
                new ParametersPair
                {
                        TypeName = urlListSearchEngineDetectableType.AssemblyQualifiedName,
                        Parameters = GetSettingsList(proxyType).Cast<object>().ToList()
                },
                new ParametersPair
                {
                        TypeName =  googleEngineDetectableBaseType.AssemblyQualifiedName,
                        Parameters = GetSettingsList(proxyType).Cast<object>().ToList()
                }
            })
        {
        }

        public static List<ParametersPair> GetSettingsList(string proxyType)
        {
            return Context.Get<IDetectableManager>().Find<ISearchEngine>(proxyType, typeof(ParallelSearchEngineDetectableBase))
                                                    .Select(item =>
                                                    {
                                                        if (typeof(UrlListSearchEngineDetectableBase).IsAssignableFrom(item.GetType()))
                                                            return new ParametersPair
                                                            {
                                                                TypeName = item.GetType().AssemblyQualifiedName,
                                                                Parameters = new List<object>
                                                                {
                                                                    string.Concat("http://proxysearcher.sourceforge.net/ProxyList.php?type=", proxyType.ToLower())
                                                                }
                                                            };

                                                        return new ParametersPair
                                                        {
                                                            TypeName = item.GetType().AssemblyQualifiedName,
                                                            Parameters = item.DefaultSettings
                                                        };
                                                    }).ToList();
        }
    }
}
