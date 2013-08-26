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
                return 14;
            }
        }

        public string VersionString
        {
            get
            {
                int minor = Version % 10;
                int major = (Version - Version % 10) / 10 + 1;

                return string.Format(Properties.Resources.ProxySearcherVersionFormat, major, minor);
            }
        }
    }
}
