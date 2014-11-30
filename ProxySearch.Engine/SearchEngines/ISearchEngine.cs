using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProxySearch.Engine.SearchEngines
{
    public interface ISearchEngine
    {
        Task<Uri> GetNext(CancellationTokenSource cancellationTokenSource);
        string Status { get; }
    }
}
