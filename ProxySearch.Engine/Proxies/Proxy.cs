using System;
using System.Net;

namespace ProxySearch.Engine.Proxies
{
    public class Proxy
    {
        public Proxy(string addressPort)
        {
            string[] args = addressPort.Split(':');

            if (args.Length != 2)
                throw new ArgumentException();

            Init(args[0], args[1]);
        }

        public Proxy(string address, string port)
        {
            Init(address, port);
        }

        public Proxy(IPAddress address, ushort port)
        {
            Init(address, port);
        }

        private void Init(string address, string port)
        {
            Init(IPAddress.Parse(address), ushort.Parse(port));
        }

        private void Init(IPAddress address, ushort port)
        {
            Address = address;
            Port = port;
        }

        public IPAddress Address
        {
            get;
            private set;
        }

        public ushort Port
        {
            get;
            private set;
        }

        public string AddressPort
        {
            get
            {
                return string.Format("{0}:{1}", Address, Port);
            }
        }

        public override bool Equals(object obj)
        {
            Proxy proxy = obj as Proxy;

            if (proxy == null)
            {
                return false;
            }

            if (Address == null || proxy.Address == null)
            {
                return Address == proxy.Address && Port == proxy.Port;
            }

            return  Address.ToString() == proxy.Address.ToString() && Port == proxy.Port;
        }

        public static bool operator ==(Proxy a, Proxy b)
        {
            if (Object.ReferenceEquals(a, b))
            {
                return true;
            }

            if ((object)a == null)
            {
                return false;
            }

            return a.Equals(b);
        }

        public static bool operator !=(Proxy a, Proxy b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            if (Address == null)
            {
                return Port.GetHashCode();
            }

            return Address.GetHashCode() + Port.GetHashCode();
        }

        public override string ToString()
        {
            return AddressPort;
        }
    }
}
