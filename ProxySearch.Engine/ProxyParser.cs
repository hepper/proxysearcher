using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ProxySearch.Engine.Proxies;

namespace ProxySearch.Engine
{
    public class ProxyParser : IProxyParser
    {
        IBlackList blackList;
        Hashtable foundIps = new Hashtable();

        public ProxyParser()
            : this(new EmptyBlackList())
        {
        }

        public ProxyParser(IBlackList blackList)
        {
            this.blackList = blackList;
        }        

        public Task<List<Proxy>> ParseProxiesAsync(string document)
        {
            return Task.Run<List<Proxy>>(() =>
            {
                List<Proxy> result = new List<Proxy>();
                string pattern = @"(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?):[0-9]+";
                Regex regEx = new Regex(pattern);

                foreach (Match match in regEx.Matches(document))
                {
                    Proxy info = GetProxyInfo(match);

                    if (info != null && !foundIps.ContainsKey(info.AddressPort) && !blackList.Contains(info))
                    {
                        foundIps.Add(info.AddressPort, info);
                        result.Add(info);
                    }
                }

                return result;
            });
        }

        private Proxy GetProxyInfo(Match match)
        {
            string[] data = match.Value.Split(':');

            ushort port;
            if (!ushort.TryParse(data[1], out port))
                return null;

            return new Proxy(IPAddress.Parse(data[0]), port);
        }
    }
}
