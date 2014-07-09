using System;
using ProxySearch.Engine.Properties;

namespace ProxySearch.Engine.Proxies.Http
{
    public class HttpProxyDetails : ProxyTypeDetails
    {
        public HttpProxyDetails(HttpProxyTypes type)
            : base(type.ToString(), GetName(type), GetDetails(type))
        {
        }

        public static string GetName(HttpProxyTypes type)
        {
            switch (type)
            {
                case HttpProxyTypes.Anonymous:
                    return Resources.Anonymous;
                case HttpProxyTypes.HighAnonymous:
                    return Resources.HighAnonymous;
                case HttpProxyTypes.Transparent:
                    return Resources.Transparent;
                case HttpProxyTypes.ChangesContent:
                    return Resources.ChangesContent;
                case HttpProxyTypes.Unchecked:
                    return Resources.Unchecked;
                case HttpProxyTypes.CannotVerify:
                    return Resources.CannotVerify;
                default:
                    throw new InvalidOperationException(Resources.UnsupportedHttpProxyType);
            }
        }

        private static string GetDetails(HttpProxyTypes type)
        {
            switch (type)
            {
                case HttpProxyTypes.Anonymous:
                    return Resources.AnonymousDetails;
                case HttpProxyTypes.HighAnonymous:
                    return Resources.HighAnonymousDetails;
                case HttpProxyTypes.Transparent:
                    return Resources.TransparentDetails;
                case HttpProxyTypes.ChangesContent:
                    return Resources.ChangesContentDetails;
                case HttpProxyTypes.Unchecked:
                    return Resources.UncheckedDetails;
                case HttpProxyTypes.CannotVerify:
                    return Resources.CannotVerifyDetails;
                default:
                    throw new InvalidOperationException(Resources.UnsupportedHttpProxyType);
            }
        }
    }
}
