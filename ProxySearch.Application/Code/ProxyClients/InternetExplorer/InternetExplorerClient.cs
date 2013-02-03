using System.Net;
using Microsoft.Win32;
using ProxySearch.Console.Properties;
using ProxySearch.Engine;
using System.Linq;
using ProxySearch.Console.Code.ProxyClients.InternetExplorer.WinInet;

namespace ProxySearch.Console.Code.ProxyClients.InternetExplorer
{
    public class InternetExplorerClient : BrowserClient
    {
        public InternetExplorerClient()
            : base(Resources.InternetExplorer, "/Images/InternerExplorer.gif", "IEXPLORE.EXE")
        {
        }

        protected override ProxyInfo GetProxy()
        {
            if (!UseProxy)
                return null;

            string value = GetValue<string>(Constants.Browsers.IE.ProxyServer);

            string[] arguments = GetHttpProxy(value).Split(':');
                        
            return new ProxyInfo(IPAddress.Parse(arguments[0]), short.Parse(arguments[1]), null);
        }

        private string GetHttpProxy(string value)
        {
            string[] arguments = value.Split(';');

            if (arguments.Length == 1)
            {
                return value;
            }

            string result = arguments.Single(item=>item.StartsWith("http="));

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
