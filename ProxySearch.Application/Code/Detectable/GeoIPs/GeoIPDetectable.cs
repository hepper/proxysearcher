using System;
using ProxySearch.Console.Properties;
using ProxySearch.Engine.GeoIP;

namespace ProxySearch.Console.Code.Detectable.GeoIPs
{
    public class GeoIPDetectable : SimpleDetectableBase<IGeoIP, GeoIPServiceAdapter>
    {
        public GeoIPDetectable()
            : base(Resources.WebServiceNetGeoIPService, Resources.WebServiceNetGeoIPServiceDescription, 2)
        {
        }
    }
}
