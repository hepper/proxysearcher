using ProxySearch.Console.Properties;

namespace ProxySearch.Console.Code.ProxyClients.InternetExplorer
{
    public class HttpInternetExplorerClient : InternetExplorerClientBase
    {
        public HttpInternetExplorerClient()
            : base(Resources.HttpProxyType)
        {
        }

        public HttpInternetExplorerClient(string name, string image, int order, string clientName)
            : base(Resources.HttpProxyType, name, image, order, clientName)
        {
        }
    }
}
