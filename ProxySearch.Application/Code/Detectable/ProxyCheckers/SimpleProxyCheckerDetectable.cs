using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProxySearch.Console.Code.Interfaces;
using ProxySearch.Console.Properties;
using ProxySearch.Engine;
using System.Windows.Controls;

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
