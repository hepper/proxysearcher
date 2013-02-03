using System.Collections.Generic;
using ProxySearch.Common;
using ProxySearch.Console.Code.Interfaces;
using System.Linq;
using System.IO;
using System;
using ProxySearch.Console.Properties;

namespace ProxySearch.Console.Code.ProxyClients
{
    public class ProxyClientsSettings
    {
        private List<ProxyClientSettings> proxyClientSettings = new List<ProxyClientSettings>();

        public ProxyClientsSettings()
        {
            if (File.Exists(Constants.ProxySettingsStorage.Location))
            {
                string settings = File.ReadAllText(Constants.ProxySettingsStorage.Location);
                proxyClientSettings = Serializer.Deserialize<List<ProxyClientSettings>>(settings);
            }
        }

        public string this[string name]
        {
            get
            {
                ProxyClientSettings settings = proxyClientSettings.SingleOrDefault(item => item.Name == name);

                if (settings == null)
                {
                    return null;
                }

                return settings.Settings;
            }
            set
            {
                if (value == null)
                {
                    proxyClientSettings.RemoveAll(item => item.Name == name);
                    return;
                }

                ProxyClientSettings settings = proxyClientSettings.SingleOrDefault(item => item.Name == name);

                if (settings != null)
                {
                    throw new InvalidOperationException(Resources.YouShouldClearValueBeforeSetNewOne);
                }

                proxyClientSettings.Add(new ProxyClientSettings
                {
                    Name = name,
                    Settings = value
                });

                File.WriteAllText(Constants.ProxySettingsStorage.Location, Serializer.Serialize<List<ProxyClientSettings>>(proxyClientSettings));
            }
        }

        public void ClearSettings(string name)
        {
            proxyClientSettings.RemoveAll(item => item.Name == name);
        }
    }
}
