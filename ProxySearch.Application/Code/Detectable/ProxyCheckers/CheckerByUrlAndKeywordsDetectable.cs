using System;
using System.Collections.Generic;
using ProxySearch.Console.Code.Interfaces;
using ProxySearch.Console.Controls;
using ProxySearch.Console.Properties;
using ProxySearch.Engine.Checkers;

namespace ProxySearch.Console.Code.Detectable.ProxyCheckers
{
    public class CheckerByUrlAndKeywordsDetectable : IDetectable
    {
        public string FriendlyName
        {
            get 
            {
                return Resources.ProxyCheckerByUrlAndKeywords;
            }
        }

        public string Description
        {
            get 
            {
                return Resources.ProxyCheckerByUrlAndKeywordsDescription;
            }
        }

        public Type Interface
        {
            get 
            {
                return typeof(IProxyChecker);
            }
        }

        public Type Implementation
        {
            get 
            {
                return typeof(ProxyCheckerByUrlAndKeywords);
            }
        }

        public Type PropertyPage
        {
            get 
            {
                return typeof(CheckerByUrlAndKeywordsControl);
            }
        }

        public List<object> DefaultSettings
        {
            get 
            {
                return new List<object>
                {
                    Resources.GoogleDotCom,
                    Resources.GoogleDotComContent
                };
            }
        }

        public List<object> InterfaceSettings
        {
            get 
            { 
                return new List<object>(); 
            }
        }
    }
}
