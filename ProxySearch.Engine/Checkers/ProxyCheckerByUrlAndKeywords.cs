using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ProxySearch.Engine.Properties;
using ProxySearch.Engine.Proxies;
using ProxySearch.Engine.ProxyDetailsProvider;
using ProxySearch.Engine.Tasks;

namespace ProxySearch.Engine.Checkers
{
    public class ProxyCheckerByUrlAndKeywords<ProxyDetailsProviderType> : ProxyCheckerBase<ProxyDetailsProviderType>
                                                                          where ProxyDetailsProviderType : IProxyDetailsProvider, new()
    {
        private string Url
        {
            get;
            set;
        }

        private string[] Keywords
        {
            get;
            set;
        }

        public ProxyCheckerByUrlAndKeywords(string url, string keywords, int maxTasksCount)
            : base(maxTasksCount)
        {
            Url = url;
            Keywords = keywords.Split(' ');
        }

        protected override async Task<bool> Alive(Proxy proxy, TaskItem task, Action begin, Action<int> firstTime, Action<int> end, CancellationTokenSource cancellationToken)
        {
            task.UpdateDetails(string.Format(Resources.ProxyDownloadingFormat, proxy, Url));

            string content = await HttpDownloaderContainer.HttpDownloader.GetContentOrNull(Url, proxy, cancellationToken, begin, firstTime, end);

            if (content == null)
            {
                return false;
            }

            return !Keywords.Any(item => !content.Contains(item));
        }
    }
}
