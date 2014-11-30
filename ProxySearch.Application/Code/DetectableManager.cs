using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using ProxySearch.Common;
using ProxySearch.Console.Code.Interfaces;
using ProxySearch.Console.Code.Settings;
using ProxySearch.Console.Properties;
using ProxySearch.Engine.Error;

namespace ProxySearch.Console.Code
{
    public class DetectableManager : IDetectableManager
    {
        public IDetectable CreateDetectableInstance<T>(string typeName)
        {
            try
            {
                return (IDetectable)Activator.CreateInstance(Type.GetType(typeName, true));
            }
            catch
            {
                return CreateDetectableInstance<T>(Context.Get<IDetectableManager>().Find<T>().First().GetType().AssemblyQualifiedName);
            }
        }

        public T CreateImplementationInstance<T>(IDetectable detectable, List<ParametersPair> parametersList, List<object> interfacesList)
        {
            try
            {
                return CreateImplementationInstanceInternal<T>(detectable, parametersList, interfacesList);
            }
            catch (TargetInvocationException exception)
            {
                throw exception.InnerException;
            }
            catch (MissingMethodException)
            {
                throw new ConfigurationErrorsException(Resources.OldConfigurationException);
            }
        }

        private static T CreateImplementationInstanceInternal<T>(IDetectable detectable, List<ParametersPair> parametersList, List<object> interfacesList)
        {
            Type type = detectable.Implementation;
            ParametersPair parameterPair = parametersList.SingleOrDefault(item => item.TypeName == detectable.GetType().AssemblyQualifiedName);

            if (parameterPair == null && !interfacesList.Any())
                return (T)Activator.CreateInstance(type);
            else
            {
                List<object> parameters = new List<object>();
                if (parameterPair != null)
                    parameters.AddRange(parameterPair.Parameters);

                parameters.AddRange(interfacesList);

                return (T)Activator.CreateInstance(type, parameters.ToArray());
            }
        }

        public List<IDetectable> Find<T>(params Type[] ignoredTypes)
        {
            return Assembly.GetExecutingAssembly().GetTypes()
                                                 .Where(type => !type.IsAbstract && typeof(IDetectable).IsAssignableFrom(type))
                                                 .Where(type => ignoredTypes.All(item => !item.IsAssignableFrom(type)))
                                                 .Select(type => (IDetectable)Activator.CreateInstance(type))
                                                 .Where(instance => instance.Interface == typeof(T))
                                                 .OrderBy(instance => instance.Order)
                                                 .ToList();
        }

        public List<IDetectable> Find<T>(string proxyType, params Type[] ignoredTypes)
        {
            return Find<T>(ignoredTypes).Where(item => item.ProxyType == proxyType).ToList();
        }

        public List<IDetectable> Find<T>(IDetectable proxyTypeDetectable, params Type[] ignoredTypes)
        {
            IProxyType proxyTypeInstance = (IProxyType)Activator.CreateInstance(proxyTypeDetectable.Implementation);

            return Find<T>(proxyTypeInstance.Type, ignoredTypes);
        }
    }
}
