using System.Collections.Generic;
using ProxySearch.Console.Code.Interfaces;

namespace ProxySearch.Console.Code.Detectable
{
    public interface IDetectableSearcher
    {
        List<IDetectable> Get<T>();
        List<IDetectable> Get<T>(IDetectable proxyTypeDetextable);
    }
}
