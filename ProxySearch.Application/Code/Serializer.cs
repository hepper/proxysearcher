using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace ProxySearch.Console.Code
{
    public static class Serializer
    {
        public static string Serialize<T>(T obj)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (TextWriter streamWriter = new StreamWriter(memoryStream))
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                    xmlSerializer.Serialize(streamWriter, obj);

                    return Encoding.UTF8.GetString(memoryStream.ToArray());
                }
            }
        }

        public static T Deserialize<T>(string obj)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(T));

            using (StringReader reader = new StringReader(obj))
            {
                return (T)deserializer.Deserialize(reader);
            }
        }
    }
}
