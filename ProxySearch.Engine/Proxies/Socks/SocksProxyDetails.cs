using System;
using System.Net;
using ProxySearch.Engine.Properties;

namespace ProxySearch.Engine.Proxies.Socks
{
    public class SocksProxyDetails : ProxyTypeDetails
    {
        public SocksProxyDetails(SocksProxyTypes type, IPAddress outgoingIPAddress)
            : base(type.ToString(), GetName(type), GetDetails(type), outgoingIPAddress)
        {
        }

        private static string GetName(SocksProxyTypes type)
        {
            switch (type)
            {
                case SocksProxyTypes.Socks4:
                    return Resources.Socks4;
                case SocksProxyTypes.Socks5:
                    return Resources.Socks5;
                case SocksProxyTypes.Unchecked:
                    return Resources.Unchecked;
                case SocksProxyTypes.ChangesContent:
                    return Resources.ChangesContent;
                case SocksProxyTypes.CannotVerify:
                    return Resources.CannotVerify;
                default:
                    throw new InvalidOperationException(Resources.UnsupportedSocksProxyType);
            }
        }

        private static string GetDetails(SocksProxyTypes type)
        {
            switch (type)
            {
                case SocksProxyTypes.Socks4:
                    return Resources.Socks4Details;
                case SocksProxyTypes.Socks5:
                    return Resources.Socks5Details;
                case SocksProxyTypes.Unchecked:
                    return Resources.UncheckedDetails;
                case SocksProxyTypes.ChangesContent:
                    return Resources.ChangesContentDetails;
                case SocksProxyTypes.CannotVerify:
                    return Resources.CannotVerifyDetails;
                default:
                    throw new InvalidOperationException(Resources.UnsupportedSocksProxyType);
            }
        }
    }
}