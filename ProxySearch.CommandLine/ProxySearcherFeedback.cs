using System;
using ProxySearch.Engine;
using ProxySearch.Engine.Proxies;

namespace ProxySearch.CommandLine
{
    public class ProxySearcherFeedback : IProxySearchFeedback
    {
        public void OnAliveProxy(ProxyInfo proxyInfo)
        {
            lock (this)
            {
                Console.WriteLine(proxyInfo.AddressPort);
            }
        }
    }
}
