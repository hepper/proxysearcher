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
                case SocksProxyTypes.None:
                    break;
                case SocksProxyTypes.Socks4:
                    Name = "SOCKS4";
                    Details = "SOCKS4";
                    break;
                case SocksProxyTypes.Socks5: 
                    Name = "SOCKS5";
                    Details = "SOCKS5";
                    break;
                default:
                    throw new InvalidOperationException("Unsupported socks proxy type");
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
