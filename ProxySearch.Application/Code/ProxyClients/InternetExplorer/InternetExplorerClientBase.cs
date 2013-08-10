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
            if (!UseProxy)
                return null;

            return new ProxyInfo(ProxyString);
        }

        private string ProxyString
        {
            get
            {
                string[] arguments = GetValue<string>(Constants.Browsers.IE.ProxyServer).Split(';');

                string value = arguments.SingleOrDefault(item => item.StartsWith(string.Concat(Type, "=")));

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
                UseProxy = UseProxy,
                AddressPort = GetValue<string>(Constants.Browsers.IE.ProxyServer)
            };
        }

        protected override void RestoreSettings(SettingsData settings)
        {
            WinINet.SetProxy(settings.UseProxy, settings.AddressPort);
        }

        private bool UseProxy
        {
            get
            {
                return GetValue<int>(Constants.Browsers.IE.ProxyEnabled) != 0;
            }
        }

        private T GetValue<T>(string name)
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(Constants.Browsers.IE.Settings))
            {
                return (T)key.GetValue(name);
            }
        }

        protected override bool ImportsInternetExplorerSettings
        {
            get 
            {
                return false; // Internet exprorer do not import his settings, it just uses his own.
            }
        }
    }
}
