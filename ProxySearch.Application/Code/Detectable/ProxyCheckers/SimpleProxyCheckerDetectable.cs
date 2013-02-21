using System;
using ProxySearch.Console.Properties;
using ProxySearch.Engine.Checkers;

namespace ProxySearch.Console.Code.Detectable.ProxyCheckers
{
    public class SimpleProxyCheckerDetectable : SimpleDetectable
    {
        public override string FriendlyName
        {
            get
            {
                return Resources.SimpleProxyChecker;
            }
        }

        public override string Description
        {
            get 
            {
                return Resources.SimpleProxyCheckerDescription; 
            }
        }

        public override Type Interface
        {
            get
            {
                return typeof(IProxyChecker);
            }
        }

        public override Type Implementation
        {
            get
            {
                return typeof(SimpleProxyChecker);
            }
        }
    }
}
