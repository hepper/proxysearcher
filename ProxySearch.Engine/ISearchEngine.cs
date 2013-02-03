using System;
using System.Threading.Tasks;

namespace ProxySearch.Engine
{
    public interface ISearchEngine
    {
        Task<Uri> GetNext();
    }
}
