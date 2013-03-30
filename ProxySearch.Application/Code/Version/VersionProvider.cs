using ProxySearch.Console.Code.Interfaces;
using ProxySearch.Console.Properties;

namespace ProxySearch.Console.Code.Version
{
    public class VersionProvider : IVersionProvider
    {
        public int Version
        {
            get
            {
                return 9;
            }
        }

        public string Name
        {
            get
            {
                return Resources.ProxySearcherVersion;
            }
        }
    }
}
