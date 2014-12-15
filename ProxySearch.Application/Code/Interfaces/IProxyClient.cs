using System;
using ProxySearch.Engine.Proxies;

namespace ProxySearch.Console.Code.Interfaces
{
    public interface IProxyClient : IProxyType
    {
        string Name { get; }
        string Image { get; }
        bool IsInstalled { get; }
        bool IsProxyChangeCancelled { get; }
        int Order { get; }

        ProxyInfo Proxy { get; set; }

        event Action ProxyChanged;
    }
}