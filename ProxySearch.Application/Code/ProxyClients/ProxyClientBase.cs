using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using ProxySearch.Common;
using ProxySearch.Console.Code.Interfaces;
using ProxySearch.Console.Code.Settings;
using ProxySearch.Engine;

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

        public event PropertyChangedEventHandler PropertyChanged;

        private static List<ProxyClientBase> Clients = new List<ProxyClientBase>();

        public ProxyClientBase(string name, string image)
        {
            Name = name;
            Image = image;
            Clients.Add(this);
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

        public abstract bool IsInstalled
        {
            get;
        }

        public ProxyInfo Proxy
        {
            get
            {
                if (Settings == null)
                {
                    return null;
                }

                return GetProxy();
            }
            set
            {
                if (value == null)
                {
                    RestoreSettings(Serializer.Deserialize<SettingsData>(Settings));
                    Settings = null;
                }
                else
                {
                    if (Settings == null)
                    {
                        Settings = Serializer.Serialize(BackupSettings());
                    }

                    SetProxy(value);
                }

                foreach (ProxyClientBase client in Clients)
                {
                    client.FirePropertyChanged("Proxy");
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
                return Context.Get<ProxyClientsSettings>()[Name];
            }
            set
            {
                Context.Get<ProxyClientsSettings>()[Name] = value;
            }
        }

        protected void FirePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
