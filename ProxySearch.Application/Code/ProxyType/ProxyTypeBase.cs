using ProxySearch.Console.Code.Interfaces;

namespace ProxySearch.Console.Code.ProxyType
{
    public class ProxyTypeBase : IProxyType
    {
        public ProxyTypeBase(string type)
        {
            Type = type;
        }

        public string Type
        {
            get;
            private set;
        }
    }
}
