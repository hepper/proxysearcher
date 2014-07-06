using System;
using System.Collections.Generic;
using System.Linq;
using ProxySearch.Common;
using ProxySearch.Console.Code.Detectable.SearchEngines;
using ProxySearch.Console.Code.Interfaces;
using ProxySearch.Console.Code.Settings;
using ProxySearch.Engine.SearchEngines;

namespace ProxySearch.Console.Code.UI
{
    public class MixedSearchEngineDataView
    {
        private ParametersPair instance;

        public int SelectedIndex
        {
            get
            {
                List<IDetectable> searchEngines = SearchEngines;  //SearchEngines recreates types after each call therefore cache variable is used here.

                IDetectable selectedEngine = searchEngines.Single(item => item.GetType().AssemblyQualifiedName == instance.TypeName);
                return searchEngines.IndexOf(selectedEngine);

            }
            set
            {
                instance.TypeName = SearchEngines[value].GetType().AssemblyQualifiedName;
            }
        }

        public List<ParametersPair> Settings
        {
            get
            {
                return instance.Parameters.Cast<ParametersPair>().ToList();
            }
        }

        public List<IDetectable> SearchEngines
        {
            get
            {
                return Context.Get<IDetectableManager>()
                              .Find<ISearchEngine>(ProxyType)
                              .Where(item => !typeof(ParallelSearchEngineDetectableBase).IsAssignableFrom(item.GetType()))
                              .ToList();
            }
        }

        public string ProxyType
        {
            get
            {
                return Context.Get<IDetectableManager>().CreateDetectableInstance<ISearchEngine>(instance.TypeName).ProxyType;
            }
        }

        public int OrderIndex
        {
            get;
            private set;
        }

        public MixedSearchEngineDataView(int orderIndex, ParametersPair instance)
        {
            OrderIndex = orderIndex;
            this.instance = instance;
        }
    }
}
