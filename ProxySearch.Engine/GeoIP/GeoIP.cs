using System.Threading.Tasks;
using ProxySearch.Engine.Properties;

namespace ProxySearch.Engine.GeoIP
{
    public class TurnOffGeoIP : IGeoIP
    {
        public async Task<CountryInfo> GetLocation(string ipAddress)
        {
            return await Task.Run<CountryInfo>(() => new CountryInfo
            {
                Code = null,
                Name = Resources.TurnedOffGeoIP
            });
        }
    }
}
