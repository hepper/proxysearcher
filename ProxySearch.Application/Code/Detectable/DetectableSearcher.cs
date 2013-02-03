using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProxySearch.Console.Code.Interfaces;
using ProxySearch.Engine;
using System.Reflection;

namespace ProxySearch.Console.Code.Detectable
{
    public class DetectableSearcher : IDetectableSearcher
    {
        public List<IDetectable> Get<T>()
        {
            List<Type> exclude = new List<Type>
            {
                typeof(IDetectable),
                typeof(SimpleDetectable)
            };

            return Assembly.GetExecutingAssembly().GetTypes()
                                                  .Where(type => !exclude.Contains(type))
                                                  .Where(type => typeof(IDetectable).IsAssignableFrom(type))
                                                  .Select(type => (IDetectable)Activator.CreateInstance(type))
                                                  .Where(instance => instance.Interface == typeof(T))
                                                  .ToList();
        }
    }
}
