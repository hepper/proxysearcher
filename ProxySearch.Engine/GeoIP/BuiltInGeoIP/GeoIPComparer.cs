using System.Collections;

namespace ProxySearch.Engine.GeoIP.BuiltInGeoIP
{
    public class GeoIPComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            GeoIPData data = (GeoIPData)x;
            long value = (long)y;

            if (data.StartAddress > value || data.EndAddress < value)
            {
                return (int)(data.StartAddress - value);
            }

            return 0;
        }
    }
}
