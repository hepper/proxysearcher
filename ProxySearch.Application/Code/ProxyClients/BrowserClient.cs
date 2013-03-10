using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;

namespace ProxySearch.Console.Code.ProxyClients
{
    public abstract class BrowserClient : ProxyClientBase
    {
        private string clientName;

        public BrowserClient(string name, string image, int order, string clientName)
            : base(name, image, order)
        {
            this.clientName = clientName;
        }

        public override bool IsInstalled
        {
            get
            {
                return InstalledBrowsers.Contains(clientName);
            }
        }

        private List<string> InstalledBrowsers
        {
            get
            {
                RegistryKey browserKeys = Registry.LocalMachine.OpenSubKey(Constants.Browsers.StartMenuInternet64Bit);

                if (browserKeys == null)
                    browserKeys = Registry.LocalMachine.OpenSubKey(Constants.Browsers.StartMenuInternet32Bit);

                return browserKeys.GetSubKeyNames().ToList();
            }
        }
    }
}
