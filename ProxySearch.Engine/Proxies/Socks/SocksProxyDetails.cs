using System;
using ProxySearch.Engine.Properties;

namespace ProxySearch.Engine.Proxies.Socks
{
    public class SocksProxyDetails
    {
        public SocksProxyDetails(SocksProxyTypes type)
        {
            switch (type)
            {
                case SocksProxyTypes.Unchecked:
                    Name = Resources.Unchecked;
                    Details = Resources.UncheckedDetails;
                    break;
                case SocksProxyTypes.Socks4:
                    Name = Resources.Socks4;
                    Details = Resources.Socks4Details;
                    break;
                case SocksProxyTypes.Socks5:
                    Name = Resources.Socks5;
                    Details = Resources.Socks5Details;
                    break;
                case SocksProxyTypes.CannotVerify:
                    Name = Resources.CannotVerify;
                    Details = Resources.CannotVerifyDetails;
                    break;
                default:
                    throw new InvalidOperationException(Resources.UnsupportedSocksProxyType);
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

        public SocksProxyTypes Type
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
