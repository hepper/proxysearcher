using System.ComponentModel;
using ProxySearch.Engine.Proxies;

namespace ProxySearch.Console.Code.Interfaces
{
    public interface IProxyClient : IProxyType, INotifyPropertyChanged
    {
        string Name { get; }
        string Image { get; }
        bool IsInstalled { get; }
        ProxyInfo Proxy { get; set; }
        int Order { get; }
    }
}
