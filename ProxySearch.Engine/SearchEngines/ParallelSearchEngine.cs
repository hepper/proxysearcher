using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ProxySearch.Engine.SearchEngines
{
    public class ParallelSearchEngine : List<ISearchEngine>, ISearchEngine
    {
        public ParallelSearchEngine(params ISearchEngine[] searchEngines)
        {
            AddRange(searchEngines);
        }

        public Task<Uri> GetNext(CancellationTokenSource cancellationTokenSource)
        {
            throw new NotSupportedException();
        }

        public string Status
        {
            get
            {
                throw new NotSupportedException();
            }
        }
    }
}
