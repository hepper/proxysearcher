using System;
using ProxySearch.Console.Properties;
using ProxySearch.Engine.GeoIP;

namespace ProxySearch.Console.Code.Detectable.GeoIPs
{
    public class DummyGeoIPDetectable : SimpleDetectableBase<IGeoIP, TurnOffGeoIP>
    {
        public DummyGeoIPDetectable()
            : base(Resources.DummyGeoIP, Resources.DummyGeoIPDescription, 0, null)
        {
        }
    }
}
