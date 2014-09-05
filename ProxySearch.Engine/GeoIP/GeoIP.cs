using System.Threading.Tasks;

namespace ProxySearch.Engine.GeoIP
{
    public class TurnOffGeoIP : IGeoIP
    {
        public async Task<CountryInfo> GetLocation(string ipAddress)
        {
            return await Task.FromResult(new CountryInfo
            {
                Code = null,
                Name = string.Empty
            });
        }
    }
}
