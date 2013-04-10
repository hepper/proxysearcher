using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ProxySearch.Engine.Proxies
{
    public class Proxy
    {
        public Proxy(IPAddress address, ushort port)
        {
            Init(address, port);
        }

        public Proxy(string addressPort)
        {
            string[] args = addressPort.Split(':');

            if (args.Length != 2)
                throw new ArgumentException();

            Init(IPAddress.Parse(args[0]), ushort.Parse(args[1]));
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
            if ((object)obj == null)
            {
                return false;
            }

            if (GetType() != obj.GetType())
            {
                return false;
            }

            Proxy proxy = (Proxy)obj;

            return this.Address.ToString() == proxy.Address.ToString() && this.Port == proxy.Port;
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
            return Address.GetHashCode() + Port.GetHashCode();
        }
    }
}
