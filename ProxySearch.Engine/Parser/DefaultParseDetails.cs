using System.Collections.Generic;
using ProxySearch.Engine.Properties;

namespace ProxySearch.Engine.Parser
{
    public class DefaultParseDetails
    {
        public List<ParseDetails> ParseDetailsList
        {
            get
            {
                return new List<ParseDetails>
                {
                    IPPortDetails,
                    XproxyComDetails
                };
            }
        }

        private ParseDetails IPPortDetails
        {
            get
            {
                return new ParseDetails
                {
                    Url = string.Empty,
                    RawRegularExpression = GetRegex("{0}:{1}"),
                    Code = EmbeddedResource.ReadToEnd("ProxySearch.Engine.Resources.DefaultProxyParseCode._cs")
                };
            }
        }

        private ParseDetails XproxyComDetails
        {
            get
            {
                return new ParseDetails
                {
                    Url = "www.xroxy.com",
                    RawRegularExpression = GetRegex("host=(?<ipgroup>.{0}?)&port=(?<portgroup>.{1}?)"),
                    Code = EmbeddedResource.ReadToEnd("ProxySearch.Engine.Resources.XproxyComParseCode._cs")
                };
            }
        }

        private string GetRegex(string format)
        {
            return string.Format(format, Resources.IPRegexKey, Resources.PortRegexKey);
        }
    }
}
