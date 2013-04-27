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
                return RegistryKey != null;
            }
        }

        private string browserPath = null;
        protected string BrowserPath
        {
            get
            {
                if (browserPath ==null)
                    browserPath = (string)RegistryKey.GetValue(null);

                return browserPath;
            }
        }

        private RegistryKey RegistryKey
        {
            get
            {
                RegistryKey browserPath = Registry.LocalMachine.OpenSubKey(string.Format(Constants.Browsers.BrowserPath64Bit, clientName));

                if (browserPath != null)
                    return browserPath;

                return Registry.LocalMachine.OpenSubKey(string.Format(Constants.Browsers.BrowserPath32Bit, clientName));
            }
        }
    }
}
