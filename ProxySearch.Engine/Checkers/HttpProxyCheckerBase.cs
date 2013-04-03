using System;
using System.Threading.Tasks;
using ProxySearch.Common;
using ProxySearch.Engine.Properties;
using ProxySearch.Engine.Proxies;
using ProxySearch.Engine.Proxies.Http;

namespace ProxySearch.Engine.Checkers
{
    public abstract class HttpProxyCheckerBase : ProxyCheckerBase
    {
        protected async override Task<object> GetProxyDetails(ProxyInfo proxy)
        {
            string result = await Context.Get<CheckerUtils>().GetContentOrNull(ProxyTypeDetectorUrl, new ProxyInfo(proxy.Address, proxy.Port));

            if (result == null)
                return new HttpProxyDetails(HttpProxyTypes.CannotVerify);

            HttpProxyTypes proxyType;

            if (!Enum.TryParse(result, out proxyType))
                return new HttpProxyDetails(HttpProxyTypes.ChangesContent);

            return new HttpProxyDetails(proxyType);
        }

        private string ProxyTypeDetectorUrl
        {
            get
            {
                return string.Format(Resources.ProxyTypeDetectorUrl, Guid.NewGuid());
            }
        }
    }
}
