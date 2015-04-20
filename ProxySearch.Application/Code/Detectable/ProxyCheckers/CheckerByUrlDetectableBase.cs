using System.Collections.Generic;
using ProxySearch.Console.Controls;
using ProxySearch.Console.Properties;
using ProxySearch.Engine.Checkers;

namespace ProxySearch.Console.Code.Detectable.ProxyCheckers
{
    public abstract class CheckerByUrlDetectableBase<ProxyCheckerType>: DetectableBase<IProxyChecker, ProxyCheckerType, ProxyCheckerByUrlControl>
       where ProxyCheckerType: IProxyChecker
    {
        public CheckerByUrlDetectableBase(string proxyType, string websiteUrl)
            : base(Resources.ProxyCheckerByUrl, Resources.ProxyCheckerByUrlDescription, 0, proxyType, new List<object> 
            { 
                 websiteUrl,
                 0.0,
                 Constants.DefaultSettings.MaxTasksCount.Value
            })
        {
        }
    }
}