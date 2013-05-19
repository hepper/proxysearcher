using System;
using System.Collections.Generic;

namespace ProxySearch.Console.Code.Interfaces
{
    public interface IDetectable
    {
        string FriendlyName
        {
            get;
        }

        string Description
        {
            get;
        }

        Type Interface
        {
            get;
        }

        Type Implementation
        {
            get;
        }

        Type PropertyPage
        {
            get;
        }

        List<object> DefaultSettings
        {
            get;
        }

        List<object> InterfaceSettings
        {
            get;
        }

        int Order
        {
            get;
        }

        string[] SupportedProxyTypes
        {
            get;
        }
    }
}
