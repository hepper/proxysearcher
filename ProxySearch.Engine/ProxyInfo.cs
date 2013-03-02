using System;
using System.Net;
using ProxySearch.Engine.Bandwidth;
using ProxySearch.Engine.Checkers.CheckerProxy.Net;

namespace ProxySearch.Engine
{
    public class ProxyInfo
    {
        public ProxyInfo(IPAddress address, ushort port, CountryInfo countryInfo)
        {
            Address = address;
            Port = port;
            CountryInfo = countryInfo;
            BandwidthData = new BandwidthData();
        }

        public IPAddress Address
        {
            get;
            private set;
        }

        public ushort Port
        {
            get;
            private set;
        }

        public CountryInfo CountryInfo
        {
            get;
            private set;
        }

        public HttpProxyInfo Details
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
            return string.Format("{0}:{1}\t{2}\t{3}", Address, Port, CountryInfo.Name, Details);
        }

        public override bool Equals(object obj)
        {
            if ((object)obj == null)
            {
                return false;
            }

            if (GetType() != obj.GetType())
            {
                return false;
            }

            ProxyInfo proxyInfo = (ProxyInfo)obj;

            return this.Address.ToString() == proxyInfo.Address.ToString() && this.Port == proxyInfo.Port;
        }

        public static bool operator ==(ProxyInfo a, ProxyInfo b)
        {
            if (Object.ReferenceEquals(a, b))
            {
                return true;
            }

            if ((object)a == null)
            {
                return false;
            }

            return a.Equals(b);
        }

        public static bool operator !=(ProxyInfo a, ProxyInfo b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return Address.GetHashCode() + Port.GetHashCode();
        }
    }
}
