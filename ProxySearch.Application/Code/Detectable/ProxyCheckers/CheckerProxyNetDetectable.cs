using System;
using System.Collections.Generic;
using ProxySearch.Console.Code.Interfaces;
using ProxySearch.Console.Controls;
using ProxySearch.Console.Properties;
using ProxySearch.Engine;
using ProxySearch.Engine.CheckerProxy.Net;

namespace ProxySearch.Console.Code.Detectable.ProxyCheckers
{
    public class CheckerProxyNetDetectable : IDetectable
    {
        public string FriendlyName
        {
            get 
            {
                return Resources.CheckerProxyDotNet;
            }
        }

        public string Description
        {
            get 
            {
                return Resources.CheckerProxyDotNetDescription;
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
                return typeof(CheckerProxyNet);
            }
        }

        public Type PropertyPage
        {
            get 
            {
                return typeof(CheckerProxyNetPropertyControl);
            }
        }

        public List<object> DefaultSettings
        {
            get 
            {
                return new List<object>
                {
                    20
                };
            }
        }
    }
}
