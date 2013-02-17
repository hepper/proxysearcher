using System.Collections.Generic;
using ProxySearch.Engine;

namespace ProxySearch.Console.Code.Settings
{
    public class UsedProxies : IComparer<AddressPortPair>
    {
        public UsedProxies()
        {
            Proxies = new List<AddressPortPair>();
        }

        public UsedProxies(List<AddressPortPair> proxies)
        {
            Proxies = new List<AddressPortPair>(proxies);
            Proxies.Sort(Compare);
        }

        public List<AddressPortPair> Proxies
        {
            get;
            private set;
        }

        public void Add(ProxyInfo proxyInfo)
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

        public bool Contains(ProxyInfo proxyInfo)
        {
            return FindIndex(proxyInfo) >= 0;
        }

        private int FindIndex(ProxyInfo proxyInfo)
        {
            return Proxies.BinarySearch(new AddressPortPair
            {
                IPAddress = proxyInfo.Address,
                Port = proxyInfo.Port
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
