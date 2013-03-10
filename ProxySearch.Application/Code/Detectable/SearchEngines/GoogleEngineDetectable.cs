using System;
using System.Collections.Generic;
using ProxySearch.Common;
using ProxySearch.Console.Code.Interfaces;
using ProxySearch.Console.Controls;
using ProxySearch.Console.Properties;
using ProxySearch.Engine;
using ProxySearch.Engine.Google;

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

        public List<object> InterfaceSettings
        {
            get
            {
                return new List<object>()
                {
                    Context.Get<ICaptchaWindow>()
                };
            }
        }
    }
}
