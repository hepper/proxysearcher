using ProxySearch.Engine;
using ProxySearch.Console.Properties;
using System.Threading.Tasks;
using ProxySearch.Common;
using System.Threading;

namespace ProxySearch.Console.Code
{
    public class GeoIPServiceAdapter : ProxySearch.Engine.GeoIP.IGeoIP
    {
        public async Task<CountryInfo> GetLocation(string ipAddress)
        {
            return await Task.Run<CountryInfo>(() =>
            {
                try
                {
                    GeoIPService.GeoIP info = new GeoIPService.GeoIPServiceSoapClient().GetGeoIP(ipAddress);
                    return new CountryInfo
                    {
                        Code = info.CountryCode,
                        Name = info.CountryName
                    };
                }
                catch
                {
                    return new CountryInfo
                    {
                        Code = null,
                        Name = null
                    };
                }
            }, Context.Get<CancellationTokenSource>().Token);
        }
    }
}
