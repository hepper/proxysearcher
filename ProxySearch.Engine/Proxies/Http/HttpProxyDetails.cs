using ProxySearch.Engine.Properties;

namespace ProxySearch.Engine.Proxies.Http
{
    public class HttpProxyInfo
    {
        public string Type
        {
            get;
            set;
        }

        public string TypeDetails
        {
            get;
            set;
        }

        public override string ToString()
        {
            return Type;
        }
    }
}
