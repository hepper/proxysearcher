using System;
using ProxySearch.Common;
using ProxySearch.Console.Code.Interfaces;
using ProxySearch.Console.Properties;
using ProxySearch.Engine.Proxies;

namespace ProxySearch.Console.Code.ProxyClients
{
    public abstract class ProxyClientBase : IProxyClient
    {
        public class SettingsData
        {
            public bool UseProxy
            {
                get;
                set;
            }

            public string AddressPort
            {
                get;
                set;
            }
        }

        public ProxyClientBase(string type, string name, string settingsKey, string image, int order)
        {
            Type = type;
            Name = name;
            Image = image;
            Order = order;

            SettingsKey = settingsKey;
        }

        public string Type
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            private set;
        }

        public string Image
        {
            get;
            private set;
        }

        public int Order
        {
            get;
            private set;
        }

        public abstract bool IsInstalled
        {
            get;
        }

        private ProxyInfo ProxyCache
        {
            get;
            set;
        }

        private DateTime Timestamp
        {
            get;
            set;
        }

        private string SettingsKey
        {
            get;
            set;
        }

        public virtual ProxyInfo Proxy
        {
            get
            {
                if ((DateTime.UtcNow - Timestamp).TotalMilliseconds > 100)
                {
                    if (ImportsInternetExplorerSettings)
                    {
                        ProxyCache = Context.Get<IProxyClientSearcher>().SelectedSystemProxy.Proxy;
                    }
                    else
                    {
                        ProxyCache = Settings != null ? GetProxy() : null;
                    }
                }

                Timestamp = DateTime.UtcNow;

                return ProxyCache;
            }
            set
            {
                if (value == null)
                {
                    if (ImportsInternetExplorerSettings)
                    {
                        Context.Get<IProxyClientSearcher>().SelectedSystemProxy.Proxy = value;
                        return;
                    }

                    if (Settings != null)
                    {
                        RestoreSettings(Serializer.Deserialize<SettingsData>(Settings));
                        Settings = null;
                    }
                }
                else
                {
                    if (Settings == null)
                        Settings = Serializer.Serialize(BackupSettings());

                    SetProxy(value);
                }
            }
        }

        protected string GetProtocolName(string httpValue, string socksValue)
        {
            if (Type == Resources.HttpProxyType)
                return httpValue;

            if (Type == Resources.SocksProxyType)
                return socksValue;

            throw new NotSupportedException();
        }

        protected abstract void SetProxy(ProxyInfo proxyInfo);
        protected abstract ProxyInfo GetProxy();
        protected abstract bool ImportsInternetExplorerSettings
        {
            get;
        }

        protected abstract SettingsData BackupSettings();
        protected abstract void RestoreSettings(SettingsData settings);

        private string Settings
        {
            get
            {
                return Context.Get<ProxyClientsSettings>()[SettingsKey];
            }
            set
            {
                Context.Get<ProxyClientsSettings>()[SettingsKey] = value;
            }
        }
    }
}
