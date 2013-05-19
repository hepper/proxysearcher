using System.Collections.Generic;
using ProxySearch.Console.Controls;
using ProxySearch.Console.Properties;
using ProxySearch.Engine.Checkers;
using ProxySearch.Engine.Checkers.CheckerProxy.Net;

namespace ProxySearch.Console.Code.Detectable.ProxyCheckers
{
    public class CheckerProxyNetDetectable : DetectableBase<IProxyChecker, CheckerProxyNet, CheckerProxyNetPropertyControl>
    {
        public CheckerProxyNetDetectable()
            : base(Resources.CheckerProxyDotNet, Resources.CheckerProxyDotNetDescription, 2, new string[] { Resources.HttpProxyType }, new List<object> { 20, 10 })
        {
        }
    }
}
