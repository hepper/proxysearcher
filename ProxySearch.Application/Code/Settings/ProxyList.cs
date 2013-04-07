using System.Collections.Generic;
using ProxySearch.Engine.Proxies;

namespace ProxySearch.Console.Code.Settings
{
    public class ProxyList : IComparer<AddressPortPair>
    {
        public ProxyList()
        {
            Proxies = new List<AddressPortPair>();
        }

        public ProxyList(List<AddressPortPair> proxies)
        {
            Proxies = new List<AddressPortPair>(proxies);
            Proxies.Sort(Compare);
        }

        public List<AddressPortPair> Proxies
        {
            get;
            private set;
        }

        public void Add(Proxy proxyInfo)
        {
            int index = FindIndex(proxyInfo);
            if (index < 0)
            {
                Proxies.Insert(~index, new AddressPortPair
                {
                    IPAddress = proxyInfo.Address,
                    Port = proxyInfo.Port
                });
            }
        }

        public bool Contains(Proxy proxy)
        {
            return FindIndex(proxy) >= 0;
        }

        private int FindIndex(Proxy proxy)
        {
            return Proxies.BinarySearch(new AddressPortPair
            {
                IPAddress = proxy.Address,
                Port = proxy.Port
            }, this);
        }

        public int Compare(AddressPortPair x, AddressPortPair y)
        {
            return GetKey(x).CompareTo(GetKey(y));
        }

        private string GetKey(AddressPortPair item)
        {
            return string.Format("{0}:{1}", item.IPAddressString, item.Port);
        }
    }
}
