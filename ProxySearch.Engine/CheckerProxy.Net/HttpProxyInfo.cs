using System;
using System.Collections.Generic;
using System.Text;
using ProxySearch.Engine.Properties;

namespace ProxySearch.Engine.CheckerProxy.Net
{
    public class HttpProxyInfo
    {
        public static class HttpProxyTypes
        {
            public static readonly string Anonymous = Resources.Anonymous;
            public static readonly string HighAnonymous = Resources.HighAnonymous;
            public static readonly string Transparent = Resources.Transparent;
        }

        public string Type
        {
            get;
            set;
        }

        public override string ToString()
        {
            return Type;
        }
    }
}
