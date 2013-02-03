using System;
using ProxySearch.Console.Properties;
using ProxySearch.Engine.GeoIP;

namespace ProxySearch.Console.Code.Detectable.GeoIPs
{
    public class DummyGeoIPDetectable : SimpleDetectable
    {
        public override string FriendlyName
        {
            get
            {
                return Resources.DummyGeoIP;
            }
        }

        public override string Description
        {
            get 
            {
                return Resources.DummyGeoIPDescription;
            }
        }

        public override Type Interface
        {
            get
            {
                return typeof(IGeoIP);
            }
        }

        public override Type Implementation
        {
            get
            {
                return typeof(TurnOffGeoIP);
            }
        }
    }
}
