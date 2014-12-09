using System.Collections.Generic;
using System.Linq;
using ProxySearch.Console.Code.ProxyClients.InternetExplorer.WinInet;
using ProxySearch.Console.Properties;
using ProxySearch.Engine.Proxies;

namespace ProxySearch.Console.Code.ProxyClients.InternetExplorer
{
    public abstract class InternetExplorerClientBase : BrowserClient
    {
        private string ProtocolName
        {
            get
            {
                return GetProtocolName("http", "socks");
            }
        }

        public InternetExplorerClientBase(string proxyType)
            : this(proxyType, Resources.InternetExplorer, "/Images/InternerExplorer.gif", 0, "IEXPLORE.EXE")
        {
        }

        public InternetExplorerClientBase(string proxyType, string name, string image, int order, string clientName)
            : base(proxyType, name, Resources.InternetExplorer, image, order, clientName)
        {
        }

        protected override ProxyInfo GetProxy()
        {
            if (!WinINet.IsProxyUsed || WinINet.ProxyIpPort == null)
                return null;

            List<string> arguments = WinINet.ProxyIpPort
                                            .Split(';')
                                            .ToList();

            //Single host is used for all types of proxy
            if (arguments.Count == 1 && arguments.Single().Split('=').Length == 1)
                return new ProxyInfo(WinINet.ProxyIpPort);

            //Single protocol specific proxy is set or array of them
            int index = GetIndexOfCurrentProtocol(arguments);

            if (index == -1)
                return null;

            return new ProxyInfo(arguments[index].Split('=').Last());
        }

        protected override void SetProxy(ProxyInfo proxyInfo)
        {
            //Proxy was not specified
            if (WinINet.ProxyIpPort == null)
            {
                WinINet.SetProxy(proxyInfo != null, proxyInfo != null ? GetProtocolString(proxyInfo) : null);
                return;
            }

            List<string> arguments = WinINet.ProxyIpPort
                                            .Split(';')
                                            .ToList();

            //Single host is used for all types of proxy
            if (arguments.Count == 1 && arguments.Single().Split('=').Length == 1)
            {
                WinINet.SetProxy(proxyInfo != null, proxyInfo != null ? proxyInfo.AddressPort : null);
                return;
            }

            //Single protocol specific proxy is set or array of them
            int index = GetIndexOfCurrentProtocol(arguments);

            if (proxyInfo == null)
            {
                if (index != -1)
                    arguments.RemoveAt(index);

                WinINet.SetProxy(arguments.Any(), arguments.Any() ? string.Join(",", arguments) : null);
                return;
            }

            if (index == -1)
                arguments.Add(GetProtocolString(proxyInfo));
            else
                arguments[index] = GetProtocolString(proxyInfo);

            WinINet.SetProxy(true, string.Join(",", arguments));
        }

        private int GetIndexOfCurrentProtocol(List<string> arguments)
        {
            return arguments.FindIndex(item =>
            {
                string[] keyValue = item.Split('=');

                if (keyValue.Length != 2)
                    return false;

                return string.Equals(keyValue[0], ProtocolName);
            });
        }

        private string GetProtocolString(ProxyInfo proxyInfo)
        {
            return string.Format("{0}={1}", GetProtocolName("http", "socks"), proxyInfo.AddressPort);
        }

        protected override bool ImportsInternetExplorerSettings
        {
            get
            {
                return false; // Internet explorer do not import his settings, it just uses his own.
            }
        }
    }
}
