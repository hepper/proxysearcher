using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProxySearch.Engine.Proxies;

namespace ProxySearch.Engine.Parser
{
    public class ProxyProvider : IProxyProvider
    {
        IBlackList blackList;
        Hashtable foundProxies = new Hashtable();
        IParseMethodsProvider parseMethodProvider;

        public ProxyProvider()
            : this(new EmptyBlackList(), new ParseMethodsProvider(new Dictionary<string, ParseDetails> 
                                                                  {
                                                                      {string.Empty, ParseDetails.IPPortDetails}
                                                                  }))
        {
        }

        public ProxyProvider(IBlackList blackList, IParseMethodsProvider parseMethodProvider)
        {
            this.blackList = blackList;
            this.parseMethodProvider = parseMethodProvider;
        }

        public async Task<List<Proxy>> ParseProxiesAsync(Uri uri, string document)
        {
            return await Task.Run<List<Proxy>>(() =>
            {
                List<Proxy> result = new List<Proxy>();

                IParseMethod parseMethod = parseMethodProvider.GetMethod(uri);

                foreach (Proxy proxy in parseMethod.Parse(document))
                {
                    if (!foundProxies.ContainsKey(proxy.AddressPort) && !blackList.Contains(proxy))
                    {
                        foundProxies.Add(proxy.AddressPort, proxy);
                        result.Add(proxy);
                    }
                }

                return result;
            });
        }
    }
}
