using System;
using ProxySearch.Console.Properties;
using ProxySearch.Engine.GeoIP;

namespace ProxySearch.Console.Code.Detectable.GeoIPs
{
    public class GeoIPDetectable : SimpleDetectable
    {
        public override string FriendlyName
        {
            get 
            {
                return Resources.WebServiceNetGeoIPService;
            }
        }

        public override string Description
        {
            get 
            {
                return Resources.WebServiceNetGeoIPServiceDescription;
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
                return typeof(GeoIPServiceAdapter);
            }
        }
    }
}
