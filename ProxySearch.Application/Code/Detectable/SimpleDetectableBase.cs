using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProxySearch.Console.Code.Interfaces;
using System.Windows.Controls;

namespace ProxySearch.Console.Code.Detectable
{
    public abstract class SimpleDetectableBase<InterfaceType, ImplementationType> : IDetectable
    {
        public SimpleDetectableBase(string friendlyName, string description, int order)
        {            
            FriendlyName = friendlyName;
            Description = description;
            Interface = typeof(InterfaceType);
            Implementation = typeof(ImplementationType);
            Order = order;
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
