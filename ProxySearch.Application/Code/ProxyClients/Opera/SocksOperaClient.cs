using ProxySearch.Console.Properties;

namespace ProxySearch.Console.Code.ProxyClients.Opera
{
    public class SocksOperaClient : OperaClientBase
    {
        public SocksOperaClient()
            : base(Resources.SocksProxyType)
        {
        }
    }
}
