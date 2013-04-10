using System;
using System.IO;
using ProxySearch.Console.Properties;
using ProxySearch.Engine.Proxies;

namespace ProxySearch.Console.Code.ProxyClients.Opera
{
    public class OperaClient : ConfigurableRestartableBrowserClient
    {
        private static readonly string SectionName = "Proxy";

        public OperaClient()
            : base(Resources.Opera, "/Images/Opera.png", 2, "Opera", "opera", Constants.BackupsLocation.OperaSettings)
        {
        }

        protected override void SetProxy(ProxyInfo proxyInfo)
        {
            IniFile.WriteValue(SettingsPath, SectionName, "HTTP server", proxyInfo.AddressPort);
            IniFile.WriteValue(SettingsPath, SectionName, "HTTPS server", proxyInfo.AddressPort);
            IniFile.WriteValue(SettingsPath, SectionName, "Use HTTP", "1");
            IniFile.WriteValue(SettingsPath, SectionName, "Use HTTPS", "1");
        }

        protected override ProxyInfo GetProxy()
        {
            if (!File.Exists(SettingsPath))
            {
                return null;
            }

            return new ProxyInfo(IniFile.ReadValue(SettingsPath, SectionName, "HTTP server"));
        }

        protected override string SettingsPath
        {
            get
            {
                return string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"\Opera\Opera\operaprefs.ini");
            }
        }
    }
}
