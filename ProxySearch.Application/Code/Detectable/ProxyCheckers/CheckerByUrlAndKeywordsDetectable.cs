using System.Collections.Generic;
using ProxySearch.Console.Controls;
using ProxySearch.Console.Properties;
using ProxySearch.Engine.Checkers;

namespace ProxySearch.Console.Code.Detectable.ProxyCheckers
{
    public class CheckerByUrlAndKeywordsDetectable : DetectableBase<IProxyChecker, ProxyCheckerByUrlAndKeywords, CheckerByUrlAndKeywordsControl>
    {
        public CheckerByUrlAndKeywordsDetectable()
            : base(Resources.ProxyCheckerByUrlAndKeywords, Resources.ProxyCheckerByUrlAndKeywordsDescription, 2, new List<object>
                {
                    Resources.GoogleDotCom,
                    Resources.GoogleDotComContent
                })
        {
        }
    }
}
