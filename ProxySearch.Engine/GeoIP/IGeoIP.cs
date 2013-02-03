using System.Threading.Tasks;

namespace ProxySearch.Engine.GeoIP
{
    public interface IGeoIP
    {
        Task<CountryInfo> GetLocation(string ipAddress);
    }
}
