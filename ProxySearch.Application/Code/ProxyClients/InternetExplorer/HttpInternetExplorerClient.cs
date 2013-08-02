using System.Linq;
using Microsoft.Win32;
using ProxySearch.Console.Code.ProxyClients.InternetExplorer.WinInet;
using ProxySearch.Console.Properties;
using ProxySearch.Engine.Proxies;

namespace ProxySearch.Console.Code.ProxyClients.InternetExplorer
{
    public class HttpInternetExplorerClient : BrowserClient
    {
        public HttpInternetExplorerClient()
            : this(Resources.InternetExplorer, "/Images/InternerExplorer.gif", 0, "IEXPLORE.EXE")
        {
        }

        public HttpInternetExplorerClient(string name, string image, int order, string clientName)
            : base(Resources.HttpProxyType, name, image, order, clientName)
        {
        }

        protected override ProxyInfo GetProxy()
        {
            if (!UseProxy)
                return null;

            return new ProxyInfo(GetValue<string>(Constants.Browsers.IE.ProxyServer));
        }

        private string GetHttpProxy(string value)
        {
            string[] arguments = value.Split(';');

            if (arguments.Length == 1)
            {
                return value;
            }

            string result = arguments.Single(item => item.StartsWith("http="));

            return result.Split('=')[1];
        }

        protected override void SetProxy(ProxyInfo proxyInfo)
        {
            WinINet.SetProxy(true, string.Concat(proxyInfo.Address, ':', proxyInfo.Port));
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
    }
}
