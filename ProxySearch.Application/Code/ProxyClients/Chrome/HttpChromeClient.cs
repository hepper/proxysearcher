using ProxySearch.Console.Code.ProxyClients.InternetExplorer;
using ProxySearch.Console.Properties;

namespace ProxySearch.Console.Code.ProxyClients.Chrome
{
    public class ChromeClient : HttpInternetExplorerClient
    {
        public ChromeClient()
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
