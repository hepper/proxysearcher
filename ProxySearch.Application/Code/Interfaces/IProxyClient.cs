using System;
using ProxySearch.Engine.Proxies;

namespace ProxySearch.Console.Code.Interfaces
{
    public interface IProxyClient : IProxyType
    {
        string Name { get; }
        string Image { get; }
        bool IsInstalled { get; }
        ProxyInfo Proxy { get; set; }
        int Order { get; }

        event Action ProxyChanged;
    }
}
