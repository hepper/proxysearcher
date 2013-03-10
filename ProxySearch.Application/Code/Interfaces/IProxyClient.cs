using System.ComponentModel;
using ProxySearch.Engine;

namespace ProxySearch.Console.Code.Interfaces
{
    public interface IProxyClient : INotifyPropertyChanged
    {
        string Name { get; }
        string Image { get; }
        bool IsInstalled { get; }
        ProxyInfo Proxy { get; set; }
        int Order { get; }
    }
}
