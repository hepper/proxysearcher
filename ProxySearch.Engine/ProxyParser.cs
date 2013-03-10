using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProxySearch.Engine
{
    public class ProxyParser : IProxyParser
    {
        Hashtable foundIps = new Hashtable();

        public Task<List<ProxyInfo>> ParseProxiesAsync(string document)
        {
            return Task.Run<List<ProxyInfo>>(() =>
            {
                List<ProxyInfo> result = new List<ProxyInfo>();
                string pattern = @"(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?):[0-9]+";
                Regex regEx = new Regex(pattern);

                foreach (Match match in regEx.Matches(document))
                {
                    ProxyInfo info = GetProxyInfo(match);

                    if (info != null && !foundIps.ContainsKey(info.AddressPort))
                    {
                        foundIps.Add(info.AddressPort, info);
                        result.Add(info);
                    }
                }

                return result;
            });
        }

        private ProxyInfo GetProxyInfo(Match match)
        {
            string[] data = match.Value.Split(':');

            ushort port;
            if (!ushort.TryParse(data[1], out port))
                return null;

            return new ProxyInfo(IPAddress.Parse(data[0]), port);
        }
    }
}
