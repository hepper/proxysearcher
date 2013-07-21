using ProxySearch.Engine;
using ProxySearch.Engine.Bandwidth;

namespace ProxySearch.Engine.DownloaderContainers
{
    public interface IHttpDownloaderContainer
    {
        IBandwidthManager BandwidthManager
        {
            get;
        }

        IHttpDownloader HttpDownloader
        {
            get;
        }
    }
}
