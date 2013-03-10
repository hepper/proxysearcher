using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProxySearch.Console.Code.Interfaces;
using System.Windows.Controls;

namespace ProxySearch.Console.Code.Detectable
{
    public abstract class SimpleDetectable : IDetectable
    {
        public abstract string FriendlyName
        {
            get;
        }

        public abstract string Description
        {
            get;
        }

        public abstract Type Interface
        {
            get;
        }

        public abstract Type Implementation
        {
            get;
        }

        public List<object> DefaultSettings
        {
            get
            {
                return new List<object>();
            }
        }

        public Type PropertyPage
        {
            get
            {
                return null;
            }
        }

        public List<object> InterfaceSettings
        {
            get
            {
                return new List<object>();
            }
        }
    }
}
