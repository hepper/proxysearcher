using System;
using System.ComponentModel;
using ProxySearch.Common;
using ProxySearch.Console.Code.Interfaces;
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

        public ProxyInfo Proxy
        {
            get
            {
                if ((DateTime.UtcNow - Timestamp).TotalMilliseconds > 100)
                {
                    ProxyCache = Settings != null ? GetProxy() : null;
                }

                Timestamp = DateTime.UtcNow;

                return ProxyCache;
            }
            set
            {
                if (value == null)
                {
                    if (Settings != null)
                    {
                        RestoreSettings(Serializer.Deserialize<SettingsData>(Settings));
                        Settings = null;
                    }
                }
                else
                {
                    if (Settings == null)
                    {
                        Settings = Serializer.Serialize(BackupSettings());
                    }

                    SetProxy(value);
                }
            }
        }

        protected abstract void SetProxy(ProxyInfo proxyInfo);
        protected abstract ProxyInfo GetProxy();

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
