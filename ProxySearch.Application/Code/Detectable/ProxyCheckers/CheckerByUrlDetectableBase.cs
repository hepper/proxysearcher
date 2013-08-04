using System;
using System.Collections.Generic;
using ProxySearch.Console.Controls;
using ProxySearch.Console.Properties;
using ProxySearch.Engine.Checkers;

namespace ProxySearch.Console.Code.Detectable.ProxyCheckers
{
    public abstract class CheckerByUrlDetectableBase<ProxyCheckerType>: SimpleDetectableBase<IProxyChecker, ProxyCheckerType>
       where ProxyCheckerType: IProxyChecker
    {
        public CheckerByUrlDetectableBase(string proxyType, string websiteUrl)
            : base(Resources.ProxyCheckerByUrl, Resources.ProxyCheckerByUrlDescription, 0, proxyType)
        {
            WebsiteUrl = websiteUrl;
        }

        public override List<object> DefaultSettings
        {
            get
            {
                return new List<object>
                {
                    WebsiteUrl,
                    0.1
                };
            }
        }

        public override Type PropertyPage
        {
            get
            {
                return typeof(ProxyCheckerByUrlControl);
            }
        }

        private string WebsiteUrl
        {
            get;
            set;
        }
    }
}
