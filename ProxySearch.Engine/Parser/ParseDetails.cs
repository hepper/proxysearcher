using System;
using System.Xml.Serialization;

namespace ProxySearch.Engine.Parser
{
    public class ParseDetails
    {
        public string RegularExpression
        {
            get;
            set;
        }

        public string Code
        {
            get;
            set;
        }

        public static ParseDetails IPPortDetails
        {
            get
            {
                string pattern = @"(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?):[0-9]+";

                return new ParseDetails
                {
                    RegularExpression = pattern,
                    Code = EmbeddedResource.ReadToEnd("ProxySearch.Engine.Resources.DefaultProxyParseCode._cs")
                };
            }
        }
    }
}
