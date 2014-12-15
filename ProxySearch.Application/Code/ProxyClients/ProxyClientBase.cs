using System;
using ProxySearch.Console.Code.Interfaces;
using ProxySearch.Console.Properties;
using ProxySearch.Engine.Proxies;

namespace ProxySearch.Console.Code.ProxyClients
{
    public abstract class ProxyClientBase : IProxyClient
    {
        public event Action ProxyChanged;

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

        public bool IsProxyChangeCancelled
        {
            get;
            set;
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
                    if (ImportsInternetExplorerSettings && Context.Get<IProxyClientSearcher>().GetInternetExplorerClientOrNull(Type) != null)
                    {
                        ProxyCache = Context.Get<IProxyClientSearcher>().GetInternetExplorerClientOrNull(Type).Proxy;
                    }
                    else
                    {
                        ProxyCache = GetProxy();
                    }
                }

                Timestamp = DateTime.UtcNow;

                return ProxyCache;
            }
            set
            {
                IProxyClient IEClient = Context.Get<IProxyClientSearcher>().GetInternetExplorerClientOrNull(Type);

                if (ImportsInternetExplorerSettings && IEClient != null)
                {
                    IEClient.Proxy = value;
                    IsProxyChangeCancelled = IEClient.IsProxyChangeCancelled;
                    return;
                }

                IsProxyChangeCancelled = false;

                SetProxy(value);

                if (ProxyChanged != null)
                {
                    ProxyChanged();
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
    }
}