using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProxySearch.Engine.SearchEngines.UrlList
{
    public class UrlListSearchEngine : ISearchEngine
    {
        private List<Uri> urls;

        public string Status
        {
            get
            {
                return null;
            }
        }

        public UrlListSearchEngine(string urlList)
        {
            this.urls = urlList.Split('\n')
                               .Where(url => Uri.IsWellFormedUriString(url, UriKind.Absolute))
                               .Select(url => new Uri(url)).ToList();
        }

        public Task<Uri> GetNext()
        {
            if (!urls.Any())
            {
                return Task.FromResult<Uri>(null);
            }

            Uri result = urls[0];
            urls.RemoveAt(0);

            return Task.FromResult(result);
        }
    }
}
