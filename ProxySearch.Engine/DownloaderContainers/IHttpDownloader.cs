using System;
using System.Threading;
using System.Threading.Tasks;
using ProxySearch.Engine.Proxies;

namespace ProxySearch.Engine.DownloaderContainers
{
    public interface IHttpDownloader
    {
        Task<string> GetContentOrNull(string url, Proxy proxy);
        Task<string> GetContentOrNull(string url, Proxy proxy, CancellationTokenSource cancellationToken);
        Task<string> GetContentOrNull(string url, Proxy proxy, CancellationTokenSource cancellationToken, Action begin, Action<int> firstTime, Action<int> end);
    }
}