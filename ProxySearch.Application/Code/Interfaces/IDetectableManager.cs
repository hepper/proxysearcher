using System;
using System.Collections.Generic;
using ProxySearch.Console.Code.Settings;

namespace ProxySearch.Console.Code.Interfaces
{
    public interface IDetectableManager
    {
        IDetectable CreateDetectableInstance<T>(string typeName);
        T CreateImplementationInstance<T>(IDetectable detectable, List<ParametersPair> parametersList, List<object> interfacesList);
        List<IDetectable> Find<T>(params Type[] ignoredTypes);
        List<IDetectable> Find<T>(string proxyType, params Type[] ignoredTypes);
        List<IDetectable> Find<T>(IDetectable proxyTypeDetectable, params Type[] ignoredTypes);
    }
}
