using System;
using System.Threading.Tasks;

namespace ProxySearch.Engine.SearchEngines
{
    public interface ISearchEngine
    {
        Task<Uri> GetNext();
    }
}
