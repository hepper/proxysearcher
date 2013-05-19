using System;
using System.Collections.Generic;
using ProxySearch.Console.Code.Interfaces;

namespace ProxySearch.Console.Code.Detectable
{
    public abstract class SimpleDetectableBase<InterfaceType, ImplementationType> : IDetectable
    {
        public SimpleDetectableBase(string friendlyName, string description, int order, string[] supportedProxyTypes)
        {
            FriendlyName = friendlyName;
            Description = description;
            Interface = typeof(InterfaceType);
            Implementation = typeof(ImplementationType);
            Order = order;
            SupportedProxyTypes = supportedProxyTypes;
        }

        public SimpleDetectableBase(string friendlyName, string description, int order)
            : this(friendlyName, description, order, new string[] { })
        {
        }

        public string FriendlyName
        {
            get;
            private set;
        }

        public string Description
        {
            get;
            private set;
        }

        public Type Interface
        {
            get;
            private set;
        }

        public Type Implementation
        {
            get;
            private set;
        }

        public int Order
        {
            get;
            private set;
        }

        public string[] SupportedProxyTypes
        {
            get;
            private set;
        }

        public virtual Type PropertyPage
        {
            get
            {
                return null;
            }
        }

        public virtual List<object> DefaultSettings
        {
            get
            {
                return new List<object>();
            }
        }

        public virtual List<object> InterfaceSettings
        {
            get
            {
                return new List<object>();
            }
        }
    }
}
