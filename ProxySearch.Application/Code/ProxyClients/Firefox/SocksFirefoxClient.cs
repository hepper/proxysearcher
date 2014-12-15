using ProxySearch.Console.Code.Interfaces;
using ProxySearch.Console.Properties;
using ProxySearch.Engine.Proxies;
using ProxySearch.Engine.Proxies.Socks;

namespace ProxySearch.Console.Code.ProxyClients.Firefox
{
    public class SocksFirefoxClient : FirefoxClientBase
    {
        public SocksFirefoxClient()
            : base(Resources.SocksProxyType)
        {
        }

        protected override string SetProxy(ProxyInfo proxyInfo, string content)
        {
            SocksProxyTypes? type = proxyInfo != null? (SocksProxyTypes?) ((SocksProxyDetails)proxyInfo.Details.Details).StrongType : null;

            if (type.HasValue)
            {
                if (type != SocksProxyTypes.Socks4 && type != SocksProxyTypes.Socks5)
                {
                    Context.Get<IMessageBox>().Information(Resources.CannotSetProxyForFirefoxWhenSocksVersionIsNotDefined);
                    IsProxyChangeCancelled = true;
                    return content;
                }
            }

            content = base.SetProxy(proxyInfo, content);

            if (type.HasValue)
            {
                return WritePref(content, "network.proxy.socks_version", ((int)type).ToString());
            }

            return content;
        }
    }
}