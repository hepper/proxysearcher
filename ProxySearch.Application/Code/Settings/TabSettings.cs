using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ProxySearch.Console.Code.Settings
{
    public class TabSettings
    {
        public TabSettings()
        {
            SearchEngineSettings = new List<ParametersPair>();
            ProxyCheckerSettings = new List<ParametersPair>();
        }

        public Guid Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string SearchEngineDetectableType
        {
            get;
            set;
        }

        public string ProxyCheckerDetectableType
        {
            get;
            set;
        }

        public List<ParametersPair> SearchEngineSettings
        {
            get;
            set;
        }

        public List<ParametersPair> ProxyCheckerSettings
        {
            get;
            set;
        }
    }
}
