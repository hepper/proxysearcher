using System.Collections.Generic;
using ProxySearch.Console.Controls;
using ProxySearch.Console.Properties;
using ProxySearch.Engine.Checkers;
using ProxySearch.Engine.ProxyDetailsProvider;

namespace ProxySearch.Console.Code.Detectable.ProxyCheckers
{
    public abstract class SimpleProxyCheckerDetectableBase<ProxyDetailsProviderType> : DetectableBase<IProxyChecker, SimpleProxyChecker<ProxyDetailsProviderType>, CheckerByIpAndPortControl>
         where ProxyDetailsProviderType : IProxyDetailsProvider, new()
    {
        public SimpleProxyCheckerDetectableBase(string proxyType)
            : base(Resources.SimpleProxyChecker, 
                   Resources.SimpleProxyCheckerDescription, 
                   3, 
                   proxyType, 
                   new List<object>()
                   {
                       Constants.DefaultSettings.MaxTasksCount.Value
                   })
        {}
    }
}