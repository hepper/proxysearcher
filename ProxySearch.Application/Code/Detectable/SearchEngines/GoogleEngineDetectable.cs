using System;
using ProxySearch.Console.Code.Interfaces;
using ProxySearch.Console.Properties;
using ProxySearch.Engine;
using ProxySearch.Engine.Google;
using System.Windows.Controls;
using ProxySearch.Console.Controls;
using System.Collections.Generic;

namespace ProxySearch.Console.Code.Detectable.SearchEngines
{
    public class GoogleEngineDetectable : IDetectable
    {
        public string FriendlyName
        {
            get
            {
                return Resources.GoogleDotCom;
            }
        }

        public string Description
        {
            get
            {
                return Resources.GoogleEngineDescription;
            }
        }

        public Type Interface
        {
            get
            {
                return typeof(ISearchEngine);
            }
        }

        public Type Implementation
        {
            get
            {
                return typeof(GoogleSearchEngine);
            }
        }

        public Type PropertyPage
        {
            get
            {
                return typeof(GoogleEnginePropertyControl);
            }
        }

        public List<object> DefaultSettings
        {
            get
            {
                return new List<object>
                {
                    20,
                    "http proxy list 3128"
                };
            }
        }
    }
}
