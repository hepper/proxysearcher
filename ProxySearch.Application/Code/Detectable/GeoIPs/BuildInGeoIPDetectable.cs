using System;
using ProxySearch.Console.Properties;
using ProxySearch.Engine.GeoIP;
using ProxySearch.Engine.GeoIP.BuiltInGeoIP;

namespace ProxySearch.Console.Code.Detectable.GeoIPs
{
    public class BuildInGeoIPDetectable : SimpleDetectable
    {
        public override string FriendlyName
        {
            get 
            {
                return Resources.BuiltInGeoIPName;
            }
        }

        public override string Description
        {
            get 
            {
                return Resources.BuiltInGeoIPDescription;
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
                return typeof(GeoIP);
            }
        }
    }
}
