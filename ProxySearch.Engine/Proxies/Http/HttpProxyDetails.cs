using System;
using ProxySearch.Engine.Properties;

namespace ProxySearch.Engine.Proxies.Http
{
    public class HttpProxyDetails
    {
        public HttpProxyDetails(HttpProxyTypes type)
        {
            switch (type)
            {
                case HttpProxyTypes.Anonymous:
                    Type = Resources.Anonymous;
                    TypeDetails = Resources.AnonymousDetails;
                    break;
                case HttpProxyTypes.HighAnonymous:
                    Type = Resources.HighAnonymous;
                    TypeDetails = Resources.HighAnonymousDetails;
                    break;
                case HttpProxyTypes.Transparent:
                    Type = Resources.Transparent;
                    TypeDetails = Resources.TransparentDetails;
                    break;
                case HttpProxyTypes.ChangesContent:
                    Type = Resources.ChangesContent;
                    TypeDetails = Resources.ChangesContentDetails;
                    break;
                case HttpProxyTypes.CannotVerify:
                    Type = Resources.CannotVerify;
                    TypeDetails = Resources.CannotVerifyDetails;
                    break;
            }
        }

        public string Type
        {
            get;
            private set;
        }

        public string TypeDetails
        {
            get;
            private set;
        }

        public override string ToString()
        {
            return Type;
        }
    }
}
