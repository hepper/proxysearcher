using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace ProxySearch.Engine.GeoIP.BuiltInGeoIP
{
    public class GeoIPDatabase
    {
        public List<GeoIPData> Read()
        {
            List<GeoIPData> data = new List<GeoIPData>();

            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ProxySearch.Engine.Resources.GeoIPDatabase.csv"))
            using (StreamReader reader = new StreamReader(stream))
            {
                reader.ReadLine();
                reader.ReadLine();

                while (!reader.EndOfStream)
                {
                    string[] values = reader.ReadLine().Split(',');

                    data.Add(new GeoIPData
                    {
                        StartAddress = long.Parse(values[0]),
                        EndAddress = long.Parse(values[1]),
                        Code = values[2],
                        Name = values[3]
                    });
                }
            }

            return data;
        }
    }
}
