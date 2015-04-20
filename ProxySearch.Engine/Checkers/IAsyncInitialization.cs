using System.Threading;
using ProxySearch.Engine.DownloaderContainers;
using ProxySearch.Engine.Error;
using ProxySearch.Engine.GeoIP;
using ProxySearch.Engine.Tasks;

namespace ProxySearch.Engine.Checkers
{
    public interface IAsyncInitialization
    {
        void InitializeAsync(CancellationTokenSource cancellationTokenSource, 
                             ITaskManager taskManager, 
                             IHttpDownloaderContainer httpDownloaderContainer,
                             IErrorFeedback errorFeedback,
                             IProxySearchFeedback proxySearchFeedback,
                             IGeoIP geoIP);
    }
}
