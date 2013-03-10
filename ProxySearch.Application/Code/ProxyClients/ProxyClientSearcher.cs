using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ProxySearch.Console.Code.Interfaces;

namespace ProxySearch.Console.Code.ProxyClients
{
    public class ProxyClientSearcher : IProxyClientSearcher
    {
        private List<IProxyClient> clients;

        public ProxyClientSearcher()
        {
            clients = Assembly.GetExecutingAssembly().GetTypes()
                                  .Where(type => !type.IsAbstract)
                                  .Where(type => typeof(IProxyClient).IsAssignableFrom(type))
                                  .Select(type => (IProxyClient)Activator.CreateInstance(type))
                                  .Where(instance => instance.IsInstalled)
                                  .OrderBy(instance => instance.Order)
                                  .ToList();

        }

        public List<IProxyClient> Clients
        {
            get
            {
                return clients;
            }
        }
    }
}
