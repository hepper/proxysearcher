using ProxySearch.Engine;

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
