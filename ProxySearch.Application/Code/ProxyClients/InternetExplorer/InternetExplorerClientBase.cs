using System;
using System.Linq;
using Microsoft.Win32;
using ProxySearch.Console.Code.ProxyClients.InternetExplorer.WinInet;
using ProxySearch.Console.Properties;
using ProxySearch.Engine.Proxies;

namespace ProxySearch.Console.Code.ProxyClients.InternetExplorer
{
    public abstract class InternetExplorerClientBase : BrowserClient
    {
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
            if (!WinINet.IsProxyUsed)
                return null;

            return new ProxyInfo(ProxyString);
        }

        private string ProxyString
        {
            get
            {
                string[] arguments = WinINet.ProxyIpPort.Split(';');

                string value = arguments.SingleOrDefault(item => item.StartsWith(string.Concat(Type, "="), StringComparison.CurrentCultureIgnoreCase));

                if (value == null)
                {
                    value = arguments.Single();
                }

                return value.Split('=').Last();
            }
        }

        protected override void SetProxy(ProxyInfo proxyInfo)
        {
            WinINet.SetProxy(true, string.Format("{0}={1}:{2}", GetProtocolName("http", "socks"), proxyInfo.Address, proxyInfo.Port));
        }

        protected override SettingsData BackupSettings()
        {
            return new SettingsData
            {
                UseProxy = WinINet.IsProxyUsed,
                AddressPort = WinINet.ProxyIpPort
            };
        }

        protected override void RestoreSettings(SettingsData settings)
        {
            WinINet.SetProxy(settings.UseProxy, settings.AddressPort);
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
