using System;
using ProxySearch.Engine.Properties;

namespace ProxySearch.Engine.Parser
{
    public class ParseDetails
    {
        private static readonly string ipRegexValue =
                                         @"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}";
        private static readonly string portRegexValue = @"[0-9]+";

        public string Url
        {
            get;
            set;
        }
 
        public string RawRegularExpression
        {
            get;
            set;
        }

        public string Code
        {
            get;
            set;
        } 

        public string RegularExpression
        {
            get
            {
                return RawRegularExpression.Replace(Resources.IPRegexKey, ipRegexValue).Replace(Resources.PortRegexKey, portRegexValue);
            }
        }
    }
}
