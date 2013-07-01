﻿using System.Collections.Generic;
using System.IO;
using ProxySearch.Common;
using ProxySearch.Console.Code.Detectable;
using ProxySearch.Console.Code.Interfaces;
using ProxySearch.Console.Code.ProxyClients;
using ProxySearch.Console.Code.Settings;
using ProxySearch.Console.Code.Version;
using ProxySearch.Engine;
using ProxySearch.Engine.Bandwidth;

namespace ProxySearch.Console.Code
{
    public class ApplicationInitializer
    {
        public void Initialize(bool shutdown)
        {
            Context.Set<IDetectableSearcher>(new DetectableSearcher());
            Context.Set<IProxyClientSearcher>(new ProxyClientSearcher());
            Context.Set(Settings);
            Context.Set<IUsedProxies>(new ProxyStorage(ReadProxyList(Constants.UsedProxiesStorage.Location)));

            ProxyStorage blacklist = new ProxyStorage(ReadProxyList(Constants.BlackListStorage.Location));
            Context.Set<IBlackList>(blacklist);
            Context.Set<IBlackListManager>(blacklist);

            Context.Set(new ProxyClientsSettings());
            Context.Set<IVersionProvider>(new VersionProvider());
            if (!shutdown)
            {
                new VersionManager().Check();
            }
         }

        public void Deinitialize()
        {
            File.WriteAllText(Constants.SettingsStorage.Location, Serializer.Serialize(Context.Get<AllSettings>()));
            SaveProxyList(Constants.UsedProxiesStorage.Location, Context.Get<IUsedProxies>().ProxyList);
            SaveProxyList(Constants.BlackListStorage.Location, Context.Get<IBlackListManager>().ProxyList);
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

        private ProxyList BlackList
        {
            get
            {
                return ReadProxyList(Constants.BlackListStorage.Location);
            }
        }

        private ProxyList ReadProxyList(string location)
        {
            if (!File.Exists(location))
            {
                return new ProxyList();
            }

            string proxiesXml = File.ReadAllText(location);
            return new ProxyList(Serializer.Deserialize<List<AddressPortPair>>(proxiesXml));
        }

        private void SaveProxyList(string location, ProxyList proxyList)
        {
            File.WriteAllText(location, Serializer.Serialize(proxyList.Proxies));
        }
    }
}
