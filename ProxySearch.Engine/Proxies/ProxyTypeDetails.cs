using System;
using System.Net;

namespace ProxySearch.Engine.Proxies
{
    public abstract class ProxyTypeDetails
    {
        public ProxyTypeDetails(string type, string name, string details, IPAddress outgoingIPAddress)
        {
            Type = type;
            Name = name;
            Details = details;
            OutgoingIPAddress = outgoingIPAddress;
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

        public IPAddress OutgoingIPAddress
        {
            get;
            private set;
        }

        public DetailsType GetStrongType<DetailsType>() where DetailsType : struct
        {
            if (!Enum.IsDefined(typeof(DetailsType), Type))
            {
                throw new ArgumentException(string.Format("Type '{0}' doesn't contain enum value '{1}'", typeof(DetailsType).FullName, Type));
            }

            return (DetailsType)Enum.Parse(typeof(DetailsType), Type);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
