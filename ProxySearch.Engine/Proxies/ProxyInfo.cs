using System;
using System.Net;
using ProxySearch.Engine.Bandwidth;

namespace ProxySearch.Engine.Proxies
{
    public class ProxyInfo
    {
        public ProxyInfo(IPAddress address, ushort port)
        {
            Address = address;
            Port = port;
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
            set;
        }

        public object Details
        {
            get;
            set;
        }

        public BandwidthData BandwidthData
        {
            get;
            set;
        }

        public string AddressPort
        {
            get
            {
                return string.Format("{0}:{1}", Address, Port);
            }
        }

        public override string ToString()
        {
            return string.Format("{0}\t{1}\t{2}", AddressPort, CountryInfo.Name, Details);
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
