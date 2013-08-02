using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ProxySearch.Common;
using ProxySearch.Console.Code.Interfaces;
using ProxySearch.Console.Code.Settings;

namespace ProxySearch.Console.Code.ProxyClients
{
    public class ProxyClientSearcher : IProxyClientSearcher
    {
        private Dictionary<string, List<IProxyClient>> clients;

        public ProxyClientSearcher()
        {
            clients = Assembly.GetExecutingAssembly().GetTypes()
                                  .Where(type => !type.IsAbstract)
                                  .Where(type => typeof(IProxyClient).IsAssignableFrom(type))
                                  .Select(type => (IProxyClient)Activator.CreateInstance(type))
                                  .Where(instance => instance.IsInstalled)
                                  .GroupBy(proxyClient => proxyClient.Type)
                                  .ToDictionary(group => group.Key, group => group.OrderBy(instance => instance.Order).ToList());
        }

        public List<IProxyClient> Clients
        {
            get
            {
                string proxyType = Context.Get<AllSettings>().SelectedTabSettings.ProxyType;

                return clients.ContainsKey(proxyType) ? clients[proxyType] : new List<IProxyClient>();
            }
        }
    }
}
