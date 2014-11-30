using System;
using System.Net;
using System.Threading.Tasks;

namespace ProxySearch.Engine.GeoIP.BuiltInGeoIP
{
    public class GeoIP : IGeoIP
    {
        private static GeoIPData[] OrderedData
        {
            get;
            set;
        }

        static GeoIP()
        {
            OrderedData = new GeoIPDatabase().Read().ToArray();

            Array.Sort<GeoIPData>(OrderedData, (item1, item2) =>
            {
                return (int)(item1.StartAddress - item2.StartAddress);
            });
        }

        public async Task<CountryInfo> GetLocation(string ipAddress)
        {
            return await Task.Run(() =>
            {
                IPAddress address = IPAddress.Parse(ipAddress);
                byte[] bytes = address.GetAddressBytes();

                long group = (long)16777216 * bytes[0] + (long)65536 * bytes[1] + (long)256 * bytes[2] + (long)bytes[3];

                int index = Array.BinarySearch(OrderedData, group, new GeoIPComparer());

                if (index < 0)
                    return new CountryInfo
                    {
                        Code = string.Empty,
                        Name = string.Empty
                    };

                return new CountryInfo
                {
                    Code = OrderedData[index].Code,
                    Name = OrderedData[index].Name
                };
            });
        }
    }
}