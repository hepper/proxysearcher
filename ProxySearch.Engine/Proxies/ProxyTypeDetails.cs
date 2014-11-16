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

        public override string ToString()
        {
            return Name;
        }
    }
}
