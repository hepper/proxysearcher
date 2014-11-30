﻿using System;
using System.Linq;
using System.Threading.Tasks;
using ProxySearch.Common;
using ProxySearch.Console.Code.Interfaces;
using ProxySearch.Console.Code.Settings;
using ProxySearch.Engine.SearchEngines;

namespace ProxySearch.Console.Code.SearchEngines
{
    public class ParallelSearchEngine : ProxySearch.Engine.SearchEngines.ParallelSearchEngine
    {
        public ParallelSearchEngine(params ParametersPair[] arguments)
            : base(arguments.Select(item =>
            {
                IDetectable detectable = Context.Get<IDetectableManager>().CreateDetectableInstance<ISearchEngine>(item.TypeName);

                return Context.Get<IDetectableManager>().CreateImplementationInstance<ISearchEngine>(detectable,
                                                                                                     item.Parameters.Cast<ParametersPair>().ToList(),

                                                                                                    detectable.InterfaceSettings);
            }).ToArray())
        {
        }

        public Task<Uri> GetNext()
        {
            throw new NotSupportedException();
        }

        public string Status
        {
            get
            {
                throw new NotSupportedException();
            }
        }
    }
}
