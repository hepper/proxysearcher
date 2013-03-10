using System;
using ProxySearch.Console.Properties;
using ProxySearch.Engine.GeoIP;
using ProxySearch.Engine.GeoIP.BuiltInGeoIP;

namespace ProxySearch.Console.Code.Detectable.GeoIPs
{
    public class BuildInGeoIPDetectable : SimpleDetectableBase<IGeoIP, GeoIP>
    {
        public BuildInGeoIPDetectable()
            : base(Resources.BuiltInGeoIPName, Resources.BuiltInGeoIPDescription, 1)
        {
        }
    }
}
