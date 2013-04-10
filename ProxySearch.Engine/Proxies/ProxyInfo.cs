using System;
using System.Net;
using ProxySearch.Engine.Bandwidth;

namespace ProxySearch.Engine.Proxies
{
    public class ProxyInfo : Proxy
    {
        public ProxyInfo(IPAddress address, ushort port)
            : base(address, port)
        {
            BandwidthData = new BandwidthData();
        }

        public ProxyInfo(Proxy proxy)
            : this(proxy.Address, proxy.Port)
        {
        }

        public ProxyInfo(string ipPort)
            : base(ipPort)
        {
            BandwidthData = new BandwidthData();
        }

        public CountryInfo CountryInfo
        {
            get;
            set;
        }

        public ProxyDetails Details
        {
            get;
            set;
        }

        public BandwidthData BandwidthData
        {
            get;
            set;
        }

        public override string ToString()
        {
            return string.Format("{0}\t{1}\t{2}", AddressPort, CountryInfo.Name, Details);
        }
    }
}
