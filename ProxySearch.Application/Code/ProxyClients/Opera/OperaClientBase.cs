using System;
using System.IO;
using ProxySearch.Console.Properties;
using ProxySearch.Engine.Proxies;

namespace ProxySearch.Console.Code.ProxyClients.Opera
{
    public abstract class OperaClientBase : RestartableBrowserClient
    {
        private static readonly string SectionName = "Proxy";

        public OperaClientBase(string proxyType)
            : base(proxyType, Resources.Opera, Resources.Opera, "/Images/Opera.png", 2, "Opera", "opera")
        {
        }

        protected override void SetProxy(ProxyInfo proxyInfo)
        {
            if (proxyInfo != null)
            {
                IniFile.WriteValue(SettingsPath, SectionName, string.Format("{0} server", ProtocolName), proxyInfo.AddressPort);
                IniFile.WriteValue(SettingsPath, SectionName, string.Format("Use {0}", ProtocolName), "1");
            }
            else
            {
                IniFile.WriteValue(SettingsPath, SectionName, string.Format("{0} server", ProtocolName), null);
                IniFile.WriteValue(SettingsPath, SectionName, string.Format("Use {0}", ProtocolName), null);
            }
        }

        protected override ProxyInfo GetProxy()
        {
            if (!File.Exists(SettingsPath))
            {
                return null;
            }

            string addressPort = IniFile.ReadValue(SettingsPath, SectionName, string.Format("{0} server", ProtocolName));

            if (string.IsNullOrWhiteSpace(addressPort))
            {
                return null;
            }

            return new ProxyInfo(addressPort);
        }

        protected string SettingsPath
        {
            get
            {
                return string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"\Opera\Opera\operaprefs.ini");
            }
        }

        private string ProtocolName
        {
            get
            {
                return GetProtocolName("HTTP", "SOCKS");
            }
        }

        protected override bool ImportsInternetExplorerSettings
        {
            get 
            {
                return false;
            }
        }
    }
}
