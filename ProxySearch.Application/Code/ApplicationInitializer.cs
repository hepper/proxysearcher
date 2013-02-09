using System.Collections.Generic;
using System.IO;
using ProxySearch.Common;
using ProxySearch.Console.Code.Detectable;
using ProxySearch.Console.Code.Interfaces;
using ProxySearch.Console.Code.ProxyClients;
using ProxySearch.Console.Code.Settings;
using ProxySearch.Console.Code.Version;

namespace ProxySearch.Console.Code
{
    public class ApplicationInitializer
    {
        public void Initialize()
        {
            Context.Set<IDetectableSearcher>(new DetectableSearcher());
            Context.Set<IProxyClientSearcher>(new ProxyClientSearcher());
            Context.Set(Settings);
            Context.Set(UsedProxies);
            Context.Set(new ProxyClientsSettings());
            Context.Set<IVersionProvider>(new VersionProvider());
            new VersionManager().Check();
         }

        public void Deinitialize()
        {
            SaveSettings();
            SaveUsedProxies();
        }

        private AllSettings Settings
        {
            get
            {
                if (!File.Exists(Constants.SettingsStorage.Location))
                {
                    return new DefaultSettingsFactory().Create();
                }

                string settingsXml = File.ReadAllText(Constants.SettingsStorage.Location);
                return Serializer.Deserialize<AllSettings>(settingsXml);
            }
        }

        private UsedProxies UsedProxies
        {
            get
            {
                if (!File.Exists(Constants.UsedProxiesStorage.Location))
                {
                    return new UsedProxies();
                }

                string usedProxiesXml = File.ReadAllText(Constants.UsedProxiesStorage.Location);
                return new UsedProxies(Serializer.Deserialize<List<AddressPortPair>>(usedProxiesXml));
            }
        }

        private void SaveSettings()
        {
            File.WriteAllText(Constants.SettingsStorage.Location, Serializer.Serialize<AllSettings>(Context.Get<AllSettings>()));
        }

        private void SaveUsedProxies()
        {
            File.WriteAllText(Constants.UsedProxiesStorage.Location, Serializer.Serialize<List<AddressPortPair>>(Context.Get<UsedProxies>().Proxies));
        }
    }
}
