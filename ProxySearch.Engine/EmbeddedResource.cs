using System.IO;
using System.Reflection;

namespace ProxySearch.Engine
{
    public static class EmbeddedResource
    {
        public static string ReadToEnd(string path)
        {
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
