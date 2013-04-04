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
                    Name = Resources.Anonymous;
                    Details = Resources.AnonymousDetails;
                    break;
                case HttpProxyTypes.HighAnonymous:
                    Name = Resources.HighAnonymous;
                    Details = Resources.HighAnonymousDetails;
                    break;
                case HttpProxyTypes.Transparent:
                    Name = Resources.Transparent;
                    Details = Resources.TransparentDetails;
                    break;
                case HttpProxyTypes.ChangesContent:
                    Name = Resources.ChangesContent;
                    Details = Resources.ChangesContentDetails;
                    break;
                case HttpProxyTypes.CannotVerify:
                    Name = Resources.CannotVerify;
                    Details = Resources.CannotVerifyDetails;
                    break;
            }

            Type = type;
        }

        public string Name
        {
            get;
            private set;
        }

        public string Details
        {
            get;
            private set;
        }

        public HttpProxyTypes Type
        {
            get;
            private set;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
