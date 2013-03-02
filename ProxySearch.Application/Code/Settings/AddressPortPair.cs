using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ProxySearch.Console.Code.Settings
{
    [Serializable]
    public class AddressPortPair
    {
        [XmlIgnore]
        public IPAddress IPAddress
        {
            get;
            set;
        }

        public ushort Port
        {
            get;
            set;
        }

        [XmlElement("IPAddress")]
        public string IPAddressString
        {
            get
            {
                return IPAddress.ToString();
            }
            set
            {
                IPAddress = IPAddress.Parse(value);
            }
        }
    }
}
