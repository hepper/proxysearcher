using System.Net.Http;
using System.Net.Http.Handlers;
using ProxySearch.Engine.Bandwidth;

namespace ProxySearch.Engine.DownloaderContainers
{
    public class HttpDownloaderContainer<HttpClientHandlerType, ProgressMessageHandlerType> : IHttpDownloaderContainer where HttpClientHandlerType : HttpClientHandler, new()
                                                                                                                       where ProgressMessageHandlerType : ProgressMessageHandler
    {
        public IBandwidthManager BandwidthManager
        {
            get { return new BandwidthManager<ProgressMessageHandlerType>(); }
        }

        public IHttpDownloader HttpDownloader
        {
            get { return new HttpDownloader<HttpClientHandlerType>(); }
        }
    }
}