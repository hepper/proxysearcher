using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using ProxySearch.Common;
using ProxySearch.Console.Code.Detectable;
using ProxySearch.Console.Code.GoogleAnalytics;
using ProxySearch.Console.Code.Interfaces;
using ProxySearch.Console.Code.ProxyClients;
using ProxySearch.Console.Code.Settings;
using ProxySearch.Console.Code.Version;
using ProxySearch.Console.Controls;
using ProxySearch.Console.Properties;
using ProxySearch.Engine;
using ProxySearch.Engine.Tasks;

namespace ProxySearch.Console.Code
{
    public class ApplicationInitializer
    {
        public void Initialize(bool shutdown)
        {
            Context.Set<IDetectableSearcher>(new DetectableSearcher());
            Context.Set(Settings);

            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(Context.Get<AllSettings>().SelectedCulture);
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.DefaultThreadCurrentCulture;
            
            Context.Set<IProxyClientSearcher>(new ProxyClientSearcher());
            
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

            Context.Get<IGA>().TrackEventAsync(EventType.Program, Resources.Started);
            Context.Get<IGA>().TrackPageViewAsync(typeof(SearchControl).Name);

            Context.Set(new TaskManager());
        }

        public void Deinitialize()
        {
            File.WriteAllText(Constants.SettingsStorage.Location, Serializer.Serialize(Context.Get<AllSettings>()));
            SaveProxyList(Constants.UsedProxiesStorage.Location, Context.Get<IUsedProxies>().ProxyList);
            SaveProxyList(Constants.BlackListStorage.Location, Context.Get<IBlackListManager>().ProxyList);

            if (Context.Get<AllSettings>().RevertUsedProxiesOnExit)
            {
                foreach (IProxyClient client in Context.Get<IProxyClientSearcher>()
                                                .AllClients
                                                .GroupBy(item => item.Name)
                                                .Where(group => group.Any(item => item.Proxy != null))
                                                .Select(group => group.First()))
                {
                    client.Proxy = null;
                }
            }

            Context.Get<IGA>().TrackPageViewAsync(string.Empty);
            Context.Get<IGA>().TrackEventAsync(EventType.Program, Resources.Closed);
        }

        private AllSettings Settings
        {
            get
            {
                if (!File.Exists(Constants.SettingsStorage.Location))
                {
                    return new DefaultSettingsFactory().Create();
                }

                try
                {
                    string settingsXml = File.ReadAllText(Constants.SettingsStorage.Location);
                    return Serializer.Deserialize<AllSettings>(settingsXml);
                }
                catch (InvalidOperationException e)
                {
                    if (e.InnerException is XmlException)
                    {
                        return new DefaultSettingsFactory().Create();
                    }

                    throw;
                }
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
