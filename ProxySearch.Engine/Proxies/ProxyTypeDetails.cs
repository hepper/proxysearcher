using System;
using ProxySearch.Engine.Properties;

namespace ProxySearch.Engine.Proxies
{
    public abstract class ProxyTypeDetails
    {
        public ProxyTypeDetails(string type, string name, string details)
        {
            Type = type;
            Name = name;
            Details = details;
        }

        public string Name
        {
            get;
            protected set;
        }

        public string Details
        {
            get;
            protected set;
        }

        public string Type
        {
            get;
            private set;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
