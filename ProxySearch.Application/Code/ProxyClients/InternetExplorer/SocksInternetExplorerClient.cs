using System.Windows;
using ProxySearch.Console.Code.Interfaces;
using ProxySearch.Console.Properties;
using ProxySearch.Engine.Proxies;
using ProxySearch.Engine.Proxies.Socks;

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

        protected override void SetProxy(ProxyInfo proxyInfo)
        {
            if (proxyInfo != null)
            {
                SocksProxyTypes type = ((SocksProxyDetails)proxyInfo.Details.Details).StrongType;

                if (type == SocksProxyTypes.Socks5)
                {
                    Context.Get<IMessageBox>().Information(Resources.ThisClientDoesntSupportSocks5Proxies);
                    IsProxyChangeCancelled = true;
                    return;
                }

                if (type != SocksProxyTypes.Socks4)
                {
                    if (Context.Get<IMessageBox>().YesNoQuestion(Resources.TypeOfProxyIsNotDefinedDoYouWantToContinue) == MessageBoxResult.No)
                    {
                        IsProxyChangeCancelled = true;
                        return;
                    }
                }
            }

            base.SetProxy(proxyInfo);
        }
    }
}
