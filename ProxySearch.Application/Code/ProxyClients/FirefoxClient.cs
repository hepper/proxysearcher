using ProxySearch.Console.Properties;
using ProxySearch.Engine;

namespace ProxySearch.Console.Code.ProxyClients
{
    public class FirefoxClient : BrowserClient
    {
        public FirefoxClient()
            : base(Resources.Firefox, "/Images/Firefox.png", 1, "FIREFOX.EXE")
        {
        }

        private ProxyInfo proxyInfo;

        protected override void SetProxy(ProxyInfo proxyInfo)
        {
            this.proxyInfo = proxyInfo;
        }

        protected override ProxyInfo GetProxy()
        {
            return proxyInfo;
        }

        protected override SettingsData BackupSettings()
        {
            return null;
        }

        protected override void RestoreSettings(SettingsData settings)
        {
        }
    }
}
