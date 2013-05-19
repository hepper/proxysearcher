using ProxySearch.Console.Code.Interfaces;
using ProxySearch.Console.Code.ProxyType;
using ProxySearch.Console.Properties;

namespace ProxySearch.Console.Code.Detectable.ProxyType
{
    public class HttpProxyTypeDetectable : SimpleDetectableBase<IProxyType, HttpProxyType> 
    {
        public HttpProxyTypeDetectable()
            : base(Resources.HttpProxyType, Resources.HttpProxyTypeDescription, 0)
        {
        }
    }
}
