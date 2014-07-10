using ProxySearch.Console.Code.Interfaces;

namespace ProxySearch.Console.Code.Version
{
    public class VersionProvider : IVersionProvider
    {
        public int Version
        {
            get
            {
                return 26;
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
