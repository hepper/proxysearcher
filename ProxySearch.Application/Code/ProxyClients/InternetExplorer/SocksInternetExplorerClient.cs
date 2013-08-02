using ProxySearch.Console.Properties;

namespace ProxySearch.Console.Code.ProxyClients.InternetExplorer
{
    public class SocksInternetExplorerClient : InternetExplorerClientBase
    {
        public SocksInternetExplorerClient()
            : base(Resources.SocksProxyType)
        {
        }

        public SocksInternetExplorerClient(string name, string image, int order, string clientName)
            : base(Resources.SocksProxyType, name, image, order, clientName)
        {
        }
    }
}
