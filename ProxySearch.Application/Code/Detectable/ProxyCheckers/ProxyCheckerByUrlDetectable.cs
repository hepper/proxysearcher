using System;
using ProxySearch.Console.Code.Interfaces;
using ProxySearch.Console.Properties;
using ProxySearch.Engine;
using System.Windows.Controls;
using ProxySearch.Console.Controls;
using System.Collections.Generic;

namespace ProxySearch.Console.Code.Detectable.ProxyCheckers
{
    public class ProxyCheckerByUrlDetectable : IDetectable
    {
        public string FriendlyName
        {
            get
            {
                return Resources.ProxyCheckerByUrl;
            }
        }

        public string Description
        {
            get
            {
                return Resources.ProxyCheckerByUrlDescription;
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
                return typeof(ProxyCheckerByUrl);
            }
        }

        public List<object> DefaultSettings
        {
            get
            {
                return new List<object>
                {
                    Resources.SimpleSite,
                    0.1
                };
            }
        }

        public Type PropertyPage
        {
            get
            {
                return typeof(ProxyCheckerByUrlControl);
            }
        }
    }
}
