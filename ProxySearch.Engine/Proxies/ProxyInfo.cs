using System.ComponentModel;
using System.Net;
using System.Text;
using ProxySearch.Engine.Bandwidth;
using ProxySearch.Engine.Ratings;

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

        public RatingData RatingData
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
            NotifyPropertyChanged("Proxy");
        }

        private void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public string ToString(bool exportCountry, bool exportProxyType)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(AddressPort);

            if (exportCountry)
            {
                builder.Append("\t");
                builder.Append(CountryInfo.Name);
            }

            if (exportProxyType)
            {
                builder.Append("\t");
                builder.Append(Details);
            }

            return builder.ToString();
        }

        public override string ToString()
        {
            return ToString(true, true);
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
