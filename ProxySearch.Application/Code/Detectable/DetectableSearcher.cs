using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ProxySearch.Console.Code.Interfaces;

namespace ProxySearch.Console.Code.Detectable
{
    public class DetectableSearcher : IDetectableSearcher
    {
        public List<IDetectable> Get<T>()
        {
            return Assembly.GetExecutingAssembly().GetTypes()
                                                  .Where(type => !type.IsAbstract && typeof(IDetectable).IsAssignableFrom(type))
                                                  .Select(type => (IDetectable)Activator.CreateInstance(type))
                                                  .Where(instance => instance.Interface == typeof(T))
                                                  .OrderBy(instance => instance.Order)
                                                  .ToList();
        }

        public List<IDetectable> Get<T>(IDetectable proxyTypeDetectable)
        {
            IProxyType proxyType = (IProxyType)Activator.CreateInstance(proxyTypeDetectable.Implementation);

            return Get<T>().Where(item => item.ProxyType == proxyType.Type).ToList();
        }
    }
}
