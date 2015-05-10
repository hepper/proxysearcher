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
            try
            {
                CultureInfo culture = Thread.CurrentThread.CurrentCulture;

                JToken response = Reader.Find(ipAddress);

                if (response == null)
                    return Task.FromResult(CountryInfo.Empty);

                JToken country = response["country"];

                if (country == null)
                    return Task.FromResult(CountryInfo.Empty);

                JToken names = country["names"];

                JToken result = names[culture.Name] ?? (culture.Parent == null
                                                        ? names["en"]
                                                        : (names[culture.Parent.Name] ?? names["en"]));

                if (result == null)
                    return Task.FromResult(CountryInfo.Empty);

                return Task.FromResult(new CountryInfo
                {
                    Code = result.ToString(),
                    Name = result.ToString()
                });
            }
            catch
            {
                return Task.FromResult(CountryInfo.Empty);
            }
        }
    }
}