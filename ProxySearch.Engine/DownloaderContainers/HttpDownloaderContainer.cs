using System.Net.Http;
using ProxySearch.Engine.Bandwidth;

namespace ProxySearch.Engine.DownloaderContainers
{
    public class HttpDownloaderContainer<HttpClientHandlerType> : IHttpDownloaderContainer where HttpClientHandlerType : HttpClientHandler, new()
    {
        public IBandwidthManager BandwidthManager
        {
            get { return new BandwidthManager<HttpClientHandlerType>(); }
        }

        public IHttpDownloader HttpDownloader
        {
            get { return new HttpDownloader<HttpClientHandlerType>(); }
        }
    }
}
