using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ProxySearch.Engine;

namespace ProxySearch.Console.Code.Settings
{
    public class UsedProxies
    {
        public UsedProxies()
        {
            Proxies = new List<AddressPortPair>();
        }

        public UsedProxies(List<AddressPortPair> proxies)
        {
            Proxies = proxies;
        }

        public List<AddressPortPair> Proxies
        {
            get;
            private set;
        }

        public void Add(ProxyInfo proxyInfo)
        {
            if (!Contains(proxyInfo))
            {
                Proxies.Add(new AddressPortPair
                {
                    IPAddress = proxyInfo.Address,
                    Port = proxyInfo.Port
                });
            }
        }

        public bool Contains(ProxyInfo proxyInfo)
        {
            return Proxies.Exists(item => item.IPAddress.ToString() == proxyInfo.Address.ToString() && item.Port == proxyInfo.Port);
        }
    }
}
