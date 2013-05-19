using ProxySearch.Console.Code.Interfaces;
using ProxySearch.Console.Code.ProxyType;
using ProxySearch.Console.Properties;

namespace ProxySearch.Console.Code.Detectable.ProxyType
{
    public class SocksProxyTypeDetectable : SimpleDetectableBase<IProxyType, SocksProxyType>
    {
        public SocksProxyTypeDetectable()
            : base(Resources.SocksProxyType, Resources.SocksProxyTypeDesciption, 1)
        {

        }
    }
}
