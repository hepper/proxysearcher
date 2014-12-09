using ProxySearch.Console.Code.ProxyClients.InternetExplorer;
using ProxySearch.Console.Properties;

namespace ProxySearch.Console.Code.ProxyClients.Chrome
{
    public class SocksChromeClient: SocksInternetExplorerClient
    {
        public SocksChromeClient()
            : base(Resources.Chrome, "/Images/Chrome.png", 3, "Google Chrome")
        {
        }

        protected override bool ImportsInternetExplorerSettings
        {
            get
            {
                return true;
            }
        }
    }
}
