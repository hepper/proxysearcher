using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using MaxMind.Db;
using Newtonsoft.Json.Linq;

namespace ProxySearch.Engine.GeoIP.BuiltInGeoIP
{
    public class GeoIP : IGeoIP
    {
        private static Reader Reader
        {
            get;
            set;
        }

        static GeoIP()
        {
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ProxySearch.Engine.Resources.GeoLite2-Country.mmdb"))
            {
                Reader = new Reader(stream);
            }
        }

        public Task<CountryInfo> GetLocation(string ipAddress)
        {
            CultureInfo culture = Thread.CurrentThread.CurrentCulture;

            JToken response = Reader.Find(ipAddress);

            JToken countries = response["country"]["names"];

            JToken result = countries[culture.Name] ?? (culture.Parent == null
                                                      ? countries["en"]
                                                      : (countries[culture.Parent.Name] ?? countries["en"]));

            if (result == null)
            {
                return Task.FromResult(new CountryInfo
                {
                    Code = string.Empty,
                    Name = string.Empty
                });
            }

            return Task.FromResult(new CountryInfo 
            {
                Code = result.ToString(),
                Name = result.ToString()
            });
        }
    }
}