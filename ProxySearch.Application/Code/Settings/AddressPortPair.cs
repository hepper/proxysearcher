using System;
using System.Net;
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
