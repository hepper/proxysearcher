using ProxySearch.Engine.Proxies;

namespace ProxySearch.Console.Code.Interfaces
{
    public interface ISearchResult
    {
        void Started();
        void Completed();
        void Cancelled();

        void Clear();
        void Add(ProxyInfo proxy);
    }
}
