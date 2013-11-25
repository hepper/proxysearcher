using System;
using System.ComponentModel;
using System.Net;
using ProxySearch.Engine.Bandwidth;

namespace ProxySearch.Engine.Proxies
{
    public class ProxyInfo : Proxy, INotifyPropertyChanged
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

        public ProxyInfo Proxy
        {
            get
            {
                return this;
            }
        }

        public void NotifyProxyChanged()
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("Proxy"));
            }
        }

        public override string ToString()
        {
            return string.Format("{0}\t{1}\t{2}", AddressPort, CountryInfo.Name, Details);
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
