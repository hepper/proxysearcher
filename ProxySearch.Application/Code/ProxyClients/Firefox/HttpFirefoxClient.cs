using ProxySearch.Console.Properties;

namespace ProxySearch.Console.Code.ProxyClients.Firefox
{
    public class HttpFirefoxClient : FirefoxClientBase
    {
        public HttpFirefoxClient()
            : base(Resources.HttpProxyType)
        {
        }
    }
}
